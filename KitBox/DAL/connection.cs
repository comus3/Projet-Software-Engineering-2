using MySql.Data.MySqlClient;
using System;
using System.Data;
namespace DAL;

public static class Connection
{
    private static readonly string connectionString = "Server=localhost;Port=3306;Database=kitboxdb;Uid=root;Pwd=boubababou;Charset=utf8;SslMode=Preferred;";

    public static MySqlConnection GetConnection()
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
                Console.WriteLine("Connection successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database: {ex.Message}");
            }
        }
    }
    public static DataTable ExecuteQuery(string query)
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
                Console.WriteLine($"Error executing query: {ex.Message}");
            }
        }
        string successMsg = $"executed query {query} successfuly";
        Console.WriteLine(successMsg);
        return dataTable;
    }
}
