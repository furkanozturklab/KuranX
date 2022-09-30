﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for libraryFrame.xaml
    /// </summary>
    public partial class libraryFrame : Page
    {
        public libraryFrame()
        {
            InitializeComponent();
        }

        private void libFileOpenFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new libraryFileItemsFrame();
            }
            catch (Exception ex)
            {
                App.logWriter("FrameLoad", ex);
            }
        }

        private void libPublisherOpenFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new libraryPublisherItemsFrame();
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
                App.mainframe.Content = new libraryNotesItems();
            }
            catch (Exception ex)
            {
                App.logWriter("FrameLoad", ex);
            }
        }

        private void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadControlAni.Visibility = Visibility.Visible;
                loadHeaderAni.Visibility = Visibility.Visible;
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadAni();
        }
    }
}