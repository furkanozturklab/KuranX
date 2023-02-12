using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private int lastPage = 0, NowPage = 1, selectedId, itemDeleteId = 0;
        private bool filter = false;
        private string filterTxt = "", resultName = "";

        public ResultItem()
        {
            try
            {

                App.errWrite($"[{DateTime.Now} InitializeComponent ] -> ResultItem");
                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} Page_Loaded ] -> ResultItem");


                App.mainScreen.navigationWriter("result", resultName);
            }
            catch (Exception ex)
            {
                App.logWriter("loading", ex);
            }
            //headerLoad.Visibility = Visibility.Visible;
        }

        public Page PageCall(int id)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} PageCall ] -> ResultItem");
                lastPage = 0;
                NowPage = 1;
                selectedId = id;
                App.loadTask = Task.Run(() => loadItem());
                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("loading", ex);
                return this;
            }
        }

        public void loadItem()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} loadItem ] -> ResultItem"); 
                loadAni();
                using (var entitydb = new AyetContext())
                {
                    decimal totalcount = 0;

                    var dResult = entitydb.Results.Where(p => p.resultId == selectedId).FirstOrDefault();

                    resultName = (string)dResult.resultName;

                    List<Classes.ResultItem> dResultItems = new List<Classes.ResultItem>();

                    App.mainScreen.navigationWriter("result", resultName);

                    if (filter)
                    {
                        switch (filterTxt)
                        {
                            case "subject":
                                dResultItems = entitydb.ResultItems.Where(p => p.resultId == selectedId && p.resultSubjectId != 0).Skip(lastPage).Take(20).ToList();
                                totalcount = entitydb.ResultItems.Where(p => p.resultId == selectedId && p.resultSubjectId != 0).Count();
                                break;

     
                            case "note":
                                dResultItems = entitydb.ResultItems.Where(p => p.resultId == selectedId && p.resultNoteId != 0).Skip(lastPage).Take(20).ToList();
                                totalcount = entitydb.ResultItems.Where(p => p.resultId == selectedId && p.resultNoteId != 0).Count();
                                break;
                        }
                    }
                    else
                    {
                        dResultItems = entitydb.ResultItems.Where(p => p.resultId == selectedId).Skip(lastPage).Take(20).ToList();
                        totalcount = entitydb.ResultItems.Where(p => p.resultId == selectedId).Count();
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        listBorder.Visibility = Visibility.Visible;
                        stackBorder.Visibility = Visibility.Collapsed;

                        countSub.Content = entitydb.ResultItems.Where(p => p.resultSubjectId != 0).Count();
                        countLib.Content = entitydb.ResultItems.Where(p => p.resultLibId != 0).Count();
                        countNot.Content = entitydb.ResultItems.Where(p => p.resultNoteId != 0).Count();
                        loadHeader.Text = dResult.resultName;
                        loadBgColor.Background = new BrushConverter().ConvertFrom("#ADB5BD") as SolidColorBrush;
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

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    loadAniComplated();

                    foreach (var item in dResultItems)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sLoc = (TextBlock)FindName("libLoc" + i);
                            var sName = (TextBlock)FindName("libName" + i);
                            var sCreate = (TextBlock)FindName("libCreate" + i);
                            var sBtn = (Button)FindName("libBtn" + i);
                            var sBtnDel = (Button)FindName("libDel" + i);

                            if (item.resultNoteId != 0)
                            {
                                sLoc.Text = "Not";
                                sName.Text = entitydb.Notes.Where(p => p.notesId == item.resultNoteId).Select(p => new Notes { noteHeader = p.noteHeader }).FirstOrDefault().noteHeader;
                                sBtn.Uid = item.resultNoteId.ToString();
                                sBtn.Content = "Not";
                            }
                            if (item.resultSubjectId != 0)
                            {
                                sLoc.Text = "Konular";
                                sName.Text = entitydb.Subject.Where(p => p.subjectId == item.resultSubjectId).Select(p => new Subject { subjectName = p.subjectName }).FirstOrDefault().subjectName;
                                sBtn.Uid = item.resultSubjectId.ToString();
                                sBtn.Content = "Konular";
                            }
              

                            sBtnDel.Uid = item.resultItemId.ToString();
                            sCreate.Text = item.sendTime.ToString("D");

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
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        // --------------- Click Func --------------- //

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} filter_Click ] -> ResultItem");


                CheckBox? btn = sender as CheckBox;

                listview.IsChecked = true;
                stackview.IsChecked = false;

                if (statusSubject.IsChecked == false && statusNote.IsChecked == false)
                {
                    filter = false;
                }
                else
                {
                    statusSubject.IsChecked = false;
                    statusNote.IsChecked = false;
                    btn.IsChecked = true;
                    filter = true;
                    filterTxt = (string)btn.GetValue(Extensions.DataStorage);
                }

                App.loadTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void stackfilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} stackfilter_Click ] -> ResultItem"); 

                Button? btn = sender as Button;

                listview.IsChecked = true;
                stackview.IsChecked = false;

                statusSubject.IsChecked = false;
               
                statusNote.IsChecked = false;

                switch ((string)btn.GetValue(Extensions.DataStorage))
                {
                    case "subject":
                        statusSubject.IsChecked = true;
                        break;


                    case "note":
                        statusNote.IsChecked = true;
                        break;

                    default:
                        statusSubject.IsChecked = false;
                       
                        statusNote.IsChecked = false;
                        break;
                }

                filter = true;
                filterTxt = (string)btn.GetValue(Extensions.DataStorage);

                App.loadTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void gotoAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} gotoAction_Click ] -> ResultItem");


                var btn = sender as Button;
                switch (btn.Content)
                {
                    case "Not":

                        App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid));

                        break;

                    case "Konular":
                        using (var entitydb = new AyetContext())
                        {
                            var dtemp = entitydb.Subject.Where(p => p.subjectId == int.Parse(btn.Uid)).Select(p => new Subject { subjectId = p.subjectId }).FirstOrDefault();
                            if (dtemp != null) App.mainframe.Content = App.navSubjectFolder.PageCall(dtemp.subjectId);
                        }

                        break;

                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void deleteResultPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} deleteResultPopup_Click ] -> ResultItem"); 

                var btn = sender as Button;
                popup_confirm.IsOpen = true;
                itemDeleteId = int.Parse(btn.Uid);
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void deleteResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                App.errWrite($"[{DateTime.Now} deleteResult_Click ] -> ResultItem");

                using (var entitydb = new AyetContext())
                {
                    entitydb.ResultItems.RemoveRange(entitydb.ResultItems.Where(p => p.resultItemId == itemDeleteId));
                    entitydb.SaveChanges();

                    if (entitydb.ResultItems.Where(p => p.resultId == selectedId).Count() == 0)
                        entitydb.Results.Where(p => p.resultId == selectedId).FirstOrDefault().resultNotes = false;

                    if (entitydb.ResultItems.Where(p => p.resultId == selectedId).Count() == 0)
                        entitydb.Results.Where(p => p.resultId == selectedId).FirstOrDefault().resultSubject = false;

                    entitydb.SaveChanges();
                    popup_confirm.IsOpen = false;
                    App.loadTask = Task.Run(() => loadItem());
                }
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
                App.errWrite($"[{DateTime.Now} popupClosed_Click ] -> ResultItem");


                Button btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void viewchange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} viewchange_Click ] -> ResultItem");


                CheckBox? chk = sender as CheckBox;
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
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} nextpageButton_Click ] -> ResultItem");


                nextpageButton.IsEnabled = false;
                lastPage += 24;
                NowPage++;
                App.loadTask = Task.Run(loadItem);
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void previusPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} previusPageButton_Click ] -> ResultItem");


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
                App.logWriter("Click", ex);
            }
        }

        private void backPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} backPage_Click ] -> ResultItem");


                App.mainframe.Content = App.navResultPage.PageCall();
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void resultScreenOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} resultScreenOpen_Click ] -> ResultItem");


                resultScreen rssc = new resultScreen(selectedId);
                rssc.Show();
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} filterButton_Click ] -> ResultItem");

                if (hoverPopup.IsOpen)
                {
                    hoverPopup.IsOpen = false;
                }
                else
                {
                    hoverPopup.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // --------------- Click Func --------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} loadAni ] -> ResultItem");
                this.Dispatcher.Invoke(() =>
                {
                    statusSubject.IsEnabled = false;
                 
                    statusNote.IsEnabled = false;
                    backPage.IsEnabled = false;
                    resultScreenOpen.IsEnabled = false;
                    filterButton.IsEnabled = false;
                    headerLoad.Visibility = Visibility.Hidden;
                    headerControl.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        public void loadAniComplated()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} loadAniComplated ] -> ResultItem");

                this.Dispatcher.Invoke(() =>
                {
                    statusSubject.IsEnabled = true;
              
                    statusNote.IsEnabled = true;
                    backPage.IsEnabled = true;
                    resultScreenOpen.IsEnabled = true;
                    filterButton.IsEnabled = true;
                    headerLoad.Visibility = Visibility.Visible;
                    headerControl.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //
    }
}