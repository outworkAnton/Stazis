namespace Stazis
{
    partial class RecentAndDirectives
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
        	this.components = new System.ComponentModel.Container();
        	System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.Recent = new System.Windows.Forms.TabPage();
        	this.recentGrid = new System.Windows.Forms.DataGridView();
        	this.DateTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.Directives = new System.Windows.Forms.TabPage();
        	this.directiveGrid = new System.Windows.Forms.DataGridView();
        	this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        	this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.новаяЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.удалитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.tabControl1.SuspendLayout();
        	this.Recent.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.recentGrid)).BeginInit();
        	this.Directives.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.directiveGrid)).BeginInit();
        	this.contextMenuStrip1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	this.tabControl1.Controls.Add(this.Recent);
        	this.tabControl1.Controls.Add(this.Directives);
        	this.tabControl1.Location = new System.Drawing.Point(0, 5);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(773, 368);
        	this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
        	this.tabControl1.TabIndex = 0;
        	// 
        	// Recent
        	// 
        	this.Recent.Controls.Add(this.recentGrid);
        	this.Recent.Location = new System.Drawing.Point(4, 22);
        	this.Recent.Name = "Recent";
        	this.Recent.Padding = new System.Windows.Forms.Padding(3);
        	this.Recent.Size = new System.Drawing.Size(765, 342);
        	this.Recent.TabIndex = 0;
        	this.Recent.Text = "Последние";
        	this.Recent.UseVisualStyleBackColor = true;
        	// 
        	// recentGrid
        	// 
        	this.recentGrid.AllowUserToAddRows = false;
        	this.recentGrid.AllowUserToDeleteRows = false;
        	this.recentGrid.AllowUserToResizeColumns = false;
        	this.recentGrid.AllowUserToResizeRows = false;
        	this.recentGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        	this.recentGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
        	this.recentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.recentGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.DateTimeColumn,
			this.dataGridViewTextBoxColumn1});
        	this.recentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.recentGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
        	this.recentGrid.Location = new System.Drawing.Point(3, 3);
        	this.recentGrid.MultiSelect = false;
        	this.recentGrid.Name = "recentGrid";
        	this.recentGrid.RowHeadersVisible = false;
        	dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
        	this.recentGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
        	this.recentGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        	this.recentGrid.Size = new System.Drawing.Size(759, 336);
        	this.recentGrid.TabIndex = 0;
        	this.recentGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.recentGrid_CellMouseClick);
        	this.recentGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.recentGrid_CellMouseDoubleClick);
        	this.recentGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.recentGrid_KeyDown);
        	// 
        	// DateTimeColumn
        	// 
        	this.DateTimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
        	this.DateTimeColumn.HeaderText = "Дата и время открытия";
        	this.DateTimeColumn.Name = "DateTimeColumn";
        	this.DateTimeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        	this.DateTimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
        	// 
        	// dataGridViewTextBoxColumn1
        	// 
        	this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
        	this.dataGridViewTextBoxColumn1.HeaderText = "Путь к файлу";
        	this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        	this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        	this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
        	this.dataGridViewTextBoxColumn1.Width = 756;
        	// 
        	// Directives
        	// 
        	this.Directives.Controls.Add(this.directiveGrid);
        	this.Directives.Location = new System.Drawing.Point(4, 22);
        	this.Directives.Name = "Directives";
        	this.Directives.Padding = new System.Windows.Forms.Padding(3);
        	this.Directives.Size = new System.Drawing.Size(765, 342);
        	this.Directives.TabIndex = 1;
        	this.Directives.Text = "Директивы";
        	this.Directives.UseVisualStyleBackColor = true;
        	// 
        	// directiveGrid
        	// 
        	this.directiveGrid.AllowUserToResizeColumns = false;
        	this.directiveGrid.AllowUserToResizeRows = false;
        	this.directiveGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
        	this.directiveGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        	this.directiveGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.directiveGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
        	this.directiveGrid.Location = new System.Drawing.Point(3, 3);
        	this.directiveGrid.MultiSelect = false;
        	this.directiveGrid.Name = "directiveGrid";
        	this.directiveGrid.ReadOnly = true;
        	this.directiveGrid.RowHeadersVisible = false;
        	this.directiveGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        	this.directiveGrid.Size = new System.Drawing.Size(759, 336);
        	this.directiveGrid.TabIndex = 0;
        	// 
        	// openFileDialog1
        	// 
        	this.openFileDialog1.DefaultExt = "*.xls";
        	this.openFileDialog1.FilterIndex = 0;
        	// 
        	// contextMenuStrip1
        	// 
        	this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.новаяЗаписьToolStripMenuItem,
			this.удалитьЗаписьToolStripMenuItem});
        	this.contextMenuStrip1.Name = "contextMenuStrip1";
        	this.contextMenuStrip1.Size = new System.Drawing.Size(159, 48);
        	// 
        	// новаяЗаписьToolStripMenuItem
        	// 
        	this.новаяЗаписьToolStripMenuItem.Name = "новаяЗаписьToolStripMenuItem";
        	this.новаяЗаписьToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
        	this.новаяЗаписьToolStripMenuItem.Text = "Новая запись...";
        	this.новаяЗаписьToolStripMenuItem.Click += new System.EventHandler(this.новаяЗаписьToolStripMenuItem_Click);
        	// 
        	// удалитьЗаписьToolStripMenuItem
        	// 
        	this.удалитьЗаписьToolStripMenuItem.Name = "удалитьЗаписьToolStripMenuItem";
        	this.удалитьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
        	this.удалитьЗаписьToolStripMenuItem.Text = "Удалить запись";
        	this.удалитьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.удалитьЗаписьToolStripMenuItem_Click);
        	// 
        	// RecentAndDirectives
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(774, 376);
        	this.Controls.Add(this.tabControl1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "RecentAndDirectives";
        	this.ShowIcon = false;
        	this.ShowInTaskbar = false;
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Выберите файл или директиву";
        	this.Activated += new System.EventHandler(this.RecentAndDirectives_Activated);
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RecentAndDirectives_FormClosing);
        	this.Load += new System.EventHandler(this.RecentAndDirectives_Load);
        	this.tabControl1.ResumeLayout(false);
        	this.Recent.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.recentGrid)).EndInit();
        	this.Directives.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.directiveGrid)).EndInit();
        	this.contextMenuStrip1.ResumeLayout(false);
        	this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Recent;
        private System.Windows.Forms.TabPage Directives;
        private System.Windows.Forms.DataGridView recentGrid;
        private System.Windows.Forms.DataGridView directiveGrid;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem новаяЗаписьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьЗаписьToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    }
}