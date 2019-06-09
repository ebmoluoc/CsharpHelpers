using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace CsharpHelpers.Helpers
{

    public static class ImageHelper
    {

        public static BitmapSource BitmapSourceFromIcon(Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

    }

}
