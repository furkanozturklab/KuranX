using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
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
    /// Interaction logic for LibraryFileFrame.xaml
    /// </summary>
    public partial class LibraryFileFrame : Page
    {
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        private Task loadTask;
        private string searchText;
        private int lastPage = 0, NowPage = 1;

        private DoubleAnimation animation = new DoubleAnimation();
        private FileDialog openFileDialog = new OpenFileDialog();

        public LibraryFileFrame()
        {
            InitializeComponent();
        }

        public Page PageCall()
        {
            loadTask = new Task(loadItem);
            loadTask.Start();
            return this;
        }

        public void loadItem()
        {
            using (var entitydb = new AyetContext())
            {
                List<PdfFile> dPdf = new List<PdfFile>();
                Decimal totalcount = 0;

                if (searchText != "")
                {
                    dPdf = entitydb.PdfFile.Where(p => EF.Functions.Like(p.FileName, "%" + searchText + "%")).Skip(lastPage).Take(20).ToList();
                    totalcount = entitydb.PdfFile.Where(p => EF.Functions.Like(p.FileName, "%" + searchText + "%")).Count();
                }
                else
                {
                    dPdf = entitydb.PdfFile.Skip(lastPage).Take(20).ToList();
                    totalcount = entitydb.PdfFile.Count();
                }

                for (int x = 1; x < 21; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ItemsControl itemslist = (ItemsControl)this.FindName("pdf" + x);
                        itemslist.ItemsSource = null;
                    });
                }
                int i = 1;
                List<PdfFile> tempPdf = new List<PdfFile>();

                foreach (var item in dPdf)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        tempPdf.Add(item);
                        ItemsControl itemslist = (ItemsControl)this.FindName("pdf" + i);
                        itemslist.ItemsSource = tempPdf;
                        tempPdf.Clear();
                        i++;
                    });
                }

                Thread.Sleep(200);

                this.Dispatcher.Invoke(() =>
                {
                    totalcountText.Tag = totalcount.ToString();

                    if (dPdf.Count() != 0)
                    {
                        // totalFileCount.Content = totalcount + " Adet Dosya Gösteriliyor";
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
                        popup_fileUp.IsOpen = true;

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
                        alertFunc("Yükleme Hatası", "Dosya yükleme işlemi sırasında hata oluştu", 3);
                    }
                });
            }
            catch (Exception ex)
            {
                App.logWriter("FileDialog", ex);
            }
        }

        // ---------------- Click Func ---------------- //

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SearchData.Text.Length >= 3)
                {
                    searchText = SearchData.Text;

                    loadTask = new Task(() => loadItem());
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
                        loadTask = new Task(() => loadItem());
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

        private void gotofilepdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                PdfViewer dViewer = new PdfViewer(int.Parse(btn.Uid));
                dViewer.Show();
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
                //    currentFileId = int.Parse(btn.Uid);
                //   fileDeletePopup.IsOpen = true;
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
                lastPage += 20;
                NowPage++;
                loadTask = new Task(loadItem);
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
                if (lastPage >= 20)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 20;
                    NowPage--;
                    loadTask = new Task(loadItem);
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("LoadEvent", ex);
            }
        }

        private void addfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fastpopupclear();
                loadTask = new Task(filedialogtask);
                loadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("FileUploadEvent", ex);
            }
        }

        // ---------------- Click Func ---------------- //

        // ---------------- Changed Func ---------------- //

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

        private void fileuploadtrackprogress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (e.NewValue == 100)
                {
                    fileuploadIcontrack.Uid = "#23d160";
                    fileuploadIcontrack.Tag = "CheckCircleFill";
                    fileSizeTxt.Text = "Yükleme Başarılı";
                    loadTask = new Task(loadItem);
                    loadTask.Start();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("FileUploadTrack", ex);
            }
        }

        // ---------------- Changed Func ---------------- //

        // ---------------- SimpleClear Func ---------------- //
        private void popupClear()
        {
            try
            {
                timeSpan.Interval = TimeSpan.FromSeconds(5);
                timeSpan.Start();
                timeSpan.Tick += delegate
                {
                    timeSpan.Stop();
                    popup_fileUp.IsOpen = false;
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
                popup_fileUp.IsOpen = false;
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

        // ---------------- SimpleClear Func ---------------- //

        // ---------------- Animations Func ---------------- //
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

        // ---------------- Animations Func ---------------- //

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

        // ---------- MessageFunc FUNC ---------- //
    }
}