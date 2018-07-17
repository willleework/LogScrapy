using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class LogJet
    {
        private ILog _log = null;

        /// <summary>
        /// 日志工具
        /// </summary>
        /// <param name="name">日志实例名称</param>
        /// <param name="filepath">日志配置文件</param>
        public LogJet(string name, string filepath)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(@filepath));
            _log = log4net.LogManager.GetLogger(name);
        }

        /// <summary>
        /// info级别日志
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(message);
            }
        }

        /// <summary>
        /// debug级别日志
        /// </summary>
        /// <param name="message"></param>
        public void LogDebug(string message)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(message);
            }
        }

        /// <summary>
        /// error级别日志
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(message);
            }
        }
    }
}
