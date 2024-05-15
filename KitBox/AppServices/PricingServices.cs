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
            price += Convert.ToDouble(row.ItemArray[12]);
        }
        obj.Attributes["price"] = price;
        obj.Save();
    }
    public static void PriceCommande(object pkCommande, Connection connection)
    {
        double price = 0.0;

        List<string> armoiresIDs = new List<string>();

        Armoire armoire = new Armoire(connection);
        Casier casier = new Casier(connection);
        Dictionary<string, object> conditionArmoire = new Dictionary<string, object>();
        Dictionary<string, object> conditionCasier = new Dictionary<string, object>();
        conditionArmoire["commande"] = pkCommande;
        List<string> colomnsArmoire = new List<string>();
        List<string> colomnsCasier = new List<string>();
        colomnsArmoire.Add("price");
        colomnsCasier.Add("price");
        colomnsArmoire.Add("id_armoire");

        DataTable armoireValues = armoire.LoadAll(conditionArmoire,colomnsArmoire);
        foreach (DataRow row in armoireValues.Rows)
        {
            double priceToAdd = Convert.ToDouble(row.ItemArray[0]);
            price = price + priceToAdd;
            armoiresIDs.Add(row.ItemArray[1].ToString());
        }
        foreach (string id in armoiresIDs)
        {
            conditionCasier["armoire"] = id;
            DataTable casierValues = casier.LoadAll(conditionCasier, colomnsCasier);
            foreach (DataRow row in casierValues.Rows)
            {
                price += Convert.ToDouble(row.ItemArray[0]);
            }
        }
        Commande commande = new Commande(connection);
        Dictionary<string, object> conditionCommande = new Dictionary<string, object>();
        conditionCommande["price"] = price;
        commande.Load(pkCommande);
        commande.Update(conditionCommande);
        commande.Save();

    }
}


//code ecrit par comus3
//nhesitez paas a me poser
//toute question sur comment il
//marche