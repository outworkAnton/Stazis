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
	class ExcelDatabase : DataBaseModel, ITable
	{
		public void LoadTablesToMemory(string pathOfFile)
		{
			using (Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				var excelReader = ExcelReaderFactory.CreateReader(fs);
                switch (Path.GetExtension(pathOfFile))
				{
					case ".xls":
						TypeOfDB = DBmode.XLS;
						break;
					case ".xlsx":
						TypeOfDB = DBmode.XLSX;
						break;
				}
                var excelDataSetConfiguration = new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = true,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        EmptyColumnNamePrefix = "Column",
                        UseHeaderRow = true,
                        ReadHeaderRow = (rowReader) =>
                        {
                            rowReader.Read();
                        }
                    }
                };
                foreach (DataTable table in excelReader.AsDataSet(excelDataSetConfiguration).Tables)
				{
					NamesOfTables.Add(table.TableName);
					DatabaseSet.Tables.Add(table);
				}
				//excelReader.Close();
				SelectedTableIndex = 0;
			}
		}

        public override string GetTypeNameOfDatabaseFile()
        {
            switch (TypeOfDB)
            {
                case DBmode.XLS:
                    return "Книга Excel 97-2003";
                case DBmode.XLSX:
                    return "Книга Excel 2007-...";
                default: return string.Empty;
            }
        }

        #region IChangebleDatabase support
        public int ChangeRecords(int Column, IList<string> InputElements, string OutputElement)
		{
			if (!FileIsAvailable(DatabasePath))
			{
				MessageBox.Show("Закройте файл в другой программе и повторите попытку", "Файл открыт в другой программе");
				return 0;
			}
			int Proceed = 0;
			if (OutputElement == "<пустое значение>")
				OutputElement = string.Empty;
			if (InputElements.Contains("<пустое значение>"))
				InputElements[InputElements.IndexOf("<пустое значение>")] = string.Empty;
			HSSFWorkbook WorkbookXLS = new HSSFWorkbook();
			XSSFWorkbook WorkbookXLSX = new XSSFWorkbook();
			ISheet currentSheet = null;
			using (FileStream fs = new FileStream(DatabasePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				switch (TypeOfDB)
				{
					case DBmode.XLS:
						WorkbookXLS = new HSSFWorkbook(fs);
						currentSheet = WorkbookXLS.GetSheetAt(SelectedTableIndex);
						break;
					case DBmode.XLSX:
						WorkbookXLSX = new XSSFWorkbook(fs);
						currentSheet = WorkbookXLSX.GetSheetAt(SelectedTableIndex);
						break;
				}
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
				switch (TypeOfDB)
				{
					case DBmode.XLS:
						WorkbookXLS.Write(fs);
						break;
					case DBmode.XLSX:
						WorkbookXLSX.Write(fs);
						break;
				}
				WorkbookXLS = null;
				WorkbookXLSX = null;
				currentSheet = null;
			}
			return Proceed;
		}

		public void AddRecord(DataRow Record)
		{

		} 
		#endregion
	}
}
