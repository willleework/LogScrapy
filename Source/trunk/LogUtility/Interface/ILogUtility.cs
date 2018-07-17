using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogDecode
{
    /// <summary>
    /// 日志辅助类
    /// </summary>
    public interface ILogUtility
    {
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string ReadLogFile(string filePath);

        /// <summary>
        /// 解析日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        List<LogEntityBase> DecodeLog(string log, string rowPattern, string timePattern);
    }
}
