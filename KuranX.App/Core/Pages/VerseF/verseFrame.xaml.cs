using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace KuranX.App.Core.Pages.VerseF
{
    /// <summary>
    /// Interaction logic for verseFrame.xaml
    /// </summary>
    public partial class verseFrame : Page
    {
        private int sSureId, sRelativeVerseId, verseId, currentP, clearNav = 1, last = 0, starindex = 0;
        public int[] feedPoint = new int[4];
        private string navLocation, meaningPattern, intelWriter = "";
        private bool remiderType = false;
        private ArrayList findIndex = new ArrayList();

        public verseFrame()
        {
            InitializeComponent();
        }

        public object PageCall(int sureId, int relativeVerseId, string Location, string interpreter = "")
        {
            sSureId = sureId;
            sRelativeVerseId = relativeVerseId;
            navLocation = Location;
            intelWriter = interpreter;

            loadAni();
            App.loadTask = Task.Run(() => loadVerseFunc(relativeVerseId));

            return this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.mainScreen.navigationWriter("verse", loadHeader.Text + "," + sRelativeVerseId);
        }

        // ---------- LOAD FUNC ---------- //

        public void loadVerseFunc(int relativeVerseId)
        {
            // Verse Load Func
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dSure = entitydb.Sure.Where(p => p.sureId == sSureId).Select(p => new Sure { name = p.name, numberOfVerses = p.numberOfVerses, landingLocation = p.landingLocation, description = p.description, deskLanding = p.deskLanding, deskMushaf = p.deskMushaf }).First();
                    var dVerse = entitydb.Verse.Where(p => p.sureId == sSureId && p.relativeDesk == relativeVerseId).First();
                    verseId = (int)dVerse.verseId;

                    sRelativeVerseId = (int)dVerse.relativeDesk;
                    allPopupClosed();
                    loadNavFunc();
                    loadItemsHeader(dSure);
                    loadItemsControl(dVerse);
                    loadItemsContent(dVerse);
                    loadInterpreterFunc(intelWriter, sRelativeVerseId);

                    this.Dispatcher.Invoke(() =>
                    {
                        App.mainScreen.navigationWriter("verse", loadHeader.Text + "," + sRelativeVerseId);

                        mainContent.Visibility = Visibility.Visible;
                        if (dSure.name == "Fâtiha" && sRelativeVerseId == 1) NavUpdatePrevSingle.IsEnabled = false;
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
                    switch (writer)
                    {
                        case "Yorumcu 1":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 1);
                            break;

                        case "Yorumcu 2":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 2);
                            break;

                        case "Yorumcu 3":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 3);
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
                        loadItemsInterpreter(dInter);
                    }

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
                for (int x = 1; x < 9; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var vNav = (CheckBox)this.FindName("vb" + x);
                        vNav.Visibility = Visibility.Collapsed;
                        vNav.IsEnabled = true;
                    });
                }

                last = sRelativeVerseId;

                if (last % 9 == 0)

                {
                    last -= 2;
                }
                else
                {
                    if (sRelativeVerseId <= 8) last = 0;
                    else last -= 2;
                }

                if (prev != 0) last -= prev;
                else last -= 2;

                var dVerseNav = entitydb.Verse.Where(p => p.sureId == sSureId).Select(p => new Verse() { verseId = p.verseId, relativeDesk = p.relativeDesk, status = p.status, verseCheck = p.verseCheck }).Skip(last).Take(8).ToList();
                var tempVerse = new List<Verse>();

                int i = 0;

                foreach (var item in dVerseNav)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        i++;

                        var vNav = (CheckBox)this.FindName("vb" + i);

                        if (vNav != null)
                        {
                            vNav.Visibility = Visibility.Visible;
                            vNav.Uid = item.verseId.ToString();
                            vNav.IsChecked = item.verseCheck;
                            vNav.Content = item.relativeDesk;
                            vNav.Tag = item.status;
                            vNav.SetValue(Extensions.DataStorage, item.relativeDesk.ToString());
                        }
                    });
                }

                Thread.Sleep(300);

                for (int x = 1; x < 9; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var vNav = (CheckBox)this.FindName("vb" + x);
                        if (int.Parse((string)vNav.GetValue(Extensions.DataStorage).ToString()) == sRelativeVerseId) vNav.IsEnabled = false;
                    });
                }

                dVerseNav = null;
                tempVerse = null;
            }

            this.Dispatcher.Invoke(() =>
            {
                navstackPanel.Visibility = Visibility.Visible;

                controlPanel.Visibility = Visibility.Visible;
            });
        }

        public void loadItemsHeader(Sure dSure)
        {
            // items Load Func
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadHeader.Text = dSure.name;
                    loadLocation.Text = dSure.landingLocation;
                    loadDesc.Text = dSure.description;
                    loadVerseCount.Text = dSure.numberOfVerses.ToString();
                    loadDeskLanding.Text = dSure.deskLanding.ToString();
                    loadDeskMushaf.Text = dSure.deskMushaf.ToString();

                    headerBorder.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("LoadFunc", ex);
            }
        }

        public void singleItemsControl(int sR)
        {
            using (var entitydb = new AyetContext())
            {
                loadItemsControl(entitydb.Verse.Where(p => p.relativeDesk == sR && p.sureId == sSureId).First());
            }
        }

        public void loadItemsControl(Verse dVerse)
        {
            // items Load Func
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    markButton.IsChecked = dVerse.markCheck;
                    markButton.Uid = dVerse.verseId.ToString();
                    bellButton.IsChecked = dVerse.remiderCheck;
                    bellButton.Uid = dVerse.verseId.ToString();
                    noteButton.Uid = dVerse.verseId.ToString();
                    checkButton.IsChecked = dVerse.verseCheck;
                    checkButton.Uid = dVerse.verseId.ToString();
                    descButton.Uid = dVerse.verseDesc;
                    feedbackButton.Uid = dVerse.verseId.ToString();
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
                    loadVerseTr.Text = dVerse.verseTr;
                    loadVerseArb.Text = dVerse.verseArabic;
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
                    var dNotes = entitydb.Notes.Where(p => p.sureId == sSureId && p.verseId == sRelativeVerseId && p.noteLocation == "Ayet").ToList();
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
                    var dSubjectFolder = entitydb.Subject.OrderByDescending(p => p.subjectName).ToList();

                    this.Dispatcher.Invoke(() =>
                    {
                        selectedSubjectFolder.Items.Clear();

                        foreach (var item in dSubjectFolder)
                        {
                            var cmbitem = new ComboBoxItem();

                            cmbitem.Content = item.subjectName;
                            cmbitem.Uid = item.subjectId.ToString();
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
                switch ((string)btn.Uid)
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

                for (int x = 1; x < 9; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var vNav = (CheckBox)FindName("vb" + x);
                        vNav.IsEnabled = true;
                    });
                }

                clearNav = int.Parse(chk.GetValue(Extensions.DataStorage).ToString().Split('b').Last());

                currentP = int.Parse(chk.Content.ToString().Split(" ")[0]);
                navstackPanel.Visibility = Visibility.Hidden;

                controlPanel.Visibility = Visibility.Hidden;

                mainContent.Visibility = Visibility.Hidden;
                App.loadTask = Task.Run(() => loadVerseFunc(currentP));
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
                if (clearNav != 0) clearNav--;

                navstackPanel.Visibility = Visibility.Hidden;

                controlPanel.Visibility = Visibility.Hidden;
                mainContent.Visibility = Visibility.Hidden;
                if (int.Parse(loadVerseCount.Text) >= sRelativeVerseId && 1 < sRelativeVerseId)
                {
                    sRelativeVerseId--;
                    if (loadHeader.Text == "Fâtiha" && sRelativeVerseId == 1) NavUpdatePrevSingle.IsEnabled = false;
                    App.loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));
                }
                else
                {
                    if (App.currentDesktype == "DeskLanding")
                    {
                        int xc = 0;
                        using (var entitydb = new AyetContext())
                        {
                            var listx = entitydb.Sure.OrderBy(p => p.deskLanding);
                            foreach (var item in listx)
                            {
                                xc++;
                                if (loadHeader.Text == item.name) break;
                            }
                            xc--;
                            var listxc = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.deskLanding == xc).FirstOrDefault();

                            headerBorder.Visibility = Visibility.Hidden;
                            App.mainframe.Content = App.navVersePage.PageCall((int)listxc.sureId, (int)listxc.numberOfVerses, "Verse");

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
                            var BeforeD = entitydb.Sure.Where(p => p.sureId == selectedSure).Select(p => new Sure() { numberOfVerses = p.numberOfVerses }).FirstOrDefault();

                            headerBorder.Visibility = Visibility.Hidden;
                            App.mainframe.Content = App.navVersePage.PageCall(--sSureId, (int)BeforeD.numberOfVerses, "Verse");

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
                if (clearNav != 9) clearNav++;
                navstackPanel.Visibility = Visibility.Hidden;

                controlPanel.Visibility = Visibility.Hidden;

                mainContent.Visibility = Visibility.Hidden;

                if (int.Parse(loadVerseCount.Text) > sRelativeVerseId)
                {
                    NavUpdatePrevSingle.IsEnabled = true;
                    sRelativeVerseId++;
                    App.loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));
                }
                else
                {
                    if (App.currentDesktype == "DeskLanding")
                    {
                        using (var entitydb = new AyetContext())
                        {
                            int xc = 0;
                            var listx = entitydb.Sure.OrderBy(p => p.deskLanding);
                            foreach (var item in listx)
                            {
                                xc++;
                                if (loadHeader.Text == item.name) break;
                            }
                            xc++;
                            var listxc = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.deskLanding == xc).First();

                            headerBorder.Visibility = Visibility.Hidden;
                            App.mainframe.Content = App.navVersePage.PageCall((int)listxc.sureId, 1, "Verse");

                            listx = null;
                            listxc = null;
                        }
                    }
                    else
                    {
                        headerBorder.Visibility = Visibility.Hidden;
                        App.mainframe.Content = App.navVersePage.PageCall(++sSureId, 1, "Verse");
                    }
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
                    var verseUpdate = entitydb.Verse.Where(p => p.verseId == int.Parse(chk.Uid)).FirstOrDefault();
                    var sureUpdate = entitydb.Sure.Where(p => p.sureId == sSureId).FirstOrDefault();

                    if (chk.IsChecked.ToString() == "True")
                    {
                        verseUpdate.verseCheck = true;
                        verseUpdate.status = "#66E21F";
                        sureUpdate.userCheckCount++;

                        if (sureUpdate.userCheckCount == sureUpdate.numberOfVerses)
                        {
                            sureUpdate.complated = true;
                            if (entitydb.Sure.Where(p => p.userLastRelativeVerse != 0).Count() >= 1) sureUpdate.status = "#0D6EFD";
                            else sureUpdate.status = "#66E21F";
                        }
                    }
                    else
                    {
                        verseUpdate.verseCheck = false;
                        verseUpdate.status = "#FFFFFF";

                        sureUpdate.complated = false;
                        if (sureUpdate.userCheckCount != 0) sureUpdate.userCheckCount--;
                    }
                    entitydb.SaveChanges();

                    // App.loadTask = Task.Run(() => loadNavFunc());

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
                        var dRemider = entitydb.Remider.Where(p => p.connectSureId == sSureId && p.connectVerseId == sRelativeVerseId).First();

                        if (dRemider != null)
                        {
                            remiderNameControl.Text = dRemider.remiderName;

                            if (dRemider.status == "Wait")
                            {
                                TimeSpan tt = DateTime.Parse(dRemider.remiderDate.ToString("d")) - DateTime.Parse(DateTime.Now.ToString("d"));
                                remiderTimerControl.Text = "Hatırlatmaya Kalan Süre " + tt.TotalDays.ToString() + " Gün ";
                            }
                            else
                            {
                                string d = "";
                                switch (dRemider.loopType)
                                {
                                    case "Gün":
                                        d = "Günlük";
                                        break;

                                    case "Hafta":
                                        d = "Haftalık";
                                        break;

                                    case "Ay":
                                        d = "Aylık";
                                        break;

                                    case "Yıl":
                                        d = "Yıllık";
                                        break;
                                }
                                remiderTimerControl.Text = $"Hatırlatıcı {d} olarak ayarlanmıştır. ";
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
                var bchk = sender as CheckBox;
                using (var entitydb = new AyetContext())
                {
                    // Tüm Önceki işaretleri kaldı

                    var dVerse = entitydb.Verse.Where(p => p.markCheck == true).ToList();
                    var dSure = entitydb.Sure.Where(p => p.status == "#0D6EFD").ToList();
                    foreach (var item in dVerse)
                    {
                        item.markCheck = false;
                    }
                    foreach (var item in dSure)
                    {
                        if (item.complated != true) item.status = "#ADB5BD";
                        else item.status = "#66E21F";
                        item.userLastRelativeVerse = 0;
                    }

                    if (bchk.IsChecked.ToString() == "True")
                    {
                        entitydb.Verse.Where(p => p.verseId == int.Parse(bchk.Uid)).First().markCheck = true;
                        entitydb.Sure.Where(p => p.sureId == sSureId).First().status = "#0D6EFD";
                        entitydb.Sure.Where(p => p.sureId == sSureId).First().userLastRelativeVerse = sRelativeVerseId;
                    }
                    else
                    {
                        entitydb.Verse.Where(p => p.verseId == int.Parse(bchk.Uid)).First().markCheck = false;
                        if (entitydb.Sure.Where(p => p.sureId == sSureId).First().complated == true) entitydb.Sure.Where(p => p.sureId == sSureId).First().status = "#66E21F";
                        else entitydb.Sure.Where(p => p.sureId == sSureId).First().status = "#ADB5BD";
                    }

                    entitydb.SaveChanges();
                    singleItemsControl(sRelativeVerseId);
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
                var popuptemp = (Popup)FindName(btntemp.Uid);
                popuptemp.IsOpen = false;
                popupRelativeId.Text = "";

                meaningNameAddPopupHeaderError.Visibility = Visibility.Hidden;
                meaningCountAddPopupHeaderError.Visibility = Visibility.Hidden;
                meaningDetailAddPopupHeaderError.Visibility = Visibility.Hidden;

                noteAddPopupHeaderError.Visibility = Visibility.Hidden;
                noteAddPopupDetailError.Visibility = Visibility.Hidden;
                popupaddsubjectError.Visibility = Visibility.Hidden;
                subjectHeaderFolderError.Visibility = Visibility.Hidden;
                meaningDetailEditPopupHeaderError.Visibility = Visibility.Hidden;
                remiderAddPopupHeaderError.Visibility = Visibility.Hidden;
                remiderAddPopupDateError.Visibility = Visibility.Hidden;
                remiderAddPopupDetailError.Visibility = Visibility.Hidden;

                resultDataContent.Children.Clear();
                SearchData.Text = "";

                loopSelectedType.SelectedIndex = 0;

                remiderDetail.Text = "";
                popupRelativeId.Text = "";
                noteName.Text = "";
                noteDetail.Text = "";
                subjectFolderHeader.Text = "";
                remiderName.Text = "";
                meaningConnectNote.Text = "";
                meaningConnectVerse.Text = "0";
                subjectpreviewName.Text = "Önizleme";
                subjectFolderHeader.Text = "";

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
                App.loadTask = Task.Run(noteConnect);
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
                popup_remiderControlPopup.IsOpen = false;
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
                App.loadTask = Task.Run(() => subjectFolder());
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

        private void searchSubjectFolder_Click(object sender, RoutedEventArgs e)
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
                App.loadTask = Task.Run(() => loadMeaning());
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
                    controlPanel.Visibility = Visibility.Hidden;

                    mainContent.Visibility = Visibility.Hidden;

                    sRelativeVerseId = int.Parse(popupRelativeId.Text);
                    App.loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));

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
                                    if (entitydb.Notes.Where(p => p.noteHeader == noteName.Text && p.sureId == sSureId && p.verseId == sRelativeVerseId).FirstOrDefault() != null)
                                    {
                                        alertFunc("Not Ekleme Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", 3);
                                    }
                                    else
                                    {
                                        var dNotes = new Notes { noteHeader = noteName.Text, noteDetail = noteDetail.Text, sureId = sSureId, verseId = sRelativeVerseId, modify = DateTime.Now, created = DateTime.Now, noteLocation = "Ayet" };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        succsessFunc("Not Ekleme Başarılı", loadHeader.Text + " Surenin " + sRelativeVerseId + " Ayetine Not Eklendiniz.", 3);
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
                    var dNotes = entitydb.Notes.Where(p => p.sureId == sSureId && p.verseId == sRelativeVerseId).ToList();
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
                App.logWriter("PopupAction", ex);
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
                App.logWriter("Other", ex);
            }
        }

        private void addRemiderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (remiderName.Text.Length >= 5)
                {
                    if (remiderName.Text.Length < 150)
                    {
                        if (remiderDetail.Text.Length >= 5)
                        {
                            if (remiderType == false)
                            {
                                if (remiderDay.SelectedDate != null)
                                {
                                    using (var entitydb = new AyetContext())
                                    {
                                        var dControl = entitydb.Remider.Where(p => p.connectSureId == sSureId && p.connectVerseId == sRelativeVerseId).ToList();

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
                                            var newRemider = new Remider { connectSureId = sSureId, connectVerseId = sRelativeVerseId, remiderDate = (DateTime)remiderDay.SelectedDate, remiderDetail = remiderDetail.Text, remiderName = remiderName.Text, create = DateTime.Now, priority = 1, lastAction = DateTime.Now, status = "Run" };
                                            entitydb.Verse.Where(p => p.sureId == sSureId && p.relativeDesk == sRelativeVerseId).First().remiderCheck = true;
                                            entitydb.Remider.Add(newRemider);
                                            entitydb.SaveChanges();
                                            succsessFunc("Hatırlatıcı Oluşturuldu.", "Yeni hatırlatıcınız başarılı bir sekilde oluşturuldu", 3);

                                            popup_remiderAddPopup.IsOpen = false;
                                            remiderName.Text = "";
                                            remiderDetail.Text = "";
                                            remiderDay.SelectedDate = null;
                                            bellButton.IsChecked = true;

                                            App.loadTask = Task.Run(() => singleItemsControl(sRelativeVerseId));

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
                                        var dControl = entitydb.Remider.Where(p => p.connectSureId == sSureId && p.connectVerseId == sRelativeVerseId).ToList();

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
                                            var newRemider = new Remider { connectSureId = sSureId, connectVerseId = sRelativeVerseId, remiderDate = new DateTime(1, 1, 1, 0, 0, 0, 0), remiderDetail = remiderDetail.Text, remiderName = remiderName.Text, create = DateTime.Now, loopType = ditem.Uid.ToString(), status = "Run", priority = pr, lastAction = DateTime.Now };
                                            entitydb.Verse.Where(p => p.sureId == sSureId && p.relativeDesk == sRelativeVerseId).FirstOrDefault().remiderCheck = true;
                                            entitydb.Remider.Add(newRemider);
                                            entitydb.SaveChanges();
                                            succsessFunc("Hatırlatıcı Oluşturuldu.", "Yeni hatırlatıcınız başarılı bir sekilde oluşturuldu", 3);

                                            popup_remiderAddPopup.IsOpen = false;
                                            remiderName.Text = "";
                                            remiderDetail.Text = "";
                                            loopSelectedType.SelectedIndex = 0;

                                            App.loadTask = Task.Run(() => singleItemsControl(sRelativeVerseId));

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
                        remiderAddPopupHeaderError.Content = "Hatırlatıcı İsmi Çok Uzun. Max 150 Karakter Olabilir";
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
                    entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.connectVerseId == sRelativeVerseId && p.connectSureId == sSureId));
                    entitydb.Verse.Where(p => p.relativeDesk == sRelativeVerseId && p.sureId == sSureId).FirstOrDefault().remiderCheck = false;
                    entitydb.SaveChanges();
                    popup_remiderDeleteConfirm.IsOpen = false;
                    popup_remiderControlPopup.IsOpen = false;
                    App.loadTask = Task.Run(() => singleItemsControl(sRelativeVerseId));
                    succsessFunc("Hatırlatıcı Silindi ", " Hatırlatıcı başarılır bir sekilde silindi yeniden hatırlatıcı oluştura bilirsiniz...", 3);
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
                        var dControl = entitydb.SubjectItems.Where(p => p.sureId == sSureId && p.verseId == sRelativeVerseId && p.subjectId == int.Parse(item.Uid)).ToList();

                        if (dControl.Count != 0)
                        {
                            alertFunc("Ayet Ekleme Başarısız", "Bu ayet Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            var dSubjectItem = new SubjectItems { subjectId = int.Parse(item.Uid), subjectNotesId = 0, sureId = sSureId, verseId = sRelativeVerseId, created = DateTime.Now, modify = DateTime.Now, subjectName = loadHeader.Text + " Suresinin " + sRelativeVerseId.ToString() + " Ayeti" };
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
                if (subjectFolderHeader.Text.Length >= 3)
                {
                    if (subjectFolderHeader.Text.Length < 150)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Subject.Where(p => p.subjectName == subjectpreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dSubjectFolder = new Subject { subjectName = subjectpreviewName.Text, subjectColor = subjectpreviewColor.Background.ToString(), created = DateTime.Now, modify = DateTime.Now };
                                entitydb.Subject.Add(dSubjectFolder);
                                entitydb.SaveChanges();
                                succsessFunc("Konu Başlığı ", " Yeni konu başlığı oluşturuldu artık ayetleri ekleye bilirsiniz.", 3);

                                subjectpreviewName.Text = "";
                                subjectFolderHeader.Text = "";
                                popup_FolderSubjectPopup.IsOpen = false;
                                App.loadTask = Task.Run(() => subjectFolder());

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
                        subjectHeaderFolderError.Visibility = Visibility.Visible;
                        subjectHeaderFolderError.Content = "Konu başlığının çok uzun max 150 karakter olabilir";
                    }
                }
                else
                {
                    subjectFolderHeader.Focus();
                    subjectHeaderFolderError.Visibility = Visibility.Visible;
                    subjectHeaderFolderError.Content = "Konu başlığının uzunluğu minimum 3 karakter olmalı";
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
                    var dInteg = entitydb.Integrity.Where(p => p.integrityId == Int16.Parse(tmpbutton.Uid)).FirstOrDefault();

                    var dSure = entitydb.Sure.Where(p => p.sureId == dInteg.connectSureId).Select(p => new Sure() { name = p.name }).FirstOrDefault();
                    var dCSure = entitydb.Sure.Where(p => p.sureId == dInteg.connectedSureId).Select(p => new Sure() { name = p.name }).FirstOrDefault();
                    meaningOpenDetailText.Text = dInteg.integrityNote;
                    meaningDetailTextHeader.Text = dInteg.integrityName;

                    meaningDTempSureId.Text = dInteg.connectedSureId.ToString();
                    meaningDTempVerseId.Text = dInteg.connectedVerseId.ToString();
                    meaningDId.Text = dInteg.integrityId.ToString();
                    meaningOpenDetailTextConnect.Text = dSure.name + " Suresini " + dInteg.connectVerseId.ToString() + " Ayeti İçin " + dCSure.name + " " + dInteg.connectedVerseId + " Ayeti ile Eşleştirilmiş";
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

                        headerText.Text = item.integrityName.ToString();
                        noteText.Text = item.integrityNote.ToString();
                        allshowButton.Uid = item.integrityId.ToString();

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
                if (meaningName.Text.Length >= 3)
                {
                    if (meaningName.Text.Length < 150)
                    {
                        var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;

                        if (item.Uid != "0")
                        {
                            if (meaningConnectVerse.Text != "0" && meaningConnectVerse.Text != "")
                            {
                                if (int.Parse(meaningConnectVerse.Text) <= int.Parse(item.Tag.ToString()))
                                {
                                    if (meaningConnectNote.Text.Length >= 3)
                                    {
                                        var tmpcbxi = (ComboBoxItem)meaningpopupNextSureId.SelectedItem;

                                        using (var entitydb = new AyetContext())
                                        {
                                            if (entitydb.Integrity.Where(p => p.integrityName == meaningName.Text).FirstOrDefault() != null)
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
                                                var dIntegrity = new Integrity { integrityName = meaningName.Text, connectSureId = sSureId, connectVerseId = sRelativeVerseId, connectedSureId = int.Parse(tmpcbxi.Uid), connectedVerseId = int.Parse(meaningConnectVerse.Text), created = DateTime.Now, modify = DateTime.Now, integrityNote = meaningConnectNote.Text };
                                                entitydb.Integrity.Add(dIntegrity);
                                                entitydb.SaveChanges();
                                                succsessFunc("Bağlantı Oluşturuldu.", "Yeni bağlantınız oluşturuldu.", 3);
                                                meaningName.Text = "";
                                                meaningConnectNote.Text = "";
                                                meaningpopupNextSureId.SelectedIndex = 0;
                                                meaningConnectVerse.Text = "";
                                                popup_meaningAddPopup.IsOpen = false;
                                                App.loadTask = Task.Run(() => loadMeaning());

                                                dIntegrity = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        meaningDetailAddPopupHeaderError.Visibility = Visibility.Visible;
                                        meaningConnectNote.Focus();
                                        meaningDetailAddPopupHeaderError.Content = "Bağlantı Notu Yeterince Uzun Değil. Min 3 Karakter Olmalıdır";
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
                        meaningNameAddPopupHeaderError.Content = "Bağlantı Başlığı çok Uzun. Max 150 Karakter Olabilir";
                    }
                }
                else
                {
                    meaningNameAddPopupHeaderError.Visibility = Visibility.Visible;
                    meaningName.Focus();
                    meaningNameAddPopupHeaderError.Content = "Bağlantı Başlığı Yeterince Uzun Değil. Min 3 Karakter Olmalıdır";
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
                App.mainframe.Content = App.navVerseStickPage.PageCall(int.Parse(meaningDTempSureId.Text), int.Parse(meaningDTempVerseId.Text), loadHeader.Text, sRelativeVerseId);
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
                    if (meaningEditNote.Text.Length >= 3)
                    {
                        entitydb.Integrity.Where(p => p.integrityId == int.Parse(meaningDId.Text)).First().integrityNote = meaningEditNote.Text;
                        meaningOpenDetailText.Text = meaningEditNote.Text;
                        succsessFunc("Bağlantı Düzeltildi ", " Anlam Bütünlüğü Bağlantınızın Notu Düzeltildi...", 3);
                        popup_meaningEditPopup.IsOpen = false;
                    }
                    else
                    {
                        meaningDetailEditPopupHeaderError.Visibility = Visibility.Visible;
                        meaningEditNote.Focus();
                        meaningDetailEditPopupHeaderError.Content = "Anlam Bütünlüğü İçeriği Yeterince Uzun Değil. Min 3 Karakter Olmalıdır.";
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
                    entitydb.Integrity.RemoveRange(entitydb.Integrity.Where(p => p.integrityId == int.Parse(meaningDId.Text)));
                    entitydb.SaveChanges();
                    popup_meaningDeleteConfirm.IsOpen = false;
                    popup_meainingConnectDetailText.IsOpen = false;
                    App.loadTask = Task.Run(() => loadMeaning());

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
            using (var entitydb = new AyetContext())
            {
                foreach (var item in entitydb.Verse.Where(p => p.sureId == sSureId))
                {
                    item.verseCheck = false;
                    item.status = "#FFFFFF";
                }

                var sureUpdate = entitydb.Sure.Where(p => p.sureId == sSureId).FirstOrDefault();

                sureUpdate.complated = false;
                sureUpdate.userCheckCount = 0;

                if (entitydb.Sure.Where(p => p.sureId == sSureId && p.userLastRelativeVerse != 0).Count() >= 1)
                {
                    sureUpdate.status = "#0D6EFD";
                }
                else
                {
                    if (entitydb.Sure.Where(p => p.sureId == sSureId).First().complated == true) sureUpdate.status = "#66E21F";
                    sureUpdate.status = "#ADB5BD";
                }

                entitydb.SaveChanges();

                succsessFunc("İlerleme Silindi ", " Suredeki ilerlemeniz başarılı bir şekilde sıfırlandı...", 3);
                popup_resetVerseConfirm.IsOpen = false;

                navstackPanel.Visibility = Visibility.Hidden;
                App.loadTask = Task.Run(() => loadVerseFunc(1));
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                if (SearchData.Text.Length >= 3)
                {
                    resultTextHeader.Text = "Aranan Kelime veya Cümle : " + SearchData.Text;

                    int totalSearch = 0; ;
                    resultDataContent.Children.Clear();
                    var searchDt1 = entitydb.Verse.Where(p => p.sureId == sSureId && EF.Functions.Like(p.verseTr, "%" + SearchData.Text + "%")).Select(p => new Verse { verseTr = p.verseTr, relativeDesk = p.relativeDesk }).ToList();
                    var searchDt2 = entitydb.Verse.Where(p => p.sureId == sSureId && EF.Functions.Like(p.verseDesc, "%" + SearchData.Text + "%")).Select(p => new Verse { verseDesc = p.verseDesc, relativeDesk = p.relativeDesk }).ToList();
                    var searchDt3 = entitydb.Interpreter.Where(p => p.sureId == sSureId && EF.Functions.Like(p.interpreterDetail, "%" + SearchData.Text + "%")).Select(p => new Interpreter
                    {
                        interpreterDetail = p.interpreterDetail,
                        verseId = p.verseId,
                        interpreterWriter = p.interpreterWriter
                    }).ToList();

                    foreach (var searchContent in searchDt1)
                    {
                        searchDataContent.Text = searchContent.verseTr.Replace(Environment.NewLine, "");
                        starindex = 0;
                        findIndex.Clear();
                        while (true)
                        {
                            starindex = searchDataContent.Text.ToLower().IndexOf(SearchData.Text.ToLower(), ++starindex);
                            if (starindex == -1) break;
                            findIndex.Add(starindex);
                            totalSearch++;
                        }
                        searchDataWrite(findIndex, loadHeader.Text + " surenin " + searchContent.relativeDesk + ". ayetinde Türkçe Okunuşu", (int)searchContent.relativeDesk);
                    }

                    foreach (var searchContent in searchDt2)
                    {
                        searchDataContent.Text = searchContent.verseDesc.Replace(Environment.NewLine, "");
                        starindex = 0;
                        findIndex.Clear();
                        while (true)
                        {
                            starindex = searchDataContent.Text.ToLower().IndexOf(SearchData.Text.ToLower(), ++starindex);
                            if (starindex == -1) break;
                            findIndex.Add(starindex);
                            totalSearch++;
                        }
                        searchDataWrite(findIndex, loadHeader.Text + " surenin " + searchContent.relativeDesk + ". ayetinde açıklamasında", (int)searchContent.relativeDesk);
                    }

                    foreach (var searchContent in searchDt3)
                    {
                        searchDataContent.Text = searchContent.interpreterDetail.Replace(Environment.NewLine, "");
                        starindex = 0;
                        findIndex.Clear();
                        while (true)
                        {
                            starindex = searchDataContent.Text.ToLower().IndexOf(SearchData.Text.ToLower(), ++starindex);
                            if (starindex == -1) break;
                            findIndex.Add(starindex);
                            totalSearch++;
                        }
                        searchDataWrite(findIndex, loadHeader.Text + " surenin " + searchContent.verseId + ". ayetinin " + searchContent.interpreterWriter + " e ait yorumunda", (int)searchContent.verseId, searchContent.interpreterWriter);
                    }

                    totalResultText.Text = "Arama sonucunda " + totalSearch.ToString() + " eşleşen kelime veya cümle bulundu";
                    popup_search.IsOpen = true;
                }
                else
                {
                    SearchData.Focus();
                    alertFunc("Başarısız İşlem", "Arama yapmak için minimum 3 karakter girmeniz gerekemetedir.", 3);
                }
            }
        }

        private void searchDataWrite(ArrayList content, string Loc, int rId, string interpreter = "")
        {
            int id = 0;

            foreach (var item in content)
            {
                var dataTextBlock = new TextBlock();
                var tempLoc = new TextBlock();
                var tempborder = new Border();
                var tempstackpanel = new StackPanel();
                var tempButton = new Button();

                tempborder.Style = (Style)FindResource("pp_searchResultPanelBorder");
                tempLoc.Style = (Style)FindResource("pp_searchResultLocation");
                dataTextBlock.Style = (Style)FindResource("pp_searchResultText");
                tempButton.Style = (Style)FindResource("pp_dynamicItemSearchButton");
                tempstackpanel.HorizontalAlignment = HorizontalAlignment.Left;

                tempButton.Uid = rId.ToString();
                tempButton.Tag = interpreter;

                tempButton.Click += searchTempButonClick;

                id = int.Parse(item.ToString());

                id -= 20;

                if (id < 0)
                {
                    searchDataContent.SelectionStart = 0;
                    searchDataContent.SelectionLength = int.Parse(item.ToString());
                }
                else
                {
                    searchDataContent.SelectionStart = id;
                    searchDataContent.SelectionLength = 20;
                }

                dataTextBlock.Text = " ... " + searchDataContent.SelectedText;
                dataTextBlock.Inlines.Add(new Run(SearchData.Text) { Foreground = Brushes.Red });

                id = (SearchData.Text).Length + int.Parse(item.ToString());

                searchDataContent.SelectionStart = id;
                searchDataContent.SelectionLength = 20;

                dataTextBlock.Inlines.Add(searchDataContent.SelectedText + " ... ");

                tempLoc.Text = Loc;

                tempstackpanel.Children.Add(dataTextBlock);
                tempstackpanel.Children.Add(tempLoc);
                tempstackpanel.Children.Add(tempButton);
                tempborder.Child = tempstackpanel;

                resultDataContent.Children.Add(tempborder);

                dataTextBlock = null;
            }
        }

        private void searchTempButonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            popup_search.IsOpen = false;
            resultDataContent.Children.Clear();
            SearchData.Text = "";
            App.mainframe.Content = App.navVersePage.PageCall(sSureId, int.Parse(btn.Uid.ToString()), "Verse", btn.Tag.ToString());

            btn = null;
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
                        intelWriter = (string)item.Tag;
                        loadInterpreterFunc((string)item.Tag, sRelativeVerseId);
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
                subjectHeaderFolderError.Visibility = Visibility.Hidden;
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
                    // var dWords = entitydb.Words.Where(p => p.sureId == dSure[0].sureId).Where(p => p.verseId == dVerse[0].VerseId).ToList();

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

        public void allPopupClosed()
        {
            this.Dispatcher.Invoke(() =>
            {
                popup_VerseGoto.IsOpen = false;
                popup_Note.IsOpen = false;
                popup_noteAddPopup.IsOpen = false;
                popup_notesAllShowPopup.IsOpen = false;
                popup_addSubjectPopup.IsOpen = false;
                popup_FolderSubjectPopup.IsOpen = false;
                popup_Meaning.IsOpen = false;
                popup_meainingAllShowPopup.IsOpen = false;
                popup_meaningAddPopup.IsOpen = false;
                popup_meainingConnectDetailText.IsOpen = false;
                popup_meaningEditPopup.IsOpen = false;
                popup_meaningDeleteConfirm.IsOpen = false;
                popup_remiderAddPopup.IsOpen = false;
                popup_remiderControlPopup.IsOpen = false;
                popup_remiderDeleteConfirm.IsOpen = false;
                popup_descVersePopup.IsOpen = false;
                popup_resetVerseConfirm.IsOpen = false;
                popup_fastExitConfirm.IsOpen = false;
                popup_Words.IsOpen = false;
            });
        }

        // ---------- Simple Clear Fonks ---------- //

        // ---------- Animation Func ------------- //

        private void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                headerBorder.Visibility = Visibility.Hidden;
                controlPanel.Visibility = Visibility.Hidden;
                mainContent.Visibility = Visibility.Hidden;
                navstackPanel.Visibility = Visibility.Hidden;
            });
        }
    }
}