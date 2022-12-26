using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.NoteF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string currentfileUrl = "";
        public int currentPdfId, selectedNoteId;

        public PdfViewer()
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

        public PdfViewer(int fileId) : this()
        {
            try
            {
                currentPdfId = fileId;
                App.loadTask = Task.Run(() => loadItem(currentPdfId));
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void loadItem(int id)
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void noteConnect()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.pdfFileId == currentPdfId).Where(p => p.noteLocation == "Kütüphane").ToList();

                    int i = 1;

                    for (int x = 1; x <= 7; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sbItem = (Button)FindName("nd" + x);
                            sbItem.Visibility = Visibility.Hidden;
                        });
                    }

                    foreach (var item in dNotes)
                    {
                        if (i == 8)
                        {
                            this.Dispatcher.Invoke(() => allShowNoteButton.Visibility = Visibility.Visible);

                            break;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            var sButton = (Button)FindName("nd" + i);
                            sButton.Content = item.noteHeader;
                            sButton.Uid = item.notesId.ToString();

                            var sbItem = (Button)FindName("nd" + i);
                            sbItem.Visibility = Visibility.Visible;
                            i++;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void libFolderLoad()
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // -------------- Click Func -------------- //

        private void libraryFileDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_deletePdf.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void deletePdfPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    browser.Dispose();
                    File.Delete(entitydb.PdfFile.Where(p => p.pdfFileId == currentPdfId).First().fileUrl);
                    entitydb.PdfFile.RemoveRange(entitydb.PdfFile.Where(p => p.pdfFileId == currentPdfId));
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.pdfFileId == currentPdfId));
                    entitydb.SaveChanges();
                    popup_deletePdf.IsOpen = false;
                    this.Close();
                    App.mainframe.Content = App.navLibraryFileFrame.PageCall();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
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
                App.logWriter("Click", ex);
            }
        }

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_notesAllShowPopup.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
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
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
            try
            {
                popup_FolderLibraryPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void noteAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_noteAddPopup.IsOpen = true;
                noteConnectVerse.Text = loadHeader.Text;
                noteType.Text = "PDF Notu";
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
                App.logWriter("Click", ex);
            }
        }

        private async void backButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task closeTask = Task.Run(() =>
                {
                    this.Dispatcher.Invoke(() => browser.Dispose());
                });

                await closeTask;

                this.Dispatcher.Invoke(() => this.Close());
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // add library func

        private void addfolderlibrary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (libraryFolderHeader.Text.Length >= 3)
                {
                    if (libraryFolderHeader.Text.Length < 150)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Librarys.Where(p => p.libraryName == librarypreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dlibraryFolder = new Classes.Library { libraryName = librarypreviewName.Text, libraryColor = librarypreviewColor.Background.ToString(), created = DateTime.Now, modify = DateTime.Now };
                                entitydb.Librarys.Add(dlibraryFolder);
                                entitydb.SaveChanges();
                                App.mainScreen.succsessFunc("İşlem Başarılı", " Yeni kütüphane başlığı oluşturuldu artık veri ekleye bilirsiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                                librarypreviewName.Text = "";
                                libraryFolderHeader.Text = "";
                                popup_FolderLibraryPopup.IsOpen = false;
                                dlibraryFolder = null;

                                App.loadTask = Task.Run(() => libFolderLoad());
                            }
                            else
                            {
                                App.mainScreen.alertFunc("İşlem Başarısız", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                            }
                            dControl = null;
                        }
                    }
                    else
                    {
                        libraryFolderHeader.Focus();
                        libraryHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                        libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığı çok uzun max 150 karakter olabilir";
                    }
                }
                else
                {
                    libraryFolderHeader.Focus();
                    libraryHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığının uzunluğu minimum 3 karakter olmalı";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                            App.mainScreen.alertFunc("İşlem Başarısız", "Bu not daha önceden kütüphaneye eklenmiş yeniden ekleyemezsiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        }
                        else
                        {
                            entitydb.Notes.Where(p => p.notesId == selectedNoteId).FirstOrDefault().libraryId = int.Parse(item.Uid);
                            entitydb.SaveChanges();
                            App.mainScreen.succsessFunc("İşlem Başarılı", "Seçmiş olduğunuz not kütüphaneye eklendi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
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
                App.logWriter("Click", ex);
            }
        }

        // new note add

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
                                    if (entitydb.Notes.Where(p => p.noteHeader == noteName.Text).Where(p => p.noteLocation == "PDF").FirstOrDefault() != null)
                                    {
                                        App.mainScreen.alertFunc("İşlem Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
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
                                            noteLocation = "Kütüphane",
                                        };

                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();

                                        App.mainScreen.succsessFunc("İşlem Başarılı", loadHeader.Text + " Pdf e notunuz başarılı bir sekilde eklendi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                browser.Dispose();
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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

                librarypreviewColor.Background = new BrushConverter().ConvertFromString((string)chk.Tag) as SolidColorBrush;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
            }
        }

        private void noteName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9a-zA-Z-ğüşöçıİĞÜŞÖÇ']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        // ---------------- Changed Func ---------------- //

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