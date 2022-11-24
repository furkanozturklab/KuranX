using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.NoteF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Window
    {
        public string currentfileUrl;
        public int currentPdfId, selectedNoteId;

        public PdfViewer()
        {
            InitializeComponent();
        }

        public PdfViewer(int fileId) : this()
        {
            currentPdfId = fileId;
            App.loadTask = Task.Run(() => loadItem(currentPdfId));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void loadItem(int id)
        {
            using (var entitydb = new AyetContext())
            {
                var dPdfFile = entitydb.PdfFile.Where(p => p.pdfFileId == id).FirstOrDefault();
                this.Dispatcher.Invoke(() =>
                {
                    this.Title = (string)dPdfFile.fileName;
                    loadHeader.Text = dPdfFile.fileName;
                    loadCreated.Text = dPdfFile.created.ToString("D");
                    browser.Source = new Uri(@dPdfFile.fileUrl);
                });
            }
        }

        private void noteConnect()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.pdfFileId == currentPdfId).Where(p => p.noteLocation == "PDF").ToList();
                    var dTempNotes = new List<Notes>();
                    int i = 1;

                    for (int x = 1; x < 8; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var itemslist = (ItemsControl)FindName("nd" + x);
                            itemslist.ItemsSource = null;
                        });
                    }

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
                            var dControlTemp = (ItemsControl)FindName("nd" + i);
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

        private void libFolderLoad()
        {
            using (var entitydb = new AyetContext())
            {
                var dLibFolder = entitydb.Librarys.ToList();

                this.Dispatcher.Invoke(() =>
                {
                    selectedLibFolder.Items.Clear();
                    foreach (var item in dLibFolder)
                    {
                        var cmbitem = new ComboBoxItem();

                        cmbitem.Content = item.libraryName;
                        cmbitem.Uid = item.libraryId.ToString();
                        selectedLibFolder.Items.Add(cmbitem);
                    }
                });
            }
        }

        // -------------- Click Func -------------- //

        private void libraryFileDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void noteDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? tmpbutton = sender as Button;
                popup_noteDetailText.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dnotes = entitydb.Notes.Where(p => p.notesId == int.Parse(tmpbutton.Uid)).Select(p => new Notes() { noteHeader = p.noteHeader, noteDetail = p.noteDetail, notesId = p.notesId }).FirstOrDefault();

                    selectedNoteId = dnotes.notesId;
                    noteDetailTextHeader.Text = dnotes.noteHeader;
                    noteOpenDetailText.Text = dnotes.noteDetail;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            popup_notesAllShowPopup.IsOpen = true;

            using (var entitydb = new AyetContext())
            {
                Debug.WriteLine(currentPdfId);
                var dPdf = entitydb.Notes.Where(p => p.pdfFileId == currentPdfId).ToList();

                foreach (var item in dPdf)
                {
                    StackPanel itemsStack = new StackPanel();
                    TextBlock headerText = new TextBlock();
                    TextBlock noteText = new TextBlock();
                    Button allshowButton = new Button();
                    Separator sp = new Separator();

                    itemsStack.Style = (Style)FindResource("defaultdynamicItemStackpanel");
                    headerText.Style = (Style)FindResource("defaultdynamicItemTextHeader");
                    noteText.Style = (Style)FindResource("defaultdynamicItemTextNote");
                    allshowButton.Style = (Style)FindResource("defaultdynamicItemShowButton");
                    sp.Style = (Style)FindResource("defaultdynamicItemShowSperator");

                    headerText.Text = item.noteHeader.ToString();
                    noteText.Text = item.noteDetail.ToString();
                    allshowButton.Uid = item.notesId.ToString();
                    allshowButton.Content = item.noteDetail.ToString();

                    allshowButton.Click += notesDetailPopup_Click;

                    itemsStack.Children.Add(headerText);
                    itemsStack.Children.Add(noteText);
                    itemsStack.Children.Add(allshowButton);
                    itemsStack.Children.Add(sp);

                    notesAllShowPopupStackPanel.Children.Add(itemsStack);
                }
            }
        }

        private void notesDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_noteDetailText.IsOpen = true;

                Button btn = sender as Button;
                noteOpenDetailText.Text = btn.Content.ToString();
            }
            catch (Exception ex)
            {
                App.logWriter("NoteDetail", ex);
            }
        }

        private void noteOpenDetailTextBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_noteDetailText.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void newLibFolder_Click(object sender, RoutedEventArgs e)
        {
            popup_FolderLibraryPopup.IsOpen = true;
        }

        private void noteAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_noteAddPopup.IsOpen = true;
                noteConnectVerse.Text = loadHeader.Text;
                noteType.Text = "Kütüphane Notu";
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
                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);
                popuptemp.IsOpen = false;

                noteName.Text = "";
                noteDetail.Text = "";

                libraryFolderHeader.Text = "";
                librarypreviewName.Text = "Önizleme";

                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.loadTask = Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        browser.Dispose();
                        this.Close();
                    });
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Navigation", ex);
            }
        }

        // add library func

        private void addfolderlibrary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (libraryFolderHeader.Text.Length >= 8)
                {
                    if (libraryFolderHeader.Text.Length < 50)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Librarys.Where(p => p.libraryName == librarypreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dlibraryFolder = new Classes.Library { libraryName = librarypreviewName.Text, libraryColor = librarypreviewColor.Background.ToString(), created = DateTime.Now, modify = DateTime.Now };
                                entitydb.Librarys.Add(dlibraryFolder);
                                entitydb.SaveChanges();
                                succsessFunc("Kütüphane Başlığı ", " Yeni kütüphane başlığı oluşturuldu artık veri ekleye bilirsiniz.", 3);

                                librarypreviewName.Text = "";
                                libraryFolderHeader.Text = "";
                                popup_FolderLibraryPopup.IsOpen = false;
                                dlibraryFolder = null;

                                App.loadTask = Task.Run(() => libFolderLoad());
                            }
                            else
                            {
                                alertFunc("Kütüphane Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                            }
                            dControl = null;
                        }
                    }
                    else
                    {
                        libraryFolderHeader.Focus();
                        libraryHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                        libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığı çok uzun max 50 karakter olabilir";
                    }
                }
                else
                {
                    libraryFolderHeader.Focus();
                    libraryHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığının uzunluğu minimum 8 karakter olmalı";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        // send library func

        private void noteOpenDetailLibSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_SendNote.IsOpen = true;
                selectedLib.Text = noteDetailTextHeader.Text;

                App.loadTask = Task.Run(() => libFolderLoad());
            }
            catch (Exception ex)
            {
                App.logWriter("PopupOpen", ex);
            }
        }

        // send lirary add func

        private void addLib_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var item = selectedLibFolder.SelectedItem as ComboBoxItem;

                    if (item != null)
                    {
                        if (entitydb.Notes.Where(p => p.notesId == selectedNoteId && p.libraryId != 0).Count() != 0)
                        {
                            alertFunc("Kütüphane Ekleme Başarısız", "Bu Not Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            entitydb.Notes.Where(p => p.notesId == selectedNoteId).FirstOrDefault().libraryId = int.Parse(item.Uid);
                            entitydb.SaveChanges();
                            succsessFunc("Kütüphane Ekleme Başarılı", "Seçmiş olduğunuz not kütüphaneye eklendi.", 3);
                            popup_SendNote.IsOpen = false;
                        }
                    }
                    else
                    {
                        popupaddLibError.Visibility = Visibility.Visible;
                        popupaddLibError.Text = "Lütfen Kütüphaneyi Seçiniz";
                        selectedLibFolder.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        // new note add

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (noteName.Text.Length <= 8)
                {
                    noteAddPopupHeaderError.Visibility = Visibility.Visible;
                    noteName.Focus();
                    noteAddPopupHeaderError.Content = "Not Başlığı Yeterince Uzun Değil. Min 8 Karakter Olmalıdır.";
                }
                else
                {
                    if (noteName.Text.Length > 50)
                    {
                        noteAddPopupHeaderError.Visibility = Visibility.Visible;
                        noteName.Focus();
                        noteAddPopupHeaderError.Content = "Not Başlığı Çok Uzun. Max 50 Karakter Olabilir.";
                    }
                    else
                    {
                        if (noteDetail.Text.Length <= 8)
                        {
                            noteAddPopupDetailError.Visibility = Visibility.Visible;
                            noteDetail.Focus();
                            noteAddPopupDetailError.Content = "Not İçeriği Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
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
                                    if (entitydb.Notes.Where(p => p.noteHeader == noteName.Text).Where(p => p.noteLocation == "PDF").FirstOrDefault() != null)
                                    {
                                        alertFunc("Not Ekleme Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", 3);
                                    }
                                    else
                                    {
                                        Notes dNotes = new()
                                        {
                                            noteHeader = noteName.Text,
                                            noteDetail = noteDetail.Text,
                                            pdfFileId = currentPdfId,
                                            modify = DateTime.Now,
                                            created = DateTime.Now,
                                            noteLocation = "PDF",
                                        };

                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();

                                        succsessFunc("Ekleme Başarılı", loadHeader.Text + " Pdf e Not Eklendiniz.", 3);

                                        App.loadTask = Task.Run(() => noteConnect());

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

        // note open popup

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_Note.IsOpen = true;
                App.loadTask = Task.Run(() => noteConnect());
            }
            catch (Exception ex)
            {
                App.logWriter("PopupOpen", ex);
            }
        }

        // -------------- Click Func -------------- //

        // ---------- Simple && Clear Func ---------- //

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

        // ---------- Simple && Clear Func ---------- //

        // ---------------- Changed Func ---------------- //

        private void libraryFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                libraryHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void libraryFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                librarypreviewName.Text = libraryFolderHeader.Text;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void libraryColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox? chk;

                foreach (object item in libraryColorStack.Children)
                {
                    chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                chk = sender as CheckBox;

                chk.IsChecked = true;

                librarypreviewColor.Background = new BrushConverter().ConvertFromString(chk.Tag.ToString()) as SolidColorBrush;
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

        private void Window_Closed(object sender, EventArgs e)
        {
            browser.Dispose();
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
    }
}