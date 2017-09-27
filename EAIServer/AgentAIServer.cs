using System;
using System.Windows.Forms;
using AgentAIServer.MyService;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Diagnostics;

namespace AgentAIServer
{
    /*
     * GUI介面
     * 定期整理訊息給使用者看
     * 
     * 設定類別：
     */
    public partial class AgentAIServer : Form
    {
        List<Icon> iconList = new List<Icon>();


        public AgentAIServer()
        {
            InitializeComponent();
        }

        #region NotifyIcon處理
        /// <summary>
        /// Form 出現, NotifyIcon消失
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NotifyIconDisappear_ShowForm();
        }
        private void notifyIcon_toolStripTextBox2_Click(object sender, EventArgs e)
        {
            NotifyIconDisappear_ShowForm();
        }
        private void NotifyIconDisappear_ShowForm()
        {
            notifyIcon1.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// 離開程式
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_toolStripTextBox1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }
        #endregion

        #region Form處理
        private void Form1_Load(object sender, EventArgs e)
        {
            //Icon納入記憶體
            string[] filelist = Directory.GetFiles("Pic\\", "*.ico", SearchOption.TopDirectoryOnly);
            foreach (string filepath in filelist)
            {
                iconList.Add(new Icon(filepath));
            }

            //標題文字
            this.Text += " 本機名稱=" + Program.hostName + " IP=" + MyCookies.host.ToString() + ":" + MyCookies.Port.ToString();
            notifyIcon1.Text = this.Text;

            //計時
            timer1.Start();

            toolStripBtn_refresh.PerformClick();
        }

        /// <summary>
        /// Form 最小化, NotifyIcon出現
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EAIServer_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        #region 前端訊息
        static int timer1_tick = 1;
        /// <summary>
        /// 前端重新整理timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            txb_ReceiveMsg.Text = Program.server.OutputReceive();
            txb_SendMsg.Text = Program.server.OutputSend();
            txb_ScheduleStatus.Text = ScheduleCounter.OutputReceive();

            string lbl_ScheduleItems = "排程 預定執行：" + Environment.NewLine;
            try
            {
                lbl_ScheduleItems += 
                      "PK"+ Repeat("",40)
                    + "|下次" + Repeat("", 20) 
                    + "|筆數"+ Repeat("", 10) 
                    + "|時間"+ Repeat("", 12) 
                    + "|回傳" 
                    + Environment.NewLine;
                lbl_ScheduleItems += Repeat("",110,"-") + Environment.NewLine;
                foreach (Schedule_Item item in ScheduleCounter.scheduleItems)
                {
                    lbl_ScheduleItems +=
                        Repeat(item.PK,42)
                        + "|" + Repeat(item.NextTime.ToString("yyyy/MM/dd HH:mm:ss"),24)
                        + "|" + Repeat(item.requestRecord + "<=" + item.record,14)
                        + "|" + Repeat(item.TotalWaitingTime + "<=" + item.longwait,14)
                        + "|" + Repeat(item.Flag_IGotMessage.ToString(),16)
                        + Environment.NewLine;
                }

                lbl_ScheduleItems += Environment.NewLine + "命令 執行緒：" + Environment.NewLine;
                foreach (Thread thread in ServerCounter.serverThreads)
                {
                    lbl_ScheduleItems +=
                          "PK:" + thread.Name
                        + " 狀態:" + thread.ThreadState
                        + Environment.NewLine;
                }

                lbl_ScheduleItems += Environment.NewLine + "Message 列表：" + Environment.NewLine;
                foreach (ClientMessage item in ClientMessageQueue.clientMsgQueue)
                {
                    lbl_ScheduleItems +=
                          "IP:" + item.IP
                        + " Msg:" + item.Message
                        + Environment.NewLine;
                }
            }
            catch { }

            timer1_tick = (timer1_tick + 1) % iconList.Count;
            this.Icon = iconList[timer1_tick];
            notifyIcon1.Icon = this.Icon;

            label1.Text = lbl_ScheduleItems;
            lbl_ScheduleItems = null;
        }
        private string Repeat(string str, int count)
        {
            int i = str.Length;

            while (i <= count) {
                str += " ";
                i++;
            }

            return str;
        }
        private string Repeat(string str, int count,string InsertChar)
        {
            int i = str.Length;

            while (i <= count)
            {
                str += InsertChar;
                i++;
            }

            return str;
        }
        #endregion

        /// <summary>
        /// 終結所有執行緒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgentAIServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        #endregion

        /// <summary>
        /// 重整畫面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            ServerCounter.ShareClientSocket.Send("resetall");

            //設定檔
            dGV_Database.DataSource = ServerCounter.dt_Database;
            dGV_Schedule.DataSource = ServerCounter.dt_Schedule;
            dGV_Agent.DataSource = DB_IO.JSONconvert.ListToDataTable<Settings_Agent_Item>(ServerCounter.Settings_Agent);
            dGV_AIML.DataSource = DB_IO.JSONconvert.ListToDataTable<Settings_AIML_Item>(ServerCounter.Settings_AIML);

            //系統設定文字
            lbl_MyCookies.Text =
                  "連線逾時限制: " + MyCookies.ConnectTimeOut.ToString() + Environment.NewLine
                + "本機host: " + MyCookies.host.ToString() + Environment.NewLine
                + "預設Port: " + MyCookies.Port.ToString() + Environment.NewLine
                + "紀錄log: " + MyCookies.Log.ToString() + Environment.NewLine
                + "排程啟動: " + MyCookies.ScheduleOn.ToString() + Environment.NewLine;

            tabControl1.SelectedIndex = 0;
            txb_ScheduleStatus.Text = "重整完畢";
        }

        /// <summary>
        /// 儲存設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtn_Save_Click(object sender, EventArgs e)
        {
            ServiceFunctions service = new ServiceFunctions();

            //排程
            ServerCounter.dt_Schedule = (DataTable)dGV_Schedule.DataSource;
            service.setList_Schedule();

            //資料庫
            ServerCounter.dt_Database = (DataTable)dGV_Database.DataSource;
            service.setList_database();

            //代理人
            ServerCounter.Settings_Agent = DB_IO.JSONconvert.DataTableToList<Settings_Agent_Item>((DataTable)dGV_Agent.DataSource);
            service.setList_Agent();

            //命令
            ServerCounter.Settings_AIML = DB_IO.JSONconvert.DataTableToList<Settings_AIML_Item>((DataTable)dGV_AIML.DataSource);
            service.setList_AIML();

            MessageBox.Show("儲存完畢", "成功");
        }

        private void toolStripButton_restart_Click(object sender, EventArgs e)
        {
            ServerCounter.ShareClientSocket.Send("restart");
        }
    }
}
