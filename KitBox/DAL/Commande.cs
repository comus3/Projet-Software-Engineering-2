namespace DAL ; 

public class Commande : Model
{

    public Commande(Connection connection) : base(connection)
    {
        tableName = "commande";
        primaryKey = "index";
    }
}