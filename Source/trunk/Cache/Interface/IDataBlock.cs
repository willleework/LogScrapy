using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 数据区块
    /// 与指定一个索引对应的数据块
    /// </summary>
    /// <typeparam name="TCacheItem"></typeparam>
    /// <typeparam name=""></typeparam>
    public interface IDataBlock<TCacheItem>
    where TCacheItem : class, ICacheItem
    {
        /// <summary>
        /// 索引
        /// </summary>
        ICacheIndex Index { get; }
        /// <summary>
        /// 数据
        /// </summary>
        List<TCacheItem> Datas { get; }
        /// <summary>
        /// 数据字典
        /// </summary>
        Dictionary<string, List<TCacheItem>> Block { get; }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Add(TCacheItem item);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="index"></param>
        /// <param name="notExistAdd"></param>
        /// <returns></returns>
        List<TCacheItem> Get(string index);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        TCacheItem Update(string index, TCacheItem item);
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        TCacheItem Remove(TCacheItem item);
    }
}
