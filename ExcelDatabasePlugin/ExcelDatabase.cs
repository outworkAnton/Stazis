using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel.Composition;
using StazisExtensibilityInterface;

namespace ExcelDatabasePlugin
{
    [Export(typeof(IDatabaseExtensibility))]
    public class ExcelDatabase : IDatabaseExtensibility
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
                using (Stream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
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
                    excelReader.Close();
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
            return string.Empty;
        }

        public string GetTypeNameOfDatabaseFile()
        {
            return "Книга MS Excel";
        }

        public bool AddRecord(IList<dynamic> valuesOfRecord)
        {
            throw new System.NotImplementedException();
        }

        public bool Reload()
        {
            throw new NotImplementedException();
        }

        public bool Export(IDatabaseExtensibility exportFrom, string filePath, bool onlyCurrentTable = false)
        {
            try
            {
                using (Stream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    IWorkbook workbook = null;
                    if (Path.GetExtension(filePath) == ".xls")
                    {
                        workbook = new HSSFWorkbook();
                    }
                    else
                    {
                        workbook = new XSSFWorkbook();
                    }
                    if (onlyCurrentTable)
                    {
                        ISheet exportSheet = workbook.CreateSheet(exportFrom.CurrentDataTable.TableName);
                        if (!FillSheetFromTable(ref exportSheet, exportFrom.CurrentDataTable))
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        foreach (DataTable dataTable in exportFrom.DatabaseSet.Tables)
                        {
                            ISheet exportSheet = workbook.CreateSheet(dataTable.TableName);
                            if (!FillSheetFromTable(ref exportSheet, dataTable))
                            {
                                throw new Exception();
                            }
                        }
                    }
                    workbook.Write(fs);
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Export failed: " + e.Message);
            }
        }

        private bool FillSheetFromTable(ref ISheet sheet, DataTable dataTable)
        {
            try
            {
                sheet.CreateRow(0);
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    sheet.GetRow(0).CreateCell(c);
                    sheet.GetRow(0).GetCell(c).SetCellValue(dataTable.Columns[c].ColumnName);
                }
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (sheet.GetRow(i + 1) == null)
                        sheet.CreateRow(i + 1);
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (sheet.GetRow(i + 1).GetCell(j) == null)
                            sheet.GetRow(i + 1).CreateCell(j);
                        if (dataTable.Rows[i][j] != null)
                            sheet.GetRow(i + 1).GetCell(j).SetCellValue(dataTable.Rows[i][j].ToString());
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool DisconnectFromDatabase()
        {
            return true;
        }

        public int ChangeRecordsInColumn(int Column, IList<string> valuesToModifyList, string changeValue)
        {
            int Proceed = 0;
            if (changeValue == "<пустое значение>")
                changeValue = string.Empty;
            if (valuesToModifyList.Contains("<пустое значение>"))
                valuesToModifyList[valuesToModifyList.IndexOf("<пустое значение>")] = string.Empty;
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
                            if (valuesToModifyList.Contains(currentSheet.GetRow(row + 1).GetCell(Column).StringCellValue))
                            {
                                currentSheet.GetRow(row + 1).GetCell(Column).SetCellValue(changeValue);
                                Proceed++;
                            }
                            break;
                        case CellType.Numeric:
                            if (valuesToModifyList.Contains(currentSheet.GetRow(row + 1).GetCell(Column).NumericCellValue.ToString()))
                            {
                                currentSheet.GetRow(row + 1).GetCell(Column).SetCellValue(Convert.ToInt32(changeValue));
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
