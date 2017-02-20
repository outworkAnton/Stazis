namespace Stazis
{
    partial class MainForm
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
        	this.components = new System.ComponentModel.Container();
        	System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        	System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        	this.MaindataGrid = new System.Windows.Forms.DataGridView();
        	this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        	this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.сменитьИсточникДанныхToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.экспортВExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.анализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.вернутьсяКНачальномуСостояниюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.сброситьРезультатыТекущегоПоискаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.многоуровневыйАнализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.автоподборВысотыСтрокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.автоподборШириныСтолбцовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.автоподборВысотыЗаголовковToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.автоподборШириныЗаголовковToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        	this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        	this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
        	this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
        	this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        	this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
        	this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.типПолейСтолбцаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.числоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.задатьОграниченияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.датаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.ограничитьДиапазонToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.текстпоУмолчаниюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.поискToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.списокУникальныхЗначенийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.пакетныйЗаменительToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.tabPage1 = new System.Windows.Forms.TabPage();
        	this.скрытьСтолбецToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	((System.ComponentModel.ISupportInitialize)(this.MaindataGrid)).BeginInit();
        	this.menuStrip1.SuspendLayout();
        	this.statusStrip1.SuspendLayout();
        	this.contextMenuStrip1.SuspendLayout();
        	this.tabControl1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// MaindataGrid
        	// 
        	this.MaindataGrid.AllowUserToAddRows = false;
        	this.MaindataGrid.AllowUserToDeleteRows = false;
        	this.MaindataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        	dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
        	dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        	dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
        	dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        	dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        	dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        	this.MaindataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        	dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        	dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
        	dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        	dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
        	dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        	dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        	dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        	this.MaindataGrid.DefaultCellStyle = dataGridViewCellStyle2;
        	this.MaindataGrid.Location = new System.Drawing.Point(5, 52);
        	this.MaindataGrid.MultiSelect = false;
        	this.MaindataGrid.Name = "MaindataGrid";
        	this.MaindataGrid.ReadOnly = true;
        	this.MaindataGrid.RowHeadersVisible = false;
        	this.MaindataGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        	this.MaindataGrid.Size = new System.Drawing.Size(631, 253);
        	this.MaindataGrid.TabIndex = 2;
        	this.MaindataGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MaindataGrid_CellMouseClick);
        	this.MaindataGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MaindataGrid_CellMouseDoubleClick);
        	this.MaindataGrid.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.MaindataGrid_SortCompare);
        	// 
        	// menuStrip1
        	// 
        	this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.данныеToolStripMenuItem,
			this.анализToolStripMenuItem,
			this.видToolStripMenuItem});
        	this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        	this.menuStrip1.Name = "menuStrip1";
        	this.menuStrip1.Size = new System.Drawing.Size(641, 24);
        	this.menuStrip1.TabIndex = 4;
        	this.menuStrip1.Text = "menuStrip1";
        	// 
        	// данныеToolStripMenuItem
        	// 
        	this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.сменитьИсточникДанныхToolStripMenuItem,
			this.экспортВExcelToolStripMenuItem});
        	this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
        	this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
        	this.данныеToolStripMenuItem.Text = "Данные";
        	// 
        	// сменитьИсточникДанныхToolStripMenuItem
        	// 
        	this.сменитьИсточникДанныхToolStripMenuItem.Name = "сменитьИсточникДанныхToolStripMenuItem";
        	this.сменитьИсточникДанныхToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
        	this.сменитьИсточникДанныхToolStripMenuItem.Text = "Выбрать источник данных...";
        	this.сменитьИсточникДанныхToolStripMenuItem.Click += new System.EventHandler(this.сменитьИсточникДанныхToolStripMenuItem_Click);
        	// 
        	// экспортВExcelToolStripMenuItem
        	// 
        	this.экспортВExcelToolStripMenuItem.Name = "экспортВExcelToolStripMenuItem";
        	this.экспортВExcelToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
        	this.экспортВExcelToolStripMenuItem.Text = "Экспорт данных...";
        	this.экспортВExcelToolStripMenuItem.Click += new System.EventHandler(this.экспортВExcelToolStripMenuItem_Click);
        	// 
        	// анализToolStripMenuItem
        	// 
        	this.анализToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.вернутьсяКНачальномуСостояниюToolStripMenuItem,
			this.сброситьРезультатыТекущегоПоискаToolStripMenuItem,
			this.сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem,
			this.многоуровневыйАнализToolStripMenuItem});
        	this.анализToolStripMenuItem.Name = "анализToolStripMenuItem";
        	this.анализToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
        	this.анализToolStripMenuItem.Text = "Анализ";
        	// 
        	// вернутьсяКНачальномуСостояниюToolStripMenuItem
        	// 
        	this.вернутьсяКНачальномуСостояниюToolStripMenuItem.Name = "вернутьсяКНачальномуСостояниюToolStripMenuItem";
        	this.вернутьсяКНачальномуСостояниюToolStripMenuItem.Size = new System.Drawing.Size(395, 22);
        	this.вернутьсяКНачальномуСостояниюToolStripMenuItem.Text = "Вернуться к начальному состоянию";
        	this.вернутьсяКНачальномуСостояниюToolStripMenuItem.Click += new System.EventHandler(this.вернутьсяКНачальномуСостояниюToolStripMenuItem_Click);
        	// 
        	// сброситьРезультатыТекущегоПоискаToolStripMenuItem
        	// 
        	this.сброситьРезультатыТекущегоПоискаToolStripMenuItem.Name = "сброситьРезультатыТекущегоПоискаToolStripMenuItem";
        	this.сброситьРезультатыТекущегоПоискаToolStripMenuItem.Size = new System.Drawing.Size(395, 22);
        	this.сброситьРезультатыТекущегоПоискаToolStripMenuItem.Text = "Сбросить результаты текущего поиска";
        	this.сброситьРезультатыТекущегоПоискаToolStripMenuItem.Click += new System.EventHandler(this.сброситьРезультатыТекущегоПоискаToolStripMenuItem_Click);
        	// 
        	// сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem
        	// 
        	this.сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Name = "сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem";
        	this.сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Size = new System.Drawing.Size(395, 22);
        	this.сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Text = "Сохранять результаты поиска при переключении вкладок";
        	this.сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem.Click += new System.EventHandler(this.сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem_Click);
        	// 
        	// многоуровневыйАнализToolStripMenuItem
        	// 
        	this.многоуровневыйАнализToolStripMenuItem.Name = "многоуровневыйАнализToolStripMenuItem";
        	this.многоуровневыйАнализToolStripMenuItem.Size = new System.Drawing.Size(395, 22);
        	this.многоуровневыйАнализToolStripMenuItem.Text = "Многоуровневый анализ...";
        	this.многоуровневыйАнализToolStripMenuItem.Click += new System.EventHandler(this.многоуровневыйАнализToolStripMenuItem_Click);
        	// 
        	// видToolStripMenuItem
        	// 
        	this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.автоподборВысотыСтрокToolStripMenuItem,
			this.автоподборШириныСтолбцовToolStripMenuItem,
			this.автоподборВысотыЗаголовковToolStripMenuItem,
			this.автоподборШириныЗаголовковToolStripMenuItem});
        	this.видToolStripMenuItem.Name = "видToolStripMenuItem";
        	this.видToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
        	this.видToolStripMenuItem.Text = "Вид";
        	// 
        	// автоподборВысотыСтрокToolStripMenuItem
        	// 
        	this.автоподборВысотыСтрокToolStripMenuItem.CheckOnClick = true;
        	this.автоподборВысотыСтрокToolStripMenuItem.Name = "автоподборВысотыСтрокToolStripMenuItem";
        	this.автоподборВысотыСтрокToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
        	this.автоподборВысотыСтрокToolStripMenuItem.Text = "Автоподбор высоты всех строк";
        	this.автоподборВысотыСтрокToolStripMenuItem.Click += new System.EventHandler(this.автоподборВысотыСтрокToolStripMenuItem_Click);
        	// 
        	// автоподборШириныСтолбцовToolStripMenuItem
        	// 
        	this.автоподборШириныСтолбцовToolStripMenuItem.CheckOnClick = true;
        	this.автоподборШириныСтолбцовToolStripMenuItem.Name = "автоподборШириныСтолбцовToolStripMenuItem";
        	this.автоподборШириныСтолбцовToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
        	this.автоподборШириныСтолбцовToolStripMenuItem.Text = "Автоподбор ширины всех столбцов";
        	this.автоподборШириныСтолбцовToolStripMenuItem.Click += new System.EventHandler(this.автоподборШириныСтолбцовToolStripMenuItem_Click);
        	// 
        	// автоподборВысотыЗаголовковToolStripMenuItem
        	// 
        	this.автоподборВысотыЗаголовковToolStripMenuItem.CheckOnClick = true;
        	this.автоподборВысотыЗаголовковToolStripMenuItem.Name = "автоподборВысотыЗаголовковToolStripMenuItem";
        	this.автоподборВысотыЗаголовковToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
        	this.автоподборВысотыЗаголовковToolStripMenuItem.Text = "Автоподбор высоты заголовков";
        	this.автоподборВысотыЗаголовковToolStripMenuItem.Click += new System.EventHandler(this.автоподборВысотыЗаголовковToolStripMenuItem_Click);
        	// 
        	// автоподборШириныЗаголовковToolStripMenuItem
        	// 
        	this.автоподборШириныЗаголовковToolStripMenuItem.CheckOnClick = true;
        	this.автоподборШириныЗаголовковToolStripMenuItem.Name = "автоподборШириныЗаголовковToolStripMenuItem";
        	this.автоподборШириныЗаголовковToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
        	this.автоподборШириныЗаголовковToolStripMenuItem.Text = "Автоподбор ширины заголовков";
        	this.автоподборШириныЗаголовковToolStripMenuItem.Click += new System.EventHandler(this.автоподборШириныЗаголовковToolStripMenuItem_Click);
        	// 
        	// statusStrip1
        	// 
        	this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripStatusLabel1,
			this.toolStripProgressBar1,
			this.toolStripStatusLabel2});
        	this.statusStrip1.Location = new System.Drawing.Point(0, 308);
        	this.statusStrip1.Name = "statusStrip1";
        	this.statusStrip1.Size = new System.Drawing.Size(641, 22);
        	this.statusStrip1.TabIndex = 5;
        	this.statusStrip1.Text = "statusStrip1";
        	// 
        	// toolStripStatusLabel1
        	// 
        	this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        	this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
        	// 
        	// toolStripProgressBar1
        	// 
        	this.toolStripProgressBar1.MarqueeAnimationSpeed = 20;
        	this.toolStripProgressBar1.Name = "toolStripProgressBar1";
        	this.toolStripProgressBar1.Size = new System.Drawing.Size(150, 16);
        	this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
        	this.toolStripProgressBar1.Visible = false;
        	// 
        	// toolStripStatusLabel2
        	// 
        	this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
        	this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
        	this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        	this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
        	this.toolStripStatusLabel2.MouseHover += new System.EventHandler(this.toolStripStatusLabel2_MouseHover);
        	// 
        	// saveFileDialog1
        	// 
        	this.saveFileDialog1.DefaultExt = "*.xls";
        	this.saveFileDialog1.FilterIndex = 0;
        	this.saveFileDialog1.Title = "Выберите место сохранения";
        	// 
        	// contextMenuStrip1
        	// 
        	this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.типПолейСтолбцаToolStripMenuItem,
			this.списокУникальныхЗначенийToolStripMenuItem,
			this.пакетныйЗаменительToolStripMenuItem,
			this.скрытьСтолбецToolStripMenuItem});
        	this.contextMenuStrip1.Name = "contextMenuStrip1";
        	this.contextMenuStrip1.Size = new System.Drawing.Size(249, 114);
        	// 
        	// типПолейСтолбцаToolStripMenuItem
        	// 
        	this.типПолейСтолбцаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.числоToolStripMenuItem,
			this.датаToolStripMenuItem,
			this.текстпоУмолчаниюToolStripMenuItem});
        	this.типПолейСтолбцаToolStripMenuItem.Name = "типПолейСтолбцаToolStripMenuItem";
        	this.типПолейСтолбцаToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
        	this.типПолейСтолбцаToolStripMenuItem.Text = "Тип полей столбца";
        	// 
        	// числоToolStripMenuItem
        	// 
        	this.числоToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.задатьОграниченияToolStripMenuItem});
        	this.числоToolStripMenuItem.Name = "числоToolStripMenuItem";
        	this.числоToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
        	this.числоToolStripMenuItem.Text = "Число";
        	this.числоToolStripMenuItem.Click += new System.EventHandler(this.числоToolStripMenuItem_Click);
        	// 
        	// задатьОграниченияToolStripMenuItem
        	// 
        	this.задатьОграниченияToolStripMenuItem.Name = "задатьОграниченияToolStripMenuItem";
        	this.задатьОграниченияToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
        	this.задатьОграниченияToolStripMenuItem.Text = "Задать ограничения...";
        	this.задатьОграниченияToolStripMenuItem.Click += new System.EventHandler(this.задатьОграниченияToolStripMenuItem_Click);
        	// 
        	// датаToolStripMenuItem
        	// 
        	this.датаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.ограничитьДиапазонToolStripMenuItem});
        	this.датаToolStripMenuItem.Name = "датаToolStripMenuItem";
        	this.датаToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
        	this.датаToolStripMenuItem.Text = "Дата";
        	this.датаToolStripMenuItem.Click += new System.EventHandler(this.датаToolStripMenuItem_Click);
        	// 
        	// ограничитьДиапазонToolStripMenuItem
        	// 
        	this.ограничитьДиапазонToolStripMenuItem.Name = "ограничитьДиапазонToolStripMenuItem";
        	this.ограничитьДиапазонToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
        	this.ограничитьДиапазонToolStripMenuItem.Text = "Ограничить диапазон...";
        	this.ограничитьДиапазонToolStripMenuItem.Click += new System.EventHandler(this.ограничитьДиапазонToolStripMenuItem_Click);
        	// 
        	// текстпоУмолчаниюToolStripMenuItem
        	// 
        	this.текстпоУмолчаниюToolStripMenuItem.Checked = true;
        	this.текстпоУмолчаниюToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.текстпоУмолчаниюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.поискToolStripMenuItem});
        	this.текстпоУмолчаниюToolStripMenuItem.Name = "текстпоУмолчаниюToolStripMenuItem";
        	this.текстпоУмолчаниюToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
        	this.текстпоУмолчаниюToolStripMenuItem.Text = "Текст (по умолчанию)";
        	this.текстпоУмолчаниюToolStripMenuItem.Click += new System.EventHandler(this.текстпоУмолчаниюToolStripMenuItem_Click);
        	// 
        	// поискToolStripMenuItem
        	// 
        	this.поискToolStripMenuItem.Name = "поискToolStripMenuItem";
        	this.поискToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
        	this.поискToolStripMenuItem.Text = "Поиск...";
        	this.поискToolStripMenuItem.Click += new System.EventHandler(this.поискToolStripMenuItem_Click);
        	// 
        	// списокУникальныхЗначенийToolStripMenuItem
        	// 
        	this.списокУникальныхЗначенийToolStripMenuItem.Name = "списокУникальныхЗначенийToolStripMenuItem";
        	this.списокУникальныхЗначенийToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
        	this.списокУникальныхЗначенийToolStripMenuItem.Text = "Список уникальных значений...";
        	this.списокУникальныхЗначенийToolStripMenuItem.Click += new System.EventHandler(this.списокУникальныхЗначенийToolStripMenuItem_Click);
        	// 
        	// пакетныйЗаменительToolStripMenuItem
        	// 
        	this.пакетныйЗаменительToolStripMenuItem.Name = "пакетныйЗаменительToolStripMenuItem";
        	this.пакетныйЗаменительToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
        	this.пакетныйЗаменительToolStripMenuItem.Text = "Пакетный корректор записей";
        	this.пакетныйЗаменительToolStripMenuItem.Click += new System.EventHandler(this.пакетныйЗаменительToolStripMenuItem_Click);
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
        	this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	this.tabControl1.Controls.Add(this.tabPage1);
        	this.tabControl1.Location = new System.Drawing.Point(5, 30);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(631, 22);
        	this.tabControl1.TabIndex = 6;
        	this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
        	// 
        	// tabPage1
        	// 
        	this.tabPage1.Location = new System.Drawing.Point(4, 4);
        	this.tabPage1.Name = "tabPage1";
        	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage1.Size = new System.Drawing.Size(623, 0);
        	this.tabPage1.TabIndex = 0;
        	this.tabPage1.UseVisualStyleBackColor = true;
        	// 
        	// скрытьСтолбецToolStripMenuItem
        	// 
        	this.скрытьСтолбецToolStripMenuItem.Name = "скрытьСтолбецToolStripMenuItem";
        	this.скрытьСтолбецToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
        	this.скрытьСтолбецToolStripMenuItem.Text = "Скрыть столбец";
        	this.скрытьСтолбецToolStripMenuItem.Click += new System.EventHandler(this.скрытьСтолбецToolStripMenuItem_Click);
        	// 
        	// MainForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(641, 330);
        	this.Controls.Add(this.tabControl1);
        	this.Controls.Add(this.statusStrip1);
        	this.Controls.Add(this.MaindataGrid);
        	this.Controls.Add(this.menuStrip1);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.MainMenuStrip = this.menuStrip1;
        	this.Name = "MainForm";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "Статико-аналитический терминал \"Стазис\"";
        	this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        	this.Load += new System.EventHandler(this.Form1_Load);
        	((System.ComponentModel.ISupportInitialize)(this.MaindataGrid)).EndInit();
        	this.menuStrip1.ResumeLayout(false);
        	this.menuStrip1.PerformLayout();
        	this.statusStrip1.ResumeLayout(false);
        	this.statusStrip1.PerformLayout();
        	this.contextMenuStrip1.ResumeLayout(false);
        	this.tabControl1.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem сменитьИсточникДанныхToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem анализToolStripMenuItem;
        public System.Windows.Forms.DataGridView MaindataGrid;
        private System.Windows.Forms.ToolStripMenuItem вернутьсяКНачальномуСостояниюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem экспортВExcelToolStripMenuItem;
        public System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem типПолейСтолбцаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem числоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem датаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem текстпоУмолчаниюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem задатьОграниченияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ограничитьДиапазонToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поискToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripMenuItem списокУникальныхЗначенийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сброситьРезультатыТекущегоПоискаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пакетныйЗаменительToolStripMenuItem;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem автоподборВысотыСтрокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem автоподборШириныСтолбцовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem автоподборВысотыЗаголовковToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem автоподборШириныЗаголовковToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранятьРезультатыПоискаПриПереключенииВкладокToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem многоуровневыйАнализToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem скрытьСтолбецToolStripMenuItem;
    }
}

