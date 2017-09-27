using System.Data;
using System;
using System.Threading;
using System.Collections.Generic;

namespace AgentAIServer.MyService
{
    /// <summary>
    /// 角色:排程櫃台    Client
    /// 任務:管理排程, 分發排程任務
    /// </summary>
    public class ScheduleCounter
    {
        #region 全域變數
        public static List<Schedule_Item> scheduleItems = new List<Schedule_Item>();   //運行中的排程清單
        public static List<Thread> scheduleThreads = new List<Thread>();
        #endregion


        /// <summary>
        /// 實際排程內容
        /// </summary>
        /// <param name="obj"></param>
        private void ScheduleStart(object obj)
        {
            ServerCounter service = new ServerCounter();
            Schedule_Item item = obj as Schedule_Item;

            try
            {
                //Start_重設排程時間
                item.NextTime = DateTime.MaxValue;

                InputReceive("排程 " + item.PK + " Start");

                #region 執行查詢指令
                if (!item.request.Equals(string.Empty))
                    ServerCounter.ShareClientSocket.Send(item.request + Environment.NewLine);
                #endregion

                #region 檢查是否滿足條件
                //條件1:滿足查詢筆數
                //條件2:超出最長等待時間
                bool Flag_Satisfy = false;

                while (item.TotalWaitingTime < item.longwait)
                {
                    if (item.requestRecord >= item.record)
                    {
                        Flag_Satisfy = true;    //條件1
                        item.TotalWaitingTime = item.longwait + 1;
                        break;
                    }

                    //排程完成一圈,+1秒
                    Thread.Sleep(1000);
                    item.TotalWaitingTime += 1;
                }
                Flag_Satisfy = true;    //條件2
                #endregion

                #region 滿足條件, 發送訊息給對方
                if (Flag_Satisfy)
                {
                    InputReceive("排程 " + item.PK + " Satisfy record:" + item.requestRecord.ToString() + " waitingTime:" + item.TotalWaitingTime.ToString());

                    #region 歸零
                    item.requestRecord = 0;
                    item.TotalWaitingTime = 0;
                    #endregion

                    int TotalDisconnectTime = item.longdisconnect; //最長連線等待時間
                    int myWaitingTime = 0;              //目前等待時間
                    int Steps = 0;  //流程步驟

                    while (myWaitingTime < TotalDisconnectTime)
                    {

                        //建立連線
                        string targetPK = item.target;
                        Settings_Agent_Item target = ServerCounter.Settings_Agent.Find(a => a.PK == targetPK);
                        switch (Steps)
                        {
                            case 0:
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
                                }
                                else
                                {
                                    throw new Exception("沒設定對象的IP");
                                }

                                //連線成功再繼續
                                if (target.clientSocket.Connected)
                                {
                                    Steps = 1;
                                }
                                break;
                            case 1:
                                //傳送ok訊息給對方
                                //send message
                                InputReceive("排程 " + item.PK + " Send Acknowledge ");
                                target.clientSocket.Send("ScheduleOK parent " + item.PK + Environment.NewLine);

                                Steps = 2;
                                break;
                            case 2:
                                //等待對方回應訊息
                                //waiting for receive message
                                //got message, break while loop
                                if (item.Flag_IGotMessage)
                                {
                                    item.Flag_IGotMessage = false;
                                    myWaitingTime = TotalDisconnectTime + 1;
                                    break;
                                }
                                break;
                        }

                        //排程完成一圈,+1秒
                        Thread.Sleep(1000);
                        myWaitingTime += 1;
                    }

                    //執行指令
                    InputReceive("排程 " + item.PK + " action");

                    if (!item.action.Equals(string.Empty))
                        ServerCounter.ShareClientSocket.Send(item.action + Environment.NewLine);

                    //正確完成
                    InputReceive("排程 " + item.PK + " Finish");
                }
                else
                {
                    InputReceive("排程 " + item.PK + " NotSatisfy record:" + item.requestRecord.ToString() + " waitingTime:" + item.TotalWaitingTime.ToString());

                    //等待下一次檢查
                    item.TotalWaitingTime += item.period;
                }
                #endregion
            }
            catch (Exception ex)
            {
                InputReceive(ex.Message);
            }
            finally
            {
                //Finish_重設排程時間
                CalcNextTime(item);
            }
        }

        #region 排程基本設定

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(object obj)
        {
            Setup();
            Start();
        }

        /// <summary>
        /// 開始運行
        /// </summary>
        public void Start()
        {
            DateTime timeNow;

            while (true)
            {
                if (MyCookies.ScheduleOn)
                {
                    timeNow = DateTime.Now;

                    foreach (Schedule_Item item in scheduleItems)
                    {
                        if (DateTime.Compare(item.NextTime, timeNow) <= 0)
                        {
                            Thread newthread = new Thread(ScheduleStart);
                            newthread.Name = item.PK;
                            newthread.IsBackground = true;
                            scheduleThreads.Add(newthread);
                            newthread.Start(item);
                        }
                    }

                    scheduleThreads.RemoveAll(a => (a.ThreadState & (ThreadState.Stopped)) != 0);
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 停用排程
        /// </summary>
        /// <param name="PK"></param>
        public static void StopSchedule()
        {
            MyCookies.ScheduleOn = false;
            scheduleItems.RemoveAll(a => a != null);
            scheduleThreads.RemoveAll(a => a != null);
            scheduleThreads = new List<Thread>();
        }
        public static void StopSchedule(string PK)
        {
            scheduleItems.RemoveAll(a => a.PK == PK);
        }

        /// <summary>
        /// 重新讀取排程
        /// </summary>
        public void Setup()
        {
            MyCookies.ScheduleOn = true;
            foreach (DataRow dr in ServerCounter.dt_Schedule.Rows)
            {
                Setup(dr);
            }
        }
        public void Setup(DataRow dr)
        {
            bool Flag_newItem = true;

            #region 尋找是否已有排成
            Schedule_Item item_ori = scheduleItems.Find(a => a.PK.Equals(dr["PK"].ToString()));

            if (item_ori != null)
            {
                //Update
                Flag_newItem = false;
            }
            else
            {
                item_ori = new Schedule_Item();
            }

            #region 填入數值
            item_ori.PK = dr["pk"].ToString();
            item_ori.DB = dr["DB"].ToString();

            //週期最低3秒
            if (!dr["period"].ToString().Equals(string.Empty))
            {
                int tmpValue = Convert.ToInt32(dr["period"]);
                if (tmpValue < 1)
                    tmpValue = 1;
                item_ori.period = tmpValue;
            }

            //等待時間不能小於週期
            if (!dr["longwait"].ToString().Equals(string.Empty))
            {
                int tmpValue = Convert.ToInt32(dr["longwait"]);
                if (tmpValue < item_ori.period)
                    tmpValue = item_ori.period;
                item_ori.longwait = tmpValue;
            }
            if (!dr["record"].ToString().Equals(string.Empty))
                item_ori.record = Convert.ToInt32(dr["record"]);
            if (!dr["longdisconnect"].ToString().Equals(string.Empty))
                item_ori.longdisconnect = Convert.ToInt32(dr["longdisconnect"]);

            #region 設定排程日期
            if (!dr["ScheduleSet"].ToString().Equals(string.Empty))
            {
                string[] strlist = dr["ScheduleSet"].ToString().ToLower().Split(' ');
                int startIndex = 0;
                bool DefaultMode = false;
                string parameter = "";

                for (int i = 0; i < strlist.Length; i++)
                {
                    startIndex = strlist[i].IndexOf(":") + 1;

                    //未設定參數, 代入預設值
                    if (startIndex == strlist[i].Length)
                    {
                        DefaultMode = true;
                        parameter = "";
                    }
                    else
                    {
                        DefaultMode = false;
                        parameter = strlist[i].Substring(
                               startIndex
                                , strlist[i].Length - startIndex
                                );
                    }

                    if (!DefaultMode)
                    {
                        if (strlist[i].Contains("day:"))
                        {
                            item_ori.Day = Convert.ToInt32(parameter);
                        }
                        else if (strlist[i].Contains("week:"))
                        {
                            item_ori.Week = new List<int>();

                            string[] week_string = parameter.Split(',');
                            foreach (string week in week_string)
                            {
                                item_ori.Week.Add(Convert.ToInt32(week));
                            }
                        }
                        else if (strlist[i].Contains("starttime:"))
                        {
                            item_ori.StartTime = parameter;
                        }
                        else if (strlist[i].Contains("endtime:"))
                        {
                            item_ori.EndTime = parameter;
                        }
                    }
                }
            }
            #endregion

            item_ori.request = dr["request"].ToString();
            item_ori.action = dr["action"].ToString();
            item_ori.target = dr["target"].ToString();
            item_ori.ScheduleSet = dr["ScheduleSet"].ToString();

            CalcNextTime(item_ori);

            #endregion

            if (Flag_newItem)
            {
                scheduleItems.Add(item_ori);
            }
            #endregion
        }

        /// <summary>
        /// 計算下次執行時間
        /// </summary>
        /// <param name="item"></param>
        public void CalcNextTime(Schedule_Item item)
        {
            //下次執行時間, 從開始時間開始算
            int NextStartTime_Hour = Convert.ToInt32(item.StartTime.Substring(0, 2))
                    , NextStartTime_Min = Convert.ToInt32(item.StartTime.Substring(2, 2))
                    , NextEndTime_Hour = Convert.ToInt32(item.EndTime.Substring(0, 2))
                    , NextEndTime_Min = Convert.ToInt32(item.EndTime.Substring(2, 2));
            DateTime NowTime = DateTime.Now;
            DateTime NextTime = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd 00:00:00.000"));
            DateTime StartTime = NextTime.AddHours(NextStartTime_Hour).AddMinutes(NextStartTime_Min);
            DateTime EndTime = NextTime.AddHours(NextEndTime_Hour).AddMinutes(NextEndTime_Min);

            if (item.Day > 0)
            {
                //指定日期
                //已過指定日期, 加一個月
                if (item.Day < NowTime.Day)
                    NextTime = NextTime.AddMonths(1);

                //將日調成一致
                NextTime = StartTime.AddDays(item.Day - NowTime.Day);
            }
            else
            {
                //指定Week
                //先算出可執行的最小增加天數
                int NextTimeDayOfWeek = (int)NowTime.DayOfWeek;
                int SmallestDiffer = 9
                    , NowDiffer = 0;

                foreach (int itemweek in item.Week)
                {
                    NowDiffer = (itemweek - NextTimeDayOfWeek);
                    if (NowDiffer == 0)
                    {
                        SmallestDiffer = NowDiffer;
                        break;
                    }
                    else if (NowDiffer < 0)
                        NowDiffer = 7 - Math.Abs(NowDiffer);

                    if (NowDiffer <= SmallestDiffer)
                    {
                        SmallestDiffer = NowDiffer;
                    }
                }

                NextTime = StartTime.AddDays(SmallestDiffer);
            }


            TimeSpan span1 = NextTime - NowTime;
            if (span1.TotalDays <= 0)
            {
                //計算目前時間是第幾個週期循環, 再加一個週期
                //ex:   現在時間是10:10, 10分鐘一個循環
                //      距離開始時間0900是, 70/10=7個週期, 下次時間是10:20
                span1 = NowTime - StartTime;

                if (span1.TotalSeconds > 0)
                {
                    int span1Count = ((int)span1.TotalSeconds) / item.period + 1;
                    NextTime = StartTime.AddSeconds(span1Count * item.period);

                    //再度檢查是否在開始時間內, 有-不做調整, 沒有-以開始時間為主
                    bool IsSatisfy = false;
                    while (!IsSatisfy)
                    {
                        //在時間之內
                        if (StartTime.Hour <= NextTime.Hour && (NextTime.Hour <= EndTime.Hour || EndTime.Hour == 0))
                        {
                            IsSatisfy = true;
                        }
                        else
                        {
                            //下次執行時間不符合,又在同一天
                            //直接指定為下一天的起始日期

                            NextTime = StartTime.AddDays(1);
                        }
                    }

                    //計算後的週期如果有換日

                    //ex:   計算出下一次循環是0004
                    //      指定時間:0000~2359 則下次時間是0004
                    //      指定時間:0900~2359 則下次時間是0900
                    span1 = NextTime - NowTime;
                    if (span1.TotalDays > 1)
                    {
                        span1 = NextTime - (StartTime.AddDays(1));
                        if (span1.TotalSeconds < 0)
                            NextTime = StartTime.AddDays(1);
                    }
                }
                else
                {
                    //下次時間比預計開始時間早, 則已開始時間為主
                    NextTime = StartTime;
                }
            }

            item.NextTime = NextTime;
        }
        #endregion

        #region Send/Receive Message顯示
        static string txb_ScheduleStatus_text = "";

        /// <summary>
        /// 接收Message, 放入queue
        /// </summary>
        /// <param name="str1"></param>
        public static void InputReceive(string str1)
        {
            txb_ScheduleStatus_text += DateTime.Now.ToString("MM/dd HH:mm:ss") + " " + str1 + Environment.NewLine;
            if (txb_ScheduleStatus_text.Split('\n').Length > 20)
            {
                int Maxrows = txb_ScheduleStatus_text.Split('\n').Length;
                string temp = txb_ScheduleStatus_text.Remove(0, txb_ScheduleStatus_text.Split('\n')[Maxrows - 20].Length + 1);
                txb_ScheduleStatus_text = null;
                txb_ScheduleStatus_text = temp;
                temp = null;
            }
        }

        /// <summary>
        /// 輸出Receive queue
        /// </summary>
        /// <returns></returns>
        public static string OutputReceive()
        {
            return txb_ScheduleStatus_text;
        }
        #endregion
    }

    /// <summary>
    /// 排程運行中需要的元素
    /// </summary>
    public class Schedule_Item
    {
        //需要設定的變數
        public string PK { get; set; }
        public string DB { get; set; }
        public int period { get; set; }
        public int longwait { get; set; }
        public int record { get; set; }
        public int longdisconnect { get; set; }
        public string request { get; set; }
        public string action { get; set; }
        public string target { get; set; }
        public string ScheduleSet { get; set; }

        //Schedule運行時的變數, 免設定, 可執行中被改變
        public DateTime NextTime { get; set; }
        public int requestRecord { get; set; }
        public int TotalWaitingTime { get; set; }
        public bool Flag_IGotMessage { get; set; }

        //解讀ScheduleSet, 免設定, 不可執行中被改變
        public int Day;
        public List<int> Week;
        public string StartTime;
        public string EndTime;

        public Schedule_Item()
        {
            PK = "";
            DB = "";
            period = 60;
            longwait = 60;
            record = 0;
            longdisconnect = 60;
            request = "";
            action = "";
            target = "";
            ScheduleSet = "";

            NextTime = DateTime.MinValue;
            requestRecord = -1;
            TotalWaitingTime = 0;
            Flag_IGotMessage = false;

            Day = 0;
            Week = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
            StartTime = "0000";
            EndTime = "2400";
        }
    }

}
