using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
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

            App.currentVersesPageD[0] = 1;
            App.currentVersesPageD[1] = 0;

            App.mainframe = (Frame)this.FindName("homeFrame");
            App.locationTxt = (TextBlock)this.FindName("UserLocationText");
            App.locationTxt.Text = "Ayetler";
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
            App.locationTxt.Text = "AnaSayfa";

            //App.mainframe.Content = new Pages.VerseF.versesFrame(App.currentVersesPageD[0], App.currentVersesPageD[1], "Hepsi");

            //App.mainframe.Content = new verseFrame(1, 1);

            //App.mainframe.Content = new Pages.SubjectF.SubjectFrame();

            //App.mainframe.Content = new SubjectItemsFrame();

            //App.mainframe.Content = new SubjectItem();

            //App.mainframe.Content = new Pages.SubjectF.SubjectItemFrame(13, "Fâtiha Suresinin 1 Ayeti", "Meltdown", "31.08.2022 19:54:27", (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFC107"));

            App.mainframe.Content = new Pages.LibraryF.libraryFrame();

            // App.mainframe.Content = new Pages.LibraryF.libraryFileItemsFrame();

            //App.mainframe.Content = new Pages.LibraryF.libraryPublisherItemsFrame();

            // App.mainframe.Content = new Pages.LibraryF.libraryOpenFile();

            //App.mainframe.Content = new Pages.LibraryF.libraryNote();
        }

        private void appClosed_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void navEnter_Click(object sender, RoutedEventArgs e)
        {
            var btn_nav = sender as Button;

            foreach (object item in menuButton.Children)
            {
            }

            switch (btn_nav.Uid)
            {
                case "nav_verses":
                    App.currentDesktype = "DeskLanding";
                    App.mainframe.Content = new Pages.VerseF.versesFrame(1, 0, "Hepsi");
                    break;

                case "nav_subject":
                    App.mainframe.Content = new Pages.SubjectF.SubjectFrame();
                    break;

                case "nav_library":
                    App.mainframe.Content = new Pages.LibraryF.libraryFrame();
                    break;

                default:
                    App.mainframe.Content = new TestFrame();

                    break;
            }
        }

        private void adminOpen_Click(object sender, RoutedEventArgs e)
        {
            AdminScreen adm = new AdminScreen();
            adm.Show();
        }
    }
}