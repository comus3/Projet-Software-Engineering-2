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
using DevTools;
using Mysqlx.Cursor;
using KitBox.AppServices;
using MySqlX.XDevAPI.Relational;
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

    public static void ReserveStock(object pieceCode, int nombreReserve, Connection connection, bool removeStock = true)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> update = new Dictionary<string, object>();
        if(removeStock){update.Add("stock", Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[8]) - nombreReserve);}
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
    public static List<string> CheckAllStockLow(Connection connection)
    {
        List<string> list = new List<string>();

        Piece piece = new Piece(connection);
        Dictionary<string, object> conditions = new Dictionary<string, object>();
        List<string> colomns = new List<string>();
        colomns.Add("code");
        DataTable pieces = piece.LoadAll(conditions, colomns);
        foreach (DataRow row in pieces.Rows)
        {
            if (!IsStockLow(row.ItemArray[0], connection))
            {
                list.Add($"Il faut commander {row.ItemArray[0]}");
            }
        }
        return list;
    }

    //<summary>
    //détecte si la différence entre (le stock + les pièces commandées)
    //et les pièces à commander (dans awaitpiece)
    //est supérieure à 5 pour une pièce donnée.
    //true si la différence est supérieure à 5
    //<param name="pieceCode"></param>
    //<param name="connection"></param>
    //</summary>
    private static bool IsStockLow(object pieceCode, Connection connection)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> code = new Dictionary<string, object>() { { "code", pieceCode } };
        DataTable pieceData = piece.LoadAll(code);
        AwaitPiece awaitPiece = new AwaitPiece(connection);
        DataTable awaitingData = awaitPiece.LoadAll(code);
        int pieces_a_commander = 0;
        foreach (DataRow row in awaitingData.Rows)
        { 
            pieces_a_commander += Convert.ToInt32(row.ItemArray[2]);
        }
        int stock = Convert.ToInt32(pieceData.Rows[0].ItemArray[8]);
        int pieces_commandées = Convert.ToInt32(pieceData.Rows[0].ItemArray[10]);
        return ((stock+pieces_commandées-pieces_a_commander) > 5);
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
    public static void UpdateAwaitPiece(object code, object commandePk, int quantite, Connection connection)
    {
        AwaitPiece awaitPiece = new AwaitPiece(connection);
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["code"] = code;
        data["commande"] = commandePk;
        data["quantite"] = quantite;
        awaitPiece.Update(data);
        awaitPiece.Insert();
    }
    public static void InputStockArrival(object code, int quantite, Connection connection)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> data = new Dictionary<string, object>();
        AwaitPiece awaitPiece = new AwaitPiece(connection);
        Dictionary<string, object> condition = new Dictionary<string, object>();
        List<string> colomns = new List<string>();
        

        int quantiteLeft = quantite;
        condition["code"] = code;
        colomns.Add("quantite");
        colomns.Add(awaitPiece.PrimaryKey.ToString());
        DataTable quantiteData = awaitPiece.LoadAll(condition, colomns);
        foreach (DataRow row in quantiteData.Rows)
        {
            quantiteLeft = quantiteLeft - Convert.ToInt32(row.ItemArray[0]);
            if (quantiteLeft >= 0)
            {
                ReserveStock(code, Convert.ToInt32(row.ItemArray[0]), connection,false);
                awaitPiece.Load(row.ItemArray[1]);
                awaitPiece.Delete();
                Logger.WriteToFile($"Added {Convert.ToInt32(row.ItemArray[0])} in reserve for {code} and removed line {row.ItemArray[1]}");
            }
            else
            {
                ReserveStock(code, quantiteLeft, connection,false);
                Dictionary<string, object> update = new Dictionary<string, object>();
                update.Add("quantite", Convert.ToInt32(row.ItemArray[0]) - quantiteLeft);
                awaitPiece.Load(row.ItemArray[1]);
                awaitPiece.Update(update);
                awaitPiece.Save();
                Logger.WriteToFile($"Added {Convert.ToInt32(row.ItemArray[0])} in reserve for {code} and removed {quantiteLeft} from line {row.ItemArray[1]}");
                break;
            }
        }
        if (quantiteLeft > 0)
        {
            Piece pieceObj = new Piece(connection);
            Dictionary<string, object> update = new Dictionary<string, object>();
            update.Add("stock", Convert.ToInt32(pieceObj.Load(code).Rows[0].ItemArray[8]) + quantiteLeft);
            pieceObj.Update(update);
            pieceObj.Load(code);
            pieceObj.Save();
            Logger.WriteToFile($"Added {quantiteLeft} in stock for {code}");
        }
    }
  public static void ExecuteReserveCommande(Connection connection, Object CommandePk)
    {
        Armoire armoire = new Armoire(connection);
        Dictionary<string, object> cond = new Dictionary<string, object>();
        cond["commande"] = CommandePk;
        List<string> colomn = new List<string>();
        colomn.Add(armoire.PrimaryKey.ToString());
        DataTable ArmoirePk = armoire.LoadAll(cond, colomn);
        List<string> listArmoirePk = new List<string>();
        Casier casier = new Casier(connection);
        List<string> listCasierPk = new List<string>();

        foreach (DataRow row  in ArmoirePk.Rows)
        {
            string currentArmoirePk = row.ItemArray[0].ToString();
            listArmoirePk.Add(currentArmoirePk);
            
            List<string> colomnsCasier = [casier.PrimaryKey.ToString()];
            Dictionary<string, object> condCas = new Dictionary<string, object>();
            condCas["armoire"] = row.ItemArray[0];
            DataTable casierPk = casier.LoadAll(condCas, colomnsCasier);
            foreach (DataRow row2 in casierPk.Rows)
            {
                listCasierPk.Add(row2.ItemArray[0].ToString());
            }
        }
        RtCasier rtcasier= new RtCasier(connection);
        RtArmoire rtarmoire = new RtArmoire(connection);
        List<string> colomns = new List<string>();
        cond = new Dictionary<string, object>();
        colomns.Add("quantite");
        colomns.Add("id_piece");
        foreach(string armoirePkString in listArmoirePk)
        {
            cond["id_armoire"] = armoirePkString;
            DataTable pieceRt = rtarmoire.LoadAll(cond, colomns);
            foreach (DataRow rowPiece in pieceRt.Rows)
            { 
                StockServices.ExecuteReserve(rowPiece.ItemArray[1], Convert.ToInt32(rowPiece.ItemArray[0]), connection); 
            }
        }
        cond = new Dictionary<string, object>();
        foreach (string casierPkString in listCasierPk)
        {
            cond["id_casier"] = casierPkString;
            DataTable pieceRt = rtcasier.LoadAll(cond, colomns);
            foreach (DataRow rowPiece in pieceRt.Rows)
            {
                StockServices.ExecuteReserve(rowPiece.ItemArray[1], Convert.ToInt32(rowPiece.ItemArray[0]), connection);
            }
                
        }
    }
    public static void AwaitAddQuantity(object code,int quantity,Connection connection)
    {
        Piece piece = new Piece(connection);
        int awaitQtt = Convert.ToInt32(piece.Load(code).Rows[0].ItemArray[10]);
        int newQtt = quantity + awaitQtt;
        piece.Attributes["await"] = newQtt;
        piece.Save();
    }
}
  





//code ecrit par comus3
//nhesitez pas a me poser
//toute question sur comment il
//marche