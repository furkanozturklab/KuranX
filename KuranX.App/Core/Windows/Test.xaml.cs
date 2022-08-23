using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
        }

        /*
        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            StackPanel itemsStack = new StackPanel();
            TextBlock headerText = new TextBlock();
            TextBlock noteText = new TextBlock();
            Button allshowButton = new Button();

            itemsStack.Style = (Style)FindResource("dynamicItemStackpanel");
            headerText.Style = (Style)FindResource("dynamicItemTextHeader");
            noteText.Style = (Style)FindResource("dynamicItemTextNote");
            allshowButton.Style = (Style)FindResource("dynamicItemShowButton");

            headerText.Text = "Bakara 1 Ayet";
            noteText.Text = "Alınan Not buraya gelecek la bebe";

            itemsStack.Children.Add(headerText);
            itemsStack.Children.Add(noteText);
            itemsStack.Children.Add(allshowButton);

            deneme.Children.Add(itemsStack);
        }
        */
    }
}