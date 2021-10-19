using System.Collections.Generic;

namespace SMPLX.ForecastingDashboard.Settings
{
    public class GeologicalOptions
    {
        public List<Location> Barangays { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}