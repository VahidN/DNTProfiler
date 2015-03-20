using System.Data.Entity.Migrations;

namespace DNTProfiler.TestEFContext.DataLayer
{
    public class Configuration : DbMigrationsConfiguration<SampleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}