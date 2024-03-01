using DAL;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using DAL;
using System;
using System.Data;

using System.Collections.Generic;
namespace KitBox.Views
{
    public partial class SecretaryPage : ContentPage
    {
        private Connection con;
        private DataTable data;

        public SecretaryPage()
        {
            InitializeComponent();
            BindingContext = this;
            Connection.TestConnection();
            con = new Connection();
            Piece affichage = new Piece(con);
            List<string> colonnes = new List<string>
            {
      
                "code",
                "stock",
                "Price_Supplier_1",
                "Price_Supplier_2"
            };
            data = affichage.LoadAll(null, colonnes);

            // Assigner les données à la source de la ListView
            listePiece.ItemsSource = data.AsEnumerable();
        }

        private async void Modifier_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Customer_catalog()); //Ceci ne constitue pas la version finale, c'est juste un test
        }
    }
}

