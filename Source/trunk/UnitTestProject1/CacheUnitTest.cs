using System;
using System.Collections.Generic;
using Cache;
using Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class CacheUnitTest
    {
        LogJet log1 = new LogJet("ceshi2", @"D:/Log/App.config");
        public interface ITestCache
        {
            int ID { get; set; }
            string Name { get; set; }
        }
        public class TestCacheModel : CacheItem, ITestCache
        {
            public int ID { get; set; }
            public string Name { get; set;}
            /// <summary>
            /// 复制
            /// </summary>
            /// <param name="des"></param>
            public override void Copy(ICacheItem des)
            {
                base.Copy(des);
                TestCacheModel model = des as TestCacheModel;
                model.ID = this.ID;
                model.Name = this.Name;
            }
        }
        /// <summary>
        /// 缓存表接口
        /// </summary>
        public interface ITestTable
        {

        }
        /// <summary>
        /// 缓存表
        /// </summary>
        public class TestTable : CacheTable, ITestTable
        {

        }

        CachePool _pool = new CachePool();

        /// <summary>
        /// 缓存初始化测试
        /// </summary>
        [TestMethod]
        public void CacheTableRegisterTest()
        {
            //注册缓存表
            _pool.Register<TestTable>();
            Assert.IsNotNull(_pool.Get<TestTable>());
        }

        [TestMethod]
        public void CacheTableAddIndexTest()
        {
            _pool.Register<TestTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey= GetKey
            };
            _pool.Get<TestTable>().AddIndex(index);
            List<ICacheIndex> indexs =  _pool.Get<TestTable>().GetAllIndexs();
            Assert.IsTrue(indexs[0].IndexName.Equals("主键测试"));
        }

        /// <summary>
        /// 缓存增加及查询
        /// </summary>
        [TestMethod]
        public void AddCacheItemTest()
        {
            _pool.Register<TestTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetKey
            };
            _pool.Get<TestTable>().AddIndex(index);
            List<ICacheIndex> indexs = _pool.Get<TestTable>().GetAllIndexs();

            TestCacheModel model = new TestCacheModel() { ID = 1, Name = "测试" };
            _pool.Get<TestTable>().Add(model);
            List<ICacheItem> item = _pool.Get<TestTable>().Get(model.ID.ToString());
            TestCacheModel queryRlt = item[0] as TestCacheModel;
            Assert.IsNotNull(queryRlt.Name.Equals(model.Name));
        }

        /// <summary>
        /// 缓存删除
        /// </summary>
        [TestMethod]
        public void RemoveCacheItemTest()
        {
            _pool.Register<TestTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetKey
            };
            _pool.Get<TestTable>().AddIndex(index);
            List<ICacheIndex> indexs = _pool.Get<TestTable>().GetAllIndexs();

            TestCacheModel model = new TestCacheModel() { ID = 1, Name = "测试" };
            _pool.Get<TestTable>().Add(model);
            _pool.Get<TestTable>().Remove(model);
            List<ICacheItem> list = _pool.Get<TestTable>().Get(model.ID.ToString());
            Assert.IsTrue(list.Count==0);
        }

        /// <summary>
        /// 改
        /// </summary>
        [TestMethod]
        public void UpdateCacheItemTest()
        {
            _pool.Register<TestTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetKey
            };
            _pool.Get<TestTable>().AddIndex(index);
            List<ICacheIndex> indexs = _pool.Get<TestTable>().GetAllIndexs();

            TestCacheModel model = new TestCacheModel() { ID = 1, Name = "测试" };
            _pool.Get<TestTable>().Add(model);

            TestCacheModel updateModel = new TestCacheModel()
            {
                ID = 1,
                Name = "测试1"
            };
            _pool.Get<TestTable>().Update(updateModel);
            List<ICacheItem> item = _pool.Get<TestTable>().Get(model.ID.ToString());
            TestCacheModel queryRlt = item[0] as TestCacheModel;
            Assert.AreEqual(updateModel.Name, queryRlt.Name);
        }

        private string GetKey(ICacheItem cache)
        {
            TestCacheModel model = cache as TestCacheModel;
            return model.ID.ToString();
        }

        [TestMethod]
        public void CacheLogTest()
        {
            //TestCacheModel model = new TestCacheModel();
            CacheLog.LogErrorEvent += LogInfo;
            _pool.Get<TestTable>()?.Add(null);
        }

        private void LogInfo(string log)
        {
            log1.LogInfo(log);
        }
    }
}
