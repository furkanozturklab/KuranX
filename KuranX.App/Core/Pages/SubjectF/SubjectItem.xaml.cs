using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class SubjectItem : Page
    {
        public int sSureId, sverseId, verseId, subId, subItemsId;
        private string intelWriter = "";

        public SubjectItem()
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

        public object subjectItemsPageCall(int SubID, int SureId, int VerseId)
        {
            try
            {
                subId = SubID;
                sSureId = SureId;
                verseId = VerseId;
                App.loadTask = Task.Run(() => loadItem(SubID, SureId, VerseId));
                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
                return this;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainScreen.navigationWriter("subject", loadHeader.Text + "," + loadBackHeader.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        // ---------- Load FUNC ---------- //

        public void loadItem(int SubID, int SureId, int VerseId)
        {
            try
            {
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
                        loadCreated.Text = dSub.created.ToString("D");
                        loadBackHeader.Text = $"{dSure.name} Suresinin {VerseId} Ayeti";

                        App.mainScreen.navigationWriter("subject", loadHeader.Text + "," + loadBackHeader.Text);

                        loadVerseTr.Text = dVerse.verseTr;
                        loadVerseArb.Text = dVerse.verseArabic;
                        sverseId = (int)dVerse.verseId;
                        loadInterpreterFunc(App.InterpreterWriter, sverseId);
                    });
                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        public void loadInterpreterFunc(string writer, int verseId)
        {
            // İnterpreter Load Func

            try
            {
                using (var entitydb = new AyetContext())
                {
                    switch (writer)
                    {
                        case "Yorumcu 1":
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

                    if (writer == "") writer = "Yorumcu 1";
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
                App.logWriter("Loading", ex);
            }
        }

        public void loadItemsInterpreter(Interpreter dInter)
        {
            // items Load Func
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadDetail.Text = dInter.interpreterDetail;
                    tempLoadDetail.Text = dInter.interpreterDetail;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        private void noteConnect()
        {
            try
            {
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
                App.logWriter("Loading", ex);
            }
        }

        // ---------- Load FUNC ---------- //

        // ---------- CLİCK FUNC ---------- //

        private void openVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = App.navVersePage.PageCall(sSureId, verseId, "Subject");
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void changeloadDetail_Click(object sender, EventArgs e)
        {
            // TR ARP İNTERPRETER TEXT CHANGE FUNC
            try
            {
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
                App.logWriter("Click", ex);
            }
        }

        private void wordText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
            }
        }

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

        private void noteAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_noteAddPopup.IsOpen = true;
                noteConnectVerse.Text = loadBackHeader.Text;
                noteType.Text = "Konu Notu";
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
                var tmpbutton = sender as Button;
                App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(tmpbutton.Uid));
                tmpbutton = null;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

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
                                        App.loadTask = Task.Run(noteConnect);

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

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_notesAllShowPopup.IsOpen = true;
                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.sureId == sSureId && p.verseId == verseId).ToList();
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
                App.logWriter("Click", ex);
            }
        }

        private void notesDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tmpbutton = sender as Button;
                App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(tmpbutton.Uid));
                tmpbutton = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // ---------- CLİCK FUNC ---------- //

        // ---------- Changed FUNC ---------- //

        private void interpreterWriterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Nav NextUpdate Click
            try
            {
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

        private void noteName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                noteAddPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
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
                App.logWriter("Change", ex);
            }
        }

        // ---------- Changed FUNC ---------- //

        // ---------- Animation Func ----------- //

        private void loadAni()
        {
            try
            {
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
                App.logWriter("Animation", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {
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
                App.logWriter("Animation", ex);
            }
        }

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
            Debug.WriteLine(ppmove);
            for (int i = 1; i < 8; i++)
            {
                var btn = FindName("pp_M" + i) as Button;
                btn.Content = ppmove;
            }
        }
    }
}