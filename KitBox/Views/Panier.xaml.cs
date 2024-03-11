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

        internal class ArmoireAttributes
        {
            public string Longueur { get; set; }
            public string Profondeur { get; set; }
            public string Price { get; set; }

            public ArmoireAttributes(string longueur, string profondeur, string price)
            {
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
            DataTable data = armoire.LoadAll(arm);
            List<ArmoireAttributes> lstArmoireItems = new List<ArmoireAttributes>();
            foreach (DataRow row in data.Rows)
            {


                string Longueur = $" La longueur de l'armoire est de: {row.ItemArray[1].ToString()}";
                string Profondeur = $" La profondeur de l'armoire est de: {row.ItemArray[2].ToString()}";
                string Price = $"Le prix total est de:{row.ItemArray[3].ToString()} ";
                Logger.WriteToFile(Longueur + "  " + Profondeur + "  " + Price);

                ArmoireAttributes armoireAttributes = new ArmoireAttributes(Longueur, Profondeur, Price);
                lstArmoireItems.Add(armoireAttributes);
            }
            lstArmoire.ItemsSource = lstArmoireItems;
        }

        private void Acheter_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Acheter", "L'armoire a été achetée avec succès.", "OK");
            Navigation.PushAsync(new CustomerRegisterForm());
        }

        private void Supprimer_Clicked(object sender, EventArgs e)
        {
            var armoire = (sender as Button).CommandParameter as ArmoireAttributes;

            DisplayAlert("Supprimer", $"L'armoire avec longueur {armoire.Longueur}, profondeur {armoire.Profondeur} et prix {armoire.Price} a été supprimée avec succès.", "OK");
        }
    }
}
