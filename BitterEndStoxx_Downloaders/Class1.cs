using System;
using System.Collections.Generic;
using BitterEndStoxx.Core;
using System.Net.Http;
namespace BitterEndStoxx.DownLoaders
{

    public interface Download
    {

        List<Equity> Download(string Ticker, DateTime Start, DateTime End, EquityInterval Interval);
    }
    public class YahooFinanceReader : Download
    {
        //sample URI: http://chart.finance.yahoo.com/table.csv?s=MSI&a=10&b=21&c=2016&d=11&e=21&f=2016&g=d&ignore=.csv

        public List<Equity> Download(string Ticker, DateTime Start, DateTime End, EquityInterval Interval)
        {
            ValidateInterval(Interval);
            ValidateTicker(Ticker);
            string Url = "http://chart.finance.yahoo.com/table.csv?";
            Url += AddTicker(Ticker);
            Url += AddStartDate(Start);
            Url += AddEndDate(End);
            Url += AddInterval(Interval);
            HttpClient getter = new HttpClient();
            
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private string AddStartDate(DateTime start)
        {
            throw new NotImplementedException();
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
    }
}
