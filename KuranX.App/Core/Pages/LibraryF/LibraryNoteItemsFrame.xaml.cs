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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for LibraryNoteItemsFrame.xaml
    /// </summary>
    public partial class LibraryNoteItemsFrame : Page
    {
        private int lastPage = 0, NowPage = 1, folderId = 0;

        public LibraryNoteItemsFrame()
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

        public object PageCall(int id)
        {
            try
            {
                lastPage = 0;
                NowPage = 1;
                folderId = id;
                App.loadTask = Task.Run(() => loadItem(folderId));
                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
                return this;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainScreen.navigationWriter("library", "Kütüphane Başlıkları," + loadHeader.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
            }
        }

        // --------------- Load Func --------------- //

        public void loadItem(int folderId)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    var dlibrary = entitydb.Librarys.Where(p => p.libraryId == folderId).First();
                    var dNotes = entitydb.Notes.Where(p => p.libraryId == folderId).Skip(lastPage).Take(20).ToList();

                    Decimal totalcount = entitydb.Notes.Where(p => p.libraryId == folderId).Count();

                    this.Dispatcher.Invoke(() =>
                    {
                        loadHeader.Text = dlibrary.libraryName;
                        loadCreated.Text = dlibrary.created.ToString("D");
                        loadHeaderColor.Background = new BrushConverter().ConvertFrom((string)dlibrary.libraryColor) as SolidColorBrush;
                        App.mainScreen.navigationWriter("library", "Kütüphane Başlıkları," + loadHeader.Text);
                    });

                    for (int x = 1; x <= 20; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sbItem = (Border)FindName("lni" + x);
                            sbItem.Visibility = Visibility.Hidden;
                        });
                    }

                    int i = 1;
                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    foreach (var item in dNotes)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sName = (TextBlock)FindName("lniName" + i);
                            sName.Text = item.noteHeader;

                            var sCreated = (TextBlock)FindName("lniCreate" + i);
                            sCreated.Text = item.created.ToString("D");

                            var sLocation = (TextBlock)FindName("lniLocation" + i);
                            sLocation.Text = item.noteLocation;

                            var sBtn = (Button)FindName("lniBtn" + i);
                            sBtn.Uid = item.notesId.ToString();

                            var sbItem = (Border)FindName("lni" + i);
                            sbItem.Visibility = Visibility.Visible;
                            i++;
                        });
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        totalcountText.Tag = totalcount.ToString();

                        if (dNotes.Count() != 0)
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
            catch (Exception ex)
            {
                App.logWriter("Loading Func", ex);
            }
        }

        // --------------- Load Func --------------- //

        // --------------- Click Func --------------- //

        private void libNoteItemsOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void libraryFolderDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_LibraryDelete.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void libraryFolderRenameBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popupNewName.Text = loadHeader.Text;
                popup_newName.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void newNamePopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    if (popupNewName.Text.Length >= 3)
                    {
                        entitydb.Librarys.Where(p => p.libraryId == folderId).First().libraryName = popupNewName.Text;
                        entitydb.Librarys.Where(p => p.libraryId == folderId).First().modify = DateTime.Now;
                        loadHeader.Text = popupNewName.Text;
                        entitydb.SaveChanges();
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Kütüphane başlığınız başarılı bir sekilde değiştirilmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        popup_newName.IsOpen = false;
                    }
                    else
                    {
                        popupNewName.Focus();
                        popupRelativeIdError.Content = "Kütüphane başlığının uzunluğu minimum 3 karakter olmalı";
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Click", ex);
            }
        }

        private void sendResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_sendResult.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 20;
                NowPage++;
                App.loadTask = Task.Run(() => loadItem(folderId));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                    App.loadTask = Task.Run(() => loadItem(folderId));
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void deleteLibraryPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dUpdate = entitydb.Notes.Where(p => p.libraryId == folderId).ToList();

                    foreach (var d in dUpdate)
                    {
                        d.libraryId = 0;
                    }
                    entitydb.Librarys.RemoveRange(entitydb.Librarys.Where(p => p.libraryId == folderId));
                    entitydb.SaveChanges();
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void connectResultControl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var item = popupResultSureId.SelectedItem as ComboBoxItem;
                    var dResult = entitydb.Results.Where(p => p.resultId == int.Parse(item.Uid)).FirstOrDefault();

                    if (entitydb.ResultItems.Where(p => p.resultId == dResult.resultId && p.resultLibId == folderId).Count() == 0)
                    {
                        dResult.resultLib = true;
                        var dTemp = new ResultItem { resultId = dResult.resultId, resultLibId = folderId, sendTime = DateTime.Now };
                        entitydb.ResultItems.Add(dTemp);
                        entitydb.SaveChanges();
                        popup_sendResult.IsOpen = false;
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Kütüphane başlığı " + item.Content + " suresinin sonucuna gönderildi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                    else
                    {
                        popup_sendResult.IsOpen = false;
                        App.mainScreen.alertFunc("İşlem Başarısız", "Kütüphane başlığı " + item.Content + " suresinin sonucuna daha önceden eklenmiştir yeniden ekleyemezsiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName((string)btntemp.Uid.ToString());

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // --------------- Click Func --------------- //

        // ------------- Change Func --------------- //

        private void popupNewName_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void popupNewName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                popupRelativeIdError.Content = "Yeni Konu Başlığını Giriniz";
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        // ------------- Change Func --------------- //

        // --------------- TimeSpan FUNC --------------- //

        private void voidgobacktimer()
        {
            try
            {
                backPage.IsEnabled = false;
                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;
                sendResult.IsEnabled = false;

                App.timeSpan.Interval = TimeSpan.FromSeconds(3);
                App.timeSpan.Start();
                App.mainScreen.succsessFunc("İşlem Başarılı", "Kütüphane başlığı ve başlığa ait tüm notlar serbes bırakıldı. Kütüphaneye yönlendiriliyorsunuz...", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                App.timeSpan.Tick += delegate
                {
                    App.timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("TimeSpan", ex);
            }
        }

        // --------------- TimeSpan FUNC --------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    backPage.IsEnabled = false;
                    sendResult.IsEnabled = false;
                    libraryItemsDeleteBtn.IsEnabled = false;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        public void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    backPage.IsEnabled = true;
                    sendResult.IsEnabled = true;
                    libraryItemsDeleteBtn.IsEnabled = true;
                    loadHeaderStack.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //

        // ----------- Popuper Spec Func ----------- //

        public void popuverMove_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            ppMoveConfing((string)btn.Uid);
            moveControlName.Text = (string)btn.Content;
            pp_moveBar.IsOpen = true;
        }

        public void ppMoveActionOfset_Click(object sender, RoutedEventArgs e)
        {
            var btntemp = sender as Button;
            var movePP = (Popup)FindName((string)btntemp.Content);

            switch (btntemp.Uid.ToString())
            {
                case "Left":
                    movePP.HorizontalOffset -= 50;
                    break;

                case "Top":
                    movePP.VerticalOffset -= 50;
                    break;

                case "Bottom":
                    movePP.VerticalOffset += 50;
                    break;

                case "Right":
                    movePP.HorizontalOffset += 50;
                    break;

                case "UpLeft":
                    movePP.Placement = PlacementMode.Absolute;
                    movePP.VerticalOffset = 0;
                    movePP.HorizontalOffset = 0;
                    break;

                case "Reset":
                    movePP.Placement = PlacementMode.Center;
                    movePP.VerticalOffset = 0;
                    movePP.HorizontalOffset = 0;
                    break;

                case "Close":
                    pp_moveBar.IsOpen = false;
                    break;
            }
        }

        public void ppMoveConfing(string ppmove)
        {
            Debug.WriteLine(ppmove);
            for (int i = 1; i < 8; i++)
            {
                var btn = FindName("pp_M" + i) as Button;
                btn.Content = ppmove;
            }
        }
    }
}