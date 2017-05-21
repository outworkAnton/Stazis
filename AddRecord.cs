
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Stazis
{
	/// <summary>
	/// Description of AddRecord.
	/// </summary>
	public partial class AddRecord : Form
	{
		public Database DB { get; set; }
		
		public AddRecord()
		{
			InitializeComponent();
		}
		
		void ConvertColumnNames()
		{
			dataGridView1.Columns.Add("Values", "Значение");
			for (int i = 0; i < DB.currentTable.Columns.Count; i++) 
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[i].HeaderCell.Value = DB.currentTable.Columns[i].ColumnName;
			}
			dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
			if (dataGridView1.RowHeadersWidth > dataGridView1.Width / 2)
				dataGridView1.RowHeadersWidth = dataGridView1.Width / 2;
			dataGridView1.Columns[0].Width = dataGridView1.Width - dataGridView1.RowHeadersWidth;
			dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllHeaders);
			dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
		}
		
		void ClearGrid()
		{
			foreach (DataGridViewRow row in dataGridView1.Rows)
				row.Cells[0].Value = string.Empty;
			dataGridView1.CurrentCell = null;
		}
		
		bool SaveRowToList()
		{
			
			DataRow tmpRow = DB.currentTable.NewRow();
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				object row = dataGridView1.Rows[i].Cells[0].Value;
				try 
				{
					switch (Type.GetTypeCode(DB.currentTable.Columns[i].DataType))
					{
						case TypeCode.DateTime: 
							tmpRow[i] = Convert.ToDateTime(row);
							break;
						case TypeCode.Double: 
							tmpRow[i] = Convert.ToDouble(row);
							break;
						case TypeCode.String: 
							if (row == null)
								tmpRow[i] = string.Empty;
							else
								tmpRow[i] = row.ToString();
							break;
						default:
							tmpRow[i] = DBNull.Value;
							break;
					}
				} 
				catch (FormatException exc) 
				{
					dataGridView1[0, i].Selected = true;
					MessageBox.Show(string.Format("Значение строки {0} не соответствует типу данных в базе данных\n" +
					                              "(Тип вводимых данных: {1}, тип значений базы данных: {2})\n" +
					                              "Исправьте формат вводимых данных и повторите попытку сохранения\nСлужебная информация: {3}", 
					                              (i + 1), row.GetType(), DB.currentTable.Columns[i].DataType, exc.StackTrace));
					return false;
				}
			}
			if (string.IsNullOrWhiteSpace(string.Join("", tmpRow.ItemArray)))
				return false;
			DB.currentTable.Rows.Add(tmpRow);
				return true;
		}
		
		void AddRecord_Load(object sender, EventArgs e)
		{
			ConvertColumnNames();
		}
		
		void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			dataGridView1.EndEdit();
			if (!SaveRowToList()) return;
			ClearGrid();
		}
		
		void новаяЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClearGrid();
		}
	}
}
