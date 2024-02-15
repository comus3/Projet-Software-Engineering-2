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

    public DataTable Load(int primaryKey)
    {
        // Query la base de données pour récupérer les données correspondant à l'ID donné
        // et les charge dans les attributs de l'objet


        // Construire la requête SQL pour charger la note avec la clé primaire spécifiée
        string query = $"SELECT * FROM {tableName} WHERE {primaryKey} = {primaryKey}";
        //return la datatable
        return this.connection.ExecuteQuery(query);
     }

    public void LoadAll()
    {
        // Charge toutes les lignes de la table dans une liste d'objets
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