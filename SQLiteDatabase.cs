using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Stazis
{
	class SQLiteDatabase : DataBaseModel, IDatabase
	{
		public SQLiteDatabase() { TypeOfDB = DBmode.SQLite; }

        public override string GetTypeNameOfDatabaseFile()
        {
             return "База данных SQLite";
        }

        public void ConnectToDatabase(string pathOfFile)
		{
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
	}
}
