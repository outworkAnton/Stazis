using System.Collections.Generic;
using System.Data;

namespace Stazis
{
	public abstract class DataBaseAbstract
	{
		public enum DBmode { XLS, XLSX, CSV, SQLite};
		public DBmode TypeOfDB { get; set; }
		public List<string> NamesOfTables { get; set; }
		public DataSet DatabaseSet { get; set; }
		public readonly string DatabasePath;
		public int SelectedTableIndex { get; set; }

		public DataBaseAbstract(string pathOfFile)
		{
			NamesOfTables = new List<string>();
			DatabaseSet = new DataSet();
			DatabasePath = pathOfFile;
			Load(pathOfFile);
		}

		public abstract void Load(string pathOfFile);
		public abstract void Reload();

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
