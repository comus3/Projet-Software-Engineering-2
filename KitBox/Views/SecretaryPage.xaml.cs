using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using DAL;
using DevTools;
namespace KitBox.Views
{
    public partial class SecretaryPage : ContentPage
    {
        private Connection con;
        private DataTable data;

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
                "`Price _Supplier_1`", // Ajouter des backticks autour des noms de colonnes avec des espaces
                "`Price -_Supplier_2`", // Ajouter des backticks autour des noms de colonnes avec des espaces
                "stock"
            };
            data = affichage.LoadAll(null, colonnes);

            // Assigner les données à la source de la ListView
            listePiece.ItemsSource = data.AsEnumerable();
        }


        private async void Modifier_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Customer_catalog()); //Ceci ne constitue pas la version finale, c'est juste un test
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = e.NewTextValue;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    // Si la barre de recherche est vide, afficher toutes les données
                    listePiece.ItemsSource = data.AsEnumerable();
                }
                else
                {
                    // Filtrer les données en fonction du texte de recherche
                    if (double.TryParse(searchText, out double searchDouble))
                    {
                        // Si le texte de recherche peut être converti en double,
                        // filtrer les colonnes de type double
                        data.DefaultView.RowFilter = $"[Price _Supplier_1] = {searchDouble} OR [Price -_Supplier_2] = {searchDouble}";
                        listePiece.ItemsSource = data.DefaultView;
                    }
                    else
                    {
                        // Si le texte de recherche ne peut pas être converti en double,
                        // filtrer les colonnes de type string
                        data.DefaultView.RowFilter = $"reference LIKE '%{searchText}%' OR code LIKE '%{searchText}%' OR stock LIKE '%{searchText}%'";
                        listePiece.ItemsSource = data.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception ou afficher un message d'erreur
                Logger.WriteToFile($"An error occurred: {ex.Message}");
            }
        }



    }

}