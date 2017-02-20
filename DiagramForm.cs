using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Stazis
{
	/// <summary>
	/// Description of DiagramForm.
	/// </summary>
	public partial class DiagramForm : Form
	{
		public DataTable table { get; set; }
		public string colName { get; set; }
		Chart chart1 = new Chart();
		
		public DiagramForm()
		{
			InitializeComponent();
		}

		void LoadDiagram(DataTable tableOfData, SeriesChartType chartType, int rowsCountLimit)
		{
			if (groupBox1.Controls.Contains(chart1)) 
			{
				Controls.Remove(chart1);
				chart1.Dispose();
				chart1 = new Chart();
			}
			groupBox1.Controls.Add(chart1);
			chart1.Location = new Point(3, 16);
			chart1.Dock = DockStyle.Fill;
			chart1.BackColor = SystemColors.Control;
			tableOfData.Rows.RemoveAt(tableOfData.Rows.Count - 1);
			tableOfData.Columns.RemoveAt(0);
			while (tableOfData.Rows.Count > rowsCountLimit)
				tableOfData.Rows.RemoveAt(tableOfData.Rows.Count - 1);
			chart1.ChartAreas.Add("Chart");
			chart1.ChartAreas[0].BackColor = SystemColors.Control;
			chart1.Series.Add("Data");
			chart1.Series[0].ChartType = chartType;
			chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
			chart1.Legends.Add("Legend");
			chart1.Legends[0].BackColor = SystemColors.Control;
			for (int i = 0; i < tableOfData.Rows.Count; i++) 
			{
				DataPoint dp = new DataPoint();
				dp.SetValueXY(i, tableOfData.Rows[i][1]);
				dp.Label = tableOfData.Rows[i][0].ToString();
				chart1.Series[0].Points.Add(dp);
				dp.LegendText = string.Format("{0}) {1}({2})", (i + 1), tableOfData.Rows[i][0], tableOfData.Rows[i][1]);
				dp.IsValueShownAsLabel = false;
				dp.IsVisibleInLegend = true;
			}
			comboBox1.Items.AddRange(Enum.GetNames(typeof(SeriesChartType)));
			comboBox1.SelectedIndex = (int)chart1.Series[0].ChartType;
		}
		void DiagramForm_Load(object sender, EventArgs e)
		{
			LoadDiagram(table.Copy(), SeriesChartType.Pie, 100);
			Text = "Диаграмма данных столбца - '" + colName + "'";
		}
		
		void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			chart1.Series[0].ChartType = (SeriesChartType)comboBox1.SelectedIndex;
		}
		void перезагрузитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LoadDiagram(table.Copy(), SeriesChartType.Pie, 100);
		}
	}
}
