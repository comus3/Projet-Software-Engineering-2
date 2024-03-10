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

/// <summary>
/// class contenant toutes lkes methodes qui seront necesssaires pour aller
/// chercher des infos dans la db telle que dimentions possibles pour armoire
/// hauteur possible pour casier ainsi que quelles sont les pieces necessaires
/// pour une armoire une commande ou un casier
/// </summary>
static class FetchingServices
{ 
    private static string? _currentCommand;
    private static List<string> _oldCommands = new List<string>();
    public static string CurrentCommand
    {
        get { return _currentCommand; }
        set
        {
            // If there's a previous command, add it to old commands
            if (_currentCommand != null)
            {
                AddOldCommand(_currentCommand);
            }
            _currentCommand = value.ToString();
        }
    }
    public static List<string> OldCommands {  get { return _oldCommands; } }
    
    /// <summary>
    /// return tout les dimentions possibles pour les plaques horizontales de casier
    /// </summary>
    /// <param name="connection"></param>
    /// <returns> type="datatable"
    /// la datatale contenant les dimentions des plaques horizontales</returns>
    public static DataTable FetchAvailableDimensions(Connection connection)
    {
        DataTable result = connection.ExecuteQuery("SELECT dimension_largeur, dimension_profondeur FROM piece WHERE `code` REGEXP '^PAH';");
        string executionMessage = "Fetching dimensions";
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }

    /// <summary>
    /// return toutes les pieces necessaires a la construction
    /// de uniquement une armoire et non les casiers lies a cette
    /// armoire
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="armoirePk"></param>
    /// <returns></returns>
    public static DataTable FetchArmoirePieces(Connection connection,object armoirePk)
    {
        string getPiecePkQuery = $"SELECT `id_piece` FROM rt_armoire WHERE `id_armoire` = {armoirePk}";
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
        string executionMessage = $"fetching pieces for armoire {armoirePk}";
        DataTable result = connection.ExecuteQuery(getPieceQuery);
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }
    /// <summary>
    /// return toutes les pieces necessaires a la construction dun casier
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="casierPk"></param>
    /// <returns></returns>
    public static DataTable FetchCasierPieces(Connection connection,object casierPk)
    {
        string getPiecePkQuery = $"SELECT `id_piece` FROM rt_casier WHERE `id_casier` = {casierPk}";
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
        string executionMessage = $"fetching pieces for casier {casierPk}";
        DataTable result = connection.ExecuteQuery(getPieceQuery);
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }

    /// <summary>
    /// return toutes les pieces assossiees a la constructiono
    /// dune commande entiere
    /// !! ATTENTION A LA STRUCTURE DU RETURN !!
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="commandePk"></param>
    /// <returns>return un dictionnaire avec comme cle les
    /// noms des armoires et casier qui appartiennent
    /// aux dites armoires
    /// attention a ne pas executer ca si larmoire
    /// ne contient aucune piece -- > erreur
    /// </returns>
    public static Dictionary<string,DataTable> FetchCommandePieces(Connection connection,object commandePk)
    {
        string executionMessage = $"fetching pieces for {commandePk}";
        Armoire armoire = new Armoire(connection);
        Dictionary<string, object> caracteristic = new Dictionary<string, object>();
        caracteristic["commande"] = commandePk;
        DataTable armoires = armoire.LoadAll(caracteristic);
        Dictionary<string,DataTable> result = new Dictionary<string, DataTable>();
        foreach (DataRow rowArm in armoires.Rows)
        {
            string ArmoireIndex = $"ArmoireOfPk{rowArm[0]}";
            result[ArmoireIndex] = FetchArmoirePieces(connection,rowArm[0]);
            foreach (DataRow rowCas in FetchCasierAssociatedToArmoire(connection, rowArm[0]).Rows)
            {
                string CasierIndex = $"CasierOfPk{rowCas[0]}OfArm{rowArm[0]}";
                result[CasierIndex] = FetchCasierPieces(connection, rowCas[0]);
            }
        }
        Logger.WriteToFile(executionMessage);
        return result;
    }

    private static DataTable FetchCasierAssociatedToArmoire(Connection connection, object armoirePk)
    {
        DataTable result = connection.ExecuteQuery($"SELECT `id_casier` FROM casier WHERE `armoire` = {armoirePk}");
        return result;
    }
    private static void AddOldCommand(string currentCommand)
    {
        _oldCommands.Add(currentCommand);
    }
}

//code ecrit par comus3
//nhesitez paas a me poser
//toute question sur comment il
//marche

