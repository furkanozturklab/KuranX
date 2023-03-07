using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ReminderF;
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

namespace KuranX.App.Core.Pages.VerseF
{
    /// <summary>
    /// Interaction logic for verseStickFrame.xaml
    /// </summary>
    public partial class verseStickFrame : Page
    {
        private string intelWriter = App.InterpreterWriter, getSure = "", sLocation = "";
        private int sSureId, sRelativeVerseId, getVerse, verseId, clearNav = 1, last = 0, currentP;

        public verseStickFrame()
        {
            try
            {

                App.errWrite($"[{DateTime.Now} InitializeComponent ] -> verseStickFrame");

                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall(int sId, int vId, string getS, int gerR, string location = "default")
        {
            try
            {

                App.errWrite($"[{DateTime.Now} PageCall ] -> verseStickFrame");


                stickHomeGrid.Visibility = Visibility.Collapsed;
                sSureId = sId;
                sRelativeVerseId = vId;
                sLocation = location;

                getSure = getS;
                getVerse = gerR;


                intelWriter = App.InterpreterWriter;
                App.loadTask = Task.Run(() => loadVerseFunc(vId));

                App.lastlocation = "verseStickFrame";

                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
                return this;
            }
        }

        // ---------- Load Func ---------- //

        public void loadVerseFunc(int relativeVerseId)
        {
            // Verse Load Func
            try
            {


                App.errWrite($"[{DateTime.Now} loadVerseFunc ] -> verseStickFrame");


                using (var entitydb = new AyetContext())
                {
                    var dSure = entitydb.Sure.Where(p => p.sureId == sSureId).Select(p => new Sure { name = p.name, numberOfVerses = p.numberOfVerses, landingLocation = p.landingLocation, description = p.description, deskLanding = p.deskLanding, deskMushaf = p.deskMushaf }).First();
                    var dVerse = entitydb.Verse.Where(p => p.sureId == sSureId && p.relativeDesk == relativeVerseId).First();
                    var dCVerse = entitydb.VerseClass.Where(p => p.sureId == sSureId && p.relativeDesk == relativeVerseId).First();

                    verseId = (int)dVerse.verseId;

                    sRelativeVerseId = (int)dVerse.relativeDesk;
                    allPopupClosed();
                    loadNavFunc();
                    loadItemsHeader(dSure);
                    loadItemsClass(dCVerse);
                    loadItemsContent(dVerse);
                    loadInterpreterFunc(intelWriter, sRelativeVerseId);

                    this.Dispatcher.Invoke(() =>
                    {
                        if (relativeVerseId >= int.Parse(loadVerseCount.Text)) NavUpdateNextSingle.IsEnabled = false;
                        else NavUpdateNextSingle.IsEnabled = true;
                        if (relativeVerseId == 1) NavUpdatePrevSingle.IsEnabled = false;
                    });

                    this.Dispatcher.Invoke(() =>
                    {
                        mainContent.Visibility = Visibility.Visible;
                        if (dSure.name == "Fâtiha" && sRelativeVerseId == 1) NavUpdatePrevSingle.IsEnabled = false;
                        else NavUpdatePrevSingle.IsEnabled = true;

                        stickHomeGrid.Visibility = Visibility.Visible;

                        App.mainScreen.homescreengrid.IsEnabled = true;
                    });

                    dSure = null;
                    dVerse = null;
                }


            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        public void loadItemsContent(Verse dVerse)
        {
            // items Load Func
            try
            {
                App.errWrite($"[{DateTime.Now} loadItemsContent ] -> verseStickFrame");


                this.Dispatcher.Invoke(() =>
                {
                    loadVerseTr.Text = dVerse.verseTr;
                    loadVerseArb.Text = dVerse.verseArabic;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        public void loadItemsHeader(Sure dSure)
        {
            // items Load Func
            try
            {
                App.errWrite($"[{DateTime.Now} loadItemsHeader ] -> verseStickFrame");


                this.Dispatcher.Invoke(() =>
                {
                    loadHeader.Text = dSure.name;
                    loadLocation.Text = dSure.landingLocation;
                    loadDesc.Text = dSure.description;
                    loadVerseCount.Text = dSure.numberOfVerses.ToString();
                    loadDeskLanding.Text = dSure.deskLanding.ToString();
                    loadDeskMushaf.Text = dSure.deskMushaf.ToString();

                    if (sLocation != "default")
                    {
                        switch (sLocation)
                        {
                            case "Note":

                                getSText.Text = "Gelinene Yer";
                                loadSure.Text = "Notlar";
                                getR.Visibility = Visibility.Hidden;
                                break;

                            case "Meaning":

                                getSText.Text = "Gelinene Yer";
                                loadSure.Text = getSure;
                                loadVerse.Text = getVerse.ToString();
                                break;

                            case "Subject":
                                getSText.Text = "Gelinene Yer";
                                loadSure.Text = "Konular";
                                getR.Visibility = Visibility.Hidden;
                                break;

                            case "Remider":
                                getSText.Text = "Gelinene Yer";
                                loadSure.Text = "Hatırlatıcı";
                                getR.Visibility = Visibility.Hidden;
                                break;

                            case "Search":
                                getSText.Text = "Gelinene Yer";
                                loadSure.Text = "Arama";
                                getR.Visibility = Visibility.Hidden;
                                break;
                            case "FastRemider":
                                getSText.Text = "Gelinene Yer";
                                loadSure.Text = "Hatırlatıcı";
                                getR.Visibility = Visibility.Hidden;
                                break;

                            case "Verse":

                                loadSure.Text = getSure;
                                loadVerse.Text = getVerse.ToString();
                                break;
                        }
                    }

                    headerBorder.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        public void loadNavFunc(int prev = 0)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} loadNavFunc ] -> verseStickFrame");



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

                    var dVerseNav = entitydb.Verse.Where(p => p.sureId == sSureId).Select(p => new Verse() { verseId = p.verseId, relativeDesk = p.relativeDesk, verseCheck = p.verseCheck }).Skip(last).Take(8).ToList();
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
                                if (item.verseCheck)
                                {
                                    vNav.Tag = "#66E21F";
                                }
                                else
                                {
                                    vNav.Tag = "#FFFFFF";
                                }
                                vNav.SetValue(Extensions.DataStorage, item.relativeDesk.ToString());
                            }
                        });
                    }

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

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
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        public void loadItemsClass(VerseClass dCVerse)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} loadItemsClass ] -> verseStickFrame");


                this.Dispatcher.Invoke(() =>
                {
                    class1.IsEnabled = dCVerse.v_hk;
                    class2.IsEnabled = dCVerse.v_tv;
                    class3.IsEnabled = dCVerse.v_cz;
                    class4.IsEnabled = dCVerse.v_mk;
                    class5.IsEnabled = dCVerse.v_du;
                    class6.IsEnabled = dCVerse.v_hr;
                    class7.IsEnabled = dCVerse.v_sn;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        public void loadInterpreterFunc(string writer, int verseId)
        {
            // İnterpreter Load Func
            // İnterpreter Load Func
            try
            {
                App.errWrite($"[{DateTime.Now} loadInterpreterFunc ] -> verseStickFrame");


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

                App.errWrite($"[{DateTime.Now} loadItemsInterpreter ] -> verseStickFrame");
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

        // ---------- Load Func ---------- //

        // ---------- Click Func -------------- //

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} popupClosed_Click ] -> verseStickFrame");


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

        private void changeloadDetail_Click(object sender, EventArgs e)
        {
            // TR ARP İNTERPRETER TEXT CHANGE FUNC
            try
            {

                App.errWrite($"[{DateTime.Now} changeloadDetail_Click ] -> verseStickFrame");



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
                App.logWriter("Click", ex);
            }
        }

        private void activeVerseSelected_Click(object sender, EventArgs e)
        {
            // Verse Change Click
            try
            {

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = false);

                App.errWrite($"[{DateTime.Now} activeVerseSelected_Click ] -> verseStickFrame");



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

                mainContent.Visibility = Visibility.Hidden;
                App.loadTask = Task.Run(() => loadVerseFunc(currentP));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void NavUpdatePrevSingle_Click(object sender, EventArgs e)
        {
            // Nav PrevSingle Click
            try
            {

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = false);
                App.errWrite($"[{DateTime.Now} NavUpdatePrevSingle_Click ] -> verseStickFrame");


                if (clearNav != 0) clearNav--;

                navstackPanel.Visibility = Visibility.Hidden;
                mainContent.Visibility = Visibility.Hidden;

                if (int.Parse(loadVerseCount.Text) >= sRelativeVerseId && 1 < sRelativeVerseId)
                {
                    sRelativeVerseId--;
                    App.loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void NavUpdateNextSingle_Click(object sender, EventArgs e)
        {
            // Nav NextSingle Click
            try
            {
                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = false);

                App.errWrite($"[{DateTime.Now} NavUpdateNextSingle_Click ] -> verseStickFrame");


                if (clearNav != 9) clearNav++;
                navstackPanel.Visibility = Visibility.Hidden;

                mainContent.Visibility = Visibility.Hidden;

                if (int.Parse(loadVerseCount.Text) > sRelativeVerseId)
                {
                    NavUpdatePrevSingle.IsEnabled = true;
                    sRelativeVerseId++;
                    App.loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void backVersesFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                App.errWrite($"[{DateTime.Now} backVersesFrame_Click ] -> verseStickFrame");


                switch (sLocation)
                {
                    case "Remider":
                        App.navRemiderItem.loadHeaderStack.Visibility = Visibility.Visible;
                        App.navRemiderItem.controlBar.Visibility = Visibility.Visible;
                        App.navRemiderItem.remiderDetail.Visibility = Visibility.Visible;
                        break;

                    case "FastRemider":
                        App.secondFrame.Visibility = Visibility.Collapsed;
                        break;

                    case "Verse":

                        App.navVersePage.headerBorder.Visibility = Visibility.Visible;
                        App.navVersePage.controlPanel.Visibility = Visibility.Visible;
                        App.navVersePage.classPanel.Visibility = Visibility.Visible;
                        App.navVersePage.mainContent.Visibility = Visibility.Visible;
                        App.navVersePage.navControlStack.Visibility = Visibility.Visible;
                        App.navVersePage.actionsControlGrid.Visibility = Visibility.Visible;

                        break;

                    case "Search":

                        App.navVersePage.headerBorder.Visibility = Visibility.Visible;
                        App.navVersePage.controlPanel.Visibility = Visibility.Visible;
                        App.navVersePage.classPanel.Visibility = Visibility.Visible;
                        App.navVersePage.mainContent.Visibility = Visibility.Visible;
                        App.navVersePage.mainContent.Visibility = Visibility.Visible;
                        App.navVersePage.navControlStack.Visibility = Visibility.Visible;
                        App.navVersePage.actionsControlGrid.Visibility = Visibility.Visible;
                        if (App.navVersePage.searchBox) App.navVersePage.popup_search.IsOpen = true;
                        break;

                    case "Meaning":

                        App.navVersePage.headerBorder.Visibility = Visibility.Visible;
                        App.navVersePage.controlPanel.Visibility = Visibility.Visible;
                        App.navVersePage.classPanel.Visibility = Visibility.Visible;
                        App.navVersePage.mainContent.Visibility = Visibility.Visible;
                        App.navVersePage.mainContent.Visibility = Visibility.Visible;
                        App.navVersePage.navControlStack.Visibility = Visibility.Visible;
                        App.navVersePage.actionsControlGrid.Visibility = Visibility.Visible;
                        if (App.navVersePage.connectBox) App.navVersePage.popup_Meaning.IsOpen = true;
                        break;
                }

                App.secondFrame.Visibility = Visibility.Collapsed;
                stickHomeGrid.Visibility = Visibility.Collapsed;
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
                App.errWrite($"[{DateTime.Now} wordText_Click ] -> verseStickFrame");


                popup_Words.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dWords = entitydb.Words.Where(p => p.sureId == sSureId && p.verseId == sRelativeVerseId).ToList();

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

        private void openVerseNumberPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} openVerseNumberPopup_Click ] -> verseStickFrame");



                popup_VerseGoto.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void loadVersePopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} loadVersePopup_Click ] -> verseStickFrame");


                if (int.Parse(loadVerseCount.Text) >= int.Parse(popupRelativeId.Text))
                {
                    mainContent.Visibility = Visibility.Hidden;

                    sRelativeVerseId = int.Parse(popupRelativeId.Text);
                    App.loadTask = Task.Run(() => loadVerseFunc(sRelativeVerseId));

                    popup_VerseGoto.IsOpen = false;
                    popupRelativeId.Text = "";
                }
                else
                {
                    App.mainScreen.alertFunc("İşlem Başarısız", "Ayet sayısını geçtiniz gidile bilecek son ayet sayısı : " + loadVerseCount.Text + " lütfen tekrar deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // ---------- Click Func -------------- //

        // ---------- SelectionChanged FUNC ---------- //

        private void interpreterWriterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Nav NextUpdate Click
            try
            {
                App.errWrite($"[{DateTime.Now} interpreterWriterCombo_SelectionChanged ] -> verseStickFrame");

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
                App.logWriter("Change", ex);
            }
        }

        private void popupRelativeId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void popupRelativeId_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        // ---------- SelectionChanged FUNC ---------- //

        // ----------- Other Func ------------ //

        public void allPopupClosed()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} allPopupClosed ] -> verseStickFrame");


                this.Dispatcher.Invoke(() =>
                {
                    popup_Words.IsOpen = false;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Closed", ex);
            }
        }

        // ----------- Other Func ------------ //

        private void loadAni()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} loadAni ] -> verseStickFrame");


                this.Dispatcher.Invoke(() =>
                {
                    headerBorder.Visibility = Visibility.Hidden;
                    mainContent.Visibility = Visibility.Hidden;
                    navstackPanel.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Aniamtion", ex);
            }
        }
    }
}