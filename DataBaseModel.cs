﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Stazis
{
	public abstract class DataBaseModel
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

		protected DataBaseModel(string pathOfFile)
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

        public abstract string GetNameOfType();

        public virtual bool FileIsAvailable(string FilePath)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}