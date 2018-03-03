using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaCreator.Designer.Common.Extensions
{
    public static class Extensions
    {
        public static double NearestFactor(this double Value, double Factor)
        {
            return Math.Round(Value / Factor, MidpointRounding.AwayFromZero) * Factor;
        }
    }
}
