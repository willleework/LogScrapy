using Cache;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 日志缓存
    /// </summary>
    public class LogInfoRow : CacheItem, ILogInfoRow
    {
        private string _guid = string.Empty;
        /// <summary>
        /// 唯一码
        /// </summary>
        public string Guid { get { return _guid; } }
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

        /// <summary>
        /// 日志缓存
        /// </summary>
        public LogInfoRow()
        {
            _guid = CommonHelper.GetGuid();
        }

        /// <summary>
        /// 拷贝
        /// 不更新唯一ID
        /// </summary>
        /// <param name="des"></param>
        public override void Copy(ICacheItem des)
        {
            base.Copy(des);
            LogInfoRow model = des as LogInfoRow;
            //model._guid = this.Guid;
            model.Level = this.Level;
            model.TimeStamp = this.TimeStamp;
            model.DataInfo = this.DataInfo;
        }

        /// <summary>
        /// 类信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ClientCacheConfig]时间戳:{0}, 日志级别:{1}", this.TimeStamp, this.Level);
        }
    }
}
