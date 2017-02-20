using System;
using System.Linq;
using System.Windows.Forms;

namespace Stazis
{
    public partial class ValueFilters : Form
    {
        public DataOperations.FilterMode FilterMode;
        public DataOperations.SearchIntMode intMode;
        public DataOperations.SearchTextMode txtMode;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double intValue { get; set; }
        public double maxDoubleRange { get; set; }
        public double minDoubleRange { get; set; }
        public DateTime maxDateRange { get; set; }
        public DateTime minDateRange { get; set; }
        public string textValue { get; set; }
        public string ColName {get;set;}
		public string AppDir { get; set; }
		
        public ValueFilters()
        {
            InitializeComponent();
        }

        void button2_Click(object sender, EventArgs e)
        {
            try 
            {
            	switch (FilterMode)
	            {
	                case DataOperations.FilterMode.Integer:
	                    intValue = (double)numericUpDown1.Value;
	                    switch (comboBox1.SelectedIndex)
	                    {
	                        case 0:
	                            intMode = DataOperations.SearchIntMode.EqualTo;
	                            break;
	                        case 1:
	                            intMode = DataOperations.SearchIntMode.LargerThen;
	                            break;
	                        case 2:
	                            intMode = DataOperations.SearchIntMode.SmallerThen;
	                            break;
	                    }
	                    break;
	                case DataOperations.FilterMode.Date:
	                    DateTime tmpDate = TimePicker1.Value;
						StartDate = new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, tmpDate.Hour, tmpDate.Minute, tmpDate.Second);
						tmpDate = TimePicker2.Value;
	                    EndDate = new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, tmpDate.Hour, tmpDate.Minute, tmpDate.Second);
	                    break;
	                case DataOperations.FilterMode.Text:
	                    if (!string.IsNullOrEmpty(textBox1.Text)) textValue = textBox1.Text;
	                    else button1_Click(this, e);
	                    switch (comboBox2.SelectedIndex)
	                    {
	                        case 0:
	                            txtMode = DataOperations.SearchTextMode.Equals;
	                            break;
	                        case 1:
	                            txtMode = DataOperations.SearchTextMode.StartWith;
	                            break;
	                        case 2:
	                            txtMode = DataOperations.SearchTextMode.Contains;
	                            break;
	                    }
	                    break;
	            }
	            DialogResult = DialogResult.Yes;
            } 
            catch (Exception exc) 
            {
            	MessageBox.Show(exc.Message);
            	LogManager.Log.AddToLog(AppDir, exc);
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        void ValueFilters_Load(object sender, EventArgs e)
        {
        	Text = "Фильтр значений столбца - '" + ColName + "'";
            switch (FilterMode)
            {
                case DataOperations.FilterMode.Integer:
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    comboBox1.SelectedIndex = 0;
                    numericUpDown1.Minimum = numericUpDown1.Value = (decimal)minDoubleRange;
                    numericUpDown1.Maximum = (decimal)maxDoubleRange;
                    break;
                case DataOperations.FilterMode.Date:
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = false;
					TimePicker1.Value = TimePicker1.MinDate = TimePicker2.MinDate = minDateRange;
					TimePicker2.Value = TimePicker1.MaxDate = TimePicker2.MaxDate = maxDateRange;
                    break;
                case DataOperations.FilterMode.Text:
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = true;
                    comboBox2.SelectedIndex = 0;
                    break;
            }
        }
    }
}
