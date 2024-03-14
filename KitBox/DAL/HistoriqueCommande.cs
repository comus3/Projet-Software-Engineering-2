namespace DAL;

public class HistoriqueCommande : Model
{

    public HistoriqueCommande(Connection connection) : base(connection)
    {
        tableName = "historiquecommande";
        primaryKey = "id_commande";
    }
}