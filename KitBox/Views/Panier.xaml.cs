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
            public object ArmoirePk { get; set; }

            public ArmoireAttributes(string longueur, string profondeur, string price, string number, object armoirePk)
            {
                Longueur = longueur;
                Profondeur = profondeur;
                Price = price;
                Number = number;
                ArmoirePk = armoirePk;
            }
        }

        public Panier()
        {
            InitializeComponent();
            Connection.TestConnection();
            con = new Connection();
            data = new DataTable(); // Initialisation de la DataTable
            lblBasket.Text = $"Basket for order number {FetchingServices.CurrentCommand}";
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
                //string Colour = $"The color of the cabinet is: {row.ItemArray[5].ToString()}";

                Logger.WriteToFile(Longueur + "  " + Profondeur + "  " + Price);

                ArmoireAttributes armoireAttributes = new ArmoireAttributes( Longueur, Profondeur, Price, Number,   row.ItemArray[0]);
                lstArmoireItems.Add(armoireAttributes);
                numeroArmoire++;
            }
            lstArmoire.ItemsSource = lstArmoireItems;
        }

        private void Acheter_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Buy", "Cabinet successfully baught.", "OK");
            Navigation.PushAsync(new CustomerRegisterForm());
        }

        private async void Supprimer_Clicked(object sender, EventArgs e)
        {
            var armoire = (sender as Button).CommandParameter as ArmoireAttributes;


            bool result = await DisplayAlert("Confirmation", $"Are you sure you want to delete this cabinet ?", "Oui", "Annuler");

            if (result)
            {
                SupprimerArmoire(armoire);
            }
        }

        private void SupprimerArmoire(ArmoireAttributes armoire)
        {


            Armoire armoireDAL = new Armoire(con);
            armoireDAL.Load(armoire.ArmoirePk);
            armoireDAL.Delete();
            ChargerDonnees();
        }

        private void Details(object sender, EventArgs e)
        {
            var armoire = (sender as Button).CommandParameter as ArmoireAttributes;

           
            DisplayAlert($"Details of the cabinet number: {armoire.Number} ", $"{armoire.Longueur}\n {armoire.Profondeur}\n  {armoire.Price} euros", "OK");


        }

    }
}
