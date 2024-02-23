using Microsoft.Maui.Controls;
using System;

namespace KitBox.Views
{
    public partial class Customer_catalog : ContentPage
    {
        public Customer_catalog()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnCreateCasierClicked(object sender, EventArgs e)
        {
            double length;
            double width;

            // Vérifier si les entrées sont valides
            if (double.TryParse(lengthEntry.Text, out length) && double.TryParse(widthEntry.Text, out width))
            {
                // Les entrées sont valides, afficher une alerte avec les dimensions et naviguer vers la page Form
                DisplayAlert("Your Cabinet is", $"Length: {length}, Width: {width}", "OK");
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
