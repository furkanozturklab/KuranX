using KuranX.App.Core.Classes;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectFolder.xaml
    /// </summary>
    public partial class SubjectFolder : Page
    {
        private int subFolderId = 1, lastPage = 0, NowPage = 1;

        public SubjectFolder()
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

        public object PageCall(int subId)
        {
            try
            {
                lastPage = 0;
                NowPage = 1;
                subFolderId = subId;
                loadHeaderGif.Visibility = Visibility.Visible;
                App.loadTask = Task.Run(() => loadItem(subId));

                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
                return this;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainScreen.navigationWriter("subject", loadHeader.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        // ------------- Load Func ------------- //

        public void loadItem(int id)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    var dSubject = entitydb.Subject.Where(p => p.subjectId == id).FirstOrDefault();
                    var dSubjects = entitydb.SubjectItems.Where(p => p.subjectId == id).Skip(lastPage).Take(20).ToList();

                    Decimal totalcount = entitydb.SubjectItems.Where(p => p.subjectId == id).Count();

                    App.mainScreen.navigationWriter("subject", dSubject.subjectName);

                    this.Dispatcher.Invoke(() =>
                    {
                        loadHeader.Text = dSubject.subjectName;
                        loadCreated.Text = dSubject.created.ToString("D");
                        loadHeaderColor.Background = new BrushConverter().ConvertFrom(dSubject.subjectColor) as SolidColorBrush;
                        subjectItemsDeleteBtn.Uid = dSubject.subjectId.ToString();
                    });

                    for (int x = 1; x <= 20; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sbItem = (StackPanel)FindName("sbItem" + x);
                            sbItem.Visibility = Visibility.Hidden;
                        });
                    }

                    int i = 1;

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    foreach (var item in dSubjects)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sName = (TextBlock)FindName("sbName" + i);
                            sName.Text = item.subjectName;

                            var sCreated = (TextBlock)FindName("sbCreate" + i);
                            sCreated.Text = item.created.ToString("D");

                            var sBtn = (Button)FindName("sbBtn" + i);
                            sBtn.Uid = item.subjectId.ToString();
                            sBtn.Content = item.sureId.ToString();
                            sBtn.Tag = item.verseId.ToString();

                            var sbItem = (StackPanel)FindName("sbItem" + i);
                            sbItem.Visibility = Visibility.Visible;
                            i++;
                        });
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        totalcountText.Tag = totalcount.ToString();

                        if (dSubjects.Count() != 0)
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
                App.logWriter("Loading", ex);
            }
        }

        // ------------- Load Func ------------- //

        // ------------- Click Func ------------- //

        private void subjectItemsOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                App.mainframe.Content = App.navSubjectItem.subjectItemsPageCall(int.Parse(btn.Uid), int.Parse((string)btn.Content), int.Parse((string)btn.Tag));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void newConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = popupNextSureId.SelectedItem as ComboBoxItem;

                popupaddnewversecountErrortxt.Content = item.Content + " Süresini " + item.Tag + " Ayeti Mevcut";

                popup_addConnect.IsOpen = true;
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

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp.Uid);

                popupNextSureId.SelectedIndex = 0;
                popupNextVerseId.Text = "1";
                popupNewName.Text = "";

                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void addnewConnectSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var item = popupNextSureId.SelectedItem as ComboBoxItem;

                    if (item != null)
                    {
                        var dControl = entitydb.SubjectItems.Where(p => p.sureId == int.Parse(item.Uid)).Where(p => p.verseId == int.Parse(popupNextVerseId.Text)).Where(p => p.subjectId == subFolderId).ToList();

                        if (dControl.Count != 0)
                        {
                            App.mainScreen.alertFunc("İşlem Başarısız", "Bu ayet daha önceden eklenmiş yeniden ekleme işlemleri yapılamaz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                            popup_addConnect.IsOpen = false;
                            popupNextVerseId.Text = "1";
                        }
                        else
                        {
                            var dSubjectItem = new SubjectItems { subjectId = subFolderId, subjectNotesId = 0, sureId = int.Parse(item.Uid), verseId = int.Parse(popupNextVerseId.Text), created = DateTime.Now, modify = DateTime.Now, subjectName = item.Content + " Suresinin " + popupNextVerseId.Text + " Ayeti" };
                            entitydb.SubjectItems.Add(dSubjectItem);
                            entitydb.SaveChanges();
                            App.mainScreen.succsessFunc("İşlem Başarılı", "Seçmiş olduğunuz konuya ayet eklendi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                            popup_addConnect.IsOpen = false;
                            popupNextVerseId.Text = "1";
                            App.loadTask = Task.Run(() => loadItem(subFolderId));
                        }
                    }
                    else
                    {
                        App.mainScreen.alertFunc("İşlem Başarısız", "Seçmiş olduğunuz konuya ayet eklenemedi. Lütfen tekrar deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                }
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
                App.loadTask = Task.Run(() => loadItem(subFolderId));
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
                    App.loadTask = Task.Run(() => loadItem(subFolderId));
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

        private void connectResultControl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var item = popupResultSureId.SelectedItem as ComboBoxItem;
                    var dResult = entitydb.Results.Where(p => p.resultId == int.Parse(item.Uid)).FirstOrDefault();

                    if (entitydb.ResultItems.Where(p => p.resultId == dResult.resultId && p.resultSubjectId == subFolderId).Count() == 0)
                    {
                        dResult.resultSubject = true;
                        var dTemp = new ResultItem { resultId = dResult.resultId, resultSubjectId = subFolderId, sendTime = DateTime.Now };
                        entitydb.ResultItems.Add(dTemp);
                        entitydb.SaveChanges();
                        popup_sendResult.IsOpen = false;
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Konu başlığı " + item.Content + " suresinin sonucuna gönderildi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                    else
                    {
                        popup_sendResult.IsOpen = false;
                        App.mainScreen.alertFunc("İşlem Başarısız", "Konu başlığı " + item.Content + " suresinin sonucuna daha önceden eklenmiştir ve yeniden ekleyemezsiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void deleteSubjectPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.SubjectItems.RemoveRange(entitydb.SubjectItems.Where(p => p.subjectId == subFolderId));
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.subjectId == subFolderId));
                    entitydb.Subject.RemoveRange(entitydb.Subject.Where(p => p.subjectId == subFolderId));
                    entitydb.SaveChanges();
                    popup_SubjectDelete.IsOpen = false;
                    voidgobacktimer();
                }
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
                        entitydb.Subject.Where(p => p.subjectId == subFolderId).First().subjectName = popupNewName.Text;
                        loadHeader.Text = popupNewName.Text;
                        entitydb.SaveChanges();
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Konu başlığınız başarılı bir sekilde değiştirilmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        popup_newName.IsOpen = false;
                    }
                    else
                    {
                        popupNewName.Focus();
                        popupRelativeIdError.Content = "Konu başlığının uzunluğu minimum 3 karakter olmalı";
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void subjectItemsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_SubjectDelete.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void subjectItemsRenameBtn_Click(object sender, RoutedEventArgs e)
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

        // ------------- Click Func ------------- //

        // ---------- SelectionChanged FUNC ---------- //

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var comboBox = (ComboBox)sender;
                if (!comboBox.IsLoaded) return;

                var item = popupNextSureId.SelectedItem as ComboBoxItem;

                if (item != null)
                {
                    popupcomboboxLabel.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void popupNextVerseId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void popupNextVerseId_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var item = popupNextSureId.SelectedItem as ComboBoxItem;

                var textbox = (TextBox)sender;
                if (!textbox.IsLoaded) return;

                if (popupNextVerseId.Text != "" && popupNextVerseId.Text != null)
                {
                    if (int.Parse(popupNextVerseId.Text) <= int.Parse((string)item.Tag) && int.Parse(popupNextVerseId.Text) > 0)
                    {
                        addnewConnectSubject.IsEnabled = true;
                        popupaddnewversecountErrortxt.Content = "Ayet Mevcut Eklene Bilir";
                    }
                    else
                    {
                        addnewConnectSubject.IsEnabled = false;
                        popupaddnewversecountErrortxt.Content = "Ayet Mevcut Değil üst sınır " + item.Tag;
                    }
                }
                else
                {
                    addnewConnectSubject.IsEnabled = false;
                    popupaddnewversecountErrortxt.Content = "Lütfen Ayet Sırasını Giriniz";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

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

        // ---------- SelectionChanced FUNC ---------- //

        // ---------- TimeSpan FUNC ---------- //

        private void voidgobacktimer()
        {
            try
            {
                backPage.IsEnabled = false;
                newConnect.IsEnabled = false;
                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;
                sendResult.IsEnabled = false;

                App.timeSpan.Interval = TimeSpan.FromSeconds(3);
                App.timeSpan.Start();
                App.mainScreen.succsessFunc("İşlem Başarılı", "Konuya ait tüm notlar ve eklenen ayetlerle birlikte silindi. Konularıma yönlendiriliyorsunuz...", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                App.timeSpan.Tick += delegate
                {
                    App.timeSpan.Stop();
                    App.mainframe.Content = App.navSubjectFrame.PageCall();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("TimeSpan", ex);
            }
        }

        // ---------- TimeSpan FUNC ---------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    subjectItemsDeleteBtn.IsEnabled = false;
                    backPage.IsEnabled = false;
                    sendResult.IsEnabled = false;
                    newConnect.IsEnabled = false;
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
                    loadHeaderGif.Visibility = Visibility.Collapsed;
                    loadStack.Visibility = Visibility.Visible;
                    subjectItemsDeleteBtn.IsEnabled = true;
                    backPage.IsEnabled = true;
                    sendResult.IsEnabled = true;
                    newConnect.IsEnabled = true;
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