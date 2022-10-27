using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for libraryFileItemsFrame.xaml
    /// </summary>
    public partial class libraryNotesItems : Page
    {
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private Task? NotesLoadTask;
        private DoubleAnimation animation = new DoubleAnimation();
        private List<Library> dLibrary = new List<Library>(), tempLibrary = new List<Library>();
        private bool searchStatus = false;
        private string searchTxt;
        private int NowPage = 1, lastLibraryItem = 0;

        public libraryNotesItems()
        {
            InitializeComponent();
            animation.Duration = new Duration(TimeSpan.FromSeconds(3));
        }

        public void loadPdffiles()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    dLibrary = entitydb.Librarys.Skip(lastLibraryItem).Take(21).ToList();
                    Decimal totalcount = entitydb.Librarys.Count();
                    this.Dispatcher.Invoke(() =>
                    {
                        for (int x = 1; x < 21; x++)
                        {
                            ItemsControl itemslist = (ItemsControl)this.FindName("lnt" + x);
                            itemslist.ItemsSource = null;
                        }
                        int i = 1;
                        foreach (var item in dLibrary)
                        {
                            tempLibrary.Add(item);
                            ItemsControl itemslist = (ItemsControl)this.FindName("lnt" + i);
                            itemslist.ItemsSource = tempLibrary;
                            tempLibrary.Clear();
                            i++;

                            if (i == 21) break; //22 kontrol break;
                        }
                    });
                    Thread.Sleep(200);
                    this.Dispatcher.Invoke(() =>
                    {
                        totalcountText.Tag = totalcount.ToString();
                        if (dLibrary.Count() != 0)
                        {
                            totalcount = Math.Ceiling(totalcount / 20);
                            nowPageStatus.Tag = NowPage + " / " + totalcount.ToString();
                            nextpageButton.Dispatcher.Invoke(() =>
                            {
                                if (NowPage != totalcount) nextpageButton.IsEnabled = true;
                                else if (NowPage == totalcount) nextpageButton.IsEnabled = false;
                            });
                            previusPageButton.Dispatcher.Invoke(() =>
                            {
                                if (NowPage != 1) previusPageButton.IsEnabled = true;
                                else if (NowPage == 1) previusPageButton.IsEnabled = false;
                            });
                        }
                        else
                        {
                            nowPageStatus.Tag = "-";
                            nextpageButton.IsEnabled = false;
                            previusPageButton.IsEnabled = false;
                        }
                    });
                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("loadpdf", ex);
            }
        }

        private void gotoNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                App.mainframe.Content = new libraryNote(int.Parse(btn.Uid));
                loadControlAni.Visibility = Visibility.Hidden;
                loadDetailAni.Visibility = Visibility.Hidden;
                loadHeaderAni.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("GotoFilePdf", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastLibraryItem += 20;
                NowPage++;
                NotesLoadTask = new Task(loadPdffiles);
                NotesLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void previusPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lastLibraryItem >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastLibraryItem -= 20;
                    NowPage--;
                    NotesLoadTask = new Task(loadPdffiles);
                    NotesLoadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        public void progressAni(ProgressBar prg, int start)
        {
            try
            {
                Storyboard.SetTarget(animation, prg);
                Storyboard.SetTargetProperty(animation, new PropertyPath(ProgressBar.ValueProperty));
                Storyboard sb = new Storyboard();
                sb.Children.Add(animation);
                animation.From = 0;
                animation.To = start;
                sb.Begin();
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void popupClear()
        {
            try
            {
                timeSpan.Interval = TimeSpan.FromSeconds(5);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("Time", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                NotesLoadTask = new Task(loadPdffiles);
                NotesLoadTask.Start();
            }
            catch (Exception ex)
            {
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
                App.logWriter("Other", ex);
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

        private void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    newLibHeader.IsEnabled = false;
                    backPage.IsEnabled = false;
                    loadinItemsGifContent.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    newLibHeader.IsEnabled = true;
                    backPage.IsEnabled = true;
                    loadinGifContent.Visibility = Visibility.Collapsed;
                    loadDetailAni.Visibility = Visibility.Visible;
                    loadControlAni.Visibility = Visibility.Visible;
                    loadHeaderAni.Visibility = Visibility.Visible;
                    loadinItemsGifContent.Visibility = Visibility.Collapsed;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void backPage_Click(object sender, RoutedEventArgs e)
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

        private void openLibraryFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                App.mainframe.Content = new LibraryNotesItemOpen(int.Parse(btn.Uid), btn.Content.ToString(), btn.Tag.ToString(), (SolidColorBrush)new BrushConverter().ConvertFrom(btn.Background.ToString()));
                loadDetailAni.Visibility = Visibility.Hidden;
                loadControlAni.Visibility = Visibility.Hidden;
                loadHeaderAni.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Navigation", ex);
            }
        }

        private void addfolderLibraryHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (libFolderHeader.Text.Length >= 8)
                {
                    using (var entitydb = new AyetContext())
                    {
                        var dControl = entitydb.Librarys.Where(p => p.LibraryName == libpreviewName.Text).ToList();

                        if (dControl.Count == 0)
                        {
                            var dLibFolder = new Library { LibraryName = libpreviewName.Text, LibraryColor = libpreviewColor.Background.ToString(), Created = DateTime.Now, Modify = DateTime.Now };
                            entitydb.Librarys.Add(dLibFolder);
                            entitydb.SaveChanges();
                            succsessFunc("Kütphane Başlığı ", " Yeni kütüphane başlığı oluşturuldu artık veri ekleye bilirsiniz.", 3);

                            libpreviewName.Text = "";
                            libFolderHeader.Text = "";
                            addFolderLibHeaderPopup.IsOpen = false;
                        }
                        else
                        {
                            alertFunc("Kütphane Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                        }
                    }
                }
                else
                {
                    libFolderHeader.Focus();
                    libHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    libHeaderFolderErrorMesssage.Content = "Kütphane başlığının uzunluğu minimum 8 karakter olmalı";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void libraryColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chk;

                foreach (object item in libColorStack.Children)
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

                libpreviewColor.Background = new BrushConverter().ConvertFromString(chk.Tag.ToString()) as SolidColorBrush;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void newLibHeader_Click(object sender, RoutedEventArgs e)
        {
            addFolderLibHeaderPopup.IsOpen = true;
        }

        private void libFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                libHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void libFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                libpreviewName.Text = libFolderHeader.Text;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }
    }
}