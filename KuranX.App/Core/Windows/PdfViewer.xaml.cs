using CefSharp;
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
using System.Windows.Shapes;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Window
    {
        public string currentfileUrl;

        public PdfViewer()
        {
            InitializeComponent();
        }

        public PdfViewer(string fileUrl, string fileName) : this()
        {
            try
            {
                currentfileUrl = fileUrl;
                this.Title = fileName;
            }
            catch (Exception ex)
            {
                App.logWriter("Loader", ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine(currentfileUrl);
                meltdown.Address = currentfileUrl;
            }
            catch (Exception ex)
            {
                App.logWriter("Loader", ex);
            }
        }
    }
}