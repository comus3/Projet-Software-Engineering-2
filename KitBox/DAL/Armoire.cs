namespace DAL ; 
using System.Data;
using DevTools;
using AppServices;

public class Armoire : Model
{

    public Armoire(Connection connection) : base(connection)
    {
        tableName = "armoire";
        primaryKey = "id_armoire";
    }
}