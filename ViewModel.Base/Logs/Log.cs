using log4net;
using log4net.Repository.Hierarchy;
using NLog;
using System;
using System.Runtime.CompilerServices;

namespace ViewModel.Base
{
    public class Log
    {
        private static readonly NLog.Logger fileLogger = NLog.LogManager.GetLogger("info");
        public static void File(string str)
           => fileLogger.Info($"信息：{str}");
        public static void File(Exception ex, string str)
           => fileLogger.Info($"信息：{str}\r\n{ex.StackTrace}");

    }
}