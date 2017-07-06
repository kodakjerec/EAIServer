namespace SocketTest
{
    partial class Form1
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txb_IP = new System.Windows.Forms.TextBox();
            this.txb_Port = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.txb_Message = new System.Windows.Forms.TextBox();
            this.txb_log = new System.Windows.Forms.TextBox();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.btn_LoopSend = new System.Windows.Forms.Button();
            this.txb_Time = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_LoopSendStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_ShowCreatePanel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txb_IP
            // 
            this.txb_IP.Location = new System.Drawing.Point(67, 0);
            this.txb_IP.Margin = new System.Windows.Forms.Padding(2);
            this.txb_IP.Name = "txb_IP";
            this.txb_IP.Size = new System.Drawing.Size(126, 27);
            this.txb_IP.TabIndex = 0;
            this.txb_IP.Text = "192.168.120.162";
            // 
            // txb_Port
            // 
            this.txb_Port.Location = new System.Drawing.Point(197, 0);
            this.txb_Port.Margin = new System.Windows.Forms.Padding(2);
            this.txb_Port.Name = "txb_Port";
            this.txb_Port.Size = new System.Drawing.Size(44, 27);
            this.txb_Port.TabIndex = 1;
            this.txb_Port.Text = "1812";
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(159, 436);
            this.btn_Send.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(82, 27);
            this.btn_Send.TabIndex = 2;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.button1_Click);
            // 
            // txb_Message
            // 
            this.txb_Message.Location = new System.Drawing.Point(3, 72);
            this.txb_Message.Margin = new System.Windows.Forms.Padding(2);
            this.txb_Message.Multiline = true;
            this.txb_Message.Name = "txb_Message";
            this.txb_Message.Size = new System.Drawing.Size(318, 329);
            this.txb_Message.TabIndex = 3;
            this.txb_Message.Text = "CMD_list";
            // 
            // txb_log
            // 
            this.txb_log.Dock = System.Windows.Forms.DockStyle.Right;
            this.txb_log.Location = new System.Drawing.Point(326, 0);
            this.txb_log.Margin = new System.Windows.Forms.Padding(2);
            this.txb_log.Multiline = true;
            this.txb_log.Name = "txb_log";
            this.txb_log.Size = new System.Drawing.Size(466, 464);
            this.txb_log.TabIndex = 4;
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(3, 403);
            this.btn_Connect.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(70, 29);
            this.btn_Connect.TabIndex = 5;
            this.btn_Connect.Text = "連線";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_LoopSend
            // 
            this.btn_LoopSend.Enabled = false;
            this.btn_LoopSend.Location = new System.Drawing.Point(159, 406);
            this.btn_LoopSend.Margin = new System.Windows.Forms.Padding(2);
            this.btn_LoopSend.Name = "btn_LoopSend";
            this.btn_LoopSend.Size = new System.Drawing.Size(82, 26);
            this.btn_LoopSend.TabIndex = 6;
            this.btn_LoopSend.Text = "迴圈Send";
            this.btn_LoopSend.UseVisualStyleBackColor = true;
            this.btn_LoopSend.Click += new System.EventHandler(this.btn_LoopSend_Click);
            // 
            // txb_Time
            // 
            this.txb_Time.Location = new System.Drawing.Point(29, 437);
            this.txb_Time.Margin = new System.Windows.Forms.Padding(2);
            this.txb_Time.Name = "txb_Time";
            this.txb_Time.Size = new System.Drawing.Size(25, 27);
            this.txb_Time.TabIndex = 7;
            this.txb_Time.Text = "3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 441);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "每";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 440);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "s循環一次";
            // 
            // btn_LoopSendStop
            // 
            this.btn_LoopSendStop.Enabled = false;
            this.btn_LoopSendStop.Location = new System.Drawing.Point(245, 406);
            this.btn_LoopSendStop.Margin = new System.Windows.Forms.Padding(2);
            this.btn_LoopSendStop.Name = "btn_LoopSendStop";
            this.btn_LoopSendStop.Size = new System.Drawing.Size(74, 26);
            this.btn_LoopSendStop.TabIndex = 10;
            this.btn_LoopSendStop.Text = "Stop";
            this.btn_LoopSendStop.UseVisualStyleBackColor = true;
            this.btn_LoopSendStop.Click += new System.EventHandler(this.btn_LoopSendStop_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "對象IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "命令列表";
            // 
            // btn_ShowCreatePanel
            // 
            this.btn_ShowCreatePanel.Location = new System.Drawing.Point(216, 40);
            this.btn_ShowCreatePanel.Name = "btn_ShowCreatePanel";
            this.btn_ShowCreatePanel.Size = new System.Drawing.Size(105, 30);
            this.btn_ShowCreatePanel.TabIndex = 13;
            this.btn_ShowCreatePanel.Text = "命令產生器";
            this.btn_ShowCreatePanel.UseVisualStyleBackColor = true;
            this.btn_ShowCreatePanel.Click += new System.EventHandler(this.btn_ShowCreatePanel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(792, 464);
            this.Controls.Add(this.btn_ShowCreatePanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_LoopSendStop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txb_Time);
            this.Controls.Add(this.btn_LoopSend);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.txb_log);
            this.Controls.Add(this.txb_Message);
            this.Controls.Add(this.btn_Send);
            this.Controls.Add(this.txb_Port);
            this.Controls.Add(this.txb_IP);
            this.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "測試視窗";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txb_IP;
        private System.Windows.Forms.TextBox txb_Port;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox txb_Message;
        private System.Windows.Forms.TextBox txb_log;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Button btn_LoopSend;
        private System.Windows.Forms.TextBox txb_Time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_LoopSendStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_ShowCreatePanel;
    }
}

