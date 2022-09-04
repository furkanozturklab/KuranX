using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace KuranX.App.Core.Pages
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

        public SubjectFrame()
        {
            InitializeComponent();
        }

        public void loadSubject()
        {
            using (var entitydb = new AyetContext())
            {
                dSubject = entitydb.Subject.Skip(lastSubject).Take(13).ToList();
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

                previusPageButton.Dispatcher.Invoke(() =>
                {
                    if (lastSubject == 0) previusPageButton.IsEnabled = false;
                    else previusPageButton.IsEnabled = true;
                });

                nextpageButton.Dispatcher.Invoke(() =>
                {
                    if (dSubject.Count() <= 12) nextpageButton.IsEnabled = false;
                    if (lastSubject == 0 && dSubject.Count() > 12) nextpageButton.IsEnabled = true;
                });

                totalcountText.Dispatcher.Invoke(() =>
                {
                    totalcountText.Tag = totalcount.ToString();
                });

                nowPageStatus.Dispatcher.Invoke(() =>
                {
                    nowPageStatus.Tag = NowPage + " / " + Math.Ceiling(decimal.Parse(totalcount.ToString()) / 12).ToString();
                });
            }
        }

        private void openSubjectFolder_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            App.mainframe.Content = new SubjectItemsFrame(int.Parse(btn.Uid), btn.Content.ToString(), btn.Tag.ToString());
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PageItemLoadTask = new Task(loadSubject);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
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
    }
}