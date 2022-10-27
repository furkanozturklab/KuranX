using KuranX.App.Core.Classes;
using Org.BouncyCastle.Asn1;
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

namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectItem.xaml
    /// </summary>
    public partial class SubjectItemFrame : Page
    {
        private List<SubjectItems> dSubjectItems = new List<SubjectItems>();
        private List<Verse> dVerseItems = new List<Verse>();
        private List<Notes> dTempNotes = new List<Notes>();
        private List<Interpreter>? dInterpreter = new List<Interpreter>();
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private int currentSubjectItemsId, tempDataInt = 0, activeSure, selectedNoteId;
        private Task? PageItemLoadTask;
        private string InterpreterWrite;
        private bool tempCheck = false;

        public SubjectItemFrame()
        {
            InitializeComponent();
        }

        public SubjectItemFrame(int subjectItemsId, string subjectName, string subjectFolderName, string create, SolidColorBrush bgcolor) : this()
        {
            loadHeader.Text = subjectFolderName;
            loadCreated.Text = create;
            loadBackHeader.Text = subjectName;
            currentSubjectItemsId = subjectItemsId;
            loadBgColor.Background = bgcolor;
        }

        private void loadSubjectItem()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();

                    dSubjectItems = entitydb.SubjectItems.Where(p => p.SubjectItemsId == currentSubjectItemsId).ToList();
                    dVerseItems = entitydb.Verse.Where(p => p.RelativeDesk == dSubjectItems[0].VerseId).Where(p => p.SureId == dSubjectItems[0].SureId).ToList();

                    tempDataInt = (int)dVerseItems[0].VerseId;
                    tempDataInt--;
                    activeSure = (int)dVerseItems[0].SureId;

                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();

                    this.Dispatcher.Invoke(() =>
                    {
                        versesFullTextData.ItemsSource = dInterpreter;
                        versesTextData.ItemsSource = dVerseItems;
                    });
                    Thread.Sleep(200);
                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PageItemLoadTask = new Task(loadSubjectItem);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void FullTextClear()
        {
            try
            {
                versesFullTextData.Visibility = Visibility.Collapsed;
                versesTrDataExtends.Visibility = Visibility.Collapsed;
                versesArDataExtends.Visibility = Visibility.Collapsed;
                backInterpreter.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                App.logWriter("TextVis", ex);
            }
        }

        private void trFullTextLoad(object sender, EventArgs e)
        {
            try
            {
                FullTextClear();
                versesTrDataExtends.Visibility = Visibility.Visible;
                versesTrDataExtendsText.Text = dVerseItems[0].VerseTr;
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void arabicFullTextLoad(object sender, EventArgs e)
        {
            try
            {
                FullTextClear();
                versesArDataExtends.Visibility = Visibility.Visible;
                versesArDataExtendsText.Text = dVerseItems[0].VerseArabic;
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

        private void noteConnect()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dNotes = (List<Notes>)entitydb.Notes.Where(p => p.SubjectId == currentSubjectItemsId).Where(p => p.NoteLocation == "Konularım").ToList();
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

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addNewConnect.IsOpen = true;
                PageItemLoadTask = new Task(noteConnect);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("NoteButton", ex);
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
                    var dnotes = entitydb.Notes.Where(p => p.NotesId == int.Parse(tmpbutton.Uid)).Select(p => new Notes() { NoteHeader = p.NoteHeader, NoteDetail = p.NoteDetail, NotesId = p.NotesId }).FirstOrDefault();
                    selectedNoteId = dnotes.NotesId;
                    meaningDetailTextHeader.Text = dnotes.NoteHeader;
                    noteOpenDetailText.Text = dnotes.NoteDetail;
                    valDetail.Text = dnotes.NoteDetail;
                    valHeader.Text = dnotes.NoteHeader;
                }
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

        private void noteSubjectAddButtonP_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                noteAddPopup.IsOpen = true;
                noteType.Text = "Konularım Notu";
            }
            catch (Exception ex)
            {
                App.logWriter("Popup", ex);
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
                        noteAddPopupDetailError.Content = "Not İçeriği Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                    }
                    else
                    {
                        if (noteDetail.Text.Length >= 3000)
                        {
                            noteAddPopupDetailError.Visibility = Visibility.Visible;
                            noteDetail.Focus();
                            noteAddPopupDetailError.Content = "Not İçeriği Maximum 3000 karakterden fazla olamaz.";
                        }
                        else
                        {
                            using (var entitydb = new AyetContext())
                            {
                                var dcontrol = entitydb.Notes.Where(p => p.NoteHeader == noteName.Text).Where(p => p.NoteLocation == "Konularım").Where(p => p.VerseId == dVerseItems[0].RelativeDesk).Where(p => p.SureId == dVerseItems[0].SureId).ToList();

                                if (dcontrol.Count == 0)
                                {
                                    Notes dNotes = new()
                                    {
                                        SubjectId = dSubjectItems[0].SubjectItemsId,
                                        NoteHeader = noteName.Text,
                                        NoteDetail = noteDetail.Text,
                                        VerseId = dVerseItems[0].RelativeDesk,
                                        SureId = dVerseItems[0].SureId,
                                        Modify = DateTime.Now,
                                        Created = DateTime.Now,
                                        NoteLocation = "Konularım",
                                        NoteStatus = "#6610F2"
                                    };
                                    entitydb.Notes.Add(dNotes);
                                    entitydb.SaveChanges();
                                    succsessFunc("Not Ekleme Başarılı", "Ayet Konularıma Not Eklendiniz.", 3);
                                    noteName.Text = "";
                                    noteDetail.Text = "";
                                    PageItemLoadTask = new Task(noteConnect);
                                    PageItemLoadTask.Start();
                                }
                                else
                                {
                                    noteName.Text = "";
                                    noteDetail.Text = "";
                                    alertFunc("Ekleme Başarısız", "Bu Not Başlığı ile daha önceden Eklenmiştir.", 3);
                                }
                            }
                            noteAddPopup.IsOpen = false;
                        }
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

        private void openVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine((int)dSubjectItems[0].VerseId);
                Debug.WriteLine((int)dSubjectItems[0].SureId);

                App.mainframe.Content = new VerseF.stickVerseFrame((int)dSubjectItems[0].SureId, (int)dSubjectItems[0].VerseId, "0", "0", "Hidden");
            }
            catch (Exception ex)
            {
                App.logWriter("Frame", ex);
            }
        }

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                notesAllShowPopup.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Notes.Where(p => p.SubjectId == currentSubjectItemsId).ToList();

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
                App.logWriter("NoteDetal", ex);
            }
        }

        private void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadHeaderAni.Visibility = Visibility.Hidden;
                    loadDetailAni.Visibility = Visibility.Hidden;
                    loadControlAni.Visibility = Visibility.Hidden;
                    loadinGifContent.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadHeaderAni.Visibility = Visibility.Visible;
                    loadDetailAni.Visibility = Visibility.Visible;
                    loadControlAni.Visibility = Visibility.Visible;
                    loadinGifContent.Visibility = Visibility.Collapsed;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void noteOpenDetailLibSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addLibPopup.IsOpen = true;
                selectedLib.Text = meaningDetailTextHeader.Text;
                libFolderLoad();
            }
            catch (Exception ex)
            {
                App.logWriter("PopupOpen", ex);
            }
        }

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
                        Debug.WriteLine(selectedNoteId);
                        if (entitydb.Notes.Where(p => p.NotesId == selectedNoteId && p.LibraryId != 0).Count() != 0)
                        {
                            alertFunc("Kütüphane Ekleme Başarısız", "Bu Not Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            Debug.WriteLine(item.Uid);
                            //entitydb.Notes.Where(p => p.NotesId == selectedNoteId).FirstOrDefault().LibraryId = int.Parse(item.Uid);
                            //entitydb.SaveChanges();
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

        private void sendlibbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("DÜZELTİLECEK ALAN ! NOTELİBHEADER");
                /*
                if (libHeaderName.Text.Length <= 8)
                {
                    libHeaderNameError.Visibility = Visibility.Visible;
                    noteName.Focus();
                    libHeaderNameError.Content = "Başlık Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                }
                else
                {
                    using (var entitydb = new AyetContext())
                    {
                        var dcontrol = entitydb.Notes.Where(p => p.NoteHeader == valHeader.Text).Where(p => p.NoteLocation == "Kütüphane").Where(p => p.VerseId == dVerseItems[0].RelativeDesk).Where(p => p.SureId == dVerseItems[0].SureId).ToList();

                        if (dcontrol.Count == 0)
                        {
                            Notes dNotes = new()
                            {
                                SubjectId = dSubjectItems[0].SubjectItemsId,
                                NoteHeader = valHeader.Text,
                                NoteDetail = valDetail.Text,
                                VerseId = dVerseItems[0].RelativeDesk,
                                SureId = dVerseItems[0].SureId,
                                NoteLibHeader = libHeaderName.Text,
                                Modify = DateTime.Now,
                                Created = DateTime.Now,
                                NoteLocation = "Kütüphane",
                                NoteStatus = "#6610F2"
                            };
                            entitydb.Notes.Add(dNotes);
                            entitydb.SaveChanges();
                            succsessFunc("Kütüphaneye Eklendi", "Notunuz kütüphanedeki notlarım kısmına eklendi", 3);
                            libHeaderName.Text = "";
                            PageItemLoadTask = new Task(noteConnect);
                            PageItemLoadTask.Start();
                        }
                        else
                        {
                            libHeaderName.Text = "";
                            alertFunc("Ekleme Başarısız", "Bu Not daha önceden Eklenmiştir.", 3);
                        }
                    }
                    libSendNotePopup.IsOpen = false;
                }
                */
            }
            catch (Exception ex)
            {
                App.logWriter("SendLib", ex);
            }
        }

        private void libHeaderName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                libHeaderNameError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void interpreterWriterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = interpreterWriterCombo.SelectedItem as ComboBoxItem;

                InterpreterWrite = item.Content.ToString();

                if (tempCheck)
                {
                    PageItemLoadTask.Dispose();
                    PageItemLoadTask = new Task(loadInterpreter);
                    PageItemLoadTask.Start();
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("SelectEvent", ex);
            }
        }

        public void loadInterpreter()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();

                    versesFullTextData.Dispatcher.Invoke(() =>
                    {
                        versesFullTextData.Visibility = Visibility.Visible;
                        versesTrDataExtends.Visibility = Visibility.Collapsed;
                        versesArDataExtends.Visibility = Visibility.Collapsed;
                        backInterpreter.Visibility = Visibility.Collapsed;
                        versesFullTextData.ItemsSource = dInterpreter;
                    });
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void wordText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wordDetailPopup.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dWords = entitydb.Words.Where(p => p.SureId == dVerseItems[0].SureId).Where(p => p.VerseId == dVerseItems[0].VerseId).ToList();

                    dynamicWordDetail.Children.Clear();
                    foreach (var item in dWords)
                    {
                        StackPanel itemsStack = new StackPanel();
                        TextBlock text = new TextBlock();
                        TextBlock re = new TextBlock();

                        itemsStack.Style = (Style)FindResource("wordStack");
                        text.Style = (Style)FindResource("wordDetail");
                        re.Style = (Style)FindResource("wordDetail");

                        text.Text = item.WordText;
                        re.Text = item.WordRe;

                        itemsStack.Children.Add(text);
                        itemsStack.Children.Add(re);

                        dynamicWordDetail.Children.Add(itemsStack);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void backFullTextLoad(object sender, EventArgs e)
        {
            try
            {
                FullTextClear();
                versesFullTextData.Visibility = Visibility.Visible;
                versesArDataExtendsText.Text = dInterpreter[0].interpreterDetail;
                backInterpreter.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }
    }
}