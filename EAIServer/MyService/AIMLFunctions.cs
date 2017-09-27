using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.Security;

namespace AgentAIServer.MyService
{
    /// <summary>
    /// 執行CMD分析後的結果
    /// </summary>
    class AIMLFunctions
    {
        public ClientMessageParameters cmp;
        public Socket MySocket;    //傳送訊息給 丟球給你的人 的Client Socket

        /// <summary>
        /// 排程/資料庫/代理人 命令狀態
        /// </summary>
        public enum ScheduleMode : int
        {
            Setup = 0,
            Stop = 1,
            Delete = 2,
            Start = 3
        }

        #region 排程 schedule*:
        /// <summary>
        /// 設定/停用/刪除 排程
        /// </summary>
        /// <param name="strlist"></param>
        public void Schedule_Setup(ScheduleMode Mode, string[] strlist)
        {
            Schedule_Item scheduleItem = new Schedule_Item();

            #region 1.先找到PK
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains(":"))
                {
                    if (strlist.Length > 1)
                        scheduleItem.PK = strlist[i + 1];
                    break;
                }
            }
            #endregion

            #region 2.找到排程設定檔
            DataRow[] drList = ServerCounter.dt_Schedule.Select("PK='" + scheduleItem.PK + "'");
            #endregion

            //Schedule操作用
            ScheduleCounter scheduleCounter = new ScheduleCounter();

            #region 3.依照功能選擇處理方式
            switch (Mode)
            {
                case ScheduleMode.Setup:
                    DataRow dr = null;
                    if (drList.Length > 0)
                    {
                        dr = drList[0];

                        #region 設定排程
                        //Update
                        string Name = "", Value = "";

                        for (int i = 0; i < strlist.Length; i++)
                        {
                            if (strlist[i].Contains("="))
                            {
                                int splitIndex = strlist[i].IndexOf("=");
                                Name = strlist[i].Substring(0, splitIndex).ToLower();
                                Value = strlist[i].Substring(splitIndex + 1, strlist[i].Length - splitIndex - 1);

                                switch (Name)
                                {
                                    case "pk":
                                        {
                                            scheduleItem.PK = Value; break;
                                        }
                                    case "db":
                                        {
                                            scheduleItem.DB = Value; break;
                                        }
                                    case "period":
                                        {
                                            scheduleItem.period = Convert.ToInt32(Value); break;
                                        }
                                    case "longwait":
                                        {
                                            scheduleItem.longwait = Convert.ToInt32(Value); break;
                                        }
                                    case "record":
                                        {
                                            scheduleItem.record = Convert.ToInt32(Value); break;
                                        }
                                    case "longdisconnect":
                                        {
                                            scheduleItem.longdisconnect = Convert.ToInt32(Value); break;
                                        }
                                    case "request":
                                        {
                                            scheduleItem.request = Value; break;
                                        }
                                    case "action":
                                        {
                                            scheduleItem.action = Value; break;
                                        }
                                    case "target":
                                        {
                                            scheduleItem.target = Value; break;
                                        }
                                    case "scheduleset":
                                        {
                                            scheduleItem.ScheduleSet = Value; break;
                                        }
                                }
                            }
                        }

                        if (scheduleItem.DB != string.Empty)
                            dr["DB"] = scheduleItem.DB;
                        if (scheduleItem.period != 0)
                            dr["period"] = scheduleItem.period;
                        if (scheduleItem.longwait != 0)
                            dr["longwait"] = scheduleItem.longwait;
                        if (scheduleItem.record != 0)
                            dr["record"] = scheduleItem.record;
                        if (scheduleItem.longdisconnect != 0)
                            dr["longdisconnect"] = scheduleItem.longdisconnect;
                        if (scheduleItem.request != string.Empty)
                            dr["request"] = scheduleItem.request;
                        if (scheduleItem.action != string.Empty)
                            dr["action"] = scheduleItem.action;
                        if (scheduleItem.target != string.Empty)
                            dr["target"] = scheduleItem.target;
                        if (scheduleItem.ScheduleSet != string.Empty)
                            dr["ScheduleSet"] = scheduleItem.ScheduleSet;
                        #endregion
                    }
                    else
                    //Insert
                    //如果為新的資料, 則塞入設定
                    {
                        if (scheduleItem.PK.Equals(string.Empty))
                            throw new Exception("沒設定PK");

                        dr = ServerCounter.dt_Schedule.NewRow();

                        #region 新增排程
                        dr[0] = scheduleItem.PK;
                        dr[1] = scheduleItem.DB;
                        dr[2] = scheduleItem.period;
                        dr[3] = scheduleItem.longwait;
                        dr[4] = scheduleItem.record;
                        dr[5] = scheduleItem.longdisconnect;
                        dr[6] = scheduleItem.request;
                        dr[7] = scheduleItem.action;
                        dr[8] = scheduleItem.target;
                        dr[9] = scheduleItem.ScheduleSet;

                        ServerCounter.dt_Schedule.Rows.Add(dr);
                        #endregion
                    }


                    //重新讀取排程設定->排程Thread
                    scheduleCounter.Setup(dr);
                    break;
                case ScheduleMode.Stop:
                    //停用Schedule
                    if (scheduleItem.PK.Equals(string.Empty))
                    {
                        ScheduleCounter.StopSchedule();
                    }
                    else
                    {
                        ScheduleCounter.StopSchedule(scheduleItem.PK);
                    }
                    break;
                case ScheduleMode.Start:
                    //啟用Schedule
                    if (scheduleItem.PK.Equals(string.Empty))
                    {
                        scheduleCounter.Setup();
                    }
                    else
                    {
                        scheduleCounter.Setup(drList[0]);
                    }
                    break;
                case ScheduleMode.Delete:
                    if (scheduleItem.PK.Equals(string.Empty))
                        throw new Exception("沒設定PK");

                    //停用Schedule
                    ScheduleCounter.StopSchedule(scheduleItem.PK);

                    //刪除設定檔
                    if (drList.Length > 0)
                    {
                        dr = drList[0];
                        ServerCounter.dt_Schedule.Rows.Remove(dr);
                    }
                    break;
            }
            #endregion

            #region 4.存檔
            ServiceFunctions service = new ServiceFunctions();
            service.setList_Schedule();
            #endregion


        }

        /// <summary>
        /// 改變 排程 執行中的狀態
        /// </summary>
        /// <param name="strlist"></param>
        internal void Schedule_Change(string[] strlist)
        {
            Schedule_Item item_target = new Schedule_Item();

            #region 1.先找到PK
            string PK = "";
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains(":"))
                {
                    if (strlist.Length > 1)
                        PK = strlist[i + 1];
                    break;
                }
            }
            if (PK == string.Empty)
                throw new Exception("沒設定PK");

            item_target = ScheduleCounter.scheduleItems.Find(a => a.PK.Equals(PK));
            #endregion

            string Name = "", Value = "";
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("="))
                {
                    int splitIndex = strlist[i].IndexOf("=");
                    Name = strlist[i].Substring(0, splitIndex);
                    Value = strlist[i].Substring(splitIndex + 1, strlist[i].Length - splitIndex - 1);

                    switch (Name)
                    {
                        case "Flag_IGotMessage":
                            if (Value == "1")
                                item_target.Flag_IGotMessage = true;
                            else
                                item_target.Flag_IGotMessage = false;
                            break;
                        case "requestRecord":
                            item_target.requestRecord = Convert.ToInt32(Value);
                            break;
                    }
                }
            }

        }
        #endregion

        #region 資料庫 database*:
        internal void database_Setup(ScheduleMode Mode, string[] strlist)
        {
            string PK = "";
            string Constring = "";

            #region 1.先找到PK
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains(":"))
                {
                    if (strlist.Length > 2)
                    {
                        PK = strlist[i + 1];
                        Constring = strlist[i + 2];
                    }
                    break;
                }
            }
            if (PK.Equals(string.Empty))
                throw new Exception("沒設定PK");
            #endregion

            #region 2.找到資料庫設定檔
            DataRow[] drlist = ServerCounter.dt_Database.Select("PK='" + PK + "'");
            #endregion

            #region 3.依照功能選擇處理方式
            switch (Mode)
            {
                case ScheduleMode.Setup:
                    //Update
                    if (drlist.Length > 0)
                    {
                        DataRow dr = drlist[0];
                        dr["ConnectionString"] = Constring;
                    }
                    else
                    //Insert
                    //如果為新的資料, 則塞入設定
                    {
                        ServerCounter.dt_Database.Rows.Add(PK, Constring);
                    }
                    break;
                case ScheduleMode.Delete:
                    if (drlist.Length > 0)
                    {
                        DataRow dr = drlist[0];
                        ServerCounter.dt_Database.Rows.Remove(dr);
                    }
                    break;
            }
            #endregion

            #region 4.存檔
            ServiceFunctions service = new ServiceFunctions();
            service.setList_database();
            #endregion
        }
        #endregion

        #region 代理人 agent*:
        internal void Agent_Setup(ScheduleMode Mode, string[] strlist)
        {
            string PK = "";
            #region 1.先找到PK
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains(":"))
                {
                    if (strlist.Length > 1)
                        PK = strlist[i + 1];
                    break;
                }
            }
            #endregion

            #region 2.找到設定檔
            bool Flag_ItIsNewItem = false;
            Settings_Agent_Item target_item = ServerCounter.Settings_Agent.Find(a => a.PK == PK);
            #endregion

            #region 3.依照功能選擇處理方式
            switch (Mode)
            {
                case ScheduleMode.Setup:
                    if (target_item == null)
                    {
                        target_item = new Settings_Agent_Item();
                        Flag_ItIsNewItem = true;
                    }

                    #region 設定變數
                    //Update
                    string Name = "", Value = "";

                    for (int i = 0; i < strlist.Length; i++)
                    {
                        if (strlist[i].Contains("="))
                        {
                            int splitIndex = strlist[i].IndexOf("=");
                            Name = strlist[i].Substring(0, splitIndex).ToLower();
                            Value = strlist[i].Substring(splitIndex + 1, strlist[i].Length - splitIndex - 1);

                            switch (Name)
                            {
                                case "pk":
                                    target_item.PK = Value;
                                    break;
                                case "ip":
                                    target_item.IP = Value;
                                    break;
                                case "port":
                                    target_item.Port = Convert.ToInt32(Value);
                                    break;
                                case "memo":
                                    target_item.MEMO = Value;
                                    break;
                            }
                        }
                    }
                    #endregion

                    //如果為新的資料, 則塞入設定
                    if (Flag_ItIsNewItem)
                    {
                        ServerCounter.Settings_Agent.Add(target_item);
                    }

                    break;
                case ScheduleMode.Delete:
                    //刪除目前設定
                    ServerCounter.Settings_Agent.Remove(target_item);
                    break;
            }
            #endregion

            #region 4.存檔
            ServiceFunctions service1001 = new ServiceFunctions();
            service1001.setList_Agent();
            #endregion
        }
        #endregion

        #region 命令 aiml*:
        internal void AIML_Setup(ScheduleMode Mode, string[] strlist)
        {
            string PK = "";
            #region 1.先找到PK
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains(":"))
                {
                    if (strlist.Length > 1)
                        PK = strlist[i + 1];
                    break;
                }
            }
            #endregion

            #region 2.找到設定檔
            bool Flag_ItIsNewItem = false;
            Settings_AIML_Item target_item = ServerCounter.Settings_AIML.Find(a => a.ID == PK);
            #endregion

            #region 3.依照功能選擇處理方式
            switch (Mode)
            {
                case ScheduleMode.Setup:
                    if (target_item == null)
                    {
                        target_item = new Settings_AIML_Item();
                        Flag_ItIsNewItem = true;
                    }

                    #region 設定變數
                    //Update
                    string Name = "", Value = "";

                    for (int i = 0; i < strlist.Length; i++)
                    {
                        if (strlist[i].Contains("="))
                        {
                            int splitIndex = strlist[i].IndexOf("=");
                            Name = strlist[i].Substring(0, splitIndex).ToLower();
                            Value = strlist[i].Substring(splitIndex + 1, strlist[i].Length - splitIndex - 1);

                            switch (Name)
                            {
                                case "id":
                                    target_item.ID = Value;
                                    break;
                                case "cmd":
                                    target_item.CMD = Value;
                                    break;
                                case "success":
                                    target_item.SUCCESS = Value;
                                    break;
                                case "fail":
                                    target_item.FAIL = Value;
                                    break;
                                case "memo":
                                    target_item.MEMO = Value;
                                    break;
                            }
                        }
                    }
                    #endregion

                    //如果為新的資料, 則塞入設定
                    if (Flag_ItIsNewItem)
                    {
                        ServerCounter.Settings_AIML.Add(target_item);
                    }

                    break;
                case ScheduleMode.Delete:
                    //刪除目前設定
                    ServerCounter.Settings_AIML.Remove(target_item);
                    break;
            }
            #endregion

            #region 4.存檔
            ServiceFunctions service1001 = new ServiceFunctions();
            service1001.setList_AIML();
            #endregion
        }
        #endregion

        #region 執行程式 local:
        /// <summary>
        /// 執行本地端執行檔
        /// 同名稱執行檔只能執行一份
        /// </summary>
        /// <param name="strlist"></param>
        internal void RunningLocalExe(string[] strlist)
        {
            string filepath = "";
            string arguments = "";
            int i = 0;

            #region 1.先找到路徑+其他參數
            for (i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("local:"))
                {
                    if (strlist.Length > 1)
                    {
                        filepath = strlist[i + 1];

                        #region 2.找出其他參數
                        for (i = i + 2; i < strlist.Length; i++)
                        {
                            arguments += strlist[i] + " ";
                        }
                        #endregion
                    }
                    break;
                }
            }
            if (filepath == string.Empty)
                throw new Exception("沒設定路徑");
            #endregion         

            string filename = Path.GetFileName(filepath).Replace(".exe", "");
            try
            {
                #region 2.程式已經在執行
                // 取得欲控制程式的名稱
                Process[] p = Process.GetProcessesByName(filename);

                // 判斷是否為長度大於 0
                if (p.Length > 0)
                {
                    int hwnd = p[0].MainWindowHandle.ToInt32();
                    clsShowWindow.ShowWindow(hwnd, (int)clsShowWindow.CommandShow.SW_SHOWDEFAULT);
                    return;
                }
                #endregion

                #region 3.開啟程式
                using (Mutex m = new Mutex(false, "Global\\" + filename))
                {
                    //檢查是否同名Mutex已存在(表示另一份程式正在執行)
                    if (!m.WaitOne(0, false))
                    {
                        throw new Exception("同名稱同參數只能執行一次 " + filename);
                    }

                    Process instance = new Process();
                    instance.StartInfo.FileName = filepath;
                    instance.StartInfo.Arguments = arguments;
                    instance.StartInfo.UseShellExecute = true;
                    instance.StartInfo.CreateNoWindow = true;
                    instance.StartInfo.WorkingDirectory = Path.GetDirectoryName(filepath);
                    instance.Start();
                    instance.WaitForExit();
                }
                #endregion
            }
            catch (Exception ex)
            {
                //回傳Socket訊息
                string[] NewstrlistError = new string[]{
                    "sendMessage:"
                    ,"parent"
                    ,"cmd:"
                    ,ex.Message};
                SendMesageToTarget(NewstrlistError);

                //記錄錯誤
                throw new Exception(filepath + " " + arguments + "\n"
                    + ex.Message + "\n"
                    + ex.StackTrace);
            }
        }
        #endregion

        #region 執行SQLcommand sqlcmd:
        /// <summary>
        /// 執行SQLcommand
        /// 外包給小程式負責執行sql指令
        /// </summary>
        /// <param name="strlist"></param>
        internal void RunningSQLcmd(string[] strlist)
        {
            //連線主機
            string LoginServer = "";
            #region 1.找到連線主機
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("db:"))
                {
                    LoginServer = strlist[i + 1];
                    break;
                }
            }
            if (LoginServer == string.Empty)
                throw new Exception("沒設定連線主機");
            #endregion

            //sql命令
            string sqlcmd = "";
            #region 2.找到sql命令
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("sqlcmd:"))
                {
                    if (strlist.Length > 1)
                        sqlcmd = strlist[i + 1];
                    break;
                }
            }
            if (sqlcmd == string.Empty)
                throw new Exception("沒設定SQL指令");
            #endregion

            string arguments =
                 "\"db:" + LoginServer + "\" "
                + "\"sqlcmd:" + sqlcmd + "\" ";
            Hashtable ht1 = new Hashtable();

            Process instance = new Process();
            instance.StartInfo.FileName = "AgentAIServer_sqlcmd.exe";
            instance.StartInfo.Arguments = arguments;
            instance.StartInfo.UseShellExecute = false;
            instance.StartInfo.RedirectStandardInput = true;
            instance.StartInfo.RedirectStandardOutput = true;
            instance.StartInfo.RedirectStandardError = true;
            instance.StartInfo.CreateNoWindow = true;
            instance.Start();

            string answer = instance.StandardOutput.ReadToEnd();
            instance.WaitForExit();

            string answerType = answer.Substring(0, answer.IndexOf(">") + 1);
            string answerContent = "";
            if (answer.IndexOf(">") >= answer.Length - 1)
                answerContent = "";
            else
                answerContent = answer.Substring(answer.IndexOf(">") + 1, answer.Length - answer.IndexOf(">") - 1);

            if (answerContent.Equals(string.Empty))
                return;

            switch (answerType)
            {
                case "table>":
                    //直接回傳給對方
                    string[] Newstrlist = new string[]{
                    "sendMessage:"
                    ,"parent"
                    ,"cmd:"
                    ,answerContent};
                    SendMesageToTarget(Newstrlist);
                    break;
                case "error>":
                    //回傳Socket訊息
                    string[] NewstrlistError = new string[]{
                    "sendMessage:"
                    ,"parent"
                    ,"cmd:"
                    ,answerContent};
                    SendMesageToTarget(NewstrlistError);

                    //紀錄log
                    throw new Exception(arguments + "\n" + answerContent);
            }
        }
        #endregion

        #region 傳送訊息給下一位 sendMessage:
        internal void SendMesageToTarget(string[] strlist)
        {
            string targetPK = "";

            string CMD = "";
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("cmd:"))
                {
                    CMD = strlist[i + 1];
                    break;
                }
            }
            if (CMD.Equals(string.Empty))
                throw new Exception("沒設定CMD");


            #region 2.找到對象
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("sendMessage:"))
                {
                    if (strlist.Length > 1)
                        targetPK = strlist[i + 1];
                    break;
                }
            }
            if (targetPK.Equals(string.Empty))
                throw new Exception("沒設定對象");
            #endregion

            string targetIP = "";
            if (targetPK.ToLower() == "parent")
            {
                ServerCounter.serverSocket.Send(MySocket, CMD + Environment.NewLine);
            }
            else
            {
                Settings_Agent_Item target = ServerCounter.Settings_Agent.Find(a => a.PK == targetPK);

                if (target != null)
                {
                    bool Flag_ReConnect = false;
                    if (target.clientSocket == null)
                        Flag_ReConnect = true;
                    else if (target.clientSocket.Connected == false)
                        Flag_ReConnect = true;

                    if (Flag_ReConnect)
                    {
                        //新增Socket, 丟訊息給對方
                        target.clientSocket = ServerCounter.ClientSocketInit(target.clientSocket, target.IP, target.Port);
                    }

                    target.clientSocket.Send(CMD + Environment.NewLine);
                }
                if (targetIP == string.Empty)
                    throw new Exception("沒設定對象的IP");
            }
        }

        #endregion

        #region 紀錄log log:
        internal void log(string[] strlist)
        {
            string content = "";
            string type = "FAIL";
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("log:"))
                {
                    if (strlist.Length > 1)
                    {
                        type = strlist[i + 1];
                        content = strlist[i + 2];
                    }
                    break;
                }
            }

            Program.recLog(content, type);
        }
        #endregion

        #region 系統指令_重整記憶體 resetall:
        internal void System_Setup()
        {
            //清除排程threads
            ScheduleCounter.StopSchedule();

            //清除Server收到的訊息
            ClientMessageQueue.RemoveAll();

            //清除Server準備做的threads
            ServerCounter.RemoveAll();

            //重新讀取設定
            ServiceFunctions service = new ServiceFunctions();
            service.reset();

            //重新讀取排程
            ScheduleCounter obj = new ScheduleCounter();
            obj.Setup();
        }
        #endregion

        #region 系統指令_重開機 restart:
        internal void System_Restart()
        {
            Program.CloseApplication();
        }
        #endregion

        #region 查詢目前狀態 query:
        internal void System_Query(string[] strlist)
        {
            string mode = "";
            #region 1.先找到路徑
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains("query:"))
                {
                    if (strlist.Length > 1)
                        mode = strlist[i + 1];
                    break;
                }
            }
            if (mode == string.Empty)
                throw new Exception("沒設定查詢模式");
            #endregion

            #region 組合回傳字串
            string answerContent = "";
            switch (mode)
            {
                //排程設定
                case "schedule":
                    answerContent = DB_IO.JSONconvert.DataTableToJSONstr(ServerCounter.dt_Schedule);
                    break;
                //資料庫設定
                case "database":
                    answerContent = DB_IO.JSONconvert.DataTableToJSONstr(ServerCounter.dt_Database);
                    break;
                //代理人設定
                case "agent":
                    DataTable dt = DB_IO.JSONconvert.ListToDataTable<Settings_Agent_Item>(ServerCounter.Settings_Agent);
                    dt.Columns.Remove("clientSocket");

                    answerContent = DB_IO.JSONconvert.DataTableToJSONstr(dt);
                    break;
                //命令設定
                case "command":
                    answerContent = DB_IO.JSONconvert.ListToJSONstr<Settings_AIML_Item>(ServerCounter.Settings_AIML);
                    break;
                //作用中的排程清單
                case "schedulelist":
                    answerContent = DB_IO.JSONconvert.ListToJSONstr<Schedule_Item>(ScheduleCounter.scheduleItems);
                    break;
            }
            #endregion

            //直接回傳給對方
            string[] Newstrlist = new string[]{
                    "sendMessage:"
                    ,"parent"
                    ,"cmd:"
                    ,answerContent};
            SendMesageToTarget(Newstrlist);
        }
        #endregion

    }

    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class clsShowWindow
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern int ShowWindow(int hwnd, int nCmdShow);

        // 以下是為了給 nCmdShow 參數相對應的代碼意思
        internal enum CommandShow : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,

        }
    }
}
