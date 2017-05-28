using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Stazis.Properties;

//TODO диаграммы
//TODO многоуровневая сортировка
//TODO мгогоуровневая фильтрация
//TODO система макросов

namespace Stazis
{
	public partial class MainForm : Form
	{
		public Database DB { get; set; }
		public DataBaseAbstract db { get; set; }
		
		public string AppDir = Application.StartupPath;
		public ValueFilters filtersForm;
		public RecentAndDirectives recentForm;
		public Replacer replacerForm;
		public Uniques uniquesForm;
		string AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

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
						Task databaseLoad = Task.Factory.StartNew( () => 
						{
							switch (Path.GetExtension(recentForm.PathOfDB))
							{
								case ".xls":
								case ".xlsx":
									db = new ExcelDatabase(recentForm.PathOfDB);
									break;
								case ".csv":
									db = new CSVDatabase(recentForm.PathOfDB);
									break;
								case ".db":
								case ".cdb":
								case ".sqlite3":
									db = new SQLiteDatabase(recentForm.PathOfDB);
									break;
							}
						});
						while (!databaseLoad.IsCompleted)
						{
							toolStripStatusLabel2.Text = "Производится загрузка данных...";
							Application.DoEvents();
						}
						toolStripStatusLabel2.Text = string.Empty;
						GetTablesList(db);
						MaindataGrid.DataSource = db.CurrentDataTable;
						//typesColumnList = GetTypesOfDataTableColumns();
						CheckViewOfGrid();
						FormatDataGrid();
						uniquesForm = new Uniques();
						filtersForm = new ValueFilters();
						replacerForm = new Replacer(this);
						toolStripProgressBar1.Visible = false;
						TimeSpan span = perfWatch.Elapsed;
						Text = string.Format("Статико-аналитический терминал \"Стазис\" - <<{0}>> - тип БД: {1}", Path.GetFileNameWithoutExtension(db.DatabasePath), db.GetNameOfType());
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
			foreach (DataColumn dc in db.CurrentDataTable.Columns)
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
				автоподборВысотыСтрокToolStripMenuItem.Checked = AppSettings.Default.AutoResizeRowHeight;
				автоподборВысотыЗаголовковToolStripMenuItem.Checked = AppSettings.Default.AutoResizeColumnHeaderHeight;
				автоподборШириныЗаголовковToolStripMenuItem.Checked = AppSettings.Default.AutoResizeColumnHeaderWidth;
				автоподборШириныСтолбцовToolStripMenuItem.Checked = AppSettings.Default.AutoResizeColumnWidth;
				сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked = AppSettings.Default.SaveSearchResult;
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
			
		}

		void UnformatDataGrid()
		{
			MaindataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
			MaindataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			MaindataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			MaindataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
		}

		void GetTablesList(DataBaseAbstract dB)
		{
			tabControl1.TabPages.Clear();
			foreach (string dTableName in dB.NamesOfTables)
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
					if (db is IChangebleDatabase)
						пакетныйЗаменительToolStripMenuItem.Enabled = true;
					else 
						пакетныйЗаменительToolStripMenuItem.Enabled = false;
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
					DataTable newTable = db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
					    newTable = DateTime.TryParse(CellValue, out tmpDate) ? DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, columnIndex, tmpDate, tmpDate) : DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, columnIndex, CellValue);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						db.CurrentDataTable.Clear();
						db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = db.CurrentDataTable;
					}
					else MaindataGrid.DataSource = newTable;
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
					DateTime.TryParse(e.CellValue1.ToString(), out DateTime FirstDate);
					DateTime.TryParse(e.CellValue2.ToString(), out DateTime SecondDate);
					e.SortResult = DateTime.Compare(FirstDate, SecondDate);
				}
				if (числоToolStripMenuItem.Checked) {
					int.TryParse(e.CellValue1.ToString(), out int FirstNumber);
					int.TryParse(e.CellValue2.ToString(), out int SecondNumber);
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
				db.SelectedTableIndex = SheetIndex;
				MaindataGrid.DataSource = db.CurrentDataTable;
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

		void вернутьсяКНачальномуСостояниюToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				toolStripProgressBar1.Visible = true;
				Task taskInThread = Task.Factory.StartNew(() =>
				{
					db.Reload();
				});
				while (!taskInThread.IsCompleted)
				{
					toolStripStatusLabel2.Text = "Производится загрузка данных...";
					Application.DoEvents();
				}
				toolStripStatusLabel2.Text = string.Empty;
				GetTablesList(db);
				MaindataGrid.DataSource = db.CurrentDataTable;
				CheckViewOfGrid();
				FormatDataGrid();
				toolStripProgressBar1.Visible = false;
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
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
				DataOperations.ChangeColumnType(MaindataGrid,db.CurrentDataTable, MaindataGrid.CurrentCell.ColumnIndex, typeof(DateTime));
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
				var query = from order in db.CurrentDataTable.AsEnumerable()
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
					DataTable newTable = db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, ColumnIndex, SearchInteger, Mode);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						db.CurrentDataTable.Clear();
						db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = db.CurrentDataTable;
					}
					else MaindataGrid.DataSource = newTable;
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
				var query = from order in db.CurrentDataTable.AsEnumerable()
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
					DataTable newTable = db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, ColumnIndex, Start, End);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						db.CurrentDataTable.Clear();
						db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = db.CurrentDataTable;
					}
					else MaindataGrid.DataSource = newTable;
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
			//List<string> list = new List<string>();
			int colIndex = MaindataGrid.CurrentCell.ColumnIndex;
			try
			{
				var lst = (db.CurrentDataTable.AsEnumerable().Select(x =>
				{
					if (!string.IsNullOrWhiteSpace(x[colIndex].ToString())) return x[colIndex].ToString();
					else return "<пустое значение>";
				})).Distinct().OrderBy(x => x).ToArray();
				//for (int i = 0; i < db.CurrentDataTable.Rows.Count; i++)
				//{
				//	string item = db.CurrentDataTable.Rows[i][MaindataGrid.CurrentCell.ColumnIndex].ToString();
				//	if (!string.IsNullOrWhiteSpace(item)) list.Add(item);
				//	else list.Add("<пустое значение>");
				//}
				replacerForm.checkedListBox1.Items.Clear();
				replacerForm.checkedListBox1.Items.AddRange(lst);
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
				return;
			}
			replacerForm.Col = colIndex;
			replacerForm.ColName = MaindataGrid.Columns[colIndex].Name;
			replacerForm.checkBox1.Enabled = (db is IChangebleDatabase);
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
						GetTablesList(db);
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
				int col = MaindataGrid.CurrentCell.ColumnIndex;
				string columnName = MaindataGrid.Columns[col].HeaderText;
				uniquesForm.colName = columnName;
				Stopwatch perfUniq = new Stopwatch();
				perfUniq.Start();
				Task loadUniquesTask = Task.Factory.StartNew(() =>
				{
					DataOperations.LoadUniques(MaindataGrid.DataSource as DataTable, col, uniquesForm.dataGridView1);
				});
				while (!loadUniquesTask.IsCompleted)
				{
					toolStripProgressBar1.Visible = true;
					toolStripStatusLabel2.Text = "Производится вычисление уникальных значений столбца " + columnName + "..." ;
					Application.DoEvents();
				}
				toolStripProgressBar1.Visible = false;
				TimeSpan spanUniq = perfUniq.Elapsed;
				toolStripStatusLabel2.Text = string.Format("Время вычисления составило: {0} мин {1} сек {2} мсек",  spanUniq.Minutes, spanUniq.Seconds, spanUniq.Milliseconds);
				uniquesForm.ShowDialog(this);
				if (uniquesForm.DialogResult == DialogResult.Yes)
				{
					toolStripStatusLabel2.Text = string.Empty;
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					string CellValue = uniquesForm.QueryText;
					DataTable newTable = db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, col, CellValue);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						db.CurrentDataTable.Clear();
						db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = db.CurrentDataTable;
					}
					else MaindataGrid.DataSource = newTable;
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
				if (автоподборВысотыСтрокToolStripMenuItem.Checked)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					MaindataGrid.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells);
					AppSettings.Default.AutoResizeRowHeight = true;
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек", span.Minutes, span.Seconds, span.Milliseconds);
				}
				else AppSettings.Default.AutoResizeRowHeight = false;
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
				if (автоподборШириныСтолбцовToolStripMenuItem.Checked)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					MaindataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
					AppSettings.Default.AutoResizeColumnWidth = true;
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек", span.Minutes, span.Seconds, span.Milliseconds);
				}
				else AppSettings.Default.AutoResizeColumnWidth = false;
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
				if (автоподборВысотыЗаголовковToolStripMenuItem.Checked)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					MaindataGrid.AutoResizeColumnHeadersHeight();
					AppSettings.Default.AutoResizeColumnHeaderHeight = true;
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек", span.Minutes, span.Seconds, span.Milliseconds);
				}
				else AppSettings.Default.AutoResizeColumnHeaderHeight = false;
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
				if (автоподборШириныЗаголовковToolStripMenuItem.Checked)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					MaindataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
					AppSettings.Default.AutoResizeColumnHeaderWidth = true;
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек", span.Minutes, span.Seconds, span.Milliseconds);
				}
				else AppSettings.Default.AutoResizeColumnHeaderWidth = false;
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
					сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked = true;
					AppSettings.Default.SaveSearchResult = true;
				}
				else
				{
					AppSettings.Default.SaveSearchResult = false;
					сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Checked = false;
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
			TimeSpan span = perfWatch.Elapsed;
			toolStripStatusLabel2.Text = string.Format("Время последней операции: {0} мин {1} сек {2} мсек",span.Minutes, span.Seconds, span.Milliseconds);
			MaindataGrid.CurrentCell = null;
		}
		
		void добавитьЗаписьВИсточникToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddRecord addRecForm = new AddRecord() { DB = DB };
			addRecForm.ShowDialog();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			AppSettings.Default.Save();
		}
	}
}