using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using AppServices;
using DAL;
using DevTools;
using MauiApp1;
using Microsoft.Maui.Controls;

namespace KitBox.Views
{
    public partial class StockManagerPage : ContentPage
    {
        private Connection con;
        private DataTable data;
        private ObservableCollection<PieceData> pieces;
        private List<string> ToCommand;
        public ICommand SearchCommand { get; private set; }

        public StockManagerPage()
        {
            InitializeComponent();

            con = new Connection();

            ToCommand = StockServices.GenerateAutoCommandMessage(con);

            RefreshData();

            

            SearchCommand = new Command<string>(Search);
        }

        private void RefreshData()
        {
            Piece affichage = new Piece(con);
            List<string> colonnes = new List<string>
            {
                "reference",
                "code",
                "stock",
                "reserve",
                "await",
                "min_stock"
            };
            data = affichage.LoadAll(null, colonnes);
            pieces = new ObservableCollection<PieceData>();
            List<PieceData> redPieces = new List<PieceData>(); // Liste pour stocker les pièces en rouge
            foreach (DataRow row in data.Rows)
            {
                var piece = new PieceData
                {
                    Reference = row["reference"].ToString(),
                    Code = row["code"].ToString(),
                    Stock = row["stock"].ToString(),
                    Reserve = row["reserve"].ToString(),
                    Await = row["await"].ToString(),
                    MinPiece = row["min_stock"].ToString(),
                };

                if (ToCommand.Contains(piece.Code))
                {
                    piece.MinPieceBackgroundColor = Colors.Red;
                    redPieces.Add(piece); 
                }
                else
                {
                    pieces.Add(piece); 
                }
            }

            
            foreach (var redPiece in redPieces)
            {
                pieces.Insert(0, redPiece);
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
            Navigation.PushAsync(new OrderPartsPage(piece.Code));
        }

        private void input_clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InputArrivalPage());
        }

        private void OnEditClicked(object sender, EventArgs e)
        {
            var piece = (PieceData)((Button)sender).BindingContext;
            Navigation.PushAsync(new EditStockManager(piece.Code, piece.Stock));

        }


        private void OnSaveClicked(object sender, EventArgs e)
        {

            try
            {
                var button = (Button)sender;
                var piece = (PieceData)button.BindingContext;

                Entry minPieceEntry = ((View)sender).FindByName<Entry>("minPieceEntry");


                string minStock = minPieceEntry.Text;


                piece.MinPiece = minStock;


                Piece Parts = new Piece(con);
                DataTable result = Parts.Load(piece.Code);
                Dictionary<string, object> updatedInfo = new Dictionary<string, object>();
                updatedInfo["min_stock"] = minStock;



                Parts.Update(updatedInfo);
                Parts.Save();
                RefreshData();
            }
            catch (Exception exception)
            {
                Logger.WriteToFile(exception);
                throw;
            }

        }






    }
}
