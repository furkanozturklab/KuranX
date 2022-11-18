using KuranX.App.Core.Classes;
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
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for LibraryNoteItemsFrame.xaml
    /// </summary>
    public partial class LibraryNoteItemsFrame : Page
    {
        private int lastPage = 0, NowPage = 1, folderId = 0;

        public LibraryNoteItemsFrame()
        {
            InitializeComponent();
        }

        public object PageCall(int id)
        {
            lastPage = 0;
            NowPage = 1;
            folderId = id;
            App.loadTask = Task.Run(() => loadItem(folderId));
            return this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.mainScreen.navigationWriter("library", "Kütüphane Başlıkları," + loadHeader.Text);
        }

        // --------------- Load Func --------------- //

        public void loadItem(int folderId)
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                var dlibrary = entitydb.Librarys.Where(p => p.LibraryId == folderId).First();
                var dNotes = entitydb.Notes.Where(p => p.LibraryId == folderId).Skip(lastPage).Take(20).ToList();

                Decimal totalcount = entitydb.Notes.Where(p => p.LibraryId == folderId).Count();

                this.Dispatcher.Invoke(() =>
                {
                    loadHeader.Text = dlibrary.LibraryName;
                    loadCreated.Text = dlibrary.Created.ToString("D");
                    loadHeaderColor.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(dlibrary.LibraryColor);

                    App.mainScreen.navigationWriter("library", "Kütüphane Başlıkları," + loadHeader.Text);

                    for (int x = 1; x < 21; x++)
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("lni" + x);
                        itemslist.ItemsSource = null;
                    }

                    int i = 1;
                    List<Notes> tempNote = new List<Notes>();
                    foreach (var item in dNotes)
                    {
                        tempNote.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("lni" + i);
                        itemslist.ItemsSource = tempNote;
                        tempNote.Clear();
                        i++;
                    }

                    Thread.Sleep(200);

                    totalcountText.Tag = totalcount.ToString();

                    if (dNotes.Count() != 0)
                    {
                        totalcount = Math.Ceiling(totalcount / 15);
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

                    loadAniComplated();
                });
            }
        }

        // --------------- Load Func --------------- //

        // --------------- Click Func --------------- //

        private void libNoteItemsOpen_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid));
        }

        private void libraryFolderDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            popup_LibraryDelete.IsOpen = true;
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

        private void sendResult_Click(object sender, RoutedEventArgs e)
        {
            popup_sendResult.IsOpen = true;
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 20;
                NowPage++;
                App.loadTask = Task.Run(() => loadItem(folderId));
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
                if (lastPage >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 20;
                    NowPage--;
                    App.loadTask = Task.Run(() => loadItem(folderId));
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void deleteLibraryPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dUpdate = entitydb.Notes.Where(p => p.LibraryId == folderId).ToList();

                    foreach (var d in dUpdate)
                    {
                        d.LibraryId = 0;
                    }
                    entitydb.Librarys.RemoveRange(entitydb.Librarys.Where(p => p.LibraryId == folderId));
                    entitydb.SaveChanges();
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Remove", ex);
            }
        }

        private void connectResultControl_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                try
                {
                    var item = popupResultSureId.SelectedItem as ComboBoxItem;
                    var dResult = entitydb.Results.Where(p => p.ResultId == int.Parse(item.Uid)).FirstOrDefault();

                    if (entitydb.ResultItems.Where(p => p.ResultId == dResult.ResultId && p.ResultLibId == folderId).Count() == 0)
                    {
                        dResult.ResultLib = "true";
                        var dTemp = new ResultItem { ResultId = dResult.ResultId, ResultLibId = folderId, SendTime = DateTime.Now };
                        entitydb.ResultItems.Add(dTemp);
                        entitydb.SaveChanges();
                        popup_sendResult.IsOpen = false;
                        succsessFunc("Gönderme Başarılı", "Kütüphane başlığı " + item.Content + " suresinin sonucuna gönderildi.", 3);
                    }
                    else
                    {
                        popup_sendResult.IsOpen = false;
                        alertFunc("Gönderme Başarısız", "Kütüphane başlığı " + item.Content + " suresinin sonucuna daha önceden eklenmiştir yeniden ekleyemezsiniz.", 3);
                    }
                }
                catch (Exception ex)
                {
                    App.logWriter("Other", ex);
                }
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // --------------- Click Func --------------- //

        // --------------- MessageFunc FUNC --------------- //

        private void alertFunc(string header, string detail, int timespan)
        {
            try
            {
                alertPopupHeader.Text = header;
                alertPopupDetail.Text = detail;
                alph.IsOpen = true;

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
                App.logWriter("Other", ex);
            }
        }

        private void infoFunc(string header, string detail, int timespan)
        {
            try
            {
                infoPopupHeader.Text = header;
                infoPopupDetail.Text = detail;
                inph.IsOpen = true;

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

        private void succsessFunc(string header, string detail, int timespan)
        {
            try
            {
                successPopupHeader.Text = header;
                successPopupDetail.Text = detail;
                scph.IsOpen = true;

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
                App.logWriter("Other", ex);
            }
        }

        // --------------- MessageFunc FUNC --------------- //

        // --------------- TimeSpan FUNC --------------- //

        private void voidgobacktimer()
        {
            try
            {
                backPage.IsEnabled = false;
                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;
                sendResult.IsEnabled = false;

                App.timeSpan.Interval = TimeSpan.FromSeconds(3);
                App.timeSpan.Start();
                succsessFunc("Silme Başarılı", "Kütüphane başlığı ve başlığa ait tüm notlar serbes bırakıldı. Kütüphaneye yönlendiriliyorsunuz...", 3);
                App.timeSpan.Tick += delegate
                {
                    App.timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("TimeSpan", ex);
            }
        }

        // --------------- TimeSpan FUNC --------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                backPage.IsEnabled = false;
                sendResult.IsEnabled = false;
                libraryItemsDeleteBtn.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                backPage.IsEnabled = true;
                sendResult.IsEnabled = true;
                libraryItemsDeleteBtn.IsEnabled = true;
            });
        }

        // ------------ Animation Func ------------ //
    }
}