namespace DAL ; 

public class Armoire : Model
{

    public Armoire(Connection connection) : base(connection)
    {
        tableName = "armoire";
        primaryKey = "id_armoire";
    }
}