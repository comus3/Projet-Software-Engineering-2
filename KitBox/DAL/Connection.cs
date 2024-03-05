using MySql.Data.MySqlClient;
using System;
using System.Data;
using DevTools;
namespace DAL;


/// <summary>
/// Pour setup la connection et la passer aux 
/// modeles il faut changer les parametres dans
/// le connection string
/// 
/// en suite vous pouvez tester la connection avec Test connection.
/// 
/// si tout est bon vous pouvez utiliser execute query pour les custom query et
/// les methodes des modeles en leur passsant les connections
/// </summary>
public class Connection
{
    private static readonly string connectionString = "Server=pat.infolab.ecam.be;Port=63408;Database=kitboxdb;Uid=newuser;Pwd=boubababou;Charset=utf8;SslMode=Preferred;";


    private static MySqlConnection GetConnection()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        return connection;
    }

    public static void TestConnection()
    {
        using (MySqlConnection connection = GetConnection())
        {
            try
            {
                connection.Open();
                Logger.WriteToFile("Connection successful!");
            }
            catch (Exception ex)
            {
                Logger.WriteToFile($"Error connecting to database: {ex.Message}");
            }
        }
    }
    public DataTable ExecuteQuery(string query)
    {
        DataTable dataTable = new DataTable();
        using (MySqlConnection connection = GetConnection())
        {
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                Logger.WriteToFile($"Error executing query: {ex.Message}");
            }
        }
        string successMsg = $"executed query {query} successfuly";
        Logger.WriteToFile(successMsg);
        return dataTable;
    }
}

//code ecrit par comus3
//nhesitez paas a me poser
//toute question sur comment il
//marche