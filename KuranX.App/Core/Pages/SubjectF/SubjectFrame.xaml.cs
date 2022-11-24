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
        private string searchText;
        private int lastPage = 0, NowPage = 1;
        private List<Subject> dSub = new List<Subject>();
        private Decimal totalcount = 0;

        public SubjectFrame()
        {
            InitializeComponent();
        }

        public Page PageCall()
        {
            lastPage = 0;
            NowPage = 1;
            App.mainScreen.navigationWriter("subject", "");
            App.loadTask = Task.Run(() => loadItem());

            return this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.mainScreen.navigationWriter("subject", "");
        }

        public void loadItem()
        {
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

                Thread.Sleep(300);

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

        /* ---------------- Click Func ---------------- */

        private void openSubjectFolder_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = App.navSubjectFolder.PageCall(int.Parse(btn.Uid));
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                App.logWriter("SearchButton", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                App.logWriter("Other", ex);
            }
        }

        private void addfolderSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (subjectFolderHeader.Text.Length >= 3)
                {
                    if (subjectFolderHeader.Text.Length < 50)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Subject.Where(p => p.subjectName == subjectpreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dSubjectFolder = new Subject { subjectName = subjectpreviewName.Text, subjectColor = subjectpreviewColor.Background.ToString(), created = DateTime.Now, modify = DateTime.Now };
                                entitydb.Subject.Add(dSubjectFolder);
                                entitydb.SaveChanges();
                                succsessFunc("Konu Başlığı ", " Yeni konu başlığı oluşturuldu artık ayetleri ekleye bilirsiniz.", 3);

                                subjectpreviewName.Text = "";
                                subjectFolderHeader.Text = "";
                                popup_FolderSubjectPopup.IsOpen = false;
                                dSubjectFolder = null;

                                App.loadTask = Task.Run(() => loadItem());
                            }
                            else
                            {
                                alertFunc("Konu Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                            }
                            dControl = null;
                        }
                    }
                    else
                    {
                        subjectFolderHeader.Focus();
                        subjectHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                        subjectHeaderFolderErrorMesssage.Content = "Konu başlığının çok uzun max 50 karakter olabilir";
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
                App.logWriter("PopupAction", ex);
            }
        }

        private void subjectColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

                subjectpreviewColor.Background = new BrushConverter().ConvertFromString(chk.Tag.ToString()) as SolidColorBrush;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPage += 18;
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
                App.logWriter("LoadEvent", ex);
            }
        }

        /* ---------------- Click Func ---------------- */

        /* ---------------- Popup Show Func ---------------- */

        private void addSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            popup_FolderSubjectPopup.IsOpen = true;
        }

        /* ---------------- Popup Show Func ---------------- */

        /* ---------------- Click Func ---------------- */

        /* ---------------- Changed Func ---------------- */

        private void SearchData_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
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
                App.logWriter("Search", ex);
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
                App.logWriter("Search", ex);
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
                App.logWriter("Other", ex);
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
                App.logWriter("Other", ex);
            }
        }

        /* ---------------- Changed Func ---------------- */

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
                // loadinGifContent.Visibility = Visibility.Visible;
                SearchBtn.IsEnabled = false;
                addSubjectButton.IsEnabled = false;
                loadBorder.Visibility = Visibility.Hidden;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                //  loadinGifContent.Visibility = Visibility.Collapsed;
                loadBorder.Visibility = Visibility.Visible;
                SearchBtn.IsEnabled = true;
                addSubjectButton.IsEnabled = true;
            });
        }

        // ------------ Animation Func ------------ //
    }
}