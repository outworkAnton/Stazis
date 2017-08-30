using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using StazisExtensibilityInterface;
using System.ComponentModel.Composition;

namespace SQLiteDatabasePlugin
{
    [Export(typeof(IExtensibility))]
    public class SqLiteDatabase : IExtensibility
    {
        public IList<string> NamesOfTables { get; set; }
        public DataSet DatabaseSet { get; set; }
        public string DatabasePath { get; set; }
        public int SelectedTableIndex { get; set; }
        public DataTable CurrentDataTable => DatabaseSet.Tables[SelectedTableIndex];

        public SqLiteDatabase()
        {
            NamesOfTables = new List<string>();
            DatabaseSet = new DataSet();
        }

        public string GetTypeNameOfDatabaseFile()
        {
            return "База данных SQLite";
        }

        public bool ConnectToDatabase(string filePath)
        {
            try
            {
                DatabasePath = filePath;
                var connection = (SQLiteConnection)SQLiteFactory.Instance.CreateConnection();
                var adapter = new SQLiteDataAdapter();
                connection.ConnectionString = "Data Source = " + DatabasePath;
                connection.Open();
                var tmpadapter = new SQLiteDataAdapter("SELECT name FROM sqlite_master WHERE type = 'table'", connection);
                DataTable tmpDt = new DataTable();
                tmpadapter.Fill(tmpDt);
                foreach (DataRow row in tmpDt.Rows)
                {
                    if (!row.ItemArray.GetValue(0).ToString().Contains("sqlite")) NamesOfTables.Add(row.ItemArray.GetValue(0).ToString());
                }
                foreach (string table in NamesOfTables)
                {
                    adapter = new SQLiteDataAdapter("select * from " + table, connection);
                    DataTable tmpdt = new DataTable();
                    adapter.Fill(tmpdt);
                    DatabaseSet.Tables.Add(tmpdt.Copy());
                }
                connection.Close();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool Reload()
        {
            throw new System.NotImplementedException();
        }

        public bool DisconnectFromDatabase()
        {
            throw new System.NotImplementedException();
        }

        public bool AddRecord(IList<string> valuesOfRecord)
        {
            throw new System.NotImplementedException();
        }

        public int ChangeRecordsInColumn(int column, IList<string> valuesToModifyList, string changeValue)
        {
            throw new System.NotImplementedException();
        }

        public IList<string> GetDatabaseFileExtension()
        {
            return new List<string> { ".sqlite3", ".db", ".cdb" };
        }

        public bool DeleteRecord(int index)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateRecord(int index, IList<string> valuesOfRecord)
        {
            throw new System.NotImplementedException();
        }
    }
}

