using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace KuranX.App.Core.Pages.VerseF
{
    /// <summary>
    /// Interaction logic for verseFrame.xaml
    /// </summary>
    public partial class verseFrame : Page
    {
        private Task loadTask;
        private int sSureId, sRelativeVerseId, verseId;
        public int[] feedPoint = new int[4];
        private string navLocation, meaningPattern;
        private bool nexting = false, remiderType = false;
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public verseFrame(int sureId, int relativeVerseId, string navLoaction)
        {
            InitializeComponent();
        }

        public object PageCall(int sureId, int relativeVerseId, string Location)
        {
            sSureId = sureId;
            sRelativeVerseId = relativeVerseId;
            navLocation = Location;
            loadTask = Task.Run(() => loadVerseFunc(relativeVerseId));
            return this;
        }

        // ---------- LOAD FUNC ---------- //

        public void loadVerseFunc(int relativeVerseId)
        {
            // Verse Load Func
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dSure = entitydb.Sure.Where(p => p.sureId == sSureId).Select(p => new Sure { Name = p.Name, NumberOfVerses = p.NumberOfVerses, LandingLocation = p.LandingLocation, Description = p.Description, DeskLanding = p.DeskLanding, DeskMushaf = p.DeskMushaf }).First();
                    var dVerse = entitydb.Verse.Where(p => p.SureId == sSureId && p.RelativeDesk == relativeVerseId).First();
                    verseId = (int)dVerse.VerseId;

                    sRelativeVerseId = (int)dVerse.RelativeDesk;

                    loadNavFunc();
                    loadItemsHeader(dSure);
                    loadItemsControl(dVerse);
                    loadItemsContent(dVerse);
                    loadInterpreterFunc("", verseId);

                    this.Dispatcher.Invoke(() =>
                    {
                        if (dSure.Name == "Fâtiha" && sRelativeVerseId == 1) NavUpdatePrevSingle.IsEnabled = false;
                        else NavUpdatePrevSingle.IsEnabled = true;
                    });

                    dSure = null;
                    dVerse = null;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadFunc", ex);
            }
        }

        public void loadInterpreterFunc(string writer, int verseId)
        {
            // İnterpreter Load Func
            try
            {
                using (var entitydb = new AyetContext())
                {
                    if (writer == "") writer = "Mehmet Okuyan";
                    verseId--;
                    var dInter = entitydb.Interpreter.Where(p => p.sureId == sSureId && p.verseId == verseId && p.interpreterWriter == writer).First();

                    loadItemsInterpreter(dInter);
                    dInter = null;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadFunc", ex);
            }
        }

        public void loadNavFunc(int prev = 0)
        {
            using (var entitydb = new AyetContext())
            {
                var last = sRelativeVerseId;

                if (sRelativeVerseId <= 7) last = 1;

                if (prev != 0) last -= prev;
                else last--;

                var dVerseNav = entitydb.Verse.Where(p => p.SureId == sSureId).Select(p => new Verse() { VerseId = p.VerseId, RelativeDesk = p.RelativeDesk, Status = p.Status, VerseCheck = p.VerseCheck }).Skip(last).Take(9).ToList();
                var tempVerse = new List<Verse>();
                int i = 1;
                for (int x = 1; x < 9; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("vb" + x);
                        itemslist.ItemsSource = null;
                    });
                }
                foreach (var item in dVerseNav)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        tempVerse.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("vb" + i);
                        itemslist.ItemsSource = tempVerse;
                        tempVerse.Clear();
                        i++;
                    });

                    if (i == 9) break; // 7 den fazla varmı kontrol etmek için koydum
                }
                dVerseNav = null;
                tempVerse = null;
            }
        }

        public void loadItemsHeader(Sure dSure)
        {
            // items Load Func
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadHeader.Text = dSure.Name;
                    loadLocation.Text = dSure.LandingLocation;
                    loadDesc.Text = dSure.Description;
                    loadVerseCount.Text = dSure.NumberOfVerses.ToString();
                    loadDeskLanding.Text = dSure.DeskLanding.ToString();
                    loadDeskMushaf.Text = dSure.DeskMushaf.ToString();
                });
            }
            catch (Exception ex)
            {
                App.logWriter("LoadFunc", ex);
            }
        }

        public void loadItemsControl(Verse dVerse)
        {
            // items Load Func
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    markButton.IsChecked = bool.Parse(dVerse.MarkCheck);
                    markButton.Uid = dVerse.VerseId.ToString();
                    bellButton.IsChecked = bool.Parse(dVerse.RemiderCheck);
                    bellButton.Uid = dVerse.VerseId.ToString();
                    noteButton.Uid = dVerse.VerseId.ToString();
                    checkButton.IsChecked = bool.Parse(dVerse.VerseCheck);
                    checkButton.Uid = dVerse.VerseId.ToString();
                    descButton.Uid = dVerse.VerseDesc;
                    feedbackButton.Uid = dVerse.VerseId.ToString();
                });
            }
            catch (Exception ex)
            {
                App.logWriter("LoadFunc", ex);
            }
        }

        public void loadItemsContent(Verse dVerse)
        {
            // items Load Func
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadVerseTr.Text = dVerse.VerseTr;
                    loadVerseArb.Text = dVerse.VerseArabic;
                    CurrentVerse.Tag = dVerse.RelativeDesk;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("LoadFunc", ex);
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
                App.logWriter("LoadFunc", ex);
            }
        }

        private void noteConnect()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.SureId == sSureId && p.VerseId == sRelativeVerseId && p.NoteLocation == "Ayet").ToList();
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

        public void loadMeaning()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Integrity.Where(p => p.connectSureId == sSureId && p.connectVerseId == sRelativeVerseId || p.connectedSureId == sSureId && p.connectedVerseId == sRelativeVerseId).ToList();
                    var dTempInteg = new List<Integrity>();
                    int i = 1;

                    for (int x = 1; x < 8; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var itemslist = (ItemsControl)FindName("mvc" + x);
                            itemslist.ItemsSource = null;
                        });
                    }

                    foreach (var item in dInteg)
                    {
                        if (i == 8)
                        {
                            allShowMeaningButton.Dispatcher.Invoke(() =>
                            {
                                allShowMeaningButton.Visibility = Visibility.Visible;
                            });

                            break;
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            var dControlTemp = (ItemsControl)FindName("mvc" + i);
                            dTempInteg.Add(item);
                            dControlTemp.ItemsSource = dTempInteg;
                            dTempInteg.Clear();

                            i++;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void subjectFolder()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dSubjectFolder = entitydb.Subject.ToList();

                    this.Dispatcher.Invoke(() =>
                    {
                        selectedSubjectFolder.Items.Clear();
                        foreach (var item in dSubjectFolder)
                        {
                            var cmbitem = new ComboBoxItem();

                            cmbitem.Content = item.SubjectName;
                            cmbitem.Uid = item.SubjectId.ToString();
                            selectedSubjectFolder.Items.Add(cmbitem);
                        }
                    });

                    dSubjectFolder = null;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        // ---------- LOAD FUNC ---------- //

        // ---------- CLİCK FUNC ---------- //

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
                App.logWriter("ClickFunc", ex);
            }
        }

        private void activeVerseSelected_Click(object sender, EventArgs e)
        {
            // Verse Change Click
            try
            {
                var chk = sender as CheckBox;
                if (chk.IsChecked.ToString() == "True") chk.IsChecked = false;
                else { chk.IsChecked = true; }
                int currentP = int.Parse(chk.Content.ToString().Split(" ")[0]);
                loadTask = Task.Run(() => loadVerseFunc(currentP));
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void NavUpdatePrevSingle_Click(object sender, EventArgs e)
        {
            // Nav PrevSingle Click
            try
            {
                if (int.Parse(loadVerseCount.Text) >= sRelativeVerseId && 1 < sRelativeVerseId)
                {
                    sRelativeVerseId--;

                    if (sRelativeVerseId % 7 == 0) nexting = true;
                    if (nexting)
                    {
                        loadTask = Task.Run(() => loadNavFunc(7));

                        nexting = false;
                    }

                    if (loadHeader.Text == "Fâtiha" && sRelativeVerseId == 1) NavUpdatePrevSingle.IsEnabled = false;
                    loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));
                }
                else
                {
                    if (App.currentDesktype == "DeskLanding")
                    {
                        int xc = 0;
                        using (var entitydb = new AyetContext())
                        {
                            var listx = entitydb.Sure.OrderBy(p => p.DeskLanding);
                            foreach (var item in listx)
                            {
                                xc++;
                                if (loadHeader.Text == item.Name) break;
                            }
                            xc--;
                            var listxc = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.DeskLanding == xc).FirstOrDefault();
                            App.mainframe.Content = App.navVersePage.PageCall((int)listxc.sureId, (int)listxc.NumberOfVerses, "Verse");
                            listx = null;
                            listxc = null;
                        }
                    }
                    else
                    {
                        using (var entitydb = new AyetContext())
                        {
                            int selectedSure = sSureId;
                            selectedSure--;
                            var BeforeD = entitydb.Sure.Where(p => p.sureId == selectedSure).Select(p => new Sure() { NumberOfVerses = p.NumberOfVerses }).FirstOrDefault();
                            App.mainframe.Content = App.navVersePage.PageCall(--sSureId, (int)BeforeD.NumberOfVerses, "Verse");
                            BeforeD = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void NavUpdateNextSingle_Click(object sender, EventArgs e)
        {
            // Nav NextSingle Click
            try
            {
                if (int.Parse(loadVerseCount.Text) > sRelativeVerseId)
                {
                    if (sRelativeVerseId % 7 == 0) nexting = true;
                    if (nexting)
                    {
                        loadTask = Task.Run(() => loadNavFunc());

                        nexting = false;
                    }

                    NavUpdatePrevSingle.IsEnabled = true;
                    sRelativeVerseId++;
                    loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));
                }
                else
                {
                    if (App.currentDesktype == "DeskLanding")
                    {
                        using (var entitydb = new AyetContext())
                        {
                            int xc = 0;
                            var listx = entitydb.Sure.OrderBy(p => p.DeskLanding);
                            foreach (var item in listx)
                            {
                                xc++;
                                if (loadHeader.Text == item.Name) break;
                            }
                            xc++;
                            var listxc = entitydb.Sure.OrderBy(p => p.DeskLanding).Where(p => p.DeskLanding == xc).First();
                            App.mainframe.Content = App.navVersePage.PageCall((int)listxc.sureId, 1, "Verse");
                            listx = null;
                            listxc = null;
                        }
                    }
                    else App.mainframe.Content = App.navVersePage.PageCall(++sSureId, 1, "Verse");
                }
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void backVersesFrame_Click(object sender, RoutedEventArgs e)
        {
            if (markButton.IsChecked == true)
            {
                NavigationService.GoBack();
            }
            else
            {
                popup_fastExitConfirm.IsOpen = true;
            }
        }

        private void fastexitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void checkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var chk = sender as CheckBox;

                using (var entitydb = new AyetContext())
                {
                    var verseUpdate = entitydb.Verse.Where(p => p.VerseId == int.Parse(chk.Uid)).FirstOrDefault();
                    var sureUpdate = entitydb.Sure.Where(p => p.sureId == sSureId).FirstOrDefault();

                    if (chk.IsChecked.ToString() == "True")
                    {
                        verseUpdate.VerseCheck = "true";
                        verseUpdate.Status = "#66E21F";
                        sureUpdate.UserCheckCount++;

                        if (sureUpdate.UserCheckCount == sureUpdate.NumberOfVerses)
                        {
                            sureUpdate.Complated = true;
                            sureUpdate.Status = "#66E21F";
                        }
                    }
                    else
                    {
                        verseUpdate.VerseCheck = "false";
                        verseUpdate.Status = "#FFFFFF";

                        sureUpdate.Complated = false;
                        if (sureUpdate.UserCheckCount != 0) sureUpdate.UserCheckCount--;
                    }
                    entitydb.SaveChanges();

                    loadTask = Task.Run(() => loadNavFunc());

                    chk = null;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("UpdateEvent", ex);
            }
        }

        private void bellButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as CheckBox;
                var control = btn.IsChecked.Value;
                if (control == false)
                {
                    btn.IsChecked = true;
                    popup_remiderControlPopup.IsOpen = true;

                    using (var entitydb = new AyetContext())
                    {
                        var dRemider = entitydb.Remider.Where(p => p.ConnectSureId == sSureId && p.ConnectVerseId == sRelativeVerseId).First();

                        if (dRemider != null)
                        {
                            remiderNameControl.Text = dRemider.RemiderName;

                            if (dRemider.Status == "Wait")
                            {
                                TimeSpan tt = DateTime.Parse(dRemider.RemiderDate.ToString("d")) - DateTime.Parse(DateTime.Now.ToString("d"));
                                remiderTimerControl.Text = "Hatırlatmaya Kalan Süre " + tt.TotalDays.ToString() + " Gün ";
                            }
                            else
                            {
                                string d = "";
                                switch (dRemider.LoopType)
                                {
                                    case "day":
                                        d = "Günlük";
                                        break;

                                    case "week":
                                        d = "Haftalık";
                                        break;

                                    case "month":
                                        d = "Aylık";
                                        break;

                                    case "years":
                                        d = "Yıllık";
                                        break;
                                }
                                remiderTimerControl.Text = "Hatırlatıcı " + d + "  olarak ayarlanmıştır. ";
                            }
                        }
                    }
                }
                else
                {
                    btn.IsChecked = false;
                    popup_remiderAddPopup.IsOpen = true;
                    remiderConnectVerse.Text = loadHeader.Text + " > " + sRelativeVerseId;
                }
                btn = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void markButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox bchk = sender as CheckBox;
                using (var entitydb = new AyetContext())
                {
                    var selectedS = entitydb.Sure.Where(p => p.Status == "#0D6EFD").ToList();
                    foreach (var item in selectedS)
                    {
                        if (item.Complated == true)
                        {
                            item.Status = "#66E21F";
                        }
                        else
                        {
                            item.Status = "#ADB5BD";
                        }
                    }

                    if (bchk.IsChecked.ToString() == "True")
                    {
                        entitydb.Sure.Where(p => p.sureId == sSureId).First().Status = "#0D6EFD";
                        entitydb.Sure.Where(p => p.sureId == sSureId).First().UserLastRelativeVerse = sRelativeVerseId;
                        entitydb.Verse.Where(p => p.SureId == sSureId && p.RelativeDesk == sRelativeVerseId).First().MarkCheck = "True";
                    }
                    else
                    {
                        entitydb.Sure.Where(p => p.sureId == sSureId).First().Status = "#ADB5BD";
                        entitydb.Sure.Where(p => p.sureId == sSureId).First().UserLastRelativeVerse = 1;
                        entitydb.Verse.Where(p => p.SureId == sSureId && p.RelativeDesk == sRelativeVerseId).First().MarkCheck = "False";

                        if (entitydb.Sure.Where(p => p.sureId == sSureId).First().Complated == true) entitydb.Sure.Where(p => p.sureId == sSureId).First().Status = "#66E21F";
                        else entitydb.Sure.Where(p => p.sureId == sSureId).First().Status = "#ADB5BD";
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("UpdateEvent", ex);
            }
        }

        private void subjectColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox? chk;

                foreach (object item in subjectColorStack.Children)
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

                subjectpreviewColor.Background = new BrushConverter().ConvertFromString(chk.Tag.ToString()) as SolidColorBrush;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // ---------- CLİCK FUNC ---------- //

        // ---------- Popup Open FUNC ---------- //

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);
                popuptemp.IsOpen = false;
                popupRelativeId.Text = "";
                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void openVerseNumberPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_VerseGoto.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_Note.IsOpen = true;
                loadTask = Task.Run(noteConnect);
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void noteAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_noteAddPopup.IsOpen = true;
                noteConnectVerse.Text = loadHeader.Text + " > " + sRelativeVerseId;
                noteType.Text = "Ayet Notu";
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void remiderDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_remiderDeleteConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void descButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var contentb = sender as Button;
                textDesc.Text = contentb.Uid.ToString();
                popupHeaderTextDesc.Text = loadHeader.Text + " Suresinin " + sRelativeVerseId + " Ayetinin Kısa Tefsiri";
                popup_descVersePopup.IsOpen = true;
                contentb = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void feedbackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_feedbackOpenPopup.IsOpen = true;
                feedbackConnectVerse.Text = loadHeader.Text + " > " + sRelativeVerseId.ToString();
                feedbackDetail.Focus();
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void openAddSubjectPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_addSubjectPopup.IsOpen = true;
                selectedSubject.Text = loadHeader.Text + " > " + sRelativeVerseId.ToString();
                loadTask = Task.Run(() => subjectFolder());
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void newSubjectFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_FolderSubjectPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void openMeaningPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_Meaning.IsOpen = true;
                loadTask = Task.Run(() => loadMeaning());
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void openMeaningAddPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_meaningAddPopup.IsOpen = true;
                popup_meaningAddPopup.Focus();
                var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                meaningPattern = loadHeader.Text + ". " + sRelativeVerseId.ToString() + " ayeti ";
                attachSureVerseCountText.Content = "Secilebilecek Ayet Sayısı " + item.Tag.ToString();
                meaningName.Text = meaningPattern;
                meaningAttachVerse.Text = loadHeader.Text + " > " + sRelativeVerseId.ToString();
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void editMeaningButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                meaningEditNote.Text = meaningOpenDetailText.Text;
                popup_meaningEditPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void deleteMeaningButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_meaningDeleteConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void meaningOpenDetailTextBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_meainingConnectDetailText.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void resetBtnSure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_resetVerseConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        // ---------- Popup Open FUNC ---------- //

        // ---------- Popup Actions FUNC ---------- //

        private void loadVersePopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.Parse(loadVerseCount.Text) >= int.Parse(popupRelativeId.Text))
                {
                    sRelativeVerseId = int.Parse(popupRelativeId.Text);
                    loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));

                    popup_VerseGoto.IsOpen = false;
                    popupRelativeId.Text = "";
                }
                else
                {
                    alertFunc("Ayet Mevcut Değil", "Ayet Sayısını Geçtiniz Gidile Bilecek Son Ayet Sayısı : " + loadVerseCount.Text + " Lütfen Tekrar Deneyiniz.", 3);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void noteDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tmpbutton = sender as Button;
                App.mainframe.Content = App.navNoteItem.noteItemPageCall(int.Parse(tmpbutton.Uid));
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
                                    if (entitydb.Notes.Where(p => p.NoteHeader == noteName.Text && p.SureId == sSureId && p.VerseId == sRelativeVerseId).FirstOrDefault() != null)
                                    {
                                        alertFunc("Not Ekleme Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", 3);
                                    }
                                    else
                                    {
                                        var dNotes = new Notes { NoteHeader = noteName.Text, NoteDetail = noteDetail.Text, SureId = sSureId, VerseId = sRelativeVerseId, Modify = DateTime.Now, Created = DateTime.Now, NoteLocation = "Ayet" };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        succsessFunc("Not Ekleme Başarılı", loadHeader.Text + " Surenin " + sRelativeVerseId + " Ayetine Not Eklendiniz.", 3);
                                        loadTask = Task.Run(noteConnect);

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

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_notesAllShowPopup.IsOpen = true;
                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.SureId == sSureId && p.VerseId == sRelativeVerseId).ToList();
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
                App.logWriter("PopupAction", ex);
            }
        }

        private void notesDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tmpbutton = sender as Button;
                App.mainframe.Content = App.navNoteItem.noteItemPageCall(int.Parse(tmpbutton.Uid));
                tmpbutton = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void addRemiderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (remiderName.Text.Length >= 5)
                {
                    if (remiderName.Text.Length < 50)
                    {
                        if (remiderDetail.Text.Length >= 5)
                        {
                            if (remiderType == false)
                            {
                                if (remiderDay.SelectedDate != null)
                                {
                                    using (var entitydb = new AyetContext())
                                    {
                                        var dControl = entitydb.Remider.Where(p => p.ConnectSureId == sSureId && p.ConnectVerseId == sRelativeVerseId).ToList();

                                        if (dControl.Count > 0)
                                        {
                                            alertFunc("Hatırlatıcı Oluşturulamadı.", "Yeni hatırlatıcınız oluşturulamadı daha önceden oluşturulmuş olabilir.", 3);
                                            popup_remiderAddPopup.IsOpen = false;
                                            remiderName.Text = "";
                                            remiderDetail.Text = "";
                                            remiderDay.SelectedDate = null;
                                        }
                                        else
                                        {
                                            var newRemider = new Remider { ConnectSureId = sSureId, ConnectVerseId = sRelativeVerseId, RemiderDate = (DateTime)remiderDay.SelectedDate, RemiderDetail = remiderDetail.Text, RemiderName = remiderName.Text, Create = DateTime.Now, Priority = 1, LastAction = DateTime.Now, Status = "Run" };
                                            entitydb.Verse.Where(p => p.SureId == sSureId && p.RelativeDesk == sRelativeVerseId).First().RemiderCheck = "true";
                                            entitydb.Remider.Add(newRemider);
                                            entitydb.SaveChanges();
                                            succsessFunc("Hatırlatıcı Oluşturuldu.", "Yeni hatırlatıcınız oluşturuldu", 3);

                                            popup_remiderAddPopup.IsOpen = false;
                                            remiderName.Text = "";
                                            remiderDetail.Text = "";
                                            remiderDay.SelectedDate = null;
                                            bellButton.IsChecked = true;
                                            loadTask = Task.Run(() => loadNavFunc());

                                            newRemider = null;
                                        }
                                        dControl = null;
                                    }
                                }
                                else
                                {
                                    remiderAddPopupDateError.Visibility = Visibility.Visible;
                                    remiderDay.Focus();
                                    remiderAddPopupDateError.Content = "Hatırlatıcı için gün secmelisiniz.";
                                }
                            }
                            else
                            {
                                var ditem = loopSelectedType.SelectedItem as ComboBoxItem;

                                if (ditem.Uid != "select")
                                {
                                    using (var entitydb = new AyetContext())
                                    {
                                        var dControl = entitydb.Remider.Where(p => p.ConnectSureId == sSureId && p.ConnectVerseId == sRelativeVerseId).ToList();

                                        if (dControl.Count > 0)
                                        {
                                            alertFunc("Hatırlatıcı Oluşturulamadı.", "Yeni hatırlatıcınız oluşturulamadı daha önceden oluşturulmuş olabilir.", 3);
                                            popup_remiderAddPopup.IsOpen = false;
                                            remiderName.Text = "";
                                            remiderDetail.Text = "";
                                            loopSelectedType.SelectedIndex = 0;
                                        }
                                        else
                                        {
                                            int pr = 0;
                                            switch (ditem.Uid)
                                            {
                                                case "Gün":
                                                    pr = 2;
                                                    break;

                                                case "Hafta":
                                                    pr = 3;
                                                    break;

                                                case "Ay":
                                                    pr = 4;
                                                    break;

                                                case "Yıl":
                                                    pr = 5;
                                                    break;
                                            }
                                            var newRemider = new Remider { ConnectSureId = sSureId, ConnectVerseId = sRelativeVerseId, RemiderDate = new DateTime(1, 1, 1, 0, 0, 0, 0), RemiderDetail = remiderDetail.Text, RemiderName = remiderName.Text, Create = DateTime.Now, LoopType = ditem.Uid.ToString(), Status = "Run", Priority = pr, LastAction = DateTime.Now };
                                            entitydb.Verse.Where(p => p.SureId == sSureId && p.RelativeDesk == sRelativeVerseId).FirstOrDefault().RemiderCheck = "true";
                                            entitydb.Remider.Add(newRemider);
                                            entitydb.SaveChanges();
                                            succsessFunc("Hatırlatıcı Oluşturuldu.", "Yeni hatırlatıcınız oluşturuldu", 3);

                                            popup_remiderAddPopup.IsOpen = false;
                                            remiderName.Text = "";
                                            remiderDetail.Text = "";
                                            loopSelectedType.SelectedIndex = 0;

                                            loadTask = Task.Run(() => loadNavFunc());

                                            newRemider = null;
                                            dControl = null;
                                        }
                                    }
                                }
                                else
                                {
                                    remiderAddPopupDateError.Visibility = Visibility.Visible;
                                    loopSelectedType.Focus();
                                    remiderAddPopupDateError.Content = "Hatırlatma Aralığını Secmelisiniz.";
                                }
                                ditem = null;
                            }
                        }
                        else
                        {
                            remiderAddPopupDetailError.Visibility = Visibility.Visible;
                            remiderDetail.Focus();
                            remiderAddPopupDetailError.Content = "Hatırlatıcı notu Yeterince Uzun Değil. Min 5 Karakter Olmalıdır";
                        }
                    }
                    else
                    {
                        remiderAddPopupHeaderError.Visibility = Visibility.Visible;
                        remiderName.Focus();
                        remiderAddPopupHeaderError.Content = "Hatırlatıcı İsmi Çok Uzun. Max 50 Karakter Olabilir";
                    }
                }
                else
                {
                    remiderAddPopupHeaderError.Visibility = Visibility.Visible;
                    remiderName.Focus();
                    remiderAddPopupHeaderError.Content = "Hatırlatıcı İsmi Yeterince Uzun Değil. Min 5 Karakter Olmalıdır";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void deleteRemiderBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.ConnectVerseId == sRelativeVerseId && p.ConnectSureId == sSureId));
                    entitydb.Verse.Where(p => p.VerseId == sRelativeVerseId && p.SureId == sSureId).FirstOrDefault().RemiderCheck = "false";
                    entitydb.SaveChanges();
                    popup_remiderDeleteConfirm.IsOpen = false;
                    popup_remiderControlPopup.IsOpen = false;
                    loadTask = Task.Run(() => loadNavFunc());

                    succsessFunc("Hatırlatıcı Silindi ", " Hatırlatıcı Silindi yeniden hatırlatıcı oluştura bilirsiniz...", 3);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void addSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var item = selectedSubjectFolder.SelectedItem as ComboBoxItem;

                    if (item != null)
                    {
                        var dControl = entitydb.SubjectItems.Where(p => p.SureId == sSureId && p.VerseId == sRelativeVerseId && p.SubjectId == int.Parse(item.Uid)).ToList();

                        if (dControl.Count != 0)
                        {
                            alertFunc("Ayet Ekleme Başarısız", "Bu ayet Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            var dSubjectItem = new SubjectItems { SubjectId = int.Parse(item.Uid), SubjectNotesId = 0, SureId = sSureId, VerseId = sRelativeVerseId, Created = DateTime.Now, Modify = DateTime.Now, SubjectName = loadHeader.Text + " Suresinin " + sRelativeVerseId.ToString() + " Ayeti" };
                            entitydb.SubjectItems.Add(dSubjectItem);
                            entitydb.SaveChanges();
                            popup_addSubjectPopup.IsOpen = false;
                            succsessFunc("Ayet Ekleme Başarılı", "Seçmiş olduğunuz konuya ayet eklendi.", 3);
                            dSubjectItem = null;
                        }
                        dControl = null;
                    }
                    else
                    {
                        popupaddsubjectError.Visibility = Visibility.Visible;
                        popupaddsubjectError.Text = "Lütfen Konuyu Seçiniz";
                        selectedSubjectFolder.Focus();
                    }
                    item = null;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void addfolderSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (subjectFolderHeader.Text.Length >= 8)
                {
                    if (subjectFolderHeader.Text.Length < 50)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Subject.Where(p => p.SubjectName == subjectpreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dSubjectFolder = new Subject { SubjectName = subjectpreviewName.Text, SubjectColor = subjectpreviewColor.Background.ToString(), Created = DateTime.Now, Modify = DateTime.Now };
                                entitydb.Subject.Add(dSubjectFolder);
                                entitydb.SaveChanges();
                                succsessFunc("Konu Başlığı ", " Yeni konu başlığı oluşturuldu artık ayetleri ekleye bilirsiniz.", 3);

                                subjectpreviewName.Text = "";
                                subjectFolderHeader.Text = "";
                                popup_FolderSubjectPopup.IsOpen = false;
                                loadTask = Task.Run(() => subjectFolder());

                                dSubjectFolder = null;
                            }
                            else
                            {
                                alertFunc("Konu Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                            }
                            dControl = null;
                        }
                    }
                    else
                    {
                        subjectFolderHeader.Focus();
                        subjectHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                        subjectHeaderFolderErrorMesssage.Content = "Konu başlığının çok uzun max 50 karakter olabilir";
                    }
                }
                else
                {
                    subjectFolderHeader.Focus();
                    subjectHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    subjectHeaderFolderErrorMesssage.Content = "Konu başlığının uzunluğu minimum 8 karakter olmalı";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void meaningDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tmpbutton = sender as Button;

                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Integrity.Where(p => p.IntegrityId == Int16.Parse(tmpbutton.Uid)).FirstOrDefault();

                    var dSure = entitydb.Sure.Where(p => p.sureId == dInteg.connectSureId).Select(p => new Sure() { Name = p.Name }).FirstOrDefault();
                    var dCSure = entitydb.Sure.Where(p => p.sureId == dInteg.connectedSureId).Select(p => new Sure() { Name = p.Name }).FirstOrDefault();
                    meaningOpenDetailText.Text = dInteg.IntegrityNote;
                    meaningDetailTextHeader.Text = dInteg.IntegrityName;

                    meaningDTempSureId.Text = dInteg.connectedSureId.ToString();
                    meaningDTempVerseId.Text = dInteg.connectedVerseId.ToString();
                    meaningDId.Text = dInteg.IntegrityId.ToString();
                    meaningOpenDetailTextConnect.Text = dSure.Name + " Suresini " + dInteg.connectVerseId.ToString() + " Ayeti İçin " + dCSure.Name + " " + dInteg.connectedVerseId + " Ayeti ile Eşleştirilmiş";
                }

                popup_meainingConnectDetailText.IsOpen = true;

                tmpbutton = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void allShowMeaningButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_meainingAllShowPopup.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Integrity.Where(p => p.connectSureId == sSureId && p.connectVerseId == sRelativeVerseId || p.connectedVerseId == sRelativeVerseId && p.connectedSureId == sSureId).ToList();

                    foreach (var item in dInteg)
                    {
                        var itemsStack = new StackPanel();
                        var headerText = new TextBlock();
                        var noteText = new TextBlock();
                        var allshowButton = new Button();
                        var sp = new Separator();

                        itemsStack.Style = (Style)FindResource("dynamicItemStackpanel");
                        headerText.Style = (Style)FindResource("dynamicItemTextHeader");
                        noteText.Style = (Style)FindResource("dynamicItemTextNote");
                        allshowButton.Style = (Style)FindResource("dynamicItemShowButton");
                        sp.Style = (Style)FindResource("dynamicItemShowSperator");

                        headerText.Text = item.IntegrityName.ToString();
                        noteText.Text = item.IntegrityNote.ToString();
                        allshowButton.Uid = item.IntegrityId.ToString();

                        allshowButton.Click += meaningDetailPopup_Click;

                        itemsStack.Children.Add(headerText);
                        itemsStack.Children.Add(noteText);
                        itemsStack.Children.Add(allshowButton);
                        itemsStack.Children.Add(sp);

                        meainingAllShowPopupStackPanel.Children.Add(itemsStack);

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
                App.logWriter("PopupAction", ex);
            }
        }

        private void newmeaningVerse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (meaningName.Text.Length >= 8)
                {
                    if (meaningName.Text.Length < 50)
                    {
                        var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;

                        if (item.Uid != "0")
                        {
                            if (meaningConnectVerse.Text != "0" && meaningConnectVerse.Text != "")
                            {
                                if (int.Parse(meaningConnectVerse.Text) <= int.Parse(item.Tag.ToString()))
                                {
                                    if (meaningConnectNote.Text.Length >= 8)
                                    {
                                        var tmpcbxi = (ComboBoxItem)meaningpopupNextSureId.SelectedItem;

                                        using (var entitydb = new AyetContext())
                                        {
                                            if (entitydb.Integrity.Where(p => p.IntegrityName == meaningName.Text).FirstOrDefault() != null)
                                            {
                                                alertFunc("Bağlantı Ekleme Başarısız", "Aynı isimli bağlantı eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", 3);
                                                meaningName.Text = "";
                                                meaningConnectNote.Text = "";
                                                meaningpopupNextSureId.SelectedIndex = 0;
                                                meaningConnectVerse.Text = "";
                                                popup_meaningAddPopup.IsOpen = false;
                                            }
                                            else
                                            {
                                                var dIntegrity = new Integrity { IntegrityName = meaningName.Text, connectSureId = sSureId, connectVerseId = sRelativeVerseId, connectedSureId = int.Parse(tmpcbxi.Uid), connectedVerseId = int.Parse(meaningConnectVerse.Text), Created = DateTime.Now, Modify = DateTime.Now, IntegrityNote = meaningConnectNote.Text };
                                                entitydb.Integrity.Add(dIntegrity);
                                                entitydb.SaveChanges();
                                                succsessFunc("Bağlantı Oluşturuldu.", "Yeni bağlantınız oluşturuldu.", 3);
                                                meaningName.Text = "";
                                                meaningConnectNote.Text = "";
                                                meaningpopupNextSureId.SelectedIndex = 0;
                                                meaningConnectVerse.Text = "";
                                                popup_meaningAddPopup.IsOpen = false;
                                                loadTask = Task.Run(() => loadMeaning());

                                                dIntegrity = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        meaningDetailAddPopupHeaderError.Visibility = Visibility.Visible;
                                        meaningConnectNote.Focus();
                                        meaningDetailAddPopupHeaderError.Content = "Bağlantı Notu Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                                    }
                                }
                                else
                                {
                                    meaningCountAddPopupHeaderError.Visibility = Visibility.Visible;
                                    meaningConnectVerse.Focus();
                                    meaningCountAddPopupHeaderError.Content = "Ayet Sınırını geçtiniz Max " + item.Tag.ToString() + " olabilir";
                                }
                            }
                            else
                            {
                                meaningCountAddPopupHeaderError.Visibility = Visibility.Visible;
                                meaningConnectVerse.Focus();
                                meaningCountAddPopupHeaderError.Content = "Ayet seçilmemiş. Ayeti seçiniz";
                                meaningConnectVerse.Text = "";
                            }
                        }
                        else
                        {
                            meaningCountAddPopupHeaderError.Visibility = Visibility.Visible;
                            meaningpopupNextSureId.Focus();
                            meaningCountAddPopupHeaderError.Content = "Bağlantı Suresi Secilmemiş";
                        }
                    }
                    else
                    {
                        meaningNameAddPopupHeaderError.Visibility = Visibility.Visible;
                        meaningName.Focus();
                        meaningNameAddPopupHeaderError.Content = "Bağlantı Başlığı çok Uzun. Max 50 Karakter Olabilir";
                    }
                }
                else
                {
                    meaningNameAddPopupHeaderError.Visibility = Visibility.Visible;
                    meaningName.Focus();
                    meaningNameAddPopupHeaderError.Content = "Bağlantı Başlığı Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void meaningOpenSure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new stickVerseFrame(int.Parse(meaningDTempSureId.Text), int.Parse(meaningDTempVerseId.Text), loadHeader.Text, sRelativeVerseId.ToString(), "Visible");
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void editmeaningVerse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    if (meaningEditNote.Text.Length >= 8)
                    {
                        entitydb.Integrity.Where(p => p.IntegrityId == int.Parse(meaningDId.Text)).First().IntegrityNote = meaningEditNote.Text;
                        meaningOpenDetailText.Text = meaningEditNote.Text;
                        succsessFunc("Bağlantı Düzeltildi ", " Anlam Bütünlüğü Bağlantınızın Notu Düzeltildi...", 3);
                        popup_meaningEditPopup.IsOpen = false;
                    }
                    else
                    {
                        meaningDetailEditPopupHeaderError.Visibility = Visibility.Visible;
                        meaningEditNote.Focus();
                        meaningDetailEditPopupHeaderError.Content = "Anlam Bütünlüğü İçeriği Yeterince Uzun Değil. Min 8 Karakter Olmalıdır.";
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void deleteMeaningBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Integrity.RemoveRange(entitydb.Integrity.Where(p => p.IntegrityId == int.Parse(meaningDId.Text)));
                    entitydb.SaveChanges();
                    popup_meaningDeleteConfirm.IsOpen = false;
                    popup_meainingConnectDetailText.IsOpen = false;
                    loadTask = Task.Run(() => loadMeaning());

                    succsessFunc("Bağlantı Kaldırıldı ", " Anlam Bütünlüğü Bağlantısı Silindi...", 3);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void resetVersePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    foreach (var item in entitydb.Verse)
                    {
                        item.VerseCheck = "false";
                        item.Status = "#FFFFFF";
                        entitydb.Verse.Attach(item);
                    }

                    var sureUpdate = entitydb.Sure.Where(p => p.sureId == sSureId).FirstOrDefault();
                    sureUpdate.Complated = false;
                    sureUpdate.UserCheckCount = 0;
                    sureUpdate.Status = "#ADB5BD";

                    entitydb.SaveChanges();

                    succsessFunc("İlerleme Silindi ", " Suredeki ilerlemeniz başarılı bir şekilde sıfırlandı...", 3);
                    popup_resetVerseConfirm.IsOpen = false;

                    loadTask = Task.Run(() => loadVerseFunc(1));
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        // ---------- Popup Actions FUNC ---------- //

        // ---------- SelectionChanged FUNC ---------- //

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
                        loadInterpreterFunc(item.Content.ToString(), verseId);
                    }
                }
                item = null;
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void popupMeiningSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    if (item.Uid != "0")
                    {
                        attachSureVerseCountText.Content = $"Secilebilecek Ayet Sayısı {item.Tag}";
                        meaningCountAddPopupHeaderError.Visibility = Visibility.Hidden;
                        meaningConnectVerse.Focus();
                        meaningConnectVerse.Text = "";
                    }
                }
                item = null;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void meaningConnectVerse_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (meaningCountAddPopupHeaderError != null) meaningCountAddPopupHeaderError.Visibility = Visibility.Hidden;

                var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    if (item.Uid != "0")
                    {
                        if (meaningConnectVerse.Text != "")
                        {
                            if (int.Parse(meaningConnectVerse.Text) > int.Parse(item.Tag.ToString()))
                            {
                                meaningCountAddPopupHeaderError.Visibility = Visibility.Visible;
                                meaningConnectVerse.Focus();
                                meaningCountAddPopupHeaderError.Content = "Ayet Sınırını geçtiniz Max " + item.Tag.ToString() + " olabilir";
                            }
                            else
                            {
                                meaningName.Text = meaningPattern + " " + item.Content + ". " + meaningConnectVerse.Text + " ayeti ile bağlantılı";
                            }
                        }
                    }
                    else
                    {
                        if (meaningCountAddPopupHeaderError != null)
                        {
                            meaningCountAddPopupHeaderError.Visibility = Visibility.Visible;
                            meaningpopupNextSureId.Focus();
                            meaningCountAddPopupHeaderError.Content = "Bağlantı Suresi Secilmemiş";
                        }
                    }
                }
                item = null;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void meaningConnectVerse_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // ---------- SelectionChanged FUNC ---------- //

        // ---------- MessageFunc FUNC ---------- //

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

        // ---------- MessageFunc FUNC ---------- //

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

        private void remiderName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                remiderAddPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void remiderDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                remiderAddPopupDetailError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void subjectFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                subjectpreviewName.Text = subjectFolderHeader.Text;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void subjectFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                subjectHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void meaningName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                meaningNameAddPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void meaningDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                meaningDetailAddPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void meaningEditDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                meaningDetailEditPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void remiderTypeChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (loopType.Visibility.ToString() == "Hidden")
            {
                remiderType = true;
                loopType.Visibility = Visibility.Visible;
                dayType.Visibility = Visibility.Hidden;
                remiderAddPopupDateError.Visibility = Visibility.Hidden;
            }
            else
            {
                remiderType = false;
                loopType.Visibility = Visibility.Hidden;
                dayType.Visibility = Visibility.Visible;
                remiderAddPopupDateError.Visibility = Visibility.Hidden;
            }
        }

        private void popupRelativeId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void popupRelativeId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            if (!textbox.IsLoaded) return;

            if (popupRelativeId.Text != "" && popupRelativeId.Text != null)
            {
                if (int.Parse(popupRelativeId.Text) <= int.Parse(loadVerseCount.Text) && int.Parse(popupRelativeId.Text) > 0)
                {
                    loadVersePopup.IsEnabled = true;
                    popupRelativeIdError.Content = "Ayet Mevcut Gidilebilir";
                }
                else
                {
                    loadVersePopup.IsEnabled = false;
                    popupRelativeIdError.Content = "Ayet Mevcut Değil";
                }
            }
            else
            {
                loadVersePopup.IsEnabled = false;
                popupRelativeIdError.Content = "Gitmek İstenilen Ayet Sırasını Giriniz";
            }
        }

        private void wordText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_Words.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    // var dWords = entitydb.Words.Where(p => p.SureId == dSure[0].sureId).Where(p => p.VerseId == dVerse[0].VerseId).ToList();

                    //DATA DÜZELTİLECEK

                    dynamicWordDetail.Children.Clear();
                    /*
                    foreach (var item in null)
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
                    */
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void feedStarOne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (object item in oneFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                var dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in oneFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (int.Parse(dchk.Uid) == i) break;
                    i++;
                }
                dchk = null;
                feedPoint[0] = i;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void feedStarTwo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (object item in twoFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }
                var dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in twoFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (int.Parse(dchk.Uid) == i) break;
                    i++;
                }
                dchk = null;
                feedPoint[1] = i;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void feedStarThree_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (object item in threeFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));
                        chk.IsChecked = false;
                    }
                }
                var dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in threeFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (int.Parse(dchk.Uid) == i) break;
                    i++;
                }
                dchk = null;
                feedPoint[2] = i;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void feedStarFour_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (object item in fourFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }
                var dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in fourFeed.Children)
                {
                    CheckBox? chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (int.Parse(dchk.Uid) == i) break;
                    i++;
                }
                dchk = null;
                feedPoint[3] = i;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // ---------- Simple Clear Fonks ---------- //
    }
}