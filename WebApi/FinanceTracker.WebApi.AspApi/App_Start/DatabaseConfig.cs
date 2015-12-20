namespace FinanceTracker.WebApi.AspApi
{
    using System.Data.Entity;
    using Data.DatabaseLink;
    using Data.DatabaseLink.Migrations;

    public static class DatabaseConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FinanceTrackerDbContext, Configuration>());
        }
    }
}