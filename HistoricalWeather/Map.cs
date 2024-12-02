using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoricalWeather
{
    internal static class Map
    {
        internal static bool ConvertstrtoB(this string cv) => cv.ToLower() == "true" ? true : false;    

    }
}
