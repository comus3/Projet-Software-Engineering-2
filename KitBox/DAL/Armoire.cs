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
        DataTable result = base.Insert();
        object armoirePk = result.Rows[0][0];

        if (armoirePk != null)
        {
            this.Load(Convert.ToInt32(armoirePk));
            if (LinkingServices.CreateAllArmoireLinks(this.connection, this))
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