using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DevTools;
using KitBox.AppServices;
namespace AppServices;

class PricingServices
{
    public static void PriceObject(Model obj ,Connection connection)
    {
        DataTable pieces = new DataTable();
        double price = 0;

        if (obj.GetType() == typeof(Armoire))
        {
            pieces = FetchingServices.FetchArmoirePieces(connection, obj.Attributes[obj.PrimaryKey]);
        }
        else if (obj.GetType() == typeof(Casier))
        {
            pieces = FetchingServices.FetchCasierPieces(connection, obj.Attributes[obj.PrimaryKey]);
        }
        else
        {
            Logger.WriteToFile($"Error: Object {obj} not appropriate for pricing");
        }
        foreach (DataRow row in pieces.Rows)
        {
            price += Convert.ToDouble(row.ItemArray[10]);
        }
        obj.Attributes["price"] = price;
        obj.Save();
    }
    public static void PriceCommande()
    {

    }
}


//code ecrit par comus3
//nhesitez paas a me poser
//toute question sur comment il
//marche