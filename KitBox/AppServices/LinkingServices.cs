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
using static KitBox.AppServices.LinkingServices;


namespace KitBox.AppServices;


static class LinkingServices
{
    public class RtCasier : Model
    {

        public RtCasier(Connection connection) : base(connection)
        {
            tableName = "rt_casier";
            primaryKey = "id_relation";
        }
    }
    public class RtArmoire : Model
    {

        public RtArmoire(Connection connection) : base(connection)
        {
            tableName = "rt_armoire";
            primaryKey = "id_relation";
        }
    }
    public static void LinkCasier(Connection connection, object pkPiece, object pkCasier)
    {
        RtCasier coucou = new RtCasier(connection);
        Dictionary<string, object> infoLink = new Dictionary<string, object>();
        infoLink["id_casier"] = pkCasier;
        infoLink["id_piece"] = pkPiece;
        coucou.Update(infoLink);
        DataTable result = coucou.Insert();
        string executionMessage = $"linking piece {pkPiece} for {pkCasier}";
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
    }

    public static void LinkArmoire(Connection connection, object pkPiece, object pkArmoire)
    {
        RtArmoire coucou = new RtArmoire(connection);
        Dictionary<string, object> infoLink = new Dictionary<string, object>();
        infoLink["id_armoire"] = pkArmoire;
        infoLink["id_piece"] = pkPiece;
        coucou.Update(infoLink);
        DataTable result = coucou.Insert();
        string executionMessage = $"linking piece {pkPiece} for {pkArmoire}";
        Logger.WriteToFile(executionMessage);
        Displayer.DisplayData(result);
    }

    public static void CreateAllCasierLinks(Connection connection, Casier casier)
    {
        //todo 
        // un casier est cree --> alors il va faloir call les methodes link casier une serie de fois
        //  pour lier toutes les pk des pieces correspondantes
        //   utilisrer le .gettype fdams la methode insert du modele pour calll cette methode
    }

    public static void CreateAllArmoireLinks(Connection connection, Armoire armoire)
    {
        //todo 
        // une armoire est cree --> alors il va faloir call les methodes link casier une serie de fois
        //  pour lier toutes les pk des pieces correspondantes
        //   utilisrer le .gettype fdams la methode insert du modele pour calll cette methode

    }
}