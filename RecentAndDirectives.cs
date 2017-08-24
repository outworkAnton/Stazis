using System;
using System.ComponentModel;
using Stazis.Properties;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace Stazis
{
    public partial class RecentAndDirectives : Form
    {
        public string PathOfDB { get; set; }

        public RecentAndDirectives()
        {
            InitializeComponent();
        }

        public void FillGrid(DataGridViewRow[] RecentList)
        {
            if (RecentList != null)
            {
                recentGrid.Columns[0].ValueType = typeof(DateTime);
                recentGrid.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
                foreach (var row in RecentList)
                {
                    recentGrid.Rows.Add(row);
                }
                recentGrid.Sort(recentGrid.Columns[0], ListSortDirection.Descending);
            }
        }

        public DataGridViewRow[] GetRecentList()
		{
            DataGridViewRow[] RecentRecords = null;
            if (AppSettings.Default.RecentList != null)
            {
                RecentRecords = new DataGridViewRow[AppSettings.Default.RecentList.Count];
                for (int i = 0; i < AppSettings.Default.RecentList.Count; i++)
                {
                    var pair = AppSettings.Default.RecentList[i].Split('#');
                    var row = new DataGridViewRow();
                    row.SetValues(DateTime.Parse(pair[0]), pair[1]);
                    RecentRecords[i] = row;
                } 
            }
            if (RecentRecords != null)
				return RecentRecords;
			return null;
		}

        public static void SetRecentList(DataGridView Grid)
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
            FillGrid(GetRecentList());
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
            if (recentGrid.CurrentRow != null)
                if (!string.IsNullOrWhiteSpace(recentGrid.CurrentRow.Cells[1].Value.ToString()))
                {
                    PathOfDB = recentGrid.CurrentRow.Cells[1].Value.ToString();
                    int oldID = recentGrid.CurrentRow.Index;
                    DataGridViewRow row = (DataGridViewRow)recentGrid.CurrentRow.Clone();
                    row.Cells[0].Value = DateTime.Now.ToString();
                    row.Cells[1].Value = recentGrid.CurrentRow.Cells[1].Value;
                    recentGrid.Rows.RemoveAt(oldID);
                    recentGrid.Rows.Insert(0, row);
                }
        }

        public void NewRecord()
        {
            var supportedTypes = new string[AppSettings.Default.SupportedImportTypes.Count];
            AppSettings.Default.SupportedImportTypes.CopyTo(supportedTypes, 0);
            openFileDialog1.Filter = string.Join("", supportedTypes);
			openFileDialog1.FilterIndex = 0;
            openFileDialog1.ShowDialog();
            if (!string.IsNullOrWhiteSpace(openFileDialog1.FileName))
            {
                recentGrid.Rows.Insert(0, 1);
                recentGrid[0, 0].Value = DateTime.Now.ToString();
                recentGrid[1, 0].Value = openFileDialog1.FileName;
                PathOfDB = openFileDialog1.FileName;
                DialogResult = DialogResult.OK;
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
            AppSettings.Default.Save();
        }

        void новаяЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewRecord();
        }

        void recentGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectRecord();
                DialogResult = DialogResult.OK;
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
        	if ((recentGrid.Rows.Count > 0))
            {
                if (!string.IsNullOrEmpty(recentGrid.SelectedCells[0].Value.ToString()))
                {
                    recentGrid.Rows.RemoveAt(recentGrid.SelectedCells[0].RowIndex);
                }
            }
            if (recentGrid.RowCount == 0)
            {
                NewRecord();
            }
        }
    }
}
