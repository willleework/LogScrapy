using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cache
{
    /// <summary>
    /// 数据块
    /// </summary>
    internal class DataBlock : IDataBlock<ICacheItem>
    {
        ReaderWriterLockSlim _blockLock = new ReaderWriterLockSlim();
        private ICacheIndex _index;
        private ICacheIndex _uniqueIndex;
        /// <summary>
        /// 索引
        /// </summary>
        public ICacheIndex Index => _index;

        private List<ICacheItem> _datas = new List<ICacheItem>();
        /// <summary>
        /// 数据块队列
        /// </summary>
        public List<ICacheItem> Datas => _datas;

        private Dictionary<string, List<ICacheItem>> _block = new Dictionary<string, List<ICacheItem>>();
        /// <summary>
        /// 数据块字典
        /// </summary>
        public Dictionary<string, List<ICacheItem>> Block => _block;

        /// <summary>
        /// 唯一索引
        /// </summary>
        private Dictionary<string, ICacheItem> _uniqueBlock = new Dictionary<string, ICacheItem>();

        /// <summary>
        /// 添加缓存项
        /// 索引重复，会抛出异常
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(ICacheItem item)
        {
            string blockKey = _index.GetIndexKey(item);
            string uniqueKey = _uniqueIndex.GetIndexKey(item);
            try
            {
                _blockLock.EnterUpgradeableReadLock();
                if (_uniqueBlock.ContainsKey(uniqueKey))
                {
                    _blockLock.ExitUpgradeableReadLock();
                    throw new Exception(string.Format("索引重复，数据已存在：唯一索引:{0}，区块索引:{1}", uniqueKey, blockKey));
                }
                try
                {
                    _blockLock.EnterWriteLock();
                    if (!_block.ContainsKey(blockKey))
                    {
                        _block.Add(blockKey, new List<ICacheItem>());
                    }
                    _block[blockKey].Add(item);
                    _uniqueBlock.Add(uniqueKey, item);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _blockLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _blockLock.ExitUpgradeableReadLock();
            }
            return true;
        }

        /// <summary>
        /// 根据索引获取缓存项
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<ICacheItem> Get(string index)
        {
            List<ICacheItem> datas = new List<ICacheItem>();
            try
            {
                _blockLock.EnterReadLock();
                if (!_block.ContainsKey(index))
                {
                    return datas;
                }
                datas.AddRange(_block[index]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _blockLock.ExitReadLock();
            }
            return datas;
        }

        /// <summary>
        /// 删除缓存项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ICacheItem Remove(ICacheItem item)
        {
            string blockKey = _index.GetIndexKey(item);
            string uniqueKey = _uniqueIndex.GetIndexKey(item);
            try
            {
                _blockLock.EnterUpgradeableReadLock();
                if (_uniqueBlock.ContainsKey(uniqueKey))
                {
                    _blockLock.ExitUpgradeableReadLock();
                    throw new Exception(string.Format("索引重复，数据已存在：唯一索引:{0}，区块索引:{1}", uniqueKey, blockKey));
                }
                try
                {
                    _blockLock.EnterWriteLock();
                    if (!_block.ContainsKey(blockKey))
                    {
                        _block.Add(blockKey, new List<ICacheItem>());
                    }
                    _block[blockKey].Remove(item);
                    _uniqueBlock.Remove(uniqueKey);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _blockLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _blockLock.ExitUpgradeableReadLock();
            }
            return item;
        }

        /// <summary>
        /// 更新缓存项目
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public ICacheItem Update(string index, ICacheItem item)
        {
            string blockKey = _index.GetIndexKey(item);
            string uniqueKey = _uniqueIndex.GetIndexKey(item);
            ICacheItem des;
            try
            {
                _blockLock.EnterUpgradeableReadLock();
                if (_uniqueBlock.ContainsKey(uniqueKey))
                {
                    _blockLock.ExitUpgradeableReadLock();
                    throw new Exception(string.Format("索引重复，数据已存在：唯一索引:{0}，区块索引:{1}", uniqueKey, blockKey));
                }
                try
                {
                    _blockLock.EnterWriteLock();
                    if (!_block.ContainsKey(blockKey))
                    {
                        _block.Add(blockKey, new List<ICacheItem>());
                    }
                    des = _uniqueBlock[uniqueKey];
                    item.Copy(des);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _blockLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _blockLock.ExitUpgradeableReadLock();
            }
            return item;
        }
    }
}
