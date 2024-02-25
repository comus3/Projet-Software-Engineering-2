using Microsoft.Maui.Controls;

using DAL; // Importez le namespace où se trouve votre modèle
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;

namespace KitBox.Views
{
    public partial class Customer_catalog : ContentPage
    {
        private Casier casier;
        private Connection con;

        private Commande com;
        private object comPk;

        private List<Object> listArmoirePk ;


        public Customer_catalog()
        {
            InitializeComponent();
            BindingContext = this;

            // Testez la connexion
            Connection.TestConnection();
            // Initialisez la connexion
            con = new Connection();

            // Initialisez une commande
            Commande com = new Commande(con);
            Dictionary<string, object> infoClient = new Dictionary<string, object>();
            infoClient["date"] = 240224;
            com.Update(infoClient);
            DataTable data = com.Insert();
            Displayer.DisplayData(data);
            this.com = com;
            this.comPk = data.Rows[0].ItemArray[0];
            this.listArmoirePk = new List<Object>();
            //Displayer.DisplayData(com.getLastPk());
        }

        private void OnCreateCasierClicked(object sender, EventArgs e)
        {
            double length;
            double width;

            // Vérifiez si les entrées sont valides
            if (double.TryParse(lengthEntry.Text, out length) && double.TryParse(widthEntry.Text, out width))
            {
                // Les entrées sont valides, préparez les données à insérer dans la base de données
                Dictionary<string, object> values = new Dictionary<string, object>();
                values["longueur"] = length;
                values["largeur"] = width;
                values["commande"] = comPk;

                // Initialisez un nouveau casier
                Armoire armoire = new Armoire(con);

                // Mettez à jour les attributs de l'objet casier avec les nouvelles valeurs
                armoire.Update(values);

                // Insérez le nouveau casier dans la base de données
                this.listArmoirePk.Add(armoire.Insert().Rows[0].ItemArray[0]);
                Logger.WriteToFile(this.listArmoirePk);

                // Affichez une alerte avec les dimensions et informez l'utilisateur que le casier a été créé
                DisplayAlert("Success", $"Your Cabinet is\nLength: {length}, Width: {width}\n\nThe cabinet has been created successfully!", "OK");

                // Naviguez vers la page Form pour continuer la configuration du casier
                object lastArmoireCree = listArmoirePk[listArmoirePk.Count-1];
                Navigation.PushAsync(new Form(lastArmoireCree));
            }
            else
            {
                // Les entrées ne sont pas valides, affichez un message d'erreur
                DisplayAlert("Error", "Please enter valid dimensions.", "OK");
            }
        }
    }
}
