using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    /// <summary>
    /// 日志上下文
    /// </summary>
    public class LogContext: ILogContext
    {
        /// <summary>
        /// 缓存日志
        /// </summary>
        private LogJet _logForCache;
        /// <summary>
        /// 任务管理日志
        /// </summary>
        private LogJet _LogForTask;
        /// <summary>
        /// 通用日志
        /// </summary>
        private LogJet _logForCommon;
        /// <summary>
        /// 缓存日志
        /// </summary>
        public LogJet LogForCache { get => _logForCache; set => _logForCache = value; }
        /// <summary>
        /// 任务管理日志
        /// </summary>
        public LogJet LogForTask { get => _LogForTask; set => _LogForTask = value; }
        /// <summary>
        /// 通用日志
        /// </summary>
        public LogJet LogForCommon { get => _logForCommon; set => _logForCommon = value; }

        ///// <summary>
        ///// 日志上下文
        ///// </summary>
        ///// <param name="path">配置文件路径</param>
        //public LogContext(string path)
        //{
        //    _logForCache = new LogJet("CacheLog", @path);
        //    _LogForTask = new LogJet("TaskLog", @path);
        //    _logForCommon = new LogJet("CommonLog", @path);
        //}

        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="path"></param>
        public void Init(string path)
        {
            _logForCache = new LogJet("CacheLog", @path);
            _LogForTask = new LogJet("TaskLog", @path);
            _logForCommon = new LogJet("CommonLog", @path);
        }
    }
}
