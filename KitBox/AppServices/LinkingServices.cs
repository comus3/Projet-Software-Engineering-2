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
    private class RtCasier : Model
    {

        public RtCasier(Connection connection) : base(connection)
        {
            tableName = "rt_casier";
            primaryKey = "id_relation";
        }
    }
    private class RtArmoire : Model
    {

        public RtArmoire(Connection connection) : base(connection)
        {
            tableName = "rt_armoire";
            primaryKey = "id_relation";
        }
    }
    private class ArmoireAttributes
    {
        string PrimaryKey{get;set;}
        string Longueur{get;set;}
        string Largeur{get;set;}
        string Price{get;set;}
        string Commande{get;set;}
        public ArmoireAttributes(string primaryKey, string longueur, string largeur, string price, string commande)
        {
            PrimaryKey = primaryKey;
            Longueur = longueur;
            Largeur = largeur;
            Price = price;
            Commande = commande;
        }
    }
    private class CasierAttributes
    {
        string PrimaryKey{get;set;}
        string Color{get;set;}
        string Hauteur{get;set;}
        string Porte{get;set;}
        string Price{get;set;}
        string Armoire{get;set;}
        string CouleurPorte{get;set;}
        public CasierAttributes(string primaryKey, string color, string hauteur, string porte, string price, string armoire, string couleurPorte)
        {
            PrimaryKey = primaryKey;
            Color = color;
            Hauteur = hauteur;
            Porte = porte;
            Price = price;
            Armoire = armoire;
            CouleurPorte = couleurPorte;
        }
    }

    /// <summary>
    /// lie un casier a une piece
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="pkPiece"></param>
    /// <param name="pkCasier"></param>
    private static void LinkCasier(Connection connection, object pkPiece, object pkCasier)
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
    /// <summary>
    /// lie une armoire a une piece
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="pkPiece"></param>
    /// <param name="pkArmoire"></param>
    private static void LinkArmoire(Connection connection, object pkPiece, object pkArmoire)
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
    /// <summary>
    /// lie automatiquement,
    /// en fonction de ses caracteristiques un casier a toutes
    /// les pieces necessaires a sa construction
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="casier"></param>
    public static void CreateAllCasierLinks(Connection connection, Casier casier)
    {
        
        //todo 
        // un casier est cree --> alors il va faloir call les methodes link casier une serie de fois
        //  pour lier toutes les pk des pieces correspondantes
        //   utilisrer le .gettype fdams la methode insert du modele pour calll cette methode
    }
    /// <summary>
    /// lie automatiquement une armoire a
    /// toutes les pieces necessaires a sa construction
    /// toutes les pieces necessaires a sa construction
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="armoire"></param>
    public static void CreateAllArmoireLinks(Connection connection, Armoire armoire)
    {
        //todo 
        // une armoire est cree --> alors il va faloir call les methodes link casier une serie de fois
        //  pour lier toutes les pk des pieces correspondantes
        //   utilisrer le .gettype fdams la methode insert du modele pour calll cette methode

    }
}


//code ecrit par comus3
//nhesitez paas a me poser
//toute question sur comment il
//marche