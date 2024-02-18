namespace KitBox;

public class Piece_armoire
{
    private string Type { get; set; }
    private double Longueur { get; set; }
    private double Largeur { get; set; }
    private double Prix { get; set; }


    public Piece_armoire(string type, double longueur, double largeur, double prix)
    {
        Type = type;
        Longueur = longueur;
        Largeur = largeur;
        Prix = prix;
    }
}

