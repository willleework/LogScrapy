using Cache;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    /// <summary>
    /// 客户端缓存表信息表
    /// </summary>
    public class ClientCacheConfigTable : CacheTable, IScrapyCacheTable
    {
        /// <summary>
        /// 唯一索引
        /// </summary>
        public static string IndexUnique => "Unique";
        /// <summary>
        /// 按业务索引
        /// </summary>
        public static string IndexByDomain => "DomainIndex";

        /// <summary>
        /// 缓存表初始化
        /// </summary>
        public void Init()
        {
            TableName = "客户端缓存表";
            CacheIndex uniqueIndex = new CacheIndex()
            {
                IndexName = IndexUnique,
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetUniqueIndex
            };
            AddIndex(uniqueIndex);
            CacheIndex domainIndex = new CacheIndex()
            {
                IndexName = IndexByDomain,
                IndexType = IndexType.非唯一索引,
                GetIndexKey = GetDomainIndex,
            };
            AddIndex(domainIndex);
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
            ClientCacheConfig config = item as ClientCacheConfig;
            if (config == null)
            {
                throw new Exception(string.Format("缓存表【{0}】获取索引错误，缓存项目信息转换错误", this.TableName));
            }
            return config.表名;
        }

        /// <summary>
        /// 获取业务分类索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetDomainIndex(ICacheItem item)
        {
            if (item == null)
            {
                throw new Exception(string.Format("缓存表【{0}】获取索引错误，缓存项为空", this.TableName));
            }
            ClientCacheConfig config = item as ClientCacheConfig;
            if (config == null)
            {
                throw new Exception(string.Format("缓存表【{0}】获取索引错误，缓存项目信息转换错误", this.TableName));
            }
            return config.后台组;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadDatas(DataTable datas)
        {
            if (datas == null || datas.Rows == null)
            {
                ScrapyCachePool.LogInfo("【客户端缓存表】数据加载失败：数据集为空");
                return;
            }
            foreach (DataRow row in datas.Rows)
            {
                try
                {
                    int port = -1;
                    int.TryParse(row["初始化功能号"].ToString(), out port);
                    ClientCacheConfig config = new ClientCacheConfig()
                    {
                        表名 = row["表名"].ToString(),
                        英文名 = row["英文名"].ToString(),
                        更新消息主题 = row["更新消息主题"].ToString(),
                        后台组 = row["后台组"].ToString(),
                        数据库表名 = row["数据库表名"].ToString(),
                        端口号 = port,
                    };
                    this.Add(config);
                }
                catch (Exception ex)
                {
                    ScrapyCachePool.LogError(string.Format("【客户端缓存表】数据加载错误：{0};StackTrace:{1}", ex.Message, ex.StackTrace));
                }
            }
        }
    }
}
