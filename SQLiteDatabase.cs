using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Stazis
{
	class SQLiteDatabase : DataBaseModel
	{
		public SQLiteDatabase(string pathOfFile) : base(pathOfFile) { }

        public override string GetNameOfType()
        {
             return "База данных SQLite";
        }

        public override void Load(string pathOfFile)
		{
			using (Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				TypeOfDB = DBmode.SQLite;
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
