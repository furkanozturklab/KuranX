using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Helpers;
using KuranX.App.Core.Classes.Tools;
using System;
using System.Globalization;
using System.Linq;

using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Navigation;


namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectItem.xaml
    /// </summary>
    public partial class SubjectItem : Page, Movebar
    {
        public int sSureId, sverseId, verseId, subId, subItemsId;
        private string intelWriter = "";
        private Task subjectitem, subjectprocess;
        private string pp_selected;
        public DraggablePopupHelper drag;
        public SubjectItem()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} InitializeComponent ] -> SubjectItem");
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        public object PageCall(int SubID, int SureId, int VerseId)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} subjectItemsPageCall ] -> SubjectItem");

                subId = SubID;
                sSureId = SureId;
                verseId = VerseId;

                subjectitem = Task.Run(() => loadItem(SubID, SureId, VerseId));

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);

                App.lastlocation = "SubjectItem";
                return this;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
                return this;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} Page_Loaded ] -> SubjectItem");


                App.mainScreen.navigationWriter("subject", loadHeader.Text + "," + loadBackHeader.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        // ---------- Load FUNC ---------- //

        public void loadItem(int SubID, int SureId, int VerseId)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadItem ] -> SubjectItem");


                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    var dSub = entitydb.Subject.Where(p => p.subjectId == SubID).First();
                    subItemsId = entitydb.SubjectItems.Where(p => p.subjectId == subId).First().subjectItemsId;
                    var dSure = entitydb.Sure.Where(p => p.sureId == SureId).Select(p => new Sure { name = p.name }).First();
                    var dVerse = entitydb.Verse.Where(p => p.sureId == SureId && p.relativeDesk == VerseId).First();

                    this.Dispatcher.Invoke(() =>
                    {
                        loadBgColor.Background = new BrushConverter().ConvertFrom(dSub.subjectColor) as SolidColorBrush;
                        loadHeader.Text = dSub.subjectName;
                        loadCreated.Text = dSub.created.ToString("D", new CultureInfo("tr-TR"));
                        loadBackHeader.Text = $"{dSure.name} Suresinin {VerseId} Ayeti";

                        App.mainScreen.navigationWriter("subject", loadHeader.Text + "," + loadBackHeader.Text);

                        loadVerseTr.Text = dVerse.verseTr;
                        loadVerseArb.Text = dVerse.verseArabic;
                        sverseId = (int)dVerse.relativeDesk;
                        loadInterpreterFunc(App.InterpreterWriter, sverseId);
                    });
                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        public void loadInterpreterFunc(string writer, int verseId)
        {
            // İnterpreter Load Func

            try
            {
                Tools.errWrite($"[{DateTime.Now} loadInterpreterFunc ] -> SubjectItem");


                using (var entitydb = new AyetContext())
                {
                    switch (writer)
                    {
                        case "Ömer Çelik":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 0);
                            break;

                        case "Yorumcu 2":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 2);
                            break;

                        case "Yorumcu 3":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 3);
                            break;

                        case "Mehmet Okuyan":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 1);
                            break;

                        default:
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 0);

                            break;
                    }

                    this.Dispatcher.Invoke(() => loadDetail.Text = "");
                    this.Dispatcher.Invoke(() => tempLoadDetail.Text = "");


                    if (writer == "") writer = "Ömer Çelik";
                    var dInter = entitydb.Interpreter.Where(p => p.sureId == sSureId && p.verseId == verseId && p.interpreterWriter == writer).FirstOrDefault();

                    if (dInter != null)
                    {
                        App.InterpreterWriter = writer;
                        loadItemsInterpreter(dInter);
                    }

                    dInter = null;
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        public void loadItemsInterpreter(Interpreter dInter)
        {
            // items Load Func
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadItemsInterpreter ] -> SubjectItem");




                this.Dispatcher.Invoke(() =>
                {
                    loadDetail.Text = dInter.interpreterDetail;
                    tempLoadDetail.Text = dInter.interpreterDetail;
                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        public void noteConnect()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} noteConnect ] -> SubjectItem");


                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.sureId == sSureId && p.verseId == verseId && p.noteLocation == "Konularım").ToList();

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
                Tools.logWriter("Loading", ex);
            }
        }

        // ---------- Load FUNC ---------- //

        // ---------- CLİCK FUNC ---------- //

        private void openVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} openVerseButton_Click ] -> SubjectItem");



                App.secondFrame.Content = App.navVerseStickPage.PageCall(sSureId, verseId, loadHeader.Text, 0, "Subject");
                App.secondFrame.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void changeloadDetail_Click(object sender, EventArgs e)
        {
            // TR ARP İNTERPRETER TEXT CHANGE FUNC
            try
            {

                Tools.errWrite($"[{DateTime.Now} changeloadDetail_Click ] -> SubjectItem");

                var btn = sender as Button;
                switch (btn.Uid)
                {
                    case "tr":
                        loadDetail.Text = loadVerseTr.Text;
                        loadDetail.Style = (Style)FindResource("vrs_loadDetailTr");
                        backInterpreter.Visibility = Visibility.Visible;
                        break;

                    case "arp":
                        loadDetail.Text = loadVerseArb.Text;
                        loadDetail.Style = (Style)FindResource("vrs_loadDetailArp");
                        backInterpreter.Visibility = Visibility.Visible;
                        break;

                    case "inter":
                        loadDetail.Text = tempLoadDetail.Text;
                        loadDetail.Style = (Style)FindResource("vrs_loadDetailInterpreter");
                        backInterpreter.Visibility = Visibility.Collapsed;
                        break;
                }

                btn = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void wordText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} wordText_Click ] -> SubjectItem");

                PopupHelpers.load_drag(popup_Words);
                popup_Words.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dWords = entitydb.Words.Where(p => p.sureId == sSureId && p.verseId == verseId).ToList();

                    dynamicWordDetail.Children.Clear();

                    foreach (var item in dWords)
                    {
                        StackPanel itemsStack = new StackPanel();
                        TextBlock arp = new TextBlock();
                        TextBlock tr = new TextBlock();
                        TextBlock meal = new TextBlock();
                        TextBlock root = new TextBlock();

                        itemsStack.Style = (Style)FindResource("pp_wordStack");
                        arp.Style = (Style)FindResource("pp_wordDetailArp");
                        tr.Style = (Style)FindResource("pp_wordDetail");
                        meal.Style = (Style)FindResource("pp_wordDetail");
                        root.Style = (Style)FindResource("pp_wordDetailArp");

                        arp.Text = item.arp_read;
                        tr.Text = item.tr_read;
                        meal.Text = item.word_meal;
                        root.Text = item.root;

                        itemsStack.Children.Add(arp);
                        itemsStack.Children.Add(tr);
                        itemsStack.Children.Add(meal);
                        itemsStack.Children.Add(root);

                        dynamicWordDetail.Children.Add(itemsStack);
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
                Tools.errWrite($"[{DateTime.Now} popupClosed_Click ] -> SubjectItem");


            

                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp, pp_moveBar);
                btntemp = null;

                
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} backButton_Click ] -> SubjectItem");


                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} noteButton_Click ] -> SubjectItem");

                PopupHelpers.load_drag(popup_Note);
                popup_Note.IsOpen = true;
                subjectitem = Task.Run(() => noteConnect());
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void noteAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} noteAddButton_Click ] -> SubjectItem");

                PopupHelpers.load_drag(popup_noteAddPopup);
                popup_noteAddPopup.IsOpen = true;
                noteConnectVerse.Text = loadBackHeader.Text;
                noteType.Text = "Konu Notu";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void noteDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} noteDetailPopup_Click ] -> SubjectItem");


                var tmpbutton = sender as Button;


                App.secondFrame.Visibility = Visibility.Visible;
                PopupHelpers.dispose_drag(popup_Note);
                popup_Note.IsOpen = false;
                App.secondFrame.Content = App.navNoteItem.PageCall(int.Parse(tmpbutton.Uid), "subjectDetail");
                tmpbutton = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("PopupAction", ex);
            }
        }

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} addNoteButton_Click ] -> SubjectItem");


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
                                    if (entitydb.Notes.Where(p => p.noteHeader == noteName.Text && p.sureId == sSureId && p.verseId == verseId).FirstOrDefault() != null)
                                    {
                                        App.mainScreen.alertFunc("İşlem Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                    }
                                    else
                                    {
                                        var dNotes = new Notes { noteHeader = noteName.Text, noteDetail = noteDetail.Text, sureId = sSureId, verseId = verseId, modify = DateTime.Now, created = DateTime.Now, noteLocation = "Konularım", subjectId = subItemsId };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        App.mainScreen.succsessFunc("İşlem Başarılı", loadBackHeader.Text + " Not Eklendiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                        subjectprocess = Task.Run(noteConnect);

                                        dNotes = null;
                                    }

                                    noteName.Text = "";
                                    noteDetail.Text = "";
                                }
                                PopupHelpers.dispose_drag(popup_noteAddPopup);
                                popup_noteAddPopup.IsOpen = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} allShowNoteButton_Click ] -> SubjectItem");


                PopupHelpers.load_drag(popup_notesAllShowPopup);
                popup_notesAllShowPopup.IsOpen = true;



                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.sureId == sSureId && p.verseId == verseId && p.subjectId == subId).ToList();
                    foreach (var item in dNotes)
                    {
                        var itemsStack = new StackPanel();
                        var headerText = new TextBlock();
                        var noteText = new TextBlock();
                        var allshowButton = new Button();
                        var sp = new Separator();

                        itemsStack.Style = (Style)FindResource("pp_dynamicItemStackpanel");
                        headerText.Style = (Style)FindResource("pp_dynamicItemTextHeader");
                        noteText.Style = (Style)FindResource("pp_dynamicItemTextNote");
                        allshowButton.Style = (Style)FindResource("pp_dynamicItemShowButton");
                        sp.Style = (Style)FindResource("pp_dynamicItemShowSperator");

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

                        itemsStack = null;
                        headerText = null;
                        noteText = null;
                        allshowButton = null;
                        sp = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void notesDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} notesDetailPopup_Click ] -> SubjectItem");


                var tmpbutton = sender as Button;



                App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(tmpbutton.Uid));
                tmpbutton = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        // ---------- CLİCK FUNC ---------- //

        // ---------- Changed FUNC ---------- //

        private void interpreterWriterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Nav NextUpdate Click
            try
            {

                Tools.errWrite($"[{DateTime.Now} interpreterWriterCombo_SelectionChanged ] -> SubjectItem");


                var item = interpreterWriterCombo.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    if (item.Uid != "0")
                    {
                        intelWriter = (string)item.Tag;
                        loadInterpreterFunc((string)item.Tag, verseId);
                    }
                }
                item = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
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
                Tools.logWriter("Change", ex);
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
                Tools.logWriter("Change", ex);
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
                Tools.logWriter("Change", ex);
            }
        }

        // ---------- Changed FUNC ---------- //

        // ---------- Animation Func ----------- //

        private void loadAni()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadAni ] -> SubjectItem");


                this.Dispatcher.Invoke(() =>
                {
                    loadHeaderStack.Visibility = Visibility.Hidden;
                    loadBackHeader.Visibility = Visibility.Hidden;
                    loadControlAni.Visibility = Visibility.Hidden;
                    loadContentGrid.Visibility = Visibility.Hidden;
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

                Tools.errWrite($"[{DateTime.Now} loadAniComplated ] -> SubjectItem");
                this.Dispatcher.Invoke(() =>
                {
                    loadHeaderStack.Visibility = Visibility.Visible;
                    loadBackHeader.Visibility = Visibility.Visible;
                    loadControlAni.Visibility = Visibility.Visible;
                    loadContentGrid.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Animation", ex);
            }
        }

        // ----------- Popuper Spec Func ----------- //

        public void popuverMove_Click(object sender, RoutedEventArgs e)
        {
            Tools.errWrite($"[{DateTime.Now} popuverMove_Click ] -> SubjectItem");



            var btn = sender as Button;
            pp_selected = (string)btn.Uid;
            moveBarController.HeaderText = btn.Content.ToString()!;
            pp_moveBar.IsOpen = true;
        }

        public Popup getPopupMove()
        {
            return pp_moveBar;
        }

        public Popup getPopupBase()
        {

            return (Popup)FindName(pp_selected);
        }


    }
}