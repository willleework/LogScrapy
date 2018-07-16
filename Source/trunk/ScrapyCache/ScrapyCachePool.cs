using Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 缓存池
    /// </summary>
    public class ScrapyCachePool: CachePool
    {
        //private static CachePool _pool = new CachePool();

        /// <summary>
        /// 数据池初始化
        /// </summary>
        public void Init()
        {
            RegisterTables();
            InitTables();
        }

        /// <summary>
        /// 注册缓存表
        /// </summary>
        private void RegisterTables()
        {
            this.Register<ClientCacheConfigTable>();
            this.Register<ClientCacheConfigColumnTable>();
        }

        /// <summary>
        /// 初始化缓存表
        /// </summary>
        private void InitTables()
        {
            ClientCacheConfigTable cacheConfig = this.Get<ClientCacheConfigTable>();
            cacheConfig.Init();
            ClientCacheConfigColumnTable cacheColumnConfig = this.Get<ClientCacheConfigColumnTable>();
            cacheColumnConfig.Init();
        }
    }
}
