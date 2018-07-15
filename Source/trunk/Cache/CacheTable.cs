using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 缓存表基类
    /// 至少调用AddIndex函数添加唯一索引
    /// </summary>
    public class CacheTable : ICacheTable<ICacheItem>
    {
        //TODO：读写所替换
        private object _tableLock = new object();
        private ICacheIndex _uniqueIndex;
        /// <summary>
        /// 缓存表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 唯一索引
        /// </summary>
        protected ICacheIndex UniqueIndex => _uniqueIndex;

        private List<ICacheIndex> _indexs = new List<ICacheIndex>();
        private Dictionary<string, ICacheIndex> _indexDiction = new Dictionary<string, ICacheIndex>();
        /// <summary>
        /// 索引字典
        /// </summary>
        protected Dictionary<string, ICacheIndex> IndexDiction => _indexDiction;

        protected Dictionary<string, IDataBlock<ICacheItem>> _dataRegion = new Dictionary<string, IDataBlock<ICacheItem>>();
        /// <summary>
        /// 数据区块
        /// </summary>
        protected Dictionary<string, IDataBlock<ICacheItem>> DataRegion => _dataRegion;

        /// <summary>
        /// 添加索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool AddIndex(ICacheIndex index)
        {
            lock (_indexs)
            {
                if (_indexDiction.ContainsKey(index.IndexName))
                {
                    throw new Exception(string.Format("索引名称重复：{0}", index.IndexName));
                }
                _indexDiction[index.IndexName] = index;
                _indexs.Add(index);
                if (index.IndexType == IndexType.唯一索引)
                {
                    if (_uniqueIndex == null)
                    {
                        _uniqueIndex = index;
                    }
                    else
                    {
                        throw new Exception(string.Format("唯一索引已存在：{0}，重复设置值：{1}", _uniqueIndex.IndexName, index.IndexName));
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取所有索引
        /// </summary>
        /// <returns></returns>
        public List<ICacheIndex> GetAllIndexs()
        {
            return _indexs;
        }

        /// <summary>
        /// 获取指定索引
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public ICacheIndex GetIndex(string indexName)
        {
            lock (_tableLock)
            {
                if (!_indexDiction.ContainsKey(indexName))
                {
                    return null;
                }
                return _indexDiction[indexName];
            }
        }

        /// <summary>
        /// 根据索引获取缓存
        /// 不指定索引名称，则按照唯一索引
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="indexName">索引名称</param>
        /// <returns></returns>
        public List<ICacheItem> Get(string key, string indexName = "")
        {
            CheckUniqueIndex();
            if (string.IsNullOrWhiteSpace(indexName))
            {
                indexName = _uniqueIndex.IndexName;
            }
            List<ICacheItem> datas = new List<ICacheItem>();
            if (!_dataRegion.ContainsKey(indexName))
            {
                return datas;
            }
            datas.AddRange(_dataRegion[indexName].Get(indexName));
            return datas;
        }

        /// <summary>
        /// 根据索引获取缓存
        /// 不指定索引名称，则按照唯一索引
        /// 缓存不存在，则增加，item为空时，增加一个无效的占位空值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item">不存在时添加值</param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public List<ICacheItem> GetOrAdd(string key, ICacheItem item = null, string indexName = "")
        {
            List<ICacheItem> datas = Get(key, indexName);
            if (datas.Count <= 0 && item!= null)
            {
                Add(item);
            }
            return datas;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public ICacheItem Add(ICacheItem item)
        {
            CheckUniqueIndex();
            lock (_tableLock)
            {
                foreach (ICacheIndex index in _indexs)
                {
                    _dataRegion[index.IndexName].Add(item);
                }
                return item;
            }
        }

        /// <summary>
        /// 更新数据
        /// 不存在时不更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ICacheItem Update(ICacheItem item)
        {
            CheckUniqueIndex();
            string uniqueKey = _uniqueIndex.GetIndexKey(item);

            List<ICacheItem> datas = Get(uniqueKey);
            if (datas.Count <= 0)
            {
                return null;
            }
            item.Copy(datas[0]);
            return datas[0];
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public ICacheItem Remove(ICacheItem item)
        {
            CheckUniqueIndex();
            lock (_tableLock)
            {
                foreach (ICacheIndex index in _indexs)
                {
                    _dataRegion[index.IndexName].Remove(item);
                }
                return item;
            }
        }

        /// <summary>
        /// 唯一索引检查
        /// </summary>
        private void CheckUniqueIndex()
        {
            if (_uniqueIndex == null)
            {
                throw new Exception(string.Format("缓存表【{0}】主键不存在错误", TableName));
            }
        }
    }
}
