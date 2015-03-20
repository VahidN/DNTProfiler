using System;

namespace DNTProfiler.Common.Logger
{
    public static class LoggerPath
    {
        public  const string DataDirectory = "|datadirectory|";

        public static string GetLogFileFullPath(string path)
        {
            if (AppDomain.CurrentDomain.GetData("DataDirectory") == null)
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            }

            var fullPath = ExpandDataDirectory(path);
            return !path.Equals(fullPath, StringComparison.Ordinal) ? fullPath : path;
        }

        public static string ExpandDataDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)
                || !path.StartsWith(DataDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            // find the replacement path
            var rootFolderObject = AppDomain.CurrentDomain.GetData("DataDirectory");
            var rootFolderPath = rootFolderObject as string;
            if ((null != rootFolderObject)
                && (null == rootFolderPath))
            {
                throw new InvalidOperationException("Invalid Data Directory");
            }

            if (rootFolderPath == String.Empty)
            {
                rootFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (null == rootFolderPath)
            {
                rootFolderPath = String.Empty;
            }

            // Make sure that the paths have exactly one "\" between them
            path = path.Substring(DataDirectory.Length);
            if (path.StartsWith(@"\", StringComparison.Ordinal))
            {
                path = path.Substring(1);
            }

            var fixedRoot = rootFolderPath.EndsWith(@"\", StringComparison.Ordinal)
                                ? rootFolderPath
                                : rootFolderPath + @"\";

            path = fixedRoot + path;

            // Verify root folder path is a real path without unexpected "..\"
            if (rootFolderPath.Contains(".."))
            {
                throw new ArgumentException("Expanding Data Directory Failed");
            }

            return path;
        }
    }
}
