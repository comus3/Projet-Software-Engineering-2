using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;
namespace KitBox.Views
{
    public partial class SecretaryPage : ContentPage
    {
        private Connection con;
        private DataTable data;
        private ObservableCollection<PieceData> pieces;

        public ICommand SearchCommand { get; private set; }

        public SecretaryPage()
        {
            InitializeComponent();
            Connection.TestConnection();
            con = new Connection();
            Piece affichage = new Piece(con);
            List<string> colonnes = new List<string>
            {
                "reference",
                "code",
                "stock",
                "Price_Supplier_2",
                "Price_Supplier_1"
                // Add other column names here
            };
            data = affichage.LoadAll(null, colonnes);

            // Convert DataTable to a format suitable for display
            pieces = new ObservableCollection<PieceData>(); // Define a class to hold your data (see below)
            foreach (DataRow row in data.Rows)
            {
                pieces.Add(new PieceData
                {
                    Reference = row["reference"].ToString(),
                    Code = row["code"].ToString(),
                    Stock = row["stock"].ToString(),
                    Price_Supplier_2 = row["Price_Supplier_2"].ToString(),
                    Price_Supplier_1 = row["Price_Supplier_1"].ToString()
                    // Assign other columns here
                });
            }

            // Bind the data to ListView
            myListView.ItemsSource = pieces;

            // Initialize search command
            SearchCommand = new Command<string>(Search);
        }

        private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // If the search query is null or empty, show all items
                myListView.ItemsSource = pieces;
            }
            else
            {
                // Filter the items based on the search query
                var filteredItems = pieces.Where(p =>
                    p.Reference.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Code.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Price_Supplier_2.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Price_Supplier_1.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Stock.Contains(query, StringComparison.OrdinalIgnoreCase)) ;
                myListView.ItemsSource = filteredItems;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(e.NewTextValue);
        }

        private void ModifyButton_Clicked(object sender, EventArgs e)
        {
            // Handle button click event here
            // Access the clicked item using the CommandParameter
            var button = (Button)sender;
            var piece = (PieceData)button.CommandParameter;
            
            Navigation.PushAsync(new Modify_Price_Page(piece.Price_Supplier_1, piece.Price_Supplier_2, piece.Code));
    
            // Now you can access the selected piece and perform the modification logic
        }

    }

    public class PieceData
    {
        public string Reference { get; set; }
        public string Code { get; set; }
        public string Stock { get; set; }
        
        public string Price_Supplier_2 { get; set; }
        
        public string Price_Supplier_1 { get; set; }
        // Add properties for other columns here
    }
}
