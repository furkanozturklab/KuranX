﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Window
    {
        private Task lifeCyclesTask, configTask, menuLTask;
        private CheckBox? navCheckBox;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public homeScreen()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();

            App.mainframe = (Frame)this.FindName("homeFrame");
        }

        /* -------------------------- LOADED FUNC  --------------------------  */

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadAni();
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var user = entitydb.Users.FirstOrDefault();

                    // hmwd_profileName.Text = user.FirstName + " " + user.LastName;

                    // hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(user.AvatarUrl);
                }

                welcoming();
                navigationWriter("verse", "");

                configTask = new Task(config_Func);
                configTask.Start();

                configTask.Wait();

                lifeCyclesTask = new Task(tasksCycler_Func);
                lifeCyclesTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadedFunc", ex);
            }

            loadAniComplated();

            App.mainframe.Content = App.navSurePage;

            // App.mainframe.Content = App.navVersePage.PageCall(1, 1, "Other");

            //App.mainframe.Content = App.navNotesPage;

            // App.mainframe.Content = App.navSubjectPage.subjectPageCall();

            // App.mainframe.Content = App.navSubjectFolder.subjectFolderPageCall(1);

            // App.mainframe.Content = App.navSubjectItem.subjectItemsPageCall(1, 1, 1);

            // App.mainframe.Content = App.navResultPage;
        }

        private void homeFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        /* -------------------------- LOADED FUNC  --------------------------  */

        /* -------------------------- SPECİAL FUNC  --------------------------  */

        private void config_Func()
        {
            try
            {
                lifeCyclesTask = new Task(remiderCycler_Func);
                lifeCyclesTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("SpecialFunc", ex);
            }
        }

        private void remiderCycler_Func()
        {
            // APP LİFECYCLER MİSSİONS
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dRemider = entitydb.Remider.Where(p => p.LastAction < DateTime.Now && p.Status == "Run").ToList();

                    foreach (var item in dRemider)
                    {
                        switch (item.LoopType)
                        {
                            case "False":
                                if (item.RemiderDate.ToString("d") == DateTime.Now.ToString("d") && item.LastAction.ToString("d") != DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.MissonsId == item.RemiderId && p.MissonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.RemiderId == item.RemiderId)
                                                        .FirstOrDefault().LastAction = DateTime.Now;

                                        var newD = new Tasks { MissonsId = item.RemiderId, MissonsColor = "#ffc107", MissonsRepeart = 5, MissonsTime = 5, MissonsType = "Remider" };
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Gün":

                                if (item.LastAction.AddDays(1).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.MissonsId == item.RemiderId && p.MissonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.RemiderId == item.RemiderId).FirstOrDefault().LastAction = DateTime.Now;

                                        var newD = new Tasks { MissonsId = item.RemiderId, MissonsColor = "#d63384", MissonsRepeart = 5, MissonsTime = 5, MissonsType = "Remider" };
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Hafta":

                                if (item.LastAction.AddDays(7).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.MissonsId == item.RemiderId && p.MissonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.RemiderId == item.RemiderId).FirstOrDefault().LastAction = DateTime.Now;

                                        var newD = new Tasks { MissonsId = item.RemiderId, MissonsColor = "#0d6efd", MissonsRepeart = 5, MissonsTime = 5, MissonsType = "Remider" };
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Ay":
                                if (item.LastAction.AddMonths(1).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.MissonsId == item.RemiderId && p.MissonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.RemiderId == item.RemiderId).FirstOrDefault().LastAction = DateTime.Now;

                                        var newD = new Tasks { MissonsId = item.RemiderId, MissonsColor = "#0dcaf0", MissonsRepeart = 5, MissonsTime = 5, MissonsType = "Remider" };
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;

                            case "Yıl":
                                if (item.LastAction.AddYears(1).ToString("d") == DateTime.Now.ToString("d"))
                                {
                                    if (entitydb.Tasks.Where(p => p.MissonsId == item.RemiderId && p.MissonsType == "Remider").Count() == 0)
                                    {
                                        entitydb.Remider.Where(p => p.RemiderId == item.RemiderId).FirstOrDefault().LastAction = DateTime.Now;

                                        var newD = new Tasks { MissonsId = item.RemiderId, MissonsColor = "#6610f2", MissonsRepeart = 5, MissonsTime = 5, MissonsType = "Remider" };
                                        entitydb.Tasks.Add(newD);
                                    }
                                }
                                break;
                        }

                        if (item.RemiderDate != DateTime.Parse("0001-01-01 00:00:00") && item.RemiderDate < DateTime.Now)
                        {
                            entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.RemiderId == item.RemiderId));

                            entitydb.Verse.Where(p => p.RelativeDesk == item.ConnectVerseId && p.SureId == item.ConnectSureId).FirstOrDefault().RemiderCheck = "false";
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
                using (var entitydb = new AyetContext())
                {
                    while (true)
                    {
                        var TasksLoad = entitydb.Tasks.ToList();

                        foreach (var item in TasksLoad)
                        {
                            Thread.Sleep(5000); // 5 sn // 10 sn // 30 sn // 60 sn // 120 sn // 240 sn // Sırdaki göreve gecme süresi
                            var dRemider = entitydb.Remider.Where(p => p.RemiderId == item.MissonsId).FirstOrDefault();

                            if (dRemider != null)
                            {
                                entitydb.Tasks.Where(p => p.MissonsId == dRemider.RemiderId).FirstOrDefault().MissonsRepeart--;

                                if (entitydb.Tasks.Where(p => p.MissonsId == dRemider.RemiderId).FirstOrDefault().MissonsRepeart == 0) entitydb.Tasks.RemoveRange(entitydb.Tasks.Where(p => p.TasksId == item.TasksId));

                                entitydb.SaveChanges();

                                this.Dispatcher.Invoke(() =>
                                {
                                    lifeCyclerPopups.IsOpen = true;
                                    lifeCyclerPopupsText.Text = dRemider.RemiderName;
                                    lifePopupGoActionButton.Uid = dRemider.RemiderId.ToString();
                                    lifePopupGoActionButton.Tag = "Remider";
                                    if (dRemider.ConnectSureId == 0) lifePopupGoDobuleActionButton.Visibility = Visibility.Collapsed;
                                    else
                                    {
                                        lifePopupGoDobuleActionButton.Visibility = Visibility.Visible;
                                        lifeCyclerPopupsTxtStck.Width = 230;
                                        lifeCyclerPopupsActStck.Width = 80;
                                        lifePopupGoDobuleActionButton.Uid = dRemider.RemiderId.ToString();
                                    }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                    lifeCyclerPopupBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(item.MissonsColor);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                                });

                                Thread.Sleep(10000); // 5 sn // 10 sn // 30 sn // 60 sn // 120 sn // 240 sn // Ekranda bekleme süresi

                                this.Dispatcher.Invoke(() =>
                                {
                                    lifeCyclerPopups.IsOpen = false;
                                    lifeCyclerPopupsText.Text = "";
                                    lifePopupGoActionButton.Uid = "";
                                    lifePopupGoActionButton.Tag = "";
                                });
                            }
                        }
                        if (TasksLoad.Count == 0) break;
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

                if (i >= 0 && i <= 7) hmwnd_headerH2.Text = "İyi Geceler";
                if (i > 7 && i <= 11) hmwnd_headerH2.Text = "İyi Sabahlar";
                if (i > 11 && i <= 18) hmwnd_headerH2.Text = "İyi Günler";
                if (i > 18 && i <= 24) hmwnd_headerH2.Text = "İyi Akşamlar";
            }
            catch (Exception ex)
            {
                App.logWriter("SpecialFunc", ex);
            }
        }

        private void navigationWriter(string baseicon, string location)
        {
            try
            {
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
                var locations = location.Split(",");

                foreach (var item in locations)
                {
                    var sp_btn = new Button();
                    var lc_txb = new TextBlock();
                    sp_btn.Style = (Style)FindResource("navigationSperator");
                    lc_txb.Style = (Style)FindResource("navigationLocationText");

                    lc_txb.Text = item.ToString();

                    NavigationStack.Children.Add(sp_btn);
                    NavigationStack.Children.Add(lc_txb);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("SpecialFunc", ex);
            }
        }

        /* -------------------------- SPECİAL FUNC  --------------------------  */

        /* -------------------------- CLİCK FUNC  --------------------------  */

        private void appClosed_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuTask(CheckBox chk)
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach (object item in hmwnd_leftNavControlStack.Children)
                {
                    CheckBox? element = (item as FrameworkElement) as CheckBox;
                    element.IsChecked = false;
                }

                if (App.mainframe.Content.ToString().Split('.').Last() == "verseFrame")
                {
                    // eğer verseFrame de isem çalış

                    if (App.navVersePage.markButton.IsChecked == true)
                    {
                        // Verseframede işaretlemiş isem çalış
                        switch (navCheckBox.Content)
                        {
                            case "Ayetler":
                                App.currentDesktype = "DeskLanding";

                                App.mainframe.Content = App.navSurePage;

                                break;

                            case "Konularım":

                                App.mainframe.Content = App.navSubjectPage;

                                break;

                            case "Kütüphane":

                                App.mainframe.Content = App.navLibraryOpen;

                                break;

                            case "Notlar":

                                App.mainframe.Content = App.navNotesPage;

                                break;

                            case "Hatırlatıcı":

                                App.mainframe.Content = App.navRemiderPage;

                                break;

                            case "Sonuc":

                                App.mainframe.Content = App.navResultPage.navResultPageCall();

                                break;

                            default:

                                App.mainframe.Content = App.navTestPage;

                                break;
                        }
                    }
                    else popup_fastExitConfirm.IsOpen = true;
                    // Eğer işaretlenmemiş ise popup aç
                }
                else
                {
                    // eğer verseFramede değilsem normal çalış
                    navCheckBox.IsChecked = true;

                    switch (navCheckBox.Content)
                    {
                        case "Ayetler":
                            App.currentDesktype = "DeskLanding";

                            App.mainframe.Content = App.navSurePage;

                            break;

                        case "Konularım":

                            App.mainframe.Content = App.navSubjectPage;

                            break;

                        case "Kütüphane":

                            App.mainframe.Content = App.navLibraryOpen;

                            break;

                        case "Notlar":

                            App.mainframe.Content = App.navNotesPage;

                            break;

                        case "Hatırlatıcı":

                            App.mainframe.Content = App.navRemiderPage;

                            break;

                        case "Sonuc":

                            App.mainframe.Content = App.navResultPage.navResultPageCall(); ;

                            break;

                        default:

                            App.mainframe.Content = App.navTestPage;

                            break;
                    }
                    Thread.Sleep(500);
                }
            });

            GC.Collect();
        }

        private void menuNavControl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                navCheckBox = sender as CheckBox;
                menuLTask = Task.Run(() => menuTask(navCheckBox));
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void adminOpen_Click(object sender, RoutedEventArgs e)
        {
            AdminScreen adm = new AdminScreen();
            adm.Show();
        }

        private void popupAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;

                using (var entitydb = new AyetContext())
                {
                    var dRemider = entitydb.Remider.Where(p => p.RemiderId == int.Parse(btn.Uid)).FirstOrDefault();

                    //if (dRemider != null && dRemider.ConnectSureId != 0) App.mainframe.Content = new Pages.ReminderF.RemiderItem(dRemider.RemiderId);

                    lifeCyclerPopups.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
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
                    var dRemider = entitydb.Remider.Where(p => p.RemiderId == int.Parse(btn.Uid)).FirstOrDefault();

                    if (dRemider != null && dRemider.ConnectSureId != 0) App.mainframe.Content = new Pages.VerseF.verseFrame(dRemider.ConnectSureId, dRemider.ConnectVerseId, "Remider");

                    lifeCyclerPopups.IsOpen = false;
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

                    App.mainframe.Content = App.navSurePage;

                    break;

                case "Konularım":

                    App.mainframe.Content = App.navSubjectPage;

                    break;

                case "Kütüphane":

                    App.mainframe.Content = App.navLibraryOpen;

                    break;

                case "Notlar":

                    App.mainframe.Content = App.navNotesPage;

                    break;

                case "Hatırlatıcı":

                    App.mainframe.Content = App.navRemiderPage;

                    break;

                case "Sonuc":

                    App.mainframe.Content = App.navResultPage.navResultPageCall(); ;

                    break;

                default:

                    App.mainframe.Content = App.navTestPage;

                    break;
            }

            GC.Collect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

        private void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadinGifContent.Visibility = Visibility.Visible;
                MainFrameGrid.Visibility = Visibility.Hidden;
                hmwnd_leftNavControlStack.IsEnabled = false;
            });
        }

        private void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                MainFrameGrid.Visibility = Visibility.Visible;
                loadinGifContent.Visibility = Visibility.Collapsed;
                hmwnd_leftNavControlStack.IsEnabled = true;
            });
        }

        /* -------------------------- CLİCK FUNC  --------------------------  */
    }
}