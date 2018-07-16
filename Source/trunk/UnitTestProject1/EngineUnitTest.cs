using System;
using Cache;
using Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class EngineUnitTest
    {
        [TestMethod]
        public void AutofacTest()
        {
            ScrapyEngine engine = new ScrapyEngine();
            ICachePool cache = engine.Get<ICachePool>();
            Assert.IsTrue(cache != null);
        }
    }
}
