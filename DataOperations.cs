﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Stazis
{
	/// <summary>
	/// Класс обслуживающий обработку данных
	/// </summary>
	public static class DataOperations
	{
		public enum FilterMode { Integer, Date, Text };
		public enum SearchIntMode { EqualTo, LargerThen, SmallerThen };
		public enum SearchTextMode { Equals, StartWith, Contains };
		
		public static DataTable QueryProcess(DataTable Source, int Column, DateTime Start, DateTime End)
		{
			DataTable tmp = Source.Clone();
			var query =
				from order in Source.AsEnumerable()
				where order.Field<DateTime>(Column) != null
				where order.Field<DateTime>(Column) >= Start
				where order.Field<DateTime>(Column) <= End
				orderby order.Field<DateTime>(Column) ascending
				select order;
			tmp = query.CopyToDataTable();
			return tmp.Rows.Count != 0 ? tmp : Source;
		}

		public static DataTable QueryProcess(DataTable Source, int Column, string SearchText, SearchTextMode ModeOfSearch = SearchTextMode.Equals)
		{
			DataTable tmp = Source.Clone();
			switch (ModeOfSearch)
			{
				case SearchTextMode.Equals:
					var query =
						from order in Source.AsEnumerable()
						where order.Field<string>(Column) != null
						where order.Field<string>(Column).ToUpper().Equals(SearchText.ToUpper())
						select order;
					tmp = query.CopyToDataTable<DataRow>();
					break;
				case SearchTextMode.StartWith:
					var queryStart =
						from order in Source.AsEnumerable()
						where order.Field<string>(Column) != null
						where order.Field<string>(Column).StartsWith(SearchText, StringComparison.OrdinalIgnoreCase)
						select order;
					tmp = queryStart.CopyToDataTable();
					break;
				case SearchTextMode.Contains:
					var queryCont =
						from order in Source.AsEnumerable()
						where order.Field<string>(Column) != null
						where order.Field<string>(Column).ToUpper().Contains(SearchText.ToUpper())
						select order;
					tmp = queryCont.CopyToDataTable();
					break;
			}
			return tmp;
		}

		public static DataTable QueryProcess(DataTable Source, int Column, double SearchInt, SearchIntMode ModeOfSearch = SearchIntMode.EqualTo)
		{
			DataTable tmp = Source.Clone();
			switch (ModeOfSearch)
			{
				case SearchIntMode.EqualTo:
					var query =
						from order in Source.AsEnumerable()
						where order.Field<double>(Column) != null
						where order.Field<double>(Column).Equals(SearchInt)
						select order;
					tmp = query.CopyToDataTable();
					break;
				case SearchIntMode.LargerThen:
					var queryStart =
						from order in Source.AsEnumerable()
						where order.Field<double>(Column) != null
						where order.Field<double>(Column) > SearchInt
						select order;
					tmp = queryStart.CopyToDataTable();
					break;
				case SearchIntMode.SmallerThen:
					var queryCont =
						from order in Source.AsEnumerable()
						where order.Field<double>(Column) != null
						where order.Field<double>(Column) < SearchInt
						select order;
					tmp = queryCont.CopyToDataTable();
					break;
			}
			return tmp.Rows.Count != 0 ? tmp : Source;
		}
		
		public static void ExportToCSV(DataTable dataTable, string pathOfCSVFile)
		{
			List<string> lines = new List<string>();
			string[] columnNames = dataTable.Columns.Cast<DataColumn>().
			                                  Select(column => column.ColumnName).
			                                  ToArray();
			string header = string.Join(";", columnNames);
			lines.Add(header);
			var valueLines = dataTable.AsEnumerable().Select(row => string.Join(";", row.ItemArray));            
			lines.AddRange(valueLines);
			File.WriteAllLines(pathOfCSVFile, lines, Encoding.Default);
		}
		
		public static void ExportToExcel(DataTable tableData, string pathToExport, string extension)
		{
			HSSFWorkbook exportWorkbook = new HSSFWorkbook();
			XSSFWorkbook exportWorkbookX = new XSSFWorkbook();
			ISheet exportSheet = null;
			switch (extension) 
			{
				case ".xls":
					exportWorkbook = new HSSFWorkbook(NPOI.HSSF.Model.InternalWorkbook.CreateWorkbook());
					exportSheet = exportWorkbook.CreateSheet("ExportedData");
					break;
				case ".xlsx":
					exportWorkbookX = new XSSFWorkbook();
					exportSheet = exportWorkbookX.CreateSheet("ExportedData");
					break;
			}
			exportSheet.CreateRow(0);
			for (int c = 0; c < tableData.Columns.Count; c++) 
			{
				exportSheet.GetRow(0).CreateCell(c);
				exportSheet.GetRow(0).GetCell(c).SetCellValue(tableData.Columns[c].ColumnName);
			}
			for (int i = 0; i < tableData.Rows.Count; i++) 
			{
				if (exportSheet.GetRow(i + 1) == null)
					exportSheet.CreateRow(i + 1);
				for (int j = 0; j < tableData.Columns.Count; j++) 
				{
					if (exportSheet.GetRow(i + 1).GetCell(j) == null)
						exportSheet.GetRow(i + 1).CreateCell(j);
					if (tableData.Rows[i][j] != null)
						exportSheet.GetRow(i + 1).GetCell(j).SetCellValue(tableData.Rows[i][j].ToString());
				}
			}
			switch (extension)
			{
				case ".xls":
					using (FileStream fs = new FileStream(pathToExport, FileMode.CreateNew, FileAccess.Write))
						exportWorkbook.Write(fs);
					break;
				case ".xlsx":
					using (FileStream fs = new FileStream(pathToExport, FileMode.CreateNew, FileAccess.Write)) 
					{
						exportWorkbookX.Write(fs);
						fs.Flush();
						fs.Close();
					}
					break;
			}
		}
		
		public static bool IsDate(object attemptedDate)
		{
			DateTime tmpDate;
			return DateTime.TryParse(attemptedDate.ToString(), out tmpDate);
		}
		
		public static void ChangeColumnType(DataGridView dataGrid, DataTable dataTable, int columnIndex, Type type)
		{
			int row = dataGrid.CurrentCell.RowIndex;
			int col = dataGrid.CurrentCell.ColumnIndex;
			string colName = dataTable.Columns[columnIndex].ColumnName;
			dataTable.Columns.Add(colName + "_new", type);
			foreach (DataRow dr in dataTable.AsEnumerable().AsParallel())
			{
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.DateTime:
						if (IsDate(dr[columnIndex].ToString()))
							dr[colName + "_new"] = DateTime.Parse(dr[columnIndex].ToString());
						else
						{
							try
							{
								dr[colName + "_new"] = DateTime.FromOADate(Convert.ToInt64(dr[columnIndex].ToString()));
							}
							catch
							{
								dataTable.Columns.Remove(colName + "_new");
								throw new Exception();
							}
						}
						break;
					case TypeCode.Int32:
					case TypeCode.Double:
						double tmpInt;
						DateTime tmpD;
						if (DateTime.TryParse(dr[columnIndex].ToString(), out tmpD))
						{
							dr[colName + "_new"] = new DateTime(tmpD.Year, tmpD.Month, tmpD.Day).ToOADate();
							break;
						}
						dr[colName + "_new"] = double.TryParse(dr[columnIndex].ToString(), out tmpInt) ? tmpInt : SearchDoubleInString(dr[columnIndex].ToString());
						break;
					default:
						if (dr[columnIndex] == DBNull.Value)
							dr[colName + "_new"] = string.Empty;
						else 
							dr[colName + "_new"] = dr[columnIndex].ToString();
						break;
				}
			}
			dataTable.Columns.RemoveAt(columnIndex);
			dataTable.Columns[colName + "_new"].ColumnName = colName;
			dataTable.Columns[colName].SetOrdinal(columnIndex);
			dataTable.AcceptChanges();
			dataGrid.Columns[colName].DisplayIndex = columnIndex;
			dataGrid.Columns[columnIndex].ValueType = type;
			dataGrid[col, row].Selected = true;
		}
		
		


		public static double SearchDoubleInString(string InputText)
		{
			double tmp = 0;
			string tmpStr = string.Empty;
			if (!string.IsNullOrWhiteSpace(InputText))
			{
				string[] subStr = InputText.Split(new[] { ".", " ", "(", ")", "\\", "/", ",", "-", "_", "=", "<", ">", "+" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (char val in subStr[0])
					if (char.IsNumber(val))
						tmpStr += val;
				if (!string.IsNullOrWhiteSpace(tmpStr))
				{
					if (double.TryParse(tmpStr, out tmp))
						return tmp;
				}
				throw new Exception();
			}
			return 0;
		}
		
		public static DataTable GetDataFromGrid(DataGridView Grid)
		{
			DataTable data = new DataTable(Grid.Name);
			int colCount = Grid.ColumnCount;
			for (int c = 0; c < colCount; c++)
				data.Columns.Add(Grid.Columns[c].HeaderText);
			foreach (DataGridViewRow dgvR in Grid.Rows)
			{
				DataRow dr = data.NewRow();
				for (int i = 0; i < colCount; i++)
					dr[i] = dgvR.Cells[i].Value;
				data.Rows.Add(dr);
			}
			return data;
		}
		
		public static void LoadUniques(DataTable Source, int Column, DataGridView Grid)
		{
			List<string> list = new List<string>();
			var query = from order in Source.AsEnumerable().AsParallel()
						select order.Field<string>(Column);
			list.AddRange(query.AsEnumerable());
			int Summary = 0;
			Grid.DataSource = null;
			Grid.Columns.Clear();
			Grid.Columns.Add("Number", "№ п/п");
			Grid.Columns.Add("Value", "Значение");
			Grid.Columns.Add("Count", "Количество");
			foreach (DataGridViewColumn col in Grid.Columns) col.SortMode = DataGridViewColumnSortMode.Programmatic;
			list.AsParallel()
				.GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
				.OrderByDescending(x => x.LongCount())
				.ToList()
				.ForEach(x =>
					{
						Grid.Rows.Add();
						Grid.Rows[Grid.Rows.Count - 1].Cells[0].Value = Grid.Rows.Count;
						if (string.IsNullOrWhiteSpace(x.Key))
							Grid.Rows[Grid.Rows.Count - 1].Cells[1].Value = "<пустое значение>";
						else
							Grid.Rows[Grid.Rows.Count - 1].Cells[1].Value = x.Key;
						Grid.Rows[Grid.Rows.Count - 1].Cells[2].Value = x.LongCount().ToString();
						Summary += (int)x.LongCount();
					});
			Grid.Rows.Add();
			Grid.Rows[Grid.Rows.Count - 1].Cells[1].Value = "Всего: ";
			Grid.Rows[Grid.Rows.Count - 1].Cells[2].Value = Summary.ToString();
		}
		
		public static void LoadUniquesQueryToDataGrid(DataTable SourceDT, DataGridView Grid)
        {
            if (SourceDT != null)
            {
                DataTable tmp = SourceDT.Copy();
                Grid.Columns.Clear();
                int sum = 0;
                for (int r = 0; r < SourceDT.Rows.Count; r++)
                {
                    tmp.Rows[r][0] = (r + 1).ToString();
                    sum += int.Parse(tmp.Rows[r][2].ToString());
                }
                tmp.Rows.Add("", "Всего:", sum.ToString());
                Grid.DataSource = tmp;
            }
        }
		
		static bool FileIsAvailable(string FilePath)
		{
			try
			{
				using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
					return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		
		public static int ReplaceInDataTable(Database dataBase, int sheetIndex, int Column, CheckedListBox.CheckedItemCollection InElements, string OutElement, bool ReplaceAlsoInSourceFile)
        {
            int Proceed = 0;
            List<string> elementList = InElements.Cast<string>().ToList<string>();
            if (OutElement == "<пустое значение>")
                OutElement = String.Empty;
            if (elementList.Contains("<пустое значение>"))
                elementList[elementList.IndexOf("<пустое значение>")] = String.Empty;
            HSSFWorkbook WorkbookXLS = new HSSFWorkbook();
            XSSFWorkbook WorkbookXLSX = new XSSFWorkbook();
            ISheet currentSheet = null;
            FileStream fs = null;
			if (ReplaceAlsoInSourceFile && !FileIsAvailable(dataBase.pathOfDatabase))
			{
				if (MessageBox.Show("Файл открыт в другой программе.\nПродолжить процесс замены в загруженном в программу экземпляре?", "Заменить в программе без сохранения в файле?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
					return 0;
				ReplaceAlsoInSourceFile = false;
			}
			if (ReplaceAlsoInSourceFile)
				using (fs = new FileStream(dataBase.pathOfDatabase, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
					switch (dataBase.TypeOfDB)
					{
						case Database.DBmode.XLS:
							WorkbookXLS = new HSSFWorkbook(fs);
							currentSheet = WorkbookXLS.GetSheetAt(sheetIndex);
							break;
						case Database.DBmode.XLSX:
							WorkbookXLSX = new XSSFWorkbook(fs);
							currentSheet = WorkbookXLSX.GetSheetAt(sheetIndex);
							break;
					}
            for (int row = 0; row < dataBase.listOfTables.Tables[sheetIndex].Rows.Count; row++)
            {
                if (elementList.Contains(dataBase.listOfTables.Tables[sheetIndex].Rows[row][Column].ToString()))
                {
                    dataBase.listOfTables.Tables[sheetIndex].Rows[row][Column] = OutElement;
                    if (ReplaceAlsoInSourceFile)
                        currentSheet.GetRow(row + 1).GetCell(Column).SetCellValue(OutElement);
                    Proceed++;
                }
                Application.DoEvents();
            }
            dataBase.listOfTables.AcceptChanges();
            if (ReplaceAlsoInSourceFile)
            {
                using (fs = new FileStream(dataBase.pathOfDatabase, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                switch (dataBase.TypeOfDB)
                {
                    case Database.DBmode.XLS:
                            WorkbookXLS.Write(fs);
                        break;
                    case Database.DBmode.XLSX:
                            WorkbookXLSX.Write(fs);
                        break;
                }
                fs.Close();
                WorkbookXLS = null;
                WorkbookXLSX = null;
                currentSheet = null;
            }
            return Proceed;
        }
		
		public static string GetExportFileTypes()
		{
			return 	"Книга Excel 97-2003|*.xls" +
					"|Книга Excel 2007-...|*.xlsx" +
					"|Файл CSV|*.csv";
		}

        public static int CorrectColumnRecords(Database dataBase, int sheetIndex, int Column, CheckedListBox.CheckedItemCollection Parameters, bool ReplaceAlsoInSourceFile)
        {
        	int Proceed = 0;
            HSSFWorkbook WorkbookXLS = new HSSFWorkbook();
        	XSSFWorkbook WorkbookXLSX = new XSSFWorkbook();
            ISheet currentSheet = null;
            FileStream fs = null;
            if (!FileIsAvailable(dataBase.pathOfDatabase))
            {
                if (MessageBox.Show("Файл открыт в другой программе.\nПродолжить процесс корректировки в загруженном в программу экземпляре?", "Корректировать в программе без сохранения в файле?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return 0;
                ReplaceAlsoInSourceFile = false;
            }
			if (ReplaceAlsoInSourceFile)
				using (fs = new FileStream(dataBase.pathOfDatabase, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
					switch (dataBase.TypeOfDB)
					{
						case Database.DBmode.XLS:
							WorkbookXLS = new HSSFWorkbook(fs);
							currentSheet = WorkbookXLS.GetSheetAt(sheetIndex);
							break;
						case Database.DBmode.XLSX:
							WorkbookXLSX = new XSSFWorkbook(fs);
							currentSheet = WorkbookXLSX.GetSheetAt(sheetIndex);
							break;
					}
            for (int row = 0; row < dataBase.listOfTables.Tables[sheetIndex].Rows.Count; row++)
            {
            	string text = dataBase.listOfTables.Tables[sheetIndex].Rows[row][Column].ToString();
				if (Parameters.Contains("Убрать пробелы с концов строки"))
					text = text.Trim();
				if (Parameters.Contains("Удалить пробелы из строки"))
					text = text.Replace(" ", "");
				if (Parameters.Contains("Сменить регистр записей на верхний"))
					text = text.ToUpper();
				if (Parameters.Contains("Сменить регистр записей на нижний"))
					text = text.ToLower();
                dataBase.listOfTables.Tables[sheetIndex].Rows[row][Column] = text;
                if (ReplaceAlsoInSourceFile) currentSheet.GetRow(row + 1).GetCell(Column).SetCellValue(text);
                Proceed++;
                Application.DoEvents();
            }
            dataBase.listOfTables.AcceptChanges();
            if (ReplaceAlsoInSourceFile)
            {
            	using (fs = new FileStream(dataBase.pathOfDatabase, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                switch (dataBase.TypeOfDB)
                {
                    case Database.DBmode.XLS:
                            WorkbookXLS.Write(fs);
                        break;
                    case Database.DBmode.XLSX:
                            WorkbookXLSX.Write(fs);
                        break;
                }
                fs.Close();
                WorkbookXLS = null;
                WorkbookXLSX = null;
                currentSheet = null;
            }
            return Proceed;
    	}
	}
}
