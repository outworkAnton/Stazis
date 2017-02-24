using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

//TODO диаграммы
//TODO многоуровневая сортировка
//TODO мгогоуровневая фильтрация
//TODO система макросов

namespace Stazis
{
	public partial class MainForm : Form
	{
		public Database DB { get; set; }
		
		public string AppDir = Application.StartupPath;
		public ValueFilters filtersForm;
		public RecentAndDirectives recentForm;
		public Replacer replacerForm;
		public Uniques uniquesForm;
		string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		enum SaveSearchResult { Yes, No }
		SaveSearchResult saveSearchResult = SaveSearchResult.No;
		List<DataTable> backUpSet;
		List<Type> typesColumnList;

		public MainForm()
		{
			InitializeComponent();
		}

		void Form1_Load(object sender, EventArgs e)
		{
			try 
			{
				a: recentForm = new RecentAndDirectives();
				recentForm.ShowDialog();
				if (recentForm.DialogResult == DialogResult.OK)
				{
					if (!string.IsNullOrWhiteSpace(recentForm.PathOfDB))
					{
						Stopwatch perfWatch = new Stopwatch();
						perfWatch.Start();
						toolStripProgressBar1.Visible = true;
						Task taskInThread = Task.Factory.StartNew( () => 
						{
							DB = new Database(recentForm.PathOfDB);							
						});
						while (!taskInThread.IsCompleted)
						{
							toolStripStatusLabel2.Text = "Производится загрузка данных...";
							Application.DoEvents();
						}
						toolStripStatusLabel2.Text = string.Empty;
						GetTablesList(DB);
						MaindataGrid.DataSource = DB.currentTable = DB.listOfTables.Tables[0];
						typesColumnList = GetTypesOfDataTableColumns();
						CheckViewOfGrid();
						FormatDataGrid();
						uniquesForm = new Uniques();
						filtersForm = new ValueFilters();
						replacerForm = new Replacer();
						toolStripProgressBar1.Visible = false;
						TimeSpan span = perfWatch.Elapsed;
						Text = string.Format("Статико-аналитический терминал \"Стазис\" - <<{0}>> - тип БД: {1}", Path.GetFileNameWithoutExtension(DB.pathOfDatabase), DB.GetTypeOfDBFile());
						toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
					}
					else
					{
						toolStripProgressBar1.Visible = false;
						recentForm.Dispose();
						goto a;
					}
				}
				MaindataGrid.CurrentCell = null;
			} 
			catch (Exception exc) 
			{
				toolStripProgressBar1.Visible = false;
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}
		
		List<Type> GetTypesOfDataTableColumns()
		{
			List<Type> tmpList = new List<Type>();
			foreach (DataColumn dc in DB.listOfTables.Tables[tabControl1.SelectedIndex].Columns)
				tmpList.Add(dc.DataType);
			return tmpList;
		}
		
		void ConvertGridColumns(List<Type> listOfTypes)
		{
			for (int i = 0; i < listOfTypes.Count; i++)
			{
				DataColumn col = DB.listOfTables.Tables[tabControl1.SelectedIndex].Columns[i];
				if (col.DataType != listOfTypes[i])
					DataOperations.ChangeColumnType(MaindataGrid, DB.listOfTables.Tables[tabControl1.SelectedIndex], col.Ordinal, listOfTypes[i]);
			}
		}

		void CheckViewOfGrid()
		{
			string[,] tmpValArray = SettingsTools.Operations.GetValuesOfKey("Software\\Convex\\Stazis\\ViewOfGrid", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
			if (tmpValArray == null) return;
			for (int i = 0; i < tmpValArray.GetLength(1); i++)
			{
				автоподборВысотыСтрокToolStripMenuItem.Checked |= (tmpValArray[0, i] == "AutoResizeRowHeight") && (tmpValArray[1, i] == "Yes");
				автоподборВысотыЗаголовковToolStripMenuItem.Checked |= (tmpValArray[0, i] == "AutoResizeColumnHeaderHeight") && (tmpValArray[1, i] == "Yes");
				автоподборШириныЗаголовковToolStripMenuItem.Checked |= (tmpValArray[0, i] == "AutoResizeColumnHeaderWidth") && (tmpValArray[1, i] == "Yes");
				автоподборШириныСтолбцовToolStripMenuItem.Checked |= (tmpValArray[0, i] == "AutoResizeColumnWidth") && (tmpValArray[1, i] == "Yes");
				сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked |= (tmpValArray[0, i] == "SaveSearchResult") && (tmpValArray[1, i] == "Yes");
			}
			if (сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked) 
				{
					saveSearchResult = SaveSearchResult.Yes;
					backUpSet = new List<DataTable>(tabControl1.TabPages.Count);
				    for (int i = 0; i < tabControl1.TabPages.Count; i++) 
				    {
				    	try 
				    	{
							if (backUpSet[i].Rows.Count != 0)
								backUpSet[i] = MaindataGrid.DataSource as DataTable;
				    	} 
				    	catch (Exception) 
				    	{
				    		backUpSet.Insert(i, new DataTable());
				    	}
				    }
				}
		}

		void FormatDataGrid()
		{
			if (автоподборВысотыСтрокToolStripMenuItem.Checked) MaindataGrid.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells);
			if (автоподборВысотыЗаголовковToolStripMenuItem.Checked) MaindataGrid.AutoResizeColumnHeadersHeight();
			if (автоподборШириныСтолбцовToolStripMenuItem.Checked) MaindataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
			if (автоподборШириныЗаголовковToolStripMenuItem.Checked) MaindataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader); 
		}

		void Reload()
		{
			Stopwatch perfWatch = new Stopwatch();
			perfWatch.Start();
			toolStripProgressBar1.Visible = true;
			string path = DB.pathOfDatabase;
			DB = null;
			Task taskInThread = Task.Factory.StartNew( () => 
			{
				DB = new Database(recentForm.PathOfDB);							
			});
			while (!taskInThread.IsCompleted)
			{
				toolStripStatusLabel2.Text = "Производится загрузка данных...";
				Application.DoEvents();
			}
			toolStripStatusLabel2.Text = string.Empty;
			GetTablesList(DB);
			MaindataGrid.DataSource = DB.currentTable = DB.listOfTables.Tables[0];
			CheckViewOfGrid();
			FormatDataGrid();
			toolStripProgressBar1.Visible = false;
			TimeSpan span = perfWatch.Elapsed;
			toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
		}

		void UnformatDataGrid()
		{
			MaindataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
			MaindataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			MaindataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			MaindataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
		}

		void GetTablesList(Database dB)
		{
			tabControl1.TabPages.Clear();
			foreach (string dTableName in dB.namesOfTables)
				tabControl1.TabPages.Add(dTableName);
		}

		void MaindataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try 
			{
				if ((e.Button == MouseButtons.Right) && (e.ColumnIndex != -1 && e.RowIndex != -1))
				{
					MaindataGrid.CurrentCell = MaindataGrid[e.ColumnIndex, e.RowIndex];
					contextMenuStrip1.Show(Cursor.Position);
					switch (Type.GetTypeCode(MaindataGrid.Columns[e.ColumnIndex].ValueType))
					{
						
						case TypeCode.Int32:
						case TypeCode.Double:
						case TypeCode.Decimal:
							числоToolStripMenuItem.Checked = true;
							датаToolStripMenuItem.Checked = false;
							текстпоУмолчаниюToolStripMenuItem.Checked = false;
							break;
						case TypeCode.DateTime:
							датаToolStripMenuItem.Checked = true;
							числоToolStripMenuItem.Checked = false;
							текстпоУмолчаниюToolStripMenuItem.Checked = false;
							break;
						case TypeCode.Object:
							датаToolStripMenuItem.Checked = false;
							числоToolStripMenuItem.Checked = false;
							текстпоУмолчаниюToolStripMenuItem.Checked = false;
							break;
						default:
							текстпоУмолчаниюToolStripMenuItem.Checked = true;
							числоToolStripMenuItem.Checked = false;
							датаToolStripMenuItem.Checked = false;
							break;
					}
					MaindataGrid.Columns[e.ColumnIndex].Selected = true;
					if (MaindataGrid.RowCount != DB.listOfTables.Tables[tabControl1.SelectedIndex].Rows.Count)
						пакетныйЗаменительToolStripMenuItem.Enabled = false;
					else 
						пакетныйЗаменительToolStripMenuItem.Enabled = true;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void MaindataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try 
			{
				if ((e.RowIndex != -1) && (e.ColumnIndex != -1))
				{
					Stopwatch perfWatch = new Stopwatch();
					DateTime tmpDate;
					perfWatch.Start();
					toolStripProgressBar1.Visible = true;
					int columnIndex = MaindataGrid.CurrentCell.ColumnIndex;;
					string CellValue = MaindataGrid.CurrentCell.Value.ToString();
					DataTable tmpDataTable = MaindataGrid.DataSource as DataTable;
					DataTable newTable = tmpDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
					    newTable = DateTime.TryParse(CellValue, out tmpDate) ? DataOperations.QueryProcess(tmpDataTable, columnIndex, tmpDate, tmpDate) : DataOperations.QueryProcess(tmpDataTable, columnIndex, CellValue);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					MaindataGrid.DataSource = newTable;
					if (saveSearchResult == SaveSearchResult.Yes) backUpSet[tabControl1.SelectedIndex] = MaindataGrid.DataSource as DataTable;
					FormatDataGrid();
					TimeSpan span = perfWatch.Elapsed;
					toolStripProgressBar1.Visible = false;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text = string.Format("Результаты выборки по запросу '{4}' в столбце <{5}> : {3} (Время загрузки: {0} мин {1} сек {2} мсек)", 
					                                           span.Minutes, 
					                                           span.Seconds, 
					                                           span.Milliseconds, 
					                                           MaindataGrid.Rows.Count, 
					                                           (DateTime.TryParse(CellValue, out tmpDate)) ? tmpDate.ToShortDateString() : CellValue,
					                                           MaindataGrid.Columns[columnIndex].HeaderText);
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc)
			{
				toolStripProgressBar1.Visible = false;
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void MaindataGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			try 
			{
				if (датаToolStripMenuItem.Checked) {
					DateTime FirstDate;
					DateTime SecondDate;
					DateTime.TryParse(e.CellValue1.ToString(), out FirstDate);
					DateTime.TryParse(e.CellValue2.ToString(), out SecondDate);
					e.SortResult = DateTime.Compare(FirstDate, SecondDate);
				}
				if (числоToolStripMenuItem.Checked) {
					int FirstNumber;
					int SecondNumber;
					int.TryParse(e.CellValue1.ToString(), out FirstNumber);
					int.TryParse(e.CellValue2.ToString(), out SecondNumber);
					e.SortResult = String.Compare(FirstNumber.ToString("00000000000"), SecondNumber.ToString("00000000000"), StringComparison.Ordinal);
				}
				if (текстпоУмолчаниюToolStripMenuItem.Checked)
					e.SortResult = String.Compare(e.CellValue1.ToString(), e.CellValue2.ToString(), StringComparison.Ordinal);
				e.Handled = true;
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex == -1) return;
			int SheetIndex = tabControl1.SelectedIndex;
			try
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				MaindataGrid.DataSource = saveSearchResult == SaveSearchResult.Yes ? backUpSet[SheetIndex].Rows.Count > 0 ? backUpSet[SheetIndex] : DB.listOfTables.Tables[SheetIndex] : DB.listOfTables.Tables[SheetIndex];
				FormatDataGrid();
				TimeSpan span = perfWatch.Elapsed;
				MaindataGrid.DataSource = DB.currentTable = DB.listOfTables.Tables[tabControl1.SelectedIndex];;
				toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
				MaindataGrid.CurrentCell = null;
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void вернутьсяКНачальномуСостояниюToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				Reload();
//				for (int i = 0, maxColumnsCount = DB.listOfTables.Tables[tabControl1.SelectedIndex].Columns.Count; i < maxColumnsCount; i++)
//				{
//					DataColumn col = DB.listOfTables.Tables[tabControl1.SelectedIndex].Columns[i];
//					if (col.DataType != typeof(string))
//						ChangeColumnType(MaindataGrid, DB.listOfTables.Tables[tabControl1.SelectedIndex], col.Ordinal, typeof(string));
//				}
			} 
			catch (Exception exc) 
			{
				toolStripProgressBar1.Visible = false;
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void датаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				датаToolStripMenuItem.Checked = true;
				числоToolStripMenuItem.Checked = false;
				текстпоУмолчаниюToolStripMenuItem.Checked = false;
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				DataTable tmpDataTable = (DataTable)MaindataGrid.DataSource;
				DataOperations.ChangeColumnType(MaindataGrid,tmpDataTable, MaindataGrid.CurrentCell.ColumnIndex, typeof(DateTime));
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
				contextMenuStrip1.Hide();
				FormatDataGrid();
			} 
			catch (Exception exc) 
			{
				contextMenuStrip1.Hide();
				MessageBox.Show("Невозможно выполнить преобразование типа столбца");
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void задатьОграниченияToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				числоToolStripMenuItem_Click(this, e);
				filtersForm.FilterMode = DataOperations.FilterMode.Integer;
				string columnName = filtersForm.ColName = MaindataGrid.Columns[MaindataGrid.CurrentCell.ColumnIndex].HeaderText;
				filtersForm.AppDir = AppDir;
				List<double> list = new List<double>();
				var query = from order in DB.listOfTables.Tables[tabControl1.SelectedIndex].AsEnumerable()
					select order.Field<double>(columnName);
				list.AddRange(query);
				filtersForm.minDoubleRange = list.Min();
				filtersForm.maxDoubleRange = list.Max();
				filtersForm.ShowDialog();
				if (filtersForm.DialogResult == DialogResult.Yes)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					double SearchInteger = filtersForm.intValue;
					DataOperations.SearchIntMode Mode = filtersForm.intMode;
					int ColumnIndex = MaindataGrid.CurrentCell.ColumnIndex;
					DataTable tmpDataTable = (DataTable)MaindataGrid.DataSource;
					DataTable newTable = new DataTable();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(tmpDataTable, ColumnIndex, SearchInteger, Mode);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					MaindataGrid.DataSource = newTable;
					if (saveSearchResult == SaveSearchResult.Yes) backUpSet[tabControl1.SelectedIndex] = MaindataGrid.DataSource as DataTable;
					FormatDataGrid();
					string SearchOption = string.Empty;
					switch (Mode)
					{
						case DataOperations.SearchIntMode.EqualTo:
							SearchOption = "равных";
							break;
	
						case DataOperations.SearchIntMode.LargerThen:
							SearchOption = "больших";
							break;
	
						case DataOperations.SearchIntMode.SmallerThen:
							SearchOption = "меньших";
							break;
					}
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text = string.Format("Результаты выборки чисел {6} '{4}' в столбце <{5}> : {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count, SearchInteger, columnName, SearchOption);
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void ограничитьДиапазонToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				датаToolStripMenuItem_Click(this, e);
				filtersForm.FilterMode = DataOperations.FilterMode.Date;
				filtersForm.ColName = MaindataGrid.Columns[MaindataGrid.CurrentCell.ColumnIndex].Name;
				filtersForm.AppDir = AppDir;
				List<DateTime> list = new List<DateTime>();
				var query = from order in DB.listOfTables.Tables[tabControl1.SelectedIndex].AsEnumerable()
					select order.Field<DateTime>(filtersForm.ColName);
				list.AddRange(query);
				filtersForm.minDateRange = list.Min();
				filtersForm.maxDateRange = list.Max();
				filtersForm.ShowDialog();
				if (filtersForm.DialogResult == DialogResult.Yes)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					DateTime Start = filtersForm.StartDate;
					DateTime End = filtersForm.EndDate;
					int ColumnIndex = MaindataGrid.CurrentCell.ColumnIndex;
					DataTable tmpDataTable = (DataTable)MaindataGrid.DataSource;
					DataTable newTable = new DataTable();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(tmpDataTable, ColumnIndex, Start, End);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					MaindataGrid.DataSource = newTable;
					if (saveSearchResult == SaveSearchResult.Yes) backUpSet[tabControl1.SelectedIndex] = MaindataGrid.DataSource as DataTable;
					FormatDataGrid();
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text = string.Format("Результаты выборки по диапазону дат (с {0} по {1}) : {2} (Время загрузки: {3} мин {4} сек {5} мсек)", Start, End, MaindataGrid.Rows.Count, span.Minutes, span.Seconds, span.Milliseconds);
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void пакетныйЗаменительToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<string> list = new List<string>();
			try
			{
				for (int i = 0; i < DB.listOfTables.Tables[tabControl1.SelectedIndex].Rows.Count; i++)
				{
					string item = DB.listOfTables.Tables[tabControl1.SelectedIndex].Rows[i][MaindataGrid.CurrentCell.ColumnIndex].ToString();
					if (!string.IsNullOrWhiteSpace(item)) list.Add(item);
					else list.Add("<пустое значение>");
				}
				replacerForm.checkedListBox1.Items.Clear();
				replacerForm.checkedListBox1.Items.AddRange(list.AsParallel().Distinct().OrderBy(x => x).ToArray());
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
				return;
			}
			replacerForm.Col = MaindataGrid.CurrentCell.ColumnIndex;
			replacerForm.ColName = MaindataGrid.Columns[MaindataGrid.CurrentCell.ColumnIndex].Name;
			if (DB.TypeOfDB != Database.DBmode.SQLite && DB.TypeOfDB != Database.DBmode.CSV)
				replacerForm.checkBox1.Enabled = true;
			else replacerForm.checkBox1.Enabled = false;
			replacerForm.db = DB;
			replacerForm.AppDir = AppDir;
			replacerForm.ShowDialog();
		}

		void поискToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				текстпоУмолчаниюToolStripMenuItem_Click(this, e);
				filtersForm.FilterMode = DataOperations.FilterMode.Text;
				string columnName = filtersForm.ColName = MaindataGrid.Columns[MaindataGrid.CurrentCell.ColumnIndex].HeaderText;
				filtersForm.AppDir = AppDir;
				filtersForm.ShowDialog();
				if (filtersForm.DialogResult == DialogResult.Yes)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					string SearchText = filtersForm.textValue;
					DataOperations.SearchTextMode Mode = filtersForm.txtMode;
					int ColumnIndex = MaindataGrid.CurrentCell.ColumnIndex;
					DataTable tmpDataTable = (MaindataGrid.DataSource as DataTable).Copy();
					DataTable newTable = new DataTable();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(tmpDataTable, ColumnIndex, SearchText, Mode);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					MaindataGrid.DataSource = newTable;
					if (saveSearchResult == SaveSearchResult.Yes) backUpSet[tabControl1.SelectedIndex] = MaindataGrid.DataSource as DataTable;
					FormatDataGrid();
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text = string.Format("Количество записей удовлетворяющих поисковому запросу '{0}' в столбце <{1}> : {2} (Время загрузки: {3} мин {4} сек {5} мсек)", SearchText, columnName, MaindataGrid.Rows.Count, span.Minutes, span.Seconds, span.Milliseconds);
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void сброситьРезультатыТекущегоПоискаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				MaindataGrid.DataSource = DB.currentTable = DB.listOfTables.Tables[tabControl1.SelectedIndex];
				ConvertGridColumns(typesColumnList);
				FormatDataGrid();
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
				MaindataGrid.CurrentCell = null;
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void сменитьИсточникДанныхToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				a: recentForm = new RecentAndDirectives();
				recentForm.ShowDialog();
				if (recentForm.DialogResult == DialogResult.OK)
				{
					if (!string.IsNullOrWhiteSpace(recentForm.PathOfDB))
					{
						Stopwatch perfWatch = new Stopwatch();
						perfWatch.Start();
						toolStripProgressBar1.Visible = true;
						Task taskInThread = Task.Factory.StartNew( () => 
						{
							DB = new Database(recentForm.PathOfDB);							
						});
						while (!taskInThread.IsCompleted)
						{
							toolStripStatusLabel2.Text = "Производится загрузка данных...";
							Application.DoEvents();
						}
						toolStripStatusLabel2.Text = string.Empty;
						GetTablesList(DB);
						MaindataGrid.DataSource = DB.currentTable = DB.listOfTables.Tables[0];
						CheckViewOfGrid();
						FormatDataGrid();
						toolStripProgressBar1.Visible = false;
						TimeSpan span = perfWatch.Elapsed;
						Text = string.Format("Статико-аналитический терминал \"Стазис\" - <<{0}>> - тип БД: {1}", Path.GetFileNameWithoutExtension(DB.pathOfDatabase), DB.GetTypeOfDBFile());
						toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
					}
					else
					{
						toolStripProgressBar1.Visible = false;
						recentForm.Dispose();
						goto a;
					}
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void списокУникальныхЗначенийToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				toolStripProgressBar1.Visible = true;
				int col = MaindataGrid.CurrentCell.ColumnIndex;
				string columnName = MaindataGrid.Columns[col].HeaderText;
				uniquesForm.colName = columnName;
				DataTable tmpDataTable = (MaindataGrid.DataSource  as DataTable).Copy();
				DataOperations.ChangeColumnType(MaindataGrid, tmpDataTable, col, typeof(string));
				DataOperations.LoadUniques(tmpDataTable, col, uniquesForm.dataGridView1);
				toolStripProgressBar1.Visible = false;
				uniquesForm.ShowDialog(this);
				if (uniquesForm.DialogResult == DialogResult.Yes)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					string CellValue = uniquesForm.QueryText;
					DataTable newTable = new DataTable();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(tmpDataTable, col, CellValue);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					MaindataGrid.DataSource = newTable;
					if (saveSearchResult == SaveSearchResult.Yes) backUpSet[tabControl1.SelectedIndex] = MaindataGrid.DataSource as DataTable;
					FormatDataGrid();
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text = string.Format("Результаты выборки по запросу '{0}' в столбце <{1}> : {2} (Время загрузки: {3} мин {4} сек {5} мсек)", CellValue, columnName, MaindataGrid.Rows.Count, span.Minutes, span.Seconds, span.Milliseconds);
					MaindataGrid.CurrentCell = null;
				}
			}
			catch (Exception exc)
			{
				toolStripProgressBar1.Visible = false;
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void текстпоУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				текстпоУмолчаниюToolStripMenuItem.Checked = true;
				числоToolStripMenuItem.Checked = false;
				датаToolStripMenuItem.Checked = false;
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				DataTable tmpDataTable = MaindataGrid.DataSource as DataTable;
				DataOperations.ChangeColumnType(MaindataGrid, tmpDataTable, MaindataGrid.CurrentCell.ColumnIndex, typeof(string));
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
				contextMenuStrip1.Hide();
				FormatDataGrid();
			} 
			catch (Exception exc) 
			{
				contextMenuStrip1.Hide();
				MessageBox.Show("Невозможно выполнить преобразование типа столбца");
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void числоToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				числоToolStripMenuItem.Checked = true;
				датаToolStripMenuItem.Checked = false;
				текстпоУмолчаниюToolStripMenuItem.Checked = false;
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				DataTable tmpDataTable = (DataTable)MaindataGrid.DataSource;
				DataOperations.ChangeColumnType(MaindataGrid, tmpDataTable, MaindataGrid.CurrentCell.ColumnIndex, typeof(double));
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
				contextMenuStrip1.Hide();
				FormatDataGrid();
			} 
			catch (Exception exc) 
			{
				contextMenuStrip1.Hide();
				MessageBox.Show("Невозможно выполнить преобразование типа столбца");
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void автоподборВысотыСтрокToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				if (автоподборВысотыСтрокToolStripMenuItem.Checked)
				{
					MaindataGrid.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells);
					SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeRowHeight", "Yes", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				}
				else SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeRowHeight", "No", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void автоподборШириныСтолбцовToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				if (автоподборШириныСтолбцовToolStripMenuItem.Checked)
				{ 
					MaindataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
					SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeColumnWidth", "Yes", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				}
				else SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeColumnWidth", "No", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void автоподборВысотыЗаголовковToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				if (автоподборВысотыЗаголовковToolStripMenuItem.Checked)
				{ 
					MaindataGrid.AutoResizeColumnHeadersHeight();
					SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeColumnHeaderHeight", "Yes", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				}
				else SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeColumnHeaderHeight", "No", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void автоподборШириныЗаголовковToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				if (автоподборШириныЗаголовковToolStripMenuItem.Checked)
				{ 
					MaindataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
					SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeColumnHeaderWidth", "Yes", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				}
				else SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "AutoResizeColumnHeaderWidth", "No", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}

		void экспортВExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				saveFileDialog1.Filter = DataOperations.GetExportFileTypes();
				saveFileDialog1.FilterIndex = 0;
				if (saveFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					Task task1 = Task.Factory.StartNew( () => 
                   	{
                       	switch (Path.GetExtension(saveFileDialog1.FileName))
                       	{
							case ".xls":
                       		case ".xlsx":
				                DataOperations.ExportToExcel(MaindataGrid.DataSource as DataTable, saveFileDialog1.FileName, (saveFileDialog1.FilterIndex == 1) ? ".xls" : ".xlsx");                   			
								break;
							case ".csv":
								DataOperations.ExportToCSV(MaindataGrid.DataSource as DataTable, saveFileDialog1.FileName);
							break;
				        }
							
			        });
					while (!task1.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится экспорт данных...";
						Application.DoEvents();
					}
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Format("(Кликните, чтобы скрыть) Экспорт данных в файл {0} завершен. (Время выгрузки: {1} мин {2} сек {3} мсек)", saveFileDialog1.FileName, span.Minutes, span.Seconds, span.Milliseconds);
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}
		void сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				if (!сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked)
				{
					saveSearchResult = SaveSearchResult.Yes;
					сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked = true;
					backUpSet = new List<DataTable>(tabControl1.TabPages.Count);
					for (int i = 0; i < tabControl1.TabPages.Count; i++)
					{
						try
						{
							if (backUpSet[i].Rows.Count != 0)
								backUpSet[i] = MaindataGrid.DataSource as DataTable;
						}
						catch
						{
							if (i == tabControl1.SelectedIndex)
								backUpSet.Insert(i, MaindataGrid.DataSource as DataTable);
							else
								backUpSet.Insert(i, new DataTable());
						}
					}
					SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "SaveSearchResult", "Yes", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				}
				else
				{
					saveSearchResult = SaveSearchResult.No;
					backUpSet = new List<DataTable>(tabControl1.TabPages.Count);
					//MaindataGrid.DataSource = DB.listOfTables.Tables[tabControl1.SelectedIndex];
					сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked = false;
					SettingsTools.Operations.SaveToRegistry("Software\\Convex\\Stazis\\ViewOfGrid", "SaveSearchResult", "No", SettingsTools.Operations.HiveKey.HKEY_CURRENT_USER);
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
		}
		
		void toolStripStatusLabel2_Click(object sender, EventArgs e)
		{
			toolStripStatusLabel2.Text = string.Empty;
		}
		
		void toolStripStatusLabel2_MouseHover(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.Hand;
		}
		void многоуровневыйАнализToolStripMenuItem_Click(object sender, EventArgs e)
		{
	
		}
		void скрытьСтолбецToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Stopwatch perfWatch = new Stopwatch();
			perfWatch.Start();
			int columnIndex = MaindataGrid.CurrentCell.ColumnIndex;;
			DataTable tmpDataTable = (MaindataGrid.DataSource as DataTable).Copy();
			tmpDataTable.Columns.RemoveAt(columnIndex);
			MaindataGrid.DataSource = tmpDataTable;
			if (saveSearchResult == SaveSearchResult.Yes) backUpSet[tabControl1.SelectedIndex] = MaindataGrid.DataSource as DataTable;
			TimeSpan span = perfWatch.Elapsed;
			toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
			MaindataGrid.CurrentCell = null;
		}
		
		void добавитьЗаписьВИсточникToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddRecord addRecForm = new AddRecord();
			addRecForm.DB = DB;
			addRecForm.ShowDialog();
		}
	}
}