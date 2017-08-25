using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Stazis
{
	class SQLiteDatabase : IDatabase
	{
        public List<string> NamesOfTables { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public DataSet DatabaseSet { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string DatabasePath { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int SelectedTableIndex { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public DataTable CurrentDataTable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public SQLiteDatabase() { }

        public string GetTypeNameOfDatabaseFile()
        {
             return "База данных SQLite";
        }

        public void ConnectToDatabase(string pathOfFile)
		{
            DatabasePath = pathOfFile;
            using (Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				
				SQLiteFactory factory = (SQLiteFactory)System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
				SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection();
				SQLiteDataAdapter adapter = new SQLiteDataAdapter();
				connection.ConnectionString = "Data Source = " + DatabasePath;
				connection.Open();
				SQLiteDataAdapter tmpadapter = new SQLiteDataAdapter("SELECT name FROM sqlite_master WHERE type = 'table'", connection);
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
				SelectedTableIndex = 0;
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
    }
}
