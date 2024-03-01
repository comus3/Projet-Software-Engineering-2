namespace DAL ; 

public class Casier : Model
{

    public Casier(Connection connection) : base(connection)
    {
        tableName = "casier";
        primaryKey = "id_casier";
    }
}