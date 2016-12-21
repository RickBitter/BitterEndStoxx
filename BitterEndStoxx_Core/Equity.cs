using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitterEndStoxx.Core
{
    public enum ohlc:int { Open,High,Low,Close}
    /// <summary>
    /// Base Equity class, contains OHLC data, the date, volume
    /// </summary>
    public class Equity
    {
        private double[] _data;
        private long _volume;

        /// <summary>
        /// Represents the Opening price for the time span
        /// </summary>
        public double Open { get { return _data[(int)ohlc.Open]; }  set { _data[(int)ohlc.Open] = value; } }
        public double Close { get { return _data[(int)ohlc.Close]; } set { _data[(int)ohlc.Close] = value; } }
        public double High { get { return _data[(int)ohlc.High]; } set { _data[(int)ohlc.High] = value; } }
        public double Low { get { return _data[(int)ohlc.Low]; } set { _data[(int)ohlc.Low] = value; } }
        public DateTime Date { get; set; }
        public TimeSpan Span { get; set; }
        public long Volume { get
            { return _volume; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("volume cannot be negative");
            }
        }
        public double[] OHLC
        {
            get { return _data; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Length != 4)
                    throw new ArgumentOutOfRangeException("needs to be an array of 4 elements");
                if (value[(int)ohlc.Low] > value[(int)ohlc.High])
                    throw new ArgumentOutOfRangeException("low value cannot be higher than high value");
                if (value[(int)ohlc.Low] > value[(int)ohlc.Close])
                    throw new ArgumentOutOfRangeException("low value must be lower or equal to close value");
                if (value[(int)ohlc.Low] > value[(int)ohlc.Open])
                    throw new ArgumentOutOfRangeException("low value must be lower or equal to open value");
                _data = value;

            }
        }

        public Equity()
        {
            _data = new double[4];
        }
        
    }

    /// <summary>
    /// represents a single value set for an analysis operation, contins a value and a date
    /// </summary>
    public class Result
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}
