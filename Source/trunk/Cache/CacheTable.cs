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

        protected Dictionary<string, DataBlock> _dataRegion = new Dictionary<string, DataBlock>();
        /// <summary>
        /// 数据区块
        /// </summary>
        protected Dictionary<string, DataBlock> DataRegion => _dataRegion;

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
                    CacheLog.LogError(string.Format("尝试添加重复的索引【{0}】{1}，表【{2}】", index.IndexName, index.IndexType, this.TableName));
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
                        CacheLog.LogError(string.Format("尝试重复添加唯一索引【{0}】（原{1}），表【{2}】", index.IndexName, _uniqueIndex.IndexName, this.TableName));
                        throw new Exception(string.Format("唯一索引已存在：{0}，重复设置值：{1}", _uniqueIndex.IndexName, index.IndexName));
                    }
                }
                DataBlock block = new DataBlock(index, _uniqueIndex);
                _dataRegion.Add(index.IndexName, block);
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
        public List<ICacheItem> Get(string key = "", string indexName = "")
        {
            try
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
                datas = _dataRegion[indexName].Get(key);
                return datas;
            }
            catch (Exception ex)
            {
                CacheLog.LogError(string.Format("查询表数据失败：{0}, 表【{1}】;StackTrace:{2}", ex.Message, TableName, ex.StackTrace != null ? ex.StackTrace : ""));
                throw ex;
            }
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
            try
            {
                List<ICacheItem> datas = Get(key, indexName);
                if (datas.Count <= 0 && item != null)
                {
                    Add(item);
                }
                return datas;
            }
            catch (Exception ex)
            {
                CacheLog.LogError(string.Format("向表查询添加数据失败：{0}, 表【{1}】;StackTrace:{2}", ex.Message, TableName, ex.StackTrace!=null? ex.StackTrace:""));
                throw ex;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public ICacheItem Add(ICacheItem item)
        {
            try
            {
                if (item == null)
                {
                    return null;
                }
                CheckUniqueIndex();
                lock (_tableLock)
                {
                    foreach (ICacheIndex index in _indexs)
                    {
                        _dataRegion[index.IndexName].Add(item);
                        //TODO: 此处如果有一个索引失败，都要回滚
                    }
                    return item;
                }
            }
            catch (Exception ex)
            {
                CacheLog.LogError(string.Format("向表添加数据失败：{0}, 表【{1}】;StackTrace:{2}", ex.Message, TableName, ex.StackTrace != null ? ex.StackTrace : ""));
                throw ex;
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
            try
            {
                if (item == null)
                {
                    return null;
                }
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
            catch (Exception ex)
            {
                CacheLog.LogError(string.Format("更新表数据失败：{0}, 表【{1}】;StackTrace:{2}", ex.Message, TableName, ex.StackTrace != null ? ex.StackTrace : ""));
                throw ex;
            }
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
            try
            {
                if (item == null)
                {
                    return null;
                }
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
            catch (Exception ex)
            {
                CacheLog.LogError(string.Format("删除表数据失败：{0}, 表【{1}】;StackTrace:{2}", ex.Message, TableName, ex.StackTrace != null ? ex.StackTrace : ""));
                throw ex;
            }
        }

        /// <summary>
        /// 清空表数据
        /// </summary>
        public void Clear()
        {
            try
            {
                CheckUniqueIndex();
                lock (_tableLock)
                {
                    foreach (ICacheIndex index in _indexs)
                    {
                        _dataRegion[index.IndexName].Clear();
                        //TODO: 此处如果有一个索引失败，都要回滚
                    }
                }
            }
            catch (Exception ex)
            {
                CacheLog.LogError(string.Format("执行清表操作失败：{0}, 表【{1}】;StackTrace:{2}", ex.Message, TableName, ex.StackTrace != null ? ex.StackTrace : ""));
                throw ex;
            }
        }

        /// <summary>
        /// 唯一索引检查
        /// </summary>
        private void CheckUniqueIndex()
        {
            if (_uniqueIndex == null)
            {
                string errorInfo = string.Format("缓存表【{0}】主键不存在错误", TableName);
                CacheLog.LogError(errorInfo);
                throw new Exception(errorInfo);
            }
        }
    }
}
