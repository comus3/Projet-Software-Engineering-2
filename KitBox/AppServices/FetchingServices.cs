﻿using System;
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
    /// <summary>
    /// return toutes les pieces necessaires a la construction dun casier
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="casierPk"></param>
    /// <returns></returns>
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

    /// <summary>
    /// return toutes les pieces assossiees a la constructiono
    /// dune commande entiere
    /// a voir comment je vais structurer tout ca
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="commandePk"></param>
    /// <returns></returns>
    public static DataTable FetchCommandePieces(Connection connection,object commandePk)
    {
        string executionMessage = $"fetching pieces for {commandePk}";
        DataTable result = new DataTable();
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
        return result;
    }
}

//code ecrit par comus3
//nhesitez paas a me poser
//toute question sur comment il
//marche
