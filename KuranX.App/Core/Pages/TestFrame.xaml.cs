using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestFrame : Page
    {
        public TestFrame()
        {
            InitializeComponent();
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            Button btntemp = sender as Button;
            var popuptemp = (Popup)this.FindName(btntemp.Uid);

            // popuptemp.IsOpen = false;
        }
    }
}