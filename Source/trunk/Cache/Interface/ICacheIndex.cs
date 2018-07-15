using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 缓存索引
    /// </summary>
    public interface ICacheIndex
    {
        /// <summary>
        /// 索引名称
        /// </summary>
        string IndexName { get; set; }
        /// <summary>
        /// 索引类型
        /// </summary>
        IndexType IndexType { get; set; }

        /// <summary>
        /// 索引匹配模式委托
        /// </summary>
        Func<ICacheItem, string> GetIndexKey { get; set; }
    }
}
