using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using KuranX.App.Core.Classes;

namespace KuranX.App.Core.Pages
{
    /// <summary>
    /// Interaction logic for ayetlerFrame.xaml
    /// </summary>
    public partial class versesFrame : Page
    {
        private List<Sure> verses = new List<Sure>();
        private int NowPage, CurrentSkip;
        private Task PageItemLoadTask;
        private string Landing, DeskType, VersesStatus;
        private List<ItemsControl> itemsList = new List<ItemsControl>();
        private bool tempCheck = false, tempCheck1 = false;

        public versesFrame()
        {
            InitializeComponent();
            nextpageButton.IsEnabled = false;
            previusPageButton.IsEnabled = false;
            NowPage = 1;
            CurrentSkip = 0;
            VersesStatus = "All";
            itemsList.Add(aIcontrol1);
            itemsList.Add(aIcontrol2);
            itemsList.Add(aIcontrol3);
            itemsList.Add(aIcontrol4);
            itemsList.Add(aIcontrol5);
            itemsList.Add(aIcontrol6);
            itemsList.Add(aIcontrol7);
            itemsList.Add(aIcontrol8);
            itemsList.Add(aIcontrol9);
            itemsList.Add(aIcontrol10);
            itemsList.Add(aIcontrol11);
            itemsList.Add(aIcontrol12);
            itemsList.Add(aIcontrol13);
            itemsList.Add(aIcontrol14);
            itemsList.Add(aIcontrol15);
        }

        private void loadVerses()
        {
            using (var entitydb = new AyetContext())
            {
                List<Sure> VersesList;
                decimal totalCount;

                int i = 0;

                for (int x = 0; x < 15; x++)
                {
                    itemsList[x].Dispatcher.Invoke(() =>
                    {
                        itemsList[x].ItemsSource = null;
                        itemsList[x].Items.Clear();
                    });
                }

                if (VersesStatus == "All")
                {
                    // TÜMÜ

                    totalCount = decimal.Parse(entitydb.Sure.ToList().Count().ToString());

                    if (Landing != "Hepsi")
                    {
                        if (DeskType == "İnişe Göre")
                        {
                            Debug.WriteLine("bende ");
                            VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).OrderBy(p => p.DeskLanding).Skip(CurrentSkip).Take(15).ToList();
                        }
                        else
                        {
                            VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == Landing).OrderBy(p => p.DeskMushaf).Skip(CurrentSkip).Take(15).ToList();
                        }
                    }
                    else
                    {
                        if (DeskType == "İnişe Göre")
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
                    totalCount = decimal.Parse(entitydb.Sure.Where(p => p.Status == "#66E21F").ToList().Count().ToString());

                    if (Landing != "Hepsi")
                    {
                        if (DeskType == "İnişe Göre")
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
                        if (DeskType == "İnişe Göre")
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
                    totalCount = decimal.Parse(entitydb.Sure.Where(p => p.Status == "#ADB5BD").ToList().Count().ToString());
                    if (Landing != "Hepsi")
                    {
                        if (DeskType == "İnişe Göre")
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
                        if (DeskType == "İnişe Göre")
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
                    foreach (var item in VersesList)
                    {
                        verses.Add(item);
                        itemsList[i].Dispatcher.Invoke(() =>
                        {
                            itemsList[i].ItemsSource = verses;
                        });
                        verses.Clear();
                        i++;
                    }

                    totalCount = Math.Ceiling(totalCount / 15);

                    nowPageStatus.Dispatcher.Invoke(() =>
                    {
                        nowPageStatus.Tag = NowPage + " / " + totalCount;
                    });

                    nextpageButton.Dispatcher.Invoke(() =>
                    {
                        if (NowPage != totalCount) nextpageButton.IsEnabled = true;
                    });
                    previusPageButton.Dispatcher.Invoke(() =>
                    {
                        if (NowPage != 1) previusPageButton.IsEnabled = true;
                    });

                    allCheck.Dispatcher.Invoke(() =>
                    {
                        allCheck.IsEnabled = true;
                    });
                    readCheck.Dispatcher.Invoke(() =>
                    {
                        readCheck.IsEnabled = true;
                    });
                    noraedCheck.Dispatcher.Invoke(() =>
                    {
                        noraedCheck.IsEnabled = true;
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

                loadinGifContent.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Collapsed;
                });
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageItemLoadTask = new Task(loadVerses);
            PageItemLoadTask.Start();
        }

        private void deskingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeskType = cmbselectedvalue(deskingCombobox.SelectedValue.ToString());
            if (tempCheck)
            {
                ChangeStatusPage();
                NowPage = 1;
                CurrentSkip = 0;
                PageItemLoadTask = new Task(loadVerses);
                PageItemLoadTask.Start();
            }
            else tempCheck = true;
        }

        private void checkstatustype_click(object sender, RoutedEventArgs e)
        {
            allCheck.IsChecked = false;
            readCheck.IsChecked = false;
            noraedCheck.IsChecked = false;
            RadioButton checkedval = sender as RadioButton;
            checkedval.IsChecked = true;
            NowPage = 1;
            CurrentSkip = 0;
            ChangeStatusPage();
            VersesStatus = checkedval.Tag.ToString();
            PageItemLoadTask = new Task(loadVerses);
            PageItemLoadTask.Start();
        }

        private void landingCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Landing = cmbselectedvalue(landingCombobox.SelectedValue.ToString());
            if (tempCheck1)
            {
                ChangeStatusPage();
                NowPage = 1;
                CurrentSkip = 0;
                PageItemLoadTask = new Task(loadVerses);
                PageItemLoadTask.Start();
            }
            else tempCheck1 = true;
        }

        private string cmbselectedvalue(string data)
        {
            string p = data;
            string[] parse = p.Split('.');
            p = parse[3];
            parse = p.Split(':');
            parse[0] = parse[1].Trim(' ');

            return parse[0];
        }

        private void ChangeStatusPage()
        {
            nextpageButton.IsEnabled = false;
            previusPageButton.IsEnabled = false;
            allCheck.IsEnabled = false;
            readCheck.IsEnabled = false;
            noraedCheck.IsEnabled = false;
            PageItemLoadTask.Dispose();
            loadinGifContent.Visibility = Visibility.Visible;
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            ChangeStatusPage();
            NowPage++;
            CurrentSkip += 15;
            PageItemLoadTask = new Task(loadVerses);
            PageItemLoadTask.Start();
        }

        private void BeforePage(object sender, RoutedEventArgs e)
        {
            ChangeStatusPage();
            NowPage--;
            CurrentSkip -= 15;
            PageItemLoadTask = new Task(loadVerses);
            PageItemLoadTask.Start();
        }

        private void versesPageOpen(object sender, RoutedEventArgs e)
        {
            Button getContent = sender as Button;

            App.mainframe.Content = new verseFrame(Int16.Parse(getContent.Tag.ToString()));

            /*
            verseFrame verseFrame = new verseFrame(int.Parse(getContent.Tag.ToString()));
            NavigationService.Navigate(verseFrame, UriKind.Relative);
            */
        }
    }
}