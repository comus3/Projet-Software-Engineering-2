using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using DAL;
using System;
using System.Data;
using DevTools;

namespace KitBox.Views
{
    public partial class Panier : ContentPage
    {
        private Connection con;
        internal class ArmoireAttributes
        {
            public string Numero { get; set; }
            public string Longueur { get; set; }
            public string Profondeur { get; set; }
            public string Price { get; set; }

            public ArmoireAttributes(string numero, string longueur, string profondeur, string price)
            {
                Numero = numero;
                Longueur = longueur;
                Profondeur = profondeur;
                Price = price;
            }
        }

        public Panier()
        {
            InitializeComponent();

            Connection.TestConnection();
            con = new Connection();

            numeroCommandeEntry.TextChanged += NumeroCommandeEntry_TextChanged;

            ChargerDonnees();
        }

        private void NumeroCommandeEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            if (int.TryParse(numeroCommandeEntry.Text, out int numeroCommande))
            {
                Armoire armoire = new Armoire(con);
                Dictionary<string, object> arm = new Dictionary<string, object>();
                arm["commande"] = numeroCommande;
                DataTable data = armoire.LoadAll(arm);

                List<ArmoireAttributes> lstArmoireItems = new List<ArmoireAttributes>();
                int numeroArmoire = 1;

                foreach (DataRow row in data.Rows)
                {
                    string Numero = $"Armoire numéro: {numeroArmoire}";
                    string Longueur = $"Longueur: {row[1]}";
                    string Profondeur = $"Profondeur: {row[2]}";
                    string Price = $"Prix: {row[3]} €";

                    Logger.WriteToFile($"{Numero} {Longueur} {Profondeur} {Price}");

                    ArmoireAttributes armoireAttributes = new ArmoireAttributes(Numero, Longueur, Profondeur, Price);
                    Logger.WriteToFile(armoireAttributes);
                    lstArmoireItems.Add(armoireAttributes);

                    numeroArmoire++;
                }

                lstArmoire.ItemsSource = lstArmoireItems;
            }
            else
            {
                lstArmoire.ItemsSource = null;
            }
        }

        private void Finish_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Acheter", "L'armoire a été achetée avec succès.", "OK");
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var armoire = (sender as Button)?.CommandParameter as ArmoireAttributes;

            if (armoire != null)
            {
                Armoire armoireToDelete = new Armoire(con);
                armoireToDelete.Delete();                
                ChargerDonnees();
                await DisplayAlert("Supprimer", $"L'armoire {armoire.Numero}, longueur {armoire.Longueur}, profondeur {armoire.Profondeur} et prix {armoire.Price} a été supprimée avec succès.", "OK");
            }
        }
    }
}
