using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
using Org.BouncyCastle.Asn1.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuranX.App.Core.Pages.ResultF
{
    /// <summary>
    /// Interaction logic for ResultItem.xaml
    /// </summary>
    public partial class ResultItem : Page
    {
        public class Dstack
        {
            public int ItemId { get; set; }
            public int BaseId { get; set; }
            public string ItemName { get; set; }
            public string ItemDateTime { get; set; }
            public string ItemColor { get; set; }
        }

        private int lastPage = 0, NowPage = 1, selectedId;
        private bool filter = false;
        private string filterTxt;

        public ResultItem()
        {
            InitializeComponent();
        }

        public Page PageCall(int id)
        {
            lastPage = 0;
            NowPage = 1;
            selectedId = id;
            App.loadTask = Task.Run(() => loadItem());
            return this;
        }

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                decimal totalcount = 0;
                var dResult = entitydb.Results.Where(p => p.ResultId == selectedId).FirstOrDefault();
                List<Classes.ResultItem> dResultItems = new List<Classes.ResultItem>();

                App.mainScreen.navigationWriter("result", dResult.ResultName);

                if (filter)
                {
                    switch (filterTxt)
                    {
                        case "subject":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultSubjectId != 0).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultSubjectId != 0).Count();
                            break;

                        case "library":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultLibId != 0).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultLibId != 0).Count();
                            break;

                        case "note":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultNoteId != 0).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultNoteId != 0).Count();
                            break;
                    }
                }
                else
                {
                    dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId).Skip(lastPage).Take(25).ToList();
                    totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId).Count();
                }

                this.Dispatcher.Invoke(() =>
                {
                    listBorder.Visibility = Visibility.Visible;
                    stackBorder.Visibility = Visibility.Collapsed;

                    countSub.Content = entitydb.ResultItems.Where(p => p.ResultSubjectId != 0).Count();
                    countLib.Content = entitydb.ResultItems.Where(p => p.ResultLibId != 0).Count();
                    countNot.Content = entitydb.ResultItems.Where(p => p.ResultNoteId != 0).Count();
                    loadHeader.Text = dResult.ResultName;
                    loadBgColor.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(dResult.ResultStatus);

                    for (int x = 1; x < 25; x++)
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("lib" + x);
                        itemslist.ItemsSource = null;
                    }

                    int i = 1;
                    List<Dstack> dTemp = new List<Dstack>();
                    foreach (var item in dResultItems)
                    {
                        Dstack tempData = new Dstack();

                        tempData.BaseId = item.ResultItemId;
                        tempData.ItemDateTime = item.SendTime.ToString("d");
                        if (item.ResultNoteId != 0)
                        {
                            tempData.ItemName = entitydb.Notes.Where(p => p.NotesId == item.ResultNoteId).Select(p => new Notes { NoteHeader = p.NoteHeader }).FirstOrDefault().NoteHeader;
                            tempData.ItemColor = "#B30B00";
                            tempData.ItemId = item.ResultNoteId;
                        }
                        if (item.ResultSubjectId != 0)
                        {
                            tempData.ItemName = entitydb.Subject.Where(p => p.SubjectId == item.ResultSubjectId).Select(p => new Subject { SubjectName = p.SubjectName }).FirstOrDefault().SubjectName;
                            tempData.ItemColor = "#FD7E14";
                            tempData.ItemId = item.ResultSubjectId;
                        }
                        if (item.ResultLibId != 0)
                        {
                            tempData.ItemName = entitydb.Librarys.Where(p => p.LibraryId == item.ResultLibId).Select(p => new Library { LibraryName = p.LibraryName }).FirstOrDefault().LibraryName;
                            tempData.ItemColor = "#E33FA1";
                            tempData.ItemId = item.ResultLibId;
                        }
                        dTemp.Add(tempData);
                        ItemsControl itemslist = (ItemsControl)this.FindName("lib" + i);
                        itemslist.ItemsSource = dTemp;
                        dTemp.Clear();
                        i++;
                    }

                    Thread.Sleep(200);
                    if (dResultItems.Count() != 0)
                    {
                        totalcountText.Tag = totalcount.ToString();

                        totalcount = Math.Ceiling(totalcount / 24);
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
                loadAniComplated();
            }
        }

        // --------------- Click Func --------------- //

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            CheckBox? btn = sender as CheckBox;

            listview.IsChecked = true;
            stackview.IsChecked = false;

            if (statusSubject.IsChecked == false && statusLib.IsChecked == false && statusNote.IsChecked == false)
            {
                filter = false;
            }
            else
            {
                statusSubject.IsChecked = false;
                statusLib.IsChecked = false;
                statusNote.IsChecked = false;
                btn.IsChecked = true;
                filter = true;
                filterTxt = btn.GetValue(Extensions.DataStorage).ToString();
            }

            App.loadTask = Task.Run(() => loadItem());
        }

        private void stackfilter_Click(object sender, RoutedEventArgs e)
        {
            Button? btn = sender as Button;

            listview.IsChecked = true;
            stackview.IsChecked = false;

            statusSubject.IsChecked = false;
            statusLib.IsChecked = false;
            statusNote.IsChecked = false;

            filter = true;
            filterTxt = btn.GetValue(Extensions.DataStorage).ToString();

            App.loadTask = Task.Run(() => loadItem());
        }

        private void gotoAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.GetValue(Extensions.DataStorage))
            {
                case "#B30B00":

                    App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid));

                    break;

                case "#FD7E14":
                    using (var entitydb = new AyetContext())
                    {
                        var dtemp = entitydb.Subject.Where(p => p.SubjectId == int.Parse(btn.Uid)).Select(p => new Subject { SubjectId = p.SubjectId }).FirstOrDefault();
                        if (dtemp != null) App.mainframe.Content = App.navSubjectFolder.PageCall(dtemp.SubjectId);
                    }

                    break;

                case "#E33FA1":
                    using (var entitydb = new AyetContext())
                    {
                        var dtemp = entitydb.Librarys.Where(p => p.LibraryId == int.Parse(btn.Uid)).FirstOrDefault();
                        if (dtemp != null) App.mainframe.Content = App.navLibraryNoteItemsFrame.PageCall(dtemp.LibraryId);
                    }
                    break;
            }
        }

        private void deleteResult_Click(object sender, RoutedEventArgs e)
        {
        }

        private void viewchange_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            listview.IsChecked = false;
            stackview.IsChecked = false;

            chk.IsChecked = true;

            if ((string)chk.Content == "Liste")
            {
                listBorder.Visibility = Visibility.Visible;
                stackBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                listBorder.Visibility = Visibility.Collapsed;
                stackBorder.Visibility = Visibility.Visible;
            }
            hoverPopup.IsOpen = false;
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 24;
                NowPage++;
                App.loadTask = Task.Run(loadItem);
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
                if (lastPage >= 24)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 24;
                    NowPage--;
                    App.loadTask = Task.Run(loadItem);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void backPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                App.logWriter("Navigation", ex);
            }
        }

        private void resultScreenOpen_Click(object sender, RoutedEventArgs e)
        {
            resultScreen rssc = new resultScreen(selectedId);
            rssc.Show();
        }

        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            if (hoverPopup.IsOpen)
            {
                hoverPopup.IsOpen = false;
            }
            else
            {
                hoverPopup.IsOpen = true;
            }
        }

        // --------------- Click Func --------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                statusSubject.IsEnabled = false;
                statusLib.IsEnabled = false;
                statusNote.IsEnabled = false;
                backPage.IsEnabled = false;
                resultScreenOpen.IsEnabled = false;
                filterButton.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                statusSubject.IsEnabled = true;
                statusLib.IsEnabled = true;
                statusNote.IsEnabled = true;
                backPage.IsEnabled = true;
                resultScreenOpen.IsEnabled = true;
                filterButton.IsEnabled = true;
            });
        }

        // ------------ Animation Func ------------ //
    }
}