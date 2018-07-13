using System;
using Config.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class ConfigUnitTest
    {
        [TestMethod]
        public void AppConfigTest()
        {
            string path = @"E:\logscrapyconfig.xml";
            string pattern = @"\[\d{4}-\d{2}-\d{2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9],[0-9]{3}:\](\[INFO \]|\[WARN \]|\[ERROR\])\[T\d+\] ";
            AppConfigManage config = new AppConfigManage();
            config.LoadConfigs(path);
            config.分行策略 = @"\[\d{4}-\d{2}-\d{2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9],[0-9]{3}:\](\[INFO \]|\[WARN \]|\[ERROR\])\[T\d+\] ";
            config.时间戳提取策略 = @"\[\d{4}-\d{2}-\d{2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9],[0-9]{3}:\]";
            config.缓存匹配策略列表.Add(new CachePattern() { CacheType = "FutureEntrust", Pattern= @"\[缓存\|FutureEntrust\]收到更新消息\(FutureEntrust,oplus.jy_tfutentrusts\)" });
            config.SaveConfigs(path);

            AppConfigManage configT = new AppConfigManage();
            configT.LoadConfigs(path);
            Assert.AreEqual(pattern, configT.分行策略);
        }
    }
}
