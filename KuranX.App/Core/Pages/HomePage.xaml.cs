
using KuranX.App.Core.Classes.Tools;
using System;

using System.Windows;
using System.Windows.Controls;


namespace KuranX.App.Core.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainScreen.navigationWriter("home", "");
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        public Page PageCall()
        {
            try
            {
                this.Dispatcher.Invoke(() => App.mainScreen.navigationWriter("home", ""));

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);

                App.lastlocation = "HomePage";
                return this;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
                return this;
            }
        }
    }
}