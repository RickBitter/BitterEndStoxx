using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitterEndStoxx.DataSource;
using BitterEndStoxx.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatasourceUnitTests
{
    [TestClass]
    public class YahooDownloaderTest
    {
        [TestMethod]
        public async Task Download()
        {
            YahooFinanceReader yfr = new YahooFinanceReader();
            List<Equity> result;
            result = await yfr.DownloadAsync("LULU", new DateTime(2016, 1, 1), new DateTime(2017, 1, 1), BitterEndStoxx.EquityInterval.daily);
        }

        [TestMethod]
        public async Task InvalidTicker()
        {
            YahooFinanceReader yfr = new YahooFinanceReader();
            List<Equity> result;
            result = await yfr.DownloadAsync("FUK", new DateTime(2016, 1, 1), new DateTime(2017, 1, 1), BitterEndStoxx.EquityInterval.daily);
        }
    }
}
