using DAL;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
namespace AppServices;

class StockServices
{
    public static void ReserveStock(object pieceCode, int nombreReserve)
    {

    }
    public static void ExecuteReserve(object pieceCode, int nombreComplete)
    {

    }
    public static void CheckStock(object pieceCode, int nombreNecessaire)
    {

    }
    /// <summary>
    /// demander au prof comment il veut
    /// </summary>
    public static void AutoCommand()
    {

    }
    public static void ExecuteAutoCommand(string code,int quantite, int supplier )
    {
        Connection connection = new Connection();
        HistoriqueCommande histcom = new HistoriqueCommande(connection);
        Dictionary<string,object> data = new Dictionary<string,object>();  
        int result = 0;
        Dictionary<string, object> condition = new Dictionary<string, object>();
        condition["id_piece"] = code;
        condition["id_supplier"] = supplier;
        List<string> colomns = new List<string>();
        colomns.Add("price_supplier");
        DataTable histoData = histcom.LoadAll(condition, colomns);
        result = quantite * Convert.ToInt32(histoData);
        data["piece"] = code;
        data["id_supplier"] = supplier;
        data["quantite"] = quantite;
        data["date"] = DateTime.Today.ToString("ddMMyy");
        data["prix_piece"] = histoData;
        data["prix_total"] = result;
    }
}


//code ecrit par comus3
//nhesitez pas a me poser
//toute question sur comment il
//marche