using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Google.Protobuf.WellKnownTypes;
namespace AppServices;

class StockServices
{
    public static void ReserveStock(object pieceCode,int nombreReserve, Connection connection)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("stock", Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[8]) - nombreReserve);
        update.Add("reserve", Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[9]) + nombreReserve);
        piece.Update(update);
        piece.Load(pieceCode);
        piece.Save();
    }
    public static void ExecuteReserve(object pieceCode,int nombreComplete, Connection connection)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> update = new Dictionary<string, object>();
        update.Add("reserve", Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[9]) - nombreComplete);
        piece.Update(update);
        piece.Load(pieceCode);
        piece.Save();
    }
    public static bool CheckStock(object pieceCode, int nombreNecessaire,Connection connection)
    {
        Piece piece = new Piece(connection);
        return (nombreNecessaire < Convert.ToInt32(piece.Load(pieceCode).Rows[0].ItemArray[8]));
    }
    /// <summary>
    /// demander au prof comment il veut
    /// </summary>
    public static List<string> AutoCommand(Connection connection)
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
            if ((Convert.ToInt32(row.ItemArray[1]) + Convert.ToInt32(row.ItemArray[2]))<3)
            {
                aCommander.Add(row.ItemArray[0].ToString());
            }
        }   
        return  aCommander;


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
}


//code ecrit par comus3
//nhesitez pas a me poser
//toute question sur comment il
//marche