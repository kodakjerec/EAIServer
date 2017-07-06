namespace AgentAIServer
{
    partial class AgentAIServer
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentAIServer));
            this.tbC_Main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txb_ScheduleStatus = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txb_ReceiveMsg = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txb_SendMsg = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.dGV_Schedule = new System.Windows.Forms.DataGridView();
            this.col_Schedule_PK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_DB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_longwait = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_record = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_longdisconnect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_request = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_Schedule_ScheduleSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.dGV_Database = new System.Windows.Forms.DataGridView();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.dGV_Agent = new System.Windows.Forms.DataGridView();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.dGV_AIML = new System.Windows.Forms.DataGridView();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.lbl_MyCookies = new System.Windows.Forms.Label();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripBtn_refresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtn_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_restart = new System.Windows.Forms.ToolStripButton();
            this.tbC_Main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Schedule)).BeginInit();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Database)).BeginInit();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Agent)).BeginInit();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AIML)).BeginInit();
            this.tabPage11.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbC_Main
            // 
            this.tbC_Main.Controls.Add(this.tabPage1);
            this.tbC_Main.Controls.Add(this.tabPage2);
            this.tbC_Main.Controls.Add(this.tabPage10);
            this.tbC_Main.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbC_Main.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbC_Main.Location = new System.Drawing.Point(0, 25);
            this.tbC_Main.Name = "tbC_Main";
            this.tbC_Main.SelectedIndex = 0;
            this.tbC_Main.Size = new System.Drawing.Size(792, 537);
            this.tbC_Main.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(784, 507);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "訊息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(778, 501);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txb_ScheduleStatus);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(770, 471);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "排程";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txb_ScheduleStatus
            // 
            this.txb_ScheduleStatus.AutoSize = true;
            this.txb_ScheduleStatus.Location = new System.Drawing.Point(1, 1);
            this.txb_ScheduleStatus.Name = "txb_ScheduleStatus";
            this.txb_ScheduleStatus.Size = new System.Drawing.Size(46, 16);
            this.txb_ScheduleStatus.TabIndex = 0;
            this.txb_ScheduleStatus.Text = "label2";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txb_ReceiveMsg);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(770, 471);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "接收";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txb_ReceiveMsg
            // 
            this.txb_ReceiveMsg.AutoSize = true;
            this.txb_ReceiveMsg.Location = new System.Drawing.Point(1, 1);
            this.txb_ReceiveMsg.Name = "txb_ReceiveMsg";
            this.txb_ReceiveMsg.Size = new System.Drawing.Size(46, 16);
            this.txb_ReceiveMsg.TabIndex = 1;
            this.txb_ReceiveMsg.Text = "label2";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txb_SendMsg);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(770, 471);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "傳送";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txb_SendMsg
            // 
            this.txb_SendMsg.AutoSize = true;
            this.txb_SendMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txb_SendMsg.Location = new System.Drawing.Point(0, 0);
            this.txb_SendMsg.Name = "txb_SendMsg";
            this.txb_SendMsg.Size = new System.Drawing.Size(46, 16);
            this.txb_SendMsg.TabIndex = 1;
            this.txb_SendMsg.Text = "label2";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(784, 507);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "設定";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Controls.Add(this.tabPage11);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(778, 501);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.dGV_Schedule);
            this.tabPage6.Location = new System.Drawing.Point(4, 26);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(770, 471);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "排程";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // dGV_Schedule
            // 
            this.dGV_Schedule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dGV_Schedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Schedule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_Schedule_PK,
            this.col_Schedule_DB,
            this.col_Schedule_period,
            this.col_Schedule_longwait,
            this.col_Schedule_record,
            this.col_Schedule_longdisconnect,
            this.col_Schedule_request,
            this.col_Schedule_action,
            this.col_Schedule_target,
            this.col_Schedule_ScheduleSet});
            this.dGV_Schedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Schedule.Location = new System.Drawing.Point(3, 3);
            this.dGV_Schedule.Name = "dGV_Schedule";
            this.dGV_Schedule.RowTemplate.Height = 24;
            this.dGV_Schedule.Size = new System.Drawing.Size(764, 465);
            this.dGV_Schedule.TabIndex = 1;
            // 
            // col_Schedule_PK
            // 
            this.col_Schedule_PK.DataPropertyName = "pk";
            this.col_Schedule_PK.HeaderText = "PK";
            this.col_Schedule_PK.Name = "col_Schedule_PK";
            this.col_Schedule_PK.Width = 52;
            // 
            // col_Schedule_DB
            // 
            this.col_Schedule_DB.DataPropertyName = "DB";
            this.col_Schedule_DB.HeaderText = "連結資料庫";
            this.col_Schedule_DB.Name = "col_Schedule_DB";
            this.col_Schedule_DB.Width = 89;
            // 
            // col_Schedule_period
            // 
            this.col_Schedule_period.DataPropertyName = "period";
            this.col_Schedule_period.HeaderText = "執行一次(秒)";
            this.col_Schedule_period.Name = "col_Schedule_period";
            this.col_Schedule_period.Width = 94;
            // 
            // col_Schedule_longwait
            // 
            this.col_Schedule_longwait.DataPropertyName = "longwait";
            this.col_Schedule_longwait.HeaderText = "無回應等待時間";
            this.col_Schedule_longwait.Name = "col_Schedule_longwait";
            this.col_Schedule_longwait.Width = 104;
            // 
            // col_Schedule_record
            // 
            this.col_Schedule_record.DataPropertyName = "record";
            this.col_Schedule_record.HeaderText = "滿足筆數";
            this.col_Schedule_record.Name = "col_Schedule_record";
            this.col_Schedule_record.Width = 75;
            // 
            // col_Schedule_longdisconnect
            // 
            this.col_Schedule_longdisconnect.DataPropertyName = "longdisconnect";
            this.col_Schedule_longdisconnect.HeaderText = "斷線等待時間";
            this.col_Schedule_longdisconnect.Name = "col_Schedule_longdisconnect";
            this.col_Schedule_longdisconnect.Width = 89;
            // 
            // col_Schedule_request
            // 
            this.col_Schedule_request.DataPropertyName = "request";
            this.col_Schedule_request.HeaderText = "詢問命令";
            this.col_Schedule_request.Name = "col_Schedule_request";
            this.col_Schedule_request.Width = 75;
            // 
            // col_Schedule_action
            // 
            this.col_Schedule_action.DataPropertyName = "action";
            this.col_Schedule_action.HeaderText = "動作命令";
            this.col_Schedule_action.Name = "col_Schedule_action";
            this.col_Schedule_action.Width = 75;
            // 
            // col_Schedule_target
            // 
            this.col_Schedule_target.DataPropertyName = "target";
            this.col_Schedule_target.HeaderText = "回應給誰(代理人)";
            this.col_Schedule_target.Name = "col_Schedule_target";
            this.col_Schedule_target.Width = 108;
            // 
            // col_Schedule_ScheduleSet
            // 
            this.col_Schedule_ScheduleSet.DataPropertyName = "ScheduleSet";
            this.col_Schedule_ScheduleSet.HeaderText = "排程日期設定";
            this.col_Schedule_ScheduleSet.Name = "col_Schedule_ScheduleSet";
            this.col_Schedule_ScheduleSet.Width = 89;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.dGV_Database);
            this.tabPage7.Location = new System.Drawing.Point(4, 26);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(770, 471);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "資料庫";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // dGV_Database
            // 
            this.dGV_Database.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dGV_Database.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Database.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Database.Location = new System.Drawing.Point(3, 3);
            this.dGV_Database.Name = "dGV_Database";
            this.dGV_Database.RowTemplate.Height = 24;
            this.dGV_Database.Size = new System.Drawing.Size(764, 465);
            this.dGV_Database.TabIndex = 3;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.dGV_Agent);
            this.tabPage8.Location = new System.Drawing.Point(4, 26);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(770, 471);
            this.tabPage8.TabIndex = 2;
            this.tabPage8.Text = "代理人";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // dGV_Agent
            // 
            this.dGV_Agent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dGV_Agent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Agent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Agent.Location = new System.Drawing.Point(0, 0);
            this.dGV_Agent.Name = "dGV_Agent";
            this.dGV_Agent.RowTemplate.Height = 24;
            this.dGV_Agent.Size = new System.Drawing.Size(770, 471);
            this.dGV_Agent.TabIndex = 2;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.dGV_AIML);
            this.tabPage9.Location = new System.Drawing.Point(4, 26);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new System.Drawing.Size(770, 471);
            this.tabPage9.TabIndex = 3;
            this.tabPage9.Text = "命令";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // dGV_AIML
            // 
            this.dGV_AIML.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dGV_AIML.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_AIML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_AIML.Location = new System.Drawing.Point(0, 0);
            this.dGV_AIML.Name = "dGV_AIML";
            this.dGV_AIML.RowTemplate.Height = 24;
            this.dGV_AIML.Size = new System.Drawing.Size(770, 471);
            this.dGV_AIML.TabIndex = 2;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.lbl_MyCookies);
            this.tabPage11.Location = new System.Drawing.Point(4, 26);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(770, 471);
            this.tabPage11.TabIndex = 4;
            this.tabPage11.Text = "系統設定";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // lbl_MyCookies
            // 
            this.lbl_MyCookies.AutoSize = true;
            this.lbl_MyCookies.Location = new System.Drawing.Point(6, 3);
            this.lbl_MyCookies.Name = "lbl_MyCookies";
            this.lbl_MyCookies.Size = new System.Drawing.Size(46, 16);
            this.lbl_MyCookies.TabIndex = 1;
            this.lbl_MyCookies.Text = "label2";
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.label1);
            this.tabPage10.Location = new System.Drawing.Point(4, 26);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new System.Drawing.Size(784, 507);
            this.tabPage10.TabIndex = 2;
            this.tabPage10.Text = "動態";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox2,
            this.toolStripTextBox1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 54);
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox2.Text = "正常視窗";
            this.toolStripTextBox2.Click += new System.EventHandler(this.notifyIcon_toolStripTextBox2_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Text = "離開";
            this.toolStripTextBox1.Click += new System.EventHandler(this.notifyIcon_toolStripTextBox1_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtn_refresh,
            this.toolStripBtn_Save,
            this.toolStripButton_restart});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripBtn_refresh
            // 
            this.toolStripBtn_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn_refresh.Name = "toolStripBtn_refresh";
            this.toolStripBtn_refresh.Size = new System.Drawing.Size(84, 22);
            this.toolStripBtn_refresh.Text = "重新整理畫面";
            this.toolStripBtn_refresh.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripBtn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // toolStripBtn_Save
            // 
            this.toolStripBtn_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtn_Save.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtn_Save.Image")));
            this.toolStripBtn_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtn_Save.Name = "toolStripBtn_Save";
            this.toolStripBtn_Save.Size = new System.Drawing.Size(60, 22);
            this.toolStripBtn_Save.Text = "儲存設定";
            this.toolStripBtn_Save.Click += new System.EventHandler(this.toolStripBtn_Save_Click);
            // 
            // toolStripButton_restart
            // 
            this.toolStripButton_restart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_restart.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_restart.Image")));
            this.toolStripButton_restart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_restart.Name = "toolStripButton_restart";
            this.toolStripButton_restart.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton_restart.Text = "重開啟";
            this.toolStripButton_restart.Click += new System.EventHandler(this.toolStripButton_restart_Click);
            // 
            // AgentAIServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.tbC_Main);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AgentAIServer";
            this.Text = "TAKOServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AgentAIServer_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.EAIServer_SizeChanged);
            this.tbC_Main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Schedule)).EndInit();
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Database)).EndInit();
            this.tabPage8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Agent)).EndInit();
            this.tabPage9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AIML)).EndInit();
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.tabPage10.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbC_Main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.DataGridView dGV_Schedule;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.DataGridView dGV_Database;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.DataGridView dGV_Agent;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.DataGridView dGV_AIML;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.Label txb_ScheduleStatus;
        private System.Windows.Forms.Label txb_ReceiveMsg;
        private System.Windows.Forms.Label txb_SendMsg;
        private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.Label lbl_MyCookies;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_PK;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_DB;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_period;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_longwait;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_record;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_longdisconnect;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_request;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_action;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_target;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_Schedule_ScheduleSet;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripBtn_refresh;
        private System.Windows.Forms.ToolStripButton toolStripBtn_Save;
        private System.Windows.Forms.ToolStripButton toolStripButton_restart;
    }
}

