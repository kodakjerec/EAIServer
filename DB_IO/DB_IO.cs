using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace DB_IO
{
    public class Connect
    {
        public string rootFolder = "";
        private DataTable dt_Database = new DataTable();
        private int ConnectTimeOut = 0;

        /// <summary>
        /// 起始
        /// </summary>
        public Connect()
        {
            string OriginFolder = "";

            //判斷是Asp.net 還是 Winform 呼叫
            Assembly assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                //Asp.net Path
                OriginFolder = new System.Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath;
            }
            else
            {
                //Winform Path
                OriginFolder = Assembly.GetExecutingAssembly().Location;
            }
            //Final Path
            rootFolder = System.IO.Path.GetDirectoryName(OriginFolder) +"\\";
            reConnect();
        }

        /// <summary>
        /// 讀取文字檔更新
        /// </summary>
        private void reConnect()
        {
            try
            {
                StreamReader sr = new StreamReader(rootFolder + "Settings.txt", Encoding.GetEncoding("utf-8"));
                DataTable dt_Settings = JSONconvert.JSONstrToDataTable(sr.ReadToEnd());
                sr.Close();

                string FilePath = rootFolder + dt_Settings.Rows[0]["DB_FilePath"].ToString() + dt_Settings.Rows[0]["DB_FileName"].ToString();
                sr = new StreamReader(FilePath, Encoding.GetEncoding("utf-8"));
                dt_Database= JSONconvert.JSONstrToDataTable(sr.ReadToEnd());
                sr.Close();
            }
            catch (Exception ex)
            {
                recLog(ex.Message);
            }
        }

        #region Log錯誤處理
        static object lockobj = new object();
        /// <summary>
        /// Log寫入備份檔
        /// </summary>
        public void recLog(string Message)
        {
            string type = "DB_IO_Error";
            Message = DateTime.Now.ToString("MM/dd HH:mm:ss") + " " + Message;
            string Folder = rootFolder + "Log";
            Directory.CreateDirectory(Folder);
            string path = Folder + "\\" + type + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            lock (lockobj)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.Write(Message + Environment.NewLine);
                        writer.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        #endregion

        #region 資料庫
        /// <summary>
        /// 資料庫連結字串
        /// </summary>
        /// <returns></returns>
        public string strCon(string DB)
        {
            string strConn = string.Empty;
            DataRow[] dr = dt_Database.Select("PK='" + DB + "'");
            if (dr.Length <= 0)
            {
                reConnect();
                dr = dt_Database.Select("PK='" + DB + "'");
                if (dr.Length <= 0)
                {
                    recLog("找不到連結字串 " + DB);
                    return "";
                }
            }
            strConn = dr[0][1].ToString();
            return strConn;
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="strDB">資料庫名稱</param>
        /// <param name="sqlcom">SLQ_Commit</param>
        /// <param name="Prm">參數</param>
        /// <returns></returns>
        public DataSet SqlQuery(string strDB, string sqlcom, Hashtable Prm)
        {
            DataSet ds = new DataSet();
            string str_Conn = strCon(strDB);
            SqlConnection Conn = new SqlConnection(str_Conn);
            try
            {
                SqlCommand com = new SqlCommand(sqlcom, Conn);
                com.CommandTimeout = ConnectTimeOut;
                foreach (DictionaryEntry entry in Prm)
                {
                    string strKey = entry.Key.ToString();
                    string strName = entry.Value.GetType().Name.ToUpper();
                    SqlParameter P;
                    switch (strName)
                    {
                        case "BYTE[]":
                            P = new SqlParameter(strKey, SqlDbType.VarBinary); break;
                        case "DATETIME":
                            P = new SqlParameter(strKey, SqlDbType.DateTime); break;
                        default:
                            P = new SqlParameter(strKey, SqlDbType.VarChar); break;
                    }
                    P.Value = entry.Value;
                    com.Parameters.Add(P);
                }
                Conn.Open();
                SqlDataAdapter dapter = new SqlDataAdapter(com);
                dapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// 更新/刪除
        /// </summary>
        /// <param name="strDB">資料庫名稱</param>
        /// <param name="sqlcom">SLQ_Commit</param>
        /// <param name="Prm">參數</param>
        /// <param name="intItems">異動筆數</param>
        /// <returns></returns>
        public bool SqlUpdate(string strDB, string sqlcom, Hashtable Prm, ref int intItems)
        {
            string str_Conn = strCon(strDB);
            SqlConnection Conn = new SqlConnection(str_Conn);
            bool booUpdate = false;
            intItems = 0;
            try
            {
                SqlCommand com = new SqlCommand(sqlcom, Conn);
                com.CommandTimeout = ConnectTimeOut;
                foreach (DictionaryEntry entry in Prm)
                {
                    string strKey = entry.Key.ToString();
                    string strName = entry.Value.GetType().Name.ToUpper();
                    SqlParameter P;
                    switch (strName)
                    {
                        case "BYTE[]":
                            P = new SqlParameter(strKey, SqlDbType.VarBinary); break;
                        case "DATETIME":
                            P = new SqlParameter(strKey, SqlDbType.DateTime); break;
                        default:
                            P = new SqlParameter(strKey, SqlDbType.VarChar); break;
                    }
                    P.Value = entry.Value;
                    com.Parameters.Add(P);
                }
                Conn.Open();
                intItems = com.ExecuteNonQuery();
                booUpdate = true;
            }
            catch
            {
                booUpdate = false;
                throw;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return booUpdate;
        }

        /// <summary>
        /// Transaction 更新/刪除
        /// </summary>
        /// <param name="strDB"></param>
        /// <param name="sqlcom"></param>
        /// <param name="Prm"></param>
        /// <returns></returns>
        public bool SqlBeginTran(string strDB, string[] sqlcom, Hashtable[] Prm)
        {
            string str_Conn = strCon(strDB);
            bool booBeginTran = false;
            SqlConnection Conn = new SqlConnection(str_Conn);
            Conn.Open();
            SqlTransaction Trans = Conn.BeginTransaction();
            SqlCommand com = new SqlCommand();
            com.CommandTimeout = ConnectTimeOut;
            com.Connection = Conn;
            com.Transaction = Trans;

            try
            {
                for (int i = 0; i < sqlcom.Length; i++)
                {
                    com.CommandText = sqlcom[i];
                    foreach (DictionaryEntry entry in Prm[i])
                    {
                        string strKey = entry.Key.ToString();
                        string strName = entry.Value.GetType().Name.ToUpper();
                        SqlParameter P;
                        switch (strName)
                        {
                            case "BYTE[]":
                                P = new SqlParameter(strKey, SqlDbType.VarBinary); break;
                            case "DATETIME":
                                P = new SqlParameter(strKey, SqlDbType.DateTime); break;
                            default:
                                P = new SqlParameter(strKey, SqlDbType.VarChar); break;
                        }
                        P.Value = entry.Value;
                        com.Parameters.Add(P);
                    }
                    com.ExecuteNonQuery();
                    com.Parameters.Clear();
                }
                Trans.Commit();
                booBeginTran = true;
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                booBeginTran = false;
                throw ex;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return booBeginTran;
        }

        /// <summary>
        /// 執行預存程序
        /// </summary>
        /// <param name="strDB">資料庫名稱</param>
        /// <param name="SpName">SP名稱</param>
        /// <param name="Prm">參數</param>
        /// <param name="OutPrm">OutPut參數</param>
        /// <returns></returns>
        public DataSet SqlSp(string strDB, string SpName, Hashtable Prm, ref Hashtable OutPrm)
        {
            DataSet Ds = new DataSet();
            string str_Conn = strCon(strDB);
            SqlConnection Conn = new SqlConnection(str_Conn);
            SqlCommand com = new SqlCommand(SpName, Conn);
            com.CommandTimeout = ConnectTimeOut;
            com.CommandType = CommandType.StoredProcedure;

            try
            {
                ArrayList arrKey = new ArrayList();

                foreach (DictionaryEntry entry in Prm)
                {
                    string strKey = entry.Key.ToString();
                    string strName = entry.Value.GetType().Name.ToUpper();
                    SqlParameter P;
                    switch (strName)
                    {
                        case "BYTE[]":
                            P = new SqlParameter(strKey, SqlDbType.VarBinary); break;
                        case "DATETIME":
                            P = new SqlParameter(strKey, SqlDbType.DateTime); break;
                        case "DATATABLE":
                            P = new SqlParameter(strKey, SqlDbType.Structured); break;
                        default:
                            P = new SqlParameter(strKey, SqlDbType.VarChar); break;
                    }
                    P.Value = entry.Value;
                    com.Parameters.Add(P);
                }
                foreach (DictionaryEntry entry in OutPrm)
                {

                    string strKey = entry.Key.ToString();
                    SqlParameter rc = new SqlParameter(strKey, SqlDbType.VarChar, 500);
                    rc.Direction = ParameterDirection.Output;
                    com.Parameters.Add(rc);
                    arrKey.Add(strKey);
                }
                com.Connection.Open();
                SqlDataAdapter dapter = new SqlDataAdapter(com);
                dapter.Fill(Ds);
                if (OutPrm.Count > 0)
                {
                    for (int i = 0; i < arrKey.Count; i++)
                    {
                        int intPrm = Prm.Count + i;
                        string HsKey = arrKey[i] == null ? "" : arrKey[i].ToString();
                        OutPrm[HsKey] = com.Parameters[intPrm].Value == null ? "" : com.Parameters[intPrm].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
            return Ds;
        }
        #endregion
    }
}
