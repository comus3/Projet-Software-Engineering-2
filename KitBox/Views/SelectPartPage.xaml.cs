using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using DAL;
using DevTools;
namespace KitBox.Views;

public partial class SelectPartPage : ContentPage
{

    private Connection con;
    private DataTable data; 
    private ObservableCollection<PieceData> pieces;
    
    public ICommand SearchCommand { get; private set; }
    public SelectPartPage()
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
            "selling_price",
               
               
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
                selling_price = row["selling_price"].ToString(),
                   
                  
            });
        }


        myListView.ItemsSource = pieces;

        // Initialize search command
        SearchCommand = new Command<string>(Search);
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
                p.selling_price.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    
                p.Stock.Contains(query, StringComparison.OrdinalIgnoreCase)) ;
            myListView.ItemsSource = filteredItems;
        }
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        Search(e.NewTextValue);
    }
    private async void Select_Clicked(object sender, EventArgs e)
    {

        try
        {
            
            var piece = (PieceData)((ViewCell)sender).BindingContext;
            SharedData.SelectedPartCode = piece.Code;
            await Navigation.PopAsync();
        }
        catch (Exception exception)
        {
           Logger.WriteToFile(exception);
            throw;
        }
        
    }

    
    
    public class PieceData
    {
        public string Reference { get; set; }
        public string Code { get; set; }
        public string Stock { get; set; }
        
        public string selling_price { get; set; }
        
       
    }
}