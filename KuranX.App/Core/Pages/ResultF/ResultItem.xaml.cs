using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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

        private bool filter = false;
        private string filterTxt;
        private int lastResultItem = 0, selectedId, NowPage = 1, itemDeleteId;
        private Task loadTask;
        private List<Classes.ResultItem> dResultItems = new List<Classes.ResultItem>();
        private List<Dstack> dTemp = new List<Dstack>();
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public ResultItem()
        {
            InitializeComponent();
        }

        public ResultItem(int id) : this()
        {
            selectedId = id;
        }

        private void resultItemsLoad()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();

                decimal totalcount = 0;
                var dResult = entitydb.Results.Where(p => p.ResultId == selectedId).FirstOrDefault();

                if (filter)
                {
                    switch (filterTxt)
                    {
                        case "subject":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultSubjectId != 0).Skip(lastResultItem).Take(25).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultSubjectId != 0).Count();
                            break;

                        case "library":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultLibId != 0).Skip(lastResultItem).Take(25).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultLibId != 0).Count();
                            break;

                        case "note":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultNoteId != 0).Skip(lastResultItem).Take(25).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultNoteId != 0).Count();
                            break;
                    }
                }
                else
                {
                    dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId).Skip(lastResultItem).Take(25).ToList();
                    totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId).Count();
                }

                this.Dispatcher.Invoke(() =>
                {
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

                        if (i == 25) break; // 24 den fazla varmı kontrol etmek için koydum
                    }

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadTask = new Task(resultItemsLoad);
            loadTask.Start();
        }

        private void gotoAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            Debug.WriteLine("btn val : " + btn.Uid);

            switch (btn.GetValue(Extensions.DataStorage))
            {
                case "#B30B00":

                    App.mainframe.Content = new NoteF.NoteItem(int.Parse(btn.Uid), "Result");

                    break;

                case "#FD7E14":
                    using (var entitydb = new AyetContext())
                    {
                        var dtemp = entitydb.Subject.Where(p => p.SubjectId == int.Parse(btn.Uid)).FirstOrDefault();
                        if (dtemp != null) App.mainframe.Content = new SubjectF.SubjectItemsFrame(dtemp.SubjectId, dtemp.SubjectName, dtemp.Created.ToString("D"), dtemp.SubjectColor);
                    }

                    break;

                case "#E33FA1":
                    using (var entitydb = new AyetContext())
                    {
                        var dtemp = entitydb.Librarys.Where(p => p.LibraryId == int.Parse(btn.Uid)).FirstOrDefault();
                        if (dtemp != null) App.mainframe.Content = new LibraryF.LibraryNotesItemOpen(dtemp.LibraryId, dtemp.LibraryName, dtemp.Created.ToString("D"), (SolidColorBrush)new BrushConverter().ConvertFrom(dtemp.LibraryColor));
                    }
                    break;
            }
        }

        private void deleteResult_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            deleteResultpopup.IsOpen = true;
            itemDeleteId = int.Parse(btn.Uid);
        }

        private void deleteResultPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                entitydb.ResultItems.RemoveRange(entitydb.ResultItems.Where(p => p.ResultItemId == itemDeleteId));
                entitydb.SaveChanges();

                if (entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultNoteId != 0).Count() == 0)
                    entitydb.Results.Where(p => p.ResultId == selectedId).FirstOrDefault().ResultNotes = "false";

                if (entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultLibId != 0).Count() == 0)
                    entitydb.Results.Where(p => p.ResultId == selectedId).FirstOrDefault().ResultLib = "false";

                if (entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultSubjectId != 0).Count() == 0)
                    entitydb.Results.Where(p => p.ResultId == selectedId).FirstOrDefault().ResultSubject = "false";

                entitydb.SaveChanges();
                deleteResultpopup.IsOpen = false;
                loadTask = new Task(resultItemsLoad);
                loadTask.Start();
            }
        }

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

            loadTask = new Task(resultItemsLoad);
            loadTask.Start();
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

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                backPage.IsEnabled = false;
                filterButton.IsEnabled = false;
                resultScreenOpen.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                filterButton.IsEnabled = true;
                backPage.IsEnabled = true;
                resultScreenOpen.IsEnabled = true;
                loadinGifContent.Visibility = Visibility.Collapsed;
            });
        }

        private void resultScreenOpen_Click(object sender, RoutedEventArgs e)
        {
            resultScreen rssc = new resultScreen();
            rssc.Show();
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastResultItem += 24;
                NowPage++;
                loadTask = new Task(resultItemsLoad);
                loadTask.Start();
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
                if (lastResultItem >= 24)
                {
                    previusPageButton.IsEnabled = false;
                    lastResultItem -= 24;
                    NowPage--;
                    loadTask = new Task(resultItemsLoad);
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }
    }
}