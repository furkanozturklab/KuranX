using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ReminderF;
using System;
using System.Collections.Generic;
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
        private string intelWriter = "", getSure;
        private int sSureId, sRelativeVerseId, getVerse, verseId, clearNav = 1, last = 0, currentP;

        public verseStickFrame()
        {
            InitializeComponent();
        }

        public Page PageCall(int sId, int vId, string getS, int gerR)
        {
            sSureId = sId;
            sRelativeVerseId = vId;
            getSure = getS;
            getVerse = gerR;
            App.loadTask = Task.Run(() => loadVerseFunc(vId));
            return this;
        }

        // ---------- Load Func ---------- //

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

                    loadSure.Text = getSure;
                    loadVerse.Text = getVerse.ToString();

                    headerBorder.Visibility = Visibility.Visible;
                });
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
            });
        }

        public void loadInterpreterFunc(string writer, int verseId)
        {
            // İnterpreter Load Func
            try
            {
                using (var entitydb = new AyetContext())
                {
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

        // ---------- Load Func ---------- //

        // ---------- Click Func -------------- //

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
                App.logWriter("Other", ex);
            }
        }

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
            NavigationService.GoBack();
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

        private void loadVersePopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                    alertFunc("Ayet Mevcut Değil", "Ayet Sayısını Geçtiniz Gidile Bilecek Son Ayet Sayısı : " + loadVerseCount.Text + " Lütfen Tekrar Deneyiniz.", 3);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        // ---------- Click Func -------------- //

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

        // ---------- SelectionChanged FUNC ---------- //

        // ----------- Other Func ------------ //

        public void allPopupClosed()
        {
            this.Dispatcher.Invoke(() =>
            {
                popup_Words.IsOpen = false;
            });
        }

        // ----------- Other Func ------------ //

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

        // ---------- MessageFunc FUNC ---------- //
    }
}