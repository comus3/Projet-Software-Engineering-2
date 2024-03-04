using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

using DAL;
using DevTools;
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
    private static void unlinkAll(Connection connection, object toUnlink)
    {

        object primaryKey = new object();
        string toUnlinkColomn = "";
        string table = "";
        if (toUnlink is Armoire)
        {
            Armoire armoire = (Armoire)toUnlink;
            toUnlinkColomn = "id_armoire";
            table = "rt_armoire";
            primaryKey = armoire.PrimaryKey;
        }
        else if (toUnlink is Casier)
        {
            Casier casier = (Casier)toUnlink;
            toUnlinkColomn = "id_casier";
            table = "rt_casier";
            primaryKey = casier.PrimaryKey;
        }
        else
        {
            Exception e = new Exception("toUnlink is not a valid type");
        }
        string query = $"DELETE FROM {table} WHERE {toUnlinkColomn} = {primaryKey}";


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
        if Vbatten(connection, casier.Hauteur, casier.PrimaryKey)
        {
            if (CupHandle(connection, casier.PrimaryKey))
            {
                if (Door(connection, casier.Hauteur, casier.CouleurPorte, casier.PrimaryKey))
                {
                    if (SidePanel(connection, casier.Hauteur, casier.Color, casier.PrimaryKey))
                    {
                        if (BackPanel(connection, casier.Hauteur, casier.Color, casier.PrimaryKey))
                        {
                            if (HorizontalPanel(connection, casier.Color, casier.PrimaryKey))
                            {
                                if (CrossBarFront(connection, casier.PrimaryKey))
                                {
                                    if (CrossBarSide(connection, casier.PrimaryKey))
                                    {
                                        if (CrossBarBack(connection, casier.PrimaryKey))
                                        {
                                            Console.WriteLine("casier complet");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            
            }
        }
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
        //get couleur de l'armoire 
        DataTable armoireInfo = armoire.Load(armoire.PrimaryKey);
        string? couleur = armoireInfo.Rows[0]["couleur"].ToString();
        int hauteurtot = GetHauteur(connection, armoire.PrimaryKey);    
    }
    public static int GetHauteur(Connection connection, string armoireId)
    {
        Piece piece = new Piece(connection);
        Dictionary<string, object> condition = new Dictionary<string, object>();
        condition["armoire"] = armoireId;
        List<string> column = new List<string>();
        column.Add("hauteur");
        //get toutes les hauteurs existantes dans l'armoire
        DataTable hauteurinfo = piece.LoadAll(condition, column);

        int hauteurtot = 0;
        int countIdenticalRows = 0;
        DataRow firstRow = hauteurinfo.Rows[0];

        foreach (DataRow row in hauteurinfo.Rows)
        {
            bool identical = true;

            for (int i = 0; i < hauteurinfo.Columns.Count; i++)
            {
                if (!row[i].Equals(firstRow[i]))
                {
                    // Si les lignes sont différentes, incrémenter hauteurtot et sortir de la boucle
                    hauteurtot += Convert.ToInt32(row["hauteur"]);
                    identical = false;
                    break;
                }
            }

            if (identical)
            {
                countIdenticalRows++;
            }
        }

        // Si toutes les lignes sont identiques, retourner le nombre de fois où la valeur est apparue fois la valeur
        if (countIdenticalRows == hauteurinfo.Rows.Count)
        {
            hauteurtot = Convert.ToInt32(firstRow["hauteur"]) * countIdenticalRows;
        }

        return hauteurtot;
    }
        //si meme l de casier alors on fait autre chose encore

        //fin de autrechose avec un return dedans
            // if (couleur != null)
            // {
            //     DataTable test = connection.ExecuteQuery($"SELECT code FROM piece WHERE type='{couleur}' AND REGEXP_REPLACE(code, '[^0-9]+', '')");
                    
            // }
            // else
            // {
            //     Console.WriteLine("attention la piece ne possède pas de couleur");
            // }



    
    private static Boolean VBatten(Connection connection, int height, object pkCasier,DataTable armoireData)
    {
        return true;
    }
    private static Boolean CupHandle(Connection connection, object pkCasier,DataTable armoireData)
    {
        return true;
    }
    private static Boolean Door(Connection connection, int height, string color, object pkCasier,DataTable armoireData)
    {
        return true;
    }
    private static Boolean SidePanel(Connection connection, int height, string color, object pkCasier,DataTable armoireData)
    {
        return true;
    }
    private static Boolean BackPanel(Connection connection, int height, string color, object pkCasier,DataTable armoireData)
    {
        return true;
    }
    private static Boolean HorizontalPanel(Connection connection, string color, object pkCasier,DataTable armoireData)
    {
        return true;
    }
    private static Boolean CrossBarFront(Connection connection, object pkCasier,DataTable armoireData)
    {
        return true;
    }
    private static Boolean CrossBarSide(Connection connection, object pkCasier,DataTable armireData)
    {
        return true;
    }
    private static Boolean CrossBarBack(Connection connection, object pkCasier,DataTable armoireData)
    {
        return true;
    }
}


//code ecrit par comus3 et baptiste
//nhesitez paas a nous poser
//toute question sur comment il
//marche