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
        private string searchText = "", filterText = "";
        private int lastPage = 0, NowPage = 1, selectedId;
        private bool filter;
        private List<Notes> dNotes = new List<Notes>();

        public NoteFrame()
        {
            try
            {
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
                listBorder.Visibility = Visibility.Visible;
                App.mainScreen.navigationWriter("notes", "");
            }
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
            }
        }

        public Page PageCall()
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
                return this;
            }
        }

        // -------------  Load Func ------------- //

        public void loadItem()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();

                    decimal totalcount = entitydb.Notes.Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Kütüphane" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı" || p.noteLocation == "Bölüm").Count();

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
                            dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == filterText).OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == filterText).ToList().Count();
                        }
                        else
                        {
                            dNotes = entitydb.Notes.Where(p => p.noteLocation == filterText).OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => p.noteLocation == filterText).ToList().Count();
                        }
                    }
                    else
                    {
                        if (searchText != "")
                        {
                            dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").ToList().Count();
                        }
                        else
                        {
                            dNotes = entitydb.Notes.Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").ToList().Count();
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
                        subStat.Content = entitydb.Notes.Where(p => p.noteLocation == "Konularım").Count();
                
                        verseStat.Content = entitydb.Notes.Where(p => p.noteLocation == "Ayet").Count();

                        userStat.Content = entitydb.Notes.Where(p => p.noteLocation == "Kullanıcı").Count();

                        sectionStart.Content = entitydb.Notes.Where(p => p.noteLocation == "Bölüm").Count();
                    });
                    int i = 1;

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    foreach (var item in dNotes)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sColor = (Border)FindName("ntColor" + i);
                            switch (item.noteLocation)
                            {
                                case "Konularım":
                                    sColor.Background = new BrushConverter().ConvertFrom("#FD7E14") as SolidColorBrush;
                                    break;

                                case "Bölüm":

                                    sColor.Background = new BrushConverter().ConvertFrom("#6610F2") as SolidColorBrush;
                                    break;

                                case "Ayet":

                                    sColor.Background = new BrushConverter().ConvertFrom("#0DCAF0") as SolidColorBrush;
                                    break;

                                default:
                                    sColor.Background = new BrushConverter().ConvertFrom("#ADB5BD") as SolidColorBrush;
                                    break;
                            }

                            var sName = (TextBlock)FindName("ntName" + i);
                            sName.Text = item.noteHeader;

                            var sCreated = (TextBlock)FindName("ntCreate" + i);
                            sCreated.Text = item.created.ToString("D");

                            var sBtnGo = (Button)FindName("ntBtnGo" + i);
                            sBtnGo.Uid = item.notesId.ToString();

                            var sBtnDel = (Button)FindName("ntBtnDel" + i);
                            sBtnDel.Uid = item.notesId.ToString();

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
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
            }
        }

        // -------------  Load Func ------------- //

        // -------------  Click Func ------------- //

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
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
                    filterText = (string)btn.Content;
                    searchText = "";
                    SearchData.Text = "";
                }

                App.loadTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
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
            try
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
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void addNewNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_noteAddPopup.IsOpen = true;
                noteType.Text = "Kullanıcı Notu";
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
                                    if (entitydb.Notes.Where(p => p.noteHeader == noteName.Text && p.noteLocation == "Kullanıcı").FirstOrDefault() != null)
                                    {
                                        App.mainScreen.alertFunc("İşlem Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen notu ismini kontrol edip yeniden deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                    }
                                    else
                                    {
                                        var dNotes = new Notes { noteHeader = noteName.Text, noteDetail = noteDetail.Text, sureId = 0, verseId = 0, modify = DateTime.Now, created = DateTime.Now, noteLocation = "Kullanıcı" };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        App.mainScreen.succsessFunc("İşlem Başarılı", "Notunuz başarılı bir sekilde eklenmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp.Uid);
                pp_moveBar.IsOpen = false;

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void gotoNotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                listBorder.Visibility = Visibility.Hidden;
                App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void deleteNotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                popup_deleteNote.IsOpen = true;
                selectedId = int.Parse(btn.Uid);
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void deleteNotePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.notesId == selectedId));

                    entitydb.ResultItems.RemoveRange(entitydb.ResultItems.Where(p => p.resultNoteId == selectedId));

                    entitydb.SaveChanges();

                    foreach (var item in entitydb.Results)
                    {
                        if (entitydb.ResultItems.Where(p => p.resultId == item.resultId).Count() == 0) item.resultNotes = false;
                    }

                    entitydb.SaveChanges();

                    popup_deleteNote.IsOpen = false;
                    App.loadTask = Task.Run(loadItem);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Changed", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
            }
        }

        // ---------------- Changed Func ---------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
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
                this.Dispatcher.Invoke(() =>
                {
                    SearchBtn.IsEnabled = true;
                    addNewNote.IsEnabled = true;
                    filterButton.IsEnabled = true;
                    filterAll.IsEnabled = true;
                    filterUser.IsEnabled = true;
                    filterSubject.IsEnabled = true;

                    filterVerse.IsEnabled = true;
              
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //

        // ----------- Popuper Spec Func ----------- //

        public void popuverMove_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            ppMoveConfing((string)btn.Uid);
            moveControlName.Text = (string)btn.Content;
            pp_moveBar.IsOpen = true;
        }

        public void ppMoveActionOfset_Click(object sender, RoutedEventArgs e)
        {
            var btntemp = sender as Button;
            var movePP = (Popup)FindName((string)btntemp.Content);

            switch (btntemp.Uid.ToString())
            {
                case "Left":
                    movePP.HorizontalOffset -= 50;
                    break;

                case "Top":
                    movePP.VerticalOffset -= 50;
                    break;

                case "Bottom":
                    movePP.VerticalOffset += 50;
                    break;

                case "Right":
                    movePP.HorizontalOffset += 50;
                    break;

                case "UpLeft":
                    movePP.Placement = PlacementMode.Absolute;
                    movePP.VerticalOffset = 0;
                    movePP.HorizontalOffset = 0;
                    break;

                case "Reset":
                    movePP.Placement = PlacementMode.Center;
                    movePP.VerticalOffset = 0;
                    movePP.HorizontalOffset = 0;
                    break;

                case "Close":
                    pp_moveBar.IsOpen = false;
                    break;
            }
        }

        public void ppMoveConfing(string ppmove)
        {
            for (int i = 1; i < 8; i++)
            {
                var btn = FindName("pp_M" + i) as Button;
                btn.Content = ppmove;
            }
        }
    }
}