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

namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectFrame.xaml
    /// </summary>
    public partial class SubjectFrame : Page
    {
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private List<Subject> dSubject = new List<Subject>();
        private List<Subject> tempSubject = new List<Subject>();
        private Task PageItemLoadTask;
        private int lastSubject = 0, totalcount, NowPage = 1;
        private bool searchStatus = false;
        private string searchTxt;

        public SubjectFrame()
        {
            InitializeComponent();
        }

        public void loadSubject()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadingAni();
                    if (searchStatus) dSubject = entitydb.Subject.Where(p => EF.Functions.Like(p.SubjectName, "%" + searchTxt + "%")).Skip(lastSubject).Take(13).ToList();
                    else dSubject = entitydb.Subject.Skip(lastSubject).Take(13).ToList();

                    totalcount = entitydb.Subject.Count();

                    for (int x = 1; x < 13; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            ItemsControl itemslist = (ItemsControl)this.FindName("sb" + x);
                            //itemslist.Items.Clear();
                            itemslist.ItemsSource = null;
                        });
                    }
                    int i = 1;

                    foreach (var item in dSubject)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            tempSubject.Add(item);
                            ItemsControl itemslist = (ItemsControl)this.FindName("sb" + i);
                            itemslist.ItemsSource = tempSubject;
                            tempSubject.Clear();
                            i++;
                        });

                        if (i == 13) break; // 12 den fazla varmı kontrol etmek için koydum
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        if (lastSubject == 0) previusPageButton.IsEnabled = false;
                        else previusPageButton.IsEnabled = true;

                        if (dSubject.Count() <= 12) nextpageButton.IsEnabled = false;
                        if (lastSubject == 0 && dSubject.Count() > 12) nextpageButton.IsEnabled = true;

                        totalcountText.Tag = totalcount.ToString();

                        nowPageStatus.Tag = NowPage + " / " + Math.Ceiling(decimal.Parse(totalcount.ToString()) / 12).ToString();
                    });
                    Thread.Sleep(200);
                    loadingAniComplated();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Load", ex);
            }
        }

        private void openSubjectFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                SearchData.Text = "";
                loadinGifContent.Visibility = Visibility.Visible;
                App.mainframe.Content = new SubjectItemsFrame(int.Parse(btn.Uid), (string)btn.Content, btn.Tag.ToString(), btn.Background.ToString());
            }
            catch (Exception ex)
            {
                App.logWriter("opensubject", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Debug.WriteLine("TRİGER PAGE LOAD");
                PageItemLoadTask = new Task(loadSubject);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        // ------------ POPUP OPEN ACTİONS  ------------ //

        private void addSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addSubjectButton.IsEnabled = false;
                addFolderSubjectPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void addfolderSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (subjectFolderHeader.Text.Length >= 8)
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
                            addFolderSubjectPopup.IsOpen = false;
                            addSubjectButton.IsEnabled = true;
                            subjectFolderLoad();
                            loadSubject();
                        }
                        else
                        {
                            alertFunc("Konu Başlığı Oluşturulamadı ", " Daha önce aynı isimde bir konu zaten mevcut lütfen kontrol ediniz.", 3);
                        }
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

        // ------------  OTHER FUNC --------------- //

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);

                addSubjectButton.IsEnabled = true;
                subjectFolderHeader.Text = "";
                subjectpreviewName.Text = "";
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

        private void subjectFolderLoad()
        {
            /*
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dSubjectFolder = entitydb.Subject.ToList();

                    selectedSubjectFolder.Items.Clear();
                    foreach (var item in dSubjectFolder)
                    {
                        var cmbitem = new ComboBoxItem();

                        cmbitem.Content = item.SubjectName;
                        cmbitem.Uid = item.SubjectId.ToString();
                        selectedSubjectFolder.Items.Add(cmbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
            */
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

        private void subjectColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chk;

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

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SearchData.Text.Length >= 3)
                {
                    searchStatus = true;
                    searchTxt = SearchData.Text;

                    PageItemLoadTask = new Task(loadSubject);
                    PageItemLoadTask.Start();
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchStatus = false;
                        PageItemLoadTask = new Task(loadSubject);
                        PageItemLoadTask.Start();
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

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastSubject += 12;
                NowPage++;
                PageItemLoadTask = new Task(loadSubject);
                PageItemLoadTask.Start();
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
                if (lastSubject >= 12)
                {
                    previusPageButton.IsEnabled = false;
                    lastSubject -= 12;
                    NowPage--;
                    PageItemLoadTask = new Task(loadSubject);
                    PageItemLoadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void loadingAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    addSubjectButton.IsEnabled = false;
                    nextpageButton.IsEnabled = false;
                    previusPageButton.IsEnabled = false;
                    nextpageButton.IsEnabled = false;
                    SearchData.IsEnabled = false;
                    SearchBtn.IsEnabled = false;
                    loadinGifContent.Visibility = Visibility.Visible;

                    loadBorder.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void loadingAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    addSubjectButton.IsEnabled = true;
                    SearchData.IsEnabled = true;
                    SearchBtn.IsEnabled = true;
                    loadinGifContent.Visibility = Visibility.Collapsed;
                    loadBorderHeader.Visibility = Visibility.Visible;
                    loadBorder.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }
    }
}