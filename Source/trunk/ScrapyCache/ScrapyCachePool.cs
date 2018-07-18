using Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 缓存池
    /// </summary>
    public class ScrapyCachePool: CachePool
    {
        /// <summary>
        /// Debug级别日志 
        /// </summary>
        public static Action<string> LogDebugEvent;
        /// <summary>
        /// Info级别日志
        /// </summary>
        public static Action<string> LogInfoEvent;
        /// <summary>
        /// Error级别日志
        /// </summary>
        public static Action<string> LogErrorEvent;

        /// <summary>
        /// 数据池初始化
        /// </summary>
        public void Init()
        {
            CacheLog.LogDebugEvent += LogDebug;
            CacheLog.LogInfoEvent += LogInfo;
            CacheLog.LogErrorEvent += LogError;
            RegisterTables();
            InitTables();
        }

        /// <summary>
        /// 注册缓存表
        /// </summary>
        private void RegisterTables()
        {
            this.Register<ClientCacheConfigTable>();
            this.Register<ClientCacheConfigColumnTable>();
            this.Register<LogInfoRowTable>();
        }

        /// <summary>
        /// 初始化缓存表
        /// </summary>
        private void InitTables()
        {
            ClientCacheConfigTable cacheConfig = this.Get<ClientCacheConfigTable>();
            cacheConfig.Init();
            ClientCacheConfigColumnTable cacheColumnConfig = this.Get<ClientCacheConfigColumnTable>();
            cacheColumnConfig.Init();
            LogInfoRowTable logInfoRow = this.Get<LogInfoRowTable>();
            logInfoRow.Init();
        }

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
