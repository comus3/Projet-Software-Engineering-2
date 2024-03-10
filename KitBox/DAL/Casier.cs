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
        DataTable result = base.Insert();
        object casierPk = result.Rows[0][0];
        
        if (casierPk != null)
        {
            this.Load(Convert.ToInt32(casierPk));
            if (LinkingServices.CreateAllCasierLinks(this.connection, this))
            {
                return result;
            }
            else
            {
                Logger.WriteToFile("Error creating links for casier.");
                return null;
            }
        }
        else
        {
            Logger.WriteToFile("Error inserting casier.");
            return null;
        }
    }
}