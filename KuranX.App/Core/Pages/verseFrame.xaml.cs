using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;

namespace KuranX.App.Core.Pages
{
    /// <summary>
    /// Interaction logic for VerseFrame.xaml
    /// </summary>
    ///

    public partial class verseFrame : Page
    {
        private List<Sure> dSure = new List<Sure>();
        private List<Verse> dVerse, dVerseNav, tempVerses = new List<Verse>();
        private List<Integrity> tempInteg = new List<Integrity>();
        private List<Interpreter> dInterpreter = new List<Interpreter>();
        private Task PageItemLoadTask;
        private CheckBox chk;
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private string InterpreterWrite, verseName;
        public int activeSure, activeVerse, tempDataInt = 0, lastVerse = 0, getpopupRelativeId, relativeDeskV;
        public int[] feedPoint = new int[4];
        private bool tempCheck = false, tempCheck2, tempCheck3;

        public verseFrame()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        public verseFrame(int data, int relativeDeskTemp) : this()
        {
            try
            {
                relativeDeskV = relativeDeskTemp;

                activeSure = data;
                activeVerse = 1;

                if (relativeDeskTemp <= 7) lastVerse = 0;
                else lastVerse = --relativeDeskV;

                using (var entitydb = new AyetContext())
                {
                    dSure = (List<Sure>)entitydb.Sure.Where(p => p.sureId == activeSure).ToList();
                    dVerseNav = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Select(p => new Verse() { VerseId = p.VerseId, RelativeDesk = p.RelativeDesk, Status = p.Status, VerseCheck = p.VerseCheck }).Skip(lastVerse).Take(8).ToList();

                    verseName = dSure[0].Name;
                    if (relativeDeskTemp <= 7) activeVerse = (int)dVerseNav[--relativeDeskV].VerseId;
                    else activeVerse = (int)dVerseNav[0].VerseId;
                    tempDataInt = activeVerse;
                    tempDataInt--;
                    CurrentVerse.Tag = dVerseNav[0].RelativeDesk.ToString();

                    dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.VerseId == activeVerse).ToList();
                    dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        // ---------- LOADED ---------- //

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

        public void specialLoad()
        {
            singleLoadVerse();
            loadVerse();
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

                homeScreen.locationTxt.Dispatcher.Invoke(() =>
                {
                    homeScreen.locationTxt.Text = "Ayetler >" + " " + dSure[0].Name;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        public void loadMeaning()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dInteg = (List<Integrity>)entitydb.Integrity.Where(p => p.connectSureId == dSure[0].sureId).ToList();
                    int i = 1;
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
                            ItemsControl dControlTemp = (ItemsControl)this.FindName("mvc" + i);
                            tempInteg.Add(item);
                            dControlTemp.ItemsSource = tempInteg;
                            tempInteg.Clear();

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

        // ---------- LOADED ---------- //

        // ---------- EVENTLAR ---------- //
        private void check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox bchk = sender as CheckBox;
                PageItemLoadTask.Dispose();

                using (var entity = new AyetContext())
                {
                    var verseUpdate = entity.Verse.Where(p => p.VerseId == Int16.Parse(bchk.Uid.ToString())).FirstOrDefault();
                    var sureUpdate = entity.Sure.Where(p => p.sureId == dSure[0].sureId).FirstOrDefault();

                    if (bchk.IsChecked.ToString() == "True")
                    {
                        verseUpdate.VerseCheck = "true";
                        verseUpdate.Status = "#66E21F";
                        PopupShow("Okudum Olarak Güncellendi", "#66E21F");
                        sureUpdate.UserCheckCount++;

                        if (sureUpdate.UserCheckCount == sureUpdate.NumberOfVerses) sureUpdate.Status = "#66E21F";
                    }
                    else
                    {
                        verseUpdate.VerseCheck = "false";
                        verseUpdate.Status = "#FFFFFF";
                        PopupShow("Okumadım Olarak Güncellendi", "#F0433A");

                        if (sureUpdate.Status == "#66E21F") sureUpdate.Status = "#ADB5BD";

                        if (sureUpdate.UserCheckCount != 0) sureUpdate.UserCheckCount--;
                    }

                    entity.SaveChanges();

                    PageItemLoadTask = new Task(singleLoadVerse);
                    PageItemLoadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("UpdateEvent", ex);
            }
        }

        private void markButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox bchk = sender as CheckBox;
                using (var entity = new AyetContext())
                {
                    var selectedS = entity.Sure.Where(p => p.Status == "#0D6EFD").ToList();
                    foreach (var item in selectedS) item.Status = "#ADB5BD";

                    if (bchk.IsChecked.ToString() == "True")
                    {
                        entity.Sure.Where(p => p.sureId == dSure[0].sureId).FirstOrDefault().Status = "#0D6EFD";
                        entity.Sure.Where(p => p.sureId == dSure[0].sureId).FirstOrDefault().UserLastRelativeVerse = dVerse[0].RelativeDesk;
                        entity.Verse.Where(p => p.VerseId == dVerse[0].VerseId).FirstOrDefault().MarkCheck = "true";

                        PopupShow("Kaldığınız Yer İşaretlendi", "#0D6EFD");
                    }
                    else
                    {
                        entity.Sure.Where(p => p.sureId == dSure[0].sureId).FirstOrDefault().Status = "#ADB5BD";
                        entity.Verse.Where(p => p.VerseId == dVerse[0].VerseId).FirstOrDefault().MarkCheck = "false";
                        entity.Sure.Where(p => p.sureId == dSure[0].sureId).FirstOrDefault().UserLastRelativeVerse = 1;
                        PopupShow("Kaldığınız Yer İşareti Kaldırıldı", "#0D6EFD");
                    }

                    entity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("UpdateEvent", ex);
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

        private void FeedStarClear()
        {
            try
            {
                feedbackDetail.Text = "";
                foreach (object item in oneFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                foreach (object item in twoFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                foreach (object item in threeFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                foreach (object item in fourFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
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
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                CheckBox dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in oneFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (Int16.Parse(dchk.Uid) == i) break;
                    i++;
                }
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
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }
                CheckBox dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in twoFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (Int16.Parse(dchk.Uid) == i) break;
                    i++;
                }
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
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }
                CheckBox dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in threeFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (Int16.Parse(dchk.Uid) == i) break;
                    i++;
                }
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
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }
                CheckBox dchk = sender as CheckBox;
                int i = 1;
                foreach (object item in fourFeed.Children)
                {
                    CheckBox chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = true;
                    }

                    if (Int16.Parse(dchk.Uid) == i) break;
                    i++;
                }
                feedPoint[3] = i;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void subjectColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chk;

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

        private void subjectFolderLoad()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dSubjectFolder = entitydb.Subject.ToList();

                    selectedSubjectFolder.Items.Clear();
                    foreach (var item in dSubjectFolder)
                    {
                        var cmbitem = new ComboBoxItem();

                        cmbitem.Content = item.SubjectName;
                        cmbitem.Uid = item.SubjectId.ToString();
                        selectedSubjectFolder.Items.Add(cmbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        // ---------- EVENTLAR ---------- //

        // -------------------------- ALL TEXT SHOW HİDE -------------------------- //

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

        // -------------------------- ALL TEXT SHOW HİDE -------------------------- //

        // -------------------------- Verse Nav Controls -------------------------- //

        // -------------------------- Verse Nav Controls -------------------------- //

        private void bottomNavUpdatePrev(object sender, EventArgs e)
        {
            try
            {
                if (lastVerse >= 7)
                {
                    NavUpdatePrev.IsEnabled = false;
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
                NavUpdateNext.IsEnabled = false;
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

                PageItemLoadTask = new Task(loadVerse);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        // -------------------------- Verse Nav Controls -------------------------- //

        // --------------------------  POPUP -------------------------- //
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
                App.logWriter("Other", ex);
            }
        }

        private void resetBtnSure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resetSurepopup.IsOpen = true;
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
                meaningAddPopup.IsOpen = true;
                var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                attachSureVerseCountText.Text = "Secilebilecek Ayet Sayısı " + item.Tag.ToString();
                meaningAttachVerse.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk;
                meaningConnectVerse.Text = "1";
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
                meainingConnect.IsOpen = true;
                PageItemLoadTask = new Task(loadMeaning);
                PageItemLoadTask.Start();
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
                loadChangeVerseFramePopup.IsOpen = true;
                var item = popupNextSureId.SelectedItem as ComboBoxItem;
                popupcomboboxLabel.Text = "Secilebilecek Ayet Sayısı " + item.Tag.ToString();
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
                textDesc.Text = contentb.Uid.ToString();
                popupHeaderTextDesc.Text = verseName + " Suresinin " + dVerse[0].RelativeDesk + " Ayetinin Açıklaması";
                descVersePopup.IsOpen = true;
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

        private void feedbackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                feedbackOpenPopup.IsOpen = true;
                feedbackConnectVerse.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk.ToString();
                feedbackDetail.Focus();
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
                noteAddPopup.IsOpen = true;
                noteConnectVerse.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void meaningDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? tmpbutton = sender as Button;

                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Integrity.Where(p => p.IntegrityId == Int16.Parse(tmpbutton.Uid)).FirstOrDefault();

                    var dSure = entitydb.Sure.Where(p => p.sureId == dInteg.connectSureId).Select(p => new Sure() { Name = p.Name }).FirstOrDefault();
                    var dCSure = entitydb.Sure.Where(p => p.sureId == dInteg.connectedSureId).Select(p => new Sure() { Name = p.Name }).FirstOrDefault();
                    meaningOpenDetailText.Text = dInteg.IntegrityNote;
                    meaningDetailTextHeader.Text = dInteg.IntegrityName;
                    meaningOpenDetailTextConnect.Text = dSure.Name + " Suresini " + dInteg.connectVerseId.ToString() + " Ayeti İçin " + dCSure.Name + " " + dInteg.connectedVerseId + " Ayeti ile Eşleştirilmiş";
                }

                meainingConnectDetailText.IsOpen = true;
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
                addSubjectPopup.IsOpen = true;
                selectedSubject.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk.ToString();
                subjectFolderLoad();
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
                addFolderSubjectPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
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

        // --------------------------  POPUP -------------------------- //

        // -------------------------- POPUP ACTİON -------------------------- //

        private void resetSureBtn_Click(object sender, RoutedEventArgs e)
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

                    var sureUpdate = entitydb.Sure.Where(p => p.sureId == dSure[0].sureId).FirstOrDefault();
                    sureUpdate.Status = "#ADB5BD";
                    sureUpdate.UserCheckCount = 0;

                    entitydb.SaveChanges();
                }

                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(specialLoad);
                PageItemLoadTask.Start();

                resetSurepopup.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
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

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tempCheck2)
                {
                    var item = popupNextSureId.SelectedItem as ComboBoxItem;

                    popupcomboboxLabel.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
                }
                else tempCheck2 = true;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void meaningOpenDetailTextBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                meainingConnectDetailText.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void allShowMeaningButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                meainingAllShowPopup.IsOpen = true;

                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Integrity.Where(p => p.connectSureId == dSure[0].sureId).ToList();

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

                        headerText.Text = item.IntegrityName.ToString();
                        noteText.Text = item.IntegrityNote.ToString();
                        allshowButton.Uid = item.IntegrityId.ToString();

                        allshowButton.Click += meaningDetailPopup_Click;

                        itemsStack.Children.Add(headerText);
                        itemsStack.Children.Add(noteText);
                        itemsStack.Children.Add(allshowButton);
                        itemsStack.Children.Add(sp);

                        meainingAllShowPopupStackPanel.Children.Add(itemsStack);
                    }
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
                            addFolderSubjectPopup.IsOpen = false;
                            subjectFolderLoad();
                        }
                        else
                        {
                            alertFunc("Konu Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                        }
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

        private void popupMeiningSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tempCheck3)
                {
                    var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                    attachSureVerseCountText.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
                }
                else tempCheck3 = true;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
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
                        var dControl = entitydb.SubjectItems.Where(p => p.SureId == dSure[0].sureId).Where(p => p.VerseId == dVerse[0].RelativeDesk).Where(p => p.SubjectId == Int16.Parse(item.Uid)).ToList();

                        if (dControl.Count != 0)
                        {
                            alertFunc("Ayet Ekleme Başarısız", "Bu ayet Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            var dSubjectItem = new SubjectItems { SubjectId = Int16.Parse(item.Uid), SubjectNotesId = 0, SureId = dSure[0].sureId, VerseId = dVerse[0].RelativeDesk, Created = DateTime.Now, Modify = DateTime.Now };
                            entitydb.SubjectItems.Add(dSubjectItem);
                            entitydb.SaveChanges();
                            succsessFunc("Ayet Ekleme Başarılı", "Seçmiş olduğunuz konuya ayet eklendi.", 3);
                        }
                    }
                    else
                    {
                        popupaddsubjectError.Visibility = Visibility.Visible;
                        popupaddsubjectError.Text = "Lütfen Konuyu Seçiniz";
                        selectedSubjectFolder.Focus();
                    }
                }
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
                    noteAddPopupHeaderError.Content = "Not Başlığı Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                }
                else
                {
                    if (noteDetail.Text.Length <= 8)
                    {
                        noteAddPopupDetailError.Visibility = Visibility.Visible;
                        noteDetail.Focus();
                        noteAddPopupDetailError.Content = "Not Başlığı Yeterince Uzun Değil. Min 8 Karakter Olmalıdır";
                    }
                    else
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dNotes = new Notes { NoteHeader = noteName.Text, NoteDetail = noteDetail.Text, SureId = activeSure, VerseId = activeVerse, Modify = DateTime.Now, Created = DateTime.Now, NoteLocation = "Ayet" };
                            entitydb.Notes.Add(dNotes);
                            entitydb.SaveChanges();
                            succsessFunc("Not Ekleme Başarılı", dSure[0].Name + " Surenin " + dVerse[0].RelativeDesk + " Ayetine Not Eklendiniz.", 3);
                            noteName.Text = "";
                            noteDetail.Text = "";
                        }
                        noteAddPopup.IsOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void nextSure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = popupNextSureId.SelectedItem as ComboBoxItem;

                if (Int16.Parse(item.Tag.ToString()) >= Int16.Parse(popupNextVerseId.Text))
                {
                    App.mainframe.Content = new stickVerseFrame(Int16.Parse(popupNextVerseId.Text), Int16.Parse(item.Uid.ToString()), dSure[0].Name, dVerse[0].RelativeDesk.Value);
                    popupNextVerseId.Text = "1";
                }
                else
                {
                    popupcomboboxLabel.Text = "Ayet Numarası " + item.Tag.ToString() + " Üzerinde Olamaz";
                    popupNextVerseId.Focus();
                }

                //  App.mainframe.Content = new stickVerseFrame(1, 1);
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
                    if (meaningConnectNote.Text.Length >= 8)
                    {
                        ComboBoxItem tmpcbxi = (ComboBoxItem)meaningpopupNextSureId.SelectedItem;
                        if (Int16.Parse(tmpcbxi.Tag.ToString()) >= Int16.Parse(meaningConnectVerse.Text))
                        {
                            using (var entitydb = new AyetContext())
                            {
                                var dIntegrity = new Integrity { IntegrityName = meaningName.Text, connectSureId = dSure[0].sureId, connectVerseId = dVerse[0].RelativeDesk, connectedSureId = Int16.Parse(tmpcbxi.Uid), connectedVerseId = Int16.Parse(meaningConnectVerse.Text), Created = DateTime.Now, Modify = DateTime.Now, IntegrityNote = meaningConnectNote.Text };
                                entitydb.Integrity.Add(dIntegrity);
                                entitydb.SaveChanges();
                                succsessFunc("Bağlantı Oluşturuldu.", "Yeni bağlantınız oluşturuldu.", 3);
                                meaningName.Text = "";
                                meaningConnectNote.Text = "";
                                meaningAddPopup.IsOpen = false;

                                PageItemLoadTask = new Task(loadMeaning);
                                PageItemLoadTask.Start();
                            }
                        }
                        else
                        {
                            meaningCountAddPopupHeaderError.Visibility = Visibility.Visible;
                            meaningConnectVerse.Focus();
                            meaningCountAddPopupHeaderError.Content = "Ayet Sınırı Geçtiniz.";
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

        // -------------------------- POPUP ACTİON -------------------------- //

        // Simple Clear Fonks //

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

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);

                FeedStarClear();

                meaningName.Text = "";
                meaningConnectNote.Text = "";
                noteName.Text = "";
                noteDetail.Text = "";
                subjectFolderHeader.Text = "";
                subjectpreviewName.Text = "";
                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }
    }
}