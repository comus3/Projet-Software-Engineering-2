using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

using DAL;
using DevTools;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Controls.Shapes;
using System.Text.RegularExpressions;


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
    private static void LinkCasier(Connection connection, object pkPiece, object pkCasier, int quantite)
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
        if (StockServices.MakeOrder(pkPiece, quantite, connection))
        {
            Logger.WriteToFile($"Reserve for {pkPiece} has been made");
        }
        else
        {
            Logger.WriteToFile($"Error while making reserve for {pkPiece}");
        }
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
        if (StockServices.MakeOrder(pkPiece, quantite, connection))
        {
            Logger.WriteToFile($"Reserve for {pkPiece} has been made");
        }
        else
        {
            Logger.WriteToFile($"Error while making reserve for {pkPiece}");
        }
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
            primaryKey = armoire.Attributes[armoire.PrimaryKey];
        }
        else if (toUnlink is Casier)
        {
            Casier casier = (Casier)toUnlink;
            toUnlinkColomn = "id_casier";
            table = "rt_casier";
            primaryKey = casier.Attributes[casier.PrimaryKey];
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
    /// FONCTIONNE
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="casier"></param>
    public static bool CreateAllCasierLinks(Connection connection, Casier casier)
    {
        DataTable casierData = casier.Load(casier.Attributes[casier.PrimaryKey]);
        CasierAttributes casierAttributes = new CasierAttributes(casierData.Rows[0]["id_casier"].ToString(), casierData.Rows[0]["couleur"].ToString(), Convert.ToInt32(casierData.Rows[0]["h"]), casierData.Rows[0]["porte"].ToString(), "0", casierData.Rows[0]["armoire"].ToString(), casierData.Rows[0]["couleur_porte"].ToString());
        Armoire armoire = new Armoire(connection);
        DataTable armoireData = armoire.Load(casier.Load(casier.Attributes[casier.PrimaryKey]).Rows[0]["armoire"]);
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
            if (CupHandle(connection, casierAttributes.PrimaryKey, casierAttributes.Porte, casierAttributes.CouleurPorte))
            {
                if (Door(connection, casierAttributes.Hauteur, casierAttributes.CouleurPorte, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["longueur"]), casierAttributes.Porte))
                {
                    if (SidePanel(connection, casierAttributes.Hauteur, casierAttributes.Color, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["largeur"])))
                    {
                        if (BackPanel(connection, casierAttributes.Hauteur, casierAttributes.Color, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["longueur"])))
                        {
                            if (HorizontalPanel(connection, casierAttributes.Color, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["longueur"]), Convert.ToInt32(armoireData.Rows[0]["largeur"])))
                            {
                                if (CrossBarFront(connection, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["longueur"])))
                                {
                                    if (CrossBarSide(connection, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["largeur"])))
                                    {
                                        if (CrossBarBack(connection, casierAttributes.PrimaryKey, Convert.ToInt32(armoireData.Rows[0]["longueur"])))
                                        {
                                            Logger.WriteToFile($"All pieces for casierAttributes {casierAttributes.PrimaryKey} have been linked");
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
        DataTable armoireInfo = armoire.Load(armoire.Attributes[armoire.PrimaryKey]);
        string? couleur = armoireInfo.Rows[0]["couleur"].ToString();
        string couleurCode;
        switch (couleur)
        {
            case "white":
                couleurCode = "BL";
                break;
            case "marron":
                couleurCode = "BR";
                break;
            case "galva":
                couleurCode = "GV";
                break;
            case "black":
                couleurCode = "NR";
                break;
            default:
                Logger.WriteToFile($"error, couleur {couleur} is not valid for Armoire for Primary key of armoire : {armoire.Attributes[armoire.PrimaryKey].ToString()}");
                return false;
        }
        (int hauteur, bool identical) result = GetHauteur(connection, armoire.Attributes[armoire.PrimaryKey]);
        if (result.identical)
        {
            int nombreCasier = GetCasierNombre(connection, armoire.Attributes[armoire.PrimaryKey]);
            Piece piece = new Piece(connection);
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition["Dimensions_hauteur"] = $"{nombreCasier}x{result.hauteur}(h)";
            condition["type"] = couleur;
            List<string> colomns = new List<string>();
            colomns.Add("code");
            DataTable pieceData = piece.LoadAll(condition, colomns);
            if (pieceData.Rows.Count == 0)
            {
                Logger.WriteToFile($"error, no piece found for armoire {armoire.Attributes[armoire.PrimaryKey]}");
                return false;
            }
            if (pieceData.Rows.Count > 1)
            {
                Logger.WriteToFile($"error, multiple pieces found for armoire {armoire.Attributes[armoire.PrimaryKey]}");
                return false;
            }
            LinkArmoire(connection, pieceData.Rows[0]["code"], armoire.Attributes[armoire.PrimaryKey], 4);
        }
        else
        {
            string query = $"SELECT  reference, code AS extracted_numbers FROM piece WHERE type = '{couleur}' AND code LIKE 'COR%'";
            DataTable resultData = connection.ExecuteQuery(query);
            if (resultData.Rows.Count == 0)
            {
                Logger.WriteToFile($"error, no piece found for armoire {armoire.Attributes[armoire.PrimaryKey]}");
                return false;
            }
            foreach (DataRow row in resultData.Rows)
            {
                int extractedNumbers = int.Parse(Regex.Replace(row["extracted_numbers"].ToString(), @"[^0-9]", ""));
                if (extractedNumbers > result.hauteur)
                {
                    LinkArmoire(connection, $"COR{extractedNumbers}{couleurCode}", armoire.Attributes[armoire.PrimaryKey], 4);
                    break;
                }
            }

        }

        return true;
    }
    private static int GetCasierNombre(Connection connection, object armoireId)
    {
        Casier casier = new Casier(connection);
        Dictionary<string, object> conditionCasierOfArmoire = new Dictionary<string, object>();
        conditionCasierOfArmoire["armoire"] = armoireId;
        DataTable casierData = casier.LoadAll(conditionCasierOfArmoire);
        return casierData.Rows.Count;
    }
    private static (int, bool) GetHauteur(Connection connection, object armoireId)
    {
        bool identical = true;
        int resultHeight = 0;

        Casier casier = new Casier(connection);
        Dictionary<string, object> conditionCasierOfArmoire = new Dictionary<string, object>();
        conditionCasierOfArmoire["armoire"] = armoireId;
        DataTable casierData = casier.LoadAll(conditionCasierOfArmoire);
        if (casierData.Rows.Count == 0)
        {
            throw new Exception("No casier found for armoire");
        }
        int initialHauteur = Convert.ToInt32(casierData.Rows[0]["h"]);
        foreach (DataRow casierRow in casierData.Rows)
        {
            if (casierRow["h"].ToString() != casierData.Rows[0]["h"].ToString() && identical)
            {
                identical = false;
            }
            resultHeight += Convert.ToInt32(casierRow["h"]);
        }
        if (identical)
        {
            return (initialHauteur, identical);
        }
        return (resultHeight, identical);

    }



    private static Boolean VBatten(Connection connection, int height, object pkCasier)
    {
        string pieceCode;
        switch (height)
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
    private static Boolean CupHandle(Connection connection, object pkCasier, string porte, string couleurPorte)
    {
        //2 doors (in option) with 2 cup handles (not available for glass doors)
        string pieceCode = "COUPEL";
        if (porte == "True")
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
        else if (porte == "False")
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
    private static Boolean Door(Connection connection, int height, string colorDoor, object pkCasier, int largeur, string porte)
    {
        //two doors with largeur
        //ce que je comprends pas cest que la largeur des doors minimum = la largeur minimale de l'armoire
        //pour linstant voici limplementation
        //
        // si largeur < 63 alors larguer porte = largeur armoire mais si largeur > 63 alors largeur porte = 62
        //aussi porte est en verre ou de couleur
        string type;
        string codeL;
        if (porte == "True")
        {
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
        return true;
    }
    private static Boolean SidePanel(Connection connection, int height, string color, object pkCasier, int largeur)
    {
        //2 side panels
        //of height hauteur and profondeur profondeur
        //color couleur
        //code built like this : PAG{hauteur}{profondeur}{couleur}
        string colorCode;
        switch (color)
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
    private static Boolean BackPanel(Connection connection, int height, string color, object pkCasier, int longeur)
    {
        //1 back panel
        //color height and longueur
        //code built like this : PAR{hauteur}{longueur}{couleur}
        string colorCode;
        switch (color)
        {
            case "white":
                colorCode = "BL";
                break;
            case "marron":
                colorCode = "BR";
                break;
            default:
                Logger.WriteToFile($"error, color {color} is not valid for BackPanel for Primary key of casier : {pkCasier.ToString()}");
                return false;
        }
        LinkCasier(connection, $"PAR{height}{longeur}{colorCode}", pkCasier, 1);
        return true;
    }
    private static Boolean HorizontalPanel(Connection connection, string color, object pkCasier, int largeur, int longeur)
    {
        //2 horizontal panels
        //color and longueur and largeur
        //code built like this : PAH{longueur}{largeur}{couleur}
        string colorCode;
        switch (color)
        {
            case "white":
                colorCode = "BL";
                break;
            case "marron":
                colorCode = "BR";
                break;
            default:
                Logger.WriteToFile($"error, color {color} is not valid for HorizontalPanel for Primary key of casier : {pkCasier.ToString()}");
                return false;
        }
        LinkCasier(connection, $"PAH{longeur}{largeur}{colorCode}", pkCasier, 2);
        return true;
    }
    private static Boolean CrossBarFront(Connection connection, object pkCasier, int longueur)
    {
        //2 front cross bars
        //code built like this : TRF{longueur}
        List<int> possibleValues = new List<int> { 32, 42, 52, 62, 80, 100, 120 };
        if (possibleValues.Contains(longueur))
        {
            LinkCasier(connection, $"TRF{longueur}", pkCasier, 2);
            return true;
        }
        Logger.WriteToFile($"error, longueur {longueur} is not valid for CrossBarFront for Primary key of casier : {pkCasier.ToString()}");
        return false;
    }
    private static Boolean CrossBarSide(Connection connection, object pkCasier, int largeur)
    {
        //4 side cross bars
        //code built like this : TRG{largeur}
        List<int> possibleValues = new List<int> { 32, 42, 52, 62 };
        if (possibleValues.Contains(largeur))
        {
            LinkCasier(connection, $"TRG{largeur}", pkCasier, 4);
            return true;
        }
        return false;
    }
    private static Boolean CrossBarBack(Connection connection, object pkCasier, int longeur)
    {
        //2 front cross bars
        //code built like this : TRR{longueur}
        List<int> possibleValues = new List<int> { 32, 42, 52, 62, 80, 100, 120 };
        if (possibleValues.Contains(longeur))
        {
            LinkCasier(connection, $"TRR{longeur}", pkCasier, 2);
            return true;
        }
        Logger.WriteToFile($"error, longueur {longeur} is not valid for CrossBarFront for Primary key of casier : {pkCasier.ToString()}");
        return false;
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



//code ecrit par comus3
//nhesitez paas a nous poser
//toute question sur comment il
//marche