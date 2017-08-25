using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Stazis
{
    class ExcelDatabase : IDatabase
    {
        public DataSet DatabaseSet { get; private set; }
        public List<string> NamesOfTables { get; private set; }
        public int SelectedTableIndex { get; private set; }
        public string DatabasePath { get; set; }
        List<string> IDatabase.NamesOfTables { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        DataSet IDatabase.DatabaseSet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IDatabase.SelectedTableIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataTable CurrentDataTable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            SelectedTableIndex = 0;
        }

        public string GetTypeNameOfDatabaseFile()
        {
            return "Книга MS Excel";
        }

        #region IChangebleDatabase support
        public int ChangeRecords(int Column, IList<string> InputElements, string OutputElement)
        {
            if (! DataOperations.FileIsAvailable(DatabasePath))
            {
                MessageBox.Show("Закройте файл в другой программе и повторите попытку", "Файл открыт в другой программе");
                return 0;
            }
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
        #endregion
    }
}
