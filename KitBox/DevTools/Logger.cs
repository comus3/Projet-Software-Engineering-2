using System;
using System.IO;
namespace DevTools;

public static class Logger
{
    private static string? filename = null;

    public static void CreateLogFile()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string folderPath = Path.Combine(documentsPath, "execLogs");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        filename = $"Log_Of_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
        string filePath = Path.Combine(folderPath, filename);

        // Create an empty file if it doesn't exist
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    public static void WriteToFile(object data)
    {
        if (filename == null)
        {
            CreateLogFile();
        }

        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string folderPath = Path.Combine(documentsPath, "execLogs");
        string filePath = Path.Combine(folderPath, filename);

        // Write data to the end of the file
        using (StreamWriter writer = File.AppendText(filePath))
        {
            writer.WriteLine(data);
        }
    }
}
