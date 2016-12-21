using System;
using System.Collections.Generic;
using System.Linq;
using BitterEndStoxx.Core;
namespace BitterEndStoxx.BasicAnalsis
{
    public enum maType { simple, exponential, weighted}

    public class MovingAverage:Analysis
    {
        public maType AveragingType { get; set; }
        public int Period { get; set; }
        public List<Result> Analyze(List<Equity> target, ohlc operateOn=ohlc.Close )
        {
            validateConfiguration(target.Count);
            List<Result> res = new List<Result>(target.Count - Period);
            int offset = 0; 
            while (offset<target.Count-Period)
            {
                List<Equity> t = target.GetRange(offset, Period);
                double avg = t.Average(item => item.OHLC[(int)operateOn]);
                Result r = new Result();
                r.Date = t[Period - 1].Date;
                r.Value = avg;
                res.Add(r);
                offset++;
            }
            return res;
        }

        /// <summary>
        /// abstraction wrapper for input validation, verify the average period and the series data make sense
        /// </summary>
        /// <param name="count"></param>
        private void validateConfiguration( int count)
        {
            if (Period < 2)
                throw new InvalidOperationException("Period is less than 2, nothing to average");
            if (Period > count)
                throw new InvalidOperationException("Period is longer than the series data");
        }
    }
}
