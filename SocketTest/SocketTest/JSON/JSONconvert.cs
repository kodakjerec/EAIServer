using Newtonsoft.Json;
using System.Data;

namespace AgentAIServer.JSON
{
    class JSONconvert
    {
        //把DataTable轉成JSON字串
        public static string DataTableToJSONstr(DataTable dt)
        {
            string str_json = "";
            //將DataTable轉成JSON字串
            str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);

            return str_json;
        }

        //把JSON字串轉成DataTable
        public static DataTable JSONstrToDataTable(string str_json)
        {
            DataTable dt = new DataTable();

            //Newtonsoft.Json.Linq.JArray jArray = 
            //    JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JArray>(li_showData.Text.Trim());
            //或
            dt = JsonConvert.DeserializeObject<DataTable>(str_json);

            return dt;
        }
    }
}
