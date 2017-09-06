using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using StazisExtensibilityInterface;
using DatabaseFactoryCore;
using DataOperationsModule;

//TODO диаграммы
//TODO многоуровневая сортировка
//TODO мгогоуровневая фильтрация
//TODO система макросов

namespace Stazis
{
	public partial class MainForm : Form
	{
        readonly DatabaseFactory _stazisDatabaseFactory = DatabaseFactory.GetFactory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"));
        public IExtensibility Db;
		
		string _appDir = Application.StartupPath;
		ValueFilters _filtersForm;
		RecentAndDirectives _recentForm;
		Replacer _replacerForm;
		Uniques _uniquesForm;
		string _appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

		public MainForm()
		{
			InitializeComponent();
		}

		void Form1_Load(object sender, EventArgs e)
		{
			try 
			{
//                AppSettings.Default.Upgrade();
			    AppSettings.Default.SupportedDatabaseTypes = string.Join("|", _stazisDatabaseFactory.GetSupportedFormats());
				_recentForm = new RecentAndDirectives();
				_recentForm.ShowDialog();
				if (_recentForm.DialogResult == DialogResult.OK)
				{
					if (!string.IsNullOrWhiteSpace(_recentForm.PathOfDb))
					{
						var perfWatch = new Stopwatch();
						perfWatch.Start();
                        Task databaseLoad = Task.Factory.StartNew(() =>
                        {
                            Db = _stazisDatabaseFactory.CreateDataBaseInstance(_recentForm.PathOfDb);
                        });
                        while (!databaseLoad.IsCompleted)
                        {
                            toolStripProgressBar1.Visible = true;
                            toolStripStatusLabel2.Text = "Производится загрузка данных...";
                            Application.DoEvents();
                        }
                        toolStripStatusLabel2.Text = string.Empty;
                        toolStripProgressBar1.Visible = false;
                        GetTablesList(Db);
                        Db.SelectedTableIndex = 0;
						MaindataGrid.DataSource = Db.CurrentDataTable;
						//typesColumnList = GetTypesOfDataTableColumns();
						CheckViewOfGrid();
						FormatDataGrid();
						_uniquesForm = new Uniques();
						_filtersForm = new ValueFilters();
						_replacerForm = new Replacer(this);
						TimeSpan span = perfWatch.Elapsed;
						Text =
							$"Статико-аналитический терминал \"Стазис\" - <<{Path.GetFileNameWithoutExtension(Db.DatabasePath)}>> - тип БД: {Db.GetTypeNameOfDatabaseFile()}";
						toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
					}
					else
					{
						_recentForm.Dispose();
					}
				}
				MaindataGrid.CurrentCell = null;
			} 
			catch (Exception exc) 
			{
				toolStripProgressBar1.Visible = false;
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

		List<Type> GetTypesOfDataTableColumns()
		{
			List<Type> tmpList = new List<Type>();
			foreach (DataColumn dc in Db.CurrentDataTable.Columns)
				tmpList.Add(dc.DataType);
			return tmpList;
		}
		
		//void ConvertGridColumns(List<Type> listOfTypes)
		//{
		//	for (int i = 0; i < listOfTypes.Count; i++)
		//	{
		//		DataColumn col = DB.listOfTables.Tables[tabControl1.SelectedIndex].Columns[i];
		//		if (col.DataType != listOfTypes[i])
		//			DataOperations.ChangeColumnType(MaindataGrid, DB.listOfTables.Tables[tabControl1.SelectedIndex], col.Ordinal, listOfTypes[i]);
		//	}
		//}

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

        void GetTablesList(IExtensibility dB)
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
					if (Db is IExtensibility)
						пакетныйЗаменительToolStripMenuItem.Enabled = true;
					else 
						пакетныйЗаменительToolStripMenuItem.Enabled = false;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
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
					string cellValue = MaindataGrid.CurrentCell.Value.ToString();
					DataTable newTable = Db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
					    newTable = DateTime.TryParse(cellValue, out tmpDate) ? DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, columnIndex, tmpDate, tmpDate) : DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, columnIndex, cellValue);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						Db.CurrentDataTable.Clear();
						Db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = Db.CurrentDataTable;
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
					                                           (DateTime.TryParse(cellValue, out tmpDate)) ? tmpDate.ToShortDateString() : cellValue,
					                                           MaindataGrid.Columns[columnIndex].HeaderText);
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc)
			{
				toolStripProgressBar1.Visible = false;
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

		void MaindataGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			try 
			{
				if (датаToolStripMenuItem.Checked) {
					DateTime.TryParse(e.CellValue1.ToString(), out DateTime firstDate);
					DateTime.TryParse(e.CellValue2.ToString(), out DateTime secondDate);
					e.SortResult = DateTime.Compare(firstDate, secondDate);
				}
				if (числоToolStripMenuItem.Checked) {
					int.TryParse(e.CellValue1.ToString(), out int firstNumber);
					int.TryParse(e.CellValue2.ToString(), out int secondNumber);
					e.SortResult = String.Compare(firstNumber.ToString("00000000000"), secondNumber.ToString("00000000000"), StringComparison.Ordinal);
				}
				if (текстпоУмолчаниюToolStripMenuItem.Checked)
					e.SortResult = String.Compare(e.CellValue1.ToString(), e.CellValue2.ToString(), StringComparison.Ordinal);
				e.Handled = true;
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

		void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex == -1) return;
			int sheetIndex = tabControl1.SelectedIndex;
			try
			{
				Stopwatch perfWatch = new Stopwatch();
				perfWatch.Start();
				Db.SelectedTableIndex = sheetIndex;
				MaindataGrid.DataSource = Db.CurrentDataTable;
				FormatDataGrid();
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
				MaindataGrid.CurrentCell = null;
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
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
					Db.Reload();
				});
				while (!taskInThread.IsCompleted)
				{
					toolStripStatusLabel2.Text = "Производится загрузка данных...";
					Application.DoEvents();
				}
				toolStripStatusLabel2.Text = string.Empty;
				GetTablesList(Db);
				MaindataGrid.DataSource = Db.CurrentDataTable;
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
				LogManager.Log.AddToLog(_appDir, exc);
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
				DataOperations.ChangeColumnType(MaindataGrid,Db.CurrentDataTable, MaindataGrid.CurrentCell.ColumnIndex, typeof(DateTime));
				TimeSpan span = perfWatch.Elapsed;
				toolStripStatusLabel2.Text =
					$"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
				contextMenuStrip1.Hide();
				FormatDataGrid();
			} 
			catch (Exception exc) 
			{
				contextMenuStrip1.Hide();
				MessageBox.Show("Невозможно выполнить преобразование типа столбца");
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

		void задатьОграниченияToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				числоToolStripMenuItem_Click(this, e);
				_filtersForm.FilterMode = DataOperations.FilterMode.Integer;
				string columnName = _filtersForm.ColName = MaindataGrid.Columns[MaindataGrid.CurrentCell.ColumnIndex].HeaderText;
				_filtersForm.AppDir = _appDir;
				List<double> list = new List<double>();
				var query = from order in Db.CurrentDataTable.AsEnumerable()
					select order.Field<double>(columnName);
				list.AddRange(query);
				_filtersForm.minDoubleRange = list.Min();
				_filtersForm.maxDoubleRange = list.Max();
				_filtersForm.ShowDialog();
				if (_filtersForm.DialogResult == DialogResult.Yes)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					double searchInteger = _filtersForm.intValue;
					DataOperations.SearchIntMode mode = _filtersForm.intMode;
					int columnIndex = MaindataGrid.CurrentCell.ColumnIndex;
					DataTable newTable = Db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, columnIndex, searchInteger, mode);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						Db.CurrentDataTable.Clear();
						Db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = Db.CurrentDataTable;
					}
					else MaindataGrid.DataSource = newTable;
					FormatDataGrid();
					string searchOption = string.Empty;
					switch (mode)
					{
						case DataOperations.SearchIntMode.EqualTo:
							searchOption = "равных";
							break;
						case DataOperations.SearchIntMode.LargerThen:
							searchOption = "больших";
							break;
						case DataOperations.SearchIntMode.SmallerThen:
							searchOption = "меньших";
							break;
					}
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text = string.Format("Результаты выборки чисел {6} '{4}' в столбце <{5}> : {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count, searchInteger, columnName, searchOption);
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

		void ограничитьДиапазонToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				датаToolStripMenuItem_Click(this, e);
				_filtersForm.FilterMode = DataOperations.FilterMode.Date;
				_filtersForm.ColName = MaindataGrid.Columns[MaindataGrid.CurrentCell.ColumnIndex].Name;
				_filtersForm.AppDir = _appDir;
				List<DateTime> list = new List<DateTime>();
				var query = from order in Db.CurrentDataTable.AsEnumerable()
					select order.Field<DateTime>(_filtersForm.ColName);
				list.AddRange(query);
				_filtersForm.minDateRange = list.Min();
				_filtersForm.maxDateRange = list.Max();
				_filtersForm.ShowDialog();
				if (_filtersForm.DialogResult == DialogResult.Yes)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					DateTime start = _filtersForm.StartDate;
					DateTime end = _filtersForm.EndDate;
					int columnIndex = MaindataGrid.CurrentCell.ColumnIndex;
					DataTable newTable = Db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, columnIndex, start, end);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						Db.CurrentDataTable.Clear();
						Db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = Db.CurrentDataTable;
					}
					else MaindataGrid.DataSource = newTable;
					FormatDataGrid();
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text =
						$"Результаты выборки по диапазону дат (с {start} по {end}) : {MaindataGrid.Rows.Count} (Время загрузки: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек)";
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

		void пакетныйЗаменительToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//List<string> list = new List<string>();
			int colIndex = MaindataGrid.CurrentCell.ColumnIndex;
			try
			{
				var lst = (Db.CurrentDataTable.AsEnumerable().Select(x =>
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
				_replacerForm.checkedListBox1.Items.Clear();
				_replacerForm.checkedListBox1.Items.AddRange(lst);
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
				return;
			}
			_replacerForm.Col = colIndex;
			_replacerForm.ColName = MaindataGrid.Columns[colIndex].Name;
			_replacerForm.checkBox1.Enabled = (Db is IExtensibility);
			_replacerForm.ShowDialog();
		}

		void поискToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				текстпоУмолчаниюToolStripMenuItem_Click(this, e);
				_filtersForm.FilterMode = DataOperations.FilterMode.Text;
				string columnName = _filtersForm.ColName = MaindataGrid.Columns[MaindataGrid.CurrentCell.ColumnIndex].HeaderText;
				_filtersForm.AppDir = _appDir;
				_filtersForm.ShowDialog();
				if (_filtersForm.DialogResult == DialogResult.Yes)
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					string searchText = _filtersForm.textValue;
					DataOperations.SearchTextMode mode = _filtersForm.txtMode;
					int columnIndex = MaindataGrid.CurrentCell.ColumnIndex;
					DataTable tmpDataTable = (MaindataGrid.DataSource as DataTable).Copy();
					DataTable newTable = new DataTable();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(tmpDataTable, columnIndex, searchText, mode);							
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
					toolStripStatusLabel1.Text =
						$"Количество записей удовлетворяющих поисковому запросу '{searchText}' в столбце <{columnName}> : {MaindataGrid.Rows.Count} (Время загрузки: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек)";
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

        void сброситьРезультатыТекущегоПоискаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	try 
            //	{
            //		Stopwatch perfWatch = new Stopwatch();
            //		perfWatch.Start();
            //		MaindataGrid.DataSource = DB.currentTable = DB.listOfTables.Tables[tabControl1.SelectedIndex];
            //		FormatDataGrid();
            //		TimeSpan span = perfWatch.Elapsed;
            //		toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
            //		MaindataGrid.CurrentCell = null;
            //	} 
            //	catch (Exception exc) 
            //	{
            //		MessageBox.Show(exc.Message);
            //		LogManager.Log.AddToLog(AppDir, exc);
            //	}
        }

        void сменитьИсточникДанныхToolStripMenuItem_Click(object sender, EventArgs e)
            {
                //	try 
                //	{
                //		a: recentForm = new RecentAndDirectives();
                //		recentForm.ShowDialog();
                //		if (recentForm.DialogResult == DialogResult.OK)
                //		{
                //			if (!string.IsNullOrWhiteSpace(recentForm.PathOfDB))
                //			{
                //				Stopwatch perfWatch = new Stopwatch();
                //				perfWatch.Start();
                //				toolStripProgressBar1.Visible = true;
                //				Task taskInThread = Task.Factory.StartNew( () => 
                //				{
                //					DB = new Database(recentForm.PathOfDB);							
                //				});
                //				while (!taskInThread.IsCompleted)
                //				{
                //					toolStripStatusLabel2.Text = "Производится загрузка данных...";
                //					Application.DoEvents();
                //				}
                //				toolStripStatusLabel2.Text = string.Empty;
                //				GetTablesList(db);
                //				MaindataGrid.DataSource = DB.currentTable = DB.listOfTables.Tables[0];
                //				CheckViewOfGrid();
                //				FormatDataGrid();
                //				toolStripProgressBar1.Visible = false;
                //				TimeSpan span = perfWatch.Elapsed;
                //				Text = string.Format("Статико-аналитический терминал \"Стазис\" - <<{0}>> - тип БД: {1}", Path.GetFileNameWithoutExtension(DB.pathOfDatabase), DB.GetTypeOfDBFile());
                //				toolStripStatusLabel1.Text = string.Format("Всего записей в таблице: {3} (Время загрузки: {0} мин {1} сек {2} мсек)", span.Minutes, span.Seconds, span.Milliseconds, MaindataGrid.Rows.Count);
                //			}
                //			else
                //			{
                //				toolStripProgressBar1.Visible = false;
                //				recentForm.Dispose();
                //				goto a;
                //			}
                //		}
                //	} 
                //	catch (Exception exc) 
                //	{
                //		MessageBox.Show(exc.Message);
                //		LogManager.Log.AddToLog(AppDir, exc);
                //	}
            }

        void списокУникальныхЗначенийToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				int col = MaindataGrid.CurrentCell.ColumnIndex;
				string columnName = MaindataGrid.Columns[col].HeaderText;
				_uniquesForm.colName = columnName;
				Stopwatch perfUniq = new Stopwatch();
				perfUniq.Start();
				Task loadUniquesTask = Task.Factory.StartNew(() =>
				{
					DataOperations.LoadUniques(MaindataGrid.DataSource as DataTable, col, _uniquesForm.dataGridView1);
				});
				while (!loadUniquesTask.IsCompleted)
				{
					toolStripProgressBar1.Visible = true;
					toolStripStatusLabel2.Text = "Производится вычисление уникальных значений столбца " + columnName + "..." ;
					Application.DoEvents();
				}
				toolStripProgressBar1.Visible = false;
				TimeSpan spanUniq = perfUniq.Elapsed;
				toolStripStatusLabel2.Text =
					$"Время вычисления составило: {spanUniq.Minutes} мин {spanUniq.Seconds} сек {spanUniq.Milliseconds} мсек";
				_uniquesForm.ShowDialog(this);
				if (_uniquesForm.DialogResult == DialogResult.Yes)
				{
					toolStripStatusLabel2.Text = string.Empty;
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					string cellValue = _uniquesForm.QueryText;
					DataTable newTable = Db.CurrentDataTable.Clone();
					Task taskInThread = Task.Factory.StartNew( () => 
					{
						newTable = DataOperations.QueryProcess(MaindataGrid.DataSource as DataTable, col, cellValue);							
					});
					while (!taskInThread.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится обработка данных...";
						Application.DoEvents();
					}
					if (AppSettings.Default.SaveSearchResult)
					{
						Db.CurrentDataTable.Clear();
						Db.CurrentDataTable.Merge(newTable);
						MaindataGrid.DataSource = Db.CurrentDataTable;
					}
					else MaindataGrid.DataSource = newTable;
					FormatDataGrid();
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text = string.Empty;
					toolStripStatusLabel1.Text =
						$"Результаты выборки по запросу '{cellValue}' в столбце <{columnName}> : {MaindataGrid.Rows.Count} (Время загрузки: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек)";
					MaindataGrid.CurrentCell = null;
				}
			}
			catch (Exception exc)
			{
				toolStripProgressBar1.Visible = false;
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
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
				toolStripStatusLabel2.Text =
					$"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
				contextMenuStrip1.Hide();
				FormatDataGrid();
			} 
			catch (Exception exc) 
			{
				contextMenuStrip1.Hide();
				MessageBox.Show("Невозможно выполнить преобразование типа столбца");
				LogManager.Log.AddToLog(_appDir, exc);
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
				toolStripStatusLabel2.Text =
					$"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
				contextMenuStrip1.Hide();
				FormatDataGrid();
			} 
			catch (Exception exc) 
			{
				contextMenuStrip1.Hide();
				MessageBox.Show("Невозможно выполнить преобразование типа столбца");
				LogManager.Log.AddToLog(_appDir, exc);
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
					toolStripStatusLabel2.Text =
						$"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
				}
				else AppSettings.Default.AutoResizeRowHeight = false;
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
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
					toolStripStatusLabel2.Text =
						$"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
				}
				else AppSettings.Default.AutoResizeColumnWidth = false;
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
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
					toolStripStatusLabel2.Text =
						$"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
				}
				else AppSettings.Default.AutoResizeColumnHeaderHeight = false;
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
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
					toolStripStatusLabel2.Text =
						$"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
				}
				else AppSettings.Default.AutoResizeColumnHeaderWidth = false;
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
			}
		}

		void экспортВExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try 
			{
				saveFileDialog1.Filter = AppSettings.Default.SupportedDatabaseTypes;
				saveFileDialog1.FilterIndex = 0;
				if (saveFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
				{
					Stopwatch perfWatch = new Stopwatch();
					perfWatch.Start();
					Task task1 = Task.Factory.StartNew( () =>
					{
						_stazisDatabaseFactory.Export(saveFileDialog1.FileName);
					});
					while (!task1.IsCompleted)
					{
						toolStripStatusLabel2.Text = "Производится экспорт данных...";
						Application.DoEvents();
					}
					TimeSpan span = perfWatch.Elapsed;
					toolStripStatusLabel2.Text =
						$"(Кликните, чтобы скрыть) Экспорт данных в файл {saveFileDialog1.FileName} завершен. (Время выгрузки: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек)";
					MaindataGrid.CurrentCell = null;
				}
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(_appDir, exc);
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
				LogManager.Log.AddToLog(_appDir, exc);
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
			toolStripStatusLabel2.Text = $"Время последней операции: {span.Minutes} мин {span.Seconds} сек {span.Milliseconds} мсек";
			MaindataGrid.CurrentCell = null;
		}

        void добавитьЗаписьВИсточникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            	AddRecord addRecForm = new AddRecord() { Db = Db };
            	addRecForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			AppSettings.Default.Save();
			if (Db.GetDatabaseConnectionStatus() == "Open")
			{
				Db.DisconnectFromDatabase();
			}
		}
	}
}