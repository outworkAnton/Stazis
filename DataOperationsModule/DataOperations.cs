using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ExtensibilityInterface;

namespace DataOperationsModule
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
            return (from order in Source.AsEnumerable()
                    where order.Field<DateTime>(Column) >= Start
                    where order.Field<DateTime>(Column) <= End
                    orderby order.Field<DateTime>(Column) ascending
                    select order).CopyToDataTable();
        }

        public static DataTable QueryProcess(DataTable Source, int Column, string SearchText, SearchTextMode ModeOfSearch = SearchTextMode.Equals)
        {
            switch (ModeOfSearch)
            {
                case SearchTextMode.Equals:
                    return (from order in (Source.AsEnumerable()).Select(x => { if (x[Column].GetType() != typeof(string)) x[Column] = x[Column].ToString(); return x; })
                            where order.Field<string>(Column).ToUpper().Equals(SearchText.ToUpper())
                            select order).CopyToDataTable();
                case SearchTextMode.StartWith:
                    return (from order in (Source.AsEnumerable()).Select(x => { if (x[Column].GetType() != typeof(string)) x[Column] = x[Column].ToString(); return x; })
                            where order.Field<string>(Column).StartsWith(SearchText, StringComparison.OrdinalIgnoreCase)
                            select order).CopyToDataTable();
                case SearchTextMode.Contains:
                    return (from order in (Source.AsEnumerable()).Select(x => { if (x[Column].GetType() != typeof(string)) x[Column] = x[Column].ToString(); return x; })
                            where order.Field<string>(Column).ToUpper().Contains(SearchText.ToUpper())
                            select order).CopyToDataTable();
            }
            return Source;
        }

        public static DataTable QueryProcess(DataTable Source, int Column, double SearchInt, SearchIntMode ModeOfSearch = SearchIntMode.EqualTo)
        {
            switch (ModeOfSearch)
            {
                case SearchIntMode.EqualTo:
                    return (from order in Source.AsEnumerable()
                            where order.Field<double>(Column).Equals(SearchInt)
                            select order).CopyToDataTable();
                case SearchIntMode.LargerThen:
                    return (from order in Source.AsEnumerable()
                            where order.Field<double>(Column) > SearchInt
                            select order).CopyToDataTable();
                case SearchIntMode.SmallerThen:
                    return (from order in Source.AsEnumerable()
                            where order.Field<double>(Column) < SearchInt
                            select order).CopyToDataTable();
            }
            return Source;
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
            return DateTime.TryParse(attemptedDate.ToString(), out DateTime tmpDate);
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
            int Summary = 0;
            int lastRowIndex;
            Grid.DataSource = null;
            Grid.Columns.Clear();
            Grid.Columns.Add("Number", "№ п/п");
            Grid.Columns.Add("Value", "Значение");
            Grid.Columns.Add("Count", "Количество");
            foreach (DataGridViewColumn col in Grid.Columns) col.SortMode = DataGridViewColumnSortMode.Programmatic;
            Source.AsEnumerable().Select(x => x[Column].ToString()).GroupBy(x => x, StringComparer.OrdinalIgnoreCase).OrderByDescending(x => x.LongCount()).ToList()
                .ForEach(x =>
                    {
                        Grid.Rows.Add();
                        lastRowIndex = Grid.Rows.Count - 1;
                        Grid.Rows[lastRowIndex].Cells[0].Value = Grid.Rows.Count;
                        if (string.IsNullOrWhiteSpace(x.Key))
                            Grid.Rows[lastRowIndex].Cells[1].Value = "<пустое значение>";
                        else
                            Grid.Rows[lastRowIndex].Cells[1].Value = x.Key;
                        Grid.Rows[lastRowIndex].Cells[2].Value = x.LongCount().ToString();
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

        public static bool FileIsAvailable(string FilePath)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int ChangeRecords(IExtensibility dataBase, int Column, IList<string> InElements, string OutElement)
        {
            int Proceed = 0;
            if (OutElement == "<пустое значение>")
                OutElement = String.Empty;
            if (InElements.Contains("<пустое значение>"))
                InElements[InElements.IndexOf("<пустое значение>")] = String.Empty;
            dataBase.CurrentDataTable.AsEnumerable().Select(x =>
            {
                if (InElements.Contains(x[Column].ToString()))
                {
                    x[Column] = OutElement;
                    Proceed++;
                }
                return x;
            }).ToList();
            return Proceed;
        }

        public static int CorrectColumnRecords(IExtensibility dataBase, int sheetIndex, int Column, CheckedListBox.CheckedItemCollection Parameters, bool ReplaceAlsoInSourceFile)
        {
            int Proceed = 0;
            IWorkbook workbook = null;
            ISheet currentSheet = null;
            FileStream fs = null;
            if (!FileIsAvailable(dataBase.DatabasePath))
            {
                if (MessageBox.Show("Файл открыт в другой программе.\nПродолжить процесс корректировки в загруженном в программу экземпляре?", "Корректировать в программе без сохранения в файле?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return 0;
                ReplaceAlsoInSourceFile = false;
            }
            if (ReplaceAlsoInSourceFile)
            {
                fs = new FileStream(dataBase.DatabasePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                workbook = WorkbookFactory.Create(fs);
                currentSheet = workbook.GetSheetAt(sheetIndex);
            }
            for (int row = 0; row < dataBase.DatabaseSet.Tables[sheetIndex].Rows.Count; row++)
            {
                string text = dataBase.DatabaseSet.Tables[sheetIndex].Rows[row][Column].ToString();
                if (Parameters.Contains("Убрать пробелы с концов строки"))
                {
                    text = text.Trim();
                }
                if (Parameters.Contains("Удалить пробелы из строки"))
                {
                    text = text.Replace(" ", "");
                }
                if (Parameters.Contains("Сменить регистр записей на верхний"))
                {
                    text = text.ToUpper();
                }
                if (Parameters.Contains("Сменить регистр записей на нижний"))
                {
                    text = text.ToLower();
                }
                dataBase.DatabaseSet.Tables[sheetIndex].Rows[row][Column] = text;
                if (ReplaceAlsoInSourceFile)
                {
                    currentSheet.GetRow(row + 1).GetCell(Column).SetCellValue(text);
                }
                Proceed++;
            }
            dataBase.DatabaseSet.AcceptChanges();
            if (ReplaceAlsoInSourceFile)
            {
                workbook.Write(fs);
                fs.Close();
                workbook = null;
                currentSheet = null;
            }
            return Proceed;
        }
    }
}
