using System.Net;

namespace AgentAIServer.MyService
{
    public static class MyCookies
    {
        //需要設定的變數
        public static int ConnectTimeOut = 0;
        public static bool Log = true;
        public static int Port = 0;
        public static bool ScheduleOn = false;

        //動態產生
        public static IPAddress host = null;

    }
}
