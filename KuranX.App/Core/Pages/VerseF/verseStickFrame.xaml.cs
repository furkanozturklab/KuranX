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
        private string intelWriter = "", getSure = "";
        private int sSureId, sRelativeVerseId, getVerse, verseId, clearNav = 1, last = 0, currentP;

        public verseStickFrame()
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

        public Page PageCall(int sId, int vId, string getS, int gerR)
        {
            try
            {
                sSureId = sId;
                sRelativeVerseId = vId;
                getSure = getS;
                getVerse = gerR;
                App.loadTask = Task.Run(() => loadVerseFunc(vId));
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
                App.logWriter("Loading", ex);
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
                App.logWriter("Loading", ex);
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
                App.logWriter("Loading", ex);
            }
        }

        public void loadNavFunc(int prev = 0)
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        public void loadInterpreterFunc(string writer, int verseId)
        {
            // İnterpreter Load Func
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

                        case "Mehmet Okuyan":
                            this.Dispatcher.Invoke(() => interpreterWriterCombo.SelectedIndex = 4);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
            }
        }

        private void backVersesFrame_Click(object sender, RoutedEventArgs e)
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

        private void wordText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                    App.mainScreen.alertFunc("Ayet Mevcut Değil", "Ayet Sayısını Geçtiniz Gidile Bilecek Son Ayet Sayısı : " + loadVerseCount.Text + " Lütfen Tekrar Deneyiniz.", 3);
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