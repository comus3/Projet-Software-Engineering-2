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
    }
}
