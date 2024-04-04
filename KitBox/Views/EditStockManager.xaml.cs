using System;
using Microsoft.Maui.Controls;
using DAL;
using DevTools;
using System.Data;
using KitBox.Views;
using System.Data.OleDb;

namespace MauiApp1
{
    public partial class EditStockManager : ContentPage
    {
        private Connection con;
        private object cle;

		//private object OLDstock;

        public EditStockManager( string Reference, string stock)
        {
            InitializeComponent();
            con = new Connection();
            Connection.TestConnection();
             cle = Reference;
			 OLDstock.Text = stock;

        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                

				string newStockQuantity = OLDstock.Text;

				Piece piece = new Piece(con);

                DataTable result = piece.Load(cle);

                Dictionary<string, object> valuesToUpdate = new Dictionary<string, object>();
                valuesToUpdate["stock"] = newStockQuantity;
                piece.Update(valuesToUpdate);
                piece.Save();

                Navigation.PushAsync(new StockManagerPage());;
            }
            catch (Exception ex)
            {
                Logger.WriteToFile($"Error occurred while updating stock quantity: {ex.Message}");
            }
        }
    }
}
