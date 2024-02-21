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
                Logger.WriteToFile($"{row[column],-20}");
            }
            Logger.WriteToFile("");
        }
    }
}
