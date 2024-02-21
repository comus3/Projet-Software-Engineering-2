using System;
using System.Data;
namespace DevTools;

public class Displayer
{
    public static void DisplayData(DataTable dataTable)
    {
        // Display column headers
        foreach (DataColumn column in dataTable.Columns)
        {
            Logger.WriteToFile($"{column.ColumnName,-20}");
        }
        Logger.WriteToFile("");

        // Display data rows
        foreach (DataRow row in dataTable.Rows)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                string value;
                if (row[column] != DBNull.Value) // Check for DBNull value if needed
                {
                    value = row[column].ToString(); // Convert value to string explicitly
                }
                else
                {
                    value = ""; // Handle DBNull value as needed
                }
                Logger.WriteToFile($"{value,-20}");
            }
            Logger.WriteToFile("");
        }


    }
}
