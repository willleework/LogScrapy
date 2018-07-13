using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUtility
{
    /// <summary>
    /// 日志基类
    /// </summary>
    public class LogEntityBase
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public string DataInfo { get; set; }
    }
}
