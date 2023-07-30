
using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Helpers;
using KuranX.App.Core.Classes.Tools;

using System;

using System.Diagnostics;
using System.Linq;

using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using System.Windows.Input;

using System.Windows.Navigation;


namespace KuranX.App.Core.Pages.SectionF
{
    /// <summary>
    /// Interaction logic for SectionFrame.xaml
    /// </summary>
    public partial class SectionFrame : Page,Movebar
    {

        public int selectedSure = 0, last = 0, selectedSection = 0, clearNav = 1, currentP = 0, totalSection = 0, s;
        public string getLocation;
        private Task sectionframe, sectionprocess;
        private DraggablePopupHelper drag;
        private string pp_selected;
        public SectionFrame()
        {
            Tools.errWrite($"[{DateTime.Now} InitializeComponent ] -> SectionFrame");


            InitializeComponent();
        }


        public object PageCall(int sureId, int selectedSections = 1, string location = "")
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} PageCall ] -> SectionFrame");

                headerBorder.Visibility = Visibility.Hidden;
                navControlStack.Visibility = Visibility.Hidden;
                mainContent.Visibility = Visibility.Hidden;
                controlPanel.Visibility = Visibility.Hidden;

                selectedSure = sureId;
                selectedSection = selectedSections;
                getLocation = location;


                sectionframe = Task.Run(() => loadSectionFunc(selectedSection));
                App.lastlocation = "SectionFrame";

                Debug.WriteLine("return headerBorder : " + headerBorder.Visibility);
                return this;
            }
            catch (Exception ex)
            {
                Tools.logWriter("", ex);
                return this;
            }
        }


        public void loadSectionFunc(int sSelectedSection = 1)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadSectionFunc ] -> SectionFrame");

                using (var entitydb = new AyetContext())
                {
                    var sSure = entitydb.Sure.Where(p => p.sureId == selectedSure).FirstOrDefault();
                    var sSection = entitydb.Sections.Where(p => p.SureId == selectedSure && p.SectionNumber == sSelectedSection).FirstOrDefault();
                    totalSection = entitydb.Sections.Where(p => p.SureId == sSure.sureId).Count();
                    selectedSection = sSelectedSection;

                    if (sSection != null)
                    {

                        loadHeaderFunc(sSure);
                        loadNavFunc();
                        loadItemsControl(sSection);

                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            loadSectionDetail();
                        });


                        this.Dispatcher.Invoke(() =>
                        {
                            /*
                            if (totalSection == 1) NavUpdateNextSingle.IsEnabled = false;
                            else if (selectedSection < totalSection) NavUpdateNextSingle.IsEnabled = true;
                            else NavUpdateNextSingle.IsEnabled = false;

                            if (selectedSection == 1) NavUpdatePrevSingle.IsEnabled = false;
                            else NavUpdatePrevSingle.IsEnabled = true;
                            */
                            var desktype = App.navSurePage.deskingCombobox.SelectedItem as ComboBoxItem;

                            if (sSure.name == "Fâtiha" && selectedSection == 1) NavUpdatePrevSingle.IsEnabled = false;
                            else NavUpdatePrevSingle.IsEnabled = true;

                            if ((string)desktype.Tag == "DeskLanding")
                            {

                                if (sSure.name == "Tevbe" && selectedSection == 15) NavUpdateNextSingle.IsEnabled = false;
                                else NavUpdateNextSingle.IsEnabled = true;

                            }
                            else
                            {
                                if (sSure.name == "Nâs" && selectedSection == 1) NavUpdateNextSingle.IsEnabled = false;
                                else NavUpdateNextSingle.IsEnabled = true;
                            }

                        });


                        Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                        this.Dispatcher.Invoke(() =>
                        {
                            Debug.WriteLine("debug");
                            headerBorder.Visibility = Visibility.Visible;
                            navControlStack.Visibility = Visibility.Visible;
                            mainContent.Visibility = Visibility.Visible;
                            controlPanel.Visibility = Visibility.Visible;

                        });
                    }
                    else
                    {


                        Debug.WriteLine("null geliyor veriler");                        
                    }

                }

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = true);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }

        }


        public void singleItemsControl()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} singleItemsControl ] -> SectionFrame");


                using (var entitydb = new AyetContext())
                {
                    loadItemsControl(entitydb.Sections.Where(p => p.SureId == selectedSure && p.SectionNumber == selectedSection).First());
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void markButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} markButton_Click ] -> SectionFrame");


                var bchk = sender as CheckBox;
                using (var entitydb = new AyetContext())
                {
                    // Tüm Önceki işaretleri kaldı

                    var dVerse = entitydb.Sections.Where(p => p.IsMark == true).ToList();

                    foreach (var item in dVerse)
                    {
                        item.IsMark = false;
                    }


                    if (bchk.IsChecked.ToString() == "True")
                    {
                        entitydb.Sections.Where(p => p.SectionId == int.Parse(bchk.Uid)).First().IsMark = true;

                    }
                    else
                    {
                        entitydb.Sections.Where(p => p.SectionId == int.Parse(bchk.Uid)).First().IsMark = false;

                    }

                    entitydb.SaveChanges();
                    singleItemsControl();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }


        private void loadSectionDetail()
        {


            try
            {


                Tools.errWrite($"[{DateTime.Now} loadSectionDetail ] -> SectionFrame");
                using (var entitydb = new AyetContext())
                {
                    sectionScroll.ScrollToTop();

                    var sSelection = entitydb.Sections.Where(p => p.SureId == selectedSure && p.SectionNumber == selectedSection).FirstOrDefault();
                    //var sVerse = entitydb.Verse.Where(p => p.sureId == selectedSure && p.relativeDesk >= sSelection.startVerse && p.relativeDesk <= sSelection.endVerse).ToList();

                    if (sSelection != null)
                    {

                        sectionDetailBlock.Text = sSelection.SectionDetail;
                        /*
                        this.Dispatcher.Invoke(() => selectionDetailStack.Children.Clear());
                        foreach (var item in sVerse)
                        {

                            var s = new TextBlock();

                            s.Text = item.verseTr;
                            s.Style = (Style)FindResource("sectionDetail");
                            this.Dispatcher.Invoke(() => selectionDetailStack.Children.Add(s));

                        }
                        */
                    }
                }

            }
            catch (Exception ex)
            {

                Tools.logWriter("Click", ex);
            }


        }

        private void loadHeaderFunc(Sure dSure)
        {

            try
            {
                Tools.errWrite($"[{DateTime.Now} loadHeaderFunc ] -> SectionFrame");


                this.Dispatcher.Invoke(() =>
                {
                    loadHeader.Text = dSure.name;
                    loadLocation.Text = dSure.landingLocation;
                    loadDesc.Text = dSure.description;
                    loadVerseCount.Text = dSure.numberOfVerses.ToString();
                    loadDeskLanding.Text = dSure.deskLanding.ToString();
                    loadDeskMushaf.Text = dSure.deskMushaf.ToString();




                    headerBorder.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }
        private void descButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} descButton_Click ] -> SectionFrame");



                var contentb = sender as Button;
                textDesc.Text = contentb.Uid.ToString();
                popupHeaderTextDesc.Text = loadHeader.Text + " Suresinin " + selectedSection + " Bölümünün Açıklaması";
                PopupHelpers.load_drag(popup_descSectionPopup);
                popup_descSectionPopup.IsOpen = true;
                contentb = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        public void loadItemsControl(Classes.Section dSection)
        {
            // items Load Func
            try
            {
                Tools.errWrite($"[{DateTime.Now} loadItemsControl ] -> SectionFrame");


                this.Dispatcher.Invoke(() =>
                {
                    markButton.IsChecked = dSection.IsMark;
                    markButton.Uid = dSection.SectionId.ToString();
                    noteButton.Uid = dSection.SectionId.ToString();
                    descButton.Uid = dSection.SectionDescription.ToString();

                });
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }

        public void loadNavFunc(int prev = 0)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} loadNavFunc ] -> SectionFrame");


                using (var entitydb = new AyetContext())
                {
                    for (int x = 1; x < 8; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var vNav = (CheckBox)this.FindName("vb" + x);
                            vNav.Visibility = Visibility.Collapsed;
                            vNav.IsEnabled = true;
                            vNav.IsThreeState = true;
                        });
                    }

                    last = selectedSection;

                    if (last % 8 == 0)

                    {
                        last -= 2;
                    }
                    else
                    {
                        if (selectedSection <= 7) last = 0;
                        else last -= 2;
                    }

                    if (prev != 0) last -= prev;
                    else last -= 2;


                    var dSectionNav = entitydb.Sections.Where(p => p.SureId == selectedSure).Skip(last).Take(7).ToList();


                    int i = 0;

                    foreach (var item in dSectionNav)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            i++;

                            var vNav = (CheckBox)this.FindName("vb" + i);

                            if (vNav != null)
                            {
                                vNav.Visibility = Visibility.Visible;
                                vNav.Uid = item.SectionId.ToString();
                                vNav.Content = item.SectionNumber;

                                vNav.SetValue(Extensions.DataStorage, item.SectionNumber.ToString());
                            }
                        });
                    }

                    for (int x = 1; x < 8; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var vNav = (CheckBox)FindName("vb" + x);
                            if (int.Parse((string)vNav.GetValue(Extensions.DataStorage)) == selectedSection)
                            {
                                vNav.IsEnabled = false;
                                vNav.IsThreeState = false;
                            }
                        });
                    }


                    Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));




                }

                this.Dispatcher.Invoke(() =>
                {
                    navstackPanel.Visibility = Visibility.Visible;
                    controlPanel.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {

                Tools.logWriter("Loading", ex);
            }


        }

        private void activeVerseSelected_Click(object sender, EventArgs e)
        {
            // Verse Change Click
            try
            {

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = false);


                Tools.errWrite($"[{DateTime.Now} activeVerseSelected_Click ] -> SectionFrame");

                var chk = sender as CheckBox;
                if (chk.IsChecked.ToString() == "True") chk.IsChecked = false;
                else { chk.IsChecked = true; }

                for (int x = 1; x < 8; x++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        var vNav = (CheckBox)FindName("vb" + x);
                        vNav.IsEnabled = true;
                        vNav.IsThreeState = true;
                    });
                }

                clearNav = int.Parse(chk.GetValue(Extensions.DataStorage).ToString().Split('b').Last());
                currentP = int.Parse(chk.Content.ToString().Split(" ")[0]);
                navstackPanel.Visibility = Visibility.Hidden;
                controlPanel.Visibility = Visibility.Hidden;
                mainContent.Visibility = Visibility.Hidden;
                sectionframe = Task.Run(() => loadSectionFunc(currentP));

            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void NavUpdatePrevSingle_Click(object sender, EventArgs e)
        {
            // Nav PrevSingle Click
            try
            {

                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = false);
                Tools.errWrite($"[{DateTime.Now} NavUpdatePrevSingle_Click ] -> SectionFrame");

                if (clearNav != 0) clearNav--;

                navstackPanel.Visibility = Visibility.Hidden;
                controlPanel.Visibility = Visibility.Hidden;
                mainContent.Visibility = Visibility.Hidden;

                if (totalSection >= selectedSection && 1 < selectedSection)
                {
                    selectedSection--;
                    sectionframe = Task.Run(() => loadSectionFunc(selectedSection));
                }
                else
                {
                    var desktype = App.navSurePage.deskingCombobox.SelectedItem as ComboBoxItem;

                    if ((string)desktype.Tag == "DeskLanding")
                    {
                       
                        int xc = 0;
                        using (var entitydb = new AyetContext())
                        {
                            var listx = entitydb.Sure.OrderBy(p => p.deskLanding);
                            foreach (var item in listx)
                            {
                                xc++;
                                if (loadHeader.Text == item.name) break;
                            }
                            xc--;
                            var listxc = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.deskLanding == xc).FirstOrDefault();


                            var datasection = entitydb.Sections.OrderByDescending(p => p.SectionNumber).Where(p => p.SureId == listxc.sureId).FirstOrDefault();



                            headerBorder.Visibility = Visibility.Hidden;
                 
                            App.mainframe.Content = App.navSectionPage.PageCall((int)datasection.SureId, (int)datasection.SectionNumber);

                            listx = null;
                            listxc = null;

                            Debug.WriteLine("working 2");
                        }
                    }
                    else
                    {
                        
                        using (var entitydb = new AyetContext())
                        {
                            int selectedSureX = selectedSure;
                            selectedSureX--;
                            var BeforeD = entitydb.Sure.Where(p => p.sureId == selectedSureX).Select(p => new Sure() { numberOfSection = p.numberOfSection , sureId = p.sureId}).FirstOrDefault();

                            Debug.WriteLine("working 1");

                            headerBorder.Visibility = Visibility.Hidden;

                            var datasection = entitydb.Sections.OrderByDescending(p => p.SectionNumber).Where(p => p.SureId == BeforeD.sureId).FirstOrDefault();

                            App.mainframe.Content = App.navSectionPage.PageCall((int)datasection.SureId, (int)datasection.SectionNumber);

                            BeforeD = null;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void NavUpdateNextSingle_Click(object sender, EventArgs e)
        {
            // Nav NextSingle Click
            try
            {


                this.Dispatcher.Invoke(() => App.mainScreen.homescreengrid.IsEnabled = false);
                Tools.errWrite($"[{DateTime.Now} NavUpdateNextSingle_Click ] -> SectionFrame");

                if (clearNav != 8) clearNav++;
                navstackPanel.Visibility = Visibility.Hidden;
                controlPanel.Visibility = Visibility.Hidden;
                mainContent.Visibility = Visibility.Hidden;



                if (totalSection > selectedSection)
                {

                    NavUpdatePrevSingle.IsEnabled = true;
                    selectedSection++;
                    sectionframe = Task.Run(() => loadSectionFunc(selectedSection));
                }
                else
                {
                    var desktype = App.navSurePage.deskingCombobox.SelectedItem as ComboBoxItem;

                    if ((string)desktype.Tag == "DeskLanding")
                    {
                        using (var entitydb = new AyetContext())
                        {
                            int xc = 0;
                            var listx = entitydb.Sure.OrderBy(p => p.deskLanding);
                            foreach (var item in listx)
                            {
                                xc++;
                                if (loadHeader.Text == item.name) break;
                            }
                            xc++;
                            var listxc = entitydb.Sure.OrderBy(p => p.deskLanding).Where(p => p.deskLanding == xc).First();
                            headerBorder.Visibility = Visibility.Hidden;
                            App.mainframe.Content = App.navSectionPage.PageCall((int)listxc.sureId, 1);

                            listx = null;
                            listxc = null;
                        }
                    }
                    else
                    {
                        headerBorder.Visibility = Visibility.Hidden;
                        App.mainframe.Content = App.navSectionPage.PageCall(++selectedSure, 1);
                    }
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
                Tools.errWrite($"[{DateTime.Now} popupClosed_Click ] -> SectionFrame");


                var btntemp = sender as Button;
                Popup popuptemp = (Popup)FindName(btntemp!.Uid);
                PopupHelpers.popupClosed(popuptemp, pp_moveBar);


                btntemp = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }


        private void backVersesFrame_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} backVersesFrame_Click ] -> SectionFrame");

                headerBorder.Visibility = Visibility.Visible;
                controlPanel.Visibility = Visibility.Visible;
                mainContent.Visibility = Visibility.Visible;
                navControlStack.Visibility = Visibility.Visible;

                if (getLocation != "")
                {


                    NavigationService.GoBack();


                }
                else
                {
                    if (markButton.IsChecked == true)
                    {
                        if (App.beforeFrameName == "Sure")
                        {
                            App.mainframe.Content = App.navSurePage.PageCall();
                        }
                        else
                        {
                            NavigationService.GoBack();
                        }
                    }
                    else
                    {
                        PopupHelpers.load_drag(popup_fastExitConfirm);
                        popup_fastExitConfirm.IsOpen = true;
                    }
                }


            }
            catch (Exception ex)
            {
                Tools.logWriter("", ex);
            }
        }

        private void fastexitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} fastexitBtn_Click ] -> SectionFrame");


                if (App.beforeFrameName == "Sure")
                {
                    App.mainframe.Content = App.navSurePage.PageCall();
                }
                else
                {
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("", ex);
            }
        }


        private void noteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} noteButton_Click ] -> SectionFrame");
                PopupHelpers.load_drag(popup_Note);
                popup_Note.IsOpen = true;
                sectionframe = Task.Run(noteConnect);
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
                Tools.errWrite($"[{DateTime.Now} addNoteButton_Click ] -> SectionFrame");


                if (noteName.Text.Length >= 3)
                {
                    if (noteName.Text.Length <= 150)
                    {
                        if (noteDetail.Text.Length >= 3)
                        {
                            if (noteDetail.Text.Length <= 3000)
                            {
                                using (var entitydb = new AyetContext())
                                {
                                    if (entitydb.Notes.Where(p => p.noteHeader == noteName.Text && p.sureId == selectedSure && p.sectionId == selectedSection).FirstOrDefault() != null)
                                    {
                                        App.mainScreen.alertFunc("İşlem Başarısız", "Aynı isimde not eklemiş olabilirsiniz lütfen kontrol edip yeniden deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                    }
                                    else
                                    {
                                        var dNotes = new Notes { noteHeader = noteName.Text, noteDetail = noteDetail.Text, sureId = selectedSure, sectionId = selectedSection, modify = DateTime.Now, created = DateTime.Now, noteLocation = "Bölüm" };
                                        entitydb.Notes.Add(dNotes);
                                        entitydb.SaveChanges();
                                        App.mainScreen.succsessFunc("İşlem Başarılı", loadHeader.Text + " surenin " + selectedSection + " bölümüne not eklendiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                                        sectionprocess = Task.Run(noteConnect);
                                        dNotes = null;
                                    }

                                    noteName.Text = "";
                                    noteDetail.Text = "";
                                }
                                PopupHelpers.dispose_drag(popup_noteAddPopup);
                                popup_noteAddPopup.IsOpen = false;
                            }
                            else
                            {
                                noteAddPopupDetailError.Visibility = Visibility.Visible;
                                noteDetail.Focus();
                                noteAddPopupDetailError.Content = "Not İçeriği 3000 Maximum karakterden fazla olamaz.";
                            }
                        }
                        else
                        {
                            noteAddPopupDetailError.Visibility = Visibility.Visible;
                            noteDetail.Focus();
                            noteAddPopupDetailError.Content = "Not İçeriği Yeterince Uzun Değil. Min 3 Karakter Olmalıdır";
                        }
                    }
                    else
                    {
                        noteAddPopupHeaderError.Visibility = Visibility.Visible;
                        noteName.Focus();
                        noteAddPopupHeaderError.Content = "Not Başlığı Çok Uzun. Max 150 Karakter Olabilir.";
                    }
                }
                else
                {
                    noteAddPopupHeaderError.Visibility = Visibility.Visible;
                    noteName.Focus();
                    noteAddPopupHeaderError.Content = "Not Başlığı Yeterince Uzun Değil. Min 3 Karakter Olmalıdır.";
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }


        public void noteConnect()
        {
            try
            {
                Tools.errWrite($"[{DateTime.Now} noteConnect ] -> SectionFrame");

                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.sureId == selectedSure && p.sectionId == selectedSection && p.noteLocation == "Bölüm").ToList();

                    int i = 1;

                    for (int x = 1; x <= 7; x++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var sbItem = (Button)FindName("nd" + x);
                            sbItem.Visibility = Visibility.Hidden;
                        });
                    }

                    foreach (var item in dNotes)
                    {
                        if (i == 8)
                        {
                            this.Dispatcher.Invoke(() => allShowNoteButton.Visibility = Visibility.Visible);

                            break;
                        }

                        this.Dispatcher.Invoke(() =>
                        {
                            var sButton = (Button)FindName("nd" + i);
                            sButton.Content = item.noteHeader;
                            sButton.Uid = item.notesId.ToString();

                            var sbItem = (Button)FindName("nd" + i);
                            sbItem.Visibility = Visibility.Visible;
                            i++;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loading", ex);
            }
        }


        private void noteDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} noteDetailPopup_Click ] -> SectionFrame");
                var tmpbutton = sender as Button;

                PopupHelpers.dispose_drag(popup_Note);
                popup_Note.IsOpen = false;
                App.secondFrame.Visibility = Visibility.Visible;
                App.secondFrame.Content = App.navNoteItem.PageCall(int.Parse(tmpbutton.Uid), "SectionNote");

                tmpbutton = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void notesDetailPopup_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} notesDetailPopup_Click ] -> SectionFrame");
                var tmpbutton = sender as Button;
                PopupHelpers.dispose_drag(popup_Note);
                popup_Note.IsOpen = false;
                popup_notesAllShowPopup.IsOpen = false;
                App.secondFrame.Visibility = Visibility.Visible;
                App.secondFrame.Content = App.navNoteItem.PageCall(int.Parse(tmpbutton.Uid), "SectionNoteDetail");
                tmpbutton = null;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }


        private void noteAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} noteAddButton_Click ] -> SectionFrame");
                PopupHelpers.load_drag(popup_noteAddPopup);
                popup_noteAddPopup.IsOpen = true;
                noteConnectVerse.Text = loadHeader.Text + " > " + selectedSection;
                noteType.Text = "Bölüm Notu";
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }

        private void allShowNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} allShowNoteButton_Click ] -> SectionFrame");

                PopupHelpers.load_drag(popup_notesAllShowPopup);
                popup_notesAllShowPopup.IsOpen = true;
                using (var entitydb = new AyetContext())
                {
                    var dNotes = entitydb.Notes.Where(p => p.sureId == selectedSure && p.sectionId == selectedSection).ToList();
                    foreach (var item in dNotes)
                    {
                        var itemsStack = new StackPanel();
                        var headerText = new TextBlock();
                        var noteText = new TextBlock();
                        var allshowButton = new Button();
                        var sp = new Separator();

                        itemsStack.Style = (Style)FindResource("pp_dynamicItemStackpanel");
                        headerText.Style = (Style)FindResource("pp_dynamicItemTextHeader");
                        noteText.Style = (Style)FindResource("pp_dynamicItemTextNote");
                        allshowButton.Style = (Style)FindResource("pp_dynamicItemShowButton");
                        sp.Style = (Style)FindResource("pp_dynamicItemShowSperator");

                        headerText.Text = item.noteHeader.ToString();
                        noteText.Text = item.noteDetail.ToString();
                        allshowButton.Uid = item.notesId.ToString();
                        allshowButton.Content = item.noteDetail.ToString();

                        allshowButton.Click += notesDetailPopup_Click;

                        itemsStack.Children.Add(headerText);
                        itemsStack.Children.Add(noteText);
                        itemsStack.Children.Add(allshowButton);
                        itemsStack.Children.Add(sp);

                        notesAllShowPopupStackPanel.Children.Add(itemsStack);

                        itemsStack = null;
                        headerText = null;
                        noteText = null;
                        allshowButton = null;
                        sp = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
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


        private void noteName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9a-zA-Z-ğüşöçıİĞÜŞÖÇ?.*()']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }


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
