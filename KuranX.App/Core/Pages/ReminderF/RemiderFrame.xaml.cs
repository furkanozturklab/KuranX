using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;

namespace KuranX.App.Core.Pages.ReminderF
{
    /// <summary>
    /// Interaction logic for RemiderFrame.xaml
    /// </summary>
    public partial class RemiderFrame : Page
    {
        private bool filter, remiderType = false;
        private string filterText;
        private int lastPage = 0, NowPage = 1, selectedId;
        private List<Remider> dRemider = new List<Remider>();

        public RemiderFrame()
        {
            InitializeComponent();
            remiderDay.DisplayDateStart = DateTime.Now.AddDays(1);
        }

        public Page PageCall()
        {
            lastPage = 0;
            NowPage = 1;
            App.loadTask = Task.Run(() => loadItem());
            return this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewRemider.Visibility = Visibility.Visible;
            App.loadTask = Task.Run(() => loadItem());
        }

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                Decimal totalcount = entitydb.Remider.Count();

                App.mainScreen.navigationWriter("remider", "");

                if (filter)
                {
                    dRemider = entitydb.Remider.Where(p => p.LoopType == filterText).OrderBy(p => p.Priority).Skip(lastPage).Take(6).ToList();
                    totalcount = dRemider.Count;
                }
                else
                {
                    dRemider = entitydb.Remider.OrderBy(p => p.Priority).Skip(lastPage).Take(6).ToList();
                }

                for (int x = 1; x <= 6; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var sbItem = (Border)FindName("cb" + x);
                        sbItem.Visibility = Visibility.Hidden;
                    });
                }

                int i = 1;

                Thread.Sleep(200);

                foreach (var item in dRemider)
                {
                    this.Dispatcher.InvokeAsync(() =>
                    {
                        var sColor = (Border)FindName("cbTypeColor" + i);
                        var sColor2 = (Border)FindName("cbBtTypeColor" + i);
                        var sBtnStat = (TextBlock)FindName("cbStatus" + i);

                        switch (item.LoopType)
                        {
                            case "False":
                                sColor.Background = new BrushConverter().ConvertFrom("#ffc107") as SolidColorBrush;
                                sColor2.Background = new BrushConverter().ConvertFrom("#ffc107") as SolidColorBrush;
                                sBtnStat.Text = item.RemiderDate.ToString("d") + " Tarihin de Hatırlatılacak";
                                break;

                            case "Gün":
                                sColor.Background = new BrushConverter().ConvertFrom("#d63384") as SolidColorBrush;
                                sColor2.Background = new BrushConverter().ConvertFrom("#d63384") as SolidColorBrush;
                                sBtnStat.Text = "Günlük Olarak Tekrarlanıyor";
                                break;

                            case "Hafta":
                                sColor.Background = new BrushConverter().ConvertFrom("#0d6efd") as SolidColorBrush;
                                sColor2.Background = new BrushConverter().ConvertFrom("#0d6efd") as SolidColorBrush;
                                sBtnStat.Text = "Haftalık Olarak Tekrarlanıyor";
                                break;

                            case "Ay":
                                sColor.Background = new BrushConverter().ConvertFrom("#0dcaf0") as SolidColorBrush;
                                sColor2.Background = new BrushConverter().ConvertFrom("#0dcaf0") as SolidColorBrush;
                                sBtnStat.Text = "Aylık Olarak Tekrarlanıyor";
                                break;

                            case "Yıl":
                                sColor.Background = new BrushConverter().ConvertFrom("#6610f2") as SolidColorBrush;
                                sColor2.Background = new BrushConverter().ConvertFrom("#6610f2") as SolidColorBrush;
                                sBtnStat.Text = "Yıllık Olarak Tekrarlanıyor";
                                break;
                        }

                        var sName = (TextBlock)FindName("cbName" + i);
                        var sDetail = (TextBlock)FindName("cbDetail" + i);
                        var sBtnDel = (Button)FindName("cbBtnDel" + i);
                        var sBtnGo = (Button)FindName("cbBtnGo" + i);

                        sName.Text = item.RemiderName;
                        sDetail.Text = item.RemiderDetail;
                        sBtnDel.Uid = item.RemiderId.ToString();
                        sBtnGo.Uid = item.RemiderId.ToString();

                        var sbItem = (Border)FindName("cb" + i);
                        sbItem.Visibility = Visibility.Visible;

                        i++;
                    });
                }
                this.Dispatcher.InvokeAsync(() =>
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
                loadAniComplated();
            }
        }

        // ---------------- Click Func ---------------- //

        private void newRemider_Click(object sender, RoutedEventArgs e)
        {
            popup_remiderAddPopup.IsOpen = true;
        }

        private void remiderVerseAddButton_Click(object sender, RoutedEventArgs e)
        {
            popup_VerseSelect.IsOpen = true;
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if ((string)btn.Uid == "Hepsi")
            {
                filter = false;
            }
            else
            {
                filter = true;
                filterText = (string)btn.Uid.ToString();
            }

            App.loadTask = Task.Run(() => loadItem());
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 6;
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
                if (lastPage >= 6)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 6;
                    NowPage--;
                    App.loadTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void remiderTypeChangeButton_Click(object sender, RoutedEventArgs e)
        {
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
                if (remiderName.Text.Length >= 3)
                {
                    if (remiderName.Text.Length < 150)
                    {
                        if (remiderDetail.Text.Length >= 3)
                        {
                            if (remiderType == false)
                            {
                                if (remiderDay.SelectedDate != null)
                                {
                                    using (var entitydb = new AyetContext())
                                    {
                                        var newRemider = new Remider();

                                        if (sId.Text != "0") newRemider = new Remider { ConnectSureId = int.Parse(sId.Text), ConnectVerseId = int.Parse(vId.Text), RemiderDate = (DateTime)remiderDay.SelectedDate, RemiderDetail = remiderDetail.Text, RemiderName = remiderName.Text, Create = DateTime.Now, Priority = 1, LastAction = DateTime.Now };
                                        else newRemider = new Remider { ConnectSureId = 0, ConnectVerseId = 0, RemiderDate = (DateTime)remiderDay.SelectedDate, RemiderDetail = remiderDetail.Text, RemiderName = remiderName.Text, Create = DateTime.Now, Priority = 1, LastAction = DateTime.Now };

                                        entitydb.Remider.Add(newRemider);
                                        entitydb.SaveChanges();
                                        succsessFunc("Ekleme Başarılı", "Yeni bir hatırlatıcınız oluşturuldu", 3);

                                        popup_remiderAddPopup.IsOpen = false;
                                        remiderName.Text = "";
                                        remiderDetail.Text = "";
                                        remiderDay.SelectedDate = null;
                                        App.loadTask = Task.Run(() => loadItem());
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

                                        popup_remiderAddPopup.IsOpen = false;
                                        remiderName.Text = "";
                                        remiderDetail.Text = "";
                                        loopSelectedType.SelectedIndex = 0;

                                        App.loadTask = Task.Run(() => loadItem());
                                    }
                                }
                                else
                                {
                                    remiderAddPopupDateError.Visibility = Visibility.Visible;
                                    loopSelectedType.Focus();
                                    remiderAddPopupDateError.Content = "Hatırlatma Aralığını Secmelisiniz.";
                                }
                                ditem = null;
                            }
                        }
                        else
                        {
                            remiderAddPopupDetailError.Visibility = Visibility.Visible;
                            remiderDetail.Focus();
                            remiderAddPopupDetailError.Content = "Hatırlatıcı notu Yeterince Uzun Değil. Min 3 Karakter Olmalıdır";
                        }
                    }
                    else
                    {
                        remiderAddPopupHeaderError.Visibility = Visibility.Visible;
                        remiderName.Focus();
                        remiderAddPopupHeaderError.Content = "Hatırlatıcı İsmi Çok Uzun. Max 150 Karakter Olabilir";
                    }
                }
                else
                {
                    remiderAddPopupHeaderError.Visibility = Visibility.Visible;
                    remiderName.Focus();
                    remiderAddPopupHeaderError.Content = "Hatırlatıcı İsmi Yeterince Uzun Değil. Min 3 Karakter Olmalıdır";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void remiderDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            selectedId = int.Parse(btn.Uid);
            popup_DeleteConfirm.IsOpen = true;
        }

        private void remiderDetail_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            viewRemider.Visibility = Visibility.Hidden;
            App.mainframe.Content = App.navRemiderItem.PageCall(int.Parse(btn.Uid.ToString()));
        }

        private void deleteRemiderPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.RemiderId == selectedId));
                    entitydb.Tasks.RemoveRange(entitydb.Tasks.Where(p => p.MissonsId == selectedId));
                    entitydb.SaveChanges();
                    popup_DeleteConfirm.IsOpen = false;
                    succsessFunc("Hatırlatıcı Silme Başarılı", "Hatırlatıcı başaralı bir sekilde silinmiştir.", 3);
                    App.loadTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("deleteRemiderPopupBtn_Click", ex);
            }
        }

        private void remiderAddVersePopup_Click(object sender, RoutedEventArgs e)
        {
            var item = popupNextSureId.SelectedItem as ComboBoxItem;

            sId.Text = (string)item.Uid.ToString();
            vId.Text = popupRelativeId.Text;
            remiderConnectVerse.Text = (string)item.Content.ToString() + " suresini " + popupRelativeId.Text + " ayeti"; ;

            popup_VerseSelect.IsOpen = false;
            popupNextSureId.SelectedIndex = 0;
            popupRelativeId.Text = "1";
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);
                popuptemp.IsOpen = false;

                remiderName.Text = "";
                popupNextSureId.SelectedIndex = 0;
                popupRelativeId.Text = "1";
                remiderConnectVerse.Text = "Ayet Seçilmemiş";

                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        // ---------------- Click Func ---------------- //

        // ---------------- Change Func -------------------- //

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

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Nav NextUpdate Click
            try
            {
                var item = popupNextSureId.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    if (item.Uid != "0")
                    {
                        if (popupRelativeIdError != null) popupRelativeIdError.Content = item.Content + " Surenin " + item.Tag + " ayeti mevcut";
                    }
                }
                item = null;
            }
            catch (Exception ex)
            {
                App.logWriter("ClickFunc", ex);
            }
        }

        private void popupRelativeId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void popupRelativeId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            var item = popupNextSureId.SelectedItem as ComboBoxItem;
            if (!textbox.IsLoaded) return;

            if (popupRelativeId.Text != "" && popupRelativeId.Text != null)
            {
                if (int.Parse(popupRelativeId.Text) <= int.Parse(item.Tag.ToString()) && int.Parse(popupRelativeId.Text) > 0)
                {
                    loadVersePopup.IsEnabled = true;
                    popupRelativeIdError.Content = "Ayet Mevcut Eklene Bilir";
                }
                else
                {
                    loadVersePopup.IsEnabled = false;
                    popupRelativeIdError.Content = "Ayet Mevcut Değil";
                }
            }
            else
            {
                loadVersePopup.IsEnabled = false;
                popupRelativeIdError.Content = "Eklemek İstenilen Ayet Sırasını Giriniz";
            }
        }

        // ---------------- Change Func -------------------- //

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
                addRemider.IsEnabled = false;
                filterAll.IsEnabled = false;
                filterDatetime.IsEnabled = false;
                filterDay.IsEnabled = false;
                filterWeek.IsEnabled = false;
                filterMonth.IsEnabled = false;
                filterYears.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                addRemider.IsEnabled = true;
                filterAll.IsEnabled = true;
                filterDatetime.IsEnabled = true;
                filterDay.IsEnabled = true;
                filterWeek.IsEnabled = true;
                filterMonth.IsEnabled = true;
                filterYears.IsEnabled = true;
            });
        }

        // ------------ Animation Func ------------ //
    }
}