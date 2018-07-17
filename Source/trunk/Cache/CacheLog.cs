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
        /// <summary>
        /// 注册Debug级别日志事件
        /// </summary>
        public static Action<string> LogDebugEvent;
        /// <summary>
        /// 注册Info级别日志事件
        /// </summary>
        public static Action<string> LogInfoEvent;
        /// <summary>
        /// 注册Error级别日志事件
        /// </summary>
        public static Action<string> LogErrorEvent;

        /// <summary>
        /// debug级别日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="sender"></param>
        internal static void LogDebug(string log)
        {
            LogDebugEvent?.Invoke(log);
        }

        /// <summary>
        /// Info级别日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="sender"></param>
        internal static void LogInfo(string log)
        {
            LogInfoEvent?.Invoke(log);
        }

        /// <summary>
        /// Error级别日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="sender"></param>
        internal static void LogError(string log)
        {
            LogErrorEvent?.Invoke(log);
        }
    }
}
