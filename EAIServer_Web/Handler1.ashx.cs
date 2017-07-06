using System;
using System.Web;
using Pxmart.Sockets;
using System.Net;
using System.Web.Services;
using System.Net.Sockets;
using System.Threading;

namespace EAIServer_Web
{
    /// <summary>
    /// Handler1 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler1 : IHttpHandler
    {
        ClientSocket ShareClientSocket = null;
        MyCookies myCookie = new MyCookies();
        HttpContext myContext;
        int IReceiveCount = 0;
        bool IReceiveFlag = false;
        string ReturnMsg = "";

        public void ProcessRequest(HttpContext context)
        {
            //傳入的參數
            myContext = context;
            string[] dataList = myContext.Request.Form.AllKeys;
            string Mode = myContext.Request.QueryString["mode"] ?? string.Empty;

            switch (Mode)
            {
                case "queryTAKO":
                    queryTAKO();
                    break;
                case "DoTAKOcommand":
                    DoTAKOcommand();
                    break;
                case "queryISHIDA":
                    queryISHIDA();
                    break;
                default:
                    myContext.Response.ContentType = "application/json";
                    myContext.Response.Charset = "utf-8";
                    myContext.Response.Write(
                        "{ \"RT_CODE\":" + "1"
                        + ",\"RT_MSG\":" + "NoData" + "}");
                    break;
            }


        }

        /// <summary>
        /// 查詢ISHIDA
        /// </summary>
        private void queryISHIDA()
        {
            int RT_CODE = 0;
            string ErrMsg = "";
            string myType = myContext.Request.QueryString["type"] ?? string.Empty;
            string MaxLength = myContext.Request.QueryString["MaxLength"] ?? "20";
            string StartSeq = myContext.Request.QueryString["StartSeq"] ?? "0";
            string Shop = myContext.Request.QueryString["Shop"] ?? "";
            string CALLING_NUM = myContext.Request.QueryString["CALLING_NUM"] ?? "";
            string DEVICE_AREA = myContext.Request.QueryString["DEVICE_AREA"];
            string OrderNo = myContext.Request.QueryString["OrderNo"];

            //後端的Socket
            //取得本機IP
            try
            { 
                string CMD = "DOSQLCMD \"Select * "
                    + " from [ob.DDI_UD_LCU_ISHIDA_PCRS_V1_SFD001] with(nolock)"
                    + " where TXTSEQ>0 and DEVICE_AREA='" + DEVICE_AREA + "' and OrderNo='"+ OrderNo+"' "
                    + " and Field10>=" + StartSeq
                    + (Shop == "" ? "" : " and Field08=" + Shop)
                    + (CALLING_NUM == "" ? "" : " and Field02=" + CALLING_NUM)
                    + " order by Field10\" \"DDI_UNDER\" ";

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("10.0.2.4", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    myCookie.host = IPAddress.Parse(endPoint.Address.ToString());
                    myCookie.Port = 1813;

                }

                ShareClientSocket = ClientSocketInit(ShareClientSocket, myCookie.host.ToString(), myCookie.Port);
                ShareClientSocket.Send(CMD);

                while (IReceiveCount < 65535)
                {
                    Thread.Sleep(1000);
                    IReceiveCount++;
                }
                if (!IReceiveFlag)
                {
                    RT_CODE = 1;
                    ErrMsg = "\"取得Socket回應超過 65535 秒\"";
                }
                else
                {
                    RT_CODE = 0;
                    ErrMsg = ReturnMsg;
                }
            }
            catch (Exception ex)
            {
                RT_CODE = 1;
                ErrMsg = ex.Message;
            }
            finally
            {
                ShareClientSocket.Close();
            }

            myContext.Response.ContentType = "application/json";
            myContext.Response.Charset = "utf-8";
            myContext.Response.Write(
                "{ \"RT_CODE\":" + RT_CODE.ToString()
                + ",\"RT_MSG\":" + ErrMsg + "}");
        }

        #region TAKO
        /// <summary>
        /// 查詢TAKO指令
        /// </summary>
        private void queryTAKO()
        {
            int RT_CODE = 0;
            string ErrMsg = "";
            string myType = myContext.Request.QueryString["type"] ?? string.Empty;
            //後端的Socket
            //取得本機IP
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("10.0.2.4", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    myCookie.host = IPAddress.Parse(endPoint.Address.ToString());
                    myCookie.Port = 1813;
                }

                ShareClientSocket = ClientSocketInit(ShareClientSocket, myCookie.host.ToString(), myCookie.Port);
                ShareClientSocket.Send("query " + myType);
                while (IReceiveCount < 60)
                {
                    Thread.Sleep(1000);
                    IReceiveCount++;
                }
                if (!IReceiveFlag)
                {
                    RT_CODE = 1;
                    ErrMsg = "\"取得Socket回應超過60秒\"";
                }
                else
                {
                    RT_CODE = 0;
                    ErrMsg = ReturnMsg;
                }
            }
            catch (Exception ex)
            {
                RT_CODE = 1;
                ErrMsg = ex.Message;
            }
            finally
            {
                ShareClientSocket.Close();
            }

            myContext.Response.ContentType = "application/json";
            myContext.Response.Charset = "utf-8";
            myContext.Response.Write(
                "{ \"RT_CODE\":" + RT_CODE.ToString()
                + ",\"RT_MSG\":" + ErrMsg + "}");
        }

        /// <summary>
        /// 執行TAKO 指令
        /// </summary>
        private void DoTAKOcommand()
        {
            string ErrMsg = "";
            string command = myContext.Request.QueryString["command"] ?? string.Empty;
            //後端的Socket
            //取得本機IP
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("10.0.2.4", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    myCookie.host = IPAddress.Parse(endPoint.Address.ToString());
                    myCookie.Port = 1813;
                }

                ShareClientSocket = ClientSocketInit(ShareClientSocket, myCookie.host.ToString(), myCookie.Port);
                ShareClientSocket.Send(command);
                while (IReceiveCount < 60)
                {
                    Thread.Sleep(1000);
                    IReceiveCount++;
                }
                if (!IReceiveFlag)
                {
                    ErrMsg = "\"取得Socket回應超過60秒\"";
                }
                else
                {
                    ErrMsg = ReturnMsg;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
            finally
            {
                ShareClientSocket.Close();
            }
        }
        #endregion

        #region Client-socket Init
        public ClientSocket ClientSocketInit(Pxmart.Sockets.ClientSocket obj, string host, int ip)
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
        void clientSocket1_OnSend(object sender, Pxmart.Sockets.SendEventArgs e)
        {
        }

        void clientSocket1_OnReceive(object sender, Pxmart.Sockets.ReceiveEventArgs e)
        {
            IReceiveCount = 65535;
            IReceiveFlag = true;
            ReturnMsg = e.Message;
        }

        void clientSocket1_OnError(object sender, Pxmart.Sockets.ErrorEventArgs e)
        {
            string localEndPoint = "";
            if (((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint != null)
                localEndPoint = ((Pxmart.Sockets.ClientSocket)sender).LocalEndPoint.ToString();
        }

        void clientSocket1_OnDisconnect(object sender, Pxmart.Sockets.DisconnectEventArgs e)
        {
        }

        void clientSocket1_OnConnect(object sender, Pxmart.Sockets.ConnectEventArgs e)
        {
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class MyCookies
    {
        //需要設定的變數
        public int Port = 0;

        //動態產生
        public IPAddress host = null;

    }
}