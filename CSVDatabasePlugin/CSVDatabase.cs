using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.ComponentModel.Composition;
using StazisExtensibilityInterface;

namespace CSVDatabasePlugin
{
    [Export(typeof(IDatabaseExtensibility))]
    public class CSVDatabase : IDatabaseExtensibility
    {
        public DataSet DatabaseSet { get; set; }
        public string DatabasePath { get; set; }
        public IList<string> NamesOfTables { get; set; }
        public int SelectedTableIndex { get; set; }
        public DataTable CurrentDataTable => DatabaseSet.Tables[SelectedTableIndex];

        public CSVDatabase() { }

        public bool ConnectToDatabase(string filePath)
        {
            try
            {
                DatabasePath = filePath;
                using (Stream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    NamesOfTables.Add("Imported from CSV file data");
                    DatabaseSet.Tables.Add(GetDataTableFromCSVFile(fs));
                }
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public string GetDatabaseConnectionStatus()
        {
            throw new System.NotImplementedException();
        }

        DataTable GetDataTableFromCSVFile(Stream fileStream)
        {
            var csvData = new DataTable();
            using (var csvReader = new TextFieldParser(fileStream, Encoding.Default))
            {
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = true;
                var colFields = new List<string>(csvReader.ReadFields());
                foreach (string column in colFields)
                {
                    var dataColumn = new DataColumn(column);
                    dataColumn.AllowDBNull = true;
                    if (csvData.Columns.Contains(column))
                        dataColumn.ColumnName = $"Столбец {csvData.Columns.Count} ({dataColumn.ColumnName})";
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

        public bool Reload()
        {
            throw new System.NotImplementedException();
        }

        public bool Export(IDatabaseExtensibility exportFrom, string filePath, bool onlyCurrentTable = false)
        {
            throw new System.NotImplementedException();
        }

        public bool DisconnectFromDatabase()
        {
            throw new System.NotImplementedException();
        }

        public bool AddRecord(IList<dynamic> valuesOfRecord)
        {
            throw new System.NotImplementedException();
        }

        public int ChangeRecordsInColumn(int Column, IList<string> valuesToModifyList, string changeValue)
        {
            throw new System.NotImplementedException();
        }

        public IList<string> GetDatabaseFileExtension()
        {
            return new List<string> { ".csv" };
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

