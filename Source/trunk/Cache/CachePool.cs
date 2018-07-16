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
        private Dictionary<int, CacheTable> _cacheTables = new Dictionary<int, CacheTable>();
        /// <summary>
        /// 缓存表
        /// </summary>
        Dictionary<int, CacheTable> CacheTables => _cacheTables;

        /// <summary>
        /// 获取缓存表
        /// </summary>
        /// <typeparam name="TCacheTableType"></typeparam>
        /// <returns></returns>
        public TCacheTable Get<TCacheTable>()
        where TCacheTable : CacheTable
        {
            int tableHash = typeof(TCacheTable).GetHashCode();
            if (!_cacheTables.ContainsKey(tableHash))
            {
                throw new Exception("不存在的缓存表");
            }
            return _cacheTables[tableHash] as TCacheTable;
        }
        /// <summary>
        /// 注册缓存表
        /// </summary>
        /// <typeparam name="TCacheItem"></typeparam>
        /// <param name="iTable"></param>
        /// <param name="table"></param>
        public void Register<TCacheTable>() 
        //where ITCacheTable: ICacheTable<ICacheItem>
        where TCacheTable: CacheTable, new()
        {
            int tableHash = typeof(TCacheTable).GetHashCode();
            if (_cacheTables.ContainsKey(tableHash))
            {
                return;
            }
            lock (tableLock)
            {
                if (_cacheTables.ContainsKey(tableHash))
                {
                    return;
                }
                _cacheTables[tableHash] = new TCacheTable();
            }
        }
    }
}
