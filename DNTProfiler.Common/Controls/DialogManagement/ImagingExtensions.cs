using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DNTProfiler.Common.Controls.DialogManagement
{
    public static class ImagingExtensions
    {
        public static Image CaptureImage(this FrameworkElement me, bool ensureSize = false)
        {
            var width = Convert.ToInt32(me.ActualWidth);
            width = width == 0 ? 1 : width;
            var height = Convert.ToInt32(me.ActualHeight);
            height = height == 0 ? 1 : height;

            var bmp = new RenderTargetBitmap(
                width, height,
                96, 96,
                PixelFormats.Default);
            bmp.Render(me);

            var img = new Image
            {
                Source = bmp,
                Stretch = Stretch.None,
                Width = width - (ensureSize ? 1 : 0),
                Height = height - (ensureSize ? 1 : 0)
            };
            return img;
        }
    }
}