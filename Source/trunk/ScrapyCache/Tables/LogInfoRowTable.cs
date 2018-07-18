using Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapyCache
{
    public class LogInfoRowTable : CacheTable, IScrapyCacheTable
    {/// <summary>
     /// 唯一索引
     /// </summary>
        public static string IndexUnique => "Unique";

        /// <summary>
        /// 缓存表初始化
        /// </summary>
        public void Init()
        {
            TableName = "客户端日志信息表";
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
            LogInfoRow log = item as LogInfoRow;
            if (log == null)
            {
                throw new Exception(string.Format("缓存表【{0}】获取索引错误，缓存项目信息转换错误", this.TableName));
            }
            return log.Guid;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadDatas(List<LogInfoRow> logRows)
        {
            try
            {
                this.Clear();
                //foreach (LogInfoRow row in logRows)
                //{
                //    this.Add(row);
                //}

                logRows.AsParallel().ForAll(p => 
                {
                    this.Add(p);
                });
            }
            catch (Exception ex)
            {
                ScrapyCachePool.LogError(string.Format("【客户端日志信息表】数据加载错误：{0};StackTrace:{1}", ex.Message, ex.StackTrace!=null ? ex.StackTrace:""));
            }
        }
    }
}
