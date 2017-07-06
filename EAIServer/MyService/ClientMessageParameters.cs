using System.Collections.Generic;

namespace AgentAIServer.MyService
{
    public class ClientMessageParameters
    {
        public List<ClientMessageParameter> ParameterList = new List<ClientMessageParameter>();      //系統暫存變數

        #region 變數存取
        /// <summary>
        /// 新增變數, 同名則取代
        /// </summary>
        /// <param name="item"></param>
        public void ParameterListAdd(string _Name, string _Value)
        {
            bool Flag_IsAlreadyExist = false;

            ClientMessageParameter xitem = ParameterList.Find(a => a.Name.Equals(_Name));

            if (xitem != null)
            {
                Flag_IsAlreadyExist = true;
                xitem.Value = _Value;
            }

            if (Flag_IsAlreadyExist == false)
                ParameterList.Add(new ClientMessageParameter(_Name, _Value));
        }
        /// <summary>
        /// 取得變數內容
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string ParameterListGet(string Name)
        {
            string return_str = "";

            ClientMessageParameter xitem = ParameterList.Find(a => a.Name.Equals(Name));

            if (xitem != null)
            {
                return_str = xitem.Value;
            }

            return return_str;
        }
        #endregion
    }


    /// <summary>
    /// 全域變數, 給ClientMessage使用
    /// </summary>
    public class ClientMessageParameter
    {
        public string Name;
        public string Value;
        public ClientMessageParameter() { }
        public ClientMessageParameter(string _Name, string _Value)
        {
            Name = _Name;
            Value = _Value;
        }
    }
}
