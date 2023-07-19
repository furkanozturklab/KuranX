using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

namespace KuranX.App.Core.Pages.ReminderF
{
    /// <summary>
    /// Interaction logic for RemiderItem.xaml
    /// </summary>
    public partial class RemiderItem : Page
    {
        private int cV, cS, remiderId;
        private bool tempCheck = false;
        private Task remiderItem;

        public RemiderItem()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} InitializeComponent ] -> RemiderItem");
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall(int id)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} PageCall ] -> RemiderItem");
                remiderId = id;
                remiderItem = Task.Run(() => loadItem(id));
                App.lastlocation = "RemiderItem";
                return this;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
                return this;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} Page_Loaded ] -> RemiderItem");
                loadHeaderStack.Visibility = Visibility.Visible;
                controlBar.Visibility = Visibility.Visible;
                remiderDetail.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        public void loadItem(int id)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadItem ] -> RemiderItem");


                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    var dRemider = entitydb.Remider.Where(p => p.remiderId == id).FirstOrDefault();

                    if (dRemider != null)
                    {
                        cV = dRemider.connectVerseId;
                        cS = dRemider.connectSureId;

                        if (dRemider.loopType == "False") App.mainScreen.navigationWriter("remider", "Tarih Bazlı Hatırlartıcı");
                        else App.mainScreen.navigationWriter("remider", $"{dRemider.loopType} Bazlı Hatırlartıcı");

                        switch (dRemider.loopType)
                        {
                            case "False":
                                this.Dispatcher.Invoke(() =>
                                {
                                    remiderType.Background = new BrushConverter().ConvertFrom("#ffc107") as SolidColorBrush;
                                    TimeSpan ts = dRemider.remiderDate - DateTime.Now;
                                    type.Text = "Hatırlatma İçin Kalan Süre : " + ts.Days.ToString() + " Gün " + ts.Hours.ToString() + " Saat " + ts.Minutes.ToString() + " Dakika";
                                    type.Foreground = new BrushConverter().ConvertFrom("#ffc107") as SolidColorBrush;
                                });
                                break;

                            case "Gün":
                                this.Dispatcher.Invoke(() =>
                                {
                                    remiderType.Background = new BrushConverter().ConvertFrom("#d63384") as SolidColorBrush;
                                    type.Text = "Günlük Olarak Tekrarlanıyor";
                                    type.Foreground = new BrushConverter().ConvertFrom("#d63384") as SolidColorBrush;
                                });
                                break;

                            case "Hafta":
                                this.Dispatcher.Invoke(() =>
                                {
                                    remiderType.Background = new BrushConverter().ConvertFrom("#0d6efd") as SolidColorBrush;
                                    type.Text = "Haflık Olarak Tekrarlanıyor";
                                    type.Foreground = new BrushConverter().ConvertFrom("#0d6efd") as SolidColorBrush;
                                });
                                break;

                            case "Ay":
                                this.Dispatcher.Invoke(() =>
                                {
                                    remiderType.Background = new BrushConverter().ConvertFrom("#0dcaf0") as SolidColorBrush;
                                    type.Text = "Aylık Olarak Tekrarlanıyor";
                                    type.Foreground = new BrushConverter().ConvertFrom("#0dcaf0") as SolidColorBrush;
                                });
                                break;

                            case "Yıl":
                                this.Dispatcher.Invoke(() =>
                                {
                                    remiderType.Background = new BrushConverter().ConvertFrom("#6610f2") as SolidColorBrush;
                                    type.Text = "Yıllık Olarak Tekrarlanıyor";
                                    type.Foreground = new BrushConverter().ConvertFrom("#6610f2") as SolidColorBrush;
                                });
                                break;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            header.Text = dRemider.remiderName;
                            remiderDetail.Text = dRemider.remiderDetail;
                            create.Text = dRemider.create.ToString("d") + " tarihinde oluşturulmuş.";
                        });
                    }

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    this.Dispatcher.Invoke(() =>
                    {
                        if (dRemider.connectSureId == 0) gotoVerseButton.IsEnabled = false;
                        else gotoVerseButton.IsEnabled = true;
                    });

                    loadAniComplated();
                }

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading Func", ex);
            }
        }

        // --------------------- Loadind Func ---------------- //

        // --------------------- Click Func --------------------- //

        private void gotoBackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} gotoBackButton_Click ] -> RemiderItem");

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} deleteButton_Click ] -> RemiderItem");
                popup_DeleteConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} gotoVerseButton_Click ] -> RemiderItem");
                loadHeaderStack.Visibility = Visibility.Hidden;
                controlBar.Visibility = Visibility.Hidden;
                remiderDetail.Visibility = Visibility.Hidden;

            
                App.secondFrame.Content = App.navVerseStickPage.PageCall(cS, cV, "", 0, "Remider");
                App.secondFrame.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void deleteRemiderPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} deleteRemiderPopupBtn_Click ] -> RemiderItem");


                using (var entitydb = new AyetContext())
                {
                    gotoBackButton.IsEnabled = false;
                    gotoVerseButton.IsEnabled = false;
                    deleteButton.IsEnabled = false;
                    saveButton.IsEnabled = false;

                    var control = entitydb.Remider.Where(p => p.connectSureId != 0 && p.remiderId == remiderId);
                    if (control.Count() >= 1)
                    {
                        entitydb.Verse.Where(p => p.sureId == control.FirstOrDefault().connectSureId && p.relativeDesk == control.FirstOrDefault().connectVerseId).First().remiderCheck = false;
                    }

                    entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.remiderId == remiderId));
                    entitydb.Tasks.RemoveRange(entitydb.Tasks.Where(p => p.missonsId == remiderId));
                    entitydb.SaveChanges();
                    popup_DeleteConfirm.IsOpen = false;
                    App.mainScreen.succsessFunc("İşlem Başarılı", "Hatırlatıcı başaralı bir sekilde silinmiştir. Bir önceki sayfaya yönlendiriliyorsunuz bekleyin...", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    App.mainframe.Content = App.navRemiderPage.PageCall();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} popupClosed_Click ] -> RemiderItem");

                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} saveButton_Click ] -> RemiderItem");


                using (var entitydb = new AyetContext())
                {
                    if (remiderDetail.Text.Length <= 3)
                    {
                        App.mainScreen.alertFunc("İşlem Başarısız", "Yeni hatırlatıcı notunuz çok kısa minimum 3 karakter olmalıdır.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                    else
                    {
                        entitydb.Remider.Where(p => p.remiderId == remiderId).First().remiderDetail = remiderDetail.Text;
                        entitydb.SaveChanges();
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Hatırlatıcı notunuz güncellendi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        saveButton.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        // --------------------- Click Func --------------------- //

        // --------------------- Timer Func --------------------- //

        private void voidgobacktimer()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} voidgobacktimer ] -> RemiderItem");

                App.timeSpan.Interval = TimeSpan.FromSeconds(int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                App.timeSpan.Start();
                App.mainScreen.succsessFunc("İşlem Başarılı", "Hatırlatıcı başaralı bir sekilde silinmiştir. Hatırlatıcı sayfasına yönlendiriliyorsunuz...", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                App.timeSpan.Tick += delegate
                {
                    App.timeSpan.Stop();

                    App.mainframe.Content = App.navRemiderPage.PageCall();
                };
            }
            catch (Exception ex)
            {
                Tools.logWriter("Timer", ex);
            }
        }

        // --------------------- Timer Func --------------------- //

        // -------------------- Change Func -------------------- //

        private void remiderDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} remiderDetail_TextChanged ] -> RemiderItem");

                if (tempCheck)
                {
                    saveButton.IsEnabled = true;
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        // -------------------- Change Func -------------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadAni ] -> RemiderItem");


                this.Dispatcher.Invoke(() =>
                {
                    gotoBackButton.IsEnabled = false;
                    gotoVerseButton.IsEnabled = false;
                    deleteButton.IsEnabled = false;
                    remiderDetail.IsEnabled = false;
                    gotoVerseButton.IsEnabled = false;
                    loadHeaderStack.Visibility = Visibility.Hidden;
                    controlBar.Visibility = Visibility.Hidden;
                    remiderDetail.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Animation", ex);
            }
        }

        public void loadAniComplated()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadAniComplated ] -> RemiderItem");

                this.Dispatcher.Invoke(() =>
                {
                    gotoBackButton.IsEnabled = true;
                    deleteButton.IsEnabled = true;
                    remiderDetail.IsEnabled = true;
                    saveButton.IsEnabled = false;

                    loadHeaderStack.Visibility = Visibility.Visible;
                    controlBar.Visibility = Visibility.Visible;
                    remiderDetail.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //
    }
}