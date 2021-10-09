using System;
using System.Collections.Generic;
using System.Text;

namespace SMPLX.ForecastingDashboard.Helpers
{
    public static class MathExtensions
    {
        public static double RoundOff(this double a, int d)
        {
            return Math.Round(a, d);
        }

        public static double ToDouble(this int a)
        {
            return Convert.ToDouble(a);
        }
    }
}
