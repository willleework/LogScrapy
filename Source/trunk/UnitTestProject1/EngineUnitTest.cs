using System;
using Cache.Interface;
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
            ICache cache = engine.Get<ICache>();
            Assert.IsTrue(cache != null);
        }
    }
}
