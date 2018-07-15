using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 本地缓存池
    /// </summary>
    public interface ICachePool
    {
        /// <summary>
        /// 注册缓存表
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="iTable"></param>
        /// <param name="table"></param>
        void Register<ITCacheTable, TCacheTable>()
        where ITCacheTable : ICacheTable<ICacheItem>
        where TCacheTable : CacheTable, new();

        /// <summary>
        /// 获取缓存表
        /// </summary>
        /// <typeparam name="TCacheTableType"></typeparam>
        /// <returns></returns>
        CacheTable Get<ITCacheTable>()
        where ITCacheTable : ICacheTable<ICacheItem>;
    }
}
