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

namespace KuranX.App.Core.UI.Settings
{
    /// <summary>
    /// Interaction logic for ChangeButton.xaml
    /// </summary>
    public partial class ChangeButton : UserControl
    {

        public event EventHandler<string> changeType;

        public ChangeButton()
        {
            InitializeComponent();
            
        }


        public void settingPageChange_click(object sender, RoutedEventArgs e)
        {

            foreach (UIElement child in controlGrid.Children)
            {
                if (child is CheckBox chk)
                {
                    chk.IsChecked = false;
                }
            }

            var schk = sender as CheckBox;
            schk!.IsChecked = true;
            changeType?.Invoke(this, schk!.Uid);

        }
    }
}
