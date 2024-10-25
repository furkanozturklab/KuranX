﻿using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Helpers;
using KuranX.App.Core.Classes.Tools;
using KuranX.App.Core.Pages;
using KuranX.App.Core.UC.Settings;
using System;

using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

//using KuranX.App.Core.UC.Settings;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Window, Movebar
    {
        private CheckBox? navCheckBox;
        private SystemUI systemUIController;
        private RemiderUI remiderUIController;
        private OtherUI otherUIController;
        private AccessibilityUI accessibilityUIController;
        public ChangeButton? changeButton;
        private string SettingsSave;
        private bool taskstatus = true;
        private string pp_selected;

        public homeScreen()
        {
            try
            {
                InitializeComponent();
                if (App.config.AppSettings.Settings["app_write"].Value == "onH3n03o.58!982adsa") adminPanelOpen.Visibility = Visibility.Visible; 
                else adminPanelOpen.Visibility = Visibility.Collapsed;
                App.mainframe = (Frame)this.FindName("homeFrame");
                App.secondFrame = (Frame)this.FindName("secondFrame");
                AppVersion.Text = "Version " + App.config.AppSettings.Settings["app_version"].Value;
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        // ------------ Load Func  ------------ //

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SystemParameters.WorkArea.Width <= 1366)
                {
                    if (MessageBox.Show("Ekran Boyutunuz programı tüm fonksiyonları ile kullanmak için cok düşük lütfen programı kullanmaya devam ederken full ekranda çalışınız. Büyütmek istesermisiniz.",
                    "Configuration",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        fullscreenbutton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    }
                }

                // BAŞARILI CALIŞAN
                homescreengrid.IsEnabled = false;
                App.mainTask = Task.Run(() => welcoming());
                App.mainTask = Task.Run(() => navigationWriter("verse", ""));
                App.mainTask = Task.Run(() => remiderCycler_Func());
                App.loadTask = Task.Run(() => loadProfile());

                App.mainTask.Wait();

                Tools.errWrite($"[Session Stard - {DateTime.Now}]");

                if (taskstatus) App.mainTask = Task.Run(() => tasksCycler_Func());
                App.mainframe.Content = App.navHomeFrame.PageCall();

                homescreengrid.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loaded", ex);
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
                    var dRemider = entitydb.Remider.Where(p => p.lastAction < DateTime.Now && p.status == "Wait" || p.status == "Run" || p.status == "Added").ToList();

                    foreach (var item in dRemider)
                    {
                        if (item.remiderDate < DateTime.Now && item.status == "Added")
                        {
                            var controlRem = entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault();
                            if (controlRem.connectSureId != 0)
                            {
                                entitydb.Verse.Where(p => p.sureId == controlRem.connectSureId && p.relativeDesk == controlRem.connectVerseId).First().remiderCheck = false;
                            }

                            entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.remiderId == item.remiderId));
                            entitydb.SaveChanges();
                            continue;
                        }

                        switch (item.loopType)
                        {
                            case "Default":
                                if (DateTime.Parse(item.remiderDate.ToString("d")) <= DateTime.Parse(DateTime.Now.ToString("d")))
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;
                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#ffc107", missonsRepeart = int.Parse(App.config.AppSettings.Settings["app_remiderCount"].Value), missonsTime = int.Parse(App.config.AppSettings.Settings["app_remiderTime"].Value), missonsType = "Remider" };

                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Added";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Gün":

                                if (item.lastAction.AddDays(1) <= DateTime.Now)
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#d63384", missonsRepeart = int.Parse(App.config.AppSettings.Settings["app_remiderCount"].Value), missonsTime = int.Parse(App.config.AppSettings.Settings["app_remiderTime"].Value), missonsType = "Remider" };
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Run";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Hafta":

                                if (item.lastAction.AddDays(7) <= DateTime.Now)
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#0d6efd", missonsRepeart = int.Parse(App.config.AppSettings.Settings["app_remiderCount"].Value), missonsTime = int.Parse(App.config.AppSettings.Settings["app_remiderTime"].Value), missonsType = "Remider" };
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Run";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Ay":
                                if (item.lastAction.AddMonths(1) <= DateTime.Now)
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#0dcaf0", missonsRepeart = int.Parse(App.config.AppSettings.Settings["app_remiderCount"].Value), missonsTime = int.Parse(App.config.AppSettings.Settings["app_remiderTime"].Value), missonsType = "Remider" };
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).First().status = "Run";
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Yıl":
                                if (item.lastAction.AddYears(1) <= DateTime.Now)
                                {
                                    if (entitydb.Tasks.Where(p => p.missonsId == item.remiderId && p.missonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.remiderId == item.remiderId).FirstOrDefault().lastAction = DateTime.Now;

                                        var newD = new Tasks { missonsId = item.remiderId, missonsColor = "#6610f2", missonsRepeart = int.Parse(App.config.AppSettings.Settings["app_remiderCount"].Value), missonsTime = int.Parse(App.config.AppSettings.Settings["app_remiderTime"].Value), missonsType = "Remider" };
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
                Tools.logWriter("SpecialFunc", ex);
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
                            var sleepTime = int.Parse(App.config.AppSettings.Settings["app_remiderWaitTime"].Value) * 1000;
                            Thread.Sleep(sleepTime); // 5 sn // 10 sn // 30 sn // 60 sn // 120 sn // 240 sn // Sırdaki göreve gecme süresi

                            var dRemider = entitydb.Remider.Where(p => p.remiderId == item.missonsId).FirstOrDefault();

                            if (dRemider != null)
                            {
                                entitydb.Tasks.Where(p => p.missonsId == dRemider.remiderId).First().missonsRepeart--;

                                if (entitydb.Tasks.Where(p => p.missonsId == dRemider.remiderId).First().missonsRepeart == 0)
                                {
                                    entitydb.Tasks.RemoveRange(entitydb.Tasks.Where(p => p.tasksId == item.tasksId));
                                }

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
                                sleepTime = item.missonsTime * 1000;
                                Thread.Sleep(sleepTime); // 5 sn // 10 sn // 30 sn // 60 sn // 120 sn // 240 sn // Ekranda bekleme süresi

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
                Tools.logWriter("SpecialFunc", ex);
            }
        }

        private void welcoming()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var user = entitydb.Users.FirstOrDefault();

                    this.Dispatcher.Invoke(() => hmwd_profileName.Text = user.firstName + " " + user.lastName);

                    this.Dispatcher.Invoke(() => hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(user.avatarUrl));
                }

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
                Tools.logWriter("SpecialFunc", ex);
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
                            baseNavigation.Tag = "/resources/images/icon/dashboard_r.png";
                            break;

                        case "verse":
                            baseNavigation.Tag = "/resources/images/icon/verse_r.png";
                            break;

                        case "subject":
                            baseNavigation.Tag = "/resources/images/icon/subject_r.png";
                            break;

                        case "notes":
                            baseNavigation.Tag = "/resources/images/icon/notes_r.png";
                            break;

                        case "remider":
                            baseNavigation.Tag = "/resources/images/icon/remider_r.png";
                            break;

                        case "result":
                            baseNavigation.Tag = "/resources/images/icon/result_r.png";
                            break;

                        case "help":
                            baseNavigation.Tag = "/resources/images/icon/help_r.png";
                            break;

                        default:
                            baseNavigation.Tag = "/resources/images/icon/dashboard_r.png";
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
                Tools.logWriter("SpecialFunc", ex);
            }
        }

        public void menuTask(CheckBox navCheckBox)
        {
            try
            {
                if (this.Dispatcher.Invoke(() => App.mainframe.Content.ToString().Split('.').Last() == "verseFrame"))
                {
                    // eğer verseFrame veya sectionFrame de isem çalış

                    this.Dispatcher.Invoke(() =>
                    {
                        foreach (object item in hmwnd_leftNavControlStack.Children)
                        {
                            CheckBox? element = (item as FrameworkElement) as CheckBox;
                            element.IsChecked = false;
                        }
                        specialNav.IsChecked = true;
                    });

                    if (this.Dispatcher.Invoke(() => App.navVersePage.markButton.IsChecked == true))
                    {
                        // Verseframede işaretlemiş isem çalış

                        switch (this.Dispatcher.Invoke(() => navCheckBox.Content))
                        {
                            case "AnaSayfa":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navHomeFrame.PageCall();
                                });

                                break;

                            case "Ayetler":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    Debug.WriteLine("5-> WORK");
                                    App.mainframe.Content = App.navSurePage.PageCall();
                                });

                                break;

                            case "Konularım":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navSubjectFrame.PageCall();
                                });
                                break;

                            case "Notlar":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navNotesPage.PageCall();
                                });
                                break;

                            case "Hatırlatıcı":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navRemiderPage.PageCall();
                                });
                                break;

                            case "Kullanıcı Yardımı":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navUserHelp.PageCall();
                                });
                                break;

                            default:
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navTestPage;
                                });
                                break;
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            PopupHelpers.load_drag(popup_fastExitConfirm);
                            popup_fastExitConfirm.IsOpen = true;
                        });
                    }

                    // Eğer işaretlenmemiş ise popup aç
                }
                else if (this.Dispatcher.Invoke(() => App.mainframe.Content.ToString().Split('.').Last() == "SectionFrame"))
                {
                    // eğer verseFrame veya sectionFrame de isem çalış

                    this.Dispatcher.Invoke(() =>
                    {
                        foreach (object item in hmwnd_leftNavControlStack.Children)
                        {
                            CheckBox? element = (item as FrameworkElement) as CheckBox;
                            element.IsChecked = false;
                        }
                        specialNav.IsChecked = true;
                    });

                    if (this.Dispatcher.Invoke(() => App.navSectionPage.markButton.IsChecked == true))
                    {
                        // Verseframede işaretlemiş isem çalış

                        switch (this.Dispatcher.Invoke(() => navCheckBox.Content))
                        {
                            case "AnaSayfa":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navHomeFrame.PageCall();
                                });

                                break;

                            case "Ayetler":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    Debug.WriteLine("2-> WORK");
                                    App.mainframe.Content = App.navSurePage.PageCall();
                                });

                                break;

                            case "Konularım":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navSubjectFrame.PageCall();
                                });
                                break;

                            case "Notlar":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navNotesPage.PageCall();
                                });
                                break;

                            case "Hatırlatıcı":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navRemiderPage.PageCall();
                                });
                                break;

                            case "Kullanıcı Yardımı":
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navUserHelp.PageCall();
                                });
                                break;

                            default:
                                this.Dispatcher.Invoke(() =>
                                {
                                    navClear();
                                    navCheckBox.IsChecked = true;
                                    App.mainframe.Content = App.navTestPage;
                                });
                                break;
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() => { PopupHelpers.load_drag(popup_fastExitConfirm); popup_fastExitConfirm.IsOpen = true; });
                    }

                    // Eğer işaretlenmemiş ise popup aç
                }
                else
                {
                    switch (this.Dispatcher.Invoke(() => navCheckBox.Content))
                    {
                        case "AnaSayfa":
                            this.Dispatcher.Invoke(() =>
                            {
                                navClear();
                                navCheckBox.IsChecked = true;
                                App.mainframe.Content = App.navHomeFrame.PageCall();
                            });

                            break;

                        case "Ayetler":
                            this.Dispatcher.Invoke(() =>
                            {
                                navClear();
                                navCheckBox.IsChecked = true;
                                Debug.WriteLine("3-> WORK");
                                App.mainframe.Content = App.navSurePage.PageCall();
                            });

                            break;

                        case "Konularım":
                            this.Dispatcher.Invoke(() =>
                            {
                                navClear();
                                navCheckBox.IsChecked = true;
                                App.mainframe.Content = App.navSubjectFrame.PageCall();
                            });
                            break;

                        case "Notlar":
                            this.Dispatcher.Invoke(() =>
                            {
                                navClear();
                                navCheckBox.IsChecked = true;
                                App.mainframe.Content = App.navNotesPage.PageCall();
                            });
                            break;

                        case "Hatırlatıcı":
                            this.Dispatcher.Invoke(() =>
                            {
                                navClear();
                                navCheckBox.IsChecked = true;
                                App.mainframe.Content = App.navRemiderPage.PageCall();
                            });
                            break;

                        case "Kullanıcı Yardımı":
                            this.Dispatcher.Invoke(() =>
                            {
                                navClear();
                                navCheckBox.IsChecked = true;
                                App.mainframe.Content = App.navUserHelp.PageCall();
                            });
                            break;

                        default:
                            this.Dispatcher.Invoke(() =>
                            {
                                navClear();
                                navCheckBox.IsChecked = true;
                                App.mainframe.Content = App.navTestPage;
                            });
                            break;
                    }
                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));
                }

                Thread.Sleep(200);

                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Hidden;
                    rightPanel.Visibility = Visibility.Visible;
                });

                GC.Collect();
            }
            catch (Exception ex)
            {
                Tools.logWriter("ClickFunc", ex);
            }
        }

        public void navClear()
        {
            try
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
            catch (Exception ex)
            {
                Tools.logWriter("Loaded", ex);
            }
        }

        // ------------ Special Func  ------------ //

        // ------------ Click Func  ------------  //

        private void appClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Dispatcher.Invoke(() => App.mainframe.Content.ToString().Split('.').Last() == "verseFrame"))
                {
                    if (this.Dispatcher.Invoke(() => App.navVersePage.markButton.IsChecked == true))
                    {
                        if (MessageBox.Show("Uygulamayı kapatmak istediğinize emin misiniz ? ", "Uygulamayı Kapat", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Uygulamayı kapatmak önce kaldığınız yer işaretlenmemiş. Çıkmak istediğinize emin misiniz ? ", "Uygulamayı Kapat", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("Uygulamayı kapatmak istediğinize emin misiniz ? ", "Uygulamayı Kapat", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void appFull_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void appMin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void menuNavControl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.secondFrame.Visibility = Visibility.Collapsed;
                navCheckBox = sender as CheckBox;

                homescreengrid.IsEnabled = false;

                App.loadTask = Task.Run(() => menuTask(navCheckBox));
                Tools.errWrite($"[{DateTime.Now} Menu Click] -> {navCheckBox.Content}");
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void menuNavControl2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new TestFrame();
                // Tools.errWrite($"[{DateTime.Now} Menu Click] -> {navCheckBox.Content}");
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void adminOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AdminPanel ad = new AdminPanel();
                ad.Show();
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void userExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Oturumunuzu kapatmak üzeresiniz bu işlem sonucunda yeniden giriş yapmanız gerekiyor devam etmek istiyor musunuz ?", "Oturum Sonlandırma", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    App.config.AppSettings.Settings["user_rememberMe"].Value = "false";
                    App.config.AppSettings.Settings["user_autoLogin"].Value = "false";
                    App.config.AppSettings.Settings["user_pin"].Value = "";
                    App.config.Save(ConfigurationSaveMode.Modified);
                    loginScreen lg = new loginScreen();
                    lg.Show();
                    this.Hide();
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
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
                Tools.logWriter("Click", ex);
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

                    if (dRemider != null && dRemider.connectSureId != 0)
                    {
                        App.secondFrame.Visibility = Visibility.Visible;
                        App.secondFrame.Content = App.navVerseStickPage.PageCall(dRemider.connectSureId, dRemider.connectVerseId, "", 0, "FastRemider");
                    }

                    lifeCyclerPopups.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void popupExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lifeCyclerPopups.IsOpen = false;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
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
                Tools.logWriter("Click", ex);
            }
        }

        private void fastexitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupHelpers.dispose_drag(popup_fastExitConfirm);
                popup_fastExitConfirm.IsOpen = false;

                switch (navCheckBox.Content)
                {
                    case "AnaSayfa":
                        this.Dispatcher.Invoke(() =>
                        {
                            navClear();
                            navCheckBox.IsChecked = true;
                            App.mainframe.Content = App.navHomeFrame.PageCall();
                        });

                        break;

                    case "Ayetler":
                        this.Dispatcher.Invoke(() =>
                        {
                            navClear();

                            navCheckBox.IsChecked = true;
                            Debug.WriteLine("4-> WORK");
                            App.mainframe.Content = App.navSurePage.PageCall();
                        });
                        break;

                    case "Konularım":
                        this.Dispatcher.Invoke(() =>
                        {
                            navClear();

                            navCheckBox.IsChecked = true;
                            App.mainframe.Content = App.navSubjectFrame.PageCall();
                        });
                        break;

                    case "Notlar":
                        this.Dispatcher.Invoke(() =>
                        {
                            navClear();
                            navCheckBox.IsChecked = true;

                            App.mainframe.Content = App.navNotesPage.PageCall();
                        });
                        break;

                    case "Hatırlatıcı":
                        this.Dispatcher.Invoke(() =>
                        {
                            navClear();
                            navCheckBox.IsChecked = true;

                            App.mainframe.Content = App.navRemiderPage.PageCall();
                        });
                        break;

                    case "Kullanıcı Yardımı":
                        this.Dispatcher.Invoke(() =>
                        {
                            navClear();
                            navCheckBox.IsChecked = true;
                            App.mainframe.Content = App.navUserHelp.PageCall();
                        });
                        break;

                    default:
                        this.Dispatcher.Invoke(() =>
                        {
                            navClear();
                            navCheckBox.IsChecked = true;

                            App.mainframe.Content = App.navTestPage;
                        });
                        break;
                }
                Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Hidden;
                    rightPanel.Visibility = Visibility.Visible;
                });
                GC.Collect();
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp, pp_moveBar);
                btntemp = null;
                errDetail.Text = "";
                homescreengrid.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void leftHeaderButtonsProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    App.loadTask = Task.Run(() => loadProfile());
                    PopupHelpers.load_drag(popup_profileMain);
                    popup_profileMain.IsOpen = true;
                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        // ------------ Click Func  ------------  //

        public void alertFunc(string header, string detail, int timespan)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    alertPopupHeader.Text = header;
                    alertPopupDetail.Text = detail;
                    alph.IsOpen = true;
                });

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
                Tools.logWriter("Message", ex);
            }
        }

        public void succsessFunc(string header, string detail, int timespan)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    successPopupHeader.Text = header;
                    successPopupDetail.Text = detail;
                    scph.IsOpen = true;
                });
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
                Tools.logWriter("Message", ex);
            }
        }

        public void infoFunc(string header, string detail, int timespan)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    infoPopupHeader.Text = header;
                    infoPopupDetail.Text = detail;
                    inph.IsOpen = true;
                });
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
                Tools.logWriter("Other", ex);
            }
        }

        // ---------------------  PROFİLE POPUP FUNC --------------------- //

        private void loadProfile()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} LoadProfile] -> Profil Load");
                using (var entitydb = new AyetContext())
                {
                    var dProfile = entitydb.Users.FirstOrDefault();

                    if (dProfile != null)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            userName.Text = dProfile.firstName;
                            userLastName.Text = dProfile.lastName;
                            userEmail.Text = dProfile.email;
                            userPin.Text = dProfile.pin;
                            userScreetQuestion.Text = dProfile.screetQuestion;
                            userScreetQuestionAnw.Text = dProfile.screetQuestionAnw;
                            pp_profileImageBrush.ImageSource = (ImageSource)FindResource(dProfile.avatarUrl);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void userName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userNameErr.Content = "* Zorunlu Alan ";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void userLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userLastNameErr.Content = "* Zorunlu Alan ";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void userEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userEmailErr.Content = "* Zorunlu Alan ";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void userPin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userPinErr.Content = "* Boş Bırakılabilir ";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void userScreetQuestion_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userScreetQuestionErr.Content = "* Zorunlu Alan ";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void userScreetQuestionAnw_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                userScreetQuestionAnwErr.Content = "* Zorunlu Alan ";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void userName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9a-zA-ZğüşöçıİĞÜŞÖÇ)']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void contactNote_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9a-zA-ZğüşöçıİĞÜŞÖÇ),.!-+_']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void userEmail_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9a-zA-Z-@ğüşöçıİĞÜŞÖÇ?._ ']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void userPin_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void saveProfileFunc()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} SaveProfile] -> Profil saved");
                if (IsValidEmail(this.Dispatcher.Invoke(() => userEmail.Text)))
                {
                    if (this.Dispatcher.Invoke(() => userName.Text.Length) >= 3)
                    {
                        if (this.Dispatcher.Invoke(() => userLastName.Text.Length) >= 3)
                        {
                            if (this.Dispatcher.Invoke(() => userScreetQuestion.Text.Length) >= 3)
                            {
                                if (this.Dispatcher.Invoke(() => userScreetQuestionAnw.Text.Length) >= 3)
                                {
                                    using (var entitydb = new AyetContext())
                                    {
                                        var dUser = entitydb.Users.FirstOrDefault();

                                        if (dUser != null)
                                        {
                                            this.Dispatcher.Invoke(() =>
                                            {
                                                dUser.email = userEmail.Text;
                                                dUser.lastName = userLastName.Text;
                                                dUser.firstName = userName.Text;
                                                dUser.screetQuestion = userScreetQuestion.Text;
                                                dUser.screetQuestionAnw = userScreetQuestionAnw.Text;
                                                dUser.pin = userPin.Text;
                                                App.mainScreen.hmwd_profileName.Text = dUser.firstName + " " + dUser.lastName;
                                                entitydb.SaveChanges();
                                                App.mainScreen.succsessFunc("İşlem Başarılı", "Profil bilgileriniz başarılı bir sekilde güncellenmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        userScreetQuestionAnwErr.Content = "Minimum 3 karakter olmak zorunda.";
                                        userScreetQuestionAnw.Focus();
                                    });
                                }
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    userScreetQuestionErr.Content = "Minimum 3 karakter olmak zorunda.";
                                    userScreetQuestion.Focus();
                                });
                            }
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                userLastNameErr.Content = "Minimum 3 karakter olmak zorunda.";
                                userLastName.Focus();
                            });
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            userNameErr.Content = "Minimum 3 karakter olmak zorunda.";
                            userName.Focus();
                        });
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        userEmail.Focus();
                        userEmailErr.Content = "* Geçerli bir email adresi giriniz. ";
                    });
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void saveProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainTask = Task.Run(() => saveProfileFunc());
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void saveImageFunc(Button btntemp)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Users.First().avatarUrl = this.Dispatcher.Invoke(() => btntemp.Tag.ToString());
                    entitydb.SaveChanges();

                    this.Dispatcher.Invoke(() =>
                    {
                        hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(btntemp.Tag);
                        pp_profileImageBrush.ImageSource = (ImageSource)FindResource(btntemp.Tag);
                        App.mainScreen.hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(btntemp.Tag);
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Profil resminiz başarılı bir sekilde güncellenmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        PopupHelpers.dispose_drag(popup_ErrOpenPopup);
                        popup_editImage.IsOpen = false;
                    });
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void changeImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                App.mainTask = Task.Run(() => saveImageFunc(btntemp));
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void prEditImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupHelpers.load_drag(popup_editImage);
                popup_editImage.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        // -------- settings ------------ //

        public void switch_settings(object sender, string newContent)
        {
            loadUserControll.Children.Clear();
            switch (newContent)
            {
                case "SettingsAlt":
                    systemUIController = new SystemUI();
                    SettingsSave = newContent;
                    loadUserControll.Children.Add(systemUIController);
                    break;

                case "Accessibility":
                    accessibilityUIController = new AccessibilityUI();
                    SettingsSave = newContent;
                    loadUserControll.Children.Add(accessibilityUIController);
                    break;

                case "Bell":
                    remiderUIController = new RemiderUI();
                    SettingsSave = newContent;
                    loadUserControll.Children.Add(remiderUIController);
                    break;

                case "Repeat":
                    otherUIController = new OtherUI();
                    SettingsSave = newContent;
                    loadUserControll.Children.Add(otherUIController);
                    break;
            }
        }

        private void save_settings(object sender, RoutedEventArgs e)
        {
            switch (SettingsSave)
            {
                case "SettingsAlt":
                    if (systemUIController.saveAction()) App.mainScreen.succsessFunc("İşlem Başarılı", "Sistem ayarlarınız güncellendi", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    else App.mainScreen.alertFunc("İşlem Başarısız", "Sistem ayarlarınız güncellenemedi", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    break;

                case "Accessibility":
                    if (accessibilityUIController.saveAction()) App.mainScreen.succsessFunc("İşlem Başarılı", "Erişebilirlik ayarlarınız güncellendi", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    else App.mainScreen.alertFunc("İşlem Başarısız", "Erişebilirlik ayarlarınız güncellenemedi", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    break;

                case "Bell":
                    if (remiderUIController.saveAction()) App.mainScreen.succsessFunc("İşlem Başarılı", "Hatırlatıcı ayarlarınız güncellendi", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    else App.mainScreen.alertFunc("İşlem Başarısız", "Hatırlatıcı ayarların güncellenemedi", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    break;
            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch_settings(null, "SettingsAlt");

                ChangeButton ch = this.FindName("sw_setting") as ChangeButton;

                ch.chk1.IsChecked = true;
                ch.chk2.IsChecked = false;
                ch.chk3.IsChecked = false;
                ch.chk4.IsChecked = false;

                PopupHelpers.load_drag(popup_settingsPage);
                popup_settingsPage.IsOpen = true;
                // Settings açma paneli
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        // ------------ Contact ----------------- //

        private void contact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopupHelpers.load_drag(popup_ErrOpenPopup);
                popup_Contact.IsOpen = true;
                email.Text = userEmail.Text;
                firstName.Text = userName.Text + " " + userLastName.Text;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void contactfirstName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                nameerr.Content = "*Zorunlu Alan";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void contactEmail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                emailerr.Content = "*Zorunlu Alan";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void contactNote_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                detailerr.Content = "* Hangi konu hakkında konuşmak istersin ?";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private async void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (firstName.Text.Length >= 3)
                {
                    if (email.Text.Length >= 3)
                    {
                        if (IsValidEmail(email.Text))
                        {
                            if (note.Text.Length >= 3)
                            {
                                sendContact.IsEnabled = false;
                                closeContact.IsEnabled = false;
                                var messageBody = "<b>İsim Soyisim :</b> " + firstName.Text + "<br/>" + "<b>İletişim Maili :</b> " + email.Text + "<br/>" + "<b>Platform : </b> Bilgisayar" + "<br/>"+ "<b>Gönderme zamanı : </b>"+ DateTime.Now + "<br/>" + " <b>Mesaj : </b>" + "<br/> <p>" + note.Text + "</p>";
                                sendContact.IsEnabled = false;
                           
                                await Task.Run(async () => await Tools.sendMail("Kuransunettullah Contact", messageBody,"Contact"));
                                PopupHelpers.dispose_drag(popup_ErrOpenPopup);
                                popup_Contact.IsOpen = false;
                                sendContact.IsEnabled = true;
                                closeContact.IsEnabled = true;
                                note.Text = "";
                            }
                            else
                            {
                                detailerr.Content = "Minimum 3 Karakter olmak zorunda !";
                                note.Focus();
                            }
                        }
                        else
                        {
                            emailerr.Content = "Lütfen geçerli bir email adresi giriniz.";
                            email.Focus();
                        }
                    }
                    else
                    {
                        emailerr.Content = "Minimum 3 karakter olmak zorunda !";
                        email.Focus();
                    }
                }
                else
                {
                    nameerr.Content = "Minimum 3 karakter olmak zorunda !";
                    firstName.Focus();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        public void popuverMove_Click(object sender, RoutedEventArgs e)
        {
            Tools.errWrite($"[{DateTime.Now} popuverMove_Click]");
            var btn = sender as Button;
            pp_selected = (string)btn.Uid;
            moveBarController.HeaderText = btn.Content.ToString()!;
            pp_moveBar.IsOpen = true;
        }

        public Popup getPopupMove()
        {
            return pp_moveBar;
        }

        public Popup getPopupBase()
        {
            return (Popup)FindName(pp_selected);
        }

        private void appErr_Click(object sender, RoutedEventArgs e)
        {
            errCode.Text = DateTime.Now.ToString().Replace(".", "").Replace(" ", "_").Replace(":", "").Replace("/", "") + "_" + userName.Text + userLastName.Text;
            popup_ErrOpenPopup.IsOpen = true;
        }

        private async void appErrSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (errDetail.Text.Length >= 3)
                {
                    var messageBody = $"<b>[{DateTime.Now}] -> Error Code :</b> {errCode.Text} <br/>  <b> Hata Açıklaması :</b> {errDetail.Text}";
                    feedbackPopupButton.IsEnabled = false;
                    feedbackPopupClose.IsEnabled = false;
                    feedbackPopupButton.Tag ="CloudUploadMD";
                    await Task.Run(() => Tools.sendMail("Kuransunettullah Error", messageBody, "Error" , this.Dispatcher.Invoke(() => errCode.Text)));
                    PopupHelpers.dispose_drag(popup_ErrOpenPopup);
                    popup_ErrOpenPopup.IsOpen = false;
                    feedbackPopupButton.Tag = "SendiOS";
                    feedbackPopupButton.IsEnabled = true;
                    feedbackPopupClose.IsEnabled = true;
                    errDetail.Text = ""; 
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Err Send", ex);
            }
        }
    }
}