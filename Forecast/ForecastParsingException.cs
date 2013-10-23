using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forecast
{
    public class ForecastParsingException : Exception
    {
        public ForecastParsingException(string message) : base(message) { } 
    }
}
