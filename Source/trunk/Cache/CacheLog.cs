using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 缓存日志
    /// </summary>
    public class CacheLog
    {
        public static EventHandler<string> LogDebugEvent;

        public static EventHandler<string> LogInfoEvent;

        public static EventHandler<string> LogErrorEvent;

        /// <summary>
        /// debug级别日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="sender"></param>
        internal static void LogDebug(string log, object sender = null)
        {
            LogDebugEvent?.Invoke(sender, log);
        }

        /// <summary>
        /// Info级别日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="sender"></param>
        internal static void LogInfo(string log, object sender = null)
        {
            LogInfoEvent?.Invoke(sender, log);
        }

        /// <summary>
        /// Error级别日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="sender"></param>
        internal static void LogError(string log, object sender = null)
        {
            LogErrorEvent?.Invoke(sender, log);
        }
    }
}
