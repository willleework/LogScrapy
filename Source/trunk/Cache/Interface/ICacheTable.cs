using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 缓存表
    /// </summary>
    /// <typeparam name="TCacheItem"></typeparam>
    public interface ICacheTable<TCacheItem>
    where TCacheItem : class, ICacheItem
    {
        ///// <summary>
        ///// 唯一索引
        ///// </summary>
        //ICacheIndex UniqueIndex { get; set; }
        ///// <summary>
        ///// 索引字典
        ///// </summary>
        //Dictionary<string, ICacheIndex> IndexDiction { get;}
        ///// <summary>
        ///// 数据区块
        ///// </summary>
        //Dictionary<string, IDataBlock<TCacheItem>> DataRegion { get; }

        /// <summary>
        /// 缓存表明
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool AddIndex(ICacheIndex index);
        /// <summary>
        /// 获取全部索引
        /// </summary>
        /// <returns></returns>
        List<ICacheIndex> GetAllIndexs();
        /// <summary>
        /// 获取制定索引
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        ICacheIndex GetIndex(string indexName);

        /// <summary>
        /// 根据索引获取缓存
        /// 不指定索引名称，则按照唯一索引
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="indexName">索引名称</param>
        /// <returns></returns>
        List<TCacheItem> Get(string key = "", string indexName = "");

        /// <summary>
        /// 根据索引获取缓存
        /// 不指定索引名称，则按照唯一索引
        /// 缓存不存在，则增加，item为空时，增加一个无效的占位空值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        List<TCacheItem> GetOrAdd(string key, TCacheItem item = null, string indexName = "");

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        TCacheItem Add(TCacheItem item);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        TCacheItem Update(TCacheItem item);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        TCacheItem Remove(TCacheItem item);
    }
}
