﻿using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Helpers;
using KuranX.App.Core.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace KuranX.App.Core.Pages.ReminderF
{
    /// <summary>
    /// Interaction logic for RemiderFrame.xaml
    /// </summary>
    public partial class RemiderFrame : Page, Movebar
    {
        private bool filter, remiderType = false;
        private string filterText = "";
        private int lastPage = 0, NowPage = 1, selectedId;
        private List<Remider> dRemider = new List<Remider>();
        private Task remiderTask, remiderProcess;
        private string pp_selected;
        public DraggablePopupHelper drag;
        public RemiderFrame()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} InitializeComponent ] -> RemiderFrame");
                InitializeComponent();
                remiderDay.DisplayDateStart = DateTime.Now.AddDays(1);
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} PageCall ] -> RemiderFrame");


                lastPage = 0;
                NowPage = 1;

                sId.Text = "0";
                vId.Text = "0";

                remiderTask = Task.Run(() => loadItem());

                App.lastlocation = "RemiderFrame";

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

                Tools.errWrite($"[{DateTime.Now} Page_Loaded ] -> RemiderFrame");


                sId.Text = "0";
                vId.Text = "0";
                viewRemider.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Tools.logWriter("LoadingFunc", ex);
            }
        }

        // ---------------- Loading Func -------------------- //

        public void loadItem()
        {
            try
            {

                Debug.WriteLine(filterText);

                Tools.errWrite($"[{DateTime.Now} loadItem ] -> RemiderFrame");
                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    Decimal totalcount = entitydb.Remider.Count();

                    App.mainScreen.navigationWriter("remider", "");

                    if (filter)
                    {
                        dRemider = entitydb.Remider.Where(p => p.loopType == filterText).OrderBy(p => p.priority).Skip(lastPage).Take(6).ToList();
                        totalcount = dRemider.Count;
                    }
                    else
                    {
                        dRemider = entitydb.Remider.OrderBy(p => p.priority).Skip(lastPage).Take(6).ToList();
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

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    foreach (var item in dRemider)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sColor = (Border)FindName("cbTypeColor" + i);
                            var sColor2 = (Border)FindName("cbBtTypeColor" + i);
                            var sBtnStat = (TextBlock)FindName("cbStatus" + i);

                            switch (item.loopType)
                            {
                                case "Default":
                                    sColor.Background = new BrushConverter().ConvertFrom("#ffc107") as SolidColorBrush;
                                    sColor2.Background = new BrushConverter().ConvertFrom("#ffc107") as SolidColorBrush;
                                    sBtnStat.Text = item.remiderDate.ToString("D", new CultureInfo("tr-TR")) + " Tarihin de Hatırlatılacak";
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

                            sName.Text = item.remiderName;
                            sDetail.Text = item.remiderDetail;
                            sBtnDel.Uid = item.remiderId.ToString();
                            sBtnGo.Uid = item.remiderId.ToString();

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

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);

            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading Func", ex);
            }
        }

        // ---------------- Click Func ---------------- //

        private void newRemider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} newRemider_Click ] -> RemiderFrame");

                
                PopupHelpers.load_drag(popup_remiderAddPopup);
                popup_remiderAddPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void remiderVerseAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} remiderVerseAddButton_Click ] -> RemiderFrame");
         
                PopupHelpers.load_drag(popup_VerseSelect);
                popup_VerseSelect.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} filter_Click ] -> RemiderFrame");


                var btn = sender as Button;

                if ((string)btn.Uid == "Hepsi")
                {
                    filter = false;
                }
                else
                {
                    filter = true;
                    filterText = (string)btn.Uid.ToString() == "False" ? "Default" : (string)btn.Uid.ToString();
                }

                remiderTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} nextpageButton_Click ] -> RemiderFrame");

                nextpageButton.IsEnabled = false;
                lastPage += 6;
                NowPage++;
                remiderTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void previusPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} previusPageButton_Click ] -> RemiderFrame");


                if (lastPage >= 6)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 6;
                    NowPage--;
                    remiderTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void remiderTypeChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} remiderTypeChangeButton_Click ] -> RemiderFrame");

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
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void addRemiderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} addRemiderButton_Click ] -> RemiderFrame");


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

                                        if (sId.Text != "0")
                                        {
                                            newRemider = new Remider
                                            {
                                                connectSureId = int.Parse(sId.Text),
                                                connectVerseId = int.Parse(vId.Text),
                                                remiderDate = (DateTime)remiderDay.SelectedDate,
                                                remiderDetail = remiderDetail.Text,
                                                remiderName = remiderName.Text,
                                                create = DateTime.Now,
                                                priority = 1,
                                                lastAction = DateTime.Now,
                                                status = "Wait"
                                            };
                                            entitydb.Verse.Where(p => p.sureId == int.Parse(sId.Text) && p.relativeDesk == int.Parse(vId.Text)).FirstOrDefault().remiderCheck = true;
                                        }
                                        else newRemider = new Remider
                                        {
                                            connectSureId = 0,
                                            connectVerseId = 0,
                                            remiderDate = (DateTime)remiderDay.SelectedDate,
                                            remiderDetail = remiderDetail.Text,
                                            remiderName = remiderName.Text,
                                            create = DateTime.Now,
                                            priority = 1,
                                            lastAction = DateTime.Now,
                                            status = "Wait"
                                        };

                                        entitydb.Remider.Add(newRemider);
                                        entitydb.SaveChanges();

                                        App.mainScreen.succsessFunc("İşlem Başarılı", "Yeni bir hatırlatıcınız oluşturuldu", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                                        PopupHelpers.dispose_drag(popup_remiderAddPopup);
                                        popup_remiderAddPopup.IsOpen = false;
                                        remiderName.Text = "";
                                        remiderDetail.Text = "";
                                        remiderConnectVerse.Text = "Ayet Secilmemiş";

                                        sId.Text = "0";
                                        vId.Text = "0";

                                        remiderDay.SelectedDate = null;
                                        remiderProcess = Task.Run(() => loadItem());
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

                                        var newRemider = new Remider();

                                        if (sId.Text != "0") newRemider = new Remider
                                        {
                                            connectSureId = int.Parse(sId.Text),
                                            connectVerseId = int.Parse(vId.Text),
                                            remiderDate = new DateTime(1, 1, 1, 0, 0, 0, 0),
                                            remiderDetail = remiderDetail.Text,
                                            remiderName = remiderName.Text,
                                            create = DateTime.Now,
                                            loopType = ditem.Uid.ToString(),
                                            status = "Wait",
                                            priority = pr,
                                            lastAction = DateTime.Now
                                        };
                                        else newRemider = new Remider
                                        {
                                            connectSureId = 0,
                                            connectVerseId = 0,
                                            remiderDate = new DateTime(1, 1, 1, 0, 0, 0, 0),
                                            remiderDetail = remiderDetail.Text,
                                            remiderName = remiderName.Text,
                                            create = DateTime.Now,
                                            loopType = ditem.Uid.ToString(),
                                            status = "Wait",
                                            priority = pr,
                                            lastAction = DateTime.Now
                                        };

                                        entitydb.Remider.Add(newRemider);
                                        entitydb.SaveChanges();

                                        App.mainScreen.succsessFunc("İşlem Başarılı", "Yeni hatırlatıcınız oluşturuldu", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                                        PopupHelpers.dispose_drag(popup_remiderAddPopup);
                                        popup_remiderAddPopup.IsOpen = false;
                                        remiderName.Text = "";
                                        remiderDetail.Text = "";
                                        remiderConnectVerse.Text = "Ayet Secilmemiş";
                                        sId.Text = "0";
                                        vId.Text = "0";
                                        loopSelectedType.SelectedIndex = 0;

                                        remiderProcess = Task.Run(() => loadItem());
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
                Tools.logWriter("Click", ex);
            }
        }

        private void remiderDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} remiderDelete_Click ] -> RemiderFrame");

                var btn = sender as Button;
                selectedId = int.Parse(btn.Uid);

               
                PopupHelpers.load_drag(popup_DeleteConfirm);
                popup_DeleteConfirm.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void remiderDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} remiderDetail_Click ] -> RemiderFrame");

                var btn = sender as Button;
                viewRemider.Visibility = Visibility.Hidden;


                App.mainframe.Content = App.navRemiderItem.PageCall(int.Parse(btn.Uid.ToString()));
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
                Tools.errWrite($"[{DateTime.Now} deleteRemiderPopupBtn_Click ] -> RemiderFrame");


                using (var entitydb = new AyetContext())
                {
                    var control = entitydb.Remider.Where(p => p.connectSureId != 0 && p.remiderId == selectedId);
                    if (control.Count() >= 1)
                    {
                        entitydb.Verse.Where(p => p.sureId == control.FirstOrDefault().connectSureId && p.relativeDesk == control.FirstOrDefault().connectVerseId).First().remiderCheck = false;
                    }

                    entitydb.Remider.RemoveRange(entitydb.Remider.Where(p => p.remiderId == selectedId));
                    entitydb.Tasks.RemoveRange(entitydb.Tasks.Where(p => p.missonsId == selectedId));
                    entitydb.SaveChanges();
                    PopupHelpers.dispose_drag(popup_DeleteConfirm);
                    popup_DeleteConfirm.IsOpen = false;
                    App.mainScreen.succsessFunc("İşlem Başarılı", "Hatırlatıcı başaralı bir sekilde silinmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    remiderProcess = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void remiderAddVersePopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} remiderAddVersePopup_Click ] -> RemiderFrame");


                var item = popupNextSureId.SelectedItem as ComboBoxItem;

                using (var entitydb = new AyetContext())
                {
                    if (entitydb.Remider.Where(p => p.connectSureId == int.Parse((string)item.Uid) && p.connectVerseId == int.Parse(popupRelativeId.Text)).Count() > 0)
                    {
                        popupRelativeIdError.Content = "Bu ayete daha önceden hatırlatıcı oluşturulmuş.";
                        popupRelativeIdError.Visibility = Visibility.Visible;
                        popupRelativeId.Focus();
                    }
                    else
                    {
                        sId.Text = (string)item.Uid;
                        vId.Text = popupRelativeId.Text;
                        remiderConnectVerse.Text = (string)item.Content + " suresini " + popupRelativeId.Text + " ayeti"; ;
                        PopupHelpers.dispose_drag(popup_VerseSelect);
                        popup_VerseSelect.IsOpen = false;
                        popupNextSureId.SelectedIndex = 0;
                        popupRelativeId.Text = "1";
                    }
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
                Tools.errWrite($"[{DateTime.Now} popupClosed_Click ] -> RemiderFrame");


                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp, pp_moveBar);

                remiderName.Text = "";
                popupNextSureId.SelectedIndex = 0;
                popupRelativeId.Text = "1";
                remiderConnectVerse.Text = "Ayet Seçilmemiş";

                btntemp = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
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
                Tools.logWriter("Change", ex);
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
                Tools.logWriter("Change", ex);
            }
        }

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Nav NextUpdate Click
            try
            {

                Tools.errWrite($"[{DateTime.Now} popupNextSureId_SelectionChanged ] -> RemiderFrame");


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
                Tools.logWriter("Change", ex);
            }
        }

        private void popupRelativeId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {


                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void remiderName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {



                Regex regex = new Regex("[^0-9a-zA-Z-ğüşöçıİĞÜŞÖÇ']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void popupRelativeId_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        // ---------------- Change Func -------------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadAni ] -> RemiderFrame");

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
            catch (Exception ex)
            {
                Tools.logWriter("Animation", ex);
            }
        }

        public void loadAniComplated()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadAniComplated ] -> RemiderFrame");
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
            catch (Exception ex)
            {
                Tools.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //

        // ----------- Popuper Spec Func ----------- //

        public void popuverMove_Click(object sender, RoutedEventArgs e)
        {

            Tools.errWrite($"[{DateTime.Now} popuverMove_Click] -> NoteFrame");
            var btn = sender as Button;
            pp_selected = (string)btn.Uid;
            moveBarController.HeaderText = btn.Content.ToString()!;
            pp_moveBar.IsOpen = true;

        }



        public Popup getPopupMove()
        {
            return pp_moveBar;
        }

        public Popup getPopupBase()
        {

            return (Popup)FindName(pp_selected);
        }

    }
}