using DAL;
using System;
using System.Collections.Generic;
using System.Data;


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
                this.Longueur = longueur;
                this.Profondeur = profondeur;
                this.Price = price;
            }
        }
        public Panier()
        {
            InitializeComponent();

            // Initialisez votre modèle avec une connexion à la base de données, par exemple
            //Connection connection = new Connection("your_connection_string_here");
            //model = new Model(connection);
            Connection.TestConnection();
            con = new Connection();

            // Je teste pour l'instant quelques trucs pour voir si ça marche
            /*lstArmoire.ItemsSource = new string[]
            {
                "Armoire 1",
                "Armoire 2",
                "Armoire 3",
                "Armoire 4",
                "Armoire 5"
            };*/

            // Chargez les armoires depuis la base de données
            Armoire armoire = new Armoire(con);
            Dictionary<string, object> arm = new Dictionary<string, object>();
            arm["commande"] = 1;
            DataTable data = armoire.LoadAll(arm);
            List<ArmoireAttributes> lstArmoireItems = new List<ArmoireAttributes>();
            foreach (DataRow row in data.Rows)
            {
                ArmoireAttributes armoireAttributes = new ArmoireAttributes(row["longueur"].ToString(), row["profondeur"].ToString(), row["price"].ToString());
                lstArmoireItems.Add(armoireAttributes);
            }
            // Set the ItemsSource of the ListView to your list of ArmoireAttributes
            this.lstArmoire.ItemsSource = lstArmoireItems;

            //armoire.LoadAll();

        }

        private void Acheter_Clicked(object sender, EventArgs e)
        {
            // Méthode pour gérer le clic sur le bouton "Acheter"
            // Récupérer l'armoire sélectionnée
            DisplayAlert("Acheter", "L'armoire a été achetée avec succès.", "OK");
        }

        private void Supprimer_Clicked(object sender, EventArgs e)
        {
            // Méthode pour gérer le clic sur le bouton "Supprimer"
            // Récupérer l'armoire sélectionnée
            var armoire = (sender as Button).CommandParameter as ArmoireAttributes;

            //Ajoutez ici le code pour supprimer l'armoire
            //Appelez la méthode Delete de votre modèle en utilisant l'ID de l'armoire (par exemple)
            //model.Delete(armoire);

            DisplayAlert("Supprimer", $"L'armoire avec longueur {armoire.Longueur}, profondeur {armoire.Profondeur} et prix {armoire.Price} a été supprimée avec succès.", "OK");
        }
    }
}
