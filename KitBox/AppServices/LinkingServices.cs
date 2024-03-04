using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

using DAL;
using DevTools;


namespace AppServices;


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
        string PrimaryKey { get; set; }
        string Longueur { get; set; }
        string Largeur { get; set; }
        string Price { get; set; }
        string Commande { get; set; }
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
        public string PrimaryKey { get; set; }
        public string Color { get; set; }
        public int Hauteur { get; set; }
        public string Porte { get; set; }
        public string Price { get; set; }
        public string Armoire { get; set; }
        public string CouleurPorte { get; set; }
        public CasierAttributes(string primaryKey, string color, int hauteur, string porte, string price, string armoire, string couleurPorte)
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
    private static void LinkCasier(Connection connection, object pkPiece, object pkCasier,int quantite)
    {
        RtCasier coucou = new RtCasier(connection);
        Dictionary<string, object> infoLink = new Dictionary<string, object>();
        infoLink["id_casier"] = pkCasier;
        infoLink["id_piece"] = pkPiece;
        infoLink["quantite"] = quantite;
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
    private static void LinkArmoire(Connection connection, object pkPiece, object pkArmoire, int quantite)
    {
        RtArmoire coucou = new RtArmoire(connection);
        Dictionary<string, object> infoLink = new Dictionary<string, object>();
        infoLink["id_armoire"] = pkArmoire;
        infoLink["id_piece"] = pkPiece;
        infoLink["quantite"] = quantite;
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
    public static bool CreateAllCasierLinks(Connection connection, Casier casier)
    {
        DataTable casierData = casier.Load(casier.PrimaryKey);
        CasierAttributes casierAttributes = new CasierAttributes(casier.PrimaryKey, casierData.Rows[0]["couleur"].ToString(), Convert.ToInt32(casierData.Rows[0]["h"]), casierData.Rows[0]["porte"].ToString(), "0", casierData.Rows[0]["armoire"].ToString(), casierData.Rows[0]["couleur_porte"].ToString());
        Armoire armoire = new Armoire(connection);
        DataTable armoireData = armoire.Load(casier.Load(casier.PrimaryKey).Rows[0]["armoire"]);
        if (armoireData.Rows.Count == 0)
        {
            throw new Exception("No armoire found for casier");
        }
        if (armoireData.Rows.Count > 1)
        {
            throw new Exception("Multiple armoires found for casier");
        }

        static void raiseError(string message, Connection connection, Casier casier)
        {
            unlinkAll(connection, casier);
            throw new Exception(message);
        }
        if (VBatten(connection, casierAttributes.Hauteur, casierAttributes.PrimaryKey))
        {
            if (CupHandle(connection, casierAttributes.PrimaryKey, casierAttributes.Porte,casierAttributes.CouleurPorte))
            {
                if (Door(connection, casierAttributes.Hauteur, casierAttributes.CouleurPorte, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["largeur"])))
                {
                    if (SidePanel(connection, casierAttributes.Hauteur, casierAttributes.Color, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["largeur"])))
                    {
                        if (BackPanel(connection, casierAttributes.Hauteur, casierAttributes.Color, casierAttributes.PrimaryKey, armoireData))
                        {
                            if (HorizontalPanel(connection, casierAttributes.Color, casierAttributes.PrimaryKey, armoireData))
                            {
                                if (CrossBarFront(connection, casierAttributes.PrimaryKey, armoireData))
                                {
                                    if (CrossBarSide(connection, casierAttributes.PrimaryKey, armoireData))
                                    {
                                        if (CrossBarBack(connection, casierAttributes.PrimaryKey, armoireData))
                                        {
                                            Logger.WriteToFile($"All pieces for casierAttributes {casier.PrimaryKey} have been linked");
                                            return true;
                                        }
                                        raiseError("error while linking CrossBarBack", connection, casier);
                                        return false;
                                    }
                                    raiseError("error while linking CrossBarSide", connection, casier);
                                    return false;
                                }
                                raiseError("error while linking CrossBarFront", connection, casier);
                                return false;
                            }
                            raiseError("error while linking Horizontal Panel", connection, casier);
                            return false;
                        }
                        raiseError("error while linking BackPannel", connection, casier);
                        return false;
                    }
                    raiseError("error while linking side pannels", connection, casier);
                    return false;
                }
                raiseError("error while linking Door", connection, casier);
                return false;
            }
            raiseError("error while linking cupHandle", connection, casier);
            return false;
        }
        raiseError("error while linking vBatte", connection, casier);
        return false;
    }
    /// <summary>
    /// lie automatiquement une armoire a
    /// toutes les pieces necessaires a sa construction
    /// toutes les pieces necessaires a sa construction
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="armoire"></param>
    public static bool CreateAllArmoireLinks(Connection connection, Armoire armoire)
    {
        //get couleur de l'armoire 
        DataTable armoireInfo = armoire.Load(armoire.PrimaryKey);
        string? couleur = armoireInfo.Rows[0]["couleur"].ToString();
        int hauteurtot = GetHauteur(connection, armoire.PrimaryKey);
        return true;
    }
    private static int GetHauteur(Connection connection, string armoireId)
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



    private static Boolean VBatten(Connection connection, int height, object pkCasier)
    {
        string pieceCode;
        switch(height)
        {
            case 32:
                pieceCode = "TAS27";
                break;
            case 42:
                pieceCode = "TAS37";
                break;
            case 52:
                pieceCode = "TAS47";
                break;
            default:
                Logger.WriteToFile($"error, heught {height} is not valid for VBatten for Primary key of casier : {pkCasier.ToString()}");
                return false;
        }
        LinkCasier(connection, pieceCode, pkCasier, 4);
        return true;
    }
    private static Boolean CupHandle(Connection connection, object pkCasier,string porte,string couleurPorte)
    {
        //2 doors (in option) with 2 cup handles (not available for glass doors)
        string pieceCode = "COUPEL";
        if (porte == "1")
        {
            if (couleurPorte != "verre")
            {
                LinkCasier(connection, pieceCode, pkCasier, 2);
                return true;
            }
            else
            {
                //no cup handle for glass doors
                return true;
            }
        }
        else if(porte == "0")
        {
            //no cup handle for no doors
            return true;
        }
        else
        {
            //invalid porte
            Logger.WriteToFile($"error, porte {porte} is not valid for CupHandle for Primary key of casier : {pkCasier.ToString()}");
            return false;
        }
    }
    private static Boolean Door(Connection connection, int height, string colorDoor, object pkCasier, int largeur)
    {
        //two doors with largeur
        //ce que je comprends pas cest que la largeur des doors minimum = la largeur minimale de l'armoire
        //pour linstant voici limplementation
        //
        // si largeur < 63 alors larguer porte = largeur armoire mais si largeur > 63 alors largeur porte = 62
        //aussi porte est en verre ou de couleur
        string type;
        string codeL;
        if (largeur < 63)
        {
            codeL = largeur.ToString();
        }
        else if (largeur > 63)
        {
            codeL = "62";
        }
        else
        {
            Logger.WriteToFile($"error, largeur {largeur} is not valid for Door for Primary key of casier : {pkCasier.ToString()}");
            return false;
        }
        switch (colorDoor)
        {
            case "glass":
                type = "VE";
                break;
            case "white":
                type = "BL";
                break;
            case "marron":
                type = "BR";
                break;
            default:
                Logger.WriteToFile($"error, colorDoor {colorDoor} is not valid for Door for Primary key of casier : {pkCasier.ToString()}");
                return false;
        }
        LinkCasier(connection, $"POR{height}{codeL}{type}", pkCasier, 2);
        return true;
    }
    private static Boolean SidePanel(Connection connection, int height, string color, object pkCasier, int largeur)
    {
        //2 side panels
        //of height hauteur and profondeur profondeur
        //color couleur
        //code built like this : PAG{hauteur}{profondeur}{couleur}
        string colorCode;
        switch(color)
        {
            case "white":
                colorCode = "BL";
                break;
            case "marron":
                colorCode = "BR";
                break;
            default:
                Logger.WriteToFile($"error, color {color} is not valid for SidePanel for Primary key of casier : {pkCasier.ToString()}");
                return false;
        }
        LinkCasier(connection, $"PAG{height}{largeur}{colorCode}", pkCasier, 2);
        return true;
    }
    private static Boolean BackPanel(Connection connection, int height, string color, object pkCasier, DataTable armoireData)
    {
        return true;
    }
    private static Boolean HorizontalPanel(Connection connection, string color, object pkCasier, DataTable armoireData)
    {
        return true;
    }
    private static Boolean CrossBarFront(Connection connection, object pkCasier, DataTable armoireData)
    {
        return true;
    }
    private static Boolean CrossBarSide(Connection connection, object pkCasier, DataTable armireData)
    {
        return true;
    }
    private static Boolean CrossBarBack(Connection connection, object pkCasier, DataTable armoireData)
    {
        return true;
    }
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



//code ecrit par comus3 et baptiste
//nhesitez paas a nous poser
//toute question sur comment il
//marche