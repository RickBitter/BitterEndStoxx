using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitterEndStoxx.Core
{
    public interface Analysis
    {
        List<Result> Analyze(List<Equity> target, ohlc operateOn = ohlc.Close);
        string getName();
    }
}
