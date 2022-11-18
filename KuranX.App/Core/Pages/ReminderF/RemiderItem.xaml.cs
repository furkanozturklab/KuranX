using KuranX.App.Core.Classes;
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

namespace KuranX.App.Core.Pages.ReminderF
{
    /// <summary>
    /// Interaction logic for RemiderItem.xaml
    /// </summary>
    public partial class RemiderItem : Page
    {
        private int cV, cS, remiderId;
        private bool tempCheck = false;

        public RemiderItem()
        {
            InitializeComponent();
        }

        public Page PageCall(int id)
        {
            remiderId = id;
            App.loadTask = Task.Run(() => loadItem(id));
            return this;
        }

        public void loadItem(int id)
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                var dRemider = entitydb.Remider.Where(p => p.RemiderId == id).FirstOrDefault();

                this.Dispatcher.Invoke(() =>
                {
                    if (dRemider != null)
                    {
                        if (dRemider.ConnectSureId == 0) gotoVerseButton.IsEnabled = false;
                        else gotoVerseButton.IsEnabled = true;
                        cV = dRemider.ConnectVerseId;
                        cS = dRemider.ConnectSureId;

                        if (dRemider.LoopType == "False") App.mainScreen.navigationWriter("remider", "Tarih Bazlı Hatırlartıcı");
                        else App.mainScreen.navigationWriter("remider", $"{dRemider.LoopType} Bazlı Hatırlartıcı");

                        switch (dRemider.LoopType)
                        {
                            case "False":
                                remiderType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffc107");
                                TimeSpan ts = dRemider.RemiderDate - DateTime.Now;
                                type.Text = "Hatırlatma İçin Kalan Süre : " + ts.Days.ToString() + " Gün " + ts.Hours.ToString() + " Saat " + ts.Minutes.ToString() + " Dakika";
                                type.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffc107");
                                break;

                            case "Gün":
                                remiderType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d63384");
                                type.Text = "Günlük Olarak Tekrarlanıyor";
                                type.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#d63384");
                                break;

                            case "Hafta":
                                remiderType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0d6efd");
                                type.Text = "Haflık Olarak Tekrarlanıyor";
                                type.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#0d6efd");
                                break;

                            case "Ay":
                                remiderType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0dcaf0");
                                type.Text = "Aylık Olarak Tekrarlanıyor";
                                type.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#0dcaf0");
                                break;

                            case "Yıl":
                                remiderType.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#6610f2");
                                type.Text = "Yıllık Olarak Tekrarlanıyor";
                                type.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6610f2");
                                break;
                        }

                        header.Text = dRemider.RemiderName;
                        remiderDetail.Text = dRemider.RemiderDetail;
                        create.Text = dRemider.Create.ToString("d") + " tarihinde oluşturulmuş.";
                    }
                });
                Thread.Sleep(200);
                loadAniComplated();
            }
        }

        // --------------------- Click Func --------------------- //

        private void gotoBackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                App.logWriter("gotoBackButton_Click", ex);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_DeleteConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("deleteButton_Click", ex);
            }
        }

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = App.navVersePage.PageCall(cS, cV, "Remider");
            }
            catch (Exception ex)
            {
                App.logWriter("gotoVerseButton_Click", ex);
            }
        }

        private void deleteRemiderPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    gotoBackButton.IsEnabled = false;
                    gotoVerseButton.IsEnabled = false;
                    deleteButton.IsEnabled = false;
                    saveButton.IsEnabled = false;
                    entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.RemiderId == remiderId));
                    entitydb.Tasks.RemoveRange(entitydb.Tasks.Where(p => p.MissonsId == remiderId));
                    entitydb.SaveChanges();
                    popup_DeleteConfirm.IsOpen = false;
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("deleteRemiderPopupBtn_Click", ex);
            }
        }

        private void voidgobacktimer()
        {
            try
            {
                App.timeSpan.Interval = TimeSpan.FromSeconds(3);
                App.timeSpan.Start();
                succsessFunc("Hatırlatıcı Silme Başarılı", "Hatırlatıcı başaralı bir sekilde silinmiştir. Hatırlatıcı sayfasına yönlendiriliyorsunuz...", 3);
                App.timeSpan.Tick += delegate
                {
                    App.timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("voidgobacktimer", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    if (remiderDetail.Text.Length < 8)
                    {
                        alertFunc("Güncelleme Başarısız", "Yeni hatırlatıcı notunuz çok kısa minimum 8 karakter olmalıdır.", 3);
                    }
                    else
                    {
                        entitydb.Remider.Where(p => p.RemiderId == remiderId).First().RemiderDetail = remiderDetail.Text;
                        entitydb.SaveChanges();
                        succsessFunc("Güncelleme Başarılı", "Hatırlatıcı notunuz güncellendi.", 3);
                        saveButton.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("saveButton_Click", ex);
            }
        }

        // --------------------- Click Func --------------------- //

        // -------------------- Change Func -------------------- //

        private void remiderDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tempCheck)
                {
                    saveButton.IsEnabled = true;
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("remiderDetail_TextChanged", ex);
            }
        }

        // -------------------- Change Func -------------------- //

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
                gotoBackButton.IsEnabled = false;
                gotoVerseButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                remiderDetail.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                gotoBackButton.IsEnabled = true;
                gotoVerseButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
                remiderDetail.IsEnabled = true;
            });
        }

        // ------------ Animation Func ------------ //
    }
}