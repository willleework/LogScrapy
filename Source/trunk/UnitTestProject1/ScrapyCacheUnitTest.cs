using System;
using System.Collections.Generic;
using System.Data;
using Cache;
using Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScrapyCache;

namespace UnitTestProject1
{
    [TestClass]
    public class ScrapyCacheUnitTest
    {
        [TestMethod]
        public void CacheLoadTest()
        {
            string filePath = @"E:\前台缓存表配置_OClient_最新版_基础.xlsm";
            string sheetName = "缓存表";

            DataTable sheetData = ExcelUtility.ExcelToDataTable(filePath, sheetName);

            ScrapyCachePool pool = new ScrapyCachePool();
            pool.Init();

            pool.Get<ClientCacheConfigTable>().LoadDatas(sheetData);
            List<ICacheItem>items =  pool.Get<ClientCacheConfigTable>().Get("单元信息表");
            Assert.IsTrue(items.Count > 0);
        }
    }
}
