using System;
using Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class LogUnitTest
    {
        [TestMethod]
        public void LogTest()
        {
            LogJet log1 = new LogJet("CacheLog", @"D:/Log/ScrapyLog.config");
            log1.LogInfo("info级别日志信息1");
            log1.LogDebug("debug级别日志信息1");
            log1.LogError("error级别日志信息1");

            LogJet log2 = new LogJet("TaskLog", @"D:/Log/ScrapyLog.config");
            log2.LogInfo("info级别日志信息2");
            log2.LogDebug("debug级别日志信息2");
            log2.LogError("error级别日志信息2");

            LogJet log3 = new LogJet("CommonLog", @"D:/Log/ScrapyLog.config");
            log3.LogInfo("info级别日志信息3");
            log3.LogDebug("debug级别日志信息3");
            log3.LogError("error级别日志信息3");
        }
    }
}
