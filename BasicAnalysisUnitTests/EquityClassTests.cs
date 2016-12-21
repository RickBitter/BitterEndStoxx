using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitterEndStoxx.Core;
namespace BasicAnalysisUnitTests
{
    [TestClass]
    public class EquityClassTests
    {
        [TestMethod]
        public void PropertyValidation()
        {
            Equity e = new Equity();
            e.Close = 1.0;
            e.Open = 2.0;
            e.High = 3.0;
            e.Low = 0.5;

            Assert.AreEqual(e.Close, 1.0);
            Assert.AreEqual(e.Open, 2.0);
            Assert.AreEqual(e.High, 3.0);
            Assert.AreEqual(e.Low, 0.5);
            double[] ohlc = new double[] { 2.0, 3.0, 0.5, 1.0 };
            CollectionAssert.AreEqual(e.OHLC, ohlc);
        }
        [TestMethod]
        public void ohlcExceptionHiLo()
        {
            Equity e = new Equity();
            double[] highLoInversion = new double[] { 2.0, 0.5, 3.0, 1.0 };
            try
            {
                e.OHLC = highLoInversion;
            }
            catch(ArgumentOutOfRangeException aoer)
            {
                return;
            }
            catch(Exception ex)
            {
                Assert.Fail();
            }
            Assert.Fail();
        }

        [TestMethod]
        public void ohlcExceptionnull()
        {
            Equity e = new Equity();
            double[] highLoInversion = null;
            try
            {
                e.OHLC = highLoInversion;
            }
            catch (ArgumentNullException aoer)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ohlcExceptionloClose()
        {
            Equity e = new Equity();
            double[] highLoInversion = new double[] { 2.0, 3.0, 1.1, 1.0 };
            try
            {
                e.OHLC = highLoInversion;
            }
            catch (ArgumentOutOfRangeException aoer)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ohlcExceptionloOpen()
        {
            Equity e = new Equity();
            double[] highLoInversion = new double[] { 2.0, 3.0, 2.1, 1.0 };
            try
            {
                e.OHLC = highLoInversion;
            }
            catch (ArgumentOutOfRangeException aoer)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void NegativeVolume()
        {
            Equity e = new Equity();
           
            try
            {
                e.Volume = -1234;
            }
            catch (ArgumentOutOfRangeException aoer)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }
    }
}
