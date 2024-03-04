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
    public override DataTable Insert()
    {
        if (LinkingServices.CreateAllArmoireLinks(this.connection, this))
        {
            return base.Insert();
        }
        else
        {
            Logger.WriteToFile("Error creating links for armoire.");
            return null;
        }
    }
}