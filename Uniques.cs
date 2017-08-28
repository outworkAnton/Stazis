using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using DataOperationsModule;

namespace Stazis
{
    public partial class Uniques : Form
    {
        public string QueryText { get; set; }
        public string colName { get; set; }
        DataTable Source = new DataTable();
        string AppDir = Application.StartupPath;
        MainForm mainForm = new MainForm();
		DiagramForm diagramForm = new DiagramForm();
        
        public Uniques()
        {
            InitializeComponent();
        }

		void ResizeDataGrid()
		{
			int first = dataGridView1.Columns[0].Width = 50;
			int last = dataGridView1.Columns[2].Width = 100;
			double NewWidth = Math.Floor((double)dataGridView1.Width - (first + last));
			dataGridView1.Columns[1].Width = Convert.ToInt32(NewWidth);
		}

        void Form2_Load(object sender, EventArgs e)
        {
            try 
            {
	            if (Source != null) Source.Clear();
	            Source = DataOperations.GetDataFromGrid(dataGridView1);
	            dataGridView1.Columns.Clear();
	            textBox1.ResetText();
				dataGridView1.DataSource = Source;
	            comboBox1.SelectedIndex = 0;
	            ResizeDataGrid();
            	Text = "Список уникальных значений столбца - '" + colName + "'";
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(AppDir, exc);
            }
        }

        void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try 
            {
            	if (dataGridView1.CurrentCell.Value.ToString() == "<пустое значение>") QueryText = string.Empty;
                else QueryText = dataGridView1.CurrentCell.Value.ToString();
                DialogResult = System.Windows.Forms.DialogResult.Yes;
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(AppDir, exc);
				DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }

        void экспортВExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
        	try 
        	{
        		//mainForm.saveFileDialog1.Filter = DataOperations.GetExportFileTypes();
				mainForm.saveFileDialog1.FilterIndex = 0;
        		if (mainForm.saveFileDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(mainForm.saveFileDialog1.FileName)) 
        		{
        			Stopwatch perfWatch = new Stopwatch();
        			perfWatch.Start();
        			switch (Path.GetExtension(mainForm.saveFileDialog1.FileName))
                   	{
						case ".xls":
                   		case ".xlsx":
			                DataOperations.ExportToExcel(dataGridView1.DataSource as DataTable, mainForm.saveFileDialog1.FileName, (mainForm.saveFileDialog1.FilterIndex == 1) ? ".xls" : ".xlsx");                   			
							break;
						case ".csv":
							DataOperations.ExportToCSV(dataGridView1.DataSource as DataTable, mainForm.saveFileDialog1.FileName);
						break;
			        }
        			TimeSpan span = perfWatch.Elapsed;
        			toolStripStatusLabel1.Text = string.Format("Экспорт данных в файл {0} завершен. (Время выгрузки: {1} мин {2} сек {3} мсек)", mainForm.saveFileDialog1.FileName, span.Minutes, span.Seconds, span.Milliseconds);
        		}
        	} 
        	catch (Exception exc) 
        	{
        		MessageBox.Show(exc.Message);
        		LogManager.Log.AddToLog(AppDir, exc);
				toolStripStatusLabel1.Text = string.Format("При экспорте данных в файл {0} произошла ошибка", mainForm.saveFileDialog1.FileName);
        	}
        }
                
        void textBox1_TextChanged(object sender, EventArgs e)
        {
			try 
			{
				if (!string.IsNullOrWhiteSpace(textBox1.Text))
				{
					DataTable WithoutLastRow = Source.Copy();
					WithoutLastRow.Rows.RemoveAt(WithoutLastRow.Rows.Count - 1);
					switch (comboBox1.SelectedIndex)
					{
						case 0:
							DataOperations.LoadUniquesQueryToDataGrid(DataOperations.QueryProcess(WithoutLastRow, 1, textBox1.Text, DataOperations.SearchTextMode.StartWith), dataGridView1);
							break;
						case 1:
							DataOperations.LoadUniquesQueryToDataGrid(DataOperations.QueryProcess(WithoutLastRow, 1, textBox1.Text, DataOperations.SearchTextMode.Contains), dataGridView1);
							break;
					}
					WithoutLastRow.Dispose();
					ResizeDataGrid();
				}
				else dataGridView1.DataSource = Source;
			} 
			catch (Exception exc) 
			{
//				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(AppDir, exc);
			}
        }
		void Uniques_FormClosing(object sender, FormClosingEventArgs e)
		{
			Source.Dispose();
			mainForm.Dispose();
			diagramForm.Dispose();
		}
		void Uniques_Resize(object sender, EventArgs e)
		{
			ResizeDataGrid();
		}
		void построитьДиаграммуToolStripMenuItem_Click(object sender, EventArgs e)
		{
			diagramForm = new DiagramForm();
			diagramForm.colName = colName;
			diagramForm.ShowInTaskbar = true;
			diagramForm.table = Source.Copy();
			diagramForm.Show();
		}
    }
}
