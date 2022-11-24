using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages;
using Microsoft.EntityFrameworkCore;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Window
    {
        private CheckBox? navCheckBox;
        private bool taskstatus = true;

        public homeScreen()
        {
            InitializeComponent();

            App.mainframe = (Frame)this.FindName("homeFrame");
        }

        // ------------ Load Func  ------------ //

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var user = entitydb.Users.FirstOrDefault();

                    // hmwd_profileName.Text = user.FirstName + " " + user.LastName;

                    // hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(user.AvatarUrl);
                }

                // BAŞARILI CALIŞAN

                App.mainTask = Task.Run(() => welcoming());
                App.mainTask = Task.Run(() => navigationWriter("verse", ""));
                App.mainTask = Task.Run(() => remiderCycler_Func());

                App.mainTask.Wait();

                if (taskstatus) App.mainTask = Task.Run(() => tasksCycler_Func());

                //App.mainframe.Content = App.navTestPage.PageCall();
                App.mainframe.Content = App.navVersePage.PageCall(1, 1, "other");
            }
            catch (Exception ex)
            {
                App.logWriter("LoadedFunc", ex);
            }
        }

        private void homeFrame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        // ------------ Load Func  ------------ //

        // ------------ Special Func  ------------ //

        private void remiderCycler_Func()
        {
            // APP LİFECYCLER MİSSİONS

            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dRemider = entitydb.Remider.Where(p => p.lastAction < DateTime.Now && p.status == "Wait" || p.status == "Run").ToList();

                    foreach (var item in dRemider)
                    {
                        if (item.remiderDate < DateTime.Now && item.status == "Added")
                        {
                            entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.remiderId == item.remiderId));
                            entitydb.SaveChanges();
                            continue;
                        }

                        switch (item.loopType)
                        {
                            case "False":
                                if (item.remiderDate.ToString("d") == DateTime.Now.ToString("d") && item.lastAction.ToString("d") != DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;
                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#ffc107", missonsRepeart = 5, missonsTime = 5, missonsType = "Remider" };

                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Added";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Gün":

                                if (item.lastAction.AddDays(1).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#d63384", missonsRepeart = 5, missonsTime = 5, missonsType = "Remider" };
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Run";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Hafta":

                                if (item.lastAction.AddDays(7).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#0d6efd", missonsRepeart = 5, missonsTime = 5, missonsType = "Remider" };
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Run";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Ay":
                                if (item.lastAction.AddMonths(1).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#0dcaf0", missonsRepeart = 5, missonsTime = 5, missonsType = "Remider" };
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Run";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Yıl":
                                if (item.lastAction.AddYears(1).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#6610f2", missonsRepeart = 5, missonsTime = 5, missonsType = "Remider" };
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Run";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;
                        }

                        entitydb.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("SpecialFunc", ex);
            }
        }

        private void tasksCycler_Func()
        {
            try
            {
                taskstatus = true;

                using (var entitydb = new AyetContext())
                {
                    while (true)
                    {
                        var TasksLoad = entitydb.Tasks.ToList();

                        foreach (var item in TasksLoad)
                        {
                            // 5sn sonra sıradaki göreve geç
                            Thread.Sleep(5000); // 5 sn // 10 sn // 30 sn // 60 sn // 120 sn // 240 sn // Sırdaki göreve gecme süresi

                            var dRemider = entitydb.Remider.Where(p => p.remiderId == item.missonsId).FirstOrDefault();

                            if (dRemider != null)
                            {
                                entitydb.Tasks.Where(p => p.missonsId == dRemider.remiderId).First().missonsRepeart--;

                                if (entitydb.Tasks.Where(p => p.missonsId == dRemider.remiderId).First().missonsRepeart == 0) entitydb.Tasks.RemoveRange(entitydb.Tasks.Where(p => p.tasksId == item.tasksId));

                                entitydb.SaveChanges();

                                this.Dispatcher.Invoke(() =>
                                {
                                    lifeCyclerPopups.IsOpen = true;

                                    lifeCyclerPopupsText.Text = dRemider.remiderName;
                                    lifePopupGoActionButton.Uid = dRemider.remiderId.ToString();
                                    lifePopupGoActionButton.Tag = "Remider";
                                    if (dRemider.connectSureId == 0)
                                    {
                                        lifePopupGoDobuleActionButton.Visibility = Visibility.Hidden;
                                    }
                                    else
                                    {
                                        lifePopupGoDobuleActionButton.Visibility = Visibility.Visible;
                                        lifePopupGoDobuleActionButton.Uid = dRemider.remiderId.ToString();
                                    }

                                    lifeCyclerPopupBorder.Background = new BrushConverter().ConvertFrom(item.missonsColor) as SolidColorBrush;
                                });

                                // Missiontime * 10000 -> Mission time 5 -> 5*10000 = 50000mm -> 50 sn
                                Thread.Sleep(item.missonsTime * 10000); // 5 sn // 10 sn // 30 sn // 60 sn // 120 sn // 240 sn // Ekranda bekleme süresi

                                this.Dispatcher.Invoke(() =>
                                {
                                    lifeCyclerPopups.IsOpen = false;
                                    lifeCyclerPopupsText.Text = "";
                                    lifePopupGoActionButton.Uid = "";
                                    lifePopupGoActionButton.Tag = "";
                                });
                            }
                        }
                        if (TasksLoad.Count == 0)
                        {
                            taskstatus = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("SpecialFunc", ex);
            }
        }

        private void welcoming()
        {
            try
            {
                int i = DateTime.Now.Hour;

                this.Dispatcher.Invoke(() =>
                {
                    if (i >= 0 && i <= 7) hmwnd_headerH2.Text = "İyi Geceler";
                    if (i > 7 && i <= 11) hmwnd_headerH2.Text = "İyi Sabahlar";
                    if (i > 11 && i <= 18) hmwnd_headerH2.Text = "İyi Günler";
                    if (i > 18 && i <= 24) hmwnd_headerH2.Text = "İyi Akşamlar";
                });
            }
            catch (Exception ex)
            {
                App.logWriter("SpecialFunc", ex);
            }
        }

        public void navigationWriter(string baseicon, string location)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    NavigationStackItems.Children.Clear();

                    switch (baseicon)
                    {
                        case "home":
                            baseNavigation.Tag = "/resource/images/icon/dashboard_r.png";
                            break;

                        case "verse":
                            baseNavigation.Tag = "/resource/images/icon/verse_r.png";
                            break;

                        case "subject":
                            baseNavigation.Tag = "/resource/images/icon/subject_r.png";
                            break;

                        case "library":
                            baseNavigation.Tag = "/resource/images/icon/library_r.png";
                            break;

                        case "notes":
                            baseNavigation.Tag = "/resource/images/icon/notes_r.png";
                            break;

                        case "remider":
                            baseNavigation.Tag = "/resource/images/icon/remider_r.png";
                            break;

                        case "result":
                            baseNavigation.Tag = "/resource/images/icon/result_r.png";
                            break;

                        case "help":
                            baseNavigation.Tag = "/resource/images/icon/help_r.png";
                            break;

                        default:
                            baseNavigation.Tag = "/resource/images/icon/dashboard_r.png";
                            break;
                    }

                    if (location != "")
                    {
                        var locations = location.Split(",");

                        foreach (var item in locations)
                        {
                            var sp_btn = new Button();
                            var lc_txb = new TextBlock();
                            sp_btn.Style = (Style)FindResource("navigationSperator");
                            lc_txb.Style = (Style)FindResource("navigationLocationText");

                            lc_txb.Text = item.ToString();

                            NavigationStackItems.Children.Add(sp_btn);
                            NavigationStackItems.Children.Add(lc_txb);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                App.logWriter("SpecialFunc", ex);
            }
        }

        public void menuTask(CheckBox navCheckBox)
        {
            try
            {
                if (this.Dispatcher.Invoke(() => App.mainframe.Content.ToString().Split('.').Last() == "verseFrame"))
                {
                    // eğer verseFrame de isem çalış

                    if (this.Dispatcher.Invoke(() => App.navVersePage.markButton.IsChecked == true))
                    {
                        navClear();
                        // Verseframede işaretlemiş isem çalış
                        switch (this.Dispatcher.Invoke(() => navCheckBox.Content))
                        {
                            case "Ayetler":
                                this.Dispatcher.Invoke(() =>
                                {
                                    App.currentDesktype = "DeskLanding";
                                    App.mainframe.Content = App.navSurePage.PageCall();
                                });

                                break;

                            case "Konularım":
                                this.Dispatcher.Invoke(() =>
                                {
                                    App.mainframe.Content = App.navSubjectFrame.PageCall();
                                });
                                break;

                            case "Kütüphane":
                                this.Dispatcher.Invoke(() =>
                                {
                                    App.mainframe.Content = App.navLibraryOpen.PageCall();
                                });
                                break;

                            case "Notlar":
                                this.Dispatcher.Invoke(() =>
                                {
                                    App.mainframe.Content = App.navNotesPage.PageCall();
                                });
                                break;

                            case "Hatırlatıcı":
                                this.Dispatcher.Invoke(() =>
                                {
                                    App.mainframe.Content = App.navRemiderPage.PageCall();
                                });
                                break;

                            case "Sonuc":
                                this.Dispatcher.Invoke(() =>
                                {
                                    App.mainframe.Content = App.navResultPage.PageCall();
                                });
                                break;

                            default:
                                this.Dispatcher.Invoke(() =>
                                {
                                    App.mainframe.Content = App.navTestPage;
                                });
                                break;
                        }
                    }
                    else this.Dispatcher.Invoke(() => popup_fastExitConfirm.IsOpen = true);
                    // Eğer işaretlenmemiş ise popup aç
                }
                else
                {
                    // eğer verseFramede değilsem normal çalış

                    navClear();
                    this.Dispatcher.Invoke(() =>
                    {
                        navCheckBox.IsChecked = true;
                    });

                    switch (this.Dispatcher.Invoke(() => navCheckBox.Content))
                    {
                        case "Ayetler":
                            this.Dispatcher.Invoke(() =>
                            {
                                App.currentDesktype = "DeskLanding";
                                App.mainframe.Content = App.navSurePage.PageCall();
                            });

                            break;

                        case "Konularım":
                            this.Dispatcher.Invoke(() =>
                            {
                                App.mainframe.Content = App.navSubjectFrame.PageCall();
                            });
                            break;

                        case "Kütüphane":
                            this.Dispatcher.Invoke(() =>
                            {
                                App.mainframe.Content = App.navLibraryOpen.PageCall();
                            });
                            break;

                        case "Notlar":
                            this.Dispatcher.Invoke(() =>
                            {
                                App.mainframe.Content = App.navNotesPage.PageCall();
                            });
                            break;

                        case "Hatırlatıcı":
                            this.Dispatcher.Invoke(() =>
                            {
                                App.mainframe.Content = App.navRemiderPage.PageCall();
                            });
                            break;

                        case "Sonuc":
                            this.Dispatcher.Invoke(() =>
                            {
                                App.mainframe.Content = App.navResultPage.PageCall();
                            });
                            break;

                        default:
                            this.Dispatcher.Invoke(() =>
                            {
                                App.mainframe.Content = App.navTestPage;
                            });
                            break;
                    }
                    Thread.Sleep(500);
                }

                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Hidden;
                    rightPanel.Visibility = Visibility.Visible;
                });

                GC.Collect();
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        public void navClear()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadinGifContent.Visibility = Visibility.Visible;
                rightPanel.Visibility = Visibility.Hidden;
                foreach (object item in hmwnd_leftNavControlStack.Children)
                {
                    CheckBox? element = (item as FrameworkElement) as CheckBox;
                    element.IsChecked = false;
                }
            });
        }

        // ------------ Special Func  ------------ //

        // ------------ Click Func  ------------  //

        private void appClosed_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuNavControl_Click(object sender, RoutedEventArgs e)
        {
            navCheckBox = sender as CheckBox;
            App.loadTask = Task.Run(() => menuTask((CheckBox)navCheckBox));
        }

        private void adminOpen_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel ad = new AdminPanel();
            ad.Show();
        }

        private void popupAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;

                using (var entitydb = new AyetContext())
                {
                    var dRemider = entitydb.Remider.Where(p => p.remiderId == int.Parse(btn.Uid)).FirstOrDefault();

                    if (dRemider != null) App.mainframe.Content = App.navRemiderItem.PageCall(dRemider.remiderId);

                    lifeCyclerPopups.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void popupActionDouble_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;

                using (var entitydb = new AyetContext())
                {
                    var dRemider = entitydb.Remider.Where(p => p.remiderId == int.Parse(btn.Uid)).FirstOrDefault();

                    if (dRemider != null && dRemider.connectSureId != 0) App.mainframe.Content = App.navVersePage.PageCall(dRemider.connectSureId, dRemider.connectVerseId, "Remider");

                    lifeCyclerPopups.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void popupExit_Click(object sender, RoutedEventArgs e)
        {
            lifeCyclerPopups.IsOpen = false;
        }

        private void hmwnd_sizeControl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox? cb = sender as CheckBox;

                if (cb.IsChecked == false)
                {
                    foreach (object item in hmwnd_leftNavControlStack.Children)
                    {
                        var element = (item as FrameworkElement) as CheckBox;
                        element.SetValue(Extensions.ExtendsStatus, false);
                        leftHeaderButtonsProfile.SetValue(Extensions.ExtendsStatus, false);
                    }
                }
                else
                {
                    foreach (object item in hmwnd_leftNavControlStack.Children)
                    {
                        var element = (item as FrameworkElement) as CheckBox;
                        element.SetValue(Extensions.ExtendsStatus, true);
                        leftHeaderButtonsProfile.SetValue(Extensions.ExtendsStatus, true);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void fastexitBtn_Click(object sender, RoutedEventArgs e)
        {
            popup_fastExitConfirm.IsOpen = false;

            foreach (object item in hmwnd_leftNavControlStack.Children)
            {
                CheckBox? element = (item as FrameworkElement) as CheckBox;

                element.IsChecked = false;
            }

            navCheckBox.IsChecked = true;

            switch (navCheckBox.Content)
            {
                case "Ayetler":
                    App.currentDesktype = "DeskLanding";

                    App.mainframe.Content = App.navSurePage.PageCall();

                    break;

                case "Konularım":

                    App.mainframe.Content = App.navSubjectFrame.PageCall();

                    break;

                case "Kütüphane":

                    App.mainframe.Content = App.navLibraryOpen.PageCall();

                    break;

                case "Notlar":

                    App.mainframe.Content = App.navNotesPage.PageCall();

                    break;

                case "Hatırlatıcı":

                    App.mainframe.Content = App.navRemiderPage.PageCall();

                    break;

                case "Sonuc":

                    App.mainframe.Content = App.navResultPage.PageCall();

                    break;

                default:

                    App.mainframe.Content = App.navTestPage;

                    break;
            }

            GC.Collect();
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);
                popuptemp.IsOpen = false;
                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // ------------ Click Func  ------------  //
    }
}