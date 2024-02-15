using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace DAL;

public abstract class Model
{
    protected string tableName;
    protected string primaryKey;
    protected Dictionary<string, object> attributes = new Dictionary<string, object>();

    protected SqlConnection connection;

    public Model(SqlConnection connection)
    {
        this.connection = connection;
    }
    /// <summary>
    /// Query la base de données pour récupérer les données correspondant à l'ID donné
    /// et les charge dans les attributs de l'objet
    /// </summary>
    /// <param name="primaryKey"></param>
    /// <returns></returns>
    public DataTable Load(int primaryKey)
    {
        // Construire la requête SQL pour charger la note avec la clé primaire spécifiée
        string query = $"SELECT * FROM {tableName} WHERE {primaryKey} = {primaryKey}";
        //return la datatable
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
        }

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
            query += $" {condition.Key} = {formattedValue} AND";
        }
        // Supprimer le dernier 'AND' intulie de la requête SQL
        query = query.Remove(query.Length - 4);
        return this.connection.ExecuteQuery(query);
    }

    public void Update(Dictionary<string, object> values)
    {
        // Met à jour les attributs de l'objet avec les nouvelles valeurs fournies
    }

    public void Save()
    {
        // Effectue la requête SQL appropriée pour sauvegarder les modifications dans la base de données
    }

    public void Delete()
    {
        // Supprime la ligne correspondant à l'objet dans la base de données
    }
    public void getPrimaryKey(string colomn, object value)
    {
        //recupere la primary key de la ligne dont la colonne colomn vaut value
    }
}
//EXEMPLE
/*
public class User : Model
{
    public string Name { get; set; }
    public int Age { get; set; }

    public User(SqlConnection connection) : base(connection)
    {
        tableName = "Users";
        primaryKey = "Id";
    }
}
*/
//fin exemple 