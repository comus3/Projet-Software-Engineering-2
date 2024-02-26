

namespace KitBox.Views
{
    public partial class Panier : ContentPage
    {
        public Panier()
        {
            InitializeComponent();
        }

        private void Acheter_Click(object sender, System.EventArgs e)
        {
            // Méthode pour confirmer l'achat via le panier
            DisplayAlert("Message", "Votre commande a été effectuée", "OK");
        }

        private void Supprimer_Click(object sender, System.EventArgs e)
        {
            // Méthode pour supprimer totalement une commande
            DisplayAlert("Message", "Suppression de la commande effectuée", "OK");
        }

        private void Modifier_Click(object sender, System.EventArgs e)
        {
            // Méthode pour modifier une commande
            DisplayAlert("Message", "Modification effectuée", "OK");
        }
    }
}
