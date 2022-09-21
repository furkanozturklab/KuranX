using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using KuranX.App.Core.Classes;
using Ubiety.Dns.Core;

namespace KuranX.App.Core.Pages.VerseF
{
    /// <summary>
    /// Interaction logic for ayetlerFrame.xaml
    /// </summary>
    public partial class versesFrame : Page
    {
        private List<Sure> verses = new List<Sure>();
        private int NowPage, CurrentSkip, tempRelativeVerseId, tempSureId;
        private Task PageItemLoadTask;
        private string Landing, VersesStatus;
        private bool tempCheck = false, tempCheck1 = false;
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public versesFrame(int nowPageD, int CurrentSkipD, string LandingD)
        {
            try
            {
                InitializeComponent();

                NowPage = nowPageD;
                CurrentSkip = CurrentSkipD;
                Landing = LandingD;

                if (Landing != "Hepsi") deskingStack.Visibility = Visibility.Hidden;

                switch (Landing)
                {
                    case "Hepsi":
                        landingCombobox.SelectedIndex = 0;

                        break;

                    case "Mekke":
                        landingCombobox.SelectedIndex = 1;

                        break;

                    case "Medine":
                        landingCombobox.SelectedIndex = 2;

                        break;
                }

                VersesStatus = "All";
                nextpageButton.IsEnabled = false;
                previusPageButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        public void controlDisable()
        {
            this.Dispatcher.Invoke(() =>
            {
                allCheck.IsEnabled = false;
                readCheck.IsEnabled = false;
                noraedCheck.IsEnabled = false;
                gotoMarkLocation.IsEnabled = false;
                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;
                landingCombobox.IsEnabled = false;
                deskingCombobox.IsEnabled = false;

                loadinGifContent.Visibility = Visibility.Visible;
                loadingPanel.Visibility = Visibility.Hidden;
            });
        }

        public void controlActive()
        {
            this.Dispatcher.Invoke(() =>
            {
                allCheck.IsEnabled = true;
                readCheck.IsEnabled = true;
                noraedCheck.IsEnabled = true;
                gotoMarkLocation.IsEnabled = true;
                landingCombobox.IsEnabled = true;
                deskingCombobox.IsEnabled = true;

                loadinGifContent.Visibility = Visibility.Collapsed;
                loadingPanel.Visibility = Visibility.Visible;
            });
        }

        private void loadVerses()
        {
            try
            {
                controlDisable();

                using (var entitydb = new AyetContext())
                {
                    List<Sure> VersesList;
                    decimal totalCount;

                    deskingCombobox.Dispatcher.Invoke(() =>
                    {
                        if (App.currentDesktype == "DeskLanding") deskingCombobox.SelectedIndex = 0;
                        else deskingCombobox.SelectedIndex = 1;
                    });

                    // ItemsSourceClear
                    for (int x = 1; x < 16; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            ItemsControl itemslist = (ItemsControl)this.FindName("aIcontrol" + x);
                            itemslist.ItemsSource = null;
                        });
                    }

                    // Filter
                    if (VersesStatus == "All")
                    {
                        // TÜMÜ

                        if (Landing != "Hepsi")
                        {
                            totalCount = decimal.Parse(entitydb.Sure.Where(p => p.LandingLocation == Landing).ToList().Count().ToString());

                            if (App.currentDesktype == "DeskLanding")
                            {
                                if (App.currentLanding == "Hepsi") VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                                else VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == App.currentLanding).OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                            }
                            else
                            {
                                if (App.currentLanding == "Hepsi") VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                                else VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == App.currentLanding).OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                            }
                        }
                        else
                        {
                            totalCount = decimal.Parse(entitydb.Sure.ToList().Count().ToString());
                            if (App.currentDesktype == "DeskLanding")
                            {
                                VersesList = (List<Sure>)entitydb.Sure.OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                            }
                            else
                            {
                                VersesList = (List<Sure>)entitydb.Sure.OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                            }
                        }
                    }
                    else if (VersesStatus == "Read")
                    {
                        // OKUDUKLARIM

                        if (Landing != "Hepsi")
                        {
                            totalCount = decimal.Parse(entitydb.Sure.Where(p => p.LandingLocation == Landing).ToList().Count().ToString());
                            if (App.currentDesktype == "DeskLanding")
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).Where(p => p.Status == "#66E21F").OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                            }
                            else
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).Where(p => p.Status == "#66E21F").OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                            }
                        }
                        else
                        {
                            totalCount = decimal.Parse(entitydb.Sure.Where(p => p.Status == "#66E21F").ToList().Count().ToString());

                            if (App.currentDesktype == "DeskLanding")
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.Status == "#66E21F").OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                            }
                            else
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.Status == "#66E21F").OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                            }
                        }
                    }
                    else
                    {
                        // Okumadıklarım

                        if (Landing != "Hepsi")
                        {
                            totalCount = decimal.Parse(entitydb.Sure.Where(p => p.LandingLocation == Landing).ToList().Count().ToString());
                            if (App.currentDesktype == "DeskLanding")
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).Where(p => p.Status == "#ADB5BD").OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                            }
                            else
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).Where(p => p.Status == "#ADB5BD").OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                            }
                        }
                        else
                        {
                            totalCount = decimal.Parse(entitydb.Sure.Where(p => p.Status == "#ADB5BD").ToList().Count().ToString());

                            if (App.currentDesktype == "DeskLanding")
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.Status == "#ADB5BD").OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                            }
                            else
                            {
                                VersesList = (List<Sure>)entitydb.Sure.Where(p => p.Status == "#ADB5BD").OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                            }
                        }
                    }

                    Thread.Sleep(200);

                    if (VersesList.Count() != 0)
                    {
                        int x = 1;
                        foreach (var item in VersesList)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                verses.Add(item);

                                if (App.currentDesktype == "DeskLanding") verses[0].DeskList = verses[0].DeskLanding;
                                else if (App.currentDesktype == "DeskMushaf") verses[0].DeskList = verses[0].DeskMushaf;

                                ItemsControl itemslist = (ItemsControl)this.FindName("aIcontrol" + x);
                                itemslist.ItemsSource = verses;
                                verses.Clear();
                                x++;
                            });
                        }

                        totalCount = Math.Ceiling(totalCount / 15);

                        nowPageStatus.Dispatcher.Invoke(() =>
                        {
                            nowPageStatus.Tag = NowPage + " / " + totalCount;
                        });

                        nextpageButton.Dispatcher.Invoke(() =>
                        {
                            if (NowPage != totalCount) nextpageButton.IsEnabled = true;
                            else if (NowPage == totalCount) nextpageButton.IsEnabled = false;
                        });
                        previusPageButton.Dispatcher.Invoke(() =>
                        {
                            if (NowPage != 1) previusPageButton.IsEnabled = true;
                            else if (NowPage == 1) previusPageButton.IsEnabled = false;
                        });
                    }
                    else
                    {
                        nowPageStatus.Dispatcher.Invoke(() =>
                        {
                            nowPageStatus.Tag = "-";
                        });
                        nextpageButton.Dispatcher.Invoke(() =>
                        {
                            nextpageButton.IsEnabled = false;
                        });
                        previusPageButton.Dispatcher.Invoke(() =>
                        {
                            previusPageButton.IsEnabled = false;
                        });
                    }

                    controlActive();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                controlDisable();
                PageItemLoadTask = new Task(loadVerses);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void deskingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tempCheck)
                {
                    var item = deskingCombobox.SelectedItem as ComboBoxItem;

                    switch (item.Content)
                    {
                        case "İnişe Göre":
                            App.currentDesktype = "DeskLanding";
                            break;

                        case "Mushafa Göre":
                            App.currentDesktype = "DeskMushaf";
                            break;
                    }
                    ChangeStatusPage();

                    PageItemLoadTask = new Task(loadVerses);
                    PageItemLoadTask.Start();
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("SelectEvent", ex);
            }
        }

        private void checkstatustype_click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton checkedval = sender as RadioButton;
                checkedval.IsChecked = true;
                NowPage = 1;
                CurrentSkip = 0;
                ChangeStatusPage();
                VersesStatus = checkedval.Tag.ToString();

                App.currentLanding = (string)checkedval.Tag;
                PageItemLoadTask = new Task(loadVerses);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void landingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (App.selectedBlock)
                {
                    var item = landingCombobox.SelectedItem as ComboBoxItem;
                    App.currentLanding = (string)item.Content;
                    Landing = item.Content.ToString();
                    if (tempCheck1)
                    {
                        if (Landing != "Hepsi") deskingStack.Visibility = Visibility.Hidden;
                        else deskingStack.Visibility = Visibility.Visible;

                        ChangeStatusPage();
                        NowPage = 1;
                        CurrentSkip = 0;
                        PageItemLoadTask = new Task(loadVerses);
                        PageItemLoadTask.Start();
                    }
                    else tempCheck1 = true;
                }
                else
                {
                    App.selectedBlock = true;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("SelectEvent", ex);
            }
        }

        private void ChangeStatusPage()
        {
            try
            {
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void gotoMarkLocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entity = new AyetContext())
                {
                    var selectedS = entity.Sure.Where(p => p.Status == "#0D6EFD").FirstOrDefault();
                    if (selectedS != null)
                    {
                        App.mainframe.Content = new verseFrame((int)selectedS.sureId, (int)selectedS.UserLastRelativeVerse, "Verse");
                    }
                    else
                    {
                        alertPopupHeader.Text = "Kaldığım Yere Gidilemedi";
                        alertPopupDetail.Text = "Henüz bir yer işaretlemediğinizden kaldığınız yere gidilemez";
                        alph.IsOpen = true;

                        timeSpan.Interval = TimeSpan.FromSeconds(3);
                        timeSpan.Start();
                        timeSpan.Tick += delegate
                        {
                            alph.IsOpen = false;
                            timeSpan.Stop();
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeStatusPage();
                NowPage++;
                CurrentSkip += 15;

                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;

                App.currentVersesPageD[0] = NowPage;
                App.currentVersesPageD[1] = CurrentSkip;
                PageItemLoadTask = new Task(loadVerses);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void fastLoadVerse_Click(object sender, RoutedEventArgs e)
        {
            if (tempRelativeVerseId >= Int16.Parse(popupRelativeId.Text))
            {
                App.mainframe.Content = new verseFrame(tempSureId, Int16.Parse(popupRelativeId.Text), "Verse");
            }
            else
            {
                popupRelativeId.Focus();
                fastopenVerseError.Text = "Böyle bir ayet mevcut değil !";
            }
        }

        private void BeforePage(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeStatusPage();
                NowPage--;
                CurrentSkip -= 15;

                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;

                App.currentVersesPageD[0] = NowPage;
                App.currentVersesPageD[1] = CurrentSkip;
                PageItemLoadTask = new Task(loadVerses);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void versesFastPopupOpen(object sender, RoutedEventArgs e)
        {
            fastopenVerse.IsOpen = true;

            var btn = sender as Button;
            tempRelativeVerseId = Int16.Parse(btn.Uid.ToString());
            tempSureId = Int16.Parse(btn.Tag.ToString());
            fastopenVerseInfo.Text = $"{btn.Content} Süresinin {btn.Uid} Adet Ayeti Mevcut";
        }

        private void versesPageOpen(object sender, RoutedEventArgs e)
        {
            try
            {
                Button getContent = sender as Button;
                App.mainframe.Content = new verseFrame(Int16.Parse(getContent.Tag.ToString()), 1, "Verse");
                /*
                verseFrame verseFrame = new verseFrame(int.Parse(getContent.Tag.ToString()));
                NavigationService.Navigate(verseFrame, UriKind.Relative);
                */
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);

                fastopenVerseError.Text = "Gitmek İstenilen Ayet Sırasını Giriniz";
                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }
    }
}