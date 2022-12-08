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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for LibraryOpenPage.xaml
    /// </summary>
    public partial class LibraryOpenPage : Page
    {
        public LibraryOpenPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainScreen.navigationWriter("library", "");
                libraryMain.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
            }
        }

        public Page PageCall()
        {
            try
            {
                App.mainScreen.navigationWriter("library", "");
                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
                return this;
            }
        }

        // -------------- Click Func  -------------- //

        private void libFileOpenFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = App.navLibraryFileFrame.PageCall();
                libraryMain.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void libNoteOpenFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = App.navLibraryNoteFolderFrame.PageCall();
                libraryMain.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // -------------- Click Func  -------------- //
    }
}