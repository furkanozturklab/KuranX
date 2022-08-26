using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public int activeSure, activeVerse, tempDataInt = 0, lastVerse = 0, getpopupRelativeId;
        public int[] feedPoint = new int[4];
        private bool tempCheck = false, tempCheck2, tempCheck3;

        public verseFrame()
        {
            InitializeComponent();
        }

        public verseFrame(int data) : this()
        {
            activeSure = data;
            activeVerse = 1;

            using (var entitydb = new AyetContext())
            {
                dSure = (List<Sure>)entitydb.Sure.Where(p => p.sureId == activeSure).ToList();
                dVerseNav = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Select(p => new Verse() { VerseId = p.VerseId, RelativeDesk = p.RelativeDesk, Status = p.Status, VerseCheck = p.VerseCheck }).Skip(lastVerse).Take(8).ToList();

                verseName = dSure[0].Name;
                activeVerse = (int)dVerseNav[0].VerseId;
                tempDataInt = activeVerse;
                tempDataInt--;
                CurrentVerse.Tag = dVerseNav[0].RelativeDesk.ToString();

                dVerse = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Where(p => p.VerseId == activeVerse).ToList();
                dInterpreter = (List<Interpreter>)entitydb.Interpreter.Where(p => p.verseId == tempDataInt).Where(p => p.sureId == activeSure).Where(p => p.interpreterWriter == InterpreterWrite).ToList();
            }
        }

        // ---------- LOADED ---------- //

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageItemLoadTask = new Task(loadVerse);
            PageItemLoadTask.Start();
        }

        public void specialLoad()
        {
            singleLoadVerse();
            loadVerse();
        }

        public void singleLoadVerse()
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

                Debug.WriteLine(lastVerse);

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

        public void popuploadVerse()
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

        public void loadInterpreter()
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

        public void loadVerse()
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
        }

        public void loadMeaning()
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

        // ---------- LOADED ---------- //

        // ---------- EVENTLAR ---------- //
        private void check_Click(object sender, RoutedEventArgs e)
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

        private void interpreterWriterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InterpreterWrite = cmbselectedvalue(interpreterWriterCombo.SelectedValue.ToString());

            if (tempCheck)
            {
                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(loadInterpreter);
                PageItemLoadTask.Start();
            }
            else tempCheck = true;
        }

        private string cmbselectedvalue(string data)
        {
            string p = data;
            string[] parse = p.Split('.');
            p = parse[3];
            parse = p.Split(':');
            parse[0] = parse[1].Trim(' ');

            return parse[0];
        }

        private void alertFunc(string header, string detail, int timespan)
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

        private void infoFunc(string header, string detail, int timespan)
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

        private void succsessFunc(string header, string detail, int timespan)
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

        private void FeedStarClear()
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

        private void feedStarOne_Click(object sender, RoutedEventArgs e)
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

        private void feedStarTwo_Click(object sender, RoutedEventArgs e)
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

        private void feedStarThree_Click(object sender, RoutedEventArgs e)
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

        private void feedStarFour_Click(object sender, RoutedEventArgs e)
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

        private void subjectColorPick_Click(object sender, RoutedEventArgs e)
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

        private void subjectFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            subjectpreviewName.Text = subjectFolderHeader.Text;
        }

        private void subjectFolderLoad()
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
            FullTextClear();
            versesTrDataExtends.Visibility = Visibility.Visible;
            versesTrDataExtendsText.Text = dVerse[0].VerseTr;
        }

        private void arabicFullTextLoad(object sender, EventArgs e)
        {
            FullTextClear();
            versesArDataExtends.Visibility = Visibility.Visible;
            versesArDataExtendsText.Text = dVerse[0].VerseArabic;
        }

        private void backFullTextLoad(object sender, EventArgs e)
        {
            FullTextClear();
            versesFullTextData.Visibility = Visibility.Visible;
            versesArDataExtendsText.Text = dInterpreter[0].interpreterDetail;
            backInterpreter.Visibility = Visibility.Collapsed;
        }

        // -------------------------- ALL TEXT SHOW HİDE -------------------------- //

        // -------------------------- Verse Nav Controls -------------------------- //

        // -------------------------- Verse Nav Controls -------------------------- //

        private void bottomNavUpdatePrev(object sender, EventArgs e)
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

        private void bottomNavUpdateNext(object sender, EventArgs e)
        {
            NavUpdateNext.IsEnabled = false;
            lastVerse += 7;
            PageItemLoadTask.Dispose();
            PageItemLoadTask = new Task(singleLoadVerse);
            PageItemLoadTask.Start();
        }

        private void activeVerseSelected(object sender, EventArgs e)
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

        // -------------------------- Verse Nav Controls -------------------------- //

        // --------------------------  POPUP -------------------------- //
        public void PopupShow(string data, string color)
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

        private void resetBtnSure_Click(object sender, RoutedEventArgs e)
        {
            resetSurepopup.IsOpen = true;
        }

        private void openMeaningAddPopup_Click(object sender, RoutedEventArgs e)
        {
            meaningAddPopup.IsOpen = true;
            var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
            attachSureVerseCountText.Text = "Secilebilecek Ayet Sayısı " + item.Tag.ToString();
            meaningAttachVerse.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk;
            meaningConnectVerse.Text = "1";
        }

        private void openMeaningPopup_Click(object sender, RoutedEventArgs e)
        {
            meainingConnect.IsOpen = true;
            PageItemLoadTask = new Task(loadMeaning);
            PageItemLoadTask.Start();
        }

        private void openNextVersePopup_Click(object sender, RoutedEventArgs e)
        {
            loadChangeVerseFramePopup.IsOpen = true;
            var item = popupNextSureId.SelectedItem as ComboBoxItem;
            popupcomboboxLabel.Text = "Secilebilecek Ayet Sayısı " + item.Tag.ToString();
        }

        private void showDescPopup_Click(object sender, RoutedEventArgs e)
        {
            Button contentb = sender as Button;
            textDesc.Text = contentb.Content.ToString();
            popupHeaderTextDesc.Text = verseName + " Suresinin Açıklaması";
            descVersePopup.IsOpen = true;
        }

        private void descButton_Click(object sender, RoutedEventArgs e)
        {
            Button contentb = sender as Button;
            textDesc.Text = contentb.Tag.ToString();
            popupHeaderTextDesc.Text = verseName + " Suresinin " + dVerse[0].RelativeDesk + " Ayetinin Açıklaması";
            descVersePopup.IsOpen = true;
        }

        private void openVerseNumberPopup_Click(object sender, EventArgs e)
        {
            comitVersePopup.IsOpen = true;
        }

        private void feedbackButton_Click(object sender, RoutedEventArgs e)
        {
            feedbackOpenPopup.IsOpen = true;
            feedbackConnectVerse.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk.ToString();
            feedbackDetail.Focus();
        }

        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            noteAddPopup.IsOpen = true;
            noteConnectVerse.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk;
        }

        private void meaningDetailPopup_Click(object sender, RoutedEventArgs e)
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

        private void openAddSubjectPopup_Click(object sender, RoutedEventArgs e)
        {
            addSubjectPopup.IsOpen = true;
            selectedSubject.Text = dSure[0].Name + " > " + dVerse[0].RelativeDesk.ToString();
            subjectFolderLoad();
        }

        private void newSubjectFolder_Click(object sender, RoutedEventArgs e)
        {
            addFolderSubjectPopup.IsOpen = true;
        }

        private void wordText_Click(object sender, RoutedEventArgs e)
        {
            wordDetailPopup.IsOpen = true;

            using (var entitydb = new AyetContext())
            {
                var dWords = entitydb.Words.Where(p => p.SureId == dSure[0].sureId).Where(p => p.VerseId == dVerse[0].RelativeDesk).ToList();

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

                    wordsAllShowPopupStackPanel.Children.Add(itemsStack);
                }
            }
        }

        // --------------------------  POPUP -------------------------- //

        // -------------------------- POPUP ACTİON -------------------------- //

        private void resetSureBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                foreach (var item in entitydb.Verse)
                {
                    item.VerseCheck = "false";
                    item.Status = "#FFFFFF";
                    entitydb.Verse.Attach(item);
                }
                entitydb.SaveChanges();
            }

            PageItemLoadTask.Dispose();
            PageItemLoadTask = new Task(specialLoad);
            PageItemLoadTask.Start();

            resetSurepopup.IsOpen = false;
        }

        private void loadVersePopup_Click(object sender, RoutedEventArgs e)
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

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tempCheck2)
            {
                var item = popupNextSureId.SelectedItem as ComboBoxItem;

                popupcomboboxLabel.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
            }
            else tempCheck2 = true;
        }

        private void meaningOpenDetailTextBack_Click(object sender, RoutedEventArgs e)
        {
            meainingConnectDetailText.IsOpen = false;
        }

        private void allShowMeaningButton_Click(object sender, RoutedEventArgs e)
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

        private void addfolderSubject_Click(object sender, RoutedEventArgs e)
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

        private void popupMeiningSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tempCheck3)
            {
                var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                attachSureVerseCountText.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
            }
            else tempCheck3 = true;
        }

        private void addSubject_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                var item = selectedSubjectFolder.SelectedItem as ComboBoxItem;

                Debug.WriteLine(item);

                if (item != null)
                {
                    var dControl = entitydb.SubjectItems.Where(p => p.SureId == dSure[0].sureId).Where(p => p.VerseId == dVerse[0].RelativeDesk).Where(p => p.SubjectId == Int16.Parse(item.Uid)).ToList();

                    Debug.WriteLine("Kontorl:" + dControl.Count.ToString());

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

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
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

        private void nextSure_Click(object sender, RoutedEventArgs e)
        {
            var item = popupNextSureId.SelectedItem as ComboBoxItem;

            if (Int16.Parse(item.Tag.ToString()) >= Int16.Parse(popupNextVerseId.Text))
            {
                App.mainframe.Content = new stickVerseFrame(Int16.Parse(popupNextVerseId.Text), Int16.Parse(item.Uid.ToString()));
                popupNextVerseId.Text = "1";
            }
            else
            {
                popupcomboboxLabel.Text = "Ayet Numarası " + item.Tag.ToString() + " Üzerinde Olamaz";
                popupNextVerseId.Focus();
            }

            //  App.mainframe.Content = new stickVerseFrame(1, 1);
        }

        private void newmeaningVerse_Click(object sender, RoutedEventArgs e)
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

        // -------------------------- POPUP ACTİON -------------------------- //

        // Simple Clear Fonks //

        private void noteName_KeyDown(object sender, KeyEventArgs e)
        {
            noteAddPopupHeaderError.Visibility = Visibility.Hidden;
        }

        private void noteDetail_KeyDown(object sender, KeyEventArgs e)
        {
            noteAddPopupDetailError.Visibility = Visibility.Hidden;
        }

        private void meaningName_KeyDown(object sender, KeyEventArgs e)
        {
            meaningNameAddPopupHeaderError.Visibility = Visibility.Hidden;
        }

        private void meaningDetail_KeyDown(object sender, KeyEventArgs e)
        {
            meaningDetailAddPopupHeaderError.Visibility = Visibility.Hidden;
        }

        private void subjectFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            subjectHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
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
    }
}