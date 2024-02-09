namespace KitBox.Models;
public class Casier
{
    // Les attributs de la classe Casier dépendront du diagramme de classe et de la base de données
    public string Code { get; set; }
    public string Hauteur { get; set; }
    public string Profondeur { get; set; }
    public string Largeur { get; set; }
    public string Couleur { get; set; }
    public string Prix { get; set; }
    public string Stock { get; set; }
    public string Quantite { get; set; }
    public string Image { get; set;}

    public Casier(string code, string hauteur, string profondeur, string largeur, string couleur, string prix, string stock, string quantite, string image)
    {
        Code = code;
        Hauteur = hauteur;
        Profondeur = profondeur;
        Largeur = largeur;
        Couleur = couleur;
        Prix = prix;
        Stock = stock;
        Quantite = quantite;
        Image = image;
    }
}
