using Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 客户端缓存表列信息表
    /// </summary>
    public class ClientCacheConfigColumnTable : CacheTable, IScrapyCacheTable
    {
        /// <summary>
        /// 唯一索引
        /// </summary>
        public static string IndexUnique => "Unique";

        /// <summary>
        /// 缓存表初始化
        /// </summary>
        public void Init()
        {
            TableName = "客户端缓存列信息表";
            CacheIndex uniqueIndex = new CacheIndex()
            {
                IndexName = IndexUnique,
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetUniqueIndex
            };
            AddIndex(uniqueIndex);
        }

        /// <summary>
        /// 获取唯一索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetUniqueIndex(ICacheItem item)
        {
            if (item == null)
            {
                throw new Exception(string.Format("缓存表【{0}】获取索引错误，缓存项为空", this.TableName));
            }
            ClientCacheConfigColumn cache = item as ClientCacheConfigColumn;
            if (cache == null)
            {
                throw new Exception(string.Format("缓存表【{0}】获取索引错误，缓存项目信息转换错误", this.TableName));
            }
            return string.Format("{0}_{1}", cache.表名, cache.标准字段);
        }
    }
}
