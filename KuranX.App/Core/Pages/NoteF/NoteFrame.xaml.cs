﻿using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Helpers;
using KuranX.App.Core.Classes.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;



namespace KuranX.App.Core.Pages.NoteF
{
    /// <summary>
    /// Interaction logic for NoteFrame.xaml
    /// </summary>
    ///

    public class Dstack
    {
        public int Count { get; set; }
        public string? NoteLocation { get; set; }
        public string? Bg { get; set; }
        public string? Fr { get; set; }
    }

    public partial class NoteFrame : Page, Movebar
    {
        private string searchText = "", filterText = "";
        private int lastPage = 0, NowPage = 1, selectedId;
        private bool filter;
        private List<Notes> dNotes = new List<Notes>();
        private Task noteframetask, noteprocess;
        private string pp_selected;
        public DraggablePopupHelper drag;

        public NoteFrame()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Tools.logWriter("InitializeComponent", ex);
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} PageLoaded] -> NoteFrame");


                listBorder.Visibility = Visibility.Visible;
                App.mainScreen.navigationWriter("notes", "");

            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading Func", ex);
            }
        }

        public Page PageCall(int nowpage = 1)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} PageCall] -> NoteFrame");

                if (lastPage != 0)
                {
                    lastPage = nowpage * 24;
                    lastPage -= 24;
                }
                else lastPage = 0;


                NowPage = nowpage;
                App.mainScreen.navigationWriter("notes", "");
                filter = false;
                searchText = "";
                listview.IsChecked = true;
                stackview.IsChecked = false;
                listBorder.Visibility = Visibility.Visible;
                stackBorder.Visibility = Visibility.Collapsed;

                noteframetask = Task.Run(() => loadItem());

                App.lastlocation = "NoteFrame";

                return this;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading Func", ex);
                return this;
            }
        }

        // -------------  Load Func ------------- //

        public void loadItem()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();

                    decimal totalcount = entitydb.Notes.Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı" || p.noteLocation == "Bölüm").Count();

                    App.mainScreen.navigationWriter("notes", "");

                    this.Dispatcher.Invoke(() =>
                    {
                        listBorder.Visibility = Visibility.Visible;
                        stackBorder.Visibility = Visibility.Collapsed;
                    });

                    if (filter)
                    {
                        if (searchText != "")
                        {
                            dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == filterText).OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == filterText).ToList().Count();
                        }
                        else
                        {
                            dNotes = entitydb.Notes.Where(p => p.noteLocation == filterText).OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => p.noteLocation == filterText).ToList().Count();
                        }
                    }
                    else
                    {
                        if (searchText != "")
                        {
                            dNotes = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => EF.Functions.Like(p.noteHeader, "%" + searchText + "%")).Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").ToList().Count();
                        }
                        else
                        {
                            dNotes = entitydb.Notes.Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").OrderByDescending(p => p.created).Skip(lastPage).Take(24).ToList();
                            totalcount = entitydb.Notes.Where(p => p.noteLocation == "Konularım" || p.noteLocation == "Bölüm" || p.noteLocation == "Ayet" || p.noteLocation == "Kullanıcı").ToList().Count();
                        }
                    }

                    for (int x = 1; x <= 24; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sbItem = (Border)FindName("nt" + x);
                            sbItem.Visibility = Visibility.Hidden;
                        });
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        subStat.Content = entitydb.Notes.Where(p => p.noteLocation == "Konularım").Count();

                        verseStat.Content = entitydb.Notes.Where(p => p.noteLocation == "Ayet").Count();

                        userStat.Content = entitydb.Notes.Where(p => p.noteLocation == "Kullanıcı").Count();

                        sectionStart.Content = entitydb.Notes.Where(p => p.noteLocation == "Bölüm").Count();
                    });
                    int i = 1;

                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                    foreach (var item in dNotes)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sColor = (Border)FindName("ntColor" + i);
                            switch (item.noteLocation)
                            {
                                case "Konularım":
                                    sColor.Background = new BrushConverter().ConvertFrom("#FD7E14") as SolidColorBrush;
                                    break;

                                case "Bölüm":

                                    sColor.Background = new BrushConverter().ConvertFrom("#6610F2") as SolidColorBrush;
                                    break;

                                case "Ayet":

                                    sColor.Background = new BrushConverter().ConvertFrom("#0DCAF0") as SolidColorBrush;
                                    break;

                                default:
                                    sColor.Background = new BrushConverter().ConvertFrom("#ADB5BD") as SolidColorBrush;
                                    break;
                            }

                            var sName = (TextBlock)FindName("ntName" + i);
                            sName.Text = item.noteHeader;

                            var sCreated = (TextBlock)FindName("ntCreate" + i);
                            sCreated.Text = item.created.ToString("D", new CultureInfo("tr-TR"));

                            var sBtnGo = (Button)FindName("ntBtnGo" + i);
                            sBtnGo.Uid = item.notesId.ToString();

                            var sBtnDel = (Button)FindName("ntBtnDel" + i);
                            sBtnDel.Uid = item.notesId.ToString();

                            var sbItem = (Border)FindName("nt" + i);
                            sbItem.Visibility = Visibility.Visible;

                            i++;
                        });
                    }

                    i = 1;

                    this.Dispatcher.Invoke(() =>
                    {
                        totalcountText.Tag = totalcount.ToString();

                        if (dNotes.Count() != 0)
                        {
                            totalcount = Math.Ceiling(totalcount / 24);
                            nowPageStatus.Tag = NowPage + " / " + totalcount;
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


                        App.mainScreen.homescreengrid.IsEnabled = true;
                    });
                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading Func", ex);
            }
        }

        // -------------  Load Func ------------- //

        // -------------  Click Func ------------- //

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} filter_Click] -> NoteFrame");

                var btn = sender as Button;
                listview.IsChecked = true;
                stackview.IsChecked = false;
                lastPage = 0;
                NowPage = 1;

                if ((string)btn.Content == "Hepsi")
                {
                    filter = false;
                    filterText = "";
                    searchText = "";
                    SearchData.Text = "";
                }
                else
                {
                    filter = true;
                    filterText = (string)btn.Content;
                    searchText = "";
                    SearchData.Text = "";
                }

                noteframetask = Task.Run(() => loadItem());

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
                Tools.errWrite($"[{DateTime.Now} SearchBtn_Click] -> NoteFrame");


                if (SearchData.Text.Length >= 3)
                {
                    searchText = SearchData.Text;

                    noteframetask = Task.Run(loadItem);
                }
                else
                {
                    if (SearchData.Text.Length == 0)
                    {
                        SearchData.Text = "";
                        searchErrMsgTxt.Visibility = Visibility.Hidden;
                        SearchBtn.Focus();
                        searchText = "";
                        noteframetask = Task.Run(loadItem);


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
                Tools.logWriter("SearchButton", ex);
            }
        }

        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} filterButton_Click] -> NoteFrame");


                if (hoverPopup.IsOpen)
                {
                    PopupHelpers.dispose_drag(hoverPopup);
                    hoverPopup.IsOpen = false;
                }
                else
                {
                    PopupHelpers.load_drag(hoverPopup);
                    hoverPopup.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void addNewNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} addNewNote_Click] -> NoteFrame");


               

                PopupHelpers.load_drag(popup_noteAddPopup);
                popup_noteAddPopup.IsOpen = true;
                noteType.Text = "Kullanıcı Notu";


            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void viewchange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} viewchange_Click] -> NoteFrame");


                CheckBox? chk = sender as CheckBox;
                listview.IsChecked = false;
                stackview.IsChecked = false;



                chk.IsChecked = true;

                if ((string)chk.Content == "Liste")
                {
                    listBorder.Visibility = Visibility.Visible;
                    stackBorder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    listBorder.Visibility = Visibility.Collapsed;
                    stackBorder.Visibility = Visibility.Visible;
                }

                PopupHelpers.dispose_drag(hoverPopup);
                hoverPopup.IsOpen = false;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} addNoteButton_Click] -> NoteFrame");

                if (noteName.Text.Length <= 3)
                {
                    noteAddPopupHeaderError.Visibility = Visibility.Visible;
                    noteName.Focus();
                    noteAddPopupHeaderError.Content = "Not Başlığı Yeterince Uzun Değil. Min 3 Karakter Olmalıdır.";
                }
                else
                {
                    if (noteName.Text.Length > 150)
                    {
                        noteAddPopupHeaderError.Visibility = Visibility.Visible;
                        noteName.Focus();
                        noteAddPopupHeaderError.Content = "Not Başlığı Çok Uzun. Max 150 Karakter Olabilir.";
                    }
                    else
                    {
                        if (noteDetail.Text.Length <= 3)
                        {
                            noteAddPopupDetailError.Visibility = Visibility.Visible;
                            noteDetail.Focus();
                            noteAddPopupDetailError.Content = "Not İçeriği Yeterince Uzun Değil. Min 3 Karakter Olmalıdır";
                        }
                        else
                        {
                            if (noteDetail.Text.Length >= 3000)
                            {
                                noteAddPopupDetailError.Visibility = Visibility.Visible;
                                noteDetail.Focus();
                                noteAddPopupDetailError.Content = "Not İçeriği 3000 Maximum karakterden fazla olamaz.";
                            }
                            else
                            {
                                using (var entitydb = new AyetContext())
                                {
                                    if (entitydb.Notes.Where(p => p.noteHeader == noteName.Text && p.noteLocation == "Kullanıcı").FirstOrDefault() != null)
                                    {
                                        App.mainScreen.alertFunc("İşlem Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen notu ismini kontrol edip yeniden deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                    }
                                    else
                                    {
                                        var dNotes = new Notes { noteHeader = noteName.Text, noteDetail = noteDetail.Text, sureId = 0, verseId = 0, modify = DateTime.Now, created = DateTime.Now, noteLocation = "Kullanıcı" };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        App.mainScreen.succsessFunc("İşlem Başarılı", "Notunuz başarılı bir sekilde eklenmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                        noteName.Text = "";
                                        noteDetail.Text = "";

                                        noteframetask = Task.Run(() => loadItem());

                                        dNotes = null;
                                    }

                                    noteName.Text = "";
                                    noteDetail.Text = "";
                                }
                                PopupHelpers.dispose_drag(popup_noteAddPopup);
                                popup_noteAddPopup.IsOpen = false;
                            }
                        }
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
                Tools.errWrite($"[{DateTime.Now} nextpageButton_Click] -> NoteFrame");
                nextpageButton.IsEnabled = false;
                lastPage += 24;
                NowPage++;


                noteframetask = Task.Run(loadItem);
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

                Tools.errWrite($"[{DateTime.Now} previusPageButton_Click] -> NoteFrame");


                if (lastPage >= 24)
                {
                    previusPageButton.IsEnabled = false;
                    lastPage -= 24;
                    NowPage--;

                    noteframetask = Task.Run(loadItem);
                }
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
                Tools.errWrite($"[{DateTime.Now} popupClosed_Click] -> NoteFrame");

                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp, pp_moveBar);

            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void gotoNotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} gotoNotes_Click] -> NoteFrame");


                var btn = sender as Button;
                listBorder.Visibility = Visibility.Hidden;
                App.mainframe.Content = App.navNoteItem.PageCall(int.Parse(btn.Uid), "", NowPage);

            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void deleteNotes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} deleteNotes_Click] -> NoteFrame");

                var btn = sender as Button;
                PopupHelpers.load_drag(popup_deleteNote);
                popup_deleteNote.IsOpen = true;
                selectedId = int.Parse(btn.Uid);

            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void deleteNotePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} deleteNotePopupBtn_Click] -> NoteFrame");

                using (var entitydb = new AyetContext())
                {
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.notesId == selectedId));


                    entitydb.SaveChanges();


                    PopupHelpers.dispose_drag(popup_deleteNote);
                    popup_deleteNote.IsOpen = false;
                    noteprocess = Task.Run(loadItem);

                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        // -------------  Click Func ------------- //

        // ---------------- Changed Func ---------------- //

        private void SearchData_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} SearchData_TextChanged] -> NoteFrame");
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
                Tools.logWriter("Changed", ex);
            }
        }

        private void SearchData_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} SearchData_LostFocus] -> NoteFrame");

                searchErrMsgTxt.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void noteName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                noteAddPopupHeaderError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void noteDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                noteAddPopupDetailError.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        // ---------------- Changed Func ---------------- //

        // ------------ Animation Func ------------ //

        public void loadAni()
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadAni] -> NoteFrame");

                this.Dispatcher.Invoke(() =>
                {
                    SearchBtn.IsEnabled = false;
                    addNewNote.IsEnabled = false;
                    filterButton.IsEnabled = false;
                    filterAll.IsEnabled = false;
                    filterUser.IsEnabled = false;
                    filterSubject.IsEnabled = false;
                    filterVerse.IsEnabled = false;
                    filterSection.IsEnabled = false;

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
                Tools.errWrite($"[{DateTime.Now} loadAniComplated] -> NoteFrame");

                this.Dispatcher.Invoke(() =>
                {
                    SearchBtn.IsEnabled = true;
                    addNewNote.IsEnabled = true;
                    filterButton.IsEnabled = true;
                    filterAll.IsEnabled = true;
                    filterUser.IsEnabled = true;
                    filterSubject.IsEnabled = true;

                    filterVerse.IsEnabled = true;
                    filterSection.IsEnabled = true;

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

            Tools.errWrite($"[{DateTime.Now} popuverMove_Click] -> NoteFrame");
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