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
        private bool tempCheck = false, tempCheck1 = false;

        public versesFrame()
        {
            InitializeComponent();
            nextpageButton.IsEnabled = false;
            previusPageButton.IsEnabled = false;
            NowPage = 1;
            CurrentSkip = 0;
            VersesStatus = "All";
        }

        private void loadVerses()
        {
            using (var entitydb = new AyetContext())
            {
                List<Sure> VersesList;
                decimal totalCount;

                // ItemsSourceClear
                for (int x = 1; x < 16; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("aIcontrol" + x);
                        //itemslist.Items.Clear();
                        itemslist.ItemsSource = null;
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
                    // OKUDUKLARIM

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
                    // Okumadıklarım
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
                    int x = 1;
                    foreach (var item in VersesList)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            verses.Add(item);
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