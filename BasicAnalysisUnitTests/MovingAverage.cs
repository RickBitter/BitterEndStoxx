using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BitterEndStoxx.BasicAnalsis;
using BitterEndStoxx.Core;
using System.Collections.Generic;

namespace BasicAnalysisUnitTests
{
    [TestClass]
    public class MovingAverageTests
    {
        [TestMethod]
        public void Success()
        {
            BitterEndStoxx.BasicAnalsis.MovingAverage ma = new MovingAverage();
            ma.Period = 3;
            List<Equity> vals = new List<Equity>(10);
            for (int i=0;i<10;i++)
            {
                Equity val = new Equity();
                val = new Equity();
                val.OHLC =  new double[]{ 10 + i, 11 + i, 9 + i, 10 + i};
                val.Date = new DateTime(2016, 1, i + 1);
                vals.Add(val);
            }
            
            List<Result> r= ma.Analyze(vals);
            Assert.AreEqual(r.Count, 7);
            for (int i=0;i<7;i++)
                Assert.AreEqual(r[i].Value, 11+i);
        }

        [TestMethod]
        public void InvalidPeriod()
        {
            MovingAverage ma = new MovingAverage();
            ma.Period = 20;
            List<Equity> vals = new List<Equity>(10);
            for (int i = 0; i < 10; i++)
            {
                Equity val = new Equity();
                val = new Equity();
                val.OHLC = new double[] { 10 + i, 11 + i, 9 + i, 10 + i };
                val.Date = new DateTime(2016, 1, i + 1);
                vals.Add(val);
            }
            List<Result> r;
            try { r = ma.Analyze(vals); }
            catch(InvalidOperationException aore)
            {
                return;   
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
            Assert.Fail();
        }
    }
}
