namespace Stazis
{
    partial class Uniques
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
        	System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Uniques));
        	this.dataGridView1 = new System.Windows.Forms.DataGridView();
        	this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        	this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
        	this.экспортВExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        	this.textBox1 = new System.Windows.Forms.TextBox();
        	this.comboBox1 = new System.Windows.Forms.ComboBox();
        	this.построитьДиаграммуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        	this.statusStrip1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// dataGridView1
        	// 
        	this.dataGridView1.AllowUserToAddRows = false;
        	this.dataGridView1.AllowUserToDeleteRows = false;
        	this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
        	this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        	dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        	dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
        	dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        	dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
        	dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        	dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        	dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        	this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
        	this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
        	this.dataGridView1.Location = new System.Drawing.Point(2, 27);
        	this.dataGridView1.MultiSelect = false;
        	this.dataGridView1.Name = "dataGridView1";
        	this.dataGridView1.ReadOnly = true;
        	this.dataGridView1.RowHeadersVisible = false;
        	this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        	this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        	this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        	this.dataGridView1.Size = new System.Drawing.Size(464, 510);
        	this.dataGridView1.TabIndex = 0;
        	this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
        	// 
        	// statusStrip1
        	// 
        	this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripDropDownButton1,
			this.toolStripStatusLabel1});
        	this.statusStrip1.Location = new System.Drawing.Point(0, 540);
        	this.statusStrip1.Name = "statusStrip1";
        	this.statusStrip1.Size = new System.Drawing.Size(466, 22);
        	this.statusStrip1.TabIndex = 6;
        	this.statusStrip1.Text = "Данные";
        	// 
        	// toolStripDropDownButton1
        	// 
        	this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        	this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.экспортВExcelToolStripMenuItem,
			this.построитьДиаграммуToolStripMenuItem});
        	this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
        	this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        	this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
        	this.toolStripDropDownButton1.Size = new System.Drawing.Size(63, 20);
        	this.toolStripDropDownButton1.Text = "Данные";
        	// 
        	// экспортВExcelToolStripMenuItem
        	// 
        	this.экспортВExcelToolStripMenuItem.Name = "экспортВExcelToolStripMenuItem";
        	this.экспортВExcelToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
        	this.экспортВExcelToolStripMenuItem.Text = "Экспорт...";
        	this.экспортВExcelToolStripMenuItem.Click += new System.EventHandler(this.экспортВExcelToolStripMenuItem_Click);
        	// 
        	// toolStripStatusLabel1
        	// 
        	this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        	this.toolStripStatusLabel1.Size = new System.Drawing.Size(357, 17);
        	this.toolStripStatusLabel1.Spring = true;
        	this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        	// 
        	// textBox1
        	// 
        	this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	this.textBox1.Location = new System.Drawing.Point(145, 1);
        	this.textBox1.Name = "textBox1";
        	this.textBox1.Size = new System.Drawing.Size(321, 20);
        	this.textBox1.TabIndex = 7;
        	this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
        	// 
        	// comboBox1
        	// 
        	this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBox1.FormattingEnabled = true;
        	this.comboBox1.Items.AddRange(new object[] {
			"Начинается с...",
			"Содержит..."});
        	this.comboBox1.Location = new System.Drawing.Point(2, 1);
        	this.comboBox1.Name = "comboBox1";
        	this.comboBox1.Size = new System.Drawing.Size(141, 21);
        	this.comboBox1.TabIndex = 8;
        	// 
        	// построитьДиаграммуToolStripMenuItem
        	// 
        	this.построитьДиаграммуToolStripMenuItem.Name = "построитьДиаграммуToolStripMenuItem";
        	this.построитьДиаграммуToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
        	this.построитьДиаграммуToolStripMenuItem.Text = "Построить диаграмму...";
        	this.построитьДиаграммуToolStripMenuItem.Click += new System.EventHandler(this.построитьДиаграммуToolStripMenuItem_Click);
        	// 
        	// Uniques
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(466, 562);
        	this.Controls.Add(this.comboBox1);
        	this.Controls.Add(this.textBox1);
        	this.Controls.Add(this.statusStrip1);
        	this.Controls.Add(this.dataGridView1);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "Uniques";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Уникальные значения столбца";
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Uniques_FormClosing);
        	this.Load += new System.EventHandler(this.Form2_Load);
        	this.Resize += new System.EventHandler(this.Uniques_Resize);
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        	this.statusStrip1.ResumeLayout(false);
        	this.statusStrip1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem экспортВExcelToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem построитьДиаграммуToolStripMenuItem;

    }
}