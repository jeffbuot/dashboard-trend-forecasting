using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using GemBox.Spreadsheet;

namespace ConsoleApp1
{
    public class Case
    {
        public string Month { get; set; }
        public double TotalCases { get; set; }
        public double Period { get; set; }

        public Case(double period, string month, double totalCases)
        {
            Month = month;
            TotalCases = totalCases;
            Period = period;
        }
    }

    public class Test
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Val { get; set; }

        public override string ToString()
        {
            return String.Format("Name: {0}, Val: {1}", Name, Val);
        }

        public class GroupedTest
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new List<Test>()
            {
                new Test { Name = "A1", Val = 1 ,Date = DateTime.Now.AddDays(-1)},
                new Test { Name = "B1", Val = 3 ,Date = DateTime.Now.AddDays(-4)},
                new Test { Name = "A1", Val = 4 ,Date = DateTime.Now.AddDays(-2)},
                new Test { Name = "A1", Val = 1 ,Date = DateTime.Now.AddYears(-10)},
                new Test { Name = "B1", Val = 2 ,Date = DateTime.Now.AddDays(-3)}
            };
            var sortedDate = list1.OrderBy(_ => _.Date);
            var xx = Enumerable.Range(1,12);
            Console.WriteLine(xx);
            Console.WriteLine(sortedDate);
            var x = list1.GroupBy(_ => _.Name)
                .Select(_ => new Test.GroupedTest() { Name = _.First().Name, Count = _.Count() }).ToList();
            Console.WriteLine(x);

            var list = new List<Case> { new Case(1, "", 1), new Case(1, "", 1), new Case(1, "", 1) };
            foreach (var l in list)
            {
                l.Period = 2;
            }

            Console.WriteLine(list);
            var months = Enumerable.Range(1, 12).Select(i => DateTimeFormatInfo.CurrentInfo.GetMonthName(i)).ToArray();
            // var months = new string[]
            //     { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            // var datasets = new List<Case>
            // {
            //     new(1, "Jan", 10),
            //     new(2, "Feb", 12),
            //     new(3, "Mar", 7),
            //     new(4, "Apr", 5),
            //     new(5, "May", 9),
            //     new(6, "Jun", 10),
            //     new(7, "Jul", 19),
            //     new(8, "Aug", 6),
            //     new(9, "Sep", 4),
            //     new(10, "Oct", 5),
            //     new(11, "Nov", 15),
            //     new(12, "Dec", 9),
            //     new(13, "Jan", 11),
            //     new(14, "Feb", 8),
            //     new(15, "Mar", 8),
            //     new(16, "Apr", 9),
            //     new(17, "May", 7),
            //     new(18, "Jun", 9),
            //     new(19, "Jul", 9),
            //     new(20, "Aug", 10),
            //     new(21, "Sep", 9),
            //     new(22, "Oct", 7),
            //     new(23, "Nov", 7),
            //     new(24, "Dec", 8),
            // };
            var datasets = DataHelper.ReadFromExcelFile("datasets_cases.xlsx");
            var cases = datasets.Select(_ => _.TotalCases).ToArray();
            var periods = datasets.Select(_ => _.Period).ToArray();
            var intercept = MathUtil.Intercept(cases, periods);
            var slope = MathUtil.Slope(cases, periods);
            Console.WriteLine($"Intercept: {intercept}, Slope: {slope}");
            //Get the average cases per month from history
            var sindex = months.ToDictionary(x => x,
                x => datasets.Where(m => m.Month == x).Select(_ => _.TotalCases).Average() / cases.Average());
            Console.WriteLine($"Cases average: {cases.Average()}");
            //Get the seasonality index per month
            Console.WriteLine("======Seasonality Index======");
            foreach (var kvp in sindex)
            {
                Console.WriteLine($"{kvp.Key} : {kvp.Value.RoundOff(2)}");
            }

            //Get the linear forecast and seasonal forecast
            Console.WriteLine("Linear Trend Forecast | Seasonal Forecast w/ Trend");
            foreach (var dataset in datasets)
            {
                var ltf = (intercept + slope * dataset.Period).RoundOff(2);
                var sft = sindex[dataset.Month] * ltf;
                Console.WriteLine($"{dataset.Month} | {ltf} | {sft.RoundOff(2)}");
            }

            Console.WriteLine("Next year forecast:");
            var latestPeriod = datasets.Count;
            foreach (var month in months)
            {
                latestPeriod++;

                var ltf = (intercept + slope * latestPeriod).RoundOff(2);
                var sft = sindex[month] * ltf;
                Console.WriteLine($"{month} | {ltf} | {sft.RoundOff(2)}");
            }
        }

        #region Old

//         static int TrendProjectionForecast(double[] casesPerPeriod, int forecastPeriod)
//         {
//             // variables are named from our formula
//             //x = periods
//             // casesPerPeriod = y
//
//             double n = Convert.ToDouble(casesPerPeriod.Length);
//             double[] xlist = Enumerable.Range(1, casesPerPeriod.Length).Select(x => Convert.ToDouble(x)).ToArray();
//             double xaverage = xlist.Sum() / n;
//             double yaverage = casesPerPeriod.Sum() / n;
//             double xsquaredsum = XSquaredSum(Convert.ToInt32(n));
//             double xysum = GetXYSum(xlist, casesPerPeriod);
//
//             double b = (xysum - (n * xaverage * yaverage)) / (xsquaredsum - (n * (xaverage * xaverage)));
//             // b = b * (new Random().Next(1, 10) % 2 == 0 ? -1 : 1);
//             double a = yaverage - (b * xaverage);
//
//             var forecastResult = a + (b * Convert.ToDouble(forecastPeriod));
//             var rounded = Math.Round(Convert.ToDecimal(forecastResult));
//             Console.WriteLine(
//                 $"xaverage: {xaverage}, yaverage: {yaverage}, xsquaredsum: {xsquaredsum}, xysum: {xysum}, b: {b}, a: {a}, Y: {forecastResult}");
//             return Convert.ToInt32(rounded);
//         }
//
//         // static int LinearTrendProjection(double[] casesPerPeriod, int forecastPeriod)
//         // {
//         //     
//         // }
//
// // XY Summation
//         static double GetXYSum(double[] x, double[] y)
//         {
//             double result = 0;
//             for (int i = 0; i < x.Length; i++)
//             {
//                 result += x[i] * y[i];
//             }
//
//             return result;
//         }
//
//         static double XSquaredSum(int n)
//         {
//             double sum = 0;
//             for (; n > 0; n--) sum += n * n;
//             return sum;
//         }

        #endregion
    }

    public class DataHelper
    {
        public static List<Case> ReadFromExcelFile(string filePath)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = ExcelFile.Load(filePath);

            // Select the first worksheet from the file.
            var worksheet = workbook.Worksheets[0];

            // Create DataTable from an Excel worksheet.
            var dataTable = worksheet.CreateDataTable(new CreateDataTableOptions()
            {
                ColumnHeaders = true,
                StartRow = 0,
                NumberOfColumns = 4,
                NumberOfRows = worksheet.Rows.Count,
                Resolution = ColumnTypeResolution.AutoPreferStringCurrentCulture
            });
            var sb = new StringBuilder();
            var datasets = new List<Case>();
            foreach (DataRow row in dataTable.Rows)
            {
                datasets.Add(new Case(Convert.ToDouble(row[0]), row[2].ToString(), Convert.ToDouble(row[3])));
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}", row[0], row[1], row[2], row[3]);
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
            return datasets;
        }
    }

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