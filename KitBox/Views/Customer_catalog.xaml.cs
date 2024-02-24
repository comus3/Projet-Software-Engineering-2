using Microsoft.Maui.Controls;
using DAL; // Importez le namespace où se trouve votre modèle
using System;
using System.Collections.Generic;

namespace KitBox.Views
{
    public partial class Customer_catalog : ContentPage
    {
        private Casier casier;

        public Customer_catalog()
        {
            InitializeComponent();
            BindingContext = this;

            Connection connection = new Connection();
            casier = new Casier(connection);
        }

        private void OnCreateCasierClicked(object sender, EventArgs e)
        {
            double length;
            double width;

            // Vérifier si les entrées sont valides
            if (double.TryParse(lengthEntry.Text, out length) && double.TryParse(widthEntry.Text, out width))
            {
                // Les entrés sont valide, préparez les données à insérer dans la base de données
                Dictionary<string, object> values = new Dictionary<string, object>();
                values["length"] = length;
                values["width"] = width;

                //  méthode Update pour mettre à jour les attributs de l'objet casier avec les nouvelles valeurs
                casier.Update(values);

                //  méthode Insert pour insérer le nouveau casier dans la base de données
                casier.Insert();

                // Affichez une alerte avec les dimensions et informez l'utilisateur que le casier a été créé
                DisplayAlert("Success", $"Your Cabinet is\nLength: {length}, Width: {width}\n\nThe cabinet has been created successfully!", "OK");

                // Naviguez vers la page Form pour continuer la configuration du casier
                Navigation.PushAsync(new Form());
            }
            else
            {
                // Les entrées ne sont pas valides, afficher un message d'erreur
                DisplayAlert("Error", "Please enter valid dimensions.", "OK");
            }
        }
    }
}
