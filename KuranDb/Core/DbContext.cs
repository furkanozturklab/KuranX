using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Documents;

namespace KuranDb.Core
{
    public class DbContext : IDisposable
    {
        private string DbPath { get; set; }
        private SqliteConnection connection;
        private List<string> temp = new List<string>();

        public DbContext()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KuranSunnetullah");
            Directory.CreateDirectory(folder);
            DbPath = Path.Combine(folder, "Ayet.db");
            string connectionString = $"Data Source={DbPath};";
            connection = new SqliteConnection(connectionString);
            connection.Open();
        }

        public void ExecuteQuery(string query)
        {
            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public string commandClear(string query)
        {
            string[] com = new string[] { "insert", "INSERT", "delete", "DELETE", "update", "UPDATE", "drop", "DROP", "alter", "ALTER", "create", "CREATE" };
            foreach (var item in com)
            {

                if (query.Contains(item))
                {
                    return query.Replace(@"
", "").Replace(@"\n", "");
                }
            }
            return null;
        }

        public List<string> LoadSqlFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // Dosya mevcut değilse işlem yapma.
                return null;
            }

            string sqlCommands;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                StringBuilder sqlCommandsBuilder = new StringBuilder();
                while ((line = reader.ReadLine()) != null)
                {
                    // Boş satırları ve -- ile başlayan açıklama satırlarını atlayarak diğer satırları ekleyin.
                    string trimmedLine = line.Trim();
                    if (!string.IsNullOrEmpty(trimmedLine) && !trimmedLine.StartsWith("--") && !trimmedLine.StartsWith("/*!") && !string.IsNullOrWhiteSpace(trimmedLine) && !trimmedLine.StartsWith(@"\r") && !trimmedLine.StartsWith(@"\n"))
                    {
                        sqlCommandsBuilder.AppendLine(line);
                    }
                }
                sqlCommands = sqlCommandsBuilder.ToString();
            }

            // Verilen SQL dosyasında ; ile bitiminde komutlara ayırır.
            string[] commands = sqlCommands.Split(';', StringSplitOptions.RemoveEmptyEntries);


            foreach (string commandText in commands)
            {
                if (!string.IsNullOrWhiteSpace(commandText))
                {
                    temp.Add(commandClear(commandText));
                    //ExecuteQuery(commandText);
                }
            }

            return temp;
        }

        public void Dispose()
        {
            connection.Close();
            ((IDisposable)connection).Dispose();
        }
    }
}