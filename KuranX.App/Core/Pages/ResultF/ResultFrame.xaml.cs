using KuranX.App.Core.Classes;
using MySqlX.XDevAPI.Common;
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

namespace KuranX.App.Core.Pages.ResultF
{
    /// <summary>
    /// Interaction logic for ResultFrame.xaml
    /// </summary>
    public partial class ResultFrame : Page
    {
        private Task loadTask;
        private int lastPage = 0, NowPage = 1;

        public ResultFrame()
        {
            InitializeComponent();
        }

        public Page navResultPageCall()
        {
            loadTask = Task.Run(() => loadItem());
            return this;
        }

        // -------------- Load Func  -------------- //

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                var dResults = entitydb.Results.Skip(lastPage).Take(16).ToList();
                Decimal totalcount = entitydb.Results.Count();
                List<Classes.Result> dtemp = new List<Classes.Result>();

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
                    }

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
            }
        }

        // -------------- Load Func  -------------- //

        // -------------- Click Func  -------------- //

        private void resultItemClick_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = App.navResultItem.PageCall(int.Parse(btn.Uid));
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 16;
                NowPage++;
                loadTask = new Task(() => loadItem());
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
                if (lastPage >= 16)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 16;
                    NowPage--;
                    loadTask = new Task(() => loadItem());
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        // -------------- Click Func  -------------- //

        // -------------- Change Func  -------------- //

        private void fastsureCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                /*
                if (fastSureCheck)
                {
                    var item = fastsureCombobox.SelectedItem as ComboBoxItem;
                    if (item.Uid != "0") App.mainframe.Content = new ResultItem(int.Parse(item.Uid));
                }
                else fastSureCheck = true;
                */
            }
            catch (Exception ex)
            {
                App.logWriter("SelectEvent", ex);
            }
        }

        // -------------- Change Func  -------------- //
    }
}