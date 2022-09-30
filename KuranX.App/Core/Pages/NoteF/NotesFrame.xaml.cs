using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
    /// Interaction logic for NotesFrame.xaml
    /// </summary>
    ///

    public class Dstack
    {
        public int Count { get; set; }
        public string NoteLocation { get; set; }
        public string Bg { get; set; }
        public string Fr { get; set; }
    }

    public partial class NotesFrame : Page
    {
        private bool searchStatus = false, filter;
        private string searchTxt, filterTxt;
        private List<Notes> dNotes, tempNotes = new List<Notes>();
        private int lastNotesItem = 0, totalcount, selectedId, NowPage = 1;
        private Task loadTask;
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private List<Dstack> dstack = new List<Dstack>();

        public NotesFrame()
        {
            InitializeComponent();
        }

        public void notesItemsLoad()
        {
            using (var entitydb = new AyetContext())
            {
                Debug.WriteLine("filer : " + filter);
                loadAni();
                if (filter)
                {
                    Debug.WriteLine("filer true : " + filter);
                    if (searchStatus) dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.NoteHeader, "%" + searchTxt + "%")).Where(p => p.NoteLocation == filterTxt).OrderByDescending(p => p.Created).Skip(lastNotesItem).Take(29).ToList();
                    else dNotes = entitydb.Notes.Where(p => p.NoteLocation == filterTxt).OrderByDescending(p => p.Created).Skip(lastNotesItem).Take(29).ToList();
                }
                else
                {
                    Debug.WriteLine("filer false : " + filter);
                    Debug.WriteLine("SeachStatus : " + searchStatus);
                    if (searchStatus) dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.NoteHeader, "%" + searchTxt + "%")).Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").OrderByDescending(p => p.Created).Skip(lastNotesItem).Take(29).ToList();
                    else dNotes = entitydb.Notes.Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").OrderByDescending(p => p.Created).Skip(lastNotesItem).Take(29).ToList();
                }

                var stackP = entitydb.Notes.Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").OrderByDescending(p => p.Created).GroupBy(p => p.NoteLocation).Select(p => new Dstack { NoteLocation = p.Key, Count = p.Count(), Bg = p.Key }).ToList();
                totalcount = entitydb.Notes.Where(p => p.NoteLocation == "Konularım" || p.NoteLocation == "Kütüphane" || p.NoteLocation == "PDF" || p.NoteLocation == "Ayet" || p.NoteLocation == "Kullanıcı").Count();

                this.Dispatcher.Invoke(() =>
                {
                    listBorder.Visibility = Visibility.Visible;
                    stackBorder.Visibility = Visibility.Collapsed;

                    for (int x = 1; x < 25; x++)
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("nt" + x);
                        itemslist.ItemsSource = null;
                    }

                    for (int x = 1; x < 4; x++)
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("nts" + x);
                        itemslist.ItemsSource = null;
                    }

                    int i = 0;

                    foreach (var item in stackP)
                    {
                        dstack.Add(item);
                        switch (dstack[0].NoteLocation)
                        {
                            case "Konularım":
                                dstack[0].Bg = "#FD7E14";
                                dstack[0].Fr = "#FFF";
                                break;

                            case "Kütüphane":
                                dstack[0].Bg = "#E33FA1";
                                dstack[0].Fr = "#FFF";
                                break;

                            case "Ayet":
                                dstack[0].Bg = "#0DCAF0";
                                dstack[0].Fr = "#FFF";
                                break;

                            case "PDF":
                                dstack[0].Bg = "#B30B00";
                                dstack[0].Fr = "#FFF";
                                break;

                            default:
                                dstack[0].Bg = "#ADB5BD";
                                dstack[0].Fr = "#FFF";
                                break;
                        }
                        ItemsControl itemslist = (ItemsControl)this.FindName("nts" + i);
                        itemslist.ItemsSource = dstack;
                        dstack.Clear();
                        i++;
                    }

                    i = 1;
                    foreach (var item in dNotes)
                    {
                        tempNotes.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("nt" + i);
                        switch (tempNotes[0].NoteLocation)
                        {
                            case "Konularım":
                                tempNotes[0].NoteLocation = "#FD7E14";
                                break;

                            case "Kütüphane":
                                tempNotes[0].NoteLocation = "#E33FA1";
                                break;

                            case "Ayet":
                                tempNotes[0].NoteLocation = "#0DCAF0";
                                break;

                            case "PDF":
                                tempNotes[0].NoteLocation = "#B30B00";
                                break;

                            default:
                                tempNotes[0].NoteLocation = "#ADB5BD";
                                break;
                        }
                        itemslist.ItemsSource = tempNotes;
                        tempNotes.Clear();
                        i++;

                        if (i == 25) break; // 29 den fazla varmı kontrol etmek için koydum
                    }

                    if (lastNotesItem == 0) previusPageButton.IsEnabled = false;
                    else previusPageButton.IsEnabled = true;

                    if (dNotes.Count() <= 24) nextpageButton.IsEnabled = false;
                    if (lastNotesItem == 0 && dNotes.Count() > 24) nextpageButton.IsEnabled = true;

                    totalcountText.Tag = totalcount.ToString();

                    nowPageStatus.Tag = NowPage + " / " + Math.Ceiling(decimal.Parse(totalcount.ToString()) / 24).ToString();
                });
            }
            Thread.Sleep(200);
            loadAniComplated();
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

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SearchData.Text.Length >= 3)
                {
                    searchStatus = true;
                    searchTxt = SearchData.Text;

                    loadTask = new Task(notesItemsLoad);
                    loadTask.Start();
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchStatus = false;
                        loadTask = new Task(notesItemsLoad);
                        loadTask.Start();
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
                App.logWriter("Search", ex);
            }
        }

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
                App.logWriter("TextVis", ex);
            }
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            listview.IsChecked = true;
            stackview.IsChecked = false;

            if ((string)btn.Content == "Hepsi")
            {
                filter = false;
            }
            else
            {
                filter = true;
                filterTxt = btn.Content.ToString();
            }

            loadTask = new Task(notesItemsLoad);
            loadTask.Start();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadTask = new Task(notesItemsLoad);
            loadTask.Start();
        }

        private void gotoNotes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            App.mainframe.Content = new NoteItem(int.Parse(btn.Uid));
            listBorder.Visibility = Visibility.Collapsed;
            stackBorder.Visibility = Visibility.Collapsed;
        }

        private void deleteNotes_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            deleteNotepopup.IsOpen = true;
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
                    deleteNotepopup.IsOpen = false;
                    loadTask = new Task(notesItemsLoad);
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Remove", ex);
            }
        }

        private void addNewNote_Click(object sender, RoutedEventArgs e)
        {
            noteAddPopup.IsOpen = true;
            noteType.Text = "Kullanıcı Notu";
        }

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                listBorder.Visibility = Visibility.Hidden;
                stackBorder.Visibility = Visibility.Hidden;
                loadinItemsGifContent.Visibility = Visibility.Visible;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                addNewNote.IsEnabled = true;
                filterButton.IsEnabled = true;
                filterUser.IsEnabled = true;
                filterAll.IsEnabled = true;
                filterSubject.IsEnabled = true;
                filterLib.IsEnabled = true;
                filterVerse.IsEnabled = true;
                filterPdf.IsEnabled = true;

                loadHeaderLeftAni.Visibility = Visibility.Visible;
                loadHeaderRightAni.Visibility = Visibility.Visible;
                loadFilterAni.Visibility = Visibility.Visible;
                loadControlAni.Visibility = Visibility.Visible;

                loadinGifContent.Visibility = Visibility.Collapsed;
                loadinItemsGifContent.Visibility = Visibility.Collapsed;
            });
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

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (noteName.Text.Length <= 8)
                {
                    noteAddPopupHeaderError.Visibility = Visibility.Visible;
                    noteName.Focus();
                    noteAddPopupHeaderError.Content = "Not Başlığı Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                }
                else
                {
                    if (noteDetail.Text.Length <= 8)
                    {
                        noteAddPopupDetailError.Visibility = Visibility.Visible;
                        noteDetail.Focus();
                        noteAddPopupDetailError.Content = "Not Başlığı Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                    }
                    else
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dNotes = new Notes { NoteHeader = noteName.Text, NoteDetail = noteDetail.Text, SureId = 0, VerseId = 0, Modify = DateTime.Now, Created = DateTime.Now, NoteLocation = "Kullanıcı" };
                            entitydb.Notes.Add(dNotes);
                            entitydb.SaveChanges();
                            succsessFunc("Not Ekleme Başarılı", "Notunuz Eklenmiştir.", 3);
                            noteName.Text = "";
                            noteDetail.Text = "";
                            loadTask = new Task(notesItemsLoad);
                            loadTask.Start();
                        }
                        noteAddPopup.IsOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
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

        private void infoFunc(string header, string detail, int timespan)
        {
            try
            {
                infoPopupHeader.Text = header;
                infoPopupDetail.Text = detail;
                inph.IsOpen = true;

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    inph.IsOpen = false;
                    timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
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
                Debug.WriteLine("Liste");
                listBorder.Visibility = Visibility.Visible;
                stackBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                Debug.WriteLine("Yığın");
                listBorder.Visibility = Visibility.Collapsed;
                stackBorder.Visibility = Visibility.Visible;
            }
            hoverPopup.IsOpen = false;
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

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);

                noteName.Text = "";
                noteDetail.Text = "";

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
                lastNotesItem += 20;
                NowPage++;
                loadTask = new Task(notesItemsLoad);
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
                if (lastNotesItem >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastNotesItem -= 20;
                    NowPage--;
                    loadTask = new Task(notesItemsLoad);
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