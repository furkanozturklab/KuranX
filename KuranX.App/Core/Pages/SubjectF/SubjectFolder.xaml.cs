using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Helpers;
using KuranX.App.Core.Classes.Tools;

using System;
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

using System.Windows.Navigation;

namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectFolder.xaml
    /// </summary>
    public partial class SubjectFolder : Page, Movebar
    {
        private int subFolderId = 1, lastPage = 0, NowPage = 1;
        private Task subjectfoldertask, subjectprocess;
        private string pp_selected;
        public DraggablePopupHelper drag;

        public SubjectFolder()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} InitializeComponent ] -> SubjectFolder");


                InitializeComponent();
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        public object PageCall(int subId)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} PageCall ] -> SubjectFolder");


                lastPage = 0;
                NowPage = 1;
                subFolderId = subId;
                loadHeaderGif.Visibility = Visibility.Visible;


                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);


                subjectfoldertask = Task.Run(() => loadItem(subId));
                App.lastlocation = "SubjectFolder";
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

                Tools.errWrite($"[{DateTime.Now} Page_Loaded ] -> SubjectFolder");


                App.mainScreen.navigationWriter("subject", loadHeader.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        // ------------- Load Func ------------- //

        public void loadItem(int id)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadItem ] -> SubjectFolder");


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
                        loadCreated.Text = dSubject.created.ToString("D", new CultureInfo("tr-TR"));
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
                            sCreated.Text = item.created.ToString("D", new CultureInfo("tr-TR"));

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
                Tools.logWriter("Loading", ex);
            }
        }

        // ------------- Load Func ------------- //

        // ------------- Click Func ------------- //

        private void subjectItemsOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} subjectItemsOpen_Click ] -> SubjectFolder");


                var btn = sender as Button;


                App.mainframe.Content = App.navSubjectItem.PageCall(int.Parse(btn.Uid), int.Parse((string)btn.Content), int.Parse((string)btn.Tag));
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void newConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} newConnect_Click ] -> SubjectFolder");


                var item = popupNextSureId.SelectedItem as ComboBoxItem;

                popupaddnewversecountErrortxt.Content = item.Content + " Süresini " + item.Tag + " Ayeti Mevcut";
                PopupHelpers.load_drag(popup_addConnect);
                popup_addConnect.IsOpen = true;
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
                Tools.errWrite($"[{DateTime.Now} popupClosed_Click ] -> SubjectFolder");

                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp, pp_moveBar);

                popupNextSureId.SelectedIndex = 0;
                popupNextVerseId.Text = "1";
                popupNewName.Text = "";

            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void addnewConnectSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} addnewConnectSubject_Click ] -> SubjectFolder");


                using (var entitydb = new AyetContext())
                {
                    var item = popupNextSureId.SelectedItem as ComboBoxItem;

                    if (item != null)
                    {
                        var dControl = entitydb.SubjectItems.Where(p => p.sureId == int.Parse(item.Uid)).Where(p => p.verseId == int.Parse(popupNextVerseId.Text)).Where(p => p.subjectId == subFolderId).ToList();

                        
                        if (dControl.Count != 0)
                        {
                            App.mainScreen.alertFunc("İşlem Başarısız", "Bu ayet daha önceden eklenmiş yeniden ekleme işlemleri yapılamaz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                            PopupHelpers.dispose_drag(popup_addConnect);
                            popup_addConnect.IsOpen = false;
                            popupNextVerseId.Text = "1";
                        }
                        else
                        {
                            var dSubjectItem = new SubjectItems { subjectId = subFolderId, subjectNotesId = 0, sureId = int.Parse(item.Uid), verseId = int.Parse(popupNextVerseId.Text), created = DateTime.Now, modify = DateTime.Now, subjectName = item.Content + " Suresinin " + popupNextVerseId.Text + " Ayeti" };
                            entitydb.SubjectItems.Add(dSubjectItem);
                            entitydb.SaveChanges();
                            App.mainScreen.succsessFunc("İşlem Başarılı", "Seçmiş olduğunuz konuya ayet eklendi.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                            PopupHelpers.dispose_drag(popup_addConnect);
                            popup_addConnect.IsOpen = false;
                            popupNextVerseId.Text = "1";
                            subjectprocess = Task.Run(() => loadItem(subFolderId));
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
                Tools.logWriter("Click", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} nextpageButton_Click ] -> SubjectFolder");


                nextpageButton.IsEnabled = false;
                lastPage += 20;
                NowPage++;
                subjectfoldertask = Task.Run(() => loadItem(subFolderId));
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
                Tools.errWrite($"[{DateTime.Now} previusPageButton_Click ] -> SubjectFolder");

                if (lastPage >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 20;
                    NowPage--;
                    subjectfoldertask = Task.Run(() => loadItem(subFolderId));
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void backPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} backPage_Click ] -> SubjectFolder");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }



        private void deleteSubjectPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} deleteSubjectPopupBtn_Click ] -> SubjectFolder");


                using (var entitydb = new AyetContext())
                {
                    entitydb.SubjectItems.RemoveRange(entitydb.SubjectItems.Where(p => p.subjectId == subFolderId));
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.subjectId == subFolderId));
                    entitydb.Subject.RemoveRange(entitydb.Subject.Where(p => p.subjectId == subFolderId));





                    entitydb.SaveChanges();
                    PopupHelpers.dispose_drag(popup_SubjectDelete);
                    popup_SubjectDelete.IsOpen = false;
                    App.mainScreen.succsessFunc("İşlem Başarılı", "Konuya ait tüm notlar ve eklenen ayetlerle birlikte silindi. Bir önceki sayfaya yönlendiriliyorsunuz bekleyin...", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    App.mainframe.Content = App.navSubjectFrame.PageCall();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void newNamePopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} newNamePopup_Click ] -> SubjectFolder");


                using (var entitydb = new AyetContext())
                {
                    if (popupNewName.Text.Length >= 3)
                    {
                        entitydb.Subject.Where(p => p.subjectId == subFolderId).First().subjectName = popupNewName.Text;
                        loadHeader.Text = popupNewName.Text;
                        entitydb.SaveChanges();
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Konu başlığınız başarılı bir sekilde değiştirilmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        PopupHelpers.dispose_drag(popup_newName);
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
                Tools.logWriter("Click", ex);
            }
        }

        private void subjectItemsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} subjectItemsDeleteBtn_Click ] -> SubjectFolder");

                PopupHelpers.load_drag(popup_SubjectDelete);
                popup_SubjectDelete.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void subjectItemsRenameBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} subjectItemsRenameBtn_Click ] -> SubjectFolder");

                popupNewName.Text = loadHeader.Text;
                PopupHelpers.load_drag(popup_newName);
                popup_newName.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        // ------------- Click Func ------------- //

        // ---------- SelectionChanged FUNC ---------- //

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} popupNextSureId_SelectionChanged ] -> SubjectFolder");


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
                Tools.logWriter("Change", ex);
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
                Tools.logWriter("Change", ex);
            }
        }

        private void popupNextVerseId_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} popupNextVerseId_TextChanged ] -> SubjectFolder");


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
                Tools.logWriter("Change", ex);
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
                Tools.logWriter("Change", ex);
            }
        }

        // ---------- SelectionChanced FUNC ---------- //
        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadAni ] -> SubjectFolder");


                this.Dispatcher.Invoke(() =>
                {
                    subjectItemsDeleteBtn.IsEnabled = false;
                    backPage.IsEnabled = false;

                    newConnect.IsEnabled = false;
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

                Tools.errWrite($"[{DateTime.Now} loadAniComplated ] -> SubjectFolder");


                this.Dispatcher.Invoke(() =>
                {
                    loadHeaderGif.Visibility = Visibility.Collapsed;
                    loadStack.Visibility = Visibility.Visible;
                    subjectItemsDeleteBtn.IsEnabled = true;
                    backPage.IsEnabled = true;

                    newConnect.IsEnabled = true;
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
            Tools.errWrite($"[{DateTime.Now} popuverMove_Click ] -> SubjectFolder");


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