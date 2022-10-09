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
        private int remiderId;
        private bool tempCheck = false;
        private Remider dRemider = new Remider();
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private Task loadTask;

        public RemiderItem()
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

        public RemiderItem(int id) : this()
        {
            try
            {
                remiderId = id;
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        //-------------------------- LOADED FUNC  --------------------------//

        private void loadRemider()
        {
            try
            {
                loadAni();
                using (var entitydb = new AyetContext())
                {
                    dRemider = entitydb.Remider.Where(p => p.RemiderId == remiderId).FirstOrDefault();

                    this.Dispatcher.Invoke(() =>
                    {
                        if (dRemider != null)
                        {
                            if (dRemider.ConnectSureId == 0) gotoVerseButton.IsEnabled = false;
                            else gotoVerseButton.IsEnabled = true;

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
                }
                loadAniComplated();
            }
            catch (Exception ex)
            {
                App.logWriter("loadRemider", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadTask = new Task(loadRemider);
                loadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("Page_Loaded", ex);
            }
        }

        //-------------------------- LOADED FUNC  --------------------------//

        //-------------------------- POPUP OPEN FUNC  --------------------------//

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                deleteNotepopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("deleteButton_Click", ex);
            }
        }

        //-------------------------- POPUP OPEN FUNC  --------------------------//

        //-------------------------- ACTİONS FUNC  --------------------------//

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dRemider.RemiderDetail = remiderDetail.Text;
                    entitydb.Remider.Update(dRemider);
                    entitydb.SaveChanges();
                    saveButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("saveButton_Click", ex);
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
                    deleteNotepopup.IsOpen = false;
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
                timeSpan.Interval = TimeSpan.FromSeconds(3);
                timeSpan.Start();
                succsessFunc("Hatırlatıcı Silme Başarılı", "Hatırlatıcı başaralı bir sekilde silinmiştir. Hatırlatıcı sayfasına yönlendiriliyorsunuz...", 3);
                timeSpan.Tick += delegate
                {
                    timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("voidgobacktimer", ex);
            }
        }

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new VerseF.verseFrame(dRemider.ConnectSureId, dRemider.ConnectVerseId, "Remider");
            }
            catch (Exception ex)
            {
                App.logWriter("gotoVerseButton_Click", ex);
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
                App.logWriter("popupClosed_Click", ex);
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
                App.logWriter("succsessFunc", ex);
            }
        }

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

        //-------------------------- ACTİONS FUNC  --------------------------//

        //-------------------------- KEYDOWN FUNC  --------------------------//
        private void noteDetail_TextChanged(object sender, TextChangedEventArgs e)
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
                App.logWriter("noteDetail_TextChanged", ex);
            }
        }

        //-------------------------- KEYDOWN FUNC  --------------------------//

        //-------------------------- Animations FUNC  --------------------------//

        private void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("loadAni", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Collapsed;
                    loadBorder.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("loadAniComplated", ex);
            }
        }

        //-------------------------- Animations FUNC  --------------------------//
    }
}