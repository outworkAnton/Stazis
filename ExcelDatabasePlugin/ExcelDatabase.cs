using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using StazisExtensibilityInterface;
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
        public DataTable CurrentDataTable => DatabaseSet.Tables[SelectedTableIndex];

        public ExcelDatabase()
        {
            NamesOfTables = new List<string>();
        }

        public bool ConnectToDatabase(string FilePath)
        {
            try
            {
                Stream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                foreach (DataTable table in DatabaseSet.Tables)
                {
                    NamesOfTables.Add(table.TableName);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetDatabaseConnectionStatus()
        {
            throw new NotImplementedException();
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

        public bool AddRecord(IList<dynamic> valuesOfRecord)
        {
            throw new System.NotImplementedException();
        }

        public bool Reload()
        {
            throw new NotImplementedException();
        }

        public bool DisconnectFromDatabase()
        {
            throw new NotImplementedException();
        }

        public int ChangeRecordsInColumn(int Column, IList<string> valuesToModifyList, string changeValue)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetDatabaseFileExtension()
        {
            return new List<string> { ".xls", ".xlsx" };
        }

        public bool DeleteRecord(int index)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRecord(int index, IList<string> valuesOfRecord)
        {
            throw new NotImplementedException();
        }
    }
}
