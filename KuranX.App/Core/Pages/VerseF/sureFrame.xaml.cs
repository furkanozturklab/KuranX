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
        public ComboBoxItem? deskItem, landItem;
        private int lastPage = 0, NowPage = 1, i, cControl, clist;
        private List<Sure> dSure = new List<Sure>();
        private string readType = "All";
        private Decimal totalcount = 0;

        public sureFrame()
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainScreen.loadinGifContent.Visibility = Visibility.Hidden;
                App.mainScreen.rightPanel.Visibility = Visibility.Visible;
                loadContent.Visibility = Visibility.Visible;
                //App.loadTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        public Page PageCall()
        {
            try

            {   /*
                lastPage = 0;
                NowPage = 1;
                */

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

        /* ----------- Load Func ----------- */

        private void loadItem()
        {
            try
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
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Count();
                                    }
                                    else if (landItem.Uid == "1")
                                    {
                                        // Mekke
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.landingLocation == "Mekke").Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Mekke").Count();
                                    }
                                    else
                                    {
                                        // Medine
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.landingLocation == "Medine").Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Medine").Count();
                                    }
                                }
                                else
                                {
                                    if (landItem.Uid == "0")
                                    {
                                        // Hepsi
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Count();
                                    }
                                    else if (landItem.Uid == "1")
                                    {
                                        // Mekke
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.landingLocation == "Mekke").Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Mekke").Count();
                                    }
                                    else
                                    {
                                        // Medine
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.landingLocation == "Medine").Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Medine").Count();
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
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.complated == true).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.complated == true).Count();
                                    }
                                    else if (landItem.Uid == "1")
                                    {
                                        // Mekke
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.landingLocation == "Mekke" && p.complated == true).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Mekke" && p.complated == true).Count();
                                    }
                                    else
                                    {
                                        // Medine
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.landingLocation == "Medine" && p.complated == true).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Medine" && p.complated == true).Count();
                                    }
                                }
                                else
                                {
                                    if (landItem.Uid == "0")
                                    {
                                        // Hepsi
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.complated == true).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.complated == true).Count();
                                    }
                                    else if (landItem.Uid == "1")
                                    {
                                        // Mekke
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.landingLocation == "Mekke" && p.complated == true).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Mekke" && p.complated == true).Count();
                                    }
                                    else
                                    {
                                        // Medine
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.landingLocation == "Medine" && p.complated == true).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Medine" && p.complated == true).Count();
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
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.complated == false).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.complated == false).Count();
                                    }
                                    else if (landItem.Uid == "1")
                                    {
                                        // Mekke
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.landingLocation == "Mekke" && p.complated == false).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Mekke" && p.complated == false).Count();
                                    }
                                    else
                                    {
                                        // Medine
                                        dSure = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.landingLocation == "Medine" && p.complated == false).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Medine" && p.complated == false).Count();
                                    }
                                }
                                else
                                {
                                    if (landItem.Uid == "0")
                                    {
                                        // Hepsi
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.complated == false).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.complated == false).Count();
                                    }
                                    else if (landItem.Uid == "1")
                                    {
                                        // Mekke
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.landingLocation == "Mekke" && p.complated == false).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Mekke" && p.complated == false).Count();
                                    }
                                    else
                                    {
                                        // Medine
                                        dSure = entitydb.Sure.OrderBy(p => p.deskMushaf).Where(p => p.landingLocation == "Medine" && p.complated == false).Skip(lastPage).Take(15).ToList();
                                        totalcount = entitydb.Sure.Where(p => p.landingLocation == "Medine" && p.complated == false).Count();
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
                                sStatus.Background = new BrushConverter().ConvertFrom(item.status) as SolidColorBrush;

                                var sCount = (TextBlock)FindName("srDescCount" + i);
                                sCount.Text = clist.ToString();

                                var sLand = (TextBlock)FindName("srLanding" + i);
                                sLand.Text = item.landingLocation;

                                var sDesc = (TextBlock)FindName("srDesc" + i);
                                sDesc.Text = item.description;

                                var sName = (TextBlock)FindName("srName" + i);
                                sName.Text = item.name;

                                var sNumber = (TextBlock)FindName("srNumber" + i);
                                sNumber.Text = item.numberOfVerses.ToString();

                                var sFast = (Button)FindName("srBtnFast" + i);
                                sFast.Content = item.name;
                                sFast.Uid = item.numberOfVerses.ToString();
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

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

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
            catch (Exception ex)
            {
                App.logWriter("Load", ex);
            }
        }

        /* ----------- Load Func ----------- */

        /* ----------- SelectionChanged FUNC ----------- */

        private void deskingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                deskItem = deskingCombobox.SelectedItem as ComboBoxItem;

                var comboBox = (ComboBox)sender;
                if (!comboBox.IsLoaded) return;

                if (deskItem != null)
                {
                    App.loadTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void landingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                landItem = landingCombobox.SelectedItem as ComboBoxItem;
                var comboBox = (ComboBox)sender;
                if (!comboBox.IsLoaded) return;
                if (landItem != null)
                {
                    App.loadTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void fastsureCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var comboBox = (ComboBox)sender;
                if (!comboBox.IsLoaded) return;

                var item = fastsureCombobox.SelectedItem as ComboBoxItem;
                if (item.Uid != "0")
                {
                    App.beforeFrameName = "Sure";
                    App.mainframe.Content = App.navVersePage.PageCall(int.Parse(item.Uid), 1, "Hepsi");
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void popupRelativeId_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void popupRelativeId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        /* ----------- SelectionChanged FUNC ----------- */

        /* ----------- CLİCK FUNC ----------- */

        private void typeLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rd = sender as RadioButton;
                readType = rd.Tag.ToString();
                App.loadTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void sr_FastOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                popupSureId.Text = (string)btn.Tag;
                popupMax.Text = btn.Uid;
                popupRelativeId.Text = "1";
                fastopenVerseError.Text = "Gitmek İstenilen Ayet Sırasını Giriniz";
                fastopenVerseInfo.Text = $"{btn.Content} Süresinin {btn.Uid} Adet Ayeti Mevcut";
                popup_fastopenVerse.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void sr_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                loadContent.Visibility = Visibility.Hidden;
                App.beforeFrameName = "Sure";
                App.mainframe.Content = App.navVersePage.PageCall(int.Parse((string)btn.Tag), 1, "Sure");
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
                lastPage += 15;
                NowPage++;

                App.loadTask = Task.Run(() => loadItem());
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
                App.logWriter("Click", ex);
            }
        }

        private void fastLoadVerse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadContent.Visibility = Visibility.Hidden;
                App.beforeFrameName = "Sure";
                App.mainframe.Content = App.navVersePage.PageCall(int.Parse(popupSureId.Text), int.Parse(popupRelativeId.Text), "Verse");
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void gotoMarkLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var selectedS = entitydb.Sure.Where(p => p.userLastRelativeVerse != 0).FirstOrDefault();

                    if (selectedS != null)
                    {
                        App.beforeFrameName = "Sure";
                        App.mainframe.Content = App.navVersePage.PageCall((int)selectedS.sureId, (int)selectedS.userLastRelativeVerse, "Verse");
                    }
                    else
                    {
                        App.mainScreen.alertFunc("İşlem Başarısız", "Henüz bir yer işaretlemediğinizden kaldığınız yere gidilemez lütfen önce işaretlediğinizden emin olunuz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // ----------- CLİCK FUNC ----------- //

        // ----------- Ani Func ----------- //

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

        // ----------- Ani Func ----------- //
    }
}