using CefSharp.DevTools.Page;
using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.VerseF;
using KuranX.App.Core.Windows;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for libraryOpenFile.xaml
    /// </summary>
    public partial class libraryOpenEditorFile : Page
    {
        public int currentPdfId, tempSureID, tempVerseID;
        public string currentPdfUrl, currentFileName;
        private List<Notes> dTempNotes = new List<Notes>();
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private Task? PageItemLoadTask, PageNotesLoadTask;
        private bool tempCheck3;

        public libraryOpenEditorFile()
        {
            InitializeComponent();
        }

        public libraryOpenEditorFile(int fileId) : this()
        {
            currentPdfId = fileId;
        }

        public void loadPpf()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dPdfFile = entitydb.PdfFile.Where(p => p.PdfFileId == currentPdfId).FirstOrDefault();

                    this.Dispatcher.Invoke(() =>
                    {
                        currentPdfUrl = dPdfFile.FileUrl;
                        currentFileName = dPdfFile.FileName;

                        loadHeader.Text = (string)dPdfFile.FileName;
                        loadCreated.Text = dPdfFile.Created.ToString();
                        pdfviewer.Address = dPdfFile.FileUrl;
                    });
                }
                Thread.Sleep(200);
                loadAniCompleted();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void openFileWindowButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PdfViewer pdviewer = new PdfViewer(currentPdfUrl, currentFileName);
                pdviewer.Show();
            }
            catch (Exception ex)
            {
                App.logWriter("ShowPdf", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadAni();

                PageItemLoadTask = new Task(loadPpf);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
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

        private void noteDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? tmpbutton = sender as Button;
                noteConnectDetailText.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dnotes = entitydb.Notes.Where(p => p.NotesId == int.Parse(tmpbutton.Uid)).Select(p => new Notes() { NoteHeader = p.NoteHeader, NoteDetail = p.NoteDetail, VerseId = p.VerseId, SureId = p.SureId }).FirstOrDefault();

                    if (dnotes.SureId != 0)
                    {
                        var dVerse = entitydb.Sure.Where(p => p.sureId == dnotes.SureId).FirstOrDefault();
                        meaningOpenDetailTextConnect.Text = "Not " + dVerse.Name + " suresinin " + dnotes.VerseId + " inci ayeti ile işliklendirilmiştir.";
                        sureOpenClick.Visibility = Visibility.Visible;
                        tempSureID = (int)dnotes.SureId;
                        tempVerseID = (int)dnotes.VerseId;
                    }
                    else
                    {
                        sureOpenClick.Visibility = Visibility.Collapsed;
                        meaningOpenDetailTextConnect.Text = "";
                    }

                    meaningDetailTextHeader.Text = dnotes.NoteHeader;
                    noteOpenDetailText.Text = dnotes.NoteDetail;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void noteSubjectAddButtonP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                noteAddPopup.IsOpen = true;
                pdfConnectText.Text = currentFileName;
            }
            catch (Exception ex)
            {
                App.logWriter("OpenPopup", ex);
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

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pdfNoteName.Text.Length <= 8)
                {
                    noteAddPopupHeaderError.Visibility = Visibility.Visible;
                    pdfNoteName.Focus();
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
                            var dcontrol = entitydb.Notes.Where(p => p.NoteHeader == pdfNoteName.Text).Where(p => p.NoteLocation == "Editor").ToList();
                            Notes dNotes;

                            if (dcontrol.Count == 0)
                            {
                                if (meaningConnectVerse.Text != "")
                                {
                                    ComboBoxItem tmpcbxi = (ComboBoxItem)meaningpopupNextSureId.SelectedItem;

                                    if (Int16.Parse(tmpcbxi.Tag.ToString()) >= Int16.Parse(meaningConnectVerse.Text))
                                    {
                                        dNotes = new()
                                        {
                                            NoteHeader = pdfNoteName.Text,
                                            NoteDetail = noteDetail.Text,
                                            PdfFileId = currentPdfId,
                                            VerseId = int.Parse(meaningConnectVerse.Text),
                                            SureId = int.Parse(tmpcbxi.Uid),
                                            PdfPageId = int.Parse(pdfNotePage.Text),
                                            Modify = DateTime.Now,
                                            Created = DateTime.Now,
                                            NoteLocation = "Editor",
                                            NoteStatus = "#6610F2"
                                        };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        succsessFunc("Not Ekleme Başarılı", "Ayet Konularıma Not Eklendiniz.", 3);
                                        pdfNoteName.Text = "";
                                        pdfNotePage.Text = "";
                                        noteDetail.Text = "";
                                        PageNotesLoadTask = new Task(noteConnect);
                                        PageNotesLoadTask.Start();
                                        noteAddPopup.IsOpen = false;
                                    }
                                    else
                                    {
                                        meaningCountAddPopupHeaderError.Visibility = Visibility.Visible;
                                        meaningConnectVerse.Focus();
                                        meaningCountAddPopupHeaderError.Content = "Ayet Sınırı Geçtiniz.";
                                    }
                                }
                                else
                                {
                                    if (pdfNotePage.Text == "")
                                    {
                                        dNotes = new()
                                        {
                                            NoteHeader = pdfNoteName.Text,
                                            NoteDetail = noteDetail.Text,
                                            PdfFileId = currentPdfId,
                                            Modify = DateTime.Now,
                                            Created = DateTime.Now,
                                            NoteLocation = "Editor",
                                            NoteStatus = "#6610F2"
                                        };
                                    }
                                    else
                                    {
                                        dNotes = new()
                                        {
                                            NoteHeader = pdfNoteName.Text,
                                            NoteDetail = noteDetail.Text,
                                            PdfFileId = currentPdfId,
                                            PdfPageId = int.Parse(pdfNotePage.Text),
                                            Modify = DateTime.Now,
                                            Created = DateTime.Now,
                                            NoteLocation = "Editor",
                                            NoteStatus = "#6610F2"
                                        };
                                    }

                                    entitydb.Notes.Add(dNotes);
                                    entitydb.SaveChanges();
                                    succsessFunc("Not Ekleme Başarılı", "Ayet Konularıma Not Eklendiniz.", 3);
                                    pdfNoteName.Text = "";
                                    noteDetail.Text = "";
                                    pdfNotePage.Text = "";
                                    PageNotesLoadTask = new Task(noteConnect);
                                    PageNotesLoadTask.Start();
                                    noteAddPopup.IsOpen = false;
                                }
                            }
                            else
                            {
                                pdfNoteName.Text = "";
                                noteDetail.Text = "";
                                pdfNotePage.Text = "";
                                alertFunc("Ekleme Başarısız", "Bu Not Başlığı ile daha önceden Eklenmiştir.", 3);
                                noteAddPopup.IsOpen = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("AddNote", ex);
            }
        }

        private void noteConnect()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dNotes = (List<Notes>)entitydb.Notes.Where(p => p.PdfFileId == currentPdfId).Where(p => p.NoteLocation == "Editor").ToList();
                    int i = 1;
                    foreach (var item in dNotes)
                    {
                        if (i == 8)
                        {
                            allShowNoteButton.Dispatcher.Invoke(() =>
                            {
                                allShowNoteButton.Visibility = Visibility.Visible;
                            });

                            break;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            ItemsControl dControlTemp = (ItemsControl)this.FindName("mvc" + i);
                            dTempNotes.Add(item);
                            dControlTemp.ItemsSource = dTempNotes;
                            dTempNotes.Clear();
                        });

                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("NoteConnect", ex);
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

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                notesAllShowPopup.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Notes.Where(p => p.PdfFileId == currentPdfId).ToList();

                    foreach (var item in dInteg)
                    {
                        StackPanel itemsStack = new StackPanel();
                        TextBlock headerText = new TextBlock();
                        TextBlock noteText = new TextBlock();
                        Button allshowButton = new Button();
                        Separator sp = new Separator();

                        itemsStack.Style = (Style)FindResource("dynamicItemStackpanel");
                        headerText.Style = (Style)FindResource("dynamicItemTextHeader");
                        noteText.Style = (Style)FindResource("dynamicItemTextNote");
                        allshowButton.Style = (Style)FindResource("dynamicItemShowButton");
                        sp.Style = (Style)FindResource("dynamicItemShowSperator");

                        headerText.Text = item.NoteHeader.ToString();
                        noteText.Text = item.NoteDetail.ToString();
                        allshowButton.Uid = item.NotesId.ToString();
                        allshowButton.Content = item.NoteDetail.ToString();

                        allshowButton.Click += notesDetailPopup_Click;

                        itemsStack.Children.Add(headerText);
                        itemsStack.Children.Add(noteText);
                        itemsStack.Children.Add(allshowButton);
                        itemsStack.Children.Add(sp);

                        notesAllShowPopupStackPanel.Children.Add(itemsStack);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void notesDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                noteConnectDetailText.IsOpen = true;
                Button btn = sender as Button;
                noteOpenDetailText.Text = btn.Content.ToString();
            }
            catch (Exception ex)
            {
                App.logWriter("NoteDetailShow", ex);
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

        private void noteOpenDetailTextBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                noteConnectDetailText.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addNewConnect.IsOpen = true;
                PageNotesLoadTask = new Task(noteConnect);
                PageNotesLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("NoteShow", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);
                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void editorNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window editorWindow = new PdfEditorViewer(currentPdfUrl, currentFileName, currentPdfId);
                editorWindow.Show();
            }
            catch (Exception ex)
            {
                App.logWriter("NewWİndow", ex);
            }
        }

        private void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void sureOpenClick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new verseFrame(tempSureID, tempVerseID);
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void loadAniCompleted()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Collapsed;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void popupMeiningSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tempCheck3)
                {
                    var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                    attachSureVerseCountText.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
                }
                else tempCheck3 = true;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }
    }
}