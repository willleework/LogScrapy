using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    /// <summary>
    /// 日志上下文接口
    /// </summary>
    public interface ILogContext
    {
        /// <summary>
        /// 缓存日志
        /// </summary>
        LogJet LogForCache { get; }
        /// <summary>
        /// 任务管理日志
        /// </summary>
        LogJet LogForTask { get; }
        /// <summary>
        /// 通用日志
        /// </summary>
        LogJet LogForCommon { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path">配置文件路径</param>
        void Init(string path);
    }
}
