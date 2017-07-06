using System.Collections.Generic;
using System;

/*
 * Server收到訊息後, 放入柱列
 * 避免來自同一個IP的多重訊息
 */

namespace AgentAIServer.MyService
{
    /// <summary>
    /// 柱列:來自Client的Message
    /// </summary>
    public static class ClientMessageQueue
    {
        public static List<ClientMessage> clientMsgQueue = new List<ClientMessage>();

        #region 柱列存取
        /// <summary>
        /// 新增clientMessage
        /// true-新增成功 false-新增失敗
        /// </summary>
        /// <param name="item"></param>
        public static ClientMessage Add(string _IP, string _Message)
        {
            ClientMessage obj = new ClientMessage();
            bool Flag_YouAdd = Find(_IP, _Message);
            if (Flag_YouAdd == false)
            {
                obj = new ClientMessage(_IP, _Message);
                clientMsgQueue.Add(obj);
            }

            return obj;
        }

        /// <summary>
        /// 尋找是否有來自相同client的相同Message
        /// true-有 false-沒有
        /// </summary>
        /// <param name="_IP"></param>
        /// <param name="_Message"></param>
        /// <returns></returns>
        public static bool Find(string _IP, string _Message)
        {
            ClientMessage obj = clientMsgQueue.Find(a => a != null && a.IP == _IP && a.Message == _Message);
            if (obj != null)
                return true;

            return false;
        }

        /// <summary>
        /// 刪除柱列中的命令
        /// </summary>
        /// <param name="_IP"></param>
        /// <param name="_Message"></param>
        public static void Remove(string _IP, string _Message)
        {
            try
            {
                clientMsgQueue.RemoveAll(a => a.IP == _IP && a.Message == _Message);
                //清除垃圾:刪除10分鐘前的message
                clientMsgQueue.RemoveAll(a => a.CrtDate <= DateTime.Now.AddMinutes(-10));
            }
            catch
            {
                Program.recLog(_IP + "^" + _Message, "Error");
                clientMsgQueue = new List<ClientMessage>();
            }
        }
        public static void RemoveAll()
        {
            clientMsgQueue.RemoveAll(a => a != null);
            clientMsgQueue = new List<ClientMessage>();
        }
        #endregion
    }

    /// <summary>
    /// 來自Client的Message
    /// </summary>
    public class ClientMessage
    {
        public string IP { get; set; }
        public string Message { get; set; }
        public DateTime CrtDate;

        public ClientMessage()
        { }
        public ClientMessage(string _IP, string _Message)
        {
            IP = _IP;
            Message = _Message;
            CrtDate = DateTime.Now;
        }
    }
}
