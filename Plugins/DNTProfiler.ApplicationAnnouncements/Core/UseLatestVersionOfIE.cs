using System;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;
using DNTProfiler.Common.Logger;
using DNTProfiler.Common.Mvvm;

namespace DNTProfiler.ApplicationAnnouncements.Core
{
    public static class UseLatestVersionOfIE
    {
        /// <summary>
        /// Use the latest version of IE in WebBrowser control
        /// </summary>
        public static void SetWebBrowserVersion()
        {
            RegistryKey regkey = null;
            try
            {
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", writable: true);
                if (regkey == null)
                {
                    return;
                }

                var regVal = getInstalledIEVersion();
                var appName = string.Format("{0}.exe", Process.GetCurrentProcess().ProcessName);
                regkey.SetValue(appName, regVal, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                new ExceptionLogger().LogExceptionToFile(ex, AppMessenger.LogFile);
                AppMessenger.Messenger.NotifyColleagues("ShowException", ex);
            }
            finally
            {
                if (regkey != null)
                {
                    regkey.Close();
                }
            }
        }

        private static int getInstalledIEVersion()
        {
            int browserVer;
            using (var wb = new WebBrowser())
            {
                browserVer = wb.Version.Major;
            }

            int regVal;
            if (browserVer >= 11)
                regVal = 11001;
            else
                switch (browserVer)
                {
                    case 10:
                        regVal = 10001;
                        break;
                    case 9:
                        regVal = 9999;
                        break;
                    case 8:
                        regVal = 8888;
                        break;
                    default:
                        regVal = 7000;
                        break;
                }
            return regVal;
        }
    }
}