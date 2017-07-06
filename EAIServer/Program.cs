using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using AgentAIServer.MyService;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace AgentAIServer
{
    /*
     * 主程式
     * 檔案內容解說:
     * AIMLCounter.cs       命令中的CMD傳遞到這邊，解析命令組成後再傳遞給AIMLFunctions
     * AIMLFunctions.cs     執行各種不同的動作，有可能呼叫到其他架構的原件
     * ClientMessageParameters.cs
     * ClientMessageQueue.cs負責判斷傳進來的CMD是否重複
     * DB_IO.cs             資料庫連接專用
     * MyCookies.cs         系統運作需要的參數
     * ScheduleCounter.cs   排程!
     * ServerCounter.cs     核心系統運作, socket的接收傳遞, 執行緒管理
     * ServiceFunctions.cs  維持核心系統的設定 讀取寫入
     * AgentAIServer.cs     GUI介面, 可有可無
     
     */
    static class Program
    {
        static string appGuid = "";        //每支程式以不同GUID當成Mutex名稱，可避免執行檔同名同姓的風險
        public static ServerCounter server = new ServerCounter();
        public static string hostName = "", localIP = "";

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main(string[] args)
        {
            var domain = AppDomain.CurrentDomain;
            domain.AssemblyResolve += LoadAssembly;

            //處理未捕捉的例外
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //處理UI執行緒錯誤
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //非處理UI執行緒錯誤
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //延遲呼叫
            //意外當機的時候會用上
            if (args.Length > 0 && args[0] == "-delay")
            {
                Thread.Sleep(10000);
            }

            //如果要做到跨Session唯一，名稱可加入"Global\"前綴字
            //如此即使用多個帳號透過Terminal Service登入系統
            //整台機器也只能執行一份

            //Agent_LCU專屬Mutex
            appGuid = Process.GetCurrentProcess().ProcessName + ".exe";

            using (Mutex m = new Mutex(false, "Global\\" + appGuid))
            {
                //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                if (!m.WaitOne(0, false))
                {
                    MessageBox.Show("同名稱同參數只能執行一次 \n" + appGuid, "Mutex");
                    return;
                }

                #region Log
                string path = "log\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                #endregion

                #region 核心啟動

                #region 取得IP
                //取得本機名稱
                hostName = Dns.GetHostName();

                //取得本機IP
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("10.0.2.4", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                }

                MyCookies.host = IPAddress.Parse(localIP);
                #endregion

                //Schedule ON
                server.ServerON();
                #endregion

                #region 介面顯示
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new AgentAIServer());
                #endregion
            }
        }

        #region 執行緒錯誤
        //處理UI執行緒錯誤
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception err = e.Exception as Exception;
            recLog("ThreadException " + err.Message + err.StackTrace, "Error");
        }

        //非處理UI執行緒錯誤
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception err = e.ExceptionObject as Exception;
            recLog("UnhandledException " + err.Message + err.StackTrace, "Error");

            try
            {
                //發信
                eMailFunctions email = new eMailFunctions();
                email.eMailSettings_Get();
                email.Mail_Send(MyCookies.host.ToString() + " TAKO " + " UnhandledException"
                    , "When you see this letter, I was crashed. " + "\n"
                    + "UnhandledException " + "\n"
                    + err.Message + "\n"
                    + err.StackTrace + "\n");
            }
            catch { }

            CloseApplication();
        }
        #endregion

        #region 系統功能
        /// <summary>
        /// 讀取dll檔案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Assembly LoadAssembly(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            if (args != null && !string.IsNullOrEmpty(args.Name))
            {
                //Get current exe fullpath
                FileInfo info = new FileInfo(Assembly.GetExecutingAssembly().Location);

                //Get folder of the executing .exe
                var folderPath = Path.Combine(info.Directory.FullName, "DLL");

                //Build potential fullpath to the loading assembly
                var assemblyName = args.Name.Split(new string[] { "," }, StringSplitOptions.None)[0];
                var assemblyExtension = "dll";
                var assemblyPath = Path.Combine(folderPath, string.Format("{0}.{1}", assemblyName, assemblyExtension));

                //Check if the assembly exists in our "Libs" directory
                if (File.Exists(assemblyPath))
                {
                    //Load the required assembly using our custom path
                    result = Assembly.LoadFrom(assemblyPath);
                }
                else
                {
                    //Keep default loading
                    return args.RequestingAssembly;
                }
            }

            return result;
        }

        /// <summary>
        /// 重開機
        /// </summary>
        public static void CloseApplication()
        {
            server.ServerOFF();
            Application.ExitThread();
            Environment.Exit(0);
        }

        #region Log
        static object lockobj = new object();
        /// <summary>
        /// Log寫入備份檔
        /// </summary>
        public static void recLog(string Message, string type)
        {
            if (!MyService.MyCookies.Log)
                return;

            Message = DateTime.Now.ToString("MM/dd HH:mm:ss") + " " + Message;

            string path = "log\\" + type + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            lock (lockobj)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(Message + Environment.NewLine);
                    writer.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion

        #endregion
    }
}
