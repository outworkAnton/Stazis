using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Stazis
{
	class ExcelDatabase : DataBaseModel, IDatabase
	{
		public ExcelDatabase(string pathOfFile) : base(pathOfFile) { }

		public override void Load(string pathOfFile)
		{
			using (Stream fs = new FileStream(pathOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				IExcelDataReader excelReader = null;
				switch (Path.GetExtension(pathOfFile))
				{
					case ".xls":
						TypeOfDB = DBmode.XLS;
						excelReader = ExcelReaderFactory.CreateBinaryReader(fs);
						break;
					case ".xlsx":
						TypeOfDB = DBmode.XLSX;
						excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
						break;
				}
				excelReader.IsFirstRowAsColumnNames = true;
				foreach (DataTable table in excelReader.AsDataSet(true).Tables)
				{
					NamesOfTables.Add(table.TableName);
					DatabaseSet.Tables.Add(table.Copy());
				}
				excelReader.Close();
				SelectedTableIndex = 0;
			}
		}

        public override string GetNameOfType()
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
