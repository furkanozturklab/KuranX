using KuranX.App.Core.Classes;
using MySqlX.XDevAPI.Common;
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

namespace KuranX.App.Core.Pages.ReminderF
{
    /// <summary>
    /// Interaction logic for RemiderFrame.xaml
    /// </summary>
    public partial class RemiderFrame : Page
    {
        private List<Remider> dRemider, tempRemider = new List<Remider>();
        private int selectedId, NowPage = 1, lastRemider = 0;
        private DispatcherTimer? timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private Task loadTask;
        private bool remiderType = false, filter = false;
        private string filterTxt;

        public RemiderFrame()
        {
            InitializeComponent();
        }

        //-------------------------- LOAD FUNC  --------------------------//

        private void loadRemider()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();

                    Decimal totalcount = entitydb.Remider.Count();

                    if (filter)
                    {
                        dRemider = entitydb.Remider.Where(p => p.LoopType == filterTxt).OrderBy(p => p.Priority).Skip(lastRemider).Take(7).ToList();
                        totalcount = dRemider.Count;
                    }
                    else
                    {
                        dRemider = entitydb.Remider.OrderBy(p => p.Priority).Skip(lastRemider).Take(7).ToList();
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        for (int x = 1; x < 7; x++)
                        {
                            ItemsControl itemslist = (ItemsControl)this.FindName("cb" + x);
                            itemslist.ItemsSource = null;
                        }
                        int i = 1;

                        foreach (var item in dRemider)
                        {
                            tempRemider.Add(item);

                            switch (tempRemider[0].LoopType)
                            {
                                case "False":
                                    tempRemider[0].LoopType = "#ffc107";
                                    tempRemider[0].Status = tempRemider[0].RemiderDate.ToString("d") + " Tarihin de Hatırlatılacak";
                                    break;

                                case "Gün":
                                    tempRemider[0].LoopType = "#d63384";
                                    tempRemider[0].Status = "Günlük Olarak Tekrarlanıyor";
                                    break;

                                case "Hafta":
                                    tempRemider[0].LoopType = "#0d6efd";
                                    tempRemider[0].Status = "Haftalık Olarak Tekrarlanıyor";
                                    break;

                                case "Ay":
                                    tempRemider[0].LoopType = "#0dcaf0";
                                    tempRemider[0].Status = "Aylık Olarak Tekrarlanıyor";
                                    break;

                                case "Yıl":
                                    tempRemider[0].LoopType = "#6610f2";
                                    tempRemider[0].Status = "Yıllık Olarak Tekrarlanıyor";
                                    break;
                            }

                            ItemsControl itemslist = (ItemsControl)this.FindName("cb" + i);
                            itemslist.ItemsSource = tempRemider;
                            tempRemider.Clear();
                            i++;

                            if (i == 7) break; // 7 den fazla varmı kontrol etmek için koydum
                        }

                        Thread.Sleep(200);
                        this.Dispatcher.Invoke(() =>
                        {
                            totalcountText.Tag = totalcount.ToString();

                            if (dRemider.Count() != 0)
                            {
                                totalcount = Math.Ceiling(totalcount / 6);
                                nowPageStatus.Tag = NowPage + " / " + totalcount;
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
                    });

                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadTask = new Task(loadRemider);
            loadTask.Start();
        }

        //-------------------------- LOAD FUNC  --------------------------//

        //-------------------------- POPUP OPEN FUNC  --------------------------//

        private void remiderDelay_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            selectedId = int.Parse(btn.Uid);
            deleteRemiderpopup.IsOpen = true;
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

        private void newRemider_Click(object sender, RoutedEventArgs e)
        {
            remiderAddPopup.IsOpen = true;
        }

        //-------------------------- POPUP OPEN FUNC  --------------------------//

        //-------------------------- ACTİONS FUNC  --------------------------//

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if ((string)btn.Uid == "Hepsi")
            {
                filter = false;
            }
            else
            {
                filter = true;
                filterTxt = btn.Uid.ToString();
            }

            loadTask = new Task(loadRemider);
            loadTask.Start();
        }

        private void remiderDetail_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            viewRemider.Visibility = Visibility.Hidden;
            App.mainframe.Content = new RemiderItem(int.Parse(btn.Uid.ToString()));
        }

        private void deleteRemiderPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.RemiderId == selectedId));
                    entitydb.SaveChanges();
                    succsessFunc("Hatırlatıcı Silme Başarılı", "Hatırlatıcı başaralı bir sekilde silinmiştir...", 3);
                    deleteRemiderpopup.IsOpen = false;
                    loadTask = new Task(loadRemider);
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("deleteRemiderPopupBtn_Click", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastRemider += 6;
                NowPage++;
                loadTask = new Task(loadRemider);
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
                if (lastRemider >= 6)
                {
                    previusPageButton.IsEnabled = false;
                    lastRemider -= 6;
                    NowPage--;
                    loadTask = new Task(loadRemider);
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void remiderTypeChangeButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(loopType.Visibility.ToString());

            if (loopType.Visibility.ToString() == "Hidden")
            {
                remiderType = true;
                loopType.Visibility = Visibility.Visible;
                dayType.Visibility = Visibility.Hidden;
                remiderAddPopupDateError.Visibility = Visibility.Hidden;
            }
            else
            {
                remiderType = false;
                loopType.Visibility = Visibility.Hidden;
                dayType.Visibility = Visibility.Visible;
                remiderAddPopupDateError.Visibility = Visibility.Hidden;
            }
        }

        private void addRemiderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (remiderName.Text.Length >= 5)
                {
                    if (remiderDetail.Text.Length >= 5)
                    {
                        if (remiderType == false)
                        {
                            if (remiderDay.SelectedDate != null)
                            {
                                using (var entitydb = new AyetContext())
                                {
                                    var newRemider = new Remider { ConnectSureId = 0, ConnectVerseId = 0, RemiderDate = (DateTime)remiderDay.SelectedDate, RemiderDetail = remiderDetail.Text, RemiderName = remiderName.Text, Create = DateTime.Now, Priority = 1, LastAction = DateTime.Now };

                                    entitydb.Remider.Add(newRemider);
                                    entitydb.SaveChanges();
                                    succsessFunc("Hatırlatıcı Oluşturuldu.", "Yeni hatırlatıcınız oluşturuldu", 3);

                                    remiderAddPopup.IsOpen = false;
                                    remiderName.Text = "";
                                    remiderDetail.Text = "";
                                    remiderDay.SelectedDate = null;

                                    loadTask = new Task(loadRemider);
                                    loadTask.Start();
                                }
                            }
                            else
                            {
                                remiderAddPopupDateError.Visibility = Visibility.Visible;
                                remiderDay.Focus();
                                remiderAddPopupDateError.Content = "Hatırlatıcı için gün secmelisiniz.";
                            }
                        }
                        else
                        {
                            var ditem = loopSelectedType.SelectedItem as ComboBoxItem;

                            if (ditem.Uid != "select")
                            {
                                using (var entitydb = new AyetContext())
                                {
                                    int pr = 0;
                                    switch (ditem.Uid)
                                    {
                                        case "Gün":
                                            pr = 2;
                                            break;

                                        case "Hafta":
                                            pr = 3;
                                            break;

                                        case "Ay":
                                            pr = 4;
                                            break;

                                        case "Yıl":
                                            pr = 5;
                                            break;
                                    }
                                    var newRemider = new Remider { ConnectSureId = 0, ConnectVerseId = 0, RemiderDate = new DateTime(1, 1, 1, 0, 0, 0, 0), RemiderDetail = remiderDetail.Text, RemiderName = remiderName.Text, Create = DateTime.Now, LoopType = ditem.Uid.ToString(), Status = "Run", Priority = pr, LastAction = DateTime.Now };

                                    entitydb.Remider.Add(newRemider);
                                    entitydb.SaveChanges();
                                    succsessFunc("Hatırlatıcı Oluşturuldu.", "Yeni hatırlatıcınız oluşturuldu", 3);

                                    remiderAddPopup.IsOpen = false;
                                    remiderName.Text = "";
                                    remiderDetail.Text = "";
                                    loopSelectedType.SelectedIndex = 0;

                                    loadTask = new Task(loadRemider);
                                    loadTask.Start();
                                }
                            }
                            else
                            {
                                remiderAddPopupDateError.Visibility = Visibility.Visible;
                                loopSelectedType.Focus();
                                remiderAddPopupDateError.Content = "Hatırlatma Aralığını Secmelisiniz.";
                            }
                        }
                    }
                    else
                    {
                        remiderAddPopupDetailError.Visibility = Visibility.Visible;
                        remiderDetail.Focus();
                        remiderAddPopupDetailError.Content = "Hatırlatıcı notu Yeterince Uzun Değil. Min 5 Karakter Olmalıdır";
                    }
                }
                else
                {
                    remiderAddPopupHeaderError.Visibility = Visibility.Visible;
                    remiderName.Focus();
                    remiderAddPopupHeaderError.Content = "Hatırlatıcı İsmi Yeterince Uzun Değil. Min 5 Karakter Olmalıdır";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        //-------------------------- ACTİONS FUNC  --------------------------//

        //-------------------------- KEYDOWN FUNC  --------------------------//

        private void remiderName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                remiderAddPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void remiderDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                remiderAddPopupDetailError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
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
                    filterAll.IsEnabled = false;
                    filterDatetime.IsEnabled = false;
                    filterDay.IsEnabled = false;
                    filterWeek.IsEnabled = false;
                    filterMonth.IsEnabled = false;
                    newRemider.IsEnabled = true;
                    filterYears.IsEnabled = false;
                    loadinItemsGifContent.Visibility = Visibility.Visible;
                    viewRemider.Visibility = Visibility.Hidden;
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
                    filterAll.IsEnabled = true;
                    filterDatetime.IsEnabled = true;
                    filterDay.IsEnabled = true;
                    filterWeek.IsEnabled = true;
                    filterMonth.IsEnabled = true;
                    filterYears.IsEnabled = true;
                    newRemider.IsEnabled = true;
                    viewRemider.Visibility = Visibility.Visible;
                    loadinGifContent.Visibility = Visibility.Collapsed;
                    loadinItemsGifContent.Visibility = Visibility.Collapsed;
                    loadGrid.Visibility = Visibility.Visible;
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