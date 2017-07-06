using Newtonsoft.Json;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Reflection;
using System.Linq;
namespace DB_IO
{
    public class JSONconvert
    {
        //把DataTable轉成JSON字串
        public static string DataTableToJSONstr(DataTable dt)
        {
            string str_json = "";
            //將DataTable轉成JSON字串
            str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);

            return str_json;
        }

        //把DataTable轉成List<document>
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType), null);
                    }
                }
                return objT;
            }).ToList();
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

        //把JSON字串轉成List
        public static List<T> JSONstrToList<T>(string tt)
        {
            return JsonConvert.DeserializeObject<List<T>>(tt);
        }

        //把List轉成JSON字串
        public static string ListToJSONstr<T>(List<T> objs)
        {
            return JsonConvert.SerializeObject(objs);
        }

        //把List轉成DataTable
        public static DataTable ListToDataTable<T>(List<T> objs)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in objs)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }
    }
}