using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.LibraryF;
using KuranX.App.Core.Pages.VerseF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace KuranX.App.Core.Pages.NoteF
{
    /// <summary>
    /// Interaction logic for NoteItem.xaml
    /// </summary>
    public partial class NoteItem : Page
    {
        private Notes dNotes = new Notes();
        private Task loadTask;
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private int selectedId, cSr, cVr, cPd, cSb;
        private bool tempCheck = false;
        private string gototype, navLocation;

        public NoteItem()
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

        public NoteItem(int notesid, string location) : this()
        {
            try
            {
                selectedId = notesid;
                navLocation = location;
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
                loadTask = new Task(noteLoad);
                loadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("Page_Loaded", ex);
            }
        }

        private void noteLoad()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    dNotes = entitydb.Notes.Where(p => p.NotesId == selectedId).FirstOrDefault();

                    this.Dispatcher.Invoke(() =>
                    {
                        gotoVerseButton.IsEnabled = false;
                        gotoVerseButton.Content = "Kullanıcı";
                        gotoVerseButton.Tag = "People";
                        header.Text = dNotes.NoteHeader;

                        create.Text = dNotes.Created.ToString();
                        location.Text = dNotes.NoteLocation;
                        noteDetail.Text = dNotes.NoteDetail;

                        switch (dNotes.NoteLocation)
                        {
                            case "Konularım":
                                noteType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FD7E14");
                                break;

                            case "Kütüphane":
                                noteType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#E33FA1");
                                break;

                            case "Ayet":
                                noteType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0DCAF0");
                                break;

                            case "PDF":
                                noteType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#B30B00");
                                break;

                            default:
                                noteType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ADB5BD");
                                break;
                        }

                        // Sure Bağlanmış
                        if (dNotes.SureId != 0)
                        {
                            gotoVerseButton.IsEnabled = true;
                            gotoVerseButton.Content = "Ayete Git";
                            gotoVerseButton.Tag = "ArrowRight";
                            var dSure = entitydb.Sure.Where(p => p.sureId == dNotes.SureId).FirstOrDefault();

                            infoText.Text = "Not Aldığınız Ayet " + Environment.NewLine + dSure.Name + " suresini " + dNotes.VerseId + " ayeti";
                            cSr = (int)dSure.sureId;
                            cVr = (int)dNotes.VerseId;
                            gototype = "Verse";
                        }

                        // PDF Bağlanmış
                        if (dNotes.PdfFileId != 0)
                        {
                            gotoVerseButton.IsEnabled = true;
                            gotoVerseButton.Content = "Pdf e Git";
                            gotoVerseButton.Tag = "ArrowRight";
                            var dPdf = entitydb.PdfFile.Where(p => p.PdfFileId == dNotes.PdfFileId).FirstOrDefault();
                            infoText.Text = "Not Aldığınız Dosya " + Environment.NewLine + dPdf.FileName;
                            cPd = (int)dNotes.PdfFileId;
                            gototype = "Pdf";
                        }

                        // Konu Bağlanmış
                        if (dNotes.SubjectId != 0)
                        {
                            gotoVerseButton.IsEnabled = true;
                            gotoVerseButton.Content = "Konuya Git";
                            gotoVerseButton.Tag = "ArrowRight";
                            var dSubject = entitydb.SubjectItems.Where(p => p.SubjectItemsId == dNotes.SubjectId).FirstOrDefault();
                            var dx = entitydb.Subject.Where(p => p.SubjectId == dSubject.SubjectId).FirstOrDefault();
                            infoText.Text = "Not Aldığınız Konu" + Environment.NewLine + dx.SubjectName;
                            cSb = (int)dNotes.SubjectId;
                            gototype = "Subject";
                        }
                    });
                    Thread.Sleep(200);
                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("noteLoad", ex);
            }
        }

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (gototype)
                {
                    case "Verse":

                        App.mainframe.Content = new verseFrame(cSr, cVr, "Notes");

                        break;

                    case "Pdf":
                        App.mainframe.Content = new libraryOpenFile(cPd);
                        break;

                    case "Subject":

                        using (var entitydbsb = new AyetContext())
                        {
                            var dSubjectItems = entitydbsb.SubjectItems.Where(p => p.SubjectItemsId == cSb).FirstOrDefault();

                            var dSubject = entitydbsb.Subject.Where(p => p.SubjectId == (int)dSubjectItems.SubjectId).FirstOrDefault();

                            App.mainframe.Content = new SubjectF.SubjectItemFrame(cSb, dSubjectItems.SubjectName, dSubject.SubjectName, dSubjectItems.Created.ToString(), (SolidColorBrush)new BrushConverter().ConvertFrom(dSubject.SubjectColor));
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("gotoVerseButton_Click", ex);
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
                App.logWriter("popupClosed_Click", ex);
            }
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Printer_Func(this);
            }
            catch (Exception ex)
            {
                App.logWriter("printButton_Click", ex);
            }
        }

        public void Printer_Func(FrameworkElement element)
        {
            try
            {
                if (File.Exists("print_previw.xps") == true) File.Delete("print_previw.xps");
                XpsDocument doc = new XpsDocument("print_previw.xps", FileAccess.ReadWrite);

                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                SerializerWriterCollator output_Document = writer.CreateVisualsCollator();
                output_Document.BeginBatchWrite();
                output_Document.Write(new NotePrinter(selectedId));
                output_Document.EndBatchWrite();

                FixedDocumentSequence preview = doc.GetFixedDocumentSequence();
                doc.Close();

                var dframe = new Frame();
                var dpage = new NotePrinter();
                var window = new Window();
                dpage.Content = new DocumentViewer { Document = preview };
                dframe.Content = dpage;

                window.Content = dframe;

                window.ShowDialog();

                writer = null;
                output_Document = null;
                doc = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Printer_Func", ex);
            }
        }

        private void gotoBackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                App.logWriter("gotoBackButton_Click", ex);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                deleteNotepopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("deleteButton_Click", ex);
            }
        }

        private void deleteNotePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    gotoBackButton.IsEnabled = false;
                    gotoVerseButton.IsEnabled = false;
                    sendResult.IsEnabled = false;
                    printButton.IsEnabled = false;
                    deleteButton.IsEnabled = false;
                    saveButton.IsEnabled = false;
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.NotesId == selectedId));
                    entitydb.SaveChanges();
                    deleteNotepopup.IsOpen = false;
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("deleteNotePopupBtn_Click", ex);
            }
        }

        private void voidgobacktimer()
        {
            try
            {
                timeSpan.Interval = TimeSpan.FromSeconds(3);
                timeSpan.Start();
                succsessFunc("Not Silme Başarılı", "Notunuz başaralı bir sekilde silinmiştir. Notlarım sayfasına yönlendiriliyorsunuz...", 3);
                timeSpan.Tick += delegate
                {
                    timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("voidgobacktimer", ex);
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
                App.logWriter("succsessFunc", ex);
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
                App.logWriter("loadAni", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
            {
                loadinGifContent.Visibility = Visibility.Collapsed;
                loadBorder.Visibility = Visibility.Visible;
            });
            }
            catch (Exception ex)
            {
                App.logWriter("loadAniComplated", ex);
            }
        }

        private void noteDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tempCheck)
                {
                    saveButton.IsEnabled = true;
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("noteDetail_TextChanged", ex);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dNotes.NoteDetail = noteDetail.Text;
                    entitydb.Notes.Update(dNotes);
                    entitydb.SaveChanges();
                    saveButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("saveButton_Click", ex);
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

        // --------------------------------------   SEND KÜTÜPHANE FİLES ------------------------------- //

        private void newLibFolder_Click(object sender, RoutedEventArgs e)
        {
            addFolderLibHeaderPopup.IsOpen = true;
        }

        private void addLib_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var item = selectedLibFolder.SelectedItem as ComboBoxItem;

                    if (item != null)
                    {
                        if (entitydb.Notes.Where(p => p.NotesId == selectedId && p.LibraryId != 0).Count() != 0)
                        {
                            alertFunc("Kütüphane Ekleme Başarısız", "Bu Not Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            entitydb.Notes.Where(p => p.NotesId == selectedId).FirstOrDefault().LibraryId = int.Parse(item.Uid);
                            entitydb.SaveChanges();
                            succsessFunc("Kütüphane Ekleme Başarılı", "Seçmiş olduğunuz not kütüphaneye eklendi.", 3);
                            addLibPopup.IsOpen = false;
                        }
                    }
                    else
                    {
                        popupaddLibError.Visibility = Visibility.Visible;
                        popupaddLibError.Text = "Lütfen Konuyu Seçiniz";
                        selectedLibFolder.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void libFolderLoad()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dLibFolder = entitydb.Librarys.ToList();

                    selectedLibFolder.Items.Clear();
                    foreach (var item in dLibFolder)
                    {
                        var cmbitem = new ComboBoxItem();

                        cmbitem.Content = item.LibraryName;
                        cmbitem.Uid = item.LibraryId.ToString();
                        selectedLibFolder.Items.Add(cmbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void libFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                libHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void libFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                libpreviewName.Text = libFolderHeader.Text;
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
                CheckBox chk;

                foreach (object item in libColorStack.Children)
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

                libpreviewColor.Background = new BrushConverter().ConvertFromString(chk.Tag.ToString()) as SolidColorBrush;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void addfolderLibraryHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (libFolderHeader.Text.Length >= 8)
                {
                    using (var entitydb = new AyetContext())
                    {
                        var dControl = entitydb.Librarys.Where(p => p.LibraryName == libpreviewName.Text).ToList();

                        if (dControl.Count == 0)
                        {
                            var dLibFolder = new Library { LibraryName = libpreviewName.Text, LibraryColor = libpreviewColor.Background.ToString(), Created = DateTime.Now, Modify = DateTime.Now };
                            entitydb.Librarys.Add(dLibFolder);
                            entitydb.SaveChanges();
                            succsessFunc("Kütphane Başlığı ", " Yeni kütüphane başlığı oluşturuldu artık veri ekleye bilirsiniz.", 3);
                            libFolderLoad();
                            libpreviewName.Text = "";
                            libFolderHeader.Text = "";
                            addFolderLibHeaderPopup.IsOpen = false;
                        }
                        else
                        {
                            alertFunc("Kütphane Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                        }
                    }
                }
                else
                {
                    libFolderHeader.Focus();
                    libHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    libHeaderFolderErrorMesssage.Content = "Kütphane başlığının uzunluğu minimum 8 karakter olmalı";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
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
                var item = popupResultSureId.SelectedItem as ComboBoxItem;
                var dResult = entitydb.Results.Where(p => p.ResultId == int.Parse(item.Uid)).FirstOrDefault();

                if (entitydb.ResultItems.Where(p => p.ResultId == dResult.ResultId && p.ResultNoteId == selectedId).Count() == 0)
                {
                    dResult.ResultNotes = "true";
                    var dTemp = new ResultItem { ResultId = dResult.ResultId, ResultNoteId = selectedId, SendTime = DateTime.Now };
                    entitydb.ResultItems.Add(dTemp);
                    entitydb.SaveChanges();
                    sendResultItemsPopup.IsOpen = false;
                    succsessFunc("Gönderme Başarılı", "Notunuz " + item.Content + " suresinin sonucuna gönderildi.", 3);
                }
                else
                {
                    sendResultItemsPopup.IsOpen = false;
                    alertFunc("Gönderme Başarısız", "Notunuz " + item.Content + " suresinin sonucuna daha önceden eklenmiştir yeniden ekleyemezsiniz.", 3);
                }
                /*

                */
            }
        }
    }
}