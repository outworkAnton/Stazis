using System;
using System.ComponentModel;
using Stazis.Properties;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Stazis
{
    public partial class RecentAndDirectives : Form
    {
        public string PathOfDb { get; set; }

        public RecentAndDirectives()
        {
            InitializeComponent();
        }

        void GetRecentList()
        {
            if (AppSettings.Default.RecentList != null)
            {
                recentGrid.Columns[0].ValueType = typeof(DateTime);
                recentGrid.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
                var recordsFromSettings = new Dictionary<DateTime, string>();
                foreach (var row in AppSettings.Default.RecentList)
                {
                    var pair = row.Split('#');
                    DateTime key = Convert.ToDateTime(pair[0]);
                    string value = pair[1];
                    if (recordsFromSettings.ContainsKey(key) || recordsFromSettings.ContainsValue(value))
                    {
                        AppSettings.Default.RecentList.Remove(row);
                        continue;
                    }
                    recordsFromSettings.Add(key,value);
                    recentGrid.Rows.Add(key, value);
                }
                recentGrid.Sort(recentGrid.Columns[0], ListSortDirection.Descending);
            }
        }

        void SetRecentList(DataGridView Grid)
        {
            if (AppSettings.Default.RecentList == null)
            {
                AppSettings.Default.RecentList = new System.Collections.Specialized.StringCollection();
            }
            foreach(DataGridViewRow row in Grid.Rows)
            {
                if (row != null)
                {
                    AppSettings.Default.RecentList.Add(row.Cells[0].Value + "#" + row.Cells[1].Value);
                }
            }
        }

        void RecentAndDirectives_Load(object sender, EventArgs e)
        {
            recentGrid.Columns[0].Width = 150;
            recentGrid.Columns[1].Width = recentGrid.Width - 155;
            GetRecentList();
            if (recentGrid.RowCount == 0)
                NewRecord();
        }

        void recentGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (File.Exists(recentGrid.CurrentRow.Cells[1].Value.ToString()))
                {
                    SelectRecord();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Выбранный файл не найден. Запись будет удалена.");
                    recentGrid.Rows.Remove(recentGrid.CurrentRow);
                }
            }
        }

        void SelectRecord()
        {
            if (!string.IsNullOrWhiteSpace(recentGrid.CurrentRow?.Cells[1].Value.ToString()))
            {
                PathOfDb = recentGrid.CurrentRow.Cells[1].Value.ToString();
                int oldID = recentGrid.CurrentRow.Index;
                var row = (DataGridViewRow)recentGrid.CurrentRow.Clone();
                row.Cells[0].Value = DateTime.Now.ToString();
                row.Cells[1].Value = recentGrid.CurrentRow.Cells[1].Value;
                recentGrid.Rows.RemoveAt(oldID);
                AppSettings.Default.RecentList.RemoveAt(oldID);
                recentGrid.Rows.Insert(0, row);
            }
        }

        void NewRecord()
        {
            openFileDialog1.Filter = AppSettings.Default.SupportedDatabaseTypes;
			openFileDialog1.FilterIndex = 0;
            openFileDialog1.ShowDialog();
            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                for (int i = 0; i < recentGrid.RowCount; i++)
                {
                    if (openFileDialog1.FileName.Equals(recentGrid[1, i].Value))
                    {
                        MessageBox.Show("Выбранный файл уже присутствует в списке", "Добавление дубликата", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        recentGrid.CurrentCell = recentGrid[1, i];
                        return;
                    }
                }
                recentGrid.Rows.Insert(0, 1);
                recentGrid[0, 0].Value = DateTime.Now.ToString();
                recentGrid[1, 0].Value = openFileDialog1.FileName; 
                PathOfDb = openFileDialog1.FileName;
                recentGrid.CurrentCell = recentGrid[0, 0];
            }
        }

        void recentGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
            	if (recentGrid.Rows.Count > 0)
                	recentGrid.CurrentCell = recentGrid[e.ColumnIndex, e.RowIndex];
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        void RecentAndDirectives_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            SetRecentList(recentGrid);
        }

        void новаяЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewRecord();
        }

        void recentGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SelectRecord();
                    DialogResult = DialogResult.OK;
                    break;
                case Keys.Delete:
                    DeleteRecord();
                    break;
            }
        }

        void RecentAndDirectives_Activated(object sender, EventArgs e)
        {
            if (tabControl1.TabIndex == 0)
            {
                recentGrid.Focus();
                if (recentGrid.RowCount > 0) recentGrid[0, 0].Selected = true;
            }
            else
            {
                directiveGrid.Focus();
                directiveGrid[0, 0].Selected = true;
            }
        }

        void удалитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        private void DeleteRecord()
        {
            if ((recentGrid.Rows.Count > 0))
            {
                if (!string.IsNullOrEmpty(recentGrid.SelectedCells[0].Value.ToString()))
                {
                    AppSettings.Default.RecentList.RemoveAt(recentGrid.SelectedCells[0].RowIndex);
                }
                recentGrid.Rows.RemoveAt(recentGrid.SelectedCells[0].RowIndex);
            }
            if (recentGrid.RowCount == 0)
            {
                NewRecord();
            }
        }
    }
}
