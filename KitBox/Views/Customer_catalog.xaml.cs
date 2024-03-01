using Microsoft.Maui.Controls;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;
using AppServices;
using KitBox.AppServices;

namespace KitBox.Views
{
    public partial class Customer_catalog : ContentPage
    {
        private Casier casier;
        private Connection con;
        private Commande com;
        private object comPk;
        private List<Object> listArmoirePk;

        public Customer_catalog()
        {
            InitializeComponent();
            BindingContext = this;

            // Testez la connexion
            Connection.TestConnection();
            // Initialisez la connexion
            con = new Connection();

            // Initialisez une commande
            com = new Commande(con);
            Dictionary<string, object> infoClient = new Dictionary<string, object>();
            infoClient["date"] = 240224;
            com.Update(infoClient);
            DataTable data = com.Insert();
            Displayer.DisplayData(data);
            FetchingServices.CurrentCommand = data.Rows[0].ItemArray[0].ToString();
            listArmoirePk = new List<Object>();
        }

        private void OnCreateCasierClicked(object sender, EventArgs e)
        {
            // Vérifiez si les dimensions sont sélectionnées
            if (lengthPicker.SelectedItem == null || widthPicker.SelectedItem == null)
            {
                // Affichez un message d'erreur si une ou les deux dimensions ne sont pas choisies
                DisplayAlert("Error", "Make sure to choose both dimensions.", "OK");
                return;
            }

            double width, length;
            // Convertissez les valeurs sélectionnées en chaînes de caractères, puis analysez-les en tant que double
            if (!double.TryParse(lengthPicker.SelectedItem.ToString(), out length) || !double.TryParse(widthPicker.SelectedItem.ToString(), out width))
            {
                // Affichez un message d'erreur si les valeurs sélectionnées ne sont pas valides
                DisplayAlert("Error", "Please enter valid dimensions.", "OK");
                return;
            }

            // Vérifiez que la largeur est inférieure ou égale à la longueur
            if (width <= length)
            {
                // Les dimensions sont valides, continuez le traitement
                Armoire armoire = new Armoire(con);
                Dictionary<string, object> values = new Dictionary<string, object>();
                values["longueur"] = length;
                values["largeur"] = width;
                values["commande"] = FetchingServices.CurrentCommand;
                armoire.Update(values);

                listArmoirePk.Add(armoire.Insert().Rows[0].ItemArray[0]);
                Logger.WriteToFile(listArmoirePk);

                // Affichez une alerte avec les dimensions et informez l'utilisateur que le casier a été créé
                DisplayAlert("Success", $"Your Cabinet is\nLength: {length}, Width: {width}\n\nThe cabinet has been created successfully!", "OK");

                object lastArmoireCree = listArmoirePk[listArmoirePk.Count - 1];
                Navigation.PushAsync(new Form(lastArmoireCree));
            }
            else
            {
                // Affichez un message d'erreur si la largeur est supérieure à la longueur
                DisplayAlert("Error", "Width cannot be greater than length.", "OK");
            }
        }

    }
}
