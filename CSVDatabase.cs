using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Stazis
{
	class CSVDatabase : DataBaseModel, ITable
	{
		public void LoadTablesToMemory(string pathOfFile)
		{
			using (Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				TypeOfDB = DBmode.CSV;
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

        public override string GetTypeNameOfDatabaseFile()
        {
            return "Файл CSV";
        }
    }
}
