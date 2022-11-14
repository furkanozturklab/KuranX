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
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.VerseF
{
    /// <summary>
    /// Interaction logic for sureFrame.xaml
    /// </summary>
    public partial class sureFrame : Page
    {
        private Task loadTask;
        private ComboBoxItem deskItem, landItem;
        private int lastPage = 0, NowPage = 1;
        private List<Sure> dSure;
        private string readType = "All";
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public sureFrame()
        {
            InitializeComponent();
        }

        /* ----------- Load Func ----------- */

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadTask = Task.Run(() => loadSureFunc());
        }

        public void loadSureFunc()
        {
            using (var entitydb = new AyetContext())
            {
                loadingAni();
                this.Dispatcher.Invoke(() =>
                {
                    Decimal totalcount = 0;
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

                    for (int x = 1; x < 16; x++)
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("srItem" + x);
                        itemslist.ItemsSource = null;
                    }
                    int i = 1;

                    List<Sure> tempSure = new List<Sure>();

                    int clist;
                    int cControl = NowPage;
                    if (NowPage != 1) clist = (--cControl * 15) + 1;
                    else clist = 1;

                    foreach (var item in dSure)
                    {
                        tempSure.Add(item);
                        tempSure[0].DeskList = clist;
                        clist++;

                        ItemsControl itemslist = (ItemsControl)this.FindName("srItem" + i);
                        itemslist.ItemsSource = tempSure;
                        tempSure.Clear();
                        i++;
                    }

                    Thread.Sleep(200);

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

                loadingAniComplated();
            }
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
                loadTask = Task.Run(() => loadSureFunc());
            }
        }

        private void landingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            landItem = landingCombobox.SelectedItem as ComboBoxItem;
            var comboBox = (ComboBox)sender;
            if (!comboBox.IsLoaded) return;
            if (landItem != null)
            {
                loadTask = Task.Run(() => loadSureFunc());
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
            Regex regex = new Regex("[^1-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /* ----------- SelectionChanged FUNC ----------- */

        /* ----------- CLİCK FUNC ----------- */

        private void typeLoad_Click(object sender, RoutedEventArgs e)
        {
            var rd = sender as RadioButton;
            readType = rd.Tag.ToString();
            loadTask = Task.Run(() => loadSureFunc());
        }

        private void sr_FastOpen_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            popupSureId.Text = btn.Tag.ToString();
            popupMax.Text = btn.Uid;
            popupRelativeId.Text = "1";
            fastopenVerseError.Text = "Gitmek İstenilen Ayet Sırasını Giriniz";
            fastopenVerseInfo.Text = $"{btn.Content} Süresinin {btn.Uid} Adet Ayeti Mevcut";
            popup_fastopenVerse.IsOpen = true;
        }

        private void sr_Open_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = App.navVersePage.PageCall(int.Parse(btn.Tag.ToString()), 1, "Sure");
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 15;
                NowPage++;
                loadTask = Task.Run(() => loadSureFunc());
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
                    loadTask = Task.Run(() => loadSureFunc());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void fastLoadVerse_Click(object sender, RoutedEventArgs e)
        {
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
                var selectedS = entitydb.Sure.Where(p => p.Status == "#0D6EFD").FirstOrDefault();
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

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    alph.IsOpen = false;
                    timeSpan.Stop();
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

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    inph.IsOpen = false;
                    timeSpan.Stop();
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

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    scph.IsOpen = false;
                    timeSpan.Stop();
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
                    gotoMarkLocation.IsEnabled = true;
                    allCheck.IsEnabled = true;
                    readCheck.IsEnabled = true;
                    noraedCheck.IsEnabled = true;
                    fastsureCombobox.IsEnabled = true;
                    landingCombobox.IsEnabled = true;
                    deskingCombobox.IsEnabled = true;
                    gotoMarkLocation.IsEnabled = true;
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