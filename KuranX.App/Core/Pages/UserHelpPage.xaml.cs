using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Tools;
using System;

using System.Linq;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;


namespace KuranX.App.Core.Pages
{
    /// <summary>
    /// Interaction logic for UserHelpPage.xaml
    /// </summary>
    public partial class UserHelpPage : Page
    {
        public UserHelpPage()
        {
            InitializeComponent();
        }

        private string[] helpers = new string[5] ;

        public Page PageCall()
        {
    
            try
            {
                App.mainScreen.homescreengrid.IsEnabled = true;
                App.lastlocation = "UserHelpPage";
                return this;
            }
            catch (Exception ex)
            {

                Tools.logWriter("Loading", ex);
                return this;
            }
        }

        public void loadItems(string type)
        {
            using (var entitydb = new AyetContext())
            {
                var userHelp = entitydb.UserHelp.Where(p => p.baseName == type).ToList();
                helpsItems.Children.Clear();
                foreach (var item in userHelp)
                {
                    var controlBtn = new Button();
                    controlBtn.Style = (Style)FindResource("helpSection");
                    controlBtn.Click += helpsItems_Click;
                    controlBtn.Content = item.infoName;
                    controlBtn.Uid = item.infoImage;
                    controlBtn.Tag = item.description;
                    controlBtn.ToolTip = item.baseName;

                    helpsItems.Children.Add(controlBtn);
                }
            }
        }

        private void helpsMain_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            loadItems((string)btn.Content);
            helpImage.Visibility = Visibility.Hidden;
        }

        private void helpsItems_Click(object sender, RoutedEventArgs e)
        {
            helpImage.Visibility = Visibility.Hidden;
         
            var btn = sender as Button;
            helpImage.Source = (ImageSource)FindResource((string)btn.Uid);
            helpImage.Visibility = Visibility.Visible;


        }
    }
}