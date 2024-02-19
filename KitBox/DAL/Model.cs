using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace DAL;

public abstract class Model
{
    protected string tableName;
    protected string primaryKey;
    protected Dictionary<string, object> attributes = new Dictionary<string, object>();

    protected Connection connection;

    public Model(Connection connection)
    {
        this.connection = connection;
    }
    /// <summary>
    /// Query la base de données pour récupérer les données correspondant à l'ID donné
    /// et les charge dans les attributs de l'objet
    /// </summary>
    /// <param name="primaryKey"></param>
    /// <returns></returns>
    public DataTable Load(object primaryKey)
    {
        // Construire la requête SQL pour charger la note avec la clé primaire spécifiée
        string query = $"SELECT * FROM {this.tableName} WHERE '{this.primaryKey}' = {primaryKey}";
        //return la datatable
        DataTable result = this.connection.ExecuteQuery(query);
        this.attributes[this.primaryKey] = primaryKey;
        return result;
    }

    /// <summary>
    /// Met à jour les attributs de l'objet avec les nouvelles valeurs fournies
    /// attention, cette methode ne va pas modifier la db!
    /// elle va juste changer la variable attributs de lobjet
    /// pour appliquer les modifs il faut utiliser save
    /// </summary>
    /// <param name="values"></param>

    public void Update(Dictionary<string, object> values)
    {
        foreach (var kvp in values)
        {
            this.attributes[kvp.Key] = kvp.Value;
        }
    }
    /// <summary>
    /// Aucun param en entree et aucune sortie. 
    /// execute une query qui va modifier la colone
    /// ou creer une colone dont les attributs
    /// sont les attribs du dico attributs.
    /// </summary>
    public void Save()
    {
        string query = $"UPDATE {tableName} SET";

        // Ajouter chaque colonne et sa valeur à mettre à jour à la requête SQL
        foreach (var kvp in attributes)
        {
            // s'assurer que la clé n'est pas la clé primaire
            if (kvp.Key != this.primaryKey)
            {
                // methode pr verif que la valeur est correctement formatée dans la requête SQL en fonction de son type
                string formattedValue;
                if (kvp.Value is string)
                {
                    formattedValue = $"'{kvp.Value}'";
                }
                else
                {
                    formattedValue = kvp.Value.ToString();
                }

                // Ajouter la colonne et sa valeur à mettre à jour à la requête SQL
                query += $" '{kvp.Key}' = {formattedValue},";
            }
        }

        // Supprimer la virgule finale de la requête SQL
        //rahouter le primary key pour designer la ligne quon veut
        query = query.TrimEnd(',') + $" WHERE {this.primaryKey} = {attributes[this.primaryKey]}";
        this.connection.ExecuteQuery(query);

    }
    /// <summary>
    /// Supprime la ligne correspondant à l'objet dans la base de données
    /// utilise le pk donc si vous voulexz lutiliser faire un load puis delete
    /// ca va delete celui du load.
    /// TODO (MAYBE) vous pouvez specifier le delete? a voir si cest vrmt utile
    /// </summary>
    public void Delete()
    {
        string query = $"DELETE FROM {this.tableName} WHERE '{this.primaryKey}' = {this.attributes[primaryKey]}";
        this.connection.ExecuteQuery(query);

    }
    public DataTable getPrimaryKey(Dictionary<string, object> where)
    {
        List<object> primaryKeys = new List<object>();

        // Construire la requête SQL pour sélectionner les clés primaires en fonction des conditions spécifiées
        string query = $"SELECT {primaryKey} FROM {tableName}";
        //si where ne contient rien alonrs on return toutes les pk du tableau
        // Si le dictionnaire 'where' contient des conditions, les ajouter à la requête SQL
        if (where != null && where.Count > 0)
        {
            query += " WHERE";

            // Ajouter chaque condition du dictionnaire à la requête SQL
            foreach (var condition in where)
            {
                // comme dhab correctement formatée dans la requête SQL en fonction de son type
                string formattedValue;
                if (condition.Value is string)
                {
                    formattedValue = $"'{condition.Value}'";
                }
                else
                {
                    formattedValue = condition.Value.ToString();
                }

                // Ajouter la condition à la requête SQL
                query += $" '{condition.Key}' = {formattedValue} AND";
            }

            // Supprimer le dernier 'AND' superflu de la requête SQL
            query = query.Remove(query.Length - 4);
            //recupere la primary key de la ligne dont la colonne colomn vaut value
        }

        return this.connection.ExecuteQuery(query);
    }

    /// <summary>
    /// Charge toutes les lignes de la table qui correspondent aux valeurs value des values et aux colones des keys
    /// where est un dictionnaire
    /// goodeuh leuk
    /// </summary>
    /// <param name="where"></param>
    public DataTable LoadAll(Dictionary<string, object> where)
    {
        string query = $"SELECT * FROM {this.tableName}";
        if (where != null && where.Count > 0)
        {
            query += " WHERE";


            // Ajouter chaque condition du dictionnaire à la requête SQL
            foreach (var condition in where)
            {
                //methodepour fverifier que les donnees de where sont correctement formatees avant de lajouter au string
                string formattedValue;
                if (condition.Value is string)
                {
                    formattedValue = $"'{condition.Value}'";
                }
                else
                {
                    formattedValue = condition.Value.ToString();
                }

                // Ajouter la condition à la requête SQL
                query += $" '{condition.Key}' = {formattedValue} AND";
            }
        // Supprimer le dernier 'AND' intulie de la requête SQL
        query = query.Remove(query.Length - 4);
        }
        return this.connection.ExecuteQuery(query);
    }

}
//EXEMPLE
/*
namespace DAL ; 

public class Piece : Model
{

    public Piece(Connection connection) : base(connection)
    {
        tableName = "piece";
        primaryKey = "code";
    }
}
*/
//fin exemple 


//code ecrit par come
//nhesitez paas a me poser
//toute question sur comment il
//marche