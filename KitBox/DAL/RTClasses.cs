using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL;
public class RtCasier : Model
{

    public RtCasier(Connection connection) : base(connection)
    {
        tableName = "rt_casier";
        primaryKey = "id_relation";
    }
}
public class RtArmoire : Model
{

    public RtArmoire(Connection connection) : base(connection)
    {
        tableName = "rt_armoire";
        primaryKey = "id_relation";
    }
}
public class RtSupplier : Model
{

    public RtSupplier(Connection connection) : base(connection)
    {
        tableName = "rt_supplier";
        primaryKey = "id_relation";
    }
}
