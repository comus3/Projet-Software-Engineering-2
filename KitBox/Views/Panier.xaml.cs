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
            int numeroArmoire = 1;

            foreach (DataRow row in data.Rows)
            {
                string Number = $" Cabinet number: {numeroArmoire} "; 
                string Longueur = $" The length of the cabinet is: {row.ItemArray[1].ToString()}";
                string Profondeur = $" The depth of the cabinet is: {row.ItemArray[2].ToString()}";
                string Price = $"The total price is:{row.ItemArray[3].ToString()} ";
                Logger.WriteToFile(Longueur + "  " + Profondeur + "  " + Price);

                ArmoireAttributes armoireAttributes = new ArmoireAttributes(Number, Longueur, Profondeur, Price);
                lstArmoireItems.Add(armoireAttributes);
                numeroArmoire++;
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

            DisplayAlert("Delete", $"The cabinet with length {armoire.Longueur}, depth {armoire.Profondeur}, and price {armoire.Price} has been successfully deleted.", "OK");
        }
    }
}
