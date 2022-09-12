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
using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;

namespace KuranX.App.Core.Pages
{
    /// <summary>
    /// Interaction logic for stickVerseFrame.xaml
    /// </summary>
    public partial class stickVerseFrame : Page
    {
        private List<Sure> dSure = new List<Sure>();
        private List<Verse> dVerse, dVerseNav, tempVerses = new List<Verse>();
        private List<Interpreter> dInterpreter = new List<Interpreter>();
        private Task PageItemLoadTask;
        private CheckBox chk;
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private string InterpreterWrite, verseName;
        public int activeSure, activeVerse, tempDataInt = 0, lastVerse, getpopupRelativeId;
        private bool tempCheck = false;

        public stickVerseFrame()
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

        public stickVerseFrame(int verseId, int sureId, string CurrentSure, string CurrentVerseId, string navInfoStatus) : this()
        {
            try
            {
                activeSure = sureId;
                activeVerse = verseId;

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

                if (verseId > 7)
                {
                    lastVerse = verseId;
                    lastVerse--;
                }
                else lastVerse = 0;

                using (var entitydb = new AyetContext())
                {
                    dSure = (List<Sure>)entitydb.Sure.Where(p => p.sureId == activeSure).ToList();
                    dVerseNav = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Select(p => new Verse() { VerseId = p.VerseId, RelativeDesk = p.RelativeDesk, Status = p.Status, VerseCheck = p.VerseCheck }).Skip(lastVerse).Take(8).ToList();

                    if (activeVerse >= 8) activeVerse = (int)dVerseNav[0].VerseId;
                    else activeVerse = (int)dVerseNav[tempDataInt].VerseId;

                    verseName = dSure[0].Name;
                    tempDataInt = activeVerse;
                    tempDataInt--;

                    CurrentVerse.Tag = dVerseNav[0].RelativeDesk.ToString();

                    dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.VerseId == activeVerse).ToList();
                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == activeVerse).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();

                    Debug.WriteLine(dInterpreter[0].interpreterWriter);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

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

        public void singleLoadVerse()
        {
            try
            {
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

        public void popuploadVerse()
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

                    controlNavPanelItemscontrol.Dispatcher.Invoke(() =>
                    {
                        controlNavPanelItemscontrol.ItemsSource = dVerse;
                    });

                    versesTextData.Dispatcher.Invoke(() =>
                    {
                        versesTextData.ItemsSource = dVerse;
                    });

                    versesFullTextData.Dispatcher.Invoke(() =>
                    {
                        versesFullTextData.ItemsSource = dInterpreter;
                    });

                    Thread.Sleep(200);

                    CurrentVerse.Dispatcher.Invoke(() =>
                    {
                        CurrentVerse.Tag = dVerse[0].RelativeDesk.ToString();
                    });

                    lastVerse = getpopupRelativeId;
                    lastVerse--;
                    if (dVerse[0].RelativeDesk >= 8)
                    {
                        singleLoadVerse();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
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
                        versesFullTextData.ItemsSource = dInterpreter;
                    });
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        public void loadVerse()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();
                    dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.VerseId == activeVerse).ToList();

                    versesFullTextData.Dispatcher.Invoke(() =>
                    {
                        versesFullTextData.ItemsSource = dInterpreter;
                    });

                    versesTextData.Dispatcher.Invoke(() =>
                    {
                        versesTextData.ItemsSource = dVerse;
                    });

                    verseHeader.Dispatcher.Invoke(() =>
                    {
                        verseHeader.ItemsSource = dSure;
                    });

                    controlNavPanelItemscontrol.Dispatcher.Invoke(() =>
                    {
                        controlNavPanelItemscontrol.ItemsSource = dVerse;
                    });

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
                }
                Thread.Sleep(200);

                verseDetailLoadingGif.Dispatcher.Invoke(() =>
                {
                    verseDetailLoadingGif.Visibility = Visibility.Collapsed;
                });

                pageloadinGifContent.Dispatcher.Invoke(() =>
                {
                    pageloadinGifContent.Visibility = Visibility.Collapsed;
                });

                NavUpdatePrev.Dispatcher.Invoke(() =>
                {
                    if (lastVerse == 0) NavUpdatePrev.IsEnabled = false;
                    else NavUpdatePrev.IsEnabled = true;
                });

                NavUpdateNext.Dispatcher.Invoke(() =>
                {
                    if (dVerseNav.Count() <= 7) NavUpdateNext.IsEnabled = false;
                    if (lastVerse == 0 && dVerseNav.Count() > 7) NavUpdateNext.IsEnabled = true;
                });

                CurrentVerse.Dispatcher.Invoke(() =>
                {
                    CurrentVerse.Tag = dVerse[0].RelativeDesk.ToString();
                });

                App.locationTxt.Dispatcher.Invoke(() =>
                {
                    App.locationTxt.Text = "Ayetler >" + " " + dSure[0].Name;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        public void PopupShow(string data, string color)
        {
            try
            {
                versePagePopup.IsOpen = true;
                versePagePopupText.Text = data;
                versePagePopupBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(color);

                timeSpan.Interval = TimeSpan.FromSeconds(3);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    versePagePopup.IsOpen = false;
                    timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("PopupEvent", ex);
            }
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox bchk = sender as CheckBox;
                PageItemLoadTask.Dispose();

                using (var entity = new AyetContext())
                {
                    var verseUpdate = entity.Verse.Where(p => p.VerseId == Int16.Parse(bchk.Uid.ToString())).FirstOrDefault();

                    if (bchk.IsChecked.ToString() == "True")
                    {
                        verseUpdate.VerseCheck = "true";
                        verseUpdate.Status = "#66E21F";
                        PopupShow("Okudum Olarak Güncellendi", "#66E21F");
                    }
                    else
                    {
                        verseUpdate.VerseCheck = "false";
                        verseUpdate.Status = "#FFFFFF";
                        PopupShow("Okumadım Olarak Güncellendi", "#F0433A");
                    }

                    entity.SaveChanges();

                    PageItemLoadTask = new Task(singleLoadVerse);
                    PageItemLoadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("SelectEvent", ex);
            }
        }

        private void FullTextClear()
        {
            versesFullTextData.Visibility = Visibility.Collapsed;
            versesTrDataExtends.Visibility = Visibility.Collapsed;
            versesArDataExtends.Visibility = Visibility.Collapsed;
            backInterpreter.Visibility = Visibility.Visible;
        }

        // ---------- EVENTLAR ---------- //
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
                App.logWriter("Other", ex);
            }
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
                App.logWriter("PopupEvent", ex);
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
                App.logWriter("Other", ex);
            }
        }

        private void showDescPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button contentb = sender as Button;
                textDesc.Text = contentb.Content.ToString();
                popupHeaderTextDesc.Text = verseName + " Suresinin Açıklaması";
                descVersePopup.IsOpen = true;
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
                Button contentb = sender as Button;
                textDesc.Text = contentb.Tag.ToString();
                popupHeaderTextDesc.Text = verseName + " Suresinin " + dVerse[0].RelativeDesk + " Ayetinin Açıklaması";
                descVersePopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void openNextVersePopup_Click(object sender, RoutedEventArgs e)
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
                App.logWriter("Other", ex);
            }
        }

        private void activeVerseSelected(object sender, EventArgs e)
        {
            try
            {
                chk = sender as CheckBox;
                FullTextClear();
                versesFullTextData.Visibility = Visibility.Visible;
                backInterpreter.Visibility = Visibility.Collapsed;

                if (chk.IsChecked.ToString() == "True") chk.IsChecked = false;
                else { chk.IsChecked = true; }
                verseDetailLoadingGif.Visibility = Visibility.Visible;
                activeVerse = Int16.Parse(chk.Uid);
                tempDataInt = activeVerse;
                tempDataInt--;

                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(loadVerse);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("SelectEvent", ex);
            }
        }

        private void bottomNavUpdatePrev(object sender, EventArgs e)
        {
            try
            {
                if (lastVerse >= 7)
                {
                    lastVerse -= 7;
                    PageItemLoadTask.Dispose();
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
                lastVerse += 7;
                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(singleLoadVerse);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }
    }
}