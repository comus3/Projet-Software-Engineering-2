

namespace KitBox.Views
{
    public partial class Panier : ContentPage
    {
        public Panier()
        {
            InitializeComponent();

            // Je test pour l'instant quelques trucs pour voir si ça marche
            lstArmoire.ItemsSource = new string[]
            {
                "Armoire 1",
                "Armoire 2",
                "Armoire 3",
                "Armoire 4",
                "Armoire 5"
            };
        }

        private void Acheter_Clicked(object sender, EventArgs e)
        {
            // Méthode pour gérer le clic sur le bouton "Acheter"
            // Récupérer l'armoire sélectionnée
            string phrase = (sender as Button).CommandParameter as string;

            // Ajoutez ici le code pour acheter la phrase
            DisplayAlert("Acheter", $"L'armoire'{phrase}' a été achetée avec succès.", "OK");
        }

        private void Supprimer_Clicked(object sender, EventArgs e)
        {
            // Méthode pour gérer le clic sur le bouton "Supprimer"
            // Récupérer l'armoire sélectionnée
            string phrase = (sender as Button).CommandParameter as string;

            // Ajoutez ici le code pour supprimer l'armoire
            DisplayAlert("Supprimer", $"L'armoire '{phrase}' a été supprimée avec succès.", "OK");
        }
    }
}
