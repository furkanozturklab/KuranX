using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.VerseF;
using KuranX.App.Core.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Diagnostics;
using System.Threading;

namespace KuranX.App.Core.Pages.NoteF
{
    /// <summary>
    /// Interaction logic for NoteItem.xaml
    /// </summary>
    public partial class NoteItem : Page
    {
        private int noteId, cSr, cVr, cPd, cSb;
        private bool tempCheck = false;

        private string gototype;

        public NoteItem()
        {
            InitializeComponent();
        }

        public Page PageCall(int id)
        {
            noteId = id;
            App.loadTask = Task.Run(() => loadItem());
            return this;
        }

        public void loadScreen(int id)
        {
            this.Dispatcher.Invoke(() =>
            {
                PdfViewer dViewer = new PdfViewer(id);
                dViewer.Show();
            });
        }

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                var dNote = entitydb.Notes.Where(p => p.notesId == noteId).FirstOrDefault();

                loadAni();
                this.Dispatcher.Invoke(() =>
                {
                    infoText.Text = "";
                    loadHeader.Text = dNote.noteHeader;
                    loadCreate.Text = dNote.created.ToString("D");
                    loadLocation.Text = dNote.noteLocation;
                    loadNoteDetail.Text = dNote.noteDetail;
                    switch (dNote.noteLocation)
                    {
                        case "Konularım":
                            noteType.Background = new BrushConverter().ConvertFrom("#FD7E14") as SolidColorBrush;
                            break;

                        case "Kütüphane":
                            noteType.Background = new BrushConverter().ConvertFrom("#E33FA1") as SolidColorBrush;
                            break;

                        case "Ayet":
                            noteType.Background = new BrushConverter().ConvertFrom("#0DCAF0") as SolidColorBrush;
                            break;

                        case "PDF":
                            noteType.Background = new BrushConverter().ConvertFrom("#B30B00") as SolidColorBrush;
                            break;

                        default:
                            noteType.Background = new BrushConverter().ConvertFrom("#ADB5BD") as SolidColorBrush;
                            break;
                    }

                    // Kullanıcı Notu Gelmiş İse
                    gotoVerseButton.IsEnabled = false;
                    gotoVerseButton.Content = "Kullanıcı";
                    gotoVerseButton.Tag = "People";

                    App.mainScreen.navigationWriter("notes", "Kullanıcı Notu");

                    // Sure Bağlanmış
                    if (dNote.sureId != 0)
                    {
                        gotoVerseButton.IsEnabled = true;
                        gotoVerseButton.Content = "Ayete Git";
                        gotoVerseButton.Tag = "ArrowRight";
                        var dSure = entitydb.Sure.Where(p => p.sureId == dNote.sureId).FirstOrDefault();
                        App.mainScreen.navigationWriter("notes", "Sure Notu");
                        infoText.Text = "Not Aldığınız Ayet " + Environment.NewLine + dSure.name + " suresini " + dNote.verseId + " ayeti";
                        cSr = (int)dSure.sureId;
                        cVr = (int)dNote.verseId;
                        gototype = "Verse";
                    }

                    // PDF Bağlanmış
                    if (dNote.pdfFileId != 0)
                    {
                        gotoVerseButton.IsEnabled = true;
                        gotoVerseButton.Content = "Pdf e Git";
                        gotoVerseButton.Tag = "ArrowRight";
                        gotoVerseButton.Style = (Style)FindResource("defaultActionButonBstrpRed");
                        App.mainScreen.navigationWriter("notes", "Pdf Notu");
                        var dPdf = entitydb.PdfFile.Where(p => p.pdfFileId == dNote.pdfFileId).FirstOrDefault();
                        infoText.Text = "Not Aldığınız Dosya " + Environment.NewLine + dPdf.fileName;
                        cPd = (int)dNote.pdfFileId;
                        gototype = "Pdf";
                    }

                    // Konu Bağlanmış
                    if (dNote.subjectId != 0)
                    {
                        gotoVerseButton.IsEnabled = true;
                        gotoVerseButton.Content = "Konuya Git";
                        gotoVerseButton.Tag = "ArrowRight";

                        App.mainScreen.navigationWriter("notes", "Konu Notu");
                        var dSubject = entitydb.SubjectItems.Where(p => p.subjectItemsId == dNote.subjectId).FirstOrDefault();

                        Debug.WriteLine(dSubject.subjectId);

                        var dx = entitydb.Subject.Where(p => p.subjectId == dSubject.subjectId).FirstOrDefault();
                        infoText.Text = "Not Aldığınız Konu" + Environment.NewLine + dx.subjectName;
                        cSb = (int)dNote.subjectId;
                        gototype = "Subject";
                    }
                });
                Thread.Sleep(300);
                loadAniComplated();
            }
        }

        // ------------- Click Func ------------- //

        private void BackButton_Click(object sender, RoutedEventArgs e)
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

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (gototype)
                {
                    case "Verse":
                        App.mainframe.Content = App.navVersePage.PageCall(cSr, cVr, "Notes");
                        break;

                    case "Pdf":

                        App.loadTask = Task.Run(() => loadScreen(cPd));

                        break;

                    case "Subject":

                        using (var entitydbsb = new AyetContext())
                        {
                            var dSubjectItems = entitydbsb.SubjectItems.Where(p => p.subjectItemsId == cSb).FirstOrDefault();
                            App.mainframe.Content = App.navSubjectItem.subjectItemsPageCall((int)dSubjectItems.subjectId, (int)dSubjectItems.sureId, (int)dSubjectItems.verseId);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("gotoVerseButton_Click", ex);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_DeleteConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("deleteButton_Click", ex);
            }
        }

        private void sendResult_Click(object sender, RoutedEventArgs e)
        {
            popup_sendResultItems.IsOpen = true;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    if (loadNoteDetail.Text.Length < 8)
                    {
                        alertFunc("Güncelleme Başarısız", "Yeni notunuz çok kısa minimum 8 karakter olmalıdır.", 3);
                    }
                    else
                    {
                        entitydb.Notes.Where(p => p.notesId == noteId).First().noteDetail = loadNoteDetail.Text;
                        entitydb.SaveChanges();
                        succsessFunc("Güncelleme Başarılı", "Notunuz başarılı bir sekilde güncellendi.", 3);
                        saveButton.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("saveButton_Click", ex);
            }
        }

        private void deleteNotePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    BackButton.IsEnabled = false;
                    gotoVerseButton.IsEnabled = false;
                    sendResult.IsEnabled = false;
                    printButton.IsEnabled = false;
                    deleteButton.IsEnabled = false;
                    saveButton.IsEnabled = false;
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.notesId == noteId));
                    entitydb.SaveChanges();
                    popup_DeleteConfirm.IsOpen = false;
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
                App.timeSpan.Interval = TimeSpan.FromSeconds(3);
                App.timeSpan.Start();
                succsessFunc("Not Silme Başarılı", "Notunuz başaralı bir sekilde silinmiştir. Notlarım sayfasına yönlendiriliyorsunuz...", 3);
                App.timeSpan.Tick += delegate
                {
                    App.timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("voidgobacktimer", ex);
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

        private void connectResultControl_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                var item = popupResultSureId.SelectedItem as ComboBoxItem;
                var dResult = entitydb.Results.Where(p => p.resultId == int.Parse(item.Uid)).FirstOrDefault();

                if (entitydb.ResultItems.Where(p => p.resultId == dResult.resultId && p.resultNoteId == noteId).Count() == 0)
                {
                    dResult.resultNotes = true;
                    var dTemp = new ResultItem { resultId = dResult.resultId, resultNoteId = noteId, sendTime = DateTime.Now };
                    entitydb.ResultItems.Add(dTemp);
                    entitydb.SaveChanges();
                    popup_sendResultItems.IsOpen = false;
                    succsessFunc("Gönderme Başarılı", "Notunuz " + item.Content + " suresinin sonucuna gönderildi.", 3);
                }
                else
                {
                    popup_sendResultItems.IsOpen = false;
                    alertFunc("Gönderme Başarısız", "Notunuz " + item.Content + " suresinin sonucuna daha önceden eklenmiştir yeniden ekleyemezsiniz.", 3);
                }
                /*

                */
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
                output_Document.Write(App.notePrinter.notePrinterCall(noteId));
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

        // ------------- Click Func ------------- //

        // ------------- Changed Func ------------- //

        private void loadNoteDetail_TextChanged(object sender, TextChangedEventArgs e)
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

        // ------------- Changed Func ------------- //

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

        // ---------- Animation Func -------------- //

        private void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadNoteDetail.Visibility = Visibility.Hidden;
                loadHeaderStack.Visibility = Visibility.Hidden;
                controlBar.Visibility = Visibility.Hidden;
            });
        }

        private void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadNoteDetail.Visibility = Visibility.Visible;
                loadHeaderStack.Visibility = Visibility.Visible;

                controlBar.Visibility = Visibility.Visible;
            });
        }
    }
}