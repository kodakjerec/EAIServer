using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Pxmart.Sockets;
using System.IO;

namespace SocketTest
{
    /*
     * 測試AgentAIserver
     * 操作說明：
     * 1. 設定對象IP/Port
     * 2. 按下連線
     * 3. 迴圈send
     *    依照  命令列表中的命令，依照設定秒數循環發送
     * 4. send
     *    發送  命令列表中的命令
     * 5  Stop
     *    停止迴圈send
     */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            mg = putMsgAndShow;
        }

        ClientSocket clientsocket1;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //讀入對方IP
                StreamReader sr1 = new StreamReader("JSON\\Agent_json.txt", Encoding.GetEncoding("BIG5"));
                try
                {
                    DataTable dt = AgentAIServer.JSON.JSONconvert.JSONstrToDataTable(sr1.ReadToEnd());
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["PK"].ToString() == "local")
                        {
                            txb_IP.Text = dr["IP"].ToString();
                            txb_Port.Text = dr["Port"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sr1.Close();
                }

                //讀入腳本
                StreamReader sr = new StreamReader("JSON\\Scripts_json.txt", Encoding.GetEncoding("BIG5"));
                try
                {
                    DataTable dt = AgentAIServer.JSON.JSONconvert.JSONstrToDataTable(sr.ReadToEnd());
                    string param = "";
                    foreach (DataRow dr in dt.Rows)
                        param += dr[0].ToString() + Environment.NewLine;
                    txb_Message.Text = param;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region socket Event
        private void Clientsocket1_OnSend(object sender, SendEventArgs e)
        {
            putMsgAndShow("I Send " + e.Message);
            //throw new NotImplementedException();
        }

        private void Clientsocket1_OnReceive(object sender, ReceiveEventArgs e)
        {
            putMsgAndShow("I Recv " + e.Message);
            // throw new NotImplementedException();
        }

        private void Clientsocket1_OnDisconnect(object sender, DisconnectEventArgs e)
        {
            btn_LoopSendStop.PerformClick();
            btn_Send.Enabled = false;
            btn_LoopSend.Enabled = false;


            putMsgAndShow("I Disconnect ");
            // throw new NotImplementedException();
        }

        private void Clientsocket1_OnConnect(object sender, ConnectEventArgs e)
        {
            btn_Send.Enabled = true;
            btn_LoopSend.Enabled = true;

            putMsgAndShow("I Connect ");
            // throw new NotImplementedException();
        }

        void clientsocket1_OnError(object sender, Pxmart.Sockets.ErrorEventArgs e)
        {
            putMsgAndShow("I Error " + e.Error);
            //throw new NotImplementedException();
        }
        #endregion

        #region Msg
        delegate void putMsg(string Msg);
        putMsg mg;
        void putMsgAndShow(string Msg)
        {
            if (this.InvokeRequired)
            {
                putMsg mg = new putMsg(putMsgAndShow);
                Invoke(mg, Msg);
            }
            else
            {
                txb_log.Text += DateTime.Now.ToString("HH:mm:ss") + " " + Msg + Environment.NewLine;

                if (txb_log.Lines.Length > 20)
                {
                    for (int i = 10; i < txb_log.Lines.Length; i++)
                    {
                        txb_log.Text = txb_log.Text.Remove(0, txb_log.Lines[0].Length + Environment.NewLine.Length);
                    }
                }
            }
        }
        #endregion

        //Connect
        private void button2_Click(object sender, EventArgs e)
        {
            if (clientsocket1 != null)
                if (clientsocket1.Connected)
                {
                    clientsocket1.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    clientsocket1.Close();
                }
            clientsocket1 = new ClientSocket();
            clientsocket1.OnConnect += Clientsocket1_OnConnect;
            clientsocket1.OnDisconnect += Clientsocket1_OnDisconnect;
            clientsocket1.OnError += clientsocket1_OnError;
            clientsocket1.OnReceive += Clientsocket1_OnReceive;
            clientsocket1.OnSend += Clientsocket1_OnSend;
            clientsocket1.Connect(txb_IP.Text, Convert.ToInt32(txb_Port.Text));
        }

        //SendMsg
        private void button1_Click(object sender, EventArgs e)
        {
            clientsocket1.Send(txb_Message.Text);
        }

        private void btn_LoopSend_Click(object sender, EventArgs e)
        {
            putMsgAndShow("開始測試");
            btn_LoopSendStop.Enabled = true;
            timer1.Interval = Convert.ToInt32(txb_Time.Text) * 1000;
            timer1.Start();
        }

        private void btn_LoopSendStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btn_LoopSendStop.Enabled = false;
            putMsgAndShow("停止測試");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btn_Send.PerformClick();
        }

        private void btn_ShowCreatePanel_Click(object sender, EventArgs e)
        {
            CreateAIML obj = new CreateAIML();
            obj.ShowDialog();
        }




    }
}
