using CsharpHelpers.Helpers;
using CsharpHelpers.WindowServices;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace CsharpHelpers.DialogServices
{

    public partial class AboutDialog1 : Window
    {

        public AboutDialog1()
        {
            InitializeComponent();

            Title = $"About {AppHelper.AssemblyInfo.Product}";
            ProductImage.Source = ImageHelper.BitmapSourceFromIcon(AppHelper.AssemblyInfo.Icon);
            ProductTitle.Text = AppHelper.AssemblyInfo.Product;
            ProductVersion.Text = $"Version {AppHelper.AssemblyInfo.FileVersion}";
            ProductSupportUrl.NavigateUri = new Uri(AppHelper.AssemblyInfo.SupportUrl);
            ProductSupportUrl.ToolTip = AppHelper.AssemblyInfo.SupportUrl;
            ProductSupportUrl.Inlines.Add("Submit bug reports or feature requests.");
            ProductLicense.Text = AppHelper.AssemblyInfo.License;
            OkButton.Content = "OK";

            new WindowSystemMenu(this) { IconRemoved = true };
        }


        private void ProductSupportUrl_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }

}
