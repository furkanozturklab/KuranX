using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for libraryFileItemsFrame.xaml
    /// </summary>
    public partial class libraryFileItemsFrame : Page
    {
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private Task? FileTask, PdfloadTask;
        private DoubleAnimation animation = new DoubleAnimation();
        private FileDialog openFileDialog = new OpenFileDialog();
        private List<PdfFile> dPdfFile = new List<PdfFile>(), tempPdfFileitems = new List<PdfFile>();
        private bool searchStatus = false;
        private string searchTxt;
        private int totalcount, NowPage = 1, lastPdfItems = 0, currentFileId;

        public libraryFileItemsFrame()
        {
            InitializeComponent();
            animation.Duration = new Duration(TimeSpan.FromSeconds(3));
        }

        public void loadPdffiles()
        {
            try
            {
                loadAni();
                using (var entitydb = new AyetContext())
                {
                    if (searchStatus) dPdfFile = entitydb.PdfFile.Where(p => EF.Functions.Like(p.FileName, "%" + searchTxt + "%") && p.FileType == "User").Skip(lastPdfItems).Take(21).ToList();
                    else dPdfFile = entitydb.PdfFile.Where(p => p.FileType == "User").Skip(lastPdfItems).Take(21).ToList();
                    totalcount = entitydb.PdfFile.Count();

                    this.Dispatcher.Invoke(() =>
                    {
                        totalFileCount.Content = totalcount + " Dosya Yüklü";

                        for (int x = 1; x < 21; x++)
                        {
                            ItemsControl itemslist = (ItemsControl)this.FindName("pdf" + x);
                            itemslist.ItemsSource = null;
                        }
                        int i = 1;
                        foreach (var item in dPdfFile)
                        {
                            tempPdfFileitems.Add(item);
                            ItemsControl itemslist = (ItemsControl)this.FindName("pdf" + i);
                            itemslist.ItemsSource = tempPdfFileitems;
                            tempPdfFileitems.Clear();
                            i++;

                            if (i == 21) break; // 12 den fazla varmı kontrol etmek için koydum

                            if (lastPdfItems == 0) previusPageButton.IsEnabled = false;
                            else previusPageButton.IsEnabled = true;

                            if (dPdfFile.Count() <= 20) nextpageButton.IsEnabled = false;
                            if (lastPdfItems == 0 && dPdfFile.Count() > 20) nextpageButton.IsEnabled = true;

                            totalcountText.Tag = totalcount.ToString();

                            nowPageStatus.Tag = NowPage + " / " + Math.Ceiling(decimal.Parse(totalcount.ToString()) / 20).ToString();
                        }
                    });
                }
                Thread.Sleep(300);
                loadAniComplated();
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void gotofilepdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                App.mainframe.Content = new libraryOpenFile(int.Parse(btn.Uid));
            }
            catch (Exception ex)
            {
                App.logWriter("ButtonClick", ex);
            }
        }

        private void deletepdffile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                currentFileId = int.Parse(btn.Uid);
                fileDeletePopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("ButtonClick", ex);
            }
        }

        private void resetFileBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dFile = entitydb.PdfFile.Where(p => p.PdfFileId == currentFileId).FirstOrDefault();

                    if (dFile != null)
                    {
                        entitydb.PdfFile.RemoveRange(entitydb.PdfFile.Where(p => p.PdfFileId == currentFileId));
                        entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.PdfFileId == currentFileId));

                        File.Delete(dFile.FileUrl);

                        entitydb.SaveChanges();
                        fileDeletePopup.IsOpen = false;
                        succsessFunc("Dosya Silme ", " Dosya başarılı bir sekilde silinmiştir.", 3);
                        PdfloadTask = new Task(loadPdffiles);
                        PdfloadTask.Start();
                    }
                    else
                    {
                        alertFunc("Dosya Silme", "Dosya mevcut değil silinemedi lütfen dosyanın varlığından emin olunuz.", 3);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("ButtonClick", ex);
            }
        }

        private void nextpageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                nextpageButton.IsEnabled = false;
                lastPdfItems += 20;
                NowPage++;
                PdfloadTask = new Task(loadPdffiles);
                PdfloadTask.Start();
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
                if (lastPdfItems >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastPdfItems -= 20;
                    NowPage--;
                    PdfloadTask = new Task(loadPdffiles);
                    PdfloadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        public void progressAni(ProgressBar prg, int start)
        {
            try
            {
                Storyboard.SetTarget(animation, prg);
                Storyboard.SetTargetProperty(animation, new PropertyPath(ProgressBar.ValueProperty));
                Storyboard sb = new Storyboard();
                sb.Children.Add(animation);
                animation.From = 0;
                animation.To = start;
                sb.Begin();
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        private void popupClear()
        {
            try
            {
                timeSpan.Interval = TimeSpan.FromSeconds(5);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    timeSpan.Stop();
                    fileUpPopup.IsOpen = false;
                    fileuploadIcontrack.Uid = "#0D6EFD";
                    fileuploadIcontrack.Tag = "ArrowRightCircle";
                    filepopupHeaderTxt.Text = "Dosya Yükleme";
                    fileuploadtrackprogress.Value = 0;
                };
            }
            catch (Exception ex)
            {
                App.logWriter("TimeSpan", ex);
            }
        }

        private void fastpopupclear()
        {
            try
            {
                fileUpPopup.IsOpen = false;
                fileuploadIcontrack.Uid = "#0D6EFD";
                fileuploadIcontrack.Tag = "ArrowRightCircle";
                filepopupHeaderTxt.Text = "Dosya Yükleme";
                fileuploadtrackprogress.Value = 0;
            }
            catch (Exception ex)
            {
                App.logWriter("ClearFunc", ex);
            }
        }

        private void filedialogtask()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    openFileDialog.Filter = "Pdf Files|*.pdf";
                    openFileDialog.CheckFileExists = true;
                    bool? response = openFileDialog.ShowDialog();

                    if (response == true)
                    {
                        fileUpPopup.IsOpen = true;

                        string fileS = string.Format("{0} {1}", (new FileInfo(openFileDialog.FileName).Length / 1.049e+6).ToString("0.0"), "Mb");
                        var newSoruceLocation = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + @"\KuranX\UploadFile\" + openFileDialog.FileName.Split(@"\").Last();

                        using (var entitydb = new AyetContext())
                        {
                            string name = openFileDialog.FileName.Split(@"\").Last();
                            var dControl = entitydb.PdfFile.Where(p => p.FileName == name).ToList();

                            if (File.Exists(newSoruceLocation) && dControl.Count != 0)
                            {
                                fileNameTxt.Text = name;
                                fileSizeTxt.Text = " Dosya Zaten Mevcut";
                                filepopupHeaderTxt.Text = "Dosya Yükleme Başarısız";
                                fileuploadIcontrack.Uid = "#F0433A";
                                fileuploadIcontrack.Tag = "XCircleFill";
                                popupClear();
                            }
                            else
                            {
                                File.Copy(openFileDialog.FileName, newSoruceLocation, true);
                                fileNameTxt.Text = name;
                                fileSizeTxt.Text = fileS;

                                var newFile = new PdfFile { FileName = openFileDialog.FileName.Split(@"\").Last(), FileUrl = newSoruceLocation, FileSize = fileS, Created = DateTime.Now, Modify = DateTime.Now };
                                entitydb.PdfFile.Add(newFile);
                                entitydb.SaveChanges();
                                progressAni((ProgressBar)this.FindName("fileuploadtrackprogress"), 100);

                                popupClear();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dosya yükleme sırasında bir hata meydana geldi.");
                    }
                });
            }
            catch (Exception ex)
            {
                App.logWriter("FileDialog", ex);
            }
        }

        private void fileuploadtrackprogress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (e.NewValue == 100)
                {
                    fileuploadIcontrack.Uid = "#23d160";
                    fileuploadIcontrack.Tag = "CheckCircleFill";
                    fileSizeTxt.Text = "Yükleme Başarılı";
                    PdfloadTask = new Task(loadPdffiles);
                    PdfloadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("FileUploadTrack", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchData.IsEnabled = false;
                SearchBtn.IsEnabled = false;
                addfile.IsEnabled = false;
                PdfloadTask = new Task(loadPdffiles);
                PdfloadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("PageLoad", ex);
            }
        }

        private void fileUploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fastpopupclear();
                FileTask = new Task(filedialogtask);
                FileTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("FileUploadEvent", ex);
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
                App.logWriter("TextClear", ex);
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
                App.logWriter("TextClear", ex);
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

                    PdfloadTask = new Task(loadPdffiles);
                    PdfloadTask.Start();
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchStatus = false;
                        PdfloadTask = new Task(loadPdffiles);
                        PdfloadTask.Start();
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

        private void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    SearchData.IsEnabled = false;
                    SearchBtn.IsEnabled = false;
                    addfile.IsEnabled = false;
                    loadinItemsGifContent.Visibility = Visibility.Visible;
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
                    SearchData.IsEnabled = true;
                    SearchBtn.IsEnabled = true;
                    addfile.IsEnabled = true;
                    backPage.IsEnabled = true;

                    loadHeaderAni.Visibility = Visibility.Visible;
                    loadControlAni.Visibility = Visibility.Visible;
                    loadDetailAni.Visibility = Visibility.Visible;
                    loadinGifContent.Visibility = Visibility.Collapsed;
                    loadinItemsGifContent.Visibility = Visibility.Collapsed;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
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
    }
}