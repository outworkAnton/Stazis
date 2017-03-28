using System.Text;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Stazis
{
    /// <summary>
    /// Класс обслуживающий создание объекта базы данных
    /// </summary>
    /// 

    public class Database
	{
		public readonly string pathOfDatabase;
		DBmode typeOfDB;
		public readonly DataSet listOfTables;
		public readonly List<string> namesOfTables;
		public DBmode TypeOfDB { get{return typeOfDB;} }
		public enum DBmode { XLS, XLSX, CSV, SQLite};
		public DataTable currentTable { get; set; }
		
		public Database(string pathOfDBFile)
		{
			pathOfDatabase = pathOfDBFile;
			listOfTables = new DataSet();
			namesOfTables = new List<string>();
			LoadDatabase();
		}
		
		public string GetTypeOfDBFile()
		{
			string[] types = GetImportFileTypes().Split('|');
			switch (TypeOfDB)
				{
				case DBmode.XLS:
					return types[0];//"Книга Excel 97-2003";
				case DBmode.XLSX:
					return types[2];//"Книга Excel 2007-...";
				case DBmode.CSV:
					return types[4];//Файл CSV
				case DBmode.SQLite:
					return types[6];//"SQLite database";
				default: return "Неизвестный";
				}
		}
		
		void LoadDatabase()
		{
			using (FileStream fs = new FileStream(pathOfDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				IExcelDataReader excelReader;
				switch (Path.GetExtension(pathOfDatabase))
				{
					case ".xls":
						typeOfDB = DBmode.XLS;
						excelReader = ExcelReaderFactory.CreateBinaryReader(fs);
						excelReader.IsFirstRowAsColumnNames = true;
						foreach (DataTable table in excelReader.AsDataSet(true).Tables)
						{
							namesOfTables.Add(table.TableName);
							listOfTables.Tables.Add(table.Copy());
						}
						excelReader.Close();
						break;
					case ".xlsx":
						typeOfDB = DBmode.XLSX;
						excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
						excelReader.IsFirstRowAsColumnNames = true;
						foreach (DataTable table in excelReader.AsDataSet(true).Tables)
						{
							namesOfTables.Add(table.TableName);
							listOfTables.Tables.Add(table.Copy());
						}
						excelReader.Close();
						break;
					case ".csv":
						typeOfDB = DBmode.CSV;
						namesOfTables.Add("Imported from CSV file data");
						listOfTables.Tables.Add(GetDataTableFromCSVFile(fs));
						break;
					case ".db":
					case ".cdb":
					case ".sqlite3":
						typeOfDB = DBmode.SQLite;
						SQLiteFactory factory = (SQLiteFactory) System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
						SQLiteConnection connection = (SQLiteConnection) factory.CreateConnection();
						SQLiteDataAdapter adapter = new SQLiteDataAdapter();
						connection.ConnectionString = "Data Source = " + pathOfDatabase;
						connection.Open();
						SQLiteDataAdapter tmpadapter = new SQLiteDataAdapter("SELECT name FROM sqlite_master WHERE type = 'table'", connection);
						DataTable tmpDT = new DataTable();
						tmpadapter.Fill(tmpDT);
						foreach (DataRow row in tmpDT.Rows)
							if (!row.ItemArray.GetValue(0).ToString().Contains("sqlite")) namesOfTables.Add(row.ItemArray.GetValue(0).ToString());
						foreach (string table in namesOfTables) 
						{
							adapter = new SQLiteDataAdapter("select * from " + table, connection);
							DataTable tmpdt = new DataTable();
							adapter.Fill(tmpdt);
							listOfTables.Tables.Add(tmpdt.Copy());
						}
						connection.Close();		
						break;
				}
				GC.Collect();
			}
		}
		
		public static string GetImportFileTypes()
		{
			return 	"Книга Excel 97-2003|*.xls" +
					"|Книга Excel 2007-...|*.xlsx" +
					"|Файл CSV|*.csv" +
					"|База данных SQLite|*.cdb;*.db;*.sqlite3" +
					"|Все файлы|*.*";
		}
		
		DataTable GetDataTableFromCSVFile(Stream fileStream)
        {
            DataTable csvData = new DataTable();
              using(TextFieldParser csvReader = new TextFieldParser(fileStream, Encoding.Default))
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
      }
    }
