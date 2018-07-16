using System;
using System.Collections.Generic;
using Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class CacheUnitTest
    {
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
            _pool.Register<ITestTable, CacheTable>();
            Assert.IsNotNull(_pool.Get<ITestTable>());
        }

        [TestMethod]
        public void CacheTableAddIndexTest()
        {
            _pool.Register<ITestTable, CacheTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey= GetKey
            };
            _pool.Get<ITestTable>().AddIndex(index);
            List<ICacheIndex> indexs =  _pool.Get<ITestTable>().GetAllIndexs();
            Assert.IsTrue(indexs[0].IndexName.Equals("主键测试"));
        }

        /// <summary>
        /// 缓存增加及查询
        /// </summary>
        [TestMethod]
        public void AddCacheItemTest()
        {
            _pool.Register<ITestTable, CacheTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetKey
            };
            _pool.Get<ITestTable>().AddIndex(index);
            List<ICacheIndex> indexs = _pool.Get<ITestTable>().GetAllIndexs();

            TestCacheModel model = new TestCacheModel() { ID = 1, Name = "测试" };
            _pool.Get<ITestTable>().Add(model);
            List<ICacheItem> item = _pool.Get<ITestTable>().Get(model.ID.ToString());
            TestCacheModel queryRlt = item[0] as TestCacheModel;
            Assert.IsNotNull(queryRlt.Name.Equals(model.Name));
        }

        /// <summary>
        /// 缓存删除
        /// </summary>
        [TestMethod]
        public void RemoveCacheItemTest()
        {
            _pool.Register<ITestTable, CacheTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetKey
            };
            _pool.Get<ITestTable>().AddIndex(index);
            List<ICacheIndex> indexs = _pool.Get<ITestTable>().GetAllIndexs();

            TestCacheModel model = new TestCacheModel() { ID = 1, Name = "测试" };
            _pool.Get<ITestTable>().Add(model);
            _pool.Get<ITestTable>().Remove(model);
            List<ICacheItem> list = _pool.Get<ITestTable>().Get(model.ID.ToString());
            Assert.IsTrue(list.Count==0);
        }

        /// <summary>
        /// 改
        /// </summary>
        [TestMethod]
        public void UpdateCacheItemTest()
        {
            _pool.Register<ITestTable, CacheTable>();
            CacheIndex index = new CacheIndex()
            {
                IndexName = "主键测试",
                IndexType = IndexType.唯一索引,
                GetIndexKey = GetKey
            };
            _pool.Get<ITestTable>().AddIndex(index);
            List<ICacheIndex> indexs = _pool.Get<ITestTable>().GetAllIndexs();

            TestCacheModel model = new TestCacheModel() { ID = 1, Name = "测试" };
            _pool.Get<ITestTable>().Add(model);

            TestCacheModel updateModel = new TestCacheModel()
            {
                ID = 1,
                Name = "测试1"
            };
            _pool.Get<ITestTable>().Update(updateModel);
            List<ICacheItem> item = _pool.Get<ITestTable>().Get(model.ID.ToString());
            TestCacheModel queryRlt = item[0] as TestCacheModel;
            Assert.AreEqual(updateModel.Name, queryRlt.Name);
        }

        private string GetKey(ICacheItem cache)
        {
            TestCacheModel model = cache as TestCacheModel;
            return model.ID.ToString();
        }

        [TestMethod]
        public void CacheAdd()
        {
            //TestCacheModel model = new TestCacheModel();

        }
    }
}
