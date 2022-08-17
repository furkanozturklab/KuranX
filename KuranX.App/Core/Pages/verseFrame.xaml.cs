using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for VerseFrame.xaml
    /// </summary>
    public partial class verseFrame : Page
    {
        public verseFrame()
        {
            InitializeComponent();
        }

        public verseFrame(string data) : this()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Click");
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = "Ahmet Baki";
            item.Name = "AhmetBaki";

            yorumcuCombobox.Items.Add(item);
        }
    }
}