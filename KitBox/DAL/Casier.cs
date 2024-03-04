namespace DAL ;
using System.Data;
using AppServices;
using DevTools;

public class Casier : Model
{

    public Casier(Connection connection) : base(connection)
    {
        tableName = "casier";
        primaryKey = "id_casier";
    }
    public override DataTable Insert()
    {
        if (LinkingServices.CreateAllCasierLinks(this.connection, this))
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