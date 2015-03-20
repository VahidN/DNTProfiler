# DNTProfiler
DNTProfiler.NHibernate.Core is an NHibernate 4.0+ profiler.

## Usage
* To start using DNTProfiler.NHibernate.Core package, add the following lines to app.config/web.config file:
<appSettings>
    <add key="DNTProfilerServerUri" value="http://localhost:8080" />
    <add key="DNTProfilerLogFilePath" value="|DataDirectory|\ErrorsLog.Log" />
</appSettings>

And then to configure the NHibernate to use the provided driver:
- FluentNHibernate:
Fluently.Configure().Database(
  MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString).Driver<ProfiledSql2008ClientDriver>()
);

- Loquacious style configuration:
cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver,
                typeof(ProfiledSql2008ClientDriver).AssemblyQualifiedName);

- XML style configuration:
<property name="connection.driver_class">
   DNTProfiler.NHibernate.Core.Drivers.ProfiledSql2008ClientDriver,DNTProfiler.NHibernate.Core
</property>

* To disable the DNTProfiler.NHibernate.Core, just remove or comment out the above settings.
* To view its real-time collected information and reports, you need to download the DNTProfiler application too.
You can download it from here: https://github.com/VahidN/DNTProfiler/releases
