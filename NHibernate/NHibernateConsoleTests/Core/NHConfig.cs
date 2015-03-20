using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using DNTProfiler.NHibernate.Core.Drivers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;

namespace NHibernateConsoleTests.Core
{
    public class NHConfig
    {
        public Action<ModelMapper> AutoMappingOverride { set; get; }

        public string ConnectionString { set; get; }
        public string DbSchemaOutputFile { set; get; }

        public bool DropTablesCreateDbSchema { set; get; }

        public Assembly MappingsAssembly { set; get; }

        public string MappingsNamespace { set; get; }

        public ValidatorEngine MappingsValidatorEngine { get; private set; }

        public string OutputXmlMappingsFile { set; get; }

        public bool ShowLogs { set; get; }

        public string ValidationDefinitionsNamespace { set; get; }

        public ISessionFactory SetUpSessionFactory()
        {
            var config = readConfigFromCacheFileOrBuildIt();
            var sessionFactory = config.BuildSessionFactory();
            createDbSchema(config);
            return sessionFactory;
        }

        private Configuration buildConfiguration()
        {
            var config = initConfiguration();
            var mapping = getMappings();
            config.AddDeserializedMapping(mapping, "NHSchemaTest");
            injectValidationAndFieldLengths(config);
            return config;
        }

        private void createDbSchema(Configuration cfg)
        {
            if (!DropTablesCreateDbSchema) return;
            new SchemaExport(cfg).SetOutputFile(DbSchemaOutputFile).Create(true, true);
        }

        private HbmMapping getMappings()
        {
            //Using the built-in auto-mapper
            var mapper = new ConventionModelMapper();
            var allEntities = MappingsAssembly.GetTypes().Where(t => t.Namespace == MappingsNamespace).ToList();
            mapper.AddAllManyToManyRelations(allEntities);
            mapper.ApplyNamingConventions();
            if (AutoMappingOverride != null) AutoMappingOverride(mapper);
            var mapping = mapper.CompileMappingFor(allEntities);
            showOutputXmlMappings(mapping);
            return mapping;
        }

        private Configuration initConfiguration()
        {
            var configure = new Configuration();
            configure.SessionFactoryName("BuildIt");

            configure.DataBaseIntegration(db =>
            {
                db.ConnectionProvider<DriverConnectionProvider>();
                db.Dialect<MsSql2008Dialect>();
                db.Driver<Sql2008ClientDriver>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.IsolationLevel = IsolationLevel.ReadCommitted;
                db.ConnectionString = ConnectionString;
                db.Timeout = 10;
                db.BatchSize = 20;

                if (ShowLogs)
                {
                    db.LogFormattedSql = true;
                    db.LogSqlInConsole = true;
                    db.AutoCommentSql = false;
                }
            });

            configure.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver,
                typeof(ProfiledSql2008ClientDriver).AssemblyQualifiedName);


            return configure;
        }

        private void injectValidationAndFieldLengths(Configuration nhConfig)
        {
            if (string.IsNullOrWhiteSpace(ValidationDefinitionsNamespace))
                return;

            MappingsValidatorEngine = new ValidatorEngine();
            var configuration = new FluentConfiguration();
            var validationDefinitions = MappingsAssembly.GetTypes()
                                                        .Where(t => t.Namespace == ValidationDefinitionsNamespace)
                                                        .ValidationDefinitions();
            configuration
                    .Register(validationDefinitions)
                    .SetDefaultValidatorMode(ValidatorMode.OverrideExternalWithAttribute)
                    .IntegrateWithNHibernate
                    .ApplyingDDLConstraints()
                    .And
                    .RegisteringListeners();
            MappingsValidatorEngine.Configure(configuration);

            //Registering of Listeners and DDL-applying here
            nhConfig.Initialize(MappingsValidatorEngine);
        }

        private Configuration readConfigFromCacheFileOrBuildIt()
        {
            Configuration nhConfigurationCache;
            var nhCfgCache = new ConfigurationFileCache(MappingsAssembly);
            var cachedCfg = nhCfgCache.LoadConfigurationFromFile();
            if (cachedCfg == null)
            {
                nhConfigurationCache = buildConfiguration();
                nhCfgCache.SaveConfigurationToFile(nhConfigurationCache);
            }
            else
            {
                nhConfigurationCache = cachedCfg;
            }
            return nhConfigurationCache;
        }

        private void showOutputXmlMappings(HbmMapping mapping)
        {
            if (!ShowLogs) return;
            var outputXmlMappings = mapping.AsString();
            Console.WriteLine(outputXmlMappings);
            File.WriteAllText(OutputXmlMappingsFile, outputXmlMappings);
        }
    }
}