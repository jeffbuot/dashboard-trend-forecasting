namespace SMPLX.ForecastingDashboard.Permissions
{
    public static class ForecastingDashboardPermissions
    {
        public const string GroupName = "ForecastingDashboard";

        public static class Case
        {
            public const string Default = GroupName + ".Case";
            public static readonly string Create = Default + ".Create";
            public static readonly string Edit = Default + ".Edit";
            public static readonly string Delete = Default + ".Delete";
        }
    }
}