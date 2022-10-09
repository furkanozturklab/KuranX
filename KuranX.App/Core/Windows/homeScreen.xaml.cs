using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages;
using Microsoft.VisualBasic;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for homeScreen.xaml
    /// </summary>
    public partial class homeScreen : Window
    {
        private DispatcherTimer lifeCycles = new DispatcherTimer(DispatcherPriority.Render);
        private Task lifeCyclesTask, configTask;
        private Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public homeScreen()
        {
            InitializeComponent();
            App.currentVersesPageD[0] = 1;
            App.currentVersesPageD[1] = 0;
            App.mainframe = (Frame)this.FindName("homeFrame");
            App.locationTxt = (TextBlock)this.FindName("UserLocationText");
            App.locationTxt.Text = "Ayetler";
        }

        private void config_Func()
        {
            lifeCyclesTask = new Task(remiderCycler_Func);
            lifeCyclesTask.Start();
        }

        private void remiderCycler_Func()
        {
            // APP LİFECYCLER MİSSİONS

            Debug.WriteLine("APP LİFECYCLER MİSSİONS");

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
                                    entitydb.Remider.Where(p => p.RemiderId == item.RemiderId).FirstOrDefault().LastAction = DateTime.Now;
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

        private void tasksCycler_Func()
        {
            using (var entitydb = new AyetContext())
            {
                while (true)
                {
                    var TasksLoad = entitydb.Tasks.ToList();

                    foreach (var item in TasksLoad)
                    {
                        Thread.Sleep(50000); // 5 sn // 10 sn // 30 sn // 60 sn // 120 sn // 240 sn // Sırdaki göreve gecme süresi
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
                                lifeCyclerPopupBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(item.MissonsColor);
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

        private void enterFullScreenIcon_Click(object sender, RoutedEventArgs e)
        {
            leftHeaderButtonsProfile.Style = (Style)FindResource("hmwnd_leftHeaderButtonsActive");
            leftHeaderButtonsProfileIcon.Style = (Style)FindResource("IconFontHidden");
            hmwnd_headerH1.Style = (Style)FindResource("hmwnd_leftHeaderTexth1Active");
            hmwnd_headerH2.Style = (Style)FindResource("hmwnd_leftHeaderTexth2Active");
            hmwd_profileImage.Style = (Style)FindResource("hmwnd_leftHeaderImageActive");

            foreach (var tb in menuButton.Children.OfType<Button>())
            {
                tb.Style = (Style)FindResource("hmwnd_menuButtonActive");
            }

            enterFullScreenIcon.Visibility = Visibility.Collapsed;
            exitFullScreenIcon.Visibility = Visibility.Visible;
        }

        private void exitFullScreenIcon_Click(object sender, RoutedEventArgs e)
        {
            leftHeaderButtonsProfile.Style = (Style)FindResource("hmwnd_leftHeaderButtons");
            leftHeaderButtonsProfileIcon.Style = null;
            hmwnd_headerH1.Style = (Style)FindResource("hmwnd_leftHeaderTexth1");
            hmwnd_headerH2.Style = (Style)FindResource("hmwnd_leftHeaderTexth2");
            hmwd_profileImage.Style = (Style)FindResource("hmwnd_leftHeaderImageActive");

            foreach (var tb in menuButton.Children.OfType<Button>())
            {
                tb.Style = (Style)FindResource("hmwnd_menuButton");
            }

            enterFullScreenIcon.Visibility = Visibility.Visible;
            exitFullScreenIcon.Visibility = Visibility.Collapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            configTask = new Task(config_Func);
            configTask.Start();

            configTask.Wait();

            lifeCyclesTask = new Task(tasksCycler_Func);
            lifeCyclesTask.Start();

            App.locationTxt.Text = "AnaSayfa";

            //App.mainframe.Content = new Pages.VerseF.versesFrame(App.currentVersesPageD[0], App.currentVersesPageD[1], "Hepsi");

            App.mainframe.Content = new Pages.VerseF.verseFrame(1, 1, "Other");

            //App.mainframe.Content = new Pages.SubjectF.SubjectFrame();

            //App.mainframe.Content = new SubjectItemsFrame();

            //App.mainframe.Content = new Pages.SubjectF.SubjectFrame();

            //App.mainframe.Content = new Pages.SubjectF.SubjectItemFrame(13, "Fâtiha Suresinin 1 Ayeti", "Meltdown", "31.08.2022 19:54:27", (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFC107"));

            // App.mainframe.Content = new Pages.LibraryF.libraryFrame();

            // App.mainframe.Content = new Pages.LibraryF.libraryFileItemsFrame();

            //App.mainframe.Content = new Pages.LibraryF.libraryPublisherItemsFrame();

            // App.mainframe.Content = new Pages.LibraryF.libraryOpenFile();

            //App.mainframe.Content = new Pages.LibraryF.libraryNote();

            //App.mainframe.Content = new Pages.NoteF.NotesFrame();

            //App.mainframe.Content = new Pages.NoteF.NoteItem(61);

            //App.mainframe.Content = new Pages.ReminderF.RemiderFrame();
        }

        private void appClosed_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void navEnter_Click(object sender, RoutedEventArgs e)
        {
            var btn_nav = sender as Button;

            foreach (object item in menuButton.Children)
            {
            }

            switch (btn_nav.Uid)
            {
                case "nav_verses":
                    App.currentDesktype = "DeskLanding";
                    App.mainframe.Content = new Pages.VerseF.versesFrame(1, 0, "Hepsi");
                    break;

                case "nav_subject":
                    App.mainframe.Content = new Pages.SubjectF.SubjectFrame();
                    break;

                case "nav_library":
                    App.mainframe.Content = new Pages.LibraryF.libraryFrame();
                    break;

                case "nav_notes":
                    App.mainframe.Content = new Pages.NoteF.NotesFrame();
                    break;

                case "nav_reminder":
                    App.mainframe.Content = new Pages.ReminderF.RemiderFrame();
                    break;

                default:
                    App.mainframe.Content = new TestFrame();

                    break;
            }
        }

        private void adminOpen_Click(object sender, RoutedEventArgs e)
        {
            AdminScreen adm = new AdminScreen();
            adm.Show();
        }

        private void popupAction_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            using (var entitydb = new AyetContext())
            {
                var dRemider = entitydb.Remider.Where(p => p.RemiderId == int.Parse(btn.Uid)).FirstOrDefault();
                //if (dRemider != null && dRemider.ConnectSureId != 0) App.mainframe.Content = new Pages.VerseF.verseFrame(dRemider.ConnectSureId, dRemider.ConnectVerseId, "Remider");
            }
        }

        private void popupActionDouble_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            using (var entitydb = new AyetContext())
            {
                var dRemider = entitydb.Remider.Where(p => p.RemiderId == int.Parse(btn.Uid)).FirstOrDefault();
                if (dRemider != null && dRemider.ConnectSureId != 0) App.mainframe.Content = new Pages.VerseF.verseFrame(dRemider.ConnectSureId, dRemider.ConnectVerseId, "Remider");
            }
        }
    }
}