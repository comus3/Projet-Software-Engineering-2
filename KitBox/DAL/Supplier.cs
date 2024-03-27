namespace DAL;
using System.Data;
using DevTools;
using AppServices;

public class Supplier : Model
{

    public Supplier(Connection connection) : base(connection)
    {
        tableName = "supplier";
        primaryKey = "id_supplier";
    }
}