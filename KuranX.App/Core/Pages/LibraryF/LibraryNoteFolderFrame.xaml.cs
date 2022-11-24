﻿using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for LibraryNoteFolderFrame.xaml
    /// </summary>
    public partial class LibraryNoteFolderFrame : Page
    {
        private int lastPage = 0, NowPage = 1;

        public LibraryNoteFolderFrame()
        {
            InitializeComponent();
        }

        public object PageCall()
        {
            loadHeaderAni.Visibility = Visibility.Hidden;
            App.mainScreen.navigationWriter("library", "Kütüphane Başlıkları");
            App.loadTask = Task.Run(() => loadItem());
            return this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.mainScreen.navigationWriter("library", "Kütüphane Başlıkları");
        }

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                var dLibrarys = entitydb.Librarys.Skip(lastPage).Take(20).ToList();
                Decimal totalcount = entitydb.Librarys.Count();

                for (int x = 1; x <= 20; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var pdfItem = (Border)FindName("lnt" + x);
                        pdfItem.Visibility = Visibility.Hidden;
                    });
                }

                int i = 1;

                Thread.Sleep(300);

                foreach (var item in dLibrarys)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var sColor = (Border)FindName("lntColor" + i);
                        sColor.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(item.libraryColor);

                        var sName = (TextBlock)FindName("lntName" + i);
                        sName.Text = item.libraryName;

                        var sCreated = (TextBlock)FindName("lntCreated" + i);
                        sCreated.Text = item.created.ToString("D");

                        var sBtn = (Button)FindName("lntBtn" + i);
                        sBtn.Uid = item.libraryId.ToString();

                        var sbItem = (Border)FindName("lnt" + i);
                        sbItem.Visibility = Visibility.Visible;
                        i++;
                    });
                }

                this.Dispatcher.Invoke(() =>
                {
                    totalcountText.Tag = totalcount.ToString();

                    if (dLibrarys.Count() != 0)
                    {
                        totalcount = Math.Ceiling(totalcount / 15);
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

        // -------------- Click Func -------------- //

        private void openLibraryFolder_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = App.navLibraryNoteItemsFrame.PageCall(int.Parse(btn.Uid));
        }

        private void newLibHeader_Click(object sender, RoutedEventArgs e)
        {
            popup_FolderLibraryPopup.IsOpen = true;
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 20;
                NowPage++;
                App.loadTask = Task.Run(() => loadItem());
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
                if (lastPage >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 20;
                    NowPage--;
                    App.loadTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
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

        private void addfolderlibrary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (libraryFolderHeader.Text.Length >= 3)
                {
                    if (libraryFolderHeader.Text.Length < 150)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Librarys.Where(p => p.libraryName == librarypreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dlibraryFolder = new Classes.Library { libraryName = librarypreviewName.Text, libraryColor = librarypreviewColor.Background.ToString(), created = DateTime.Now, modify = DateTime.Now };
                                entitydb.Librarys.Add(dlibraryFolder);
                                entitydb.SaveChanges();
                                succsessFunc("Kütüphane Başlığı ", " Yeni kütüphane başlığı oluşturuldu artık veri ekleye bilirsiniz.", 3);

                                librarypreviewName.Text = "Önizlem";
                                libraryFolderHeader.Text = "";
                                popup_FolderLibraryPopup.IsOpen = false;
                                dlibraryFolder = null;

                                App.loadTask = Task.Run(() => loadItem());
                            }
                            else
                            {
                                alertFunc("Kütüphane Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                            }
                            dControl = null;
                        }
                    }
                    else
                    {
                        libraryFolderHeader.Focus();
                        libraryHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                        libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığı çok uzun max 150 karakter olabilir";
                    }
                }
                else
                {
                    libraryFolderHeader.Focus();
                    libraryHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığının uzunluğu minimum 3 karakter olmalı";
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
                CheckBox? chk;

                foreach (object item in libraryColorStack.Children)
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
                librarypreviewColor.Background = new BrushConverter().ConvertFromString(chk.Tag.ToString()) as SolidColorBrush;

                chk = null;
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
                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);
                popuptemp.IsOpen = false;
                libraryHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
                librarypreviewName.Text = "Önizlem";
                libraryFolderHeader.Text = "";

                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // -------------- Click Func -------------- //

        // ---------------- Changed Func ---------------- //

        private void libraryFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                libraryHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void libraryFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                librarypreviewName.Text = libraryFolderHeader.Text;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // ---------------- Changed Func ---------------- //

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

        private void infoFunc(string header, string detail, int timespan)
        {
            try
            {
                infoPopupHeader.Text = header;
                infoPopupDetail.Text = detail;
                inph.IsOpen = true;

                App.timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                App.timeSpan.Start();
                App.timeSpan.Tick += delegate
                {
                    inph.IsOpen = false;
                    App.timeSpan.Stop();
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

                App.timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                App.timeSpan.Start();
                App.timeSpan.Tick += delegate
                {
                    scph.IsOpen = false;
                    App.timeSpan.Stop();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // ---------- MessageFunc FUNC ---------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                backPage.IsEnabled = false;
                newLibHeader.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                backPage.IsEnabled = true;
                newLibHeader.IsEnabled = true;
                loadHeaderAni.Visibility = Visibility.Visible;
            });
        }

        // ------------ Animation Func ------------ //
    }
}