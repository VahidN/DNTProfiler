using System;
using System.Windows.Controls;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.UnparametrizedWhereClauses
{
    public class UnparametrizedWhereClausesPlugin : ProfilerPluginBase
    {
        public UnparametrizedWhereClausesPlugin()
        {
            PluginMetadata = new PluginMetadata
            {
                PluginName = "Unparameterized Where Clauses",
                PluginDescription =
                "Unparameterized queries(ad-hoc queries) will cause more resource consumption (higher CPU and Memory usage of the the database server)."+
                Environment.NewLine +
                " Also if you are calling these queries using the `Database.SqlQuery/ExecuteSqlCommand` method directly, be aware of possible SQL injection attacks." +
                Environment.NewLine +
                @"If you are using SQL Server, try enabling parameterization:
-- Enabling Parameterization
ALTER DATABASE dbName SET PARAMETERIZATION FORCED;
-- Optimizing for Ad hoc Workloads
sp_configure 'show advanced options',1; RECONFIGURE; sp_configure 'optimize for ad hoc workloads',1; RECONFIGURE;" + Environment.NewLine
            };

            PluginAuthor = new PluginAuthor
            {
                Name = "VahidN",
                Email = "",
                WebSiteUrl = "http://www.dotnettips.info"
            };

            Category = PluginCategory.Alerts;
        }

        public override UserControl GetPluginUI()
        {
            return new Main(this);
        }
    }
}