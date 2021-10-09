using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMPLX.ForecastingDashboard.Helpers
{
    public class MathUtil
    {
        public static double Slope(double[] y, double[] x)
        {
            var xys = Enumerable.Zip(x, y, (x, y) => new { x = x, y = y });
            var ybar = y.Average();
            var xbar = x.Average();
            return xys.Sum(_ => (_.x - xbar) * (_.y - ybar)) / xys.Sum(_ => (_.x - xbar) * (_.x - xbar));
        }

        public static double Intercept(double[] y, double[] x)
        {
            var ybar = y.Average();
            var xbar = x.Average();
            return ybar - (Slope(y, x) * xbar);
        }
    }
}
