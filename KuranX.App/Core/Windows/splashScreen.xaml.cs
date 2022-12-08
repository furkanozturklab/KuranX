using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for splashScreen.xaml
    /// </summary>
    public partial class splashScreen : Window
    {
        public splashScreen() => InitializeComponent();

        private int time = 30;
        private DispatcherTimer? timer;

        private void splashScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            timer = new DispatcherTimer(DispatcherPriority.Loaded);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
            */
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            loadingProgress.Value += 26;
            if (loadingProgress.Value >= 806)
            {
                loginScreen loginScreen = new loginScreen();
                this.Hide();
                loginScreen.Show();
                timer.Stop();
            }
            timerCount.Text = time.ToString();
            time--;
        }
    }
}