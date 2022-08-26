﻿using System;
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
        private List<ItemsControl> controlitemsList = new List<ItemsControl>();
        private Task PageItemLoadTask;
        private CheckBox chk;
        private ComboBoxItem cmbtmp = new ComboBoxItem();
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private string InterpreterWrite, verseName;
        public int activeSure, activeVerse, tempDataInt = 0, lastVerse, getpopupRelativeId;
        private bool tempCheck = false;

        public stickVerseFrame()
        {
            InitializeComponent();
        }

        public stickVerseFrame(int verseId, int sureId) : this()
        {
            activeSure = sureId;
            activeVerse = verseId;

            if (verseId > 7)
            {
                lastVerse = verseId;
                lastVerse--;
            }
            else lastVerse = 0;
            controlitemsList.Add(vb1);
            controlitemsList.Add(vb2);
            controlitemsList.Add(vb3);
            controlitemsList.Add(vb4);
            controlitemsList.Add(vb5);
            controlitemsList.Add(vb6);
            controlitemsList.Add(vb7);

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageItemLoadTask = new Task(loadVerse);
            PageItemLoadTask.Start();
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
                singleLoadVerse();
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
            FullTextClear();
            versesTrDataExtends.Visibility = Visibility.Visible;
            versesTrDataExtendsText.Text = dVerse[0].VerseTr;
        }

        private void openVerseNumberPopup_Click(object sender, EventArgs e)
        {
            comitVersePopup.IsOpen = true;
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            Button btntemp = sender as Button;
            var popuptemp = (Popup)this.FindName(btntemp.Uid);

            popuptemp.IsOpen = false;
        }

        private void interpreterWriterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InterpreterWrite = cmbselectedvalue(interpreterWriterCombo.SelectedValue.ToString());

            if (tempCheck)
            {
                MessageBox.Show("Active");
                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(loadInterpreter);
                PageItemLoadTask.Start();
            }
            else tempCheck = true;
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

        private string cmbselectedvalue(string data)
        {
            string p = data;
            string[] parse = p.Split('.');
            p = parse[3];
            parse = p.Split(':');
            parse[0] = parse[1].Trim(' ');

            return parse[0];
        }

        private void openNextVersePopup_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void loadVersePopup_Click(object sender, RoutedEventArgs e)
        {
            comitVersePopup.IsOpen = false;
            if (dSure[0].NumberOfVerses >= Int16.Parse(popupRelativeId.Text.ToString()))
            {
                getpopupRelativeId = Int16.Parse(popupRelativeId.Text.ToString());
                PageItemLoadTask.Dispose();
                PageItemLoadTask = new Task(popuploadVerse);
                PageItemLoadTask.Start();
            }
            else
            {
                MessageBox.Show("Ayet Sayısını Geçtiniz Gidile Bilecek Son Ayet Sayısı : " + dSure[0].NumberOfVerses.ToString() + "Lütfen Tekrar Deneyiniz.");
            }
        }

        private void nextSure_Click(object sender, RoutedEventArgs e)
        {
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
    }
}