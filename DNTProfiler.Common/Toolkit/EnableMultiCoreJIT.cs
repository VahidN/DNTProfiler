using System;
using System.IO;
using System.Reflection;

namespace DNTProfiler.Common.Toolkit
{
    public static class EnableMultiCoreJit
    {
        public static void Start()
        {
            var systemRuntimeProfileOptimizationType = Type.GetType("System.Runtime.ProfileOptimization", false);
            if (systemRuntimeProfileOptimizationType == null)
                return;

            var setProfileRootMethod = systemRuntimeProfileOptimizationType.GetMethod("SetProfileRoot", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(string) }, null);
            var startProfileMethod = systemRuntimeProfileOptimizationType.GetMethod("StartProfile", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(string) }, null);

            if (setProfileRootMethod == null || startProfileMethod == null)
                return;

            try
            {
                var localSettingsDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var localAppSettingsDir = Path.Combine(localSettingsDir, "DNTProfiler");
                var profileDir = Path.Combine(localAppSettingsDir, "ProfileOptimization");
                Directory.CreateDirectory(profileDir);

                setProfileRootMethod.Invoke(null, new object[] { profileDir });
                startProfileMethod.Invoke(null, new object[] { "Startup.profile" });
            }
            catch
            {
                // discard errors. good faith effort only.
            }
        }
    }
}