using KuranX.App.Core.Classes.Helpers;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
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

        
        private string SettingsSave;

        private DraggablePopupHelper drag;

        public TestFrame()
        {
            InitializeComponent();
    
        }


        public Page PageCall()
        {
            return this;
        }



        private void ShowPopupButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            //selectedPopup = FindName(btn.Uid) as Popup;
           

           
          
        
        }

        private void kapat(object sender, RoutedEventArgs e)
        {

            drag.Dispose();
            drag = null;
        

           
           
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            var starindex = 0;
            findİndex.Clear();

            // İndex Arama
            while (true)
            {
                starindex = searchDataContent.Text.IndexOf("dizgi", ++starindex);
                if (starindex == -1) break;
                findİndex.Add(starindex);
            }

            int id = 0;

            foreach (var item in findİndex)
            {
                var dataTextBlock = new TextBlock();
                var tempLoc = new TextBlock();
                var tempborder = new Border();
                var tempstackpanel = new StackPanel();
                var tempButton = new Button();

                tempborder.Style = (Style)FindResource("pp_searchResultPanelBorder");
                tempLoc.Style = (Style)FindResource("pp_searchResultLocation");
                dataTextBlock.Style = (Style)FindResource("pp_searchResultText");
                tempButton.Style = (Style)FindResource("pp_dynamicItemSearchButton");
                tempstackpanel.HorizontalAlignment = HorizontalAlignment.Left;

                id = int.Parse(item.ToString());

                id -= 20;

                if (id < 0)
                {
                    searchDataContent.SelectionStart = 0;
                    searchDataContent.SelectionLength = int.Parse(item.ToString());
                }
                else
                {
                    searchDataContent.SelectionStart = id;
                    searchDataContent.SelectionLength = 20;
                }

                dataTextBlock.Text = " ... " + searchDataContent.SelectedText;
                dataTextBlock.Inlines.Add(new Run("dizgi") { Foreground = Brushes.Red });

                id = ("dizgi").Length + int.Parse(item.ToString());

                searchDataContent.SelectionStart = id;
                searchDataContent.SelectionLength = 20;

                dataTextBlock.Inlines.Add(searchDataContent.SelectedText + " ... ");

                tempLoc.Text = "Yorum";

                tempstackpanel.Children.Add(dataTextBlock);
                tempstackpanel.Children.Add(tempLoc);
                tempstackpanel.Children.Add(tempButton);
                tempborder.Child = tempstackpanel;

                resultDataContent.Children.Add(tempborder);

                dataTextBlock = null;
            }

            */
        }
    }
}