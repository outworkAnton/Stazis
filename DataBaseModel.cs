using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Stazis
{
	public abstract class DataBaseModel
	{
		public enum DBmode { Excel, CSV, SQLite};
		public DBmode TypeOfDB { get; protected set; }
		public List<string> NamesOfTables { get; protected set; }
		public DataSet DatabaseSet { get; protected set; }
		public string DatabasePath { get; protected set; }

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

		protected DataBaseModel()
		{
			NamesOfTables = new List<string>();
			DatabaseSet = new DataSet();
		}

		public virtual void Reload()
		{
			DatabaseSet = new DataSet();
			NamesOfTables = new List<string>();
		}

        public abstract string GetTypeNameOfDatabaseFile();

        public virtual bool FileIsAvailable(string FilePath)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
