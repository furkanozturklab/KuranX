using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.VerseF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace KuranX.App.Core.Pages.ResultF
{
    /// <summary>
    /// Interaction logic for ResultFrame.xaml
    /// </summary>
    public partial class ResultFrame : Page
    {
        private List<Result> dResults = new List<Result>();
        private int lastResult = 0, NowPage = 1;
        private Task loadTask;
        private bool fastSureCheck = false;

        public ResultFrame()
        {
            InitializeComponent();
        }

        private void resultItemClick_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = new ResultItem(int.Parse(btn.Uid));
        }

        private void loadResult()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                dResults = entitydb.Results.Skip(lastResult).Take(17).ToList();
                Decimal totalcount = entitydb.Results.Count();
                List<Result> dtemp = new List<Result>();

                this.Dispatcher.Invoke(() =>
                {
                    for (int x = 1; x < 17; x++)
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("rs" + x);
                        itemslist.ItemsSource = null;
                    }
                    int i = 1;
                    foreach (var item in dResults)
                    {
                        dtemp.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("rs" + i);
                        itemslist.ItemsSource = dtemp;
                        dtemp.Clear();
                        i++;
                        if (i == 17) break; // 17 den fazla varmı kontrol etmek için koydum
                    }
                });

                Thread.Sleep(200);
                this.Dispatcher.Invoke(() =>
                {
                    totalcountText.Tag = totalcount.ToString();

                    if (dResults.Count() != 0)
                    {
                        totalcount = Math.Ceiling(totalcount / 15);
                        nowPageStatus.Tag = NowPage + " / " + totalcount;
                        nextpageButton.Dispatcher.Invoke(() =>
                        {
                            if (NowPage != totalcount) nextpageButton.IsEnabled = true;
                            else if (NowPage == totalcount) nextpageButton.IsEnabled = false;
                        });
                        previusPageButton.Dispatcher.Invoke(() =>
                        {
                            if (NowPage != 1) previusPageButton.IsEnabled = true;
                            else if (NowPage == 1) previusPageButton.IsEnabled = false;
                        });
                    }
                    else
                    {
                        nowPageStatus.Tag = "-";
                        nextpageButton.IsEnabled = false;
                        previusPageButton.IsEnabled = false;
                    }
                });

                loadAniComplated();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadTask = new Task(loadResult);
            loadTask.Start();
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastResult += 16;
                NowPage++;
                loadTask = new Task(loadResult);
                loadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void previusPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lastResult >= 16)
                {
                    previusPageButton.IsEnabled = false;
                    lastResult -= 16;
                    NowPage--;
                    loadTask = new Task(loadResult);
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadinItemsGifContent.Visibility = Visibility.Visible;
                gridContent.Visibility = Visibility.Hidden;
                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;
                fastsureCombobox.IsEnabled = false;
            });
        }

        private void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadinGifContent.Visibility = Visibility.Collapsed;
                loadinItemsGifContent.Visibility = Visibility.Collapsed;
                gridContent.Visibility = Visibility.Visible;
                fastsureCombobox.IsEnabled = true;
            });
        }

        private void fastsureCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (fastSureCheck)
                {
                    var item = fastsureCombobox.SelectedItem as ComboBoxItem;
                    if (item.Uid != "0") App.mainframe.Content = new ResultItem(int.Parse(item.Uid));
                }
                else fastSureCheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("SelectEvent", ex);
            }
        }
    }
}