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
            alph.MouseMove += new MouseEventHandler(pop_MouseMove);
        }

        private void pop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = e.GetPosition(this.alph);

                alph.PlacementRectangle = new Rect(new Point(e.GetPosition(this).X,
                    e.GetPosition(this).Y), new Point(mousePosition.X, mousePosition.Y));
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            Button btntemp = sender as Button;
            var popuptemp = (Popup)this.FindName(btntemp.Uid);

            // popuptemp.IsOpen = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //  frame.Content = navMany;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // frame.Content = navMany.reverseLoad(val1.Text, val2.Text);
        }

        private void popup_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void alph_MouseEnter(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(this.alph);
            this.alph.HorizontalOffset = mousePosition.X;
            this.alph.VerticalOffset = mousePosition.Y;
        }
    }
}