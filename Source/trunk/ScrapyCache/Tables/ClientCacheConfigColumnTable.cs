using Cache;
using System;
using System.Collections.Generic;
using System.Data;
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
        /// 按表名称索引
        /// </summary>
        public static string IndexByTable => "Table";

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
            CacheIndex indexByTable = new CacheIndex()
            {
                IndexName = IndexByTable,
                IndexType = IndexType.非唯一索引,
                GetIndexKey = GetTableIndex
            };
            AddIndex(indexByTable);
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

        /// <summary>
        /// 获取索引（按表名）
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetTableIndex(ICacheItem item)
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
            return cache.表名;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadDatas(DataTable datas)
        {
            foreach (DataRow row in datas.Rows)
            {
                try
                {
                    bool uniqueKey = false;
                    bool.TryParse(row["主键"].ToString(), out uniqueKey);
                    ClientCacheConfigColumn config = new ClientCacheConfigColumn()
                    {
                        表名 = row["表名"].ToString(),
                        标准字段 = row["标准字段"].ToString(),
                        标准字段名称 = row["标准字段名称"].ToString(),
                        数据类型 = row["数据类型"].ToString(),
                        主键 = uniqueKey,
                    };
                    this.Add(config);
                }
                catch (Exception ex)
                {
                    //TODO: 异常处理
                }
            }
        }
    }
}
