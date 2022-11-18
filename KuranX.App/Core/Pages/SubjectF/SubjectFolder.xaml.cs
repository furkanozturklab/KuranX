using KuranX.App.Core.Classes;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

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
            InitializeComponent();
        }

        public object PageCall(int subId)
        {
            lastPage = 0;
            NowPage = 1;
            subFolderId = subId;
            App.loadTask = Task.Run(() => loadItem(subId));

            return this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.mainScreen.navigationWriter("subject", loadHeader.Text);
        }

        // ------------- Load Func ------------- //

        public void loadItem(int id)
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                var dSubject = entitydb.Subject.Where(p => p.SubjectId == id).FirstOrDefault();
                var dSubjects = entitydb.SubjectItems.Where(p => p.SubjectId == id).Skip(lastPage).Take(20).ToList();

                Decimal totalcount = entitydb.SubjectItems.Where(p => p.SubjectId == id).Count();

                App.mainScreen.navigationWriter("subject", dSubject.SubjectName);

                this.Dispatcher.Invoke(() =>
                {
                    loadHeader.Text = dSubject.SubjectName;
                    loadCreated.Text = dSubject.Created.ToString("D");
                    loadHeaderColor.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(dSubject.SubjectColor);
                    subjectItemsDeleteBtn.Uid = dSubject.SubjectId.ToString();
                });

                for (int x = 1; x < 21; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("sbItem" + x);
                        itemslist.ItemsSource = null;
                    });
                }

                int i = 1;
                List<SubjectItems> tempSub = new List<SubjectItems>();
                foreach (var item in dSubjects)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        tempSub.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("sbItem" + i);
                        itemslist.ItemsSource = tempSub;
                        tempSub.Clear();
                        i++;
                    });
                }

                Thread.Sleep(200);
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

        // ------------- Load Func ------------- //

        // ------------- Click Func ------------- //

        private void subjectItemsOpen_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = App.navSubjectItem.subjectItemsPageCall(int.Parse(btn.Uid), int.Parse(btn.Content.ToString()), int.Parse(btn.Tag.ToString()));
        }

        private void newConnect_Click(object sender, RoutedEventArgs e)
        {
            popup_addConnect.IsOpen = true;
        }

        private void sendResult_Click(object sender, RoutedEventArgs e)
        {
            popup_sendResult.IsOpen = true;
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);

                popupNextSureId.SelectedIndex = 0;
                popupNextVerseId.Text = "1";
                popupaddnewversecountErrortxt.Content = "Gitmek İstenilen Ayet Sırasını Giriniz";
                popuptemp.IsOpen = false;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
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
                        var dControl = entitydb.SubjectItems.Where(p => p.SureId == int.Parse(item.Uid)).Where(p => p.VerseId == int.Parse(popupNextVerseId.Text)).Where(p => p.SubjectId == subFolderId).ToList();

                        if (dControl.Count != 0)
                        {
                            alertFunc("Ayet Ekleme Başarısız", "Bu ayet Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            var dSubjectItem = new SubjectItems { SubjectId = subFolderId, SubjectNotesId = 0, SureId = int.Parse(item.Uid), VerseId = int.Parse(popupNextVerseId.Text), Created = DateTime.Now, Modify = DateTime.Now, SubjectName = item.Content + " Suresinin " + popupNextVerseId.Text + " Ayeti" };
                            entitydb.SubjectItems.Add(dSubjectItem);
                            entitydb.SaveChanges();
                            succsessFunc("Ayet Ekleme Başarılı", "Seçmiş olduğunuz konuya ayet eklendi.", 3);

                            popup_addConnect.IsOpen = false;
                            popupNextVerseId.Text = "1";
                            App.loadTask = Task.Run(() => loadItem(subFolderId));
                        }
                    }
                    else
                    {
                        alertFunc("Ayet Ekleme Başarısız", "Seçmiş olduğunuz konuya ayet eklenemedi.", 3);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
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
                    App.loadTask = Task.Run(() => loadItem(subFolderId));
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

        private void connectResultControl_Click(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                var item = popupResultSureId.SelectedItem as ComboBoxItem;
                var dResult = entitydb.Results.Where(p => p.ResultId == int.Parse(item.Uid)).FirstOrDefault();

                if (entitydb.ResultItems.Where(p => p.ResultId == dResult.ResultId && p.ResultSubjectId == subFolderId).Count() == 0)
                {
                    dResult.ResultSubject = "true";
                    var dTemp = new ResultItem { ResultId = dResult.ResultId, ResultSubjectId = subFolderId, SendTime = DateTime.Now };
                    entitydb.ResultItems.Add(dTemp);
                    entitydb.SaveChanges();
                    popup_sendResult.IsOpen = false;
                    succsessFunc("Gönderme Başarılı", "Konu başlığı " + item.Content + " suresinin sonucuna gönderildi.", 3);
                }
                else
                {
                    popup_sendResult.IsOpen = false;
                    alertFunc("Gönderme Başarısız", "Konu başlığı " + item.Content + " suresinin sonucuna daha önceden eklenmiştir yeniden ekleyemezsiniz.", 3);
                }
                /*

                */
            }
        }

        private void deleteSubjectPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.SubjectItems.RemoveRange(entitydb.SubjectItems.Where(p => p.SubjectId == subFolderId));
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.SubjectId == subFolderId));
                    entitydb.Subject.RemoveRange(entitydb.Subject.Where(p => p.SubjectId == subFolderId));
                    entitydb.SaveChanges();
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Remove", ex);
            }
        }

        // ------------- Click Func ------------- //

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

        // ---------- SelectionChanged FUNC ---------- //

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            if (!comboBox.IsLoaded) return;

            var item = popupNextSureId.SelectedItem as ComboBoxItem;

            if (item != null)
            {
                popupcomboboxLabel.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
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
                App.logWriter("Popup", ex);
            }
        }

        private void popupNextVerseId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void popupNextVerseId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var item = popupNextSureId.SelectedItem as ComboBoxItem;

            var textbox = (TextBox)sender;
            if (!textbox.IsLoaded) return;

            if (popupNextVerseId.Text != "" && popupNextVerseId.Text != null)
            {
                if (int.Parse(popupNextVerseId.Text) <= int.Parse(item.Tag.ToString()) && int.Parse(popupNextVerseId.Text) > 0)
                {
                    addnewConnectSubject.IsEnabled = true;
                    popupaddnewversecountErrortxt.Content = "Ayet Mevcut Gidilebilir";
                }
                else
                {
                    addnewConnectSubject.IsEnabled = false;
                    popupaddnewversecountErrortxt.Content = "Ayet Mevcut Değil";
                }
            }
            else
            {
                addnewConnectSubject.IsEnabled = false;
                popupaddnewversecountErrortxt.Content = "Lütfen Ayet Sırasını Giriniz";
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
                succsessFunc("Konu Silme Başarılı", "Konuya ait tüm notlar ve eklenen ayetlerle birlikte silindi. Konularıma yönlendiriliyorsunuz...", 3);
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

        // ---------- TimeSpan FUNC ---------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                subjectItemsDeleteBtn.IsEnabled = false;
                backPage.IsEnabled = false;
                sendResult.IsEnabled = false;
                newConnect.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                subjectItemsDeleteBtn.IsEnabled = true;
                backPage.IsEnabled = true;
                sendResult.IsEnabled = true;
                newConnect.IsEnabled = true;
            });
        }

        // ------------ Animation Func ------------ //
    }
}