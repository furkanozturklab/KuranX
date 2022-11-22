using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
using Microsoft.EntityFrameworkCore;
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

namespace KuranX.App.Core.Pages.NoteF
{
    /// <summary>
    /// Interaction logic for NoteFrame.xaml
    /// </summary>
    ///

    public class Dstack
    {
        public int Count { get; set; }
        public string? NoteLocation { get; set; }
        public string? Bg { get; set; }
        public string? Fr { get; set; }
    }

    public partial class NoteFrame : Page
    {
        private string searchText, filterText;
        private int lastPage = 0, NowPage = 1, selectedId;
        private bool filter;
        private List<Notes> dNotes = new List<Notes>();

        public NoteFrame()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listBorder.Visibility = Visibility.Visible;
            // App.loadTask = Task.Run(() => loadItem());
        }

        public Page PageCall()
        {
            lastPage = 0;
            NowPage = 1;
            App.mainScreen.navigationWriter("notes", "");
            filter = false;
            searchText = "";
            listview.IsChecked = true;
            stackview.IsChecked = false;
            listBorder.Visibility = Visibility.Visible;
            stackBorder.Visibility = Visibility.Collapsed;

            App.loadTask = Task.Run(() => loadItem());
            return this;
        }

        // -------------  Load Func ------------- //

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();

                decimal totalcount = entitydb.Notes.Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").Count();
                //var stackP = entitydb.Notes.Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").OrderByDescending(p => p.Created).GroupBy(p => p.NoteLocation).ToList();

                App.mainScreen.navigationWriter("notes", "");

                this.Dispatcher.Invoke(() =>
                {
                    listBorder.Visibility = Visibility.Visible;
                    stackBorder.Visibility = Visibility.Collapsed;
                });

                if (filter)
                {
                    if (searchText != "")
                    {
                        dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.NoteHeader, "%" + searchText + "%")).Where(p => p.NoteLocation == filterText).OrderByDescending(p => p.Created).Skip(lastPage).Take(24).ToList();
                        totalcount = entitydb.Notes.Where(p => EF.Functions.Like(p.NoteHeader, "%" + searchText + "%")).Where(p => p.NoteLocation == filterText).ToList().Count();
                    }
                    else
                    {
                        dNotes = entitydb.Notes.Where(p => p.NoteLocation == filterText).OrderByDescending(p => p.Created).Skip(lastPage).Take(24).ToList();
                        totalcount = entitydb.Notes.Where(p => p.NoteLocation == filterText).ToList().Count();
                    }
                }
                else
                {
                    if (searchText != "")
                    {
                        dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.NoteHeader, "%" + searchText + "%")).Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").OrderByDescending(p => p.Created).Skip(lastPage).Take(24).ToList();
                        totalcount = entitydb.Notes.Where(p => EF.Functions.Like(p.NoteHeader, "%" + searchText + "%")).Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").ToList().Count();
                    }
                    else
                    {
                        dNotes = entitydb.Notes.Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").OrderByDescending(p => p.Created).Skip(lastPage).Take(24).ToList();
                        totalcount = entitydb.Notes.Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").ToList().Count();
                    }
                }

                for (int x = 1; x <= 24; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var sbItem = (Border)FindName("nt" + x);
                        sbItem.Visibility = Visibility.Hidden;
                    });
                }

                this.Dispatcher.Invoke(() =>
                {
                    subStat.Content = entitydb.Notes.Where(p => p.NoteLocation == "Konularım").Count();
                    libStat.Content = entitydb.Notes.Where(p => p.NoteLocation == "Kütüphane").Count();
                    verseStat.Content = entitydb.Notes.Where(p => p.NoteLocation == "Ayet").Count();
                    pdfStat.Content = entitydb.Notes.Where(p => p.NoteLocation == "PDF").Count();
                    userStat.Content = entitydb.Notes.Where(p => p.NoteLocation == "Kullanıcı").Count();
                });
                int i = 1;

                Thread.Sleep(200);

                foreach (var item in dNotes)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var sColor = (Border)FindName("ntColor" + i);
                        switch (item.NoteLocation)
                        {
                            case "Konularım":
                                sColor.Background = new BrushConverter().ConvertFrom("#FD7E14") as SolidColorBrush;
                                break;

                            case "Kütüphane":

                                sColor.Background = new BrushConverter().ConvertFrom("#E33FA1") as SolidColorBrush;
                                break;

                            case "Ayet":

                                sColor.Background = new BrushConverter().ConvertFrom("#0DCAF0") as SolidColorBrush;
                                break;

                            case "PDF":

                                sColor.Background = new BrushConverter().ConvertFrom("#B30B00") as SolidColorBrush;
                                break;

                            default:
                                sColor.Background = new BrushConverter().ConvertFrom("#ADB5BD") as SolidColorBrush;
                                break;
                        }

                        var sName = (TextBlock)FindName("ntName" + i);
                        sName.Text = item.NoteHeader;

                        var sCreated = (TextBlock)FindName("ntCreate" + i);
                        sCreated.Text = item.Created.ToString("D");

                        var sBtnGo = (Button)FindName("ntBtnGo" + i);
                        sBtnGo.Uid = item.NotesId.ToString();

                        var sBtnDel = (Button)FindName("ntBtnDel" + i);
                        sBtnDel.Uid = item.NotesId.ToString();

                        var sbItem = (Border)FindName("nt" + i);
                        sbItem.Visibility = Visibility.Visible;

                        i++;
                    });
                }

                i = 1;

                this.Dispatcher.Invoke(() =>
                {
                    totalcountText.Tag = totalcount.ToString();

                    if (dNotes.Count() != 0)
                    {
                        totalcount = Math.Ceiling(totalcount / 24);
                        nowPageStatus.Tag = NowPage + " / " + totalcount;
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

        // -------------  Load Func ------------- //

        // -------------  Click Func ------------- //

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            listview.IsChecked = true;
            stackview.IsChecked = false;
            lastPage = 0;
            NowPage = 1;

            if ((string)btn.Content == "Hepsi")
            {
                filter = false;
                filterText = "";
                searchText = "";
                SearchData.Text = "";
            }
            else
            {
                filter = true;
                filterText = btn.Content.ToString();
                searchText = "";
                SearchData.Text = "";
            }

            App.loadTask = Task.Run(() => loadItem());
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SearchData.Text.Length >= 3)
                {
                    searchText = SearchData.Text;

                    App.loadTask = Task.Run(loadItem);
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchText = "";
                        App.loadTask = Task.Run(loadItem);
                    }
                    else
                    {
                        searchErrMsgTxt.Visibility = Visibility.Visible;
                        SearchData.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("SearchButton", ex);
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

        private void addNewNote_Click(object sender, RoutedEventArgs e)
        {
            popup_noteAddPopup.IsOpen = true;
            noteType.Text = "Kullanıcı Notu";
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

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (noteName.Text.Length <= 3)
                {
                    noteAddPopupHeaderError.Visibility = Visibility.Visible;
                    noteName.Focus();
                    noteAddPopupHeaderError.Content = "Not Başlığı Yeterince Uzun Değil. Min 3 Karakter Olmalıdır.";
                }
                else
                {
                    if (noteName.Text.Length > 150)
                    {
                        noteAddPopupHeaderError.Visibility = Visibility.Visible;
                        noteName.Focus();
                        noteAddPopupHeaderError.Content = "Not Başlığı Çok Uzun. Max 150 Karakter Olabilir.";
                    }
                    else
                    {
                        if (noteDetail.Text.Length <= 3)
                        {
                            noteAddPopupDetailError.Visibility = Visibility.Visible;
                            noteDetail.Focus();
                            noteAddPopupDetailError.Content = "Not İçeriği Yeterince Uzun Değil. Min 3 Karakter Olmalıdır";
                        }
                        else
                        {
                            if (noteDetail.Text.Length >= 3000)
                            {
                                noteAddPopupDetailError.Visibility = Visibility.Visible;
                                noteDetail.Focus();
                                noteAddPopupDetailError.Content = "Not İçeriği 3000 Maximum karakterden fazla olamaz.";
                            }
                            else
                            {
                                using (var entitydb = new AyetContext())
                                {
                                    if (entitydb.Notes.Where(p => p.NoteHeader == noteName.Text && p.NoteLocation == "Kullanıcı").FirstOrDefault() != null)
                                    {
                                        alertFunc("Not Ekleme Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", 3);
                                    }
                                    else
                                    {
                                        var dNotes = new Notes { NoteHeader = noteName.Text, NoteDetail = noteDetail.Text, SureId = 0, VerseId = 0, Modify = DateTime.Now, Created = DateTime.Now, NoteLocation = "Kullanıcı" };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        succsessFunc("Not Ekleme Başarılı", "Notunuz Eklenmiştir.", 3);
                                        noteName.Text = "";
                                        noteDetail.Text = "";

                                        App.loadTask = Task.Run(() => loadItem());

                                        dNotes = null;
                                    }

                                    noteName.Text = "";
                                    noteDetail.Text = "";
                                }
                                popup_noteAddPopup.IsOpen = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
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

        private void gotoNotes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            listBorder.Visibility = Visibility.Hidden;
            App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid));
        }

        private void deleteNotes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            popup_deleteNote.IsOpen = true;
            selectedId = int.Parse(btn.Uid);
        }

        private void deleteNotePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.NotesId == selectedId));
                    entitydb.SaveChanges();
                    popup_deleteNote.IsOpen = false;
                    App.loadTask = Task.Run(loadItem);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Remove", ex);
            }
        }

        // -------------  Click Func ------------- //

        // ---------------- Changed Func ---------------- //

        private void SearchData_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (SearchData.Text.Length >= 3)
                {
                    searchErrMsgTxt.Visibility = Visibility.Hidden;
                }
                else
                {
                    searchErrMsgTxt.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Search", ex);
            }
        }

        private void SearchData_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                searchErrMsgTxt.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Search", ex);
            }
        }

        private void noteName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                noteAddPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void noteDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                noteAddPopupDetailError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // ---------------- Changed Func ---------------- //

        // ---------- MessageFunc FUNC ---------- //

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

        // ---------- MessageFunc FUNC ---------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                SearchBtn.IsEnabled = false;
                addNewNote.IsEnabled = false;
                filterButton.IsEnabled = false;
                filterAll.IsEnabled = false;
                filterUser.IsEnabled = false;
                filterSubject.IsEnabled = false;

                filterVerse.IsEnabled = false;
                filterPdf.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                SearchBtn.IsEnabled = true;
                addNewNote.IsEnabled = true;
                filterButton.IsEnabled = true;
                filterAll.IsEnabled = true;
                filterUser.IsEnabled = true;
                filterSubject.IsEnabled = true;

                filterVerse.IsEnabled = true;
                filterPdf.IsEnabled = true;
            });
        }

        // ------------ Animation Func ------------ //
    }
}