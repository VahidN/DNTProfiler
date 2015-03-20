using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using DNTProfiler.Common.Toolkit;

namespace DNTProfiler
{
    public partial class App
    {
        void checkSingleInst()
        {
            //WPF Single Instance Application
            var process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (process.Length <= 1) return;
            MessageBox.Show("DNTProfiler is already running ...", "DNTProfiler", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Shutdown();
        }

        public App()
        {
            AppDomain.MonitoringIsEnabled = true;
            this.Startup += appStartup;
            this.Deactivated += appDeactivated;
            checkSingleInst();
            forceWpfTooltipToStayOnTheScreen();
            EnableMultiCoreJit.Start();
        }

        private static void forceWpfTooltipToStayOnTheScreen()
        {
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof (DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
        }

        static void appDeactivated(object sender, EventArgs e)
        {
            Memory.ReEvaluateWorkingSet();
        }

        static void appStartup(object sender, StartupEventArgs e)
        {
            reducingCpuConsumptionForAnimations();
        }

        static void reducingCpuConsumptionForAnimations()
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                 typeof(Timeline),
                 new FrameworkPropertyMetadata { DefaultValue = 20 }
                 );
        }
    }
}