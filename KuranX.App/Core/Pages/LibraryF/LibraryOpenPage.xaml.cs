﻿using System;
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
            InitializeComponent();
        }

        private void libFileOpenFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = App.navLibraryFileFrame;
            }
            catch (Exception ex)
            {
                App.logWriter("FrameLoad", ex);
            }
        }

        private void libNoteOpenFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = App.navLibraryNoteFolderFrame.PageCall();
            }
            catch (Exception ex)
            {
                App.logWriter("FrameLoad", ex);
            }
        }
    }
}