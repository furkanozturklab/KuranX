using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using System.Windows.Input;
using System.Windows.Media;

using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;

namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectFrame.xaml
    /// </summary>
    public partial class SubjectFrame : Page
    {
        private string searchText = "";
        private int lastPage = 0, NowPage = 1;
        private List<Subject> dSub = new List<Subject>();
        private Decimal totalcount = 0;

        public SubjectFrame()
        {
            try
            {

                App.errWrite($"[{DateTime.Now} InitializeComponent ] -> SubjectFrame");


                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} PageCall ] -> SubjectFrame");


                lastPage = 0;
                NowPage = 1;

                App.mainScreen.navigationWriter("subject", "");
                App.loadTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }

            return this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} Page_Loaded ] -> SubjectFrame");


                App.mainScreen.navigationWriter("subject", "");
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        // ------------- Loading ------------- //

        public void loadItem()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} loadItem ] -> SubjectFrame");


                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    if (this.Dispatcher.Invoke(() => searchText != ""))
                    {
                        dSub = entitydb.Subject.Where(p => EF.Functions.Like(p.subjectName, "%" + searchText + "%")).OrderByDescending(p => p.subjectId).Skip(lastPage).Take(18).ToList();
                        totalcount = entitydb.Subject.Where(p => EF.Functions.Like(p.subjectName, "%" + searchText + "%")).Count();
                    }
                    else
                    {
                        dSub = entitydb.Subject.OrderByDescending(p => p.subjectId).Skip(lastPage).Take(15).ToList();
                        totalcount = entitydb.Subject.Count();
                    }

                    for (int x = 1; x <= 18; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sbItem = (Border)FindName("sbItem" + x);
                            sbItem.Visibility = Visibility.Hidden;
                        });
                    }

                    int i = 1;

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    foreach (var item in dSub)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sColor = (Border)FindName("sbColor" + i);
                            sColor.Background = new BrushConverter().ConvertFrom((string)item.subjectColor) as SolidColorBrush;

                            var sName = (TextBlock)FindName("sbName" + i);
                            sName.Text = item.subjectName;

                            var sCreated = (TextBlock)FindName("sbCreated" + i);
                            sCreated.Text = item.created.ToString("D");

                            var sBtn = (Button)FindName("sbBtn" + i);
                            sBtn.Uid = item.subjectId.ToString();

                            var sbItem = (Border)FindName("sbItem" + i);
                            sbItem.Visibility = Visibility.Visible;
                            i++;
                        });
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        totalcountText.Tag = totalcount.ToString();

                        if (dSub.Count() != 0)
                        {
                            totalcount = Math.Ceiling(totalcount / 18);
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

        // ------------- Loading ------------- //

        // ------------- Click Func ------------- //

        private void openSubjectFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} openSubjectFolder_Click ] -> SubjectFrame");


                var btn = sender as Button;
                App.mainframe.Content = App.navSubjectFolder.PageCall(int.Parse(btn.Uid));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} SearchBtn_Click ] -> SubjectFrame");


                if (SearchData.Text.Length >= 3)
                {
                    searchText = SearchData.Text;

                    App.loadTask = Task.Run(() => loadItem());
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchText = "";
                        App.loadTask = Task.Run(() => loadItem());
                    }
                    else
                    {
                        searchErrMsgTxt.Visibility = Visibility.Visible;
                        SearchData.Focus();
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

                App.errWrite($"[{DateTime.Now} popupClosed_Click ] -> SubjectFrame");


                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);
                popuptemp.IsOpen = false;
                subjectHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;

                subjectpreviewName.Text = "Önizleme";
                subjectFolderHeader.Text = "";
                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void addfolderSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} addfolderSubject_Click ] -> SubjectFrame");

                if (subjectFolderHeader.Text.Length >= 3)
                {
                    if (subjectFolderHeader.Text.Length < 150)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Subject.Where(p => p.subjectName == CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subjectpreviewName.Text)).ToList();

                            if (dControl.Count == 0)
                            {
                                var dSubjectFolder = new Subject { subjectName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subjectpreviewName.Text), subjectColor = subjectpreviewColor.Background.ToString(), created = DateTime.Now, modify = DateTime.Now };
                                entitydb.Subject.Add(dSubjectFolder);
                                entitydb.SaveChanges();
                                App.mainScreen.succsessFunc("İşlem Başarılı", " Yeni konu başlığı başarılı bir sekilde oluşturuldu artık ayetleri ekleye bilirsiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                                subjectpreviewName.Text = "";
                                subjectFolderHeader.Text = "";
                                popup_FolderSubjectPopup.IsOpen = false;
                                dSubjectFolder = null;

                                App.loadTask = Task.Run(() => loadItem());
                            }
                            else
                            {
                                App.mainScreen.alertFunc("İşlem Başarısız", " Daha önce aynı isimde bir konu zaten mevcut lütfen konu başlığınızı kontrol ediniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                            }
                            dControl = null;
                        }
                    }
                    else
                    {
                        subjectFolderHeader.Focus();
                        subjectHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                        subjectHeaderFolderErrorMesssage.Content = "Konu başlığının çok uzun max 150 karakter olabilir";
                    }
                }
                else
                {
                    subjectFolderHeader.Focus();
                    subjectHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    subjectHeaderFolderErrorMesssage.Content = "Konu başlığının uzunluğu minimum 3 karakter olmalı";
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void subjectColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                App.errWrite($"[{DateTime.Now} subjectColorPick_Click ] -> SubjectFrame");


                CheckBox? chk;

                foreach (object item in subjectColorStack.Children)
                {
                    chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                chk = sender as CheckBox;

                chk.IsChecked = true;

                subjectpreviewColor.Background = new BrushConverter().ConvertFromString((string)chk.Tag) as SolidColorBrush;
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

                App.errWrite($"[{DateTime.Now} nextpageButton_Click ] -> SubjectFrame");
                nextpageButton.IsEnabled = false;
                lastPage += 18;
                NowPage++;
                App.loadTask = Task.Run(() => loadItem());
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
                App.errWrite($"[{DateTime.Now} previusPageButton_Click ] -> SubjectFrame");


                if (lastPage >= 18)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 18;
                    NowPage--;
                    App.loadTask = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void addSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} addSubjectButton_Click ] -> SubjectFrame");


                popup_FolderSubjectPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        // ------------- Click Func ------------- //

        // ------------- Change Func ------------- //

        private void SearchData_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                App.errWrite($"[{DateTime.Now} SearchData_TextChanged ] -> SubjectFrame");


                if (SearchData.Text.Length >= 3)
                {
                    searchErrMsgTxt.Visibility = Visibility.Hidden;
                }
                else
                {
                    searchErrMsgTxt.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void SearchData_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                searchErrMsgTxt.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void subjectFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                subjectHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void subjectFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                subjectpreviewName.Text = subjectFolderHeader.Text;
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void subjectFolderHeader_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9a-zA-Z-ğüşöçıİĞÜŞÖÇ?.*()']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        /* ---------------- Changed Func ---------------- */

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {
                App.errWrite($"[{DateTime.Now} loadAni ] -> SubjectFrame");

                this.Dispatcher.Invoke(() =>
                {
                    // loadinGifContent.Visibility = Visibility.Visible;
                    SearchBtn.IsEnabled = false;
                    addSubjectButton.IsEnabled = false;
                    loadBorder.Visibility = Visibility.Hidden;
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
                App.errWrite($"[{DateTime.Now} loadAniComplated ] -> SubjectFrame");


                this.Dispatcher.Invoke(() =>
                {
                    //  loadinGifContent.Visibility = Visibility.Collapsed;
                    loadBorder.Visibility = Visibility.Visible;
                    SearchBtn.IsEnabled = true;
                    addSubjectButton.IsEnabled = true;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //
    }
}