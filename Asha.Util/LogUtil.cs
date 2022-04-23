using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Asha.Util
{
    public class LogUtil
    {
        public static void Debug(MethodBase method, string message)
        {
            Debug(method.DeclaringType, method.Name, message);
        }

        public static void Info(MethodBase method, string message)
        {
            Info(method.DeclaringType, method.Name, message);
        }

        public static void Error(MethodBase method, string message)
        {
            Error(method.DeclaringType, method.Name, message);
        }

        public static void Error(MethodBase method, Exception e)
        {
            Error(method.DeclaringType, method.Name, e.Message + "\n" + e.StackTrace);
        }

        public static void Error(MethodBase method, string message, Exception e)
        {
            Error(method.DeclaringType, method.Name, message + "\n" + e.Message + "\n" + e.StackTrace);
        }


        private static void Debug(Type t, String action, string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Debug("ACTION:" + action + " ;MESSAGE:" + message);
        }

        private static void Info(Type t, String action, string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Info("ACTION:" + action + " ;MESSAGE:" + message);
        }

        private static void Error(Type t, String action, string message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error("ACTION:" + action + " ;MESSAGE:" + message);
        }
    }
}
