using System.Data;
using DevTools;


namespace DAL;

public abstract class Model
{
    protected string tableName;
    protected string primaryKey;
    protected Dictionary<string, object> attributes = new Dictionary<string, object>();
    protected Connection connection;
    public Dictionary<string, object> Attributes { get { return attributes;}}
    public string PrimaryKey{get{return primaryKey;}}
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
        if (primaryKey is string)
        {
            primaryKey = $"'{primaryKey}'";
        }
        string query = $"SELECT * FROM {this.tableName} WHERE {this.primaryKey} = {primaryKey}";
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
            // s`assurer que la clé n'est pas la clé primaire
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
                query += $" `{kvp.Key}` = {formattedValue},";
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
        string query = $"DELETE FROM {this.tableName} WHERE {this.primaryKey} = {this.attributes[primaryKey]}";
        this.connection.ExecuteQuery(query);

    }
    /// <summary>
    /// cree la ligne dans la base de donnees 
    /// si vous avez load une ligne sur cet objet
    /// et que vous faite un update
    /// puis insert dessus cest pas grave
    /// on ne tiendra pas compte du pk
    /// de la ligne loadee
    /// </summary>
    /// <returns>
    /// retourne la pk de lóbjet insere
    /// </returns>
    public virtual DataTable Insert()
    {

        // Get keys and values excluding the primary key
        var keys = this.attributes.Keys.Where(k => k != primaryKey);
        var values = keys.Select(k => this.attributes[k]);

        // Construct the SQL query
        string query = $"INSERT INTO {tableName} ({string.Join(", ", keys.Select(k => $"`{k}`"))}) VALUES ({string.Join(", ", values.Select(v => GetSqlValue(v)))});";
        this.connection.ExecuteQuery(query);
        return getLastPk();
    }
    public DataTable getPrimaryKey(Dictionary<string, object> where)
    {
        List<string> colomns = new List<string> {this.primaryKey};
        return LoadAll(where,colomns);
    }

    /// <summary>
    /// Charge toutes les lignes de la table qui correspondent aux valeurs value des values et aux colones des keys
    /// where est un dictionnaire
    /// goodeuh leuk
    /// </summary>
    /// <param name="where"></param>
    public DataTable LoadAll(Dictionary<string, object> where, List<string>? colomns = null)
    {
        if (colomns == null)
        {
            colomns = new List<string>();
        }
        string baseQuery = ColomnSelect(colomns);
        string query = baseQuery + this.tableName.ToString();
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
                query += $" `{condition.Key}` = {formattedValue} AND";
            }
            // Supprimer le dernier 'AND' intulie de la requête SQL
            query = query.Remove(query.Length - 4);
        }
        return this.connection.ExecuteQuery(query);
    }


    /// <summary>
    /// retourne la pk
    /// de la derniere ligne inseree
    /// </summary>
    /// <returns>DataTable containing desired primary key</returns>
    private DataTable getLastPk()
    {
        string query = "SELECT LAST_INSERT_ID();";
        return this.connection.ExecuteQuery(query);
    }
    /// <summary>
    /// shhhhhhh vous pouvew pas touchew a caw
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string GetSqlValue(object value)
    {
        if (value is string || value is DateTime)
        {
            return $"'{value}'";
        }
        else if (value is bool)
        {
            return ((bool)value) ? "1" : "0";
        }
        else
        {
            return value.ToString();
        }
    }
    /// <summary>
    /// retourne la liste des colomnes
    /// sous forme de basequery
    /// </summary>
    /// <param name="colomns"></param>
    /// <returns></returns>
    private string ColomnSelect(List<string> colomns)
    {
        string baseQuery = "SELECT ";
        if (colomns != null & colomns.Count != 0)
        {
            foreach (string name in colomns)
            {
                baseQuery += $"{name}, ";
            }
            //retirer le dernier ,  qui nest pas neeceessaire
            baseQuery = baseQuery.Substring(0, baseQuery.Length - 2) + " FROM ";
            return baseQuery;
        }
        else
        {
            Logger.WriteToFile("No colomns specified, returning all colomns.");
            return baseQuery+"* FROM ";
        }

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


//code ecrit par comus3
//nhesitez paas a me poser
//toute question sur comment il
//marche