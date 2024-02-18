namespace KitBox;

public class Piece_armoire
{
    public string Type { get; set; }
    public double Longueur { get; set; }
    public double Largeur { get; set; }
    public double Prix { get; set; }

    
    public Piece_armoire(string type, double longueur, double largeur, double prix)
    {
        Type = type;
        Longueur = longueur;
        Largeur = largeur;
        Prix = prix;
    }
}

