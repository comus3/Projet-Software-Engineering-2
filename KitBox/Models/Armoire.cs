using KitBox.Models;

namespace KitBox
{
    public class Armoire
    {
        private List<Casier> Casiers { get; set; }
        private double Longueur { get; set; }
        private double Largeur { get; set; }
        private double Prix { get; set; }
        private List<Armoire> PiecesArmoire { get; set; }

        public Armoire(List<Casier> casiers, double longueur, double largeur, double prix, List<Armoire> piecesArmoire)
        {
            Casiers = casiers;
            Longueur = longueur;
            Largeur = largeur;
            Prix = prix;
            PiecesArmoire = piecesArmoire;
        }

        // Méthode pour calculer le prix total de l'armoire
        public double CalculateTotalPrice()
        {
            double totalPrice = Prix; // Prix de base de l'armoire

            // Ajoute le prix de chaque casier à celui de l'armoire
            foreach (var casier in Casiers)
            {
                double casierPrice;
                if (double.TryParse(casier.Prix, out casierPrice))
                {
                    totalPrice += casierPrice;
                }
            }

            return totalPrice;
        }
    }
}
