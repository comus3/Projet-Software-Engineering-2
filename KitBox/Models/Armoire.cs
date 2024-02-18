using KitBox.Models;

namespace KitBox
{
    public class Armoire
    {
        private List<Casier> Casiers { get; set; }
        private int Longueur { get; set; }
        private int Largeur { get; set; }
        private int Prix { get; set; }
        private List<Armoire> PiecesArmoire { get; set; }

        public Armoire(List<Casier> casiers, int longueur, int largeur, int prix, List<Armoire> piecesArmoire)
        {
            Casiers = casiers;
            Longueur = longueur;
            Largeur = largeur;
            Prix = prix;
            PiecesArmoire = piecesArmoire;
        }

        // Méthode pour calculer le prix total de l'armoire
        public int CalculateTotalPrice()
        {
            int totalPrice = Prix; // Prix de base de l'armoire

            // Ajoute le prix de chaque casier à celui de l'armoire
            foreach (var casier in Casiers)
            {
                int casierPrice;
                if (int.TryParse(casier.Prix, out casierPrice))
                {
                    totalPrice += casierPrice;
                }
            }

            return totalPrice;
        }
    }
}
