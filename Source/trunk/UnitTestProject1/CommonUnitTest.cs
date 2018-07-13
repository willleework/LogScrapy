using System;
using System.Data;
using Common;
using Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class CommonUnitTest
    {
        [TestMethod]
        public void ExcelReadTest()
        {
            string filePath = @"E:\前台缓存表配置_OClient_最新版_基础.xlsm";
            string sheetName = "缓存表";

            DataTable sheetData =  ExcelUtility.ExcelToDataTable(filePath, sheetName);

            Assert.IsTrue(sheetData.Rows.Count > 0);
        }
    }
}
