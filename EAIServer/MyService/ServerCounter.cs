using System;
using System.Net.Sockets;
using System.Data;

using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.IO;
/*
 * log
 * 2017.06.26 Kota  1. 加入函數ServerOFF
 *                  2. 函數ServerON加入清除log
 */
namespace AgentAIServer.MyService
{
    /// <summary>
    /// 角色:櫃台    Server
    /// </summary>
    public class ServerCounter
    {
        #region 全域變數
        //Server Socket
        public static Pxmart.Sockets.ServerSocket serverSocket = null;       //server Socket, 每台一個
        public static Pxmart.Sockets.ClientSocket ShareClientSocket = null;

        public static List<Thread> serverThreads = new List<Thread>();      //接收到的訊息, 一個訊息一個執行緒

        public static DataTable aimls = new DataTable();          //命令清單
        public static bool ServerDuplexON = true;

        //JSON讀入後的設定檔, 所有基礎設定來源
        public static DataTable dt_Database
                                , dt_Schedule;

        //有自定結構或是要求效能才改用List
        public static List<Settings_Agent_Item> Settings_Agent = new List<Settings_Agent_Item>();
        public static List<Settings_AIML_Item> Settings_AIML = new List<Settings_AIML_Item>();
        #endregion

        #region 開機關機
        /// <summary>
        /// 啟動設定檔
        /// </summary>
        public void ServerON()
        {
            //清除log
            //清除 agent本身的log
            string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + @"\log");
            foreach (string file in files)
            {
                if (File.GetLastWriteTime(file) <= DateTime.Now.AddDays(-7))
                    File.Delete(file);
            }

            //read Settings
            ServiceFunctions service1001 = new ServiceFunctions();
            service1001.reset();

            //Server Socket ON
            Init(MyCookies.Port);

            //重啟排程thread
            ScheduleCounter scheduleCounter = new ScheduleCounter();
            Thread newThread1 = new Thread(scheduleCounter.Init);
            newThread1.IsBackground = true;
            newThread1.Name = "ScheduleCounter";
            newThread1.Start();

            //Server的單工作業判斷啟動Duplex
            Thread newThread2 = new Thread(FindNextThreadToExecute);
            newThread2.IsBackground = true;
            newThread2.Name = "Server單工作業";
            newThread2.Start();
        }

        public void ServerOFF()
        {
            //單工作業判斷OFF
            ServerDuplexON = false;

            //排程thread OFF
            ScheduleCounter.StopSchedule();
        }
        #endregion


        /// <summary>
        /// 櫃台接收到工作, 準備分發工作給其他人
        /// 負責讀取AIML設定檔
        /// </summary>
        /// <param name="Message"></param>
        public void getMessage(string[] args, Socket clientSocket)
        {
            //必定要有變數
            if (args.Length <= 0)
                return;

            //尋找命令
            string ID = args[0];  //第一個參數一定是命令

            Settings_AIML_Item item = Settings_AIML.Find(a => a.ID == ID);

            //找到命令了!
            if (item != null)
            {
                ClientMessageParameters cmp = new ClientMessageParameters();
                AIMLCounter aiml = new AIMLCounter();
                aiml.cmp = cmp;
                aiml.MySocket = clientSocket;

                #region 命令參數+單工模式判斷
                string DuplexNum = "";

                //執行緒and訊息柱列為1:1
                //額外分支  訊息柱列  為方便系統運作
                int argsCount = 1;
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i].ToLower().IndexOf("simplex:") >= 0)
                    {
                        DuplexNum = args[i + 1];
                        i++;
                    }
                    else
                    {
                        cmp.ParameterListAdd("args" + argsCount.ToString(), args[i]);
                        argsCount++;
                    }
                }
                #endregion

                #region 產生Thread
                Thread threadCMD = new Thread(
                    delegate ()
                    {
                        try
                        {
                            #region 主要執行命令
                            //命令:CMD
                            string CMD = item.CMD;
                            if (!CMD.Equals(string.Empty))
                            {
                                //aiml.PutParameter(ref CMD);
                                ScheduleCounter.InputReceive("ID:[" + DuplexNum + "]" + ID + " 內容:" + CMD);
                                aiml.Chat(CMD);
                            }
                            #endregion

                            //成功:SUCCESS
                            CMD = item.SUCCESS;
                            if (!CMD.Equals(string.Empty))
                            {
                                aiml.PutParameter(ref CMD);
                                ScheduleCounter.InputReceive("ID:[" + DuplexNum + "]" + ID + " 成功:" + CMD);
                                ShareClientSocket.Send(CMD + Environment.NewLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            //例外:FAIL
                            string CMD = item.FAIL;
                            if (!CMD.Equals(string.Empty))
                            {
                                if (CMD.IndexOf("ErrorMessage") >= 0)
                                {
                                    //錯誤訊息存入變數ErrorMessage
                                    cmp.ParameterListAdd("ErrorMessage", ex.Message);
                                }

                                //替換錯誤訊息
                                aiml.PutParameter(ref CMD);
                                ScheduleCounter.InputReceive("ID:[" + DuplexNum + "]" + ID + " 例外:" + CMD);
                                ShareClientSocket.Send(CMD + Environment.NewLine);
                            }
                        }
                    }
                    );
                #endregion

                #region 執行Thread
                threadCMD.Name = "[" + DuplexNum + "]" + string.Join("", args);
                serverThreads.Add(threadCMD);
                if (DuplexNum == string.Empty)
                {
                    threadCMD.Start();
                }
                #endregion
            }
        }

        #region 執行緒管理
        /*
         * 重要的執行緒放入ThreadList,搭配單工分組可延後執行, 例如:執行命令內容
         * 不重要的執行緒放入ThreadPool, 例如:呼叫下一個thread起床, 傳送命令給自己
         */
        /// <summary>
        /// 尋找下個可執行的執行緒
        /// 有指定Duplex, 則執行單工作業
        /// </summary>
        public void FindNextThreadToExecute()
        {
            while (ServerDuplexON)
            {
                try
                {
                    serverThreads.RemoveAll(a => (a.ThreadState & (ThreadState.Stopped)) != 0);

                    var result = (from a in serverThreads
                                  where a.Name.Substring(0, 2) != "[]"
                                  select a.Name.Substring(0, a.Name.IndexOf("]") + 1)).Distinct();
                    for (int i = 0; i < result.Count(); i++)
                    {
                        string DuplexNum = result.ElementAt(i).ToString();

                        Thread Firstthread = serverThreads.Find(a => a.Name.IndexOf(DuplexNum) >= 0
                                && (a.ThreadState & (ThreadState.Running)) != 0);
                        if (Firstthread == null)
                        {
                            //沒有正在執行中的Thread, 才能執行
                            Firstthread = serverThreads.Find(a => a.Name.IndexOf(DuplexNum) >= 0
                                    && (a.ThreadState & (ThreadState.Unstarted)) != 0);

                            if (Firstthread != null)
                            {
                                Firstthread.Start();
                            }
                        }
                    }
                }
                catch { }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 清除所有等待執行命令
        /// </summary>
        public static void RemoveAll()
        {
            serverThreads.RemoveAll(a => a != null);
            serverThreads = new List<Thread>();
        }
        #endregion

        #region Send/Receive Message顯示
        static string txb_recv_Test = "";   //接受字串
        static string txb_send_Test = "";   //傳送字串
        static int DefaultMaxRows = 20; //最長畫面能顯示的行數

        /// <summary>
        /// 接收Message, 放入queue
        /// </summary>
        /// <param name="str1"></param>
        public static void InputReceive(string str1)
        {
            if (str1.Length > 1000)
                str1 = str1.Substring(0, 200);
            str1=str1.Replace("\r\n", "");

            txb_recv_Test += DateTime.Now.ToString("MM/dd HH:mm:ss") + " " + str1 + Environment.NewLine;
            if (txb_recv_Test.Split('\n').Length > DefaultMaxRows)
            {
                int Maxrows = txb_recv_Test.Split('\n').Length;
                string temp = txb_recv_Test.Remove(0, txb_recv_Test.Split('\n')[Maxrows - DefaultMaxRows].Length + 1);
                txb_recv_Test = null;
                txb_recv_Test = temp;
                temp = null;
            }
        }

        /// <summary>
        /// 傳送Message, 放入queue
        /// </summary>
        /// <param name="str1"></param>
        public static void InputSend(string str1)
        {
            if (str1.Length > 1000)
                str1 = str1.Substring(0, 200);
            str1=str1.Replace("\r\n", "");

            txb_send_Test += DateTime.Now.ToString("MM/dd HH:mm:ss") + " " + str1 + Environment.NewLine;
            if (txb_send_Test.Split('\n').Length > DefaultMaxRows)
            {
                int Maxrows = txb_send_Test.Split('\n').Length;
                string temp = txb_send_Test.Remove(0, txb_send_Test.Split('\n')[Maxrows - DefaultMaxRows].Length + 1);
                txb_send_Test = null;
                txb_send_Test = temp;
                temp = null;
            }
        }

        /// <summary>
        /// 輸出Receive queue
        /// </summary>
        /// <returns></returns>
        public string OutputReceive()
        {
            return txb_recv_Test;
        }

        /// <summary>
        /// 輸出Send queue
        /// </summary>
        /// <returns></returns>
        public string OutputSend()
        {
            return txb_send_Test;
        }
        #endregion

        #region Server-socket Init
        public void Init(object obj)
        {
            int Port = Convert.ToInt32(obj);
            serverSocket = new Pxmart.Sockets.ServerSocket();
            serverSocket.OnAccept += new Pxmart.Sockets.AcceptHandler(serverSocket_OnAccept);
            serverSocket.OnClientConnect += new Pxmart.Sockets.ClientConnectHandler(serverSocket_OnClientConnect);
            serverSocket.OnClientDisconnect += new Pxmart.Sockets.ClientDisconnectHandler(serverSocket_OnClientDisconnect);
            serverSocket.OnClientError += new Pxmart.Sockets.ClientErrorHandler(serverSocket_OnClientError);
            serverSocket.OnClientRead += new Pxmart.Sockets.ClientReadHandler(serverSocket_OnClientRead);
            serverSocket.OnClientWrite += new Pxmart.Sockets.ClientWriteHandler(serverSocket_OnClientWrite);
            serverSocket.OnError += new Pxmart.Sockets.ErrorHandler(serverSocket_OnError);
            serverSocket.OnListen += new Pxmart.Sockets.ListenHandler(serverSocket_OnListen);
            serverSocket.Bind(Port);
            serverSocket.Listen(100);
            serverSocket.Accept();

            //建立公用的clientSocket, 與自己的Server溝通
            ShareClientSocket = ClientSocketInit(ShareClientSocket, MyCookies.host.ToString(), MyCookies.Port);
            Settings_Agent_Item target = ServerCounter.Settings_Agent.Find(a => a.PK == "local");
            target.clientSocket = ShareClientSocket;
        }
        #endregion

        #region Server-socket Event
        void serverSocket_OnListen(object sender, Pxmart.Sockets.ListenEventArgs e)
        {

        }

        void serverSocket_OnError(object sender, Pxmart.Sockets.ErrorEventArgs e)
        {

        }

        void serverSocket_OnClientWrite(object sender, Pxmart.Sockets.ClientWriteEventArgs e)
        {
            InputSend(e.Client.RemoteEndPoint.ToString() + " ServerSend " + " " + e.Message);

        }

        void serverSocket_OnClientError(object sender, Pxmart.Sockets.ClientErrorEventArgs e)
        {

        }

        void serverSocket_OnClientDisconnect(object sender, Pxmart.Sockets.ClientDisconnectEventArgs e)
        {

        }

        void serverSocket_OnClientConnect(object sender, Pxmart.Sockets.ClientConnectEventArgs e)
        {
            InputReceive(e.Client.RemoteEndPoint.ToString() + " Server ClientConnect ");

        }

        void serverSocket_OnAccept(object sender, Pxmart.Sockets.AcceptEventArgs e)
        {

        }

        void serverSocket_OnClientRead(object sender, Pxmart.Sockets.ClientReadEventArgs e)
        {
            InputReceive(e.Client.RemoteEndPoint.ToString() + " ServerReceive " + " " + e.Message);

            if (e.Message == string.Empty)
                return;

            //放入訊息柱列, 避免同IP同訊息重複傳送
            string clientIP = ((IPEndPoint)(e.Client.RemoteEndPoint)).Address.ToString();
            if (clientIP == null)
            {
                return;
            }
            ClientMessage clientMessage = ClientMessageQueue.Add(clientIP, e.Message);
            if (clientMessage.IP == string.Empty)
            {
                //重複發送
                return;
            }

            ClientMessageParameters cmp = new ClientMessageParameters();
            AIMLCounter aiml = new AIMLCounter();
            aiml.cmp = cmp;
            List<string[]> stringLists = aiml.splitter(e.Message);
            foreach (string[] message in stringLists)
            {
                getMessage(message, (Socket)e.Client);
            }
            //刪除訊息柱列
            ClientMessageQueue.Remove(clientIP, e.Message);
        }
        #endregion

        #region Client-socket Init
        public static Pxmart.Sockets.ClientSocket ClientSocketInit(Pxmart.Sockets.ClientSocket obj, string host, int ip)
        {
            obj = new Pxmart.Sockets.ClientSocket();
            obj.OnConnect += clientSocket1_OnConnect;
            obj.OnDisconnect += clientSocket1_OnDisconnect;
            obj.OnError += clientSocket1_OnError;
            obj.OnReceive += clientSocket1_OnReceive;
            obj.OnSend += clientSocket1_OnSend;
            obj.Connect(host, ip);
            return obj;
        }
        #endregion

        #region Client-socket Event
        static void clientSocket1_OnSend(object sender, Pxmart.Sockets.SendEventArgs e)
        {
            InputSend(((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint.ToString() + " Client Send " + " " + e.Message);
        }

        static void clientSocket1_OnReceive(object sender, Pxmart.Sockets.ReceiveEventArgs e)
        {
            //Log
            InputReceive(((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint.ToString() + " Client Receive " + " " + e.Message);
            ShareClientSocket.Send(e.Message);
        }

        static void clientSocket1_OnError(object sender, Pxmart.Sockets.ErrorEventArgs e)
        {
            string localEndPoint = "";
            if (((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint != null)
                localEndPoint = ((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint.ToString();
            InputReceive(localEndPoint + " Error " + e.Error);

        }

        static void clientSocket1_OnDisconnect(object sender, Pxmart.Sockets.DisconnectEventArgs e)
        {
            InputReceive(" Client Disconnect " + ((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint.ToString());

        }

        static void clientSocket1_OnConnect(object sender, Pxmart.Sockets.ConnectEventArgs e)
        {
            InputReceive(" Client Connect " + ((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint.ToString());

        }

        #endregion
    }

    #region List內的item
    /// <summary>
    /// 設定:代理人
    /// </summary>
    public class Settings_Agent_Item
    {
        public string PK { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string MEMO { get; set; }
        public Pxmart.Sockets.ClientSocket clientSocket { get; set; }
    }

    /// <summary>
    /// 設定:命令
    /// </summary>
    public class Settings_AIML_Item
    {
        public string ID { get; set; }
        public string CMD { get; set; }
        public string SUCCESS { get; set; }
        public string FAIL { get; set; }
        public string MEMO { get; set; }
    }
    #endregion
}
