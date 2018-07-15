using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CachePool : ICachePool
    {
        private object tableLock = new object();
        /// <summary>
        /// 缓存表
        /// </summary>
        Dictionary<Type, CacheTable> CacheTables { get; set; }

        /// <summary>
        /// 获取缓存表
        /// </summary>
        /// <typeparam name="TCacheTableType"></typeparam>
        /// <returns></returns>
        public CacheTable Get<ITCacheTable>()
        where ITCacheTable : ICacheTable<ICacheItem>
        {
            if (CacheTables.ContainsKey(typeof(ITCacheTable)))
            {
                throw new Exception("不存在的缓存表");
            }
            return CacheTables[typeof(ITCacheTable)];
        }
        /// <summary>
        /// 注册缓存表
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="iTable"></param>
        /// <param name="table"></param>
        public void Register<ITCacheTable, TCacheTable>() 
        where ITCacheTable: ICacheTable<ICacheItem>
        where TCacheTable: CacheTable, new()
        {
            if (CacheTables.ContainsKey(typeof(ITCacheTable)))
            {
                return;
            }
            lock (tableLock)
            {
                if (CacheTables.ContainsKey(typeof(ITCacheTable)))
                {
                    return;
                }
                CacheTables[typeof(ITCacheTable)] = new TCacheTable();
            }
        }
    }
}
