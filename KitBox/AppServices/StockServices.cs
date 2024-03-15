using DAL;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace AppServices;

class StockServices
{
    private class RtSupplier : Model
    {

        public RtSupplier(Connection connection) : base(connection)
        {
            tableName = "rt_supplier";
            primaryKey = "id_relation";
        }
    }

    private class AwaitPiece : Model
    {
        public AwaitPiece(Connection connection) : base(connection)
        {
            tableName = "awaitpiece";
            primaryKey = "idawaitpiece";
        }
    }

    public static void ReserveStock(object pieceCode, int nombreReserve, Connection connection)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("stock", Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[8]) - nombreReserve);
        update.Add("reserve", Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[9]) + nombreReserve);
        piece.Update(update);
        piece.Load(pieceCode);
        piece.Save();
    }
    public static void ExecuteReserve(object pieceCode, int nombreComplete, Connection connection)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("reserve", Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[9]) - nombreComplete);
        piece.Update(update);
        piece.Load(pieceCode);
        piece.Save();
    }
    public static bool CheckStock(object pieceCode, int nombreNecessaire, Connection connection)
    {
        Piece piece = new Piece(connection);
        return (nombreNecessaire < Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[8]));
    }
    /// <summary>
    /// demander au prof comment il veut
    /// </summary>
    public static List<string> GenerateAutoCommandMessage(Connection connection)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> conditions = new Dictionary<string, object>();
        List<string> colomns = new List<string>();
        colomns.Add("code");
        colomns.Add("stock");
        colomns.Add("await");
        List<string> aCommander = new List<string>();
        DataTable pieces = piece.LoadAll(conditions);
        foreach (DataRow row in pieces.Rows)
        {
            if ((Convert.ToInt32(row.ItemArray[1]) + Convert.ToInt32(row.ItemArray[2])) < 3)
            {
                aCommander.Add(row.ItemArray[0].ToString());
            }
        }
        return aCommander;


    }
    public static bool MakeOrder(object pieceCode, int nombreNecessaire, Connection connection)
    {
        if (CheckStock(pieceCode, nombreNecessaire, connection))
        {
            ReserveStock(pieceCode, nombreNecessaire, connection);
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void ExecuteAutoCommand(string code, int quantite, int supplier, Connection connection)
    {
        int result;
        int prixPiece;

        RtSupplier rtSupplier = new RtSupplier(connection);
        HistoriqueCommande histCommande = new HistoriqueCommande(connection);

        Dictionary<string, object> data = new Dictionary<string, object>();
        Dictionary<string, object> condition = new Dictionary<string, object>();

        condition["id_piece"] = code;
        condition["id_supplier"] = supplier;
        List<string> colomns = new List<string>();
        colomns.Add("price_supplier");
        DataTable priceData = rtSupplier.LoadAll(condition, colomns);

        prixPiece = Convert.ToInt32(priceData.Rows[0].ItemArray[0]);
        int prixTotal = quantite * prixPiece;
        data["piece"] = code;
        data["id_supplier"] = supplier;
        data["quantite"] = quantite;
        data["date"] = DateTime.Today.ToString("ddMMyy");
        data["prix_piece"] = prixPiece;
        data["prix_total"] = prixTotal;

        histCommande.Update(data);
        histCommande.Insert();


    }
    public static void UpdateAwaitPiece(object code,object commandePk , int quantite, Connection connection)
    {
        AwaitPiece awaitPiece = new AwaitPiece(connection);
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["code"] = code;
        data["commande"] = commandePk;
        data["quantite"] = quantite;
        awaitPiece.Update(data);
        awaitPiece.Insert();
    }
}


//code ecrit par comus3
//nhesitez pas a me poser
//toute question sur comment il
//marche