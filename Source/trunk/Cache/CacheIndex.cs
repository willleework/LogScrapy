using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 索引
    /// </summary>
    public class CacheIndex : ICacheIndex
    {
        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexName { get; set; }
        /// <summary>
        /// 索引类型
        /// </summary>
        public IndexType IndexType { get; set; }
        /// <summary>
        /// 获取索引主键
        /// </summary>
        public Func<ICacheItem, string> GetIndexKey { get; set; }
    }
}
