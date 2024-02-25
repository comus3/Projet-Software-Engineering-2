using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

using DAL;
using DevTools;
using Microsoft.Maui.Controls;
using MySqlX.XDevAPI.Common;
using Mysqlx.Session;
namespace KitBox.AppServices;
static class FetchingServices
{
    public static DataTable FetchAvailableDimensions(Connection connection)
    {
        DataTable result = connection.ExecuteQuery("SELECT dimension_largeur, dimension_profondeur FROM piece WHERE `code` REGEXP '^PAH';");
        string executionMessage = "Fetching dimensions";
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }

    public static DataTable FetchArmoirePieces(Connection connection,object armoirePk)
    {
        string getPiecePkQuery = $"SELECT id_piece FROM rt_armoire WHERE `id_armoire` = {armoirePk}";
        DataTable piecesPk = connection.ExecuteQuery(getPiecePkQuery);
        Displayer.DisplayData(piecesPk);
        string getPieceQuery = "SELECT * FROM piece WHERE `code` IN (";
        foreach (DataRow row in piecesPk.Rows)
        {
            getPieceQuery += "'";
            getPieceQuery += row[0].ToString();
            getPieceQuery += "'";
            getPieceQuery += ", ";
        }

        getPieceQuery = getPieceQuery.Substring(0, getPieceQuery.Length - 2);
        getPieceQuery += ");";
        string executionMessage = $"fetching pieces for {armoirePk}";
        DataTable result = connection.ExecuteQuery(getPieceQuery);
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }
    public static DataTable FetchCasierPieces(Connection connection,object casierPk)
    {
        string getPiecePkQuery = $"SELECT id_piece FROM rt_casier WHERE `id_casier` = {casierPk}";
        DataTable piecesPk = connection.ExecuteQuery(getPiecePkQuery);
        Displayer.DisplayData(piecesPk);
        string getPieceQuery = "SELECT * FROM piece WHERE `code` IN (";
        foreach (DataRow row in piecesPk.Rows)
        {
            getPieceQuery += "'";
            getPieceQuery += row[0].ToString();
            getPieceQuery += "'";
            getPieceQuery += ", ";
        }

        getPieceQuery = getPieceQuery.Substring(0, getPieceQuery.Length - 2);
        getPieceQuery += ");";
        string executionMessage = $"fetching pieces for {casierPk}";
        DataTable result = connection.ExecuteQuery(getPieceQuery);
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }
    public static DataTable FetchCommandePieces(Connection connection,object commandePk)
    {
        string executionMessage = $"fetching pieces for {commandePk}";
        DataTable result = new DataTable();
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }
}

