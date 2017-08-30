using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using StazisExtensibilityInterface;
using System.ComponentModel.Composition;

namespace SQLiteDatabasePlugin
{
    [Export(typeof(IExtensibility))]
    public class SQLiteDatabase : IExtensibility
    {
        public IList<string> NamesOfTables { get; set; }
        public DataSet DatabaseSet { get; set; }
        public string DatabasePath { get; set; }
        public int SelectedTableIndex { get; set; }
        public DataTable CurrentDataTable => DatabaseSet.Tables[SelectedTableIndex];

        public SQLiteDatabase()
        {
            NamesOfTables = new List<string>();
            DatabaseSet = new DataSet();
        }

        public string GetTypeNameOfDatabaseFile()
        {
            return "База данных SQLite";
        }

        public void ConnectToDatabase(string pathOfFile)
        {
            DatabasePath = pathOfFile;
            using (Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var connection = (SQLiteConnection)SQLiteFactory.Instance.CreateConnection();
                var adapter = new SQLiteDataAdapter();
                connection.ConnectionString = "Data Source = " + DatabasePath;
                connection.Open();
                var tmpadapter = new SQLiteDataAdapter("SELECT name FROM sqlite_master WHERE type = 'table'", connection);
                DataTable tmpDT = new DataTable();
                tmpadapter.Fill(tmpDT);
                foreach (DataRow row in tmpDT.Rows)
                    if (!row.ItemArray.GetValue(0).ToString().Contains("sqlite")) NamesOfTables.Add(row.ItemArray.GetValue(0).ToString());
                foreach (string table in NamesOfTables)
                {
                    adapter = new SQLiteDataAdapter("select * from " + table, connection);
                    DataTable tmpdt = new DataTable();
                    adapter.Fill(tmpdt);
                    DatabaseSet.Tables.Add(tmpdt.Copy());
                }
                connection.Close();
            }
        }

        public void Reload()
        {
            throw new System.NotImplementedException();
        }

        public void DisconnectFromDatabase()
        {
            throw new System.NotImplementedException();
        }

        public void AddRecord(DataRow Record)
        {
            throw new System.NotImplementedException();
        }

        public int ChangeRecordsInColumn(int Column, IList<string> InputElements, string OutputElement)
        {
            throw new System.NotImplementedException();
        }

        public IList<string> GetDatabaseFileExtension()
        {
            return new List<string> { ".sqlite3", ".db", ".cdb" };
        }
    }
}

