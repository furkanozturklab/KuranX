using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Ocsp;
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
        private int lastPage = 0, NowPage = 1, selectedId;
        private bool filter = false;
        private string filterTxt, resultName;

        public ResultItem()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //headerLoad.Visibility = Visibility.Visible;
            App.mainScreen.navigationWriter("result", resultName);
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
            loadAni();
            using (var entitydb = new AyetContext())
            {
                decimal totalcount = 0;

                var dResult = entitydb.Results.Where(p => p.ResultId == selectedId).FirstOrDefault();

                resultName = dResult.ResultName;

                List<Classes.ResultItem> dResultItems = new List<Classes.ResultItem>();

                App.mainScreen.navigationWriter("result", resultName);

                if (filter)
                {
                    switch (filterTxt)
                    {
                        case "subject":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultSubjectId != 0).Skip(lastPage).Take(20).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultSubjectId != 0).Count();
                            break;

                        case "library":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultLibId != 0).Skip(lastPage).Take(20).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultLibId != 0).Count();
                            break;

                        case "note":
                            dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultNoteId != 0).Skip(lastPage).Take(20).ToList();
                            totalcount = entitydb.ResultItems.Where(p => p.ResultId == selectedId && p.ResultNoteId != 0).Count();
                            break;
                    }
                }
                else
                {
                    dResultItems = entitydb.ResultItems.Where(p => p.ResultId == selectedId).Skip(lastPage).Take(20).ToList();
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
                    loadBgColor.Background = new BrushConverter().ConvertFrom(dResult.ResultStatus) as SolidColorBrush;
                });
                for (int x = 1; x <= 20; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var sbItem = (Border)FindName("lib" + x);
                        sbItem.Visibility = Visibility.Hidden;
                    });
                }

                int i = 1;

                Thread.Sleep(300);

                foreach (var item in dResultItems)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var sLoc = (TextBlock)FindName("libLoc" + i);
                        var sName = (TextBlock)FindName("libName" + i);
                        var sCreate = (TextBlock)FindName("libCreate" + i);
                        var sBtn = (Button)FindName("libBtn" + i);
                        var sBtnDel = (Button)FindName("libDel" + i);

                        if (item.ResultNoteId != 0)
                        {
                            sLoc.Text = "Not";
                            sName.Text = entitydb.Notes.Where(p => p.NotesId == item.ResultNoteId).Select(p => new Notes { NoteHeader = p.NoteHeader }).FirstOrDefault().NoteHeader;
                            sBtn.Uid = item.ResultNoteId.ToString();
                            sBtn.Content = "Not";
                        }
                        if (item.ResultSubjectId != 0)
                        {
                            sLoc.Text = "Konular";
                            sName.Text = entitydb.Subject.Where(p => p.SubjectId == item.ResultSubjectId).Select(p => new Subject { SubjectName = p.SubjectName }).FirstOrDefault().SubjectName;
                            sBtn.Uid = item.ResultSubjectId.ToString();
                            sBtn.Content = "Konular";
                        }
                        if (item.ResultLibId != 0)
                        {
                            sLoc.Text = "Kütüphane";
                            sName.Text = entitydb.Librarys.Where(p => p.LibraryId == item.ResultLibId).Select(p => new Library { LibraryName = p.LibraryName }).FirstOrDefault().LibraryName;
                            sBtn.Uid = item.ResultLibId.ToString();
                            sBtn.Content = "Kütüphane";
                        }

                        sBtnDel.Uid = item.ResultItemId.ToString();
                        sCreate.Text = item.SendTime.ToString("D");

                        var sbItem = (Border)FindName("lib" + i);
                        sbItem.Visibility = Visibility.Visible;

                        i++;
                    });
                }

                this.Dispatcher.Invoke(() =>
                {
                    if (dResultItems.Count() != 0)
                    {
                        totalcountText.Tag = totalcount.ToString();

                        totalcount = Math.Ceiling(totalcount / 20);
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
            }
            loadAniComplated();
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

            switch (btn.GetValue(Extensions.DataStorage).ToString())
            {
                case "subject":
                    statusSubject.IsChecked = true;
                    break;

                case "library":
                    statusLib.IsChecked = true;
                    break;

                case "note":
                    statusNote.IsChecked = true;
                    break;

                default:
                    statusSubject.IsChecked = false;
                    statusLib.IsChecked = false;
                    statusNote.IsChecked = false;
                    break;
            }

            filter = true;
            filterTxt = btn.GetValue(Extensions.DataStorage).ToString();

            App.loadTask = Task.Run(() => loadItem());
        }

        private void gotoAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.Content)
            {
                case "Not":

                    App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid));

                    break;

                case "Konular":
                    using (var entitydb = new AyetContext())
                    {
                        var dtemp = entitydb.Subject.Where(p => p.SubjectId == int.Parse(btn.Uid)).Select(p => new Subject { SubjectId = p.SubjectId }).FirstOrDefault();
                        if (dtemp != null) App.mainframe.Content = App.navSubjectFolder.PageCall(dtemp.SubjectId);
                    }

                    break;

                case "Kütüphane":
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
                headerLoad.Visibility = Visibility.Hidden;
                headerControl.Visibility = Visibility.Hidden;
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
                headerLoad.Visibility = Visibility.Visible;
                headerControl.Visibility = Visibility.Visible;
            });
        }

        // ------------ Animation Func ------------ //
    }
}