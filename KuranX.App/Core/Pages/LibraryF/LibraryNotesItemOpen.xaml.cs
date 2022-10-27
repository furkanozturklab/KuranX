using KuranX.App.Core.Classes;
using Microsoft.Extensions.DependencyModel;
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
    /// Interaction logic for LibraryNotesItemOpen.xaml
    /// </summary>
    public partial class LibraryNotesItemOpen : Page
    {
        private int selectedLibId, lastNotesItems, NowPage = 1;
        private List<Notes> dNotes, tempNotes = new List<Notes>();
        private Task PageItemLoadTask;
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public LibraryNotesItemOpen()
        {
            InitializeComponent();
        }

        public LibraryNotesItemOpen(int libId, string libName, string create, SolidColorBrush bgcolor) : this()
        {
            selectedLibId = libId;
            loadHeader.Text = libName;
            loadCreated.Text = create;
            loadHeaderColor.Background = bgcolor;
        }

        private void loadNotes()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                dNotes = entitydb.Notes.Where(p => p.LibraryId == selectedLibId).Skip(lastNotesItems).Take(21).ToList();
                decimal totalcount = entitydb.Notes.Where(p => p.LibraryId == selectedLibId).Count();
                for (int x = 1; x < 21; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("sbi" + x);
                        itemslist.ItemsSource = null;
                    });
                }
                int i = 1;

                foreach (var item in dNotes)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        tempNotes.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("sbi" + i);
                        itemslist.ItemsSource = tempNotes;
                        tempNotes.Clear();
                        i++;
                    });

                    if (i == 21) break; // 12 den fazla varmı kontrol etmek için koydum
                }

                Thread.Sleep(200);
                this.Dispatcher.Invoke(() =>
                {
                    totalcountText.Tag = totalcount.ToString();

                    if (dNotes.Count() != 0)
                    {
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

                loadAniComplated();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageItemLoadTask = new Task(loadNotes);
            PageItemLoadTask.Start();
        }

        private void libNoteItemsOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                App.mainframe.Content = new NoteF.NoteItem(int.Parse(btn.Uid), "Kütüphane");
                loadBorder.Visibility = Visibility.Hidden;
                loadHeaderBorder.Visibility = Visibility.Hidden;
                loadStack.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Navigation", ex);
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

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastNotesItems += 20;
                NowPage++;
                PageItemLoadTask = new Task(loadNotes);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void libHeaderItemsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            deleteLibHeaderpopup.IsOpen = true;
        }

        private void deleteLibHeaderPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dUpdate = entitydb.Notes.Where(p => p.LibraryId == selectedLibId).ToList();

                    foreach (var d in dUpdate)
                    {
                        d.LibraryId = 0;
                    }
                    entitydb.Librarys.RemoveRange(entitydb.Librarys.Where(p => p.LibraryId == selectedLibId));
                    entitydb.SaveChanges();
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Remove", ex);
            }
        }

        private void voidgobacktimer()
        {
            try
            {
                backPage.IsEnabled = false;
                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;

                timeSpan.Interval = TimeSpan.FromSeconds(3);
                timeSpan.Start();
                succsessFunc("Silme Başarılı", "Kütüphane başlığı ve başlığa ait tüm notlar serbes bırakıldı. Kütüphaneye yönlendiriliyorsunuz...", 3);
                timeSpan.Tick += delegate
                {
                    timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("TimeSpan", ex);
            }
        }

        private void previusPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lastNotesItems >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastNotesItems -= 20;
                    NowPage--;
                    PageItemLoadTask = new Task(loadNotes);
                    PageItemLoadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
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

        private void sendResult_Click(object sender, RoutedEventArgs e)
        {
            sendResultItemsPopup.IsOpen = true;
        }

        private void connectResultControl_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                try
                {
                    var item = popupResultSureId.SelectedItem as ComboBoxItem;
                    var dResult = entitydb.Results.Where(p => p.ResultId == int.Parse(item.Uid)).FirstOrDefault();

                    if (entitydb.ResultItems.Where(p => p.ResultId == dResult.ResultId && p.ResultLibId == selectedLibId).Count() == 0)
                    {
                        dResult.ResultLib = "true";
                        var dTemp = new ResultItem { ResultId = dResult.ResultId, ResultLibId = selectedLibId, SendTime = DateTime.Now };
                        entitydb.ResultItems.Add(dTemp);
                        entitydb.SaveChanges();
                        sendResultItemsPopup.IsOpen = false;
                        succsessFunc("Gönderme Başarılı", "Kütüphane başlığı " + item.Content + " suresinin sonucuna gönderildi.", 3);
                    }
                    else
                    {
                        sendResultItemsPopup.IsOpen = false;
                        alertFunc("Gönderme Başarısız", "Kütüphane başlığı " + item.Content + " suresinin sonucuna daha önceden eklenmiştir yeniden ekleyemezsiniz.", 3);
                    }
                }
                catch (Exception ex)
                {
                    App.logWriter("Other", ex);
                }
            }
        }

        private void alertFunc(string header, string detail, int timespan)
        {
            try
            {
                alertPopupHeader.Text = header;
                alertPopupDetail.Text = detail;
                alph.IsOpen = true;

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    alph.IsOpen = false;
                    timeSpan.Stop();
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

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    scph.IsOpen = false;
                    timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    sendResult.IsEnabled = false;
                    backPage.IsEnabled = false;
                    previusPageButton.IsEnabled = false;
                    nextpageButton.IsEnabled = false;
                    loadinGifContent.Visibility = Visibility.Visible;
                    loadBorder.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    backPage.IsEnabled = true;
                    sendResult.IsEnabled = true;
                    loadinGifContent.Visibility = Visibility.Collapsed;
                    loadBorder.Visibility = Visibility.Visible;
                    loadHeaderBorder.Visibility = Visibility.Visible;
                    loadStack.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }
    }
}