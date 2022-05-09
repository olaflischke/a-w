using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TradingDayDal;

namespace TradingDayDalTests
{
    [TestClass]
    public class ArchiveTests
    {
        string url;

        public ArchiveTests()
        {
            url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
        }

        [TestMethod]
        public void IsArchiveInitializing()
        {
            Archive archive = new Archive(url);

            Console.WriteLine($"USD vom 1. Tradingday: {archive.TradingDays.FirstOrDefault()?.ExchangeRates.LastOrDefault()?.EuroValue}");

            Assert.AreEqual(CountAttributes("time"), archive.TradingDays.Count());
        }

        private int CountAttributes(string attributeName)
        {
            return 63;
        }


    }
}
