using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace KuranX.App.Core.Pages.VerseF
{
    /// <summary>
    /// Interaction logic for sureFrame.xaml
    /// </summary>
    public partial class sureFrame : Page
    {
        private ComboBoxItem deskItem, landItem;
        private int lastPage = 0, NowPage = 1, i, cControl, clist;
        private List<Sure> dSure = new List<Sure>();
        private string readType = "All";
        private Decimal totalcount = 0;

        public sureFrame()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadContent.Visibility = Visibility.Visible;
        }

        public Page PageCall()
        {
            lastPage = 0;
            NowPage = 1;
            App.loadTask = Task.Run(() => loadItem());
            return this;
        }

        /* ----------- Load Func ----------- */

        private void loadItem()
        {
            loadingAni();
            App.mainScreen.navigationWriter("verse", "");
            using (var entitydb = new AyetContext())
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (readType == "All")
                    {
                        if (deskItem != null)
                        {
                            if (deskItem.Uid == "0")
                            {
                                if (landItem.Uid == "0")
                                {
                                    // Hepsi
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Count();
                                }
                                else if (landItem.Uid == "1")
                                {
                                    // Mekke
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.LandingLocation == "Mekke").Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Mekke").Count();
                                }
                                else
                                {
                                    // Medine
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.LandingLocation == "Medine").Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Medine").Count();
                                }
                            }
                            else
                            {
                                if (landItem.Uid == "0")
                                {
                                    // Hepsi
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Count();
                                }
                                else if (landItem.Uid == "1")
                                {
                                    // Mekke
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.LandingLocation == "Mekke").Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Mekke").Count();
                                }
                                else
                                {
                                    // Medine
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.LandingLocation == "Medine").Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Medine").Count();
                                }
                            }
                        }
                    }
                    else if (readType == "Read")
                    {
                        if (deskItem != null)
                        {
                            if (deskItem.Uid == "0")
                            {
                                if (landItem.Uid == "0")
                                {
                                    // Hepsi
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.Complated == true).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.Complated == true).Count();
                                }
                                else if (landItem.Uid == "1")
                                {
                                    // Mekke
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.LandingLocation == "Mekke" && p.Complated == true).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Mekke" && p.Complated == true).Count();
                                }
                                else
                                {
                                    // Medine
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.LandingLocation == "Medine" && p.Complated == true).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Medine" && p.Complated == true).Count();
                                }
                            }
                            else
                            {
                                if (landItem.Uid == "0")
                                {
                                    // Hepsi
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.Complated == true).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.Complated == true).Count();
                                }
                                else if (landItem.Uid == "1")
                                {
                                    // Mekke
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.LandingLocation == "Mekke" && p.Complated == true).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Mekke" && p.Complated == true).Count();
                                }
                                else
                                {
                                    // Medine
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.LandingLocation == "Medine" && p.Complated == true).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Medine" && p.Complated == true).Count();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (deskItem != null)
                        {
                            if (deskItem.Uid == "0")
                            {
                                if (landItem.Uid == "0")
                                {
                                    // Hepsi
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.Complated == false).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.Complated == false).Count();
                                }
                                else if (landItem.Uid == "1")
                                {
                                    // Mekke
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.LandingLocation == "Mekke" && p.Complated == false).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Mekke" && p.Complated == false).Count();
                                }
                                else
                                {
                                    // Medine
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.LandingLocation == "Medine" && p.Complated == false).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Medine" && p.Complated == false).Count();
                                }
                            }
                            else
                            {
                                if (landItem.Uid == "0")
                                {
                                    // Hepsi
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.Complated == false).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.Complated == false).Count();
                                }
                                else if (landItem.Uid == "1")
                                {
                                    // Mekke
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.LandingLocation == "Mekke" && p.Complated == false).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Mekke" && p.Complated == false).Count();
                                }
                                else
                                {
                                    // Medine
                                    dSure = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.LandingLocation == "Medine" && p.Complated == false).Skip(lastPage).Take(15).ToList();
                                    totalcount = entitydb.Sure.Where(p => p.LandingLocation == "Medine" && p.Complated == false).Count();
                                }
                            }
                        }
                    }
                });

                cControl = NowPage;
                if (NowPage != 1) clist = (--cControl * 15) + 1;
                else clist = 1;

                for (int x = 1; x <= 15; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var srItem = (Border)FindName("srItem" + x);
                        srItem.Visibility = Visibility.Hidden;
                    });
                }

                i = 1;

                foreach (var item in dSure)
                {
                    if (item != null)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sStatus = (Border)FindName("srColor" + i);
                            sStatus.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(item.Status);

                            var sCount = (TextBlock)FindName("srDescCount" + i);
                            sCount.Text = clist.ToString();

                            var sLand = (TextBlock)FindName("srLanding" + i);
                            sLand.Text = item.LandingLocation;

                            var sDesc = (TextBlock)FindName("srDesc" + i);
                            sDesc.Text = item.Description;

                            var sName = (TextBlock)FindName("srName" + i);
                            sName.Text = item.Name;

                            var sNumber = (TextBlock)FindName("srNumber" + i);
                            sNumber.Text = item.NumberOfVerses.ToString();

                            var sFast = (Button)FindName("srBtnFast" + i);
                            sFast.Content = item.Name;
                            sFast.Uid = item.NumberOfVerses.ToString();
                            sFast.Tag = item.sureId.ToString();

                            var sClick = (Button)FindName("srBtn" + i);
                            sClick.Tag = item.sureId.ToString();

                            var srItem = (Border)FindName("srItem" + i);
                            srItem.Visibility = Visibility.Visible;
                        });
                        clist++;
                        i++;
                    }
                }

                Thread.Sleep(200);

                this.Dispatcher.Invoke(() =>
                {
                    if (dSure.Count() != 0)
                    {
                        totalcount = Math.Ceiling(totalcount / 15);
                        nowPageStatus.Tag = NowPage + " / " + totalcount.ToString();
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
            loadingAniComplated();
        }

        /* ----------- Load Func ----------- */

        /* ----------- SelectionChanged FUNC ----------- */

        private void deskingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deskItem = deskingCombobox.SelectedItem as ComboBoxItem;

            var comboBox = (ComboBox)sender;
            if (!comboBox.IsLoaded) return;

            if (deskItem != null)
            {
                App.loadTask = Task.Run(() => loadItem());
            }
        }

        private void landingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            landItem = landingCombobox.SelectedItem as ComboBoxItem;
            var comboBox = (ComboBox)sender;
            if (!comboBox.IsLoaded) return;
            if (landItem != null)
            {
                App.loadTask = Task.Run(() => loadItem());
            }
        }

        private void fastsureCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (!comboBox.IsLoaded) return;

            var item = fastsureCombobox.SelectedItem as ComboBoxItem;
            if (item.Uid != "0") App.mainframe.Content = App.navVersePage.PageCall(int.Parse(item.Uid), 1, "Hepsi");
        }

        private void popupRelativeId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            if (!textbox.IsLoaded) return;

            if (popupRelativeId.Text != "" && popupRelativeId.Text != null)
            {
                if (int.Parse(popupRelativeId.Text) <= int.Parse(popupMax.Text) && int.Parse(popupRelativeId.Text) > 0)
                {
                    fastLoadVerse.IsEnabled = true;
                    fastopenVerseError.Text = "Ayet Mevcut Gidilebilir";
                }
                else
                {
                    fastLoadVerse.IsEnabled = false;
                    fastopenVerseError.Text = "Ayet Mevcut Değil";
                }
            }
            else
            {
                fastLoadVerse.IsEnabled = false;
                fastopenVerseError.Text = "Lütfen Ayet Sırasını Giriniz";
            }
        }

        private void popupRelativeId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /* ----------- SelectionChanged FUNC ----------- */

        /* ----------- CLİCK FUNC ----------- */

        private void typeLoad_Click(object sender, RoutedEventArgs e)
        {
            var rd = sender as RadioButton;
            readType = rd.Tag.ToString();
            App.loadTask = Task.Run(() => loadItem());
        }

        private void sr_FastOpen_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            popupSureId.Text = (string)btn.Tag;
            popupMax.Text = btn.Uid;
            popupRelativeId.Text = "1";
            fastopenVerseError.Text = "Gitmek İstenilen Ayet Sırasını Giriniz";
            fastopenVerseInfo.Text = $"{btn.Content} Süresinin {btn.Uid} Adet Ayeti Mevcut";
            popup_fastopenVerse.IsOpen = true;
        }

        private void sr_Open_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            loadContent.Visibility = Visibility.Hidden;
            App.mainframe.Content = App.navVersePage.PageCall(int.Parse(btn.Tag.ToString()), 1, "Sure");
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 15;
                NowPage++;

                App.loadTask = Task.Run(() => loadItem());
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
                if (lastPage >= 15)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 15;
                    NowPage--;

                    App.loadTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void fastLoadVerse_Click(object sender, RoutedEventArgs e)
        {
            loadContent.Visibility = Visibility.Hidden;
            App.mainframe.Content = App.navVersePage.PageCall(int.Parse(popupSureId.Text), int.Parse(popupRelativeId.Text), "Verse");
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void gotoMarkLocation_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                var selectedS = entitydb.Sure.Where(p => p.UserLastRelativeVerse != 0).FirstOrDefault();

                if (selectedS != null)
                {
                    App.mainframe.Content = App.navVersePage.PageCall((int)selectedS.sureId, (int)selectedS.UserLastRelativeVerse, "Verse");
                }
                else
                {
                    alertFunc("Kaldığım Yere Gidilemedi", "Henüz bir yer işaretlemediğinizden kaldığınız yere gidilemez", 3);
                }
            }
        }

        /* ----------- CLİCK FUNC ----------- */

        private void alertFunc(string header, string detail, int timespan)
        {
            try
            {
                alertPopupHeader.Text = header;
                alertPopupDetail.Text = detail;
                alph.IsOpen = true;

                App.timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                App.timeSpan.Start();
                App.timeSpan.Tick += delegate
                {
                    alph.IsOpen = false;
                    App.timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void infoFunc(string header, string detail, int timespan)
        {
            try
            {
                infoPopupHeader.Text = header;
                infoPopupDetail.Text = detail;
                inph.IsOpen = true;

                App.timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                App.timeSpan.Start();
                App.timeSpan.Tick += delegate
                {
                    inph.IsOpen = false;
                    App.timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void succsessFunc(string header, string detail, int timespan)
        {
            try
            {
                successPopupHeader.Text = header;
                successPopupDetail.Text = detail;
                scph.IsOpen = true;

                App.timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                App.timeSpan.Start();
                App.timeSpan.Tick += delegate
                {
                    scph.IsOpen = false;
                    App.timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        /* ----------- Ani Func -----------*/

        private void loadingAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadContent.Visibility = Visibility.Hidden;
                    gotoMarkLocation.IsEnabled = false;
                    allCheck.IsEnabled = false;
                    readCheck.IsEnabled = false;
                    noraedCheck.IsEnabled = false;
                    fastsureCombobox.IsEnabled = false;
                    landingCombobox.IsEnabled = false;
                    deskingCombobox.IsEnabled = false;
                    gotoMarkLocation.IsEnabled = false;
                    nextpageButton.IsEnabled = false;
                    previusPageButton.IsEnabled = false;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void loadingAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadContent.Visibility = Visibility.Visible;
                    gotoMarkLocation.IsEnabled = true;
                    allCheck.IsEnabled = true;
                    readCheck.IsEnabled = true;
                    noraedCheck.IsEnabled = true;
                    fastsureCombobox.IsEnabled = true;
                    landingCombobox.IsEnabled = true;
                    deskingCombobox.IsEnabled = true;
                    gotoMarkLocation.IsEnabled = true;

                    loadinGifContent.Visibility = Visibility.Collapsed;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        /* ----------- Ani Func -----------*/
    }
}