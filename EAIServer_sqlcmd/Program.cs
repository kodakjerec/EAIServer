using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Reflection;

namespace EAIServer_sqlcmd
{
    /*
     AgentAIserver_sqlcmd 修改日期：2016.08.18 v1.0

    搭配AgentAIserver使用，專門用來執行老大發給他的sql指令
    本程式不做任何判斷，只負責執行sql command

    傳入變數：
    args[0]
    連接資料庫名稱，參考Database_json.txt

    args[1]
    執行指令，可以是sp或command
    命令設定的時候可以加參數，老大會過濾掉參數給予數值

    回傳變數：
    有錯誤：
    範例：Error>找不到資料庫

    無錯誤：
    範例：table>JSON字串

     */
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var domain = AppDomain.CurrentDomain;
            domain.AssemblyResolve += LoadAssembly;

            string myAnswer = "";

            try
            {
                if (args.Length != 2)
                {
                    string str_args = "";
                    for (int i = 0; i < args.Length; i++)
                        str_args += args[i] + " ";
                    throw new Exception("args個數不正確" + str_args);
                }

                goSQL(args,ref myAnswer);
            }
            catch (Exception ex)
            {
                myAnswer = "error>" + ex.Message;
            }
            finally
            {
                Console.Write(myAnswer);
            }
        }

        /// <summary>
        /// SQL查詢核心
        /// </summary>
        /// <param name="args"></param>
        /// <param name="myAnswer"></param>
        private static void goSQL(string[] args,ref string myAnswer) {
            DB_IO.Connect db_IO = new DB_IO.Connect();

            int startIndex = 0;

            string LoginServer = "";
            startIndex = args[0].IndexOf(":") + 1;
            LoginServer = args[0].Substring(startIndex, args[0].Length - startIndex);


            string sqlcmd = "";
            startIndex = args[1].IndexOf(":") + 1;
            sqlcmd = args[1].Substring(startIndex, args[1].Length - startIndex);

            DataSet ds = db_IO.SqlQuery(LoginServer, sqlcmd, new Hashtable());
            if (ds.Tables.Count > 0)
                myAnswer = "table>" + DB_IO.JSONconvert.DataTableToJSONstr(ds.Tables[0]);
            else
                myAnswer = "table>";
        }

        static object lockobj = new object();
        /// <summary>
        /// Log寫入備份檔
        /// </summary>
        public static void recLog(string Message, string type)
        {
            Message = DateTime.Now.ToString("MM/dd HH:mm:ss") + " " + Message;

            string path = "log\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + type + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            lock (lockobj)
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(Message, true);
                    writer.Close();
                }
            }
        }

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
    }
}
