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
    /// Interaction logic for VerseFrame.xaml
    /// </summary>
    ///

    public partial class verseFrame : Page
    {
        private List<Sure> dSure = new List<Sure>();
        private List<Verse> dVerse, dVerseNav, tempVerses = new List<Verse>();
        private List<Integrity> tempInteg = new List<Integrity>();
        private List<Interpreter> dInterpreter = new List<Interpreter>();
        private List<ItemsControl> controlitemsList = new List<ItemsControl>();
        private Task PageItemLoadTask;
        private CheckBox chk;
        private ComboBoxItem cmbtmp = new ComboBoxItem();
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private string InterpreterWrite, verseName;
        public int activeSure, activeVerse, tempDataInt = 0, lastVerse = 0, getpopupRelativeId;
        public int[] feedPoint = new int[4];
        private bool tempCheck = false, tempCheck2, tempCheck3;

        public verseFrame()
        {
            InitializeComponent();
            controlitemsList.Add(vb1);
            controlitemsList.Add(vb2);
            controlitemsList.Add(vb3);
            controlitemsList.Add(vb4);
            controlitemsList.Add(vb5);
            controlitemsList.Add(vb6);
            controlitemsList.Add(vb7);
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
                dVerseNav = (List<Verse>)entitydb.Verse.Where(p => p.SureId == activeSure).Select(p => new Verse() { VerseId = p.VerseId, RelativeDesk = p.RelativeDesk, Status = p.Status, VerseCheck = p.VerseCheck }).Skip(lastVerse).Take(8).ToList();
                int i = 0;

                for (int x = 0; x < 7; x++)
                {
                    controlitemsList[x].Dispatcher.Invoke(() =>
                    {
                        controlitemsList[x].ItemsSource = null;
                        controlitemsList[x].Items.Clear();
                    });
                }

                foreach (var item in dVerseNav)
                {
                    tempVerses.Add(item);

                    controlitemsList[i].Dispatcher.Invoke(() =>
                    {
                        controlitemsList[i].ItemsSource = tempVerses;
                    });

                    tempVerses.Clear();
                    i++;
                    if (i == 7) break; // 7 den fazla varmı kontrol etmek için koydum
                }

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
                if (dVerse[0].RelativeDesk > 7) singleLoadVerse();
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

                int i = 0;

                for (int x = 0; x < 7; x++)
                {
                    controlitemsList[x].Dispatcher.Invoke(() =>
                    {
                        controlitemsList[x].ItemsSource = null;
                        controlitemsList[x].Items.Clear();
                    });
                }

                foreach (var item in dVerseNav)
                {
                    tempVerses.Add(item);

                    controlitemsList[i].Dispatcher.Invoke(() =>
                    {
                        controlitemsList[i].ItemsSource = tempVerses;
                    });

                    tempVerses.Clear();
                    i++;
                    if (i == 7) break; // 7 den fazla varmı kontrol etmek için koydum
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
                lastVerse -= 7;
                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(singleLoadVerse);
                PageItemLoadTask.Start();
            }
        }

        private void bottomNavUpdateNext(object sender, EventArgs e)
        {
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

            PageItemLoadTask.Dispose();
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

                meaningOpenDetailTextConnect.Text = dSure.Name + " Suresini " + dInteg.connectVerseId.ToString() + " Ayeti İçin " + dCSure.Name + dInteg.connectedVerseId + " Ayeti ile Eşleştirilmiş";
            }

            meainingConnectDetailText.IsOpen = true;
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

        private void popupMeiningSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tempCheck3)
            {
                var item = meaningpopupNextSureId.SelectedItem as ComboBoxItem;
                attachSureVerseCountText.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
            }
            else tempCheck3 = true;
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

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            Button btntemp = sender as Button;
            var popuptemp = (Popup)this.FindName(btntemp.Uid);

            FeedStarClear();

            meaningName.Text = "";
            meaningConnectNote.Text = "";
            noteName.Text = "";
            noteDetail.Text = "";
            popuptemp.IsOpen = false;
        }
    }
}