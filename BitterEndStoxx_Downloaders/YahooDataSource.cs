using System;
using System.Collections.Generic;
using BitterEndStoxx.Core;
using System.Net.Http;
using System.Threading.Tasks;

namespace BitterEndStoxx.DataSource
{

    public interface Download
    {

        Task< List<Equity>>  DownloadAsync(string Ticker, DateTime Start, DateTime End, EquityInterval Interval);
        string getName();
    }
    public delegate void DataSourceLogger(string msg);
    public class YahooFinanceReader : Download
    {
        public event DataSourceLogger YahooFinanceLogger;
        private enum yahooFormat:int
        {
            date = 0,
            open = 1,
            high = 2,
            low = 3,
            close = 4,
            volume = 5,
            adjVolume = 6,
            adjClose = 7

        }
        private int _LoggingLevel=0; 
        public int LoggingLevel
        {
            get
            { return _LoggingLevel; }
            set
            {
                if (value > 10) throw new ArgumentOutOfRangeException();
                if (value < 0) throw new ArgumentOutOfRangeException();
                _LoggingLevel = value;
            }
        }
        //sample URI: http://chart.finance.yahoo.com/table.csv?s=MSI&a=10&b=21&c=2016&d=11&e=21&f=2016&g=d&ignore=.csv

        public async Task<List<Equity>> DownloadAsync(string Ticker, DateTime Start, DateTime End, EquityInterval Interval)
        {
            ValidateInterval(Interval);
            ValidateTicker(Ticker);
            string Url = buildUrl(Ticker, Start, End, Interval);
            HttpClient getter = new HttpClient();


            string result = "";
            try
            {
                result = await getter.GetStringAsync(Url);
            }
            catch (HttpRequestException hre)
            {
                if (YahooFinanceLogger != null & LoggingLevel > 0)
                    YahooFinanceLogger(String.Format("ERROR: download failed due to {0}", hre.Message));
                
                return null; 
            }
            return BuildDataPoints(result, Interval);
        }

        private List<Equity> BuildDataPoints(string result, EquityInterval interval)
        {
            TimeSpan span = ConvertInterval(interval);
            string[] dataPoints = result.Split('\n');
            if (dataPoints.Length == 0)
                throw new InvalidOperationException("no data points seen in download.");
            //yahoo! finance returns a CSV file 
            //"Date,Open,High,Low,Close,Volume,Adj Close\n2016-12-30,64.669998,65.720001,64.43,64.989998,1611300,64.989998\n
            List<Equity> res = new List<Equity>(dataPoints.Length);
            foreach (string s in dataPoints)
            {
                string[] subs = s.Split(',');
                if (subs.Length ==7)
                {
                    Equity localVal = new Equity();
                    DateTime t;
                    bool convert = DateTime.TryParse(subs[(int)yahooFormat.date],out t);
                    if (convert == true)
                        localVal.Date = t;
                    else
                        continue;
                    convert = tryOhlc(subs, ref localVal);
                    if (convert == false)
                        continue;
                    long Vol;
                    convert = long.TryParse(subs[(int)yahooFormat.volume], out Vol);
                    if (convert)
                        localVal.Volume = Vol;
                    else
                        continue;
                    localVal.Span = span;
                    res.Add(localVal);
                }
                else
                {
                    //todo-log i threw
                }
            }
            return res;
        }

        private TimeSpan ConvertInterval(EquityInterval interval)
        {
            switch (interval)
            {
                case EquityInterval.daily:
                    {
                        return new TimeSpan(1, 0, 0);
                    }
                case EquityInterval.weekly:
                    {
                        return new TimeSpan(7, 0, 0);
                    }
                case EquityInterval.monthly:
                    {
                        return new TimeSpan(30, 0, 0);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("shouldn't be here- yahoo! only does daily, weekly, monthly");
                    }
            }
        }

        /// <summary>
        /// Convert OHLC values in one loop.
        /// </summary>
        /// <param name="subs">line of data from yahoo! finance</param>
        /// <param name="localVal">the equity data point we're operating on</param>
        /// <returns>Bool indicating all 4 conversions were successful.</returns>
        private bool tryOhlc(string[] subs, ref Equity localVal)
        {
            bool conv = false;
            double[] ohlc = new double[4];
            for (int i=0; i<4;i++)
            {
                conv = double.TryParse(subs[i + 1], out ohlc[i]);
                if (conv == false)
                    return false; //failed to convert, just quit
            }
            localVal.OHLC = ohlc;
            return true;
        }

        private string buildUrl(string Ticker, DateTime Start, DateTime End, EquityInterval Interval)
        {
            string Url = "http://chart.finance.yahoo.com/table.csv?";
            Url += AddTicker(Ticker);
            Url += AddStartDate(Start);
            Url += AddEndDate(End);
            Url += AddInterval(Interval);
            Url += "&ignore=.csv";
            return Url;
        }

        /// <summary>
        /// Validates the ticker symbol makes sense; check items like string length
        /// </summary>
        /// <param name="ticker">Ticker symbol that was passed to the downloader</param>
        private void ValidateTicker(string ticker)
        {
            if (ticker.Length > 5)
                throw new ArgumentOutOfRangeException("ticker can't be more than 5 symbols");
        }

        private string AddInterval(EquityInterval interval)
        {
            switch(interval)
            {
                case EquityInterval.daily:
                    {
                        return "d";
                    }
                case EquityInterval.weekly:
                    {
                        return "w";
                    }
                case EquityInterval.monthly:
                    {
                        return "m";
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException("shouldn't be here, interval nto supported by yahoo.");
                    }
            }
        }

        private string AddEndDate(DateTime end)
        {

            string e = "&d=" + (end.Month-1);
            e += "&e=" + (end.Day);
            e += "&f=" + end.Year;
            return e;
        }

        private string AddStartDate(DateTime start)
        {
            string s = "&a=" + (start.Month - 1);
            s += "&b=" + (start.Day);
            s += "&c=" + start.Year;
            return s;
        }

        private string AddTicker(string ticker)
        {
            return "s=" + ticker;
        }

        private void ValidateInterval(EquityInterval interval)
        {
            if (interval == EquityInterval.daily |
                interval == EquityInterval.monthly |
                interval == EquityInterval.weekly)
                return;//note we're OK, Yahoo supports these
            else
                throw new ArgumentOutOfRangeException("Interval " + interval.ToString() + " not supported by Yahoo! finance");
        }

        
        public string getName()
        {
            return "Yahoo! Finance Downloader";
        }
    }
}
