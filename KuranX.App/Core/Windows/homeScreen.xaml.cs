using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using KuranX.App.Core.Classes;

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
            try
            {
                InitializeComponent();
                App.mainframe = (Frame)this.FindName("homeFrame");
                App.secondFrame = (Frame)this.FindName("secondFrame");
                AppVersion.Text = "Version " + App.config.AppSettings.Settings["application_version"].Value;
                settingsVersion.Content = "build" + App.config.AppSettings.Settings["application_type"].Value + "_" + App.config.AppSettings.Settings["application_platform"].Value + "_" + App.config.AppSettings.Settings["application_version"].Value;

            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
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


                App.errWrite($"[Session Stard - {DateTime.Now}]");

                if (taskstatus) App.mainTask = Task.Run(() => tasksCycler_Func());
                App.mainframe.Content = App.navHomeFrame.PageCall();

                homescreengrid.IsEnabled = true;

            }
            catch (Exception ex)
            {
                App.logWriter("Loaded", ex);
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

                            Debug.WriteLine(item.missonsId);

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
                App.logWriter("SpecialFunc", ex);
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
                    else this.Dispatcher.Invoke(() => popup_fastExitConfirm.IsOpen = true);
                    // Eğer işaretlenmemiş ise popup aç
                }
                else
                {
                    // eğer verseFramede değilsem normal çalış

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
                App.logWriter("ClickFunc", ex);
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
                App.logWriter("Loaded", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.errWrite($"[{DateTime.Now} Menu Click] -> {navCheckBox.Content}");


            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
            }
        }

        private void fastexitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_fastExitConfirm.IsOpen = false;

                Debug.WriteLine("Burdan Cıktı ?");

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
                pp_moveBar.IsOpen = false;
                errDetail.Text = "";
                homescreengrid.IsEnabled = true;
                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void leftHeaderButtonsProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    App.loadTask = Task.Run(() => loadProfile());
                    popup_profileMain.IsOpen = true;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Message", ex);
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
                App.logWriter("Message", ex);
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
                App.logWriter("Other", ex);
            }
        }

        // ---------------------  PROFİLE POPUP FUNC --------------------- //

        private void loadProfile()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} LoadProfile] -> Profil Load");
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
            }
        }

        private void saveProfileFunc()
        {
            try
            {

                App.errWrite($"[{DateTime.Now} SaveProfile] -> Profil saved");
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
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
                        popup_editImage.IsOpen = false;
                    });
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
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
                App.logWriter("Other", ex);
            }
        }

        private void prEditImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_editImage.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // -------- settings ------------ //

        private void versionText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                updateNoteStack.Children.Clear();
                this.Dispatcher.Invoke(() => popup_updateNotes.IsOpen = true);

                string[] split = App.returnPostNotes.Data[0].project_UpdateNote.Split("½");

                if (split.Length > 0)
                {
                    foreach (var item in split)
                    {
                        var txt = new TextBlock();
                        txt.Style = (Style)FindResource("updateText");
                        txt.Text = "-" + item;
                        updateNoteStack.Children.Add(txt);
                    }
                }
                else
                {
                    var txt = new TextBlock();
                    txt.Style = (Style)FindResource("updateText");
                    txt.Text = "-" + App.returnPostNotes.Data[0].project_UpdateNote;
                    updateNoteStack.Children.Add(txt);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void openExe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() => popup_updateNotes.IsOpen = false);
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void deleteData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_settingsPage.IsOpen = false;
                using (var entitydb = new AyetContext())
                {
                    if (MessageBox.Show("Lütfen dikkat bu işlem geri alınamaz ve tüm verileriniz , bağlantılarınız ve ilerlemeniz silinecektir.", "Verileri Sıfırla", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                    {
                        resetData(entitydb);
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Tüm verileriniz başarılı bir sekilde silinmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void deleteProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_settingsPage.IsOpen = false;
                using (var entitydb = new AyetContext())
                {
                    if (MessageBox.Show("Lütfen dikkat bu işlem geri alınamaz.Tüm verileriniz silinecek ve yeniden giriş yapmanız gerekicektir.", "Hesabı Sıfırla", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                    {
                        resetData(entitydb);
                        entitydb.Users.RemoveRange(entitydb.Users.First());
                        App.config.AppSettings.Settings["user_pin"].Value = "";
                        App.config.AppSettings.Settings["user_rememberMe"].Value = "false";
                        App.config.AppSettings.Settings["user_autoLogin"].Value = "false";
                        App.config.Save(ConfigurationSaveMode.Modified);
                        entitydb.SaveChanges();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void resetData(AyetContext db)
        {
            try
            {
                foreach (var item in db.Notes) db.Notes.Remove(item);

                foreach (var item in db.Subject) db.Subject.Remove(item);

                foreach (var item in db.Subject) db.Subject.Remove(item);



                foreach (var item in db.Remider) db.Remider.Remove(item);

                foreach (var item in db.Tasks) db.Tasks.Remove(item);

                foreach (var item in db.ResultItems) db.ResultItems.Remove(item);

                foreach (var item in db.Integrity.Where(p => p.integrityProtected == false)) db.Integrity.Remove(item);

                foreach (var item in db.Results)
                {
                    db.Results.Where(p => p.resultId == item.resultId).First().resultNotes = false;
                    db.Results.Where(p => p.resultId == item.resultId).First().resultSubject = false;

                    db.Results.Where(p => p.resultId == item.resultId).First().resultFinallyNote = "Sonuç Metninizi buraya yaza bilirsiniz.";
                }

                foreach (var item in db.Sure)
                {
                    db.Sure.Where(p => p.sureId == item.sureId).First().userCheckCount = 0;
                    db.Sure.Where(p => p.sureId == item.sureId).First().userLastRelativeVerse = 0;
                    db.Sure.Where(p => p.sureId == item.sureId).First().complated = false;
                    db.Sure.Where(p => p.sureId == item.sureId).First().status = "#ADB5BD";
                }

                foreach (var item in db.Verse)
                {
                    db.Verse.Where(p => p.verseId == item.verseId).First().markCheck = false;
                    db.Verse.Where(p => p.verseId == item.verseId).First().remiderCheck = false;
                    db.Verse.Where(p => p.verseId == item.verseId).First().verseCheck = false;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                st_aniSecond.Text = App.config.AppSettings.Settings["app_animationSpeed"].Value;
                st_warningSecond.Text = App.config.AppSettings.Settings["app_warningShowTime"].Value;
                st_remiderCount.Text = App.config.AppSettings.Settings["app_remiderCount"].Value;
                st_remiderRepeartTime.Text = App.config.AppSettings.Settings["app_remiderWaitTime"].Value;
                st_remiderTime.Text = App.config.AppSettings.Settings["app_remiderTime"].Value;

                switch (App.config.AppSettings.Settings["user_autoLogin"].Value)
                {
                    case "false":
                        st_start.SelectedIndex = 0;
                        break;

                    case "true":
                        st_start.SelectedIndex = 1;
                        break;
                }
                popup_settingsPage.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void settingSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.Parse(st_aniSecond.Text) > 0 && int.Parse(st_aniSecond.Text) <= 10000 && IsNumeric(st_aniSecond.Text))
                {
                    if (int.Parse(st_warningSecond.Text) > 0 && int.Parse(st_warningSecond.Text) <= 30 && IsNumeric(st_warningSecond.Text))
                    {
                        if (int.Parse(st_remiderTime.Text) > 0 && int.Parse(st_remiderTime.Text) <= 240 && IsNumeric(st_remiderTime.Text))
                        {
                            if (int.Parse(st_remiderRepeartTime.Text) > 0 && int.Parse(st_remiderRepeartTime.Text) <= 3600 && IsNumeric(st_remiderRepeartTime.Text))
                            {
                                if (int.Parse(st_remiderCount.Text) > 0 && int.Parse(st_remiderCount.Text) <= 30 && IsNumeric(st_remiderCount.Text))
                                {
                                    var item = st_start.SelectedItem as ComboBoxItem;
                                    if (item != null)
                                    {
                                        switch (item.Tag)
                                        {
                                            case "false":
                                                App.config.AppSettings.Settings["user_autoLogin"].Value = "false";
                                                break;

                                            case "true":
                                                App.config.AppSettings.Settings["user_autoLogin"].Value = "true";
                                                break;
                                        }

                                        App.config.AppSettings.Settings["app_animationSpeed"].Value = st_aniSecond.Text;
                                        App.config.AppSettings.Settings["app_remiderTime"].Value = st_remiderTime.Text;
                                        App.config.AppSettings.Settings["app_remiderWaitTime"].Value = st_remiderRepeartTime.Text;
                                        App.config.AppSettings.Settings["app_remiderCount"].Value = st_remiderCount.Text;
                                        App.config.AppSettings.Settings["app_warningShowTime"].Value = st_warningSecond.Text;
                                        App.config.Save(ConfigurationSaveMode.Modified);
                                        succsessFunc("İşlem Başarılı", "Ayarlarınız başarılı bir sekilde güncellenemiştir.", int.Parse((App.config.AppSettings.Settings["app_warningShowTime"].Value)));
                                        popup_settingsPage.IsOpen = false;
                                    }
                                }
                                else
                                {
                                    if (int.Parse(st_remiderCount.Text) > 3600) st_remiderCountErr.Content = "Maksimum tekrarlama sayısı aşıldı Max:30";
                                    st_remiderCount.Focus();
                                }
                            }
                            else
                            {
                                if (int.Parse(st_remiderRepeartTime.Text) > 3600) st_remiderRepeartTimeErr.Content = "3600 sn den uzun değerler kabul edilmez.";
                                st_remiderRepeartTime.Focus();
                            }
                        }
                        else
                        {
                            if (int.Parse(st_remiderTime.Text) > 240) st_remiderTimeErr.Content = "240 sn den uzun değerler kabul edilmez.";
                            st_remiderTime.Focus();
                        }
                    }
                    else
                    {
                        if (int.Parse(st_warningSecond.Text) > 30) st_warningSecondErr.Content = "30 sn den uzun değerler kabul edilmez.";
                        st_warningSecond.Focus();
                    }
                }
                else
                {
                    if (!IsNumeric(st_aniSecond.Text)) st_aniSecondErr.Content = "Lütfen sayısal bir değer giriniz.";
                    else st_aniSecondErr.Content = "Lütfen 0 dan büyük bir değer giriniz.";

                    if (int.Parse(st_aniSecond.Text) > 10000) st_aniSecondErr.Content = "Maksimum üst sınırı geçtiniz Max:10000";
                    st_aniSecond.Focus();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void st_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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

        public bool IsNumeric(string value)
        {
            try
            {
                if (int.TryParse(value, out int numericValue)) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
                App.logWriter("Other", ex);
            }
        }

        // ------------ Contact ----------------- //

        private void contact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_Contact.IsOpen = true;
                email.Text = userEmail.Text;
                firstName.Text = userName.Text + " " + userLastName.Text;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
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
                                var messageBody = "<b>İsim Soyisim :</b> " + firstName.Text + "<br/>" + "<b>İletişim Maili :</b> " + email.Text + "<br/>" + "<b>Platform : </b> Bilgisayar" + "<br/>" + " <b>Mesaj : </b>" + "<br/> <p>" + note.Text + "</p>";
                                sendContact.IsEnabled = false;
                                var returnBool = false;
                                App.loadTask = Task.Run(() => returnBool = App.sendMail("Kuransunettüllah Destek", messageBody));
                                await App.loadTask;

                                if (returnBool)
                                {
                                    popup_Contact.IsOpen = false;
                                    sendContact.IsEnabled = true;
                                    closeContact.IsEnabled = true;
                                    note.Text = "";
                                }
                                else
                                {
                                    sendContact.IsEnabled = true;
                                    closeContact.IsEnabled = true;
                                }
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
                App.logWriter("Click", ex);
            }
        }

        public void popuverMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                ppMoveConfing((string)btn.Uid);
                moveControlName.Text = (string)btn.Content;
                pp_moveBar.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        public void ppMoveActionOfset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var movePP = (Popup)FindName((string)btntemp.Content);

                switch (btntemp.Uid.ToString())
                {
                    case "Left":
                        movePP.HorizontalOffset -= 50;
                        break;

                    case "Top":
                        movePP.VerticalOffset -= 50;
                        break;

                    case "Bottom":
                        movePP.VerticalOffset += 50;
                        break;

                    case "Right":
                        movePP.HorizontalOffset += 50;
                        break;

                    case "UpLeft":
                        movePP.Placement = PlacementMode.Absolute;
                        movePP.VerticalOffset = 0;
                        movePP.HorizontalOffset = 0;
                        break;

                    case "Reset":
                        movePP.Placement = PlacementMode.Center;
                        movePP.VerticalOffset = 0;
                        movePP.HorizontalOffset = 0;
                        movePP.Child.Opacity = 1;
                        movePP.Child.IsEnabled = true;
                        break;

                    case "Close":
                        pp_moveBar.IsOpen = false;
                        movePP.Child.Opacity = 1;
                        movePP.Child.IsEnabled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        public void ppMoveActionOpacity_Click(object sender, RoutedEventArgs e)
        {
            var btntemp = sender as Button;
            var movePP = (Popup)FindName((string)btntemp.Content);

            switch (btntemp.Uid.ToString())
            {
                case "Up":
                    movePP.Child.Opacity = 1;
                    movePP.Child.IsEnabled = true;
                    break;

                case "Down":
                    movePP.Child.Opacity = 0.1;
                    movePP.Child.IsEnabled = false;
                    break;
            }
        }

        public void ppMoveConfing(string ppmove)
        {
            try
            {
                for (int i = 1; i < 10; i++)
                {
                    var btn = FindName("pp_M" + i) as Button;
                    btn.Content = ppmove;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
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



                    var postingdata = new Dictionary<string, string>
                    {
                            { App.config.AppSettings.Settings["api_tokenName"].Value, App.config.AppSettings.Settings["api_token"].Value},
                            { "post_projectid", App.config.AppSettings.Settings["application_id"].Value },
                            { "post_action", "POST" },
                            { "post_type", "AddErr" },
                            { "post_fileName",errCode.Text },
                            { "post_projectId", "3" },
                            { "post_date", DateTime.Now.ToString() },
                            { "post_errDetail", errDetail.Text },
                            { "post_code",   errCode.Text },
                            { "post_location",   string.Format("{0}/{1}", "ftp://furkanozturklab.com/projects/kuranx/err", "kuranx_" + errCode.Text + ".txt") }

                    };

                    App.loadTask = Task.Run(() => App.apiPostRun(postingdata, "AddError"));
                    await App.loadTask;



                    var messageBody = $"[{DateTime.Now}] -> Error Code : {errCode.Text} <br/> Hata Açıklaması : {errDetail.Text}";
                    App.loadTask = Task.Run(() => App.sendMailErr("Kuransunettüllah Destek", messageBody));

                    popup_ErrOpenPopup.IsOpen = false;

                    errDetail.Text = "";
                }
            }
            catch (Exception ex)
            {

                App.logWriter("FTP", ex);
            }


        }


        private void ftp_write()
        {




            NetworkCredential credentials = new NetworkCredential("ftpUpdate@furkanozturklab.com", "(ATb%#}QpK14");

            FtpWebRequest ftbrequest = (FtpWebRequest)WebRequest.Create(new Uri(string.Format("{0}/{1}", "ftp://furkanozturklab.com/projects/kuranx/err", "kuranx_" + errCode.Text + ".txt")));
            ftbrequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftbrequest.Credentials = credentials;
            Stream ftbStream = ftbrequest.GetRequestStream();


            FileStream fs = File.OpenRead(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\KuranSunnetullah\err.txt");

            byte[] buffer = new byte[4096];
            double total = (double)fs.Length;
            double read = 0;
            int byteRead = 0;


            do
            {
                byteRead = fs.Read(buffer, 0, 1024);
                ftbStream.Write(buffer, 0, byteRead);
                read += (double)byteRead;
                double prec = read / total * 100;
                if (total == 0) prec = 100;

                // 0 size crash

            }
            while (byteRead != 0);


            fs.Close();
            ftbStream.Close();


        }

    }
}