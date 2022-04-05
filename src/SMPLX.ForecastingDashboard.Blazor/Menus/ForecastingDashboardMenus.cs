namespace SMPLX.ForecastingDashboard.Blazor.Menus
{
    public static class ForecastingDashboardMenus
    {
        private const string Prefix = "ForecastingDashboard";
        public const string Home = Prefix + ".Home";
        public const string Dashboard = Prefix + ".Dashboard";
        public const string Heatmap = Prefix + ".Heatmap";
        public const string Analytics = Prefix + ".Dashboard";

        public static class Case
        {
            public static string Default = Prefix + ".Cases";
            public static string Query = Default + ".Query";
            public static string Import = Default + ".Import";
        }

        //Add your menu items here...
    }
}