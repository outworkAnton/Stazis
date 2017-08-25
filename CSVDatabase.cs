using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Stazis
{
	class CSVDatabase : IDatabase
	{
        public List<string> NamesOfTables { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public DataSet DatabaseSet { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string DatabasePath { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int SelectedTableIndex { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public DataTable CurrentDataTable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public CSVDatabase() { }

		public void ConnectToDatabase(string pathOfFile)
		{
            DatabasePath = pathOfFile;
            using (Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				NamesOfTables.Add("Imported from CSV file data");
				DatabaseSet.Tables.Add(GetDataTableFromCSVFile(fs));
				SelectedTableIndex = 0;
			}
		}

		DataTable GetDataTableFromCSVFile(Stream fileStream)
		{
			DataTable csvData = new DataTable();
			using (TextFieldParser csvReader = new TextFieldParser(fileStream, Encoding.Default))
			{
				csvReader.SetDelimiters(new string[] { ";" });
				csvReader.HasFieldsEnclosedInQuotes = true;
				List<string> colFields = new List<string>(csvReader.ReadFields());
				foreach (string column in colFields)
				{
					DataColumn dataColumn = new DataColumn(column);
					dataColumn.AllowDBNull = true;
					if (csvData.Columns.Contains(column))
						dataColumn.ColumnName = string.Format("Столбец {0} ({1})", csvData.Columns.Count, dataColumn.ColumnName);
					if (string.IsNullOrWhiteSpace(column))
						dataColumn.ColumnName = "Столбец " + csvData.Columns.Count;
					csvData.Columns.Add(dataColumn);
				}
				while (!csvReader.EndOfData)
				{
					string[] fieldData = csvReader.ReadFields();
					for (int i = 0; i < colFields.Count; i++)
					{
						if (fieldData[i] == "")
							fieldData[i] = null;
					}
					csvData.Rows.Add(fieldData);
				}
			}
			return csvData;
		}

        public string GetTypeNameOfDatabaseFile()
        {
            return "Файл CSV";
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
