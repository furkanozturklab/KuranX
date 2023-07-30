using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Helpers;
using KuranX.App.Core.Classes.Tools;
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
        private Task subjectframe, subjectprocess;
        private string pp_selected;
        public DraggablePopupHelper drag;


        public SubjectFrame()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} InitializeComponent ] -> SubjectFrame");

                InitializeComponent();
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} PageCall ] -> SubjectFrame");


                lastPage = 0;
                NowPage = 1;


                App.mainScreen.navigationWriter("subject", "");
                subjectframe = Task.Run(() => loadItem());
                App.lastlocation = "SubjectFrame";
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
                Tools.errWrite($"[{DateTime.Now} Page_Loaded ] -> SubjectFrame");


                App.mainScreen.navigationWriter("subject", "");
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        // ------------- Loading ------------- //

        public void loadItem()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadItem ] -> SubjectFrame");


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
                            sCreated.Text = item.created.ToString("D", new CultureInfo("tr-TR"));

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


                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        // ------------- Loading ------------- //

        // ------------- Click Func ------------- //

        private void openSubjectFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} openSubjectFolder_Click ] -> SubjectFrame");


                var btn = sender as Button;

                App.mainframe.Content = App.navSubjectFolder.PageCall(int.Parse(btn.Uid));
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} SearchBtn_Click ] -> SubjectFrame");


                if (SearchData.Text.Length >= 3)
                {
                    searchText = SearchData.Text;

                    subjectframe = Task.Run(() => loadItem());
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchText = "";
                        subjectframe = Task.Run(() => loadItem());
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
                Tools.logWriter("Click", ex);
            }
        }

        public void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} popupClosed_Click ] -> SubjectFrame");



                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp);
                btntemp = null;


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

                Tools.errWrite($"[{DateTime.Now} nextpageButton_Click ] -> SubjectFrame");
                nextpageButton.IsEnabled = false;
                lastPage += 18;
                NowPage++;
                subjectframe = Task.Run(() => loadItem());
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
                Tools.errWrite($"[{DateTime.Now} previusPageButton_Click ] -> SubjectFrame");


                if (lastPage >= 18)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 18;
                    NowPage--;
                    subjectframe = Task.Run(() => loadItem());
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void addSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} addSubjectButton_Click ] -> SubjectFrame");

                PopupHelpers.load_drag(popup_FolderSubjectPopup);
                popup_FolderSubjectPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        // ------------- Click Func ------------- //

        // ------------- Change Func ------------- //

        private void SearchData_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} SearchData_TextChanged ] -> SubjectFrame");


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
                Tools.logWriter("Change", ex);
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
                Tools.logWriter("Change", ex);
            }
        }





        /* ---------------- Changed Func ---------------- */

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadAni ] -> SubjectFrame");

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
                Tools.logWriter("Animation", ex);
            }
        }

        public void loadAniComplated()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadAniComplated ] -> SubjectFrame");


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
                Tools.logWriter("Animation", ex);
            }
        }

        // ------------ Animation Func ------------ //



    }
}