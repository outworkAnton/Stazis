using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stazis
{
    public partial class Replacer : Form
    {
        MainForm mainForm;
        public int Col { get; set; }
        public string ColName {get;set;}
		
        public Replacer(MainForm form)
        {
            InitializeComponent();
			mainForm = form;
        }

        void changeButton_Click(object sender, EventArgs e)
        {
            try 
            {
            	if (string.IsNullOrWhiteSpace(comboBox1.Text))
				if (MessageBox.Show("Вы действительно хотите заменить элементы на пустое значение?", "Поле элемента замены пустое!", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
	            progressBar1.Visible = true;
				int changesCount = DataOperations.ChangeRecords(mainForm.db, Col, checkedListBox1.CheckedItems.OfType<string>().ToList(), comboBox1.Text);
				if (checkBox1.Checked) (mainForm.db as IChangebleDatabase).ChangeRecords(Col, checkedListBox1.CheckedItems.OfType<string>().ToList(), comboBox1.Text);
				progressBar1.Visible = false;
				MessageBox.Show(string.Format("Операция успешно завершена\nВыполнено замен: {0}", changesCount));
				for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
				{
					foreach (string item in checkedListBox1.CheckedItems)
						backUp.Remove(item);
					if (checkedListBox1.GetItemChecked(i))
						checkedListBox1.Items.RemoveAt(i);
				}
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }

        void correctButton_Click(object sender, EventArgs e)
        {
            try 
            {
            	if (checkedListBox2.CheckedItems.Count == 0) return;
	            progressBar1.Visible = true;
	            //MessageBox.Show(string.Format("Операция успешно завершена\nВыполнено корректировок: {0}", DataOperations.CorrectColumnRecords(db, mainForm.tabControl1.SelectedIndex, Col, checkedListBox2.CheckedItems, checkBox1.Checked)));
	            progressBar1.Visible = false;
	            button5_Click(this, e);
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }

        public enum searchMode {StartWith, Contains}

        public List<string> backUp = new List<string>();

        void searchInList(string searchText, searchMode Mode)
        {
        	if (string.IsNullOrWhiteSpace(searchText))
            {
                checkedListBox1.Items.Clear();
                checkedListBox1.Items.AddRange(backUp.ToArray());
                comboBox1.Enabled = false;
                return;
            }
            List<string> tmp = new List<string>();
            switch (Mode)
            {
                case searchMode.Contains:
					tmp = (from order in backUp
						   where order.ToUpper().Contains(searchText.ToUpper())
						   select order).ToList();
                    break;
                case searchMode.StartWith:
					tmp = (from order in backUp
						   where order.StartsWith(searchText, StringComparison.OrdinalIgnoreCase)
						   select order).ToList();
                    break;
            }
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(tmp.ToArray());
        }

        void Replacer_Load(object sender, EventArgs e)
        {
			try 
			{
				Text = "Автоматизированный корректор элементов столбца - '" + ColName + "'";
	            checkBox1.Checked = false;
	            comboBox2.SelectedIndex = 0;
	            backUp = checkedListBox1.Items.Cast<string>().ToList();
			} 
			catch (Exception exc) 
			{
				MessageBox.Show(exc.Message);
				LogManager.Log.AddToLog(mainForm.AppDir, exc);
			}
        }

        void button3_Click(object sender, EventArgs e)
        {
            try 
            {
            	for (int i = 0; i < checkedListBox1.Items.Count; i++)
	                checkedListBox1.SetItemChecked(i, true);
	            comboBox1.Enabled = true;
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }

        void button4_Click(object sender, EventArgs e)
        {
            try 
            {
            	for (int i = 0; i < checkedListBox1.Items.Count; i++)
	                checkedListBox1.SetItemChecked(i, false);
	            comboBox1.Enabled = false;
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }

        void button6_Click(object sender, EventArgs e)
        {
            try 
            {
            	for (int i = 0; i < checkedListBox2.Items.Count; i++)
	                checkedListBox2.SetItemChecked(i, true);
	            correctButton.Enabled = true;
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }

        void button5_Click(object sender, EventArgs e)
        {
            try 
            {
            	for (int i = 0; i < checkedListBox2.Items.Count; i++)
	                checkedListBox2.SetItemChecked(i, false);
	            correctButton.Enabled = false;
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }

        void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 0)
                comboBox1.Text = checkedListBox1.Items[checkedListBox1.SelectedIndex].ToString();
            else
                comboBox1.Enabled = false;
        }

        void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            try
            {
                if (checkedListBox1.CheckedItems.Count > 0)
                {
                    comboBox1.Enabled = true;
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Unchecked)
                            comboBox1.Items.Add(checkedListBox1.Items[i]);
                }
                else
                    comboBox1.Enabled = false;
                if (comboBox1.Items.Count < 10)
                    comboBox1.MaxDropDownItems = comboBox1.Items.Count;
                else
                    comboBox1.MaxDropDownItems = 10;
            }
            catch (Exception exc)
            {
				MessageBox.Show(exc.Message);
                LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }

        void comboBox1_EnabledChanged(object sender, EventArgs e)
        {
			changeButton.Enabled = comboBox1.Enabled;
        }

        void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
			correctButton.Enabled = checkedListBox2.CheckedItems.Count > 0;
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            try 
            {
            	searchInList(textBox1.Text, (comboBox2.SelectedIndex == 0) ? searchMode.StartWith : searchMode.Contains);
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(mainForm.AppDir, exc);
            }
        }
    }
}
