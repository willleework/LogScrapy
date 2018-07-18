using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 日志信息接口
    /// </summary>
    public interface ILogInfoRow
    {
        string Guid { get; }
        /// <summary>
        /// 日志级别
        /// </summary>
        string Level { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        string TimeStamp { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        string DataInfo { get; set; }
    }
}
