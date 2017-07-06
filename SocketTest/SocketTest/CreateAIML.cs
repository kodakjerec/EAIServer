using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SocketTest
{
    /*
     * 產生指令的xml檔案
     * (滑鼠先點一下要更改的欄位，注意label變成紅色)
     * 下方可以選擇不同的命令類別
     * 
     * 1. ID:   系統之後判斷的關鍵字
     * 2. CMD:  系統真正執行的指令
     * 3. SUCCESS:  CMD成功後要執行的動作
     * 4. FAIL:     CMD發生意外要執行的動作
     */
    public partial class CreateAIML : Form
    {
        public CreateAIML()
        {
            InitializeComponent();
        }

        private void CreateAIML_Load(object sender, EventArgs e)
        {
            txb_CMD_MouseCaptureChanged(sender, e);
        }

        #region 命令產生器
        string TotalMsg = "";

        //顯示/關閉 產生器
        private void btn_ShowCreatePanel_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
        }

        //清空 產生器內容
        private void btn_ClearAll_Click(object sender, EventArgs e)
        {
            foreach (TabPage tabp in tabControl2.TabPages)
            {
                CleanContent<TabPage>(tabp);
            }
        }
        private void CleanContent<T>(Object obj)
        {
            string TypeName = "";

            if (typeof(T) == typeof(TabPage))
            {
                foreach (Control con in ((TabPage)obj).Controls)
                {
                    TypeName = con.GetType().Name.ToLower();
                    switch (TypeName)
                    {
                        case "textbox":
                            con.Text = "";
                            break;
                        case "tabpage":
                            CleanContent<TabPage>(con);
                            break;
                        case "groupbox":
                            CleanContent<GroupBox>(con);
                            break;
                        case "checkbox":
                            ((CheckBox)con).Checked = false;
                            break;
                    }
                }
            }
            else if (typeof(T) == typeof(GroupBox))
            {
                foreach (Control con in ((GroupBox)obj).Controls)
                {
                    TypeName = con.GetType().Name.ToLower();
                    switch (TypeName)
                    {
                        case "textbox":
                            con.Text = "";
                            break;
                        case "tabpage":
                            CleanContent<TabPage>(con);
                            break;
                        case "groupbox":
                            CleanContent<GroupBox>(con);
                            break;
                        case "checkbox":
                            ((CheckBox)con).Checked = false;
                            break;
                    }
                }
            }
        }

        //帶入訊息
        private void InputMsg()
        {
            ((TextBox)target_txbInput).Text = TotalMsg;
            TotalMsg = "";
        }

        #region Page_傳送訊息
        private void button1_Click(object sender, EventArgs e)
        {
            TotalMsg +=
                "sendmessage:" + txb_sendMessage_target.Text
                + " cmd:\"" + txb_SendMessage_cmd.Text + "\"";
            InputMsg();
        }
        #endregion

        #region Page_排程執行中
        private void button2_Click(object sender, EventArgs e)
        {
            string Msg = "schedulechange:" + txb_ScheduleChange_target.Text;
            if (ckB_ScheduleChange_1.Checked)
                Msg += " Flag_IGotMessage=1";
            if (ckB_ScheduleChange_2.Checked)
                Msg += " requestRecord=999";

            TotalMsg += Msg;
            InputMsg();
        }
        #endregion

        #region Page_排程設定
        private void button3_Click(object sender, EventArgs e)
        {
            string Msg = "";
            if (ckB_ScheduleSet_start.Checked)
            {
                Msg = "schedulestart:" + txb_ScheduleSet_Oripk.Text;
            }
            else if (ckB_ScheduleSet_stop.Checked)
            {
                Msg = "schedulestop:" + txb_ScheduleSet_Oripk.Text;
            }
            else if (ckB_ScheduleSet_delete.Checked)
            {
                Msg = "scheduledelete:" + txb_ScheduleSet_Oripk.Text;
            }
            else
            {
                Msg = "scheduleset:" + txb_ScheduleSet_Oripk.Text;
                if (txb_ScheduleSet_pk.Text != string.Empty)
                    Msg += " pk=" + txb_ScheduleSet_pk.Text;
                if (txb_ScheduleSet_db.Text != string.Empty)
                    Msg += " db=" + txb_ScheduleSet_db.Text;
                if (txb_ScheduleSet_period.Text != string.Empty)
                    Msg += " period=" + txb_ScheduleSet_period.Text;
                if (txb_ScheduleSet_longwait.Text != string.Empty)
                    Msg += " longwait=" + txb_ScheduleSet_longwait.Text;
                if (txb_ScheduleSet_record.Text != string.Empty)
                    Msg += " record=" + txb_ScheduleSet_record.Text;
                if (txb_ScheduleSet_longdisconnect.Text != string.Empty)
                    Msg += " longdisconnect=" + txb_ScheduleSet_longdisconnect.Text;
                if (txb_ScheduleSet_target.Text != string.Empty)
                    Msg += " target=" + txb_ScheduleSet_target.Text;
                if (txb_ScheduleSet_request.Text != string.Empty)
                    Msg += " request=" + txb_ScheduleSet_request.Text;
                if (txb_ScheduleSet_action.Text != string.Empty)
                    Msg += " action=" + txb_ScheduleSet_action.Text;

                string Msg_Scheduleset = " scheduleset=\"";
                if (txb_ScheduleSet_scheduleset_Day.Text != string.Empty
                    || txb_ScheduleSet_scheduleset_week.Text != string.Empty
                    || txb_ScheduleSet_scheduleset_starttime.Text != string.Empty
                    || txb_ScheduleSet_scheduleset_endtime.Text != string.Empty)
                {
                    if (txb_ScheduleSet_scheduleset_Day.Text != string.Empty)
                        Msg_Scheduleset += " Day:" + txb_ScheduleSet_scheduleset_Day.Text;
                    if (txb_ScheduleSet_scheduleset_week.Text != string.Empty)
                        Msg_Scheduleset += " week:" + txb_ScheduleSet_scheduleset_week.Text;
                    if (txb_ScheduleSet_scheduleset_starttime.Text != string.Empty)
                        Msg_Scheduleset += " StartTime:" + txb_ScheduleSet_scheduleset_starttime.Text;
                    if (txb_ScheduleSet_scheduleset_endtime.Text != string.Empty)
                        Msg_Scheduleset += " EndTime:" + txb_ScheduleSet_scheduleset_endtime.Text;
                }
                Msg_Scheduleset += "\"";
                Msg += Msg_Scheduleset;
            }
            TotalMsg += Msg;
            InputMsg();
        }

        /// <summary>
        /// 變更打勾類別
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckB_ScheduleSet_start_CheckedChanged(object sender, EventArgs e)
        {
            if (ckB_ScheduleSet_start.Checked
                || ckB_ScheduleSet_stop.Checked
                || ckB_ScheduleSet_delete.Checked)
            {
                groupBox3.Visible = false;
                groupBox1.Visible = false;
            }
            else
            {
                groupBox3.Visible = true;
                groupBox1.Visible = true;
            }
        }
        #endregion

        #region Page_資料庫設定
        private void button4_Click(object sender, EventArgs e)
        {
            string Msg = "";
            if (ckB_databaseset_delete.Checked)
            {
                Msg = "databasedelete:" + txb_databaseset_db.Text;
            }
            else
            {
                Msg = " databaseset:" + txb_databaseset_db.Text
                    + " \"" + txb_databaseset_connection.Text + "\"";
            }
            TotalMsg += Msg;
            InputMsg();
        }
        #endregion

        #region Page_代理人設定
        private void button5_Click(object sender, EventArgs e)
        {
            string Msg = "";
            if (ckB_agentset_delete.Checked)
            {
                Msg = "agentdelete:" + txb_agentset_Oripk.Text;
            }
            else
            {
                Msg = "agentset:" + txb_agentset_Oripk.Text;
                if (txb_agentset_pk.Text != string.Empty)
                    Msg += " pk=" + txb_agentset_pk.Text;
                if (txb_agentset_ip.Text != string.Empty)
                    Msg += " ip=\"" + txb_agentset_ip.Text + "\"";
                if (txb_agentset_port.Text != string.Empty)
                    Msg += " port=" + txb_agentset_port.Text;
                if (txb_agentset_memo.Text != string.Empty)
                    Msg += " memo=" + txb_agentset_memo.Text;

            }
            TotalMsg += Msg;
            InputMsg();
        }
        #endregion

        #region Page 執行sqlcmd
        private void button6_Click(object sender, EventArgs e)
        {
            string Msg = "";
            Msg += "sqlcmd:\"" + txb_sqlcmd_sqlcmd.Text + "\""
                + " db:" + txb_sqlcmd_db.Text;
            TotalMsg += Msg;
            InputMsg();
        }
        #endregion

        #region Page 執行EXE
        private void button7_Click(object sender, EventArgs e)
        {
            string Msg = "local:\"" + txb_local_path.Text + "\"";
            TotalMsg += Msg;
            InputMsg();
        }
        #endregion

        #region Page 紀錄log
        private void button8_Click(object sender, EventArgs e)
        {
            string Msg = "log:\"" + txb_log_log.Text + "\"";
            TotalMsg += Msg;
            InputMsg();
        }
        #endregion

        #region Page 系統指令
        private void button9_Click(object sender, EventArgs e)
        {
            string Msg = "resetall:";
            TotalMsg += Msg;
            InputMsg();
        }
        #endregion

        #endregion

        #region 變更目標
        object target_txbInput = null;
        private void txb_CMD_MouseCaptureChanged(object sender, EventArgs e)
        {
            target_txbInput = txb_CMD;
            lbl_CMD.ForeColor = Color.Red;
            lbl_SUCCESS.ForeColor = Color.Black;
            lbl_FAIL.ForeColor = Color.Black;
        }

        private void txb_SUCCESS_MouseCaptureChanged(object sender, EventArgs e)
        {
            target_txbInput = txb_SUCCESS;
            lbl_CMD.ForeColor = Color.Black;
            lbl_SUCCESS.ForeColor = Color.Red;
            lbl_FAIL.ForeColor = Color.Black;
        }

        private void txb_FAIL_MouseCaptureChanged(object sender, EventArgs e)
        {
            target_txbInput = txb_FAIL;
            lbl_CMD.ForeColor = Color.Black;
            lbl_SUCCESS.ForeColor = Color.Black;
            lbl_FAIL.ForeColor = Color.Red;
        }
        #endregion

        /// <summary>
        /// 產生XML指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CreateXML_Click(object sender, EventArgs e)
        {
            string Msg=
                "<AIML>" + Environment.NewLine
                + "  <ID>" + txb_ID.Text + "</ID>" + Environment.NewLine
                + "  <CMD>" + txb_CMD.Text + "</CMD>" + Environment.NewLine
                + "  <SUCCESS>" + txb_SUCCESS.Text + "</SUCCESS>" + Environment.NewLine
                + "  <FAIL>" + txb_FAIL.Text + "</FAIL>" + Environment.NewLine
                + "</AIML>" + Environment.NewLine;
            Msg = Msg.Replace("<=", "&lt;");
                txb_FinalXML.Text = Msg;
        }

    }
}
