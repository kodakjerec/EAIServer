using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;

namespace AgentAIServer.MyService
{
    /*
     * 關鍵字 reset database schedule
     * 角色 服務員ServiceCore
     * 任務:重新開機
     */
    class ServiceFunctions
    {
        /// <summary>
        /// 重新啟動服務
        /// </summary>
        public void reset()
        {
            //重啟AIML清單
            getList_AIML();

            //重新讀取MyCookies
            getList_MyCookies();

            //重啟連線設定
            getList_database();

            //重啟排程設定
            getList_Schedule();

            //重啟代理人設定
            getList_Agent();

            //將本機IP寫入設定檔
            foreach (Settings_Agent_Item item in ServerCounter.Settings_Agent)
            {
                if (item.PK == "local"
                    && (item.IP != MyCookies.host.ToString() || item.Port != MyCookies.Port))
                {
                    item.IP = MyCookies.host.ToString();
                    item.Port = MyCookies.Port;
                    setList_Agent();
                }
            }
        }

        #region 讀取設定檔
        /// <summary>
        /// 重新讀取AIML指令集
        /// </summary>
        private void getList_AIML()
        {
            try
            {
                StreamReader sr = new StreamReader("JSON\\AIML_xml.xml", Encoding.GetEncoding("utf-8"));
                string tmpAIMLxml = sr.ReadToEnd();
                sr.Close();
                tmpAIMLxml = tmpAIMLxml.Replace("<=", "&lt;=");
                XmlReader xmlReader = XmlReader.Create(new StringReader(tmpAIMLxml));

                DataSet ds = new DataSet();
                ds.ReadXml(xmlReader);

                ServerCounter.Settings_AIML = ds.Tables[0].AsEnumerable().Select(a => new Settings_AIML_Item()
                {
                    ID = a.Field<string>("ID"),
                    CMD = a.Field<string>("CMD"),
                    SUCCESS = a.Field<string>("SUCCESS"),
                    FAIL = a.Field<string>("FAIL"),
                    MEMO = a.Field<string>("MEMO"),
                }).ToList<Settings_AIML_Item>();

                for (int i = 0; i < ServerCounter.Settings_AIML.Count; i++)
                {
                    for (int j = 0; j < ServerCounter.Settings_AIML.Count; j++)
                    {
                        if (i != j)
                        {
                            if (ServerCounter.Settings_AIML.ElementAt(i).ID == ServerCounter.Settings_AIML.ElementAt(j).ID)
                            {
                                throw new Exception("getList_AIML PK重複");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重新讀取MyCookies
        /// </summary>
        public void getList_MyCookies()
        {
            StreamReader sr = new StreamReader("JSON\\MyCookies_json.txt", Encoding.GetEncoding("utf-8"));
            try
            {
                DataTable dt = DB_IO.JSONconvert.JSONstrToDataTable(sr.ReadToEnd());

                MyCookies.ConnectTimeOut = Convert.ToInt32(dt.Rows[0]["ConnectTimeOut"]);

                if (dt.Rows[0]["Log"].ToString() == "OFF")
                    MyCookies.Log = false;
                else
                    MyCookies.Log = true;

                MyCookies.Port = Convert.ToInt32(dt.Rows[0]["Port"]);

                if (dt.Rows[0]["ScheduleOn"].ToString() == "OFF")
                    MyCookies.ScheduleOn = false;
                else
                    MyCookies.ScheduleOn = true;
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

        /// <summary>
        /// 讀取連線主機清單
        /// </summary>
        public void getList_database()
        {
            if (ServerCounter.dt_Database != null)
                ServerCounter.dt_Database = null;

            StreamReader sr = new StreamReader("DLL\\DB_IO_Database_json.txt", Encoding.GetEncoding("utf-8"));
            try
            {
                ServerCounter.dt_Database = DB_IO.JSONconvert.JSONstrToDataTable(sr.ReadToEnd());
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

        /// <summary>
        /// 讀取排程清單
        /// </summary>
        public void getList_Schedule()
        {
            if (ServerCounter.dt_Schedule != null)
                ServerCounter.dt_Schedule = null;

            StreamReader sr = new StreamReader("JSON\\Schedule_json.txt", Encoding.GetEncoding("utf-8"));
            try
            {
                ServerCounter.dt_Schedule = DB_IO.JSONconvert.JSONstrToDataTable(sr.ReadToEnd());

                for (int i = 0; i < ServerCounter.dt_Schedule.Rows.Count; i++)
                {
                    for (int j = 0; j < ServerCounter.dt_Schedule.Rows.Count; j++)
                    {
                        if (i != j)
                        {
                            if (ServerCounter.dt_Schedule.Rows[i]["PK"].ToString() == ServerCounter.dt_Schedule.Rows[j]["PK"].ToString())
                            {
                                throw new Exception("getList_Schedule PK重複");
                            }
                        }
                    }
                }
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

        /// <summary>
        /// 讀取代理人清單
        /// </summary>
        public void getList_Agent()
        {
            StreamReader sr = new StreamReader("JSON\\Agent_json.txt", Encoding.GetEncoding("utf-8"));
            try
            {
                ServerCounter.Settings_Agent = DB_IO.JSONconvert.JSONstrToList<Settings_Agent_Item>(sr.ReadToEnd());

                for (int i = 0; i < ServerCounter.Settings_Agent.Count; i++)
                {
                    for (int j = 0; j < ServerCounter.Settings_Agent.Count; j++)
                    {
                        if (i != j)
                        {
                            if (ServerCounter.Settings_Agent.ElementAt(i).PK == ServerCounter.Settings_Agent.ElementAt(j).PK)
                            {
                                throw new Exception("getList_Agent PK重複");
                            }
                        }
                    }
                }
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
        #endregion

        #region 寫入設定檔
        /// <summary>
        /// 寫入代理人清單
        /// </summary>
        public void setList_Agent()
        {
            StreamWriter sw = new StreamWriter("JSON\\Agent_json.txt", false, Encoding.GetEncoding("utf-8"));
            try
            {
                DataTable dt= DB_IO.JSONconvert.ListToDataTable<Settings_Agent_Item>(ServerCounter.Settings_Agent);
                dt.Columns.Remove("clientSocket");

                string json = DB_IO.JSONconvert.DataTableToJSONstr(dt);
                sw.WriteLine(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// 寫入命令清單
        /// </summary>
        public void setList_AIML()
        {
            StreamWriter sw = new StreamWriter("JSON\\AIML_xml.xml", false, Encoding.GetEncoding("utf-8"));
            try
            {
                DataTable dt = DB_IO.JSONconvert.ListToDataTable<Settings_AIML_Item>(ServerCounter.Settings_AIML);
                //欄位有DBnull,需替換成空白字元""
                foreach (DataRow dr in dt.Rows) {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dr[i] == DBNull.Value)
                            dr[i] = "";
                    }
                }
                dt.TableName = "AIML";
                dt.WriteXml(sw, XmlWriteMode.IgnoreSchema);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// 寫入資料庫清單
        /// </summary>
        public void setList_database()
        {
            if (ServerCounter.dt_Database == null)
                return;

            StreamWriter sw = new StreamWriter("DLL\\DB_IO_Database_json.txt", false, Encoding.GetEncoding("utf-8"));
            try
            {
                string json = DB_IO.JSONconvert.DataTableToJSONstr(ServerCounter.dt_Database);
                sw.WriteLine(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// 寫入排程清單
        /// </summary>
        public void setList_Schedule()
        {
            if (ServerCounter.dt_Schedule == null)
                return;

            StreamWriter sw = new StreamWriter("JSON\\Schedule_json.txt", false, Encoding.GetEncoding("utf-8"));

            try
            {
                string json = DB_IO.JSONconvert.DataTableToJSONstr(ServerCounter.dt_Schedule);
                sw.WriteLine(json);             
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sw.Close();
            }
        }
        #endregion
    }
}
