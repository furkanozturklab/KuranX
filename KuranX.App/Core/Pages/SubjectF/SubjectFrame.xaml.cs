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

using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectFrame.xaml
    /// </summary>
    public partial class SubjectFrame : Page
    {
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private Task loadTask;
        private string searchText;
        private int lastPage = 0, NowPage = 1;

        public SubjectFrame()
        {
            InitializeComponent();
        }

        public object subjectPageCall()
        {
            return this;
        }

        public void loadFolders()
        {
            using (var entitydb = new AyetContext())
            {
                List<Subject> dSub = new List<Subject>();
                Decimal totalcount = 0;

                if (searchText != "")
                {
                    dSub = entitydb.Subject.Where(p => EF.Functions.Like(p.SubjectName, "%" + searchText + "%")).Skip(lastPage).Take(15).ToList();
                    totalcount = entitydb.Subject.Where(p => EF.Functions.Like(p.SubjectName, "%" + searchText + "%")).Count();
                }
                else
                {
                    dSub = entitydb.Subject.Skip(lastPage).Take(15).ToList();
                    totalcount = entitydb.Subject.Count();
                }

                for (int x = 1; x < 16; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("sbItem" + x);
                        itemslist.ItemsSource = null;
                    });
                }
                int i = 1;
                List<Subject> tempSub = new List<Subject>();

                foreach (var item in dSub)
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

                    if (dSub.Count() != 0)
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
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadTask = new Task(() => loadFolders());
            loadTask.Start();
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

                    loadTask = new Task(() => loadFolders());
                    loadTask.Start();
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchText = "";
                        loadTask = new Task(() => loadFolders());
                        loadTask.Start();
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
                if (subjectFolderHeader.Text.Length >= 8)
                {
                    if (subjectFolderHeader.Text.Length < 50)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Subject.Where(p => p.SubjectName == subjectpreviewName.Text).ToList();

                            if (dControl.Count == 0)
                            {
                                var dSubjectFolder = new Subject { SubjectName = subjectpreviewName.Text, SubjectColor = subjectpreviewColor.Background.ToString(), Created = DateTime.Now, Modify = DateTime.Now };
                                entitydb.Subject.Add(dSubjectFolder);
                                entitydb.SaveChanges();
                                succsessFunc("Konu Başlığı ", " Yeni konu başlığı oluşturuldu artık ayetleri ekleye bilirsiniz.", 3);

                                subjectpreviewName.Text = "";
                                subjectFolderHeader.Text = "";
                                popup_FolderSubjectPopup.IsOpen = false;
                                dSubjectFolder = null;

                                loadTask = new Task(() => loadFolders());
                                loadTask.Start();
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
                    subjectHeaderFolderErrorMesssage.Content = "Konu başlığının uzunluğu minimum 8 karakter olmalı";
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
                lastPage += 15;
                NowPage++;
                loadTask = new Task(() => loadFolders());
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
                if (lastPage >= 15)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 15;
                    NowPage--;
                    loadTask = new Task(() => loadFolders());
                    loadTask.Start();
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

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    alph.IsOpen = false;
                    timeSpan.Stop();
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

                timeSpan.Interval = TimeSpan.FromSeconds(timespan);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    inph.IsOpen = false;
                    timeSpan.Stop();
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

        // ---------- MessageFunc FUNC ---------- //
    }
}