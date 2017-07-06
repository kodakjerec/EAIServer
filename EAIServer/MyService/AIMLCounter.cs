using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace AgentAIServer.MyService
{
    /*
     * 1.翻譯命令
     * 2.分派命令給其他人作業
    */

    /// <summary>
    /// 接收到CMD, 負責分析
    /// </summary>
    class AIMLCounter
    {
        public ClientMessageParameters cmp;
        public Socket MySocket;                //傳送訊息的Client Socket

        /// <summary>
        /// 開始聊天
        /// </summary>
        /// <param name="request">聊天指令</param>
        /// <returns></returns>
        public void Chat(string Fromrequest)
        {
            string keyword = "";

            if (Fromrequest == string.Empty)
                return;

            try
            {
                //字串有幾行
                string[] TotalRows = Fromrequest.Split(new char[] { ';' });

                //判斷命令類別
                for (int i = 0; i < TotalRows.Length; i++)
                {
                    //分解字串
                    //此時再帶入變數
                    List<string[]> rawSentence = this.splitter(TotalRows[i]);

                    //必定要有變數
                    if (rawSentence.Count <= 0)
                        return;
                    if (rawSentence[0].Length <= 0)
                        return;

                    string[] strlist = (string[])rawSentence[0];
                    this.processNode(strlist, ref keyword);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("&lt;" + ex.Message + "&rt;");
            }
        }

        #region 判斷命令類別
        /// <summary>
        /// 判斷命令類別
        /// </summary>
        /// <param name="strlist"></param>
        private void processNode(string[] strlist, ref string keyword)
        {
            AIMLFunctions aimlFunctions = new AIMLFunctions();
            aimlFunctions.MySocket = MySocket;
            aimlFunctions.cmp = cmp;

            #region 辨識命令
            for (int i = 0; i < strlist.Length; i++)
            {
                if (strlist[i].Contains(":"))
                {
                    keyword = strlist[i].ToLower();
                    break;
                }
            }
            if (keyword == string.Empty)
                throw new Exception("無法辨識命令");
            #endregion

            switch (keyword)
            {
                case "sendmessage:":
                    aimlFunctions.SendMesageToTarget(strlist);
                    break;
                case "schedulechange:":
                    aimlFunctions.Schedule_Change(strlist);
                    break;
                case "scheduleset:":
                    aimlFunctions.Schedule_Setup(AIMLFunctions.ScheduleMode.Setup, strlist);
                    break;
                case "schedulestop:":
                    aimlFunctions.Schedule_Setup(AIMLFunctions.ScheduleMode.Stop, strlist);
                    break;
                case "schedulestart:":
                    aimlFunctions.Schedule_Setup(AIMLFunctions.ScheduleMode.Start, strlist);
                    break;
                case "scheduledelete:":
                    aimlFunctions.Schedule_Setup(AIMLFunctions.ScheduleMode.Delete, strlist);
                    break;
                case "databaseset:":
                    aimlFunctions.database_Setup(AIMLFunctions.ScheduleMode.Setup, strlist);
                    break;
                case "databasedelete:":
                    aimlFunctions.database_Setup(AIMLFunctions.ScheduleMode.Delete, strlist);
                    break;
                case "agentset:":
                    aimlFunctions.Agent_Setup(AIMLFunctions.ScheduleMode.Setup, strlist);
                    break;
                case "agentdelete:":
                    aimlFunctions.Agent_Setup(AIMLFunctions.ScheduleMode.Delete, strlist);
                    break;
                case "aimlset:":
                    aimlFunctions.AIML_Setup(AIMLFunctions.ScheduleMode.Setup, strlist);
                    break;
                case "aimldelete:":
                    aimlFunctions.AIML_Setup(AIMLFunctions.ScheduleMode.Delete, strlist);
                    break;
                case "sqlcmd:":
                    aimlFunctions.RunningSQLcmd(strlist);
                    break;
                case "local:":
                    aimlFunctions.RunningLocalExe(strlist);
                    break;
                case "log:":
                    aimlFunctions.log(strlist);
                    break;
                case "resetall:":
                    aimlFunctions.System_Setup();
                    break;
                case "restart:":
                    aimlFunctions.System_Restart();
                    break;
                case "query:":
                    aimlFunctions.System_Query(strlist);
                    break;
                default:
                    throw new Exception("無法辨識系統命令");
            }
        }
        #endregion

        #region 分解字串
        /// <summary>
        /// 分解字串
        /// </summary>
        /// <param name="Fromrequest"></param>
        /// <returns></returns>
        public List<string[]> splitter(string Fromrequest)
        {
            string[] keywords_systemParameter = new string[] { "&" };
            string[] keywords_important = new string[] { "^", ":", ".", " ", "," };
            string[] keywords_string = new string[] { "\"", "'" };
            string[] keywords_separate = new string[] { ";", "\r", "\n", Environment.NewLine };

            List<string> tidyResult = new List<string>();
            List<string[]> stringLists = new List<string[]>();

            string result = ""      //結果字串
                  , code = "";         //被比對的字元
            int LastTimeIndex = 0;  //上一次最後擷取字串的起始位置

            try
            {
                for (int Ln = 0; Ln < Fromrequest.Length; Ln++)
                {

                    code = Fromrequest[Ln].ToString();

                    #region 是否為系統變數
                    foreach (string keywords in keywords_systemParameter)
                    {
                        if (keywords.Equals(code))
                        {
                            if (Fromrequest.Substring(Ln, 4) == "&lt;")
                            {
                                int Final_index = 0
                                    , tmpLn = Ln;
                                Final_index = Fromrequest.IndexOf("&rt;", tmpLn + 1);
                                //可能沒有&rt;
                                //抓取所有資串
                                if (Final_index < 0)
                                {
                                    Final_index = Fromrequest.Length - 1;
                                    result = Fromrequest.Substring(Ln + 4, Final_index - Ln - 4 + 1);
                                    InsertTidyResult(ref tidyResult, result);
                                    Ln = Final_index;
                                    LastTimeIndex = Ln;
                                }
                                else
                                {
                                    //符合, 輸出結果
                                    result = Fromrequest.Substring(Ln + 4, Final_index - Ln - 4);
                                    InsertTidyResult(ref tidyResult, result);
                                    Ln = Final_index + 3;
                                    if (Ln != Fromrequest.Length)
                                        LastTimeIndex = Ln + 1;
                                }
                                goto Finish;
                            }
                        }
                    }
                    #endregion

                    #region 是否為字串
                    foreach (string keywords in keywords_string)
                    {
                        if (keywords.Equals(code))
                        {
                            //符合, 擷取字串
                            int Final_index = 0
                                , tmpLn = Ln;
                            while (true)
                            {
                                Final_index = Fromrequest.IndexOf(keywords, tmpLn + 1);
                                if (Final_index <= 0)
                                {
                                    Final_index = Fromrequest.Length - Ln;
                                    break;
                                }
                                else if (Fromrequest.Substring(Final_index - 1, 1) == "\\")
                                    tmpLn = Final_index;
                                else
                                {
                                    break;
                                }
                            }
                            if (Final_index >= 0)
                            {
                                result = Fromrequest.Substring(Ln, Final_index - Ln + 1);
                                InsertTidyResult(ref tidyResult, result);
                                Ln = Final_index;
                                if (Ln != Fromrequest.Length)
                                    LastTimeIndex = Ln + 1;
                                goto Finish;
                            }
                        }
                    }
                    #endregion

                    #region 是否為系統命令
                    foreach (string keywords in keywords_important)
                    {
                        if (keywords.Equals(code))
                        {
                            //符合, 輸出結果
                            result = Fromrequest.Substring(LastTimeIndex, Ln - LastTimeIndex + 1);
                            if (Ln != Fromrequest.Length)
                                LastTimeIndex = Ln + 1;
                            InsertTidyResult(ref tidyResult, result);
                            goto Finish;
                        }
                    }
                    #endregion

                    #region 是否為斷行
                    foreach (string keywords in keywords_separate)
                    {
                        if (keywords.Equals(code))
                        {
                            //符合, 輸出結果
                            result = Fromrequest.Substring(LastTimeIndex, Ln - LastTimeIndex);
                            if (Ln != Fromrequest.Length)
                                LastTimeIndex = Ln + 1;
                            if (result.Length > 0)
                            {
                                InsertTidyResult(ref tidyResult, result);
                            }
                            InsertStringLists(ref stringLists, ref tidyResult);

                            goto Finish;
                        }
                    }
                #endregion

                Finish:
                    #region 字串結尾
                    if (Ln >= Fromrequest.Length - 1)
                    {
                        //剩餘字串直接輸出結果
                        if (Ln == Fromrequest.Length - 1)
                        {
                            result = Fromrequest.Substring(LastTimeIndex, Ln - LastTimeIndex + 1);
                            if (Ln != Fromrequest.Length)
                                LastTimeIndex = Ln + 1;
                            if (result.Length > 0)
                            {
                                InsertTidyResult(ref tidyResult, result);
                            }
                        }
                        InsertStringLists(ref stringLists, ref tidyResult);
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                Program.recLog(Fromrequest + Environment.NewLine
                                + e.Message + Environment.NewLine
                                + e.StackTrace, "Error");
            }

            return stringLists;
        }
        /// <summary>
        /// 一個命令, 多個變數
        /// </summary>
        /// <param name="tidyResult"></param>
        /// <param name="result"></param>
        private void InsertTidyResult(ref List<string> tidyResult, string result)
        {
            char[] Leftsplitters = new char[] { ' ', ',', '\"' };

            result = result.Trim(Leftsplitters);

            if (result != string.Empty)
            {
                PutParameter(ref result);
                tidyResult.Add(result);
            }
        }
        /// <summary>
        /// 不同命令
        /// </summary>
        /// <param name="stringLists"></param>
        /// <param name="tidyResult"></param>
        private void InsertStringLists(ref List<string[]> stringLists, ref List<string> tidyResult)
        {
            if (tidyResult.Count > 0)
            {
                stringLists.Add(tidyResult.ToArray());
            }
            tidyResult.Clear();
        }

        /// <summary>
        /// 帶入系統參數
        /// 系統帶入的參數內容, 前面會有&lt; 後面會有&rt;
        /// </summary>
        /// <param name="rawSentence"></param>
        public void PutParameter(ref string rawSentence)
        {
            if (rawSentence.IndexOf("@") < 0)
                return;

            string[] keywords_parameter = new string[] { "@" };
            string[] keywords_important = new string[] { "^", ":", ".", "=", " ", "," };
            string[] keywords_string = new string[] { "\"", "'" };
            string[] keywords_separate = new string[] { ";", Environment.NewLine };

            string result = ""      //結果字串
                  , code = "";         //被比對的字元
            int LastTimeIndex = 0;  //上一次最後擷取字串的起始位置
            int FinalIndex = 0; //最後位置

            bool Flag_IsParameterMode = false;  //進入變數模式

            #region 字串檢查是否有變數 "@"
            for (int k = 0; k < rawSentence.Length; k++)
            {
                //取出一個字元
                code = rawSentence[k].ToString();

                if (Flag_IsParameterMode == false)
                {
                    foreach (string keyword in keywords_parameter)
                    {
                        if (keyword.Equals(code))
                        {
                            LastTimeIndex = k;
                            Flag_IsParameterMode = true;
                        }
                    }
                }
                else
                {
                    //判斷字元是否為重要字元
                    foreach (string keyword_2 in keywords_important)
                    {
                        if (keyword_2.Equals(code))
                        {
                            FinalIndex = k;
                            goto FindParameter_Finish;
                        }
                    }
                    foreach (string keyword_2 in keywords_string)
                    {
                        if (keyword_2.Equals(code))
                        {
                            FinalIndex = k;
                            goto FindParameter_Finish;
                        }
                    }
                    foreach (string keyword_2 in keywords_separate)
                    {
                        if (keyword_2.Equals(code))
                        {
                            FinalIndex = k;
                            goto FindParameter_Finish;
                        }
                    }
                    if (k == rawSentence.Length - 1)
                    {
                        //符合, 輸出結果
                        FinalIndex = k + 1;
                        goto FindParameter_Finish;
                    }
                }
            }
        #endregion

        #region 變數取代字串內的"@"
        FindParameter_Finish:
            if (Flag_IsParameterMode)
            {
                bool Flag_ReplaceSomeThing = false;
                result = rawSentence.Substring(LastTimeIndex, FinalIndex - LastTimeIndex);

                bool Flag_NextCharacterEquals_Equal = false;
                try
                {
                    if (rawSentence.Substring(FinalIndex, 2) == "=@")
                    {
                        Flag_NextCharacterEquals_Equal = true;
                    }
                }
                catch { }

                //下一個字元是"=", 表示為設定參數
                if (Flag_NextCharacterEquals_Equal)
                {
                    //把整行字串都取代為空白
                    rawSentence = rawSentence.Remove(LastTimeIndex, FinalIndex + 1);

                    string NextString = rawSentence.Substring(LastTimeIndex + 1, rawSentence.Length - (LastTimeIndex + 1));
                    string afterString = NextString;
                    if (NextString.IndexOf("@") >= 0)
                    {
                        PutParameter(ref afterString);
                        rawSentence = rawSentence.Replace(NextString, afterString);
                    }

                    ClientMessageParameter item = new ClientMessageParameter(result, afterString);
                    //ParameterListAdd(item);
                }
                else
                {
                    //取得參數
                    string compare_result = result.Replace("@", "");

                    //取得參數
                    string itemValue = cmp.ParameterListGet(compare_result);

                    if (!itemValue.Equals(string.Empty))
                    {
                        Flag_ReplaceSomeThing = true;
                        rawSentence = rawSentence.Replace(result, itemValue);
                    }

                    //找不到參數, 帶入空白
                    if (Flag_ReplaceSomeThing == false)
                        rawSentence = rawSentence.Replace(result, "");

                    if (rawSentence.IndexOf("@") >= 0)
                    {
                        PutParameter(ref rawSentence);
                    }
                }
            }
            #endregion
        }

        #endregion
    }
}
