using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KuranX.App.Core.Pages;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Window
    {
        public homeScreen()
        {
            InitializeComponent();
        }

        private void enterFullScreenIcon_Click(object sender,
            RoutedEventArgs e)
        {
            leftHeaderButtonsProfile.Style = (Style)FindResource("hmwnd_leftHeaderButtonsActive");
            leftHeaderButtonsProfileIcon.Style = (Style)FindResource("IconFontHidden");
            hmwnd_headerH1.Style = (Style)FindResource("hmwnd_leftHeaderTexth1Active");
            hmwnd_headerH2.Style = (Style)FindResource("hmwnd_leftHeaderTexth2Active");
            hmwd_profileImage.Style = (Style)FindResource("hmwnd_leftHeaderImageActive");

            foreach (var tb in menuButton.Children.OfType<Button>())
            {
                tb.Style = (Style)FindResource("hmwnd_menuButtonActive");
            }

            enterFullScreenIcon.Visibility = Visibility.Collapsed;
            exitFullScreenIcon.Visibility = Visibility.Visible;
            leftHeaderButtonsHelp.Visibility = Visibility.Collapsed;
        }

        private void exitFullScreenIcon_Click(object sender,
            RoutedEventArgs e)
        {
            leftHeaderButtonsProfile.Style = (Style)FindResource("hmwnd_leftHeaderButtons");
            leftHeaderButtonsProfileIcon.Style = null;
            hmwnd_headerH1.Style = (Style)FindResource("hmwnd_leftHeaderTexth1");
            hmwnd_headerH2.Style = (Style)FindResource("hmwnd_leftHeaderTexth2");
            hmwd_profileImage.Style = (Style)FindResource("hmwnd_leftHeaderImageActive");

            foreach (var tb in menuButton.Children.OfType<Button>())
            {
                tb.Style = (Style)FindResource("hmwnd_menuButton");
            }

            enterFullScreenIcon.Visibility = Visibility.Visible;
            exitFullScreenIcon.Visibility = Visibility.Collapsed;
            leftHeaderButtonsHelp.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //homeFrame.Content = new versesFrame();

            homeFrame.Content = new verseFrame(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AdminScreen adm = new AdminScreen();
            adm.Show();
        }
    }
}