using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using StazisExtensibilityInterface;

namespace Stazis
{
    /// <summary>
    /// Description of AddRecord.
    /// </summary>
    public partial class AddRecord : Form
	{
        public IDatabaseExtensibility Db { get; set; }
		
		public AddRecord()
		{
			InitializeComponent();
		}
		
		void ConvertColumnNames()
		{
			dataGridView1.Columns.Add("Values", "Значение");
			for (int i = 0; i < Db.CurrentDataTable.Columns.Count; i++) 
			{
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = Db.CurrentDataTable.Columns[i].ColumnName;
                if (Db.CurrentDataTable.Columns[i].AutoIncrement)
                {
                    dataGridView1.Rows[i].Cells[0].Value = "This field increments automatically";
                    dataGridView1.Rows[i].ReadOnly = true;
                }
                dataGridView1.Rows[i].Cells[0].ValueType = Db.CurrentDataTable.Columns[i].DataType;
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
            var values = new List<dynamic>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				object row = dataGridView1.Rows[i].Cells[0].Value;
				try 
				{
                    if (Db.CurrentDataTable.Columns[i].AutoIncrement) continue;
					switch (Type.GetTypeCode(Db.CurrentDataTable.Columns[i].DataType))
					{
						case TypeCode.DateTime:
							values.Add(Convert.ToDateTime(row));
							break;
						case TypeCode.Double:
                            values.Add(Convert.ToDouble(row));
							break;
                        case TypeCode.Int32:
                            values.Add(Convert.ToInt32(row));
                            break;
						case TypeCode.String: 
							if (row == null)
                                values.Add(string.Empty);
							else
                                values.Add(row.ToString());
							break;
						default:
                            values.Add(DBNull.Value);
							break;
					}
                    if (!Db.CurrentDataTable.Columns[i].AllowDBNull && row == null) throw new ArgumentNullException();
                }
                catch (FormatException exc) 
				{
					dataGridView1[0, i].Selected = true;
					MessageBox.Show($"Значение строки {(i + 1)} не соответствует типу данных в базе данных\n" +
					                $"(Тип вводимых данных: {row.GetType()}, тип значений базы данных: {Db.CurrentDataTable.Columns[i].DataType})\n" +
					                $"Исправьте формат вводимых данных и повторите попытку сохранения\nСлужебная информация: {exc.StackTrace}");
					return false;
				}
                catch (ArgumentNullException arg)
                {
                    dataGridView1[0, i].Selected = true;
                    MessageBox.Show($"Значение строки {i} не может быть пустым");
                    return false;
                }
			}
            if (values.TrueForAll(x => string.IsNullOrWhiteSpace(x.ToString())))
            {
                return false;
            }
            else
            {
                Db.AddRecord(values);
                return true;
            }
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
