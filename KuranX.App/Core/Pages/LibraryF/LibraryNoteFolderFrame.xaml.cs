using KuranX.App.Core.Classes;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for LibraryNoteFolderFrame.xaml
    /// </summary>
    public partial class LibraryNoteFolderFrame : Page
    {
        private int lastPage = 0, NowPage = 1;
        private Task loadTask;
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public LibraryNoteFolderFrame()
        {
            InitializeComponent();
        }

        public object PageCall()
        {
            loadTask = new Task(() => loadItem());
            loadTask.Start();
            return this;
        }

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                var dLibrarys = entitydb.Librarys.Skip(lastPage).Take(20).ToList();

                Decimal totalcount = entitydb.Librarys.Count();

                for (int x = 1; x < 21; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("lnt" + x);
                        itemslist.ItemsSource = null;
                    });
                }

                int i = 1;
                List<Classes.Library> tempSub = new List<Classes.Library>();
                foreach (var item in dLibrarys)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        tempSub.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("lnt" + i);
                        itemslist.ItemsSource = tempSub;
                        tempSub.Clear();
                        i++;
                    });
                }

                Thread.Sleep(200);
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
                loadTask = new Task(() => loadItem());
                loadTask.Start();
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
                    loadTask = new Task(() => loadItem());
                    loadTask.Start();
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
                if (libraryFolderHeader.Text.Length >= 8)
                {
                    if (libraryFolderHeader.Text.Length < 50)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Librarys.Where(p => p.LibraryName == librarypreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dlibraryFolder = new Classes.Library { LibraryName = librarypreviewName.Text, LibraryColor = librarypreviewColor.Background.ToString(), Created = DateTime.Now, Modify = DateTime.Now };
                                entitydb.Librarys.Add(dlibraryFolder);
                                entitydb.SaveChanges();
                                succsessFunc("Kütüphane Başlığı ", " Yeni kütüphane başlığı oluşturuldu artık veri ekleye bilirsiniz.", 3);

                                librarypreviewName.Text = "";
                                libraryFolderHeader.Text = "";
                                popup_FolderLibraryPopup.IsOpen = false;
                                dlibraryFolder = null;

                                loadTask = new Task(() => loadItem());
                                loadTask.Start();
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
                        libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığı çok uzun max 50 karakter olabilir";
                    }
                }
                else
                {
                    libraryFolderHeader.Focus();
                    libraryHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    libraryHeaderFolderErrorMesssage.Content = "Kütüphane başlığının uzunluğu minimum 8 karakter olmalı";
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

        // ---------- MessageFunc FUNC ---------- //
    }
}