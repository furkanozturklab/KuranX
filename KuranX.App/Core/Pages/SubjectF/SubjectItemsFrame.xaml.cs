using KuranX.App.Core.Classes;
using Org.BouncyCastle.Crypto.Generators;
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
using Ubiety.Dns.Core.Records;

namespace KuranX.App.Core.Pages.SubjectF
{
    /// <summary>
    /// Interaction logic for SubjectItemsFrame.xaml
    /// </summary>
    public partial class SubjectItemsFrame : Page
    {
        private int loadSubjectId, lastSubjectItems = 0, totalcount, NowPage = 1;
        private List<SubjectItems> dsubjectItems = new List<SubjectItems>();
        private List<SubjectItems> tempsubjectitems = new List<SubjectItems>();
        private Task PageItemLoadTask;
        private string subjectFolder, dcreate;
        private SolidColorBrush dbg;
        private bool tempcheck = false;
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public SubjectItemsFrame()
        {
            InitializeComponent();
        }

        public SubjectItemsFrame(int subjectId, string subjectName, string create, string bgcolor) : this()
        {
            loadHeader.Text = subjectName;
            loadCreated.Text = create;
            loadSubjectId = subjectId;
            subjectFolder = subjectName;
            dcreate = create;
            dbg = (SolidColorBrush)new BrushConverter().ConvertFrom(bgcolor);
            loadHeaderColor.Background = dbg;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PageItemLoadTask = new Task(loadSubjectItems);
                PageItemLoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void loadSubjectItems()
        {
            try
            {
                loadAni();
                using (var entity = new AyetContext())
                {
                    dsubjectItems = entity.SubjectItems.Where(p => p.SubjectId == loadSubjectId).Skip(lastSubjectItems).Take(21).ToList();
                    totalcount = entity.SubjectItems.Where(p => p.SubjectId == loadSubjectId).Count();
                    for (int x = 1; x < 21; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            ItemsControl itemslist = (ItemsControl)this.FindName("sbi" + x);
                            //itemslist.Items.Clear();
                            itemslist.ItemsSource = null;
                        });
                    }
                    int i = 1;

                    foreach (var item in dsubjectItems)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            tempsubjectitems.Add(item);
                            ItemsControl itemslist = (ItemsControl)this.FindName("sbi" + i);
                            itemslist.ItemsSource = tempsubjectitems;
                            tempsubjectitems.Clear();
                            i++;
                        });

                        if (i == 21) break; // 12 den fazla varmı kontrol etmek için koydum
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        if (lastSubjectItems == 0) previusPageButton.IsEnabled = false;
                        else previusPageButton.IsEnabled = true;

                        if (dsubjectItems.Count() <= 20) nextpageButton.IsEnabled = false;
                        if (lastSubjectItems == 0 && dsubjectItems.Count() > 20) nextpageButton.IsEnabled = true;

                        totalcountText.Tag = totalcount.ToString();

                        nowPageStatus.Tag = NowPage + " / " + Math.Ceiling(decimal.Parse(totalcount.ToString()) / 20).ToString();
                    });
                }

                Thread.Sleep(200);
                loadAniComplated();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void newConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = popupNextSureId.SelectedItem as ComboBoxItem;
                popupcomboboxLabel.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
                loadChangeVerseFramePopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Other", ex);
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

        private void subjectItemsOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                App.mainframe.Content = new SubjectItemFrame(int.Parse(btn.Uid.ToString()), btn.Content.ToString(), subjectFolder, dcreate, dbg);

                loadBorder.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Frame", ex);
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
                        var dControl = entitydb.SubjectItems.Where(p => p.SureId == int.Parse(item.Uid)).Where(p => p.VerseId == int.Parse(popupNextVerseId.Text)).Where(p => p.SubjectId == loadSubjectId).ToList();

                        if (dControl.Count != 0)
                        {
                            alertFunc("Ayet Ekleme Başarısız", "Bu ayet Daha Önceden Eklenmiş Yeniden Ekleyemezsiniz.", 3);
                        }
                        else
                        {
                            var dSubjectItem = new SubjectItems { SubjectId = loadSubjectId, SubjectNotesId = 0, SureId = int.Parse(item.Uid), VerseId = int.Parse(popupNextVerseId.Text), Created = DateTime.Now, Modify = DateTime.Now, SubjectName = item.Content + " Suresinin " + popupNextVerseId.Text + " Ayeti" };
                            entitydb.SubjectItems.Add(dSubjectItem);
                            entitydb.SaveChanges();
                            succsessFunc("Ayet Ekleme Başarılı", "Seçmiş olduğunuz konuya ayet eklendi.", 3);

                            loadChangeVerseFramePopup.IsOpen = false;
                            popupNextVerseId.Text = "1";
                            PageItemLoadTask = new Task(loadSubjectItems);
                            PageItemLoadTask.Start();
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
                lastSubjectItems += 20;
                NowPage++;
                PageItemLoadTask = new Task(loadSubjectItems);
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
                if (lastSubjectItems >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastSubjectItems -= 20;
                    NowPage--;
                    PageItemLoadTask = new Task(loadSubjectItems);
                    PageItemLoadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void popupNextSureId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tempcheck)
                {
                    var item = popupNextSureId.SelectedItem as ComboBoxItem;

                    popupcomboboxLabel.Text = $"Secilebilecek Ayet Sayısı {item.Tag}";
                }
                else tempcheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("PopupAction", ex);
            }
        }

        private void subjectItemsDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                deleteSubjectpopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Popup", ex);
            }
        }

        private void deleteSubjectPopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.SubjectItems.RemoveRange(entitydb.SubjectItems.Where(p => p.SubjectId == loadSubjectId));
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.SubjectId == loadSubjectId));
                    entitydb.Subject.RemoveRange(entitydb.Subject.Where(p => p.SubjectId == loadSubjectId));
                    entitydb.SaveChanges();
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Remove", ex);
            }
        }

        private void voidgobacktimer()
        {
            try
            {
                backPage.IsEnabled = false;
                newConnect.IsEnabled = false;
                previusPageButton.IsEnabled = false;
                nextpageButton.IsEnabled = false;

                timeSpan.Interval = TimeSpan.FromSeconds(3);
                timeSpan.Start();
                succsessFunc("Konu Silme Başarılı", "Konuya ait tüm notlar ve eklenen ayetlerle birlikte silindi. Konularıma yönlendiriliyorsunuz...", 3);
                timeSpan.Tick += delegate
                {
                    timeSpan.Stop();
                    NavigationService.GoBack();
                };
            }
            catch (Exception ex)
            {
                App.logWriter("TimeSpan", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btntemp = sender as Button;
                var popuptemp = (Popup)this.FindName(btntemp.Uid);

                popuptemp.IsOpen = false;
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

        private void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    backPage.IsEnabled = false;
                    newConnect.IsEnabled = false;
                    previusPageButton.IsEnabled = false;
                    nextpageButton.IsEnabled = false;
                    loadinGifContent.Visibility = Visibility.Visible;
                    loadBorder.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    backPage.IsEnabled = true;
                    newConnect.IsEnabled = true;
                    loadinGifContent.Visibility = Visibility.Collapsed;
                    loadBorder.Visibility = Visibility.Visible;
                    loadHeaderBorder.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }
    }
}