using KuranX.App.Core.Classes;
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

        public Page PageCall()
        {
            //helpImage.Source = (ImageSource)FindResource("help_1");

            return this;
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
            helpsDesc.Visibility = Visibility.Hidden;
        }

        private void helpsItems_Click(object sender, RoutedEventArgs e)
        {
            helpImage.Visibility = Visibility.Hidden;
            helpsDesc.Visibility = Visibility.Hidden;
            var btn = sender as Button;
            helpImage.Source = (ImageSource)FindResource((string)btn.Uid);
            helpImage.Visibility = Visibility.Visible;

            helpsDesc.Text = btn.Tag.ToString();
            helpsDesc.Visibility = Visibility.Visible;
        }
    }
}