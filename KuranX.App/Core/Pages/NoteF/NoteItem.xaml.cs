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
        private Notes dNotes, tempNotes = new Notes();
        private Task loadTask;
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private int selectedId, cSr, cVr, cPd, cSb;
        private bool tempCheck = false;
        private string gototype;

        public NoteItem()
        {
            InitializeComponent();
        }

        public NoteItem(int notesid) : this()
        {
            selectedId = notesid;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadTask = new Task(noteLoad);
            loadTask.Start();
        }

        private void noteLoad()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                dNotes = entitydb.Notes.Where(p => p.NotesId == selectedId).FirstOrDefault();

                this.Dispatcher.Invoke(() =>
                {
                    gotoVerseButton.Visibility = Visibility.Collapsed;
                    header.Text = dNotes.NoteHeader;
                    create.Text = dNotes.Created.ToString();
                    location.Text = dNotes.NoteLocation;
                    noteDetail.Text = dNotes.NoteDetail;

                    // Sure Bağlanmış
                    if (dNotes.SureId != 0)
                    {
                        gotoVerseButton.Visibility = Visibility.Visible;
                        gotoVerseButton.Content = "Ayete Git";

                        var dSure = entitydb.Sure.Where(p => p.sureId == dNotes.SureId).FirstOrDefault();

                        infoText.Text = "Not Aldığınız Ayet " + Environment.NewLine + dSure.Name + " suresini " + dNotes.VerseId + " ayeti";
                        cSr = (int)dSure.sureId;
                        cVr = (int)dNotes.VerseId;
                        gototype = "Verse";
                    }

                    // PDF Bağlanmış
                    if (dNotes.PdfFileId != 0)
                    {
                        gotoVerseButton.Visibility = Visibility.Visible;
                        gotoVerseButton.Content = "Pdf e Git";
                        var dPdf = entitydb.PdfFile.Where(p => p.PdfFileId == dNotes.PdfFileId).FirstOrDefault();
                        infoText.Text = "Not Aldığınız Dosya " + Environment.NewLine + dPdf.FileName;
                        cPd = (int)dNotes.PdfFileId;
                        gototype = "Pdf";
                    }

                    // Konu Bağlanmış
                    if (dNotes.SubjectId != 0)
                    {
                        gotoVerseButton.Visibility = Visibility.Visible;
                        gotoVerseButton.Content = "Konuya Git";
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

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
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

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            Printer_Func(this);
        }

        public void Printer_Func(FrameworkElement element)
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

        private void gotoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteNotepopup.IsOpen = true;
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
                App.logWriter("Remove", ex);
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
                App.logWriter("TimeSpan", ex);
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
            this.Dispatcher.Invoke(() =>
            {
                loadinGifContent.Visibility = Visibility.Visible;
            });
        }

        private void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                loadinGifContent.Visibility = Visibility.Collapsed;
                loadBorder.Visibility = Visibility.Visible;
            });
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
                App.logWriter("ChangeText", ex);
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
                App.logWriter("Save", ex);
            }
        }
    }
}