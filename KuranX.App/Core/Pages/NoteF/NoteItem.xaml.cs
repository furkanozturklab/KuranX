using KuranX.App.Core.Classes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;
using System.Threading;
using KuranX.App.Core.Classes.Tools;
using KuranX.App.Core.Classes.Helpers;
using System.Globalization;

namespace KuranX.App.Core.Pages.NoteF
{
    /// <summary>
    /// Interaction logic for NoteItem.xaml
    /// </summary>
    public partial class NoteItem : Page
    {
        private int noteId, cSr, cVr, cPd, cSb, gototypeId, cBr, conpage = 1;
        private bool tempCheck = false;
        private string gototype = "", getLocation;
        private Task noteframetask , secondtask;
        private DraggablePopupHelper drag;

        public NoteItem()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} Initıalıze] -> NoteItem");
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall(int id, string location = "", int nowpage = 1)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} PageCall] -> NoteItem");
                noteId = id;
                saveButton.IsEnabled = false;
              



                if (location != "")
                {
                    secondtask = Task.Run(() => loadItem());
                    App.secondFrame.Visibility = Visibility.Visible;
      
                }
                else{
                    noteframetask = Task.Run(() => loadItem());
                }



                getLocation = location;
                conpage = nowpage;
                App.lastlocation = "NoteItem";
                return this;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading Func", ex);
                return this;
            }
        }

        // ------------- Loading Func ------------- //

        public void loadScreen(int id)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        public void loadItem()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadItem] -> NoteItem");
                using (var entitydb = new AyetContext())
                {
                    var dNote = entitydb.Notes.Where(p => p.notesId == noteId).FirstOrDefault();

                    loadAni();
                    this.Dispatcher.Invoke(() =>
                    {
                        infoText.Text = "";
                        loadHeader.Text = dNote.noteHeader;
                        loadCreate.Text = dNote.created.ToString("D", new CultureInfo("tr-TR"));
                        loadLocation.Text = dNote.noteLocation;
                        loadNoteDetail.Text = dNote.noteDetail;

                        switch (dNote.noteLocation)
                        {
                            case "Konularım":
                                noteType.Background = new BrushConverter().ConvertFrom("#FD7E14") as SolidColorBrush;
                                break;

                            case "Bölüm":
                                noteType.Background = new BrushConverter().ConvertFrom("#6610F2") as SolidColorBrush;
                                break;

                            case "Ayet":
                                noteType.Background = new BrushConverter().ConvertFrom("#0DCAF0") as SolidColorBrush;
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


                        // Konu Bağlanmış
                        if (dNote.subjectId != 0)
                        {
                            gotoVerseButton.IsEnabled = true;
                            gotoVerseButton.Content = "Konuya Git";
                            gotoVerseButton.Tag = "ArrowRight";

                            App.mainScreen.navigationWriter("notes", "Konu Notu");
                            var dSubject = entitydb.SubjectItems.Where(p => p.subjectItemsId == dNote.subjectId).FirstOrDefault();

                            var dx = entitydb.Subject.Where(p => p.subjectId == dSubject.subjectId).FirstOrDefault();
                            infoText.Text = "Not Aldığınız Konu" + Environment.NewLine + dx.subjectName;
                            cSb = (int)dNote.subjectId;
                            gototype = "Subject";
                        }

                        // Sure Bağlanmış
                        if (dNote.sureId != 0)
                        {
                            gotoVerseButton.IsEnabled = true;
                            gotoVerseButton.Content = "Ayete Git";
                            gotoVerseButton.Tag = "ArrowRight";
                            var dSure = entitydb.Sure.Where(p => p.sureId == dNote.sureId).FirstOrDefault();
                            App.mainScreen.navigationWriter("notes", "Sure Notu");
                            infoText.Text = "Not Aldığınız Ayet " + Environment.NewLine + dSure.name + " suresinin " + dNote.verseId + " ayeti";
                            cSr = (int)dSure.sureId;
                            cVr = (int)dNote.verseId;
                            gototype = "Verse";

                            if (getLocation == "VerseNoteOpen" || getLocation == "VerseNoteDetail")
                            {
                                gotoVerseButton.Visibility = Visibility.Collapsed;
                                BackButton.SetValue(Grid.ColumnSpanProperty, 2);
                                BackButton.Width = 400;
                            }
                            else
                            {
                                gotoVerseButton.Visibility = Visibility.Visible;
                                BackButton.SetValue(Grid.ColumnSpanProperty, 1);
                                BackButton.Width = 150;

                            }
                          
                        }


                        // Bölüm Bağlanmış
                        if (dNote.sectionId != 0)
                        {
                          
                            gotoVerseButton.IsEnabled = true;
                            gotoVerseButton.Content = "Bölüme Git";
                            gotoVerseButton.Tag = "ArrowRight";
                            var dSure = entitydb.Sure.Where(p => p.sureId == dNote.sureId).FirstOrDefault();
                            App.mainScreen.navigationWriter("notes", "Bölüm Notu");
                            infoText.Text = "Not Aldığınız Bölüm " + Environment.NewLine + dSure.name + " suresinin " + dNote.sectionId + " bölümü";
                            cSr = (int)dSure.sureId;
                            cBr = (int)dNote.sectionId;
                            gototype = "Section";

                            if (getLocation == "SectionNote" || getLocation == "SectionNoteDetail")
                            {
                                gotoVerseButton.Visibility = Visibility.Collapsed;
                                BackButton.SetValue(Grid.ColumnSpanProperty, 2);
                                BackButton.Width = 400;
                            }
                            else
                            {
                                gotoVerseButton.Visibility = Visibility.Visible;
                                BackButton.SetValue(Grid.ColumnSpanProperty, 1);
                                BackButton.Width = 150;

                            }
                        }
                    });


                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));



                    this.Dispatcher.Invoke(() =>
                    {

                        BackButton.IsEnabled = true;
                        printButton.IsEnabled = true;
                        deleteButton.IsEnabled = true;
                        saveButton.IsEnabled = false;

                    });


                    loadAniComplated();

                   

                    this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading Func", ex);
            }
        }

        public void Printer_Func(FrameworkElement element)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} Printer_Func ] -> NoteItem");

                if (File.Exists("print_previw.xps") == true) File.Delete("print_previw.xps");
                XpsDocument? doc = new XpsDocument("print_previw.xps", FileAccess.ReadWrite);

                XpsDocumentWriter? writer = XpsDocument.CreateXpsDocumentWriter(doc);
                SerializerWriterCollator? output_Document = writer.CreateVisualsCollator();
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
                Tools.logWriter("Loading Func", ex);
            }
        }

        // ------------- Loading Func ------------- //

        // ------------- Click Func ------------- //

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} BackButton_Click ] -> NoteItem");

               

                switch (getLocation)
                {
                    case "VerseNoteOpen":

                        App.secondFrame.Visibility = Visibility.Hidden;
                        App.mainframe.Visibility = Visibility.Visible;
                        App.navVersePage.popup_Note.IsOpen = true;

                        break;
                    case "VerseNoteDetail":
                        App.secondFrame.Visibility = Visibility.Hidden;
                        App.navVersePage.popup_Note.IsOpen = true;
                        App.navVersePage.popup_notesAllShowPopup.IsOpen = true;
                        break;
                    case "SectionNote":
                        App.secondFrame.Visibility = Visibility.Hidden;
                        App.navSectionPage.popup_Note.IsOpen = true;
                        break;
                    case "SectionNoteDetail":
                        App.secondFrame.Visibility = Visibility.Hidden;
                        App.navSectionPage.popup_Note.IsOpen = true;
                        App.navSectionPage.popup_notesAllShowPopup.IsOpen = true;

                        break;
                    case "subjectDetail":
                        App.secondFrame.Visibility = Visibility.Hidden;
                        App.navSubjectItem.noteConnect();
                        App.navSubjectItem.popup_Note.IsOpen = true;
                        break;

                    default:
                        App.mainframe.Content = App.navNotesPage.PageCall(1);
                        break;
                }

            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} gotoVerseButton_Click ] -> NoteItem");
                switch (gototype)
                {
                    case "Verse":

                        App.secondFrame.Content = App.navVerseStickPage.PageCall(cSr, cVr, loadHeader.Text, 0, "Note");
                        App.secondFrame.Visibility = Visibility.Visible;

                        break;

                    case "Section":
                        App.mainframe.Content = App.navSectionPage.PageCall(cSr, cBr, "Note");
                        break;

                    case "Subject":

                        using (var entitydbsb = new AyetContext())
                        {
                            var dSubjectItems = entitydbsb.SubjectItems.Where(p => p.subjectItemsId == cSb).FirstOrDefault();
                            App.mainframe.Content = App.navSubjectItem.PageCall((int)dSubjectItems.subjectId, (int)dSubjectItems.sureId, (int)dSubjectItems.verseId);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} deleteButton_Click ] -> NoteItem");

                // drag = new DraggablePopupHelper((Border)popup_DeleteConfirm.Child, popup_DeleteConfirm);
                PopupHelpers.load_drag(popup_DeleteConfirm);
                popup_DeleteConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} saveButton_Click ] -> NoteItem");
                using (var entitydb = new AyetContext())
                {
                    if (loadNoteDetail.Text.Length < 3)
                    {
                        App.mainScreen.alertFunc("İşlem Başarısız", "Yeni notunuz çok kısa minimum 3 karakter olmalıdır.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                    else
                    {
                        entitydb.Notes.Where(p => p.notesId == noteId).First().noteDetail = loadNoteDetail.Text;
                        entitydb.SaveChanges();
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Notunuz başarılı bir sekilde güncellendi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        saveButton.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void deleteNotePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} deleteNotePopupBtn_Click ] -> NoteItem");

                using (var entitydb = new AyetContext())
                {
                    BackButton.IsEnabled = false;
                    gotoVerseButton.IsEnabled = false;
                    printButton.IsEnabled = false;
                    deleteButton.IsEnabled = false;
                    saveButton.IsEnabled = false;
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.notesId == noteId));

             

                    entitydb.SaveChanges();
                    PopupHelpers.dispose_drag(popup_DeleteConfirm);
                    popup_DeleteConfirm.IsOpen = false;
                    App.mainScreen.succsessFunc("İşlem Başarılı", "Notunuz başaralı bir sekilde silinmiştir. Bir önceki sayfaya yönlendiriliyorsunuz...", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));




                    switch (getLocation)
                    {
                        case "VerseNoteOpen":

                            App.secondFrame.Visibility = Visibility.Collapsed;
                            App.navVersePage.noteConnect();
                            App.navVersePage.popup_Note.IsOpen = true;

                            break;
                        case "VerseNoteDetail":
                            App.secondFrame.Visibility = Visibility.Collapsed;
                            App.navVersePage.popup_Note.IsOpen = true;
                            App.navVersePage.popup_notesAllShowPopup.IsOpen = true;
                            break;
                        case "SectionNote":
                            App.secondFrame.Visibility = Visibility.Collapsed;
                            App.navSectionPage.noteConnect();
                            App.navSectionPage.popup_Note.IsOpen = true;
                            break;
                        case "SectionNoteDetail":
                            App.secondFrame.Visibility = Visibility.Collapsed;
                            App.navSectionPage.popup_Note.IsOpen = true;
                            App.navSectionPage.popup_notesAllShowPopup.IsOpen = true;

                            break;
                        case "subjectDetail":
                            App.secondFrame.Visibility = Visibility.Collapsed;
                            App.navSubjectItem.noteConnect();
                            App.navSubjectItem.popup_Note.IsOpen = true;
                            break;

                        default:
                         
                            App.mainframe.Content = App.navNotesPage.PageCall(conpage);
                            break;
                    }



                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} popupClosed_Click] -> NoteFrame");

                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

     

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} printButton_Click ] -> NoteItem");
                Printer_Func(this);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        // ------------- Click Func ------------- //

        // -------------- Timer Func -------------- //

        private void voidgobacktimer()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} voidgobacktimer ] -> NoteItem");
                App.timeSpan.Interval = TimeSpan.FromSeconds(3);
                App.timeSpan.Start();
                App.mainScreen.succsessFunc("İşlem Başarılı", "Notunuz başaralı bir sekilde silinmiştir. Notlarım sayfasına yönlendiriliyorsunuz...", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                App.timeSpan.Tick += delegate
                {
                    App.timeSpan.Stop();

                    App.mainframe.Content = App.navNotesPage.PageCall();

                    BackButton.IsEnabled = true;
                    gotoVerseButton.IsEnabled = true;
                    printButton.IsEnabled = true;
                    deleteButton.IsEnabled = true;
                    saveButton.IsEnabled = false;
                };
            }
            catch (Exception ex)
            {
                Tools.logWriter("TimeSpan", ex);
            }
        }

        // -------------- Timer Func -------------- //

        // ------------- Changed Func ------------- //

        private void loadNoteDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadNoteDetail_TextChanged ] -> NoteItem");
                if (tempCheck)
                {
                    saveButton.IsEnabled = true;
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        // ------------- Changed Func ------------- //

        // ---------- Animation Func -------------- //

        private void loadAni()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadAni ] -> NoteItem");
                this.Dispatcher.Invoke(() =>
                {
                    loadNoteDetail.Visibility = Visibility.Hidden;
                    loadHeaderStack.Visibility = Visibility.Hidden;
                    controlBar.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Animation", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadAniComplated ] -> NoteItem");
                this.Dispatcher.Invoke(() =>
                {
                    loadNoteDetail.Visibility = Visibility.Visible;
                    loadHeaderStack.Visibility = Visibility.Visible;
                    controlBar.Visibility = Visibility.Visible;

                });

            }
            catch (Exception ex)
            {
                Tools.logWriter("Animation", ex);
            }
        }
    }
}