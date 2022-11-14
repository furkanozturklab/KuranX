using KuranX.App.Core.Classes;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.VerseF
{
    /// <summary>
    /// Interaction logic for stickVerseFrame.xaml
    /// </summary>
    public partial class stickVerseFrame : Page
    {
        private List<Sure>? dSure = new List<Sure>();
        private List<Verse>? dVerse, dVerseNav, tempVerses = new List<Verse>();
        private List<Integrity>? tempInteg = new List<Integrity>();
        private List<Interpreter>? dInterpreter = new List<Interpreter>();
        private Task? PageItemLoadTask;
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private int activeSure, activeVerse, relativeVerseDesk, tempDataInt = 0, lastVerse = 0, getpopupRelativeId, relativeDeskV, singlerelativeDesk = 1, singlenextnavcontrol = 1;
        private string InterpreterWrite, verseName, navLocation;
        private bool tempCheck = false;
        private CheckBox? chk;

        public stickVerseFrame()
        {
            InitializeComponent();
        }

        public stickVerseFrame(int sureId, int verseId, string CurrentSure, string CurrentVerseId, string navInfoStatus) : this()
        {
            try
            {
                activeSure = sureId;
                activeVerse = verseId;

                Debug.WriteLine("initialize sure : " + activeSure);
                Debug.WriteLine("initialize verse : " + activeVerse);

                switch (navInfoStatus)
                {
                    case "Visible":
                        navInfo.Visibility = Visibility.Visible;
                        break;

                    case "Hidden":
                        navInfo.Visibility = Visibility.Hidden;
                        break;

                    case "Collapsed":
                        navInfo.Visibility = Visibility.Collapsed;
                        break;
                }

                val1.Text = CurrentSure;
                val2.Text = CurrentVerseId.ToString();

                relativeDeskV = verseId;
                singlerelativeDesk = verseId;
                this.navLocation = navLocation;
                activeSure = sureId;
                activeVerse = 1;

                if (verseId <= 7) lastVerse = 0;
                else lastVerse = --relativeDeskV;

                using (var entitydb = new AyetContext())
                {
                    dSure = (List<Sure>)entitydb.Sure.Where(p => p.sureId == activeSure).ToList();
                    dVerseNav = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Select(p => new Verse() { VerseId = p.VerseId, RelativeDesk = p.RelativeDesk, Status = p.Status, VerseCheck = p.VerseCheck }).Skip(lastVerse).Take(8).ToList();

                    verseName = (string)dSure[0].Name;
                    if (verseId <= 7) activeVerse = (int)dVerseNav[--relativeDeskV].VerseId;
                    else activeVerse = (int)dVerseNav[0].VerseId;
                    tempDataInt = activeVerse;
                    tempDataInt--;
                    CurrentVerse.Tag = dVerseNav[0].RelativeDesk.ToString();
                    relativeVerseDesk = (int)dVerseNav[0].RelativeDesk;

                    dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.VerseId == activeVerse).ToList();
                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();

                    if (dSure[0].Name == "Fâtiha") NavUpdatePrevSingle.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        //-------------------------- LOAD FUNC  --------------------------//

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PageItemLoadTask = new Task(loadVerse);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void loadInterpreter()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();

                    versesFullTextData.Dispatcher.Invoke(() =>
                    {
                        versesFullTextData.ItemsSource = dInterpreter;
                    });
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void specialLoad()
        {
            singleLoadVerse();
            loadVerse();
        }

        private void singleLoadVerse()
        {
            try
            {
                dsingleLoadVerseAni();
                using (var entitydb = new AyetContext())
                {
                    if (lastVerse < 7) lastVerse = 0;

                    dVerseNav = (List<Verse>)entitydb.Verse.Where(p => p.SureId == dSure[0].sureId).Select(p => new Verse() { VerseId = p.VerseId, RelativeDesk = p.RelativeDesk, Status = p.Status, VerseCheck = p.VerseCheck }).Skip(lastVerse).Take(8).ToList();

                    int i = 1;
                    for (int x = 1; x < 8; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            ItemsControl itemslist = (ItemsControl)this.FindName("vb" + x);
                            //itemslist.Items.Clear();
                            itemslist.ItemsSource = null;
                        });
                    }

                    foreach (var item in dVerseNav)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            tempVerses.Add(item);
                            ItemsControl itemslist = (ItemsControl)this.FindName("vb" + i);
                            itemslist.ItemsSource = tempVerses;
                            tempVerses.Clear();
                            i++;
                        });

                        if (i == 8) break; // 7 den fazla varmı kontrol etmek için koydum
                    }

                    NavUpdatePrev.Dispatcher.Invoke(() =>
                    {
                        if (lastVerse == 0) NavUpdatePrev.IsEnabled = false;
                        else NavUpdatePrev.IsEnabled = true;
                    });

                    NavUpdateNext.Dispatcher.Invoke(() =>
                    {
                        if (dVerseNav.Count() <= 7) NavUpdateNext.IsEnabled = false;
                        if (dVerseNav.Count() > 7) NavUpdateNext.IsEnabled = true;
                    });

                    Thread.Sleep(200);
                }
                dsingleLoadVerseAniComplated();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void popuploadVerse()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.RelativeDesk == getpopupRelativeId).ToList();

                    activeVerse = (int)dVerse[0].VerseId;
                    tempDataInt = activeVerse;
                    tempDataInt--;

                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();

                    this.Dispatcher.Invoke(() =>
                    {
                        versesTextData.ItemsSource = dVerse;
                        versesFullTextData.ItemsSource = dInterpreter;

                        CurrentVerse.Tag = dVerse[0].RelativeDesk.ToString();
                        relativeVerseDesk = (int)dVerse[0].RelativeDesk;
                    });

                    Thread.Sleep(200);

                    lastVerse = getpopupRelativeId;
                    lastVerse--;
                    // if (dVerse[0].RelativeDesk >= 8)
                    singleLoadVerse();
                    singlerelativeDesk = (int)dVerse[0].RelativeDesk;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void loadVerse()
        {
            try
            {
                dloadAni();
                using (var entitydb = new AyetContext())
                {
                    Debug.WriteLine("LOAD sure : " + activeSure);
                    Debug.WriteLine("LOAD verse : " + activeVerse);

                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();
                    dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.VerseId == activeVerse).ToList();

                    this.Dispatcher.Invoke(() =>
                    {
                        versesFullTextData.ItemsSource = dInterpreter;
                        versesTextData.ItemsSource = dVerse;

                        sureName.Text = dSure[0].Name;
                        landingLocation.Text = dSure[0].LandingLocation;
                        NumberOfVerses.Text = dSure[0].NumberOfVerses.ToString();
                        deskMushaf.Text = dSure[0].DeskMushaf.ToString();
                        deskLanding.Text = dSure[0].DeskLanding.ToString();
                    });

                    int i = 1;

                    for (int x = 1; x < 8; x++)
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
                            tempVerses.Add(item);
                            ItemsControl itemslist = (ItemsControl)this.FindName("vb" + i);
                            itemslist.ItemsSource = tempVerses;
                            tempVerses.Clear();
                            i++;
                        });

                        if (i == 8) break; // 7 den fazla varmı kontrol etmek için koydum
                    }
                }
                Thread.Sleep(200);

                this.Dispatcher.Invoke(() =>
                {
                    // NavUpdateNext
                    if (dVerseNav.Count() <= 7) NavUpdateNext.IsEnabled = false;
                    else NavUpdateNext.IsEnabled = true;

                    // NavUpdatePrev

                    if (lastVerse == 0) NavUpdatePrev.IsEnabled = false;
                    else NavUpdatePrev.IsEnabled = true;

                    // Min Ayet Fatiha

                    // CurrentVerse
                    CurrentVerse.Tag = dVerse[0].RelativeDesk.ToString();
                    relativeVerseDesk = (int)dVerse[0].RelativeDesk;

                    // Location Nav Content
                });

                singlerelativeDesk = (int)dVerse[0].RelativeDesk;
                pageLoadAniComplated();
                dloadAniComplated();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void dloadVerse()
        {
            dloadAni();
            using (var entitydb = new AyetContext())
            {
                dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.RelativeDesk == singlerelativeDesk).ToList();
                int tempInterId = (int)dVerse[0].VerseId;
                tempInterId--;
                dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempInterId).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();

                this.Dispatcher.Invoke(() =>
                {
                    versesFullTextData.ItemsSource = dInterpreter;
                    versesTextData.ItemsSource = dVerse;
                });

                int i = 1;

                for (int x = 1; x < 8; x++)
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
                        tempVerses.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("vb" + i);
                        itemslist.ItemsSource = tempVerses;
                        tempVerses.Clear();
                        i++;
                    });

                    if (i == 8) break; // 7 den fazla varmı kontrol etmek için koydum
                }

                this.Dispatcher.Invoke(() =>
                {
                });

                Thread.Sleep(200);
                CurrentVerse.Dispatcher.Invoke(() =>
                {
                    CurrentVerse.Tag = dVerse[0].RelativeDesk.ToString();
                    relativeVerseDesk = (int)dVerse[0].RelativeDesk;
                });
            }
            dloadAniComplated();
        }

        //-------------------------- LOAD FUNC  --------------------------//

        //-------------------------- ACTİONS FUNC  --------------------------//

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

        private void FullTextClear()
        {
            versesFullTextData.Visibility = Visibility.Collapsed;
            versesTrDataExtends.Visibility = Visibility.Collapsed;
            versesArDataExtends.Visibility = Visibility.Collapsed;
            backInterpreter.Visibility = Visibility.Visible;
        }

        private void trFullTextLoad(object sender, EventArgs e)
        {
            try
            {
                FullTextClear();
                versesTrDataExtends.Visibility = Visibility.Visible;
                versesTrDataExtendsText.Text = dVerse[0].VerseTr;
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
                versesArDataExtendsText.Text = dVerse[0].VerseArabic;
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
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

        private void wordText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wordDetailPopup.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dWords = entitydb.Words.Where(p => p.SureId == dSure[0].sureId).Where(p => p.VerseId == dVerse[0].VerseId).ToList();

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

        private void backVersesFrame_Click(object sender, RoutedEventArgs e)
        {
            if (navLocation == "Verse")
            {
                // App.selectedBlock = false;
                //  App.mainframe.Content = new versesFrame(App.currentVersesPageD[0], App.currentVersesPageD[1], App.currentLanding);
            }
            else NavigationService.GoBack();
        }

        private void openVerseNumberPopup_Click(object sender, EventArgs e)
        {
            try
            {
                comitVersePopup.IsOpen = true;
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
                comitVersePopup.IsOpen = false;
                if (dSure[0].NumberOfVerses >= Int16.Parse(popupRelativeId.Text.ToString()))
                {
                    getpopupRelativeId = Int16.Parse(popupRelativeId.Text.ToString());
                    popupRelativeId.Text = "";
                    PageItemLoadTask.Dispose();
                    PageItemLoadTask = new Task(popuploadVerse);
                    PageItemLoadTask.Start();
                }
                else
                {
                    alertFunc("Ayet Mevcut Değil", "Ayet Sayısını Geçtiniz Gidile Bilecek Son Ayet Sayısı : " + dSure[0].NumberOfVerses.ToString() + " Lütfen Tekrar Deneyiniz.", 3);
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

        //-------------------------- ACTİONS FUNC  --------------------------//

        // -------------------------- Verse Nav Controls -------------------------- //

        private void bottomNavUpdatePrev(object sender, EventArgs e)
        {
            try
            {
                if (lastVerse >= 7)
                {
                    NavUpdatePrev.IsEnabled = false;
                    lastVerse -= 7;
                    singlerelativeDesk = lastVerse;
                    PageItemLoadTask = new Task(singleLoadVerse);
                    PageItemLoadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void bottomNavUpdateNext(object sender, EventArgs e)
        {
            try
            {
                NavUpdateNext.IsEnabled = false;
                lastVerse += 7;
                singlerelativeDesk = lastVerse;
                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(singleLoadVerse);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void NavUpdateNextSingle_Click(object sender, RoutedEventArgs e)
        {
            if (dSure[0].NumberOfVerses > singlerelativeDesk)
            {
                singlerelativeDesk++;
                NavUpdatePrevSingle.IsEnabled = true;

                PageItemLoadTask = new Task(dloadVerse);
                PageItemLoadTask.Start();

                lastVerse++;

                if (singlerelativeDesk == 8)
                {
                    lastVerse = 7;
                }

                PageItemLoadTask = new Task(singleLoadVerse);
                PageItemLoadTask.Start();
            }
            else
            {
                if (App.currentDesktype == "DeskMushaf")
                {
                    int xc = 0;
                    using (var entitydb = new AyetContext())
                    {
                        var listx = entitydb.Sure.OrderBy(p => p.DeskMushaf);

                        foreach (var item in listx)
                        {
                            xc++;
                            if (dSure[0].Name == item.Name) break;
                        }

                        xc++;
                        var listxc = entitydb.Sure.OrderBy(p => p.DeskMushaf).Where(p => p.DeskMushaf == xc).FirstOrDefault();

                        App.mainframe.Content = new verseFrame((int)listxc.sureId, 1, "Verse");
                    }
                }
                else
                {
                    App.mainframe.Content = new verseFrame((int)++dSure[0].sureId, 1, "Verse");
                }
            }
        }

        private void NavUpdatePrevSingle_Click(object sender, RoutedEventArgs e)
        {
            singlerelativeDesk--;

            if (singlerelativeDesk == -1) singlerelativeDesk = 0;
            if (singlerelativeDesk == 0)
            {
            }
            else
            {
                if (dSure[0].Name == "Fâtiha" && singlerelativeDesk == 1) NavUpdatePrevSingle.IsEnabled = false;
                if (lastVerse != 0)
                {
                    lastVerse--;
                    PageItemLoadTask = new Task(singleLoadVerse);
                    PageItemLoadTask.Start();
                }
                if (singlerelativeDesk != 0)
                {
                    PageItemLoadTask = new Task(dloadVerse);
                    PageItemLoadTask.Start();
                }
            }
        }

        private void activeVerseSelected_Click(object sender, EventArgs e)
        {
            try
            {
                chk = sender as CheckBox;
                //if (lastCurrentBtn != null) lastCurrentBtn.Style = (Style)FindResource("versesNavButtonV");
                FullTextClear();
                versesFullTextData.Visibility = Visibility.Visible;
                backInterpreter.Visibility = Visibility.Collapsed;

                if (chk.IsChecked.ToString() == "True") chk.IsChecked = false;
                else { chk.IsChecked = true; }

                activeVerse = Int16.Parse(chk.Uid);
                tempDataInt = activeVerse;
                tempDataInt--;

                PageItemLoadTask = new Task(loadVerse);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        // -------------------------- Verse Nav Controls -------------------------- //

        // Animation Fonks //

        private void pageLoadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                pageloadinGif.Visibility = Visibility.Visible;
                verseFrameGrid.Visibility = Visibility.Hidden;
            });
        }

        private void pageLoadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                pageloadinGif.Visibility = Visibility.Collapsed;
                verseFrameGrid.Visibility = Visibility.Visible;
            });
        }

        private void dloadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                navLoadGifTop.Visibility = Visibility.Visible;
                navLoadGif.Visibility = Visibility.Visible;
                navContenStack.Visibility = Visibility.Hidden;

                verseLoadGif.Visibility = Visibility.Visible;
            });
        }

        private void dloadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                navLoadGifTop.Visibility = Visibility.Hidden;
                navLoadGif.Visibility = Visibility.Hidden;
                navContenStack.Visibility = Visibility.Visible;

                verseLoadGif.Visibility = Visibility.Collapsed;
            });
        }

        private void dsingleLoadVerseAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                navLoadGif.Visibility = Visibility.Visible;
                navContenStack.Visibility = Visibility.Hidden;
            });
        }

        private void dsingleLoadVerseAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                navLoadGif.Visibility = Visibility.Hidden;
                navContenStack.Visibility = Visibility.Visible;
            });
        }

        // Animation Fonks //
    }
}