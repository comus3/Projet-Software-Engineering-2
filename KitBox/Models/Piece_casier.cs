namespace KitBox;



public class Piece_casier
    {
    public string Type { get; set; }
    public double Longueur { get; set; }
    public double Largeur { get; set; }
    public double Prix { get; set; }

    public Piece_casier(string type, double longueur, double largeur, double prix)
    {
        Type = type;
        Longueur = longueur;
        Largeur = largeur;
        Prix = prix;
    }
}