using System;
using System.Collections.Generic;
using System.Data;
using KitBox.AppServices;
using DevTools;
using DAL;

namespace KitBox.Views
{
    public partial class Panier : ContentPage
    {
        private Connection con;
        private DataTable data; // Déclaration de la DataTable

        internal class ArmoireAttributes
        {
            public string Longueur { get; set; }
            public string Profondeur { get; set; }
            public string Price { get; set; }
            public string Number { get; set; }

            public ArmoireAttributes(string longueur, string profondeur, string price, string number)
            {
                Longueur = longueur;
                Profondeur = profondeur;
                Price = price;
                Number = number;
            }
        }

        public Panier()
        {
            InitializeComponent();
            Connection.TestConnection();
            con = new Connection();
            data = new DataTable(); // Initialisation de la DataTable
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ChargerDonnees();
        }

        private void ChargerDonnees()
        {
            Armoire armoire = new Armoire(con);
            Dictionary<string, object> arm = new Dictionary<string, object>();
            arm["commande"] = FetchingServices.CurrentCommand;
            data = armoire.LoadAll(arm);
            List<ArmoireAttributes> lstArmoireItems = new List<ArmoireAttributes>();
            int numeroArmoire = 1;

            foreach (DataRow row in data.Rows)
            {
                string Number = $"Cabinet number: {numeroArmoire}";
                string Longueur = $"The length of the cabinet is: {row.ItemArray[1].ToString()}";
                string Profondeur = $"The depth of the cabinet is: {row.ItemArray[2].ToString()}";
                string Price = $"The total price is: {row.ItemArray[3].ToString()}";
                Logger.WriteToFile(Longueur + "  " + Profondeur + "  " + Price);

                ArmoireAttributes armoireAttributes = new ArmoireAttributes(Number, Longueur, Profondeur, Price);
                lstArmoireItems.Add(armoireAttributes);
                numeroArmoire++;
            }
            lstArmoire.ItemsSource = lstArmoireItems;
        }

        private async void Acheter_Clicked(object sender, EventArgs e) // Ajout du mot-clé async pour utiliser await avec DisplayAlert
        {
            await DisplayAlert("Acheter", "L'armoire a été achetée avec succès.", "OK");
            await Navigation.PushAsync(new CustomerRegisterForm());
        }

        private async void Supprimer_Clicked(object sender, EventArgs e) // Ajout du mot-clé async pour utiliser await avec DisplayAlert
        {
            var armoire = (sender as Button).CommandParameter as ArmoireAttributes;

            // Affichage du message de confirmation
            bool result = await DisplayAlert("Confirmation", $"Voulez-vous vraiment supprimer l'armoire {armoire.Number} ?", "Oui", "Annuler");

            if (result)
            {
                SupprimerArmoire(armoire);
            }
        }

        private void SupprimerArmoire(ArmoireAttributes armoire)
        {
            // Trouver la ligne correspondante dans la DataTable
            DataRow[] rowsToDelete = data.Select($"Number = '{armoire.Number}'");
            foreach (DataRow row in rowsToDelete)
            {
                row.Delete(); // Supprimer la ligne de la DataTable
            }

            // Appliquer les modifications à la base de données
            Armoire armoireDAL = new Armoire(con);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["Number"] = armoire.Number;
            armoireDAL.Delete(parameters);

            // Mettre à jour l'affichage
            ChargerDonnees();
        }
    }
}
