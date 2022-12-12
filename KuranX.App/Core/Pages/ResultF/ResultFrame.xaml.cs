using KuranX.App.Core.Classes;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
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
        private int lastPage = 0, NowPage = 1;

        public ResultFrame()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall()
        {
            try
            {
                lastPage = 0;
                NowPage = 1;
                App.mainScreen.loadinGifContent.Visibility = Visibility.Hidden;
                App.mainScreen.rightPanel.Visibility = Visibility.Visible;
                App.loadTask = Task.Run(() => loadItem());
                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
                return this;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                gridContent.Visibility = Visibility.Visible;
                App.mainScreen.navigationWriter("result", "");
            }
            catch (Exception ex)
            {
                App.logWriter("loading", ex);
            }
        }

        // -------------- Load Func  -------------- //

        public void loadItem()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    App.mainScreen.navigationWriter("result", "");
                    var dResults = entitydb.Results.Skip(lastPage).Take(16).ToList();
                    Decimal totalcount = entitydb.Results.Count();

                    for (int x = 1; x <= 16; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sbItem = (Border)FindName("rs" + x);
                            sbItem.Visibility = Visibility.Hidden;
                        });
                    }
                    int i = 1;

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));
                    foreach (var item in dResults)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sName = (TextBlock)FindName("rsName" + i);
                            sName.Text = item.resultName;

                            var sColor = (Border)FindName("rsColor" + i);
                            sColor.Background = new BrushConverter().ConvertFrom("#ADB5BD") as SolidColorBrush;

                            var sBtn = (Button)FindName("rsBtn" + i);
                            sBtn.Uid = item.resultId.ToString();

                            var sS = (Image)FindName("rsS" + i);
                            sS.IsEnabled = (bool)item.resultSubject;

                            var sL = (Image)FindName("rsL" + i);
                            sL.IsEnabled = (bool)item.resultLib;

                            var sN = (Image)FindName("rsN" + i);
                            sN.IsEnabled = (bool)item.resultNotes;

                            var sbItem = (Border)FindName("rs" + i);
                            sbItem.Visibility = Visibility.Visible;

                            i++;
                        });
                    }

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
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        // -------------- Load Func  -------------- //

        // -------------- Click Func  -------------- //

        private void resultItemClick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                gridContent.Visibility = Visibility.Hidden;
                App.mainframe.Content = App.navResultItem.PageCall(int.Parse(btn.Uid));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 16;
                NowPage++;
                App.loadTask = new Task(() => loadItem());
                App.loadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                    App.loadTask = new Task(() => loadItem());
                    App.loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // -------------- Click Func  -------------- //

        // -------------- Change Func  -------------- //

        private void fastsureCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cbi = fastsureCombobox.SelectedItem as ComboBoxItem;
                if (cbi != null)
                {
                    if (cbi.Uid != "0")
                    {
                        if (cbi.Uid != "0") App.mainframe.Content = App.navResultItem.PageCall(int.Parse(cbi.Uid));
                    }
                }
                cbi = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        // -------------- Change Func  -------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    fastsureCombobox.IsEnabled = false;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        public void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    fastsureCombobox.IsEnabled = true;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //
    }
}