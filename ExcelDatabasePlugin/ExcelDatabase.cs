using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ExtensibilityInterface;
using System.ComponentModel.Composition;

namespace ExcelDatabasePlugin
{
    [Export(typeof(IExtensibility))]
    public class ExcelDatabase : IExtensibility
    {
        public string DatabasePath { get; set; }
        public IList<string> NamesOfTables { get; set; }
        public DataSet DatabaseSet { get; set; }
        public int SelectedTableIndex { get; set; }
        public DataTable CurrentDataTable
        {
            get
            {
                return DatabaseSet.Tables[SelectedTableIndex];
            }
        }

        public ExcelDatabase() { }

        public void ConnectToDatabase(string pathOfFile)
        {
            Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var excelReader = ExcelReaderFactory.CreateReader(fs);
            var excelDataSetConfiguration = new ExcelDataSetConfiguration
            {
                UseColumnDataType = true,
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration
                {
                    EmptyColumnNamePrefix = "Column",
                    UseHeaderRow = true
                }
            };
            DatabaseSet = excelReader.AsDataSet(excelDataSetConfiguration);
            NamesOfTables = new List<string>();
            foreach (DataTable table in DatabaseSet.Tables)
            {
                NamesOfTables.Add(table.TableName);
            }
        }

        public string GetTypeNameOfDatabaseFile()
        {
            return "Книга MS Excel";
        }

        #region IChangebleDatabase support
        public int ChangeRecords(int Column, IList<string> InputElements, string OutputElement)
        {
            int Proceed = 0;
            if (OutputElement == "<пустое значение>")
                OutputElement = string.Empty;
            if (InputElements.Contains("<пустое значение>"))
                InputElements[InputElements.IndexOf("<пустое значение>")] = string.Empty;
            IWorkbook workbook = null;
            ISheet currentSheet = null;
            using (FileStream fs = new FileStream(DatabasePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                workbook = WorkbookFactory.Create(fs);
                currentSheet = workbook.GetSheetAt(SelectedTableIndex);
                for (int row = 0; row < DatabaseSet.Tables[SelectedTableIndex].Rows.Count; row++)
                {
                    switch (currentSheet.GetRow(row + 1).GetCell(Column).CellType)
                    {
                        case CellType.String:
                            if (InputElements.Contains(currentSheet.GetRow(row + 1).GetCell(Column).StringCellValue))
                            {
                                currentSheet.GetRow(row + 1).GetCell(Column).SetCellValue(OutputElement);
                                Proceed++;
                            }
                            break;
                        case CellType.Numeric:
                            if (InputElements.Contains(currentSheet.GetRow(row + 1).GetCell(Column).NumericCellValue.ToString()))
                            {
                                currentSheet.GetRow(row + 1).GetCell(Column).SetCellValue(Convert.ToInt32(OutputElement));
                                Proceed++;
                            }
                            break;
                    }
                }
                workbook.Write(fs);
                currentSheet = null;
                workbook = null;
            }
            return Proceed;
        }
        #endregion

        public void AddRecord(DataRow Record)
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public void DisconnectFromDatabase()
        {
            throw new NotImplementedException();
        }

        public int ChangeRecordsInColumn(int Column, IList<string> InputElements, string OutputElement)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetDatabaseFileExtension()
        {
            return new List<string> { ".xls", ".xlsx" };
        }
    }
}
