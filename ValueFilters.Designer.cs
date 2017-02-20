namespace Stazis
{
    partial class ValueFilters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.button1 = new System.Windows.Forms.Button();
        	this.button2 = new System.Windows.Forms.Button();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.comboBox1 = new System.Windows.Forms.ComboBox();
        	this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
        	this.groupBox2 = new System.Windows.Forms.GroupBox();
        	this.TimePicker2 = new System.Windows.Forms.DateTimePicker();
        	this.TimePicker1 = new System.Windows.Forms.DateTimePicker();
        	this.label2 = new System.Windows.Forms.Label();
        	this.label1 = new System.Windows.Forms.Label();
        	this.groupBox3 = new System.Windows.Forms.GroupBox();
        	this.textBox1 = new System.Windows.Forms.TextBox();
        	this.comboBox2 = new System.Windows.Forms.ComboBox();
        	this.label5 = new System.Windows.Forms.Label();
        	this.groupBox1.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
        	this.groupBox2.SuspendLayout();
        	this.groupBox3.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// button1
        	// 
        	this.button1.Location = new System.Drawing.Point(12, 201);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(134, 23);
        	this.button1.TabIndex = 0;
        	this.button1.Text = "Отмена";
        	this.button1.UseVisualStyleBackColor = true;
        	this.button1.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// button2
        	// 
        	this.button2.Location = new System.Drawing.Point(246, 201);
        	this.button2.Name = "button2";
        	this.button2.Size = new System.Drawing.Size(134, 23);
        	this.button2.TabIndex = 1;
        	this.button2.Text = "Применить";
        	this.button2.UseVisualStyleBackColor = true;
        	this.button2.Click += new System.EventHandler(this.button2_Click);
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.comboBox1);
        	this.groupBox1.Controls.Add(this.numericUpDown1);
        	this.groupBox1.Location = new System.Drawing.Point(12, 12);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(368, 54);
        	this.groupBox1.TabIndex = 2;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Число";
        	// 
        	// comboBox1
        	// 
        	this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBox1.FormattingEnabled = true;
        	this.comboBox1.Items.AddRange(new object[] {
			"Равно",
			"Больше",
			"Меньше"});
        	this.comboBox1.Location = new System.Drawing.Point(12, 19);
        	this.comboBox1.Name = "comboBox1";
        	this.comboBox1.Size = new System.Drawing.Size(164, 21);
        	this.comboBox1.TabIndex = 1;
        	// 
        	// numericUpDown1
        	// 
        	this.numericUpDown1.Location = new System.Drawing.Point(192, 19);
        	this.numericUpDown1.Maximum = new decimal(new int[] {
			1000000000,
			0,
			0,
			0});
        	this.numericUpDown1.Name = "numericUpDown1";
        	this.numericUpDown1.Size = new System.Drawing.Size(164, 20);
        	this.numericUpDown1.TabIndex = 0;
        	// 
        	// groupBox2
        	// 
        	this.groupBox2.Controls.Add(this.label5);
        	this.groupBox2.Controls.Add(this.TimePicker2);
        	this.groupBox2.Controls.Add(this.TimePicker1);
        	this.groupBox2.Controls.Add(this.label2);
        	this.groupBox2.Controls.Add(this.label1);
        	this.groupBox2.Location = new System.Drawing.Point(12, 72);
        	this.groupBox2.Name = "groupBox2";
        	this.groupBox2.Size = new System.Drawing.Size(368, 68);
        	this.groupBox2.TabIndex = 3;
        	this.groupBox2.TabStop = false;
        	this.groupBox2.Text = "Дата";
        	// 
        	// TimePicker2
        	// 
        	this.TimePicker2.CustomFormat = "dd.MM.yyyy HH:mm:ss";
        	this.TimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        	this.TimePicker2.Location = new System.Drawing.Point(192, 33);
        	this.TimePicker2.Name = "TimePicker2";
        	this.TimePicker2.Size = new System.Drawing.Size(164, 20);
        	this.TimePicker2.TabIndex = 5;
        	// 
        	// TimePicker1
        	// 
        	this.TimePicker1.CustomFormat = "dd.MM.yyyy HH:mm:ss";
        	this.TimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        	this.TimePicker1.Location = new System.Drawing.Point(12, 33);
        	this.TimePicker1.Name = "TimePicker1";
        	this.TimePicker1.Size = new System.Drawing.Size(164, 20);
        	this.TimePicker1.TabIndex = 4;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(189, 17);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(98, 13);
        	this.label2.TabIndex = 3;
        	this.label2.Text = "Конец диапазона:";
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(9, 17);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(104, 13);
        	this.label1.TabIndex = 2;
        	this.label1.Text = "Начало диапазона:";
        	// 
        	// groupBox3
        	// 
        	this.groupBox3.Controls.Add(this.textBox1);
        	this.groupBox3.Controls.Add(this.comboBox2);
        	this.groupBox3.Location = new System.Drawing.Point(12, 141);
        	this.groupBox3.Name = "groupBox3";
        	this.groupBox3.Size = new System.Drawing.Size(368, 54);
        	this.groupBox3.TabIndex = 5;
        	this.groupBox3.TabStop = false;
        	this.groupBox3.Text = "Текст";
        	// 
        	// textBox1
        	// 
        	this.textBox1.Location = new System.Drawing.Point(192, 19);
        	this.textBox1.Name = "textBox1";
        	this.textBox1.Size = new System.Drawing.Size(164, 20);
        	this.textBox1.TabIndex = 2;
        	// 
        	// comboBox2
        	// 
        	this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBox2.FormattingEnabled = true;
        	this.comboBox2.Items.AddRange(new object[] {
			"Соответствует ...",
			"Начинается с ...",
			"Содержит ..."});
        	this.comboBox2.Location = new System.Drawing.Point(12, 19);
        	this.comboBox2.Name = "comboBox2";
        	this.comboBox2.Size = new System.Drawing.Size(164, 21);
        	this.comboBox2.TabIndex = 1;
        	// 
        	// label5
        	// 
        	this.label5.Location = new System.Drawing.Point(179, 35);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(11, 23);
        	this.label5.TabIndex = 8;
        	this.label5.Text = "-";
        	// 
        	// ValueFilters
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(392, 233);
        	this.ControlBox = false;
        	this.Controls.Add(this.groupBox3);
        	this.Controls.Add(this.groupBox2);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.button2);
        	this.Controls.Add(this.button1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.Name = "ValueFilters";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Фильтр значений";
        	this.Load += new System.EventHandler(this.ValueFilters_Load);
        	this.groupBox1.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
        	this.groupBox2.ResumeLayout(false);
        	this.groupBox2.PerformLayout();
        	this.groupBox3.ResumeLayout(false);
        	this.groupBox3.PerformLayout();
        	this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.DateTimePicker TimePicker2;
        private System.Windows.Forms.DateTimePicker TimePicker1;
        private System.Windows.Forms.Label label5;
    }
}