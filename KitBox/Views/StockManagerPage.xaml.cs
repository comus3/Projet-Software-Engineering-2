using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;
using Microsoft.Maui.Controls;

namespace KitBox.Views
{
    public partial class StockManagerPage : ContentPage
    {
        private Connection con;
        private DataTable data;
        private ObservableCollection<PieceData> pieces;
        
        public ICommand SearchCommand { get; private set; }

        public StockManagerPage()
        {
            InitializeComponent();
            RefreshData();
            SearchCommand = new Command<string>(Search);
        }

        private void RefreshData()
        {
            Connection.TestConnection();
            con = new Connection();
            Piece affichage = new Piece(con);
            List<string> colonnes = new List<string>
            {
                "reference",
                "code",
                "stock",
                "reserve",
                "await"
            };
            data = affichage.LoadAll(null, colonnes);
            pieces = new ObservableCollection<PieceData>();
            foreach (DataRow row in data.Rows)
            {
                pieces.Add(new PieceData
                {
                    Reference = row["reference"].ToString(),
                    Code = row["code"].ToString(),
                    Stock = row["stock"].ToString(),
                    Reserve = row["reserve"].ToString(),
                    Await = row["await"].ToString()
                });
            }
            myListView.ItemsSource = pieces;
        }

        private void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                myListView.ItemsSource = pieces;
            }
            else
            {
                var filteredItems = pieces.Where(p =>
                    p.Reference.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Code.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Await.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Reserve.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Stock.Contains(query, StringComparison.OrdinalIgnoreCase));
                myListView.ItemsSource = filteredItems;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search(e.NewTextValue);
        }

        private void OnorderClicked(object sender, EventArgs e)
        {
            var piece = (PieceData)((Button)sender).BindingContext;
            Navigation.PushAsync(new OrderPartsPage(piece.Code, piece.Await));
        }

        private void OnInputClicked(object sender, EventArgs e)
        {
            var piece = (PieceData)((Button)sender).BindingContext;
            Navigation.PushAsync(new InputArrivalPage(piece.Code));
        }
        
    }
}
