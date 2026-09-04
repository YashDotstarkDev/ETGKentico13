using ETG.Data.Currency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class DataFixerTest

    {
        private ICurrencyConversionRetriever _retriever = new CurrencyConversionRetriever();

        

        [TestMethod]
        public void TestMethod1()
        {
            var result = _retriever.GetLatestCurrencyConverion("AUD", "USD,EUR");

            Assert.IsNotNull(result);
        }
    }
}
