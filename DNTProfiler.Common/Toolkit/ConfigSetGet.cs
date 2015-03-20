using System;
using System.Configuration;
using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.Common.Toolkit
{
    public static class ConfigSetGet
    {
        /// <summary>
        /// read settings from app.config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigData(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            //don't load on design time
            if (Designer.IsInDesignModeStatic)
                return "0";

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = configuration.AppSettings;
            string res = appSettings.Settings[key].Value;
            if (res == null) throw new Exception("Undefined: " + key);
            return res;
        }

        public static void SetConfigData(string key, string data)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = data;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}