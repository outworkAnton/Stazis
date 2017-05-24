using System.Collections.Generic;
using System.Data;

namespace Stazis
{
	public abstract class DataBaseAbstract
	{
		public enum DBmode { XLS, XLSX, CSV, SQLite};
		public DBmode TypeOfDB { get; protected set; }
		public List<string> NamesOfTables { get; protected set; }
		public DataSet DatabaseSet { get; set; }
		public readonly string DatabasePath;

		int selectedTableIndex;
		public int SelectedTableIndex
		{
			get { return selectedTableIndex; }
			set
			{
				selectedTableIndex = value;
				CurrentDataTable = DatabaseSet.Tables[selectedTableIndex];
			}
		}
		public DataTable CurrentDataTable { get; set; }

		public DataBaseAbstract(string pathOfFile)
		{
			NamesOfTables = new List<string>();
			DatabaseSet = new DataSet();
			DatabasePath = pathOfFile;
			Load(pathOfFile);
		}

		public abstract void Load(string pathOfFile);

		public virtual void Reload()
		{
			DatabaseSet = new DataSet();
			NamesOfTables = new List<string>();
			Load(DatabasePath);
		}

		public string GetNameOfType()
		{
			switch (TypeOfDB)
			{
				case DBmode.XLS:
					return "Книга Excel 97-2003";
				case DBmode.XLSX:
					return "Книга Excel 2007-...";
				case DBmode.CSV:
					return "Файл CSV";
				case DBmode.SQLite:
					return "База данных SQLite";
				default: return "Неопознанный";
			}
		}
	}
}
