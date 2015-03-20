using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using NHibernate.Cfg;

namespace NHibernateConsoleTests.Core
{
    public class ConfigurationFileCache
    {
        private readonly string _cacheFile;
        private readonly Assembly _definitionsAssembly;

        public ConfigurationFileCache(Assembly definitionsAssembly)
        {
            _definitionsAssembly = definitionsAssembly;
            _cacheFile = "nh.cfg";
            if (HttpContext.Current != null) //if is in web
                _cacheFile = HttpContext.Current.Server.MapPath(
                                string.Format("~/App_Data/{0}", _cacheFile)
                                );
        }

        public bool IsConfigurationFileValid
        {
            get
            {
                if (!File.Exists(_cacheFile))
                    return false;
                var configInfo = new FileInfo(_cacheFile);
                var asmInfo = new FileInfo(_definitionsAssembly.Location);

                if (configInfo.Length < 5 * 1024)
                    return false;

                return configInfo.LastWriteTime >= asmInfo.LastWriteTime;
            }
        }

        public void DeleteCacheFile()
        {
            if (File.Exists(_cacheFile))
                File.Delete(_cacheFile);
        }

        public Configuration LoadConfigurationFromFile()
        {
            if (!IsConfigurationFileValid)
                return null;

            using (var file = File.Open(_cacheFile, FileMode.Open, FileAccess.Read))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(file) as Configuration;
            }
        }

        public void SaveConfigurationToFile(Configuration configuration)
        {
            using (var file = File.Open(_cacheFile, FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, configuration);
            }
        }
    }
}
