using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.VerseF;
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

namespace KuranX.App.Core.Pages.LibraryF
{
    /// <summary>
    /// Interaction logic for libraryNote.xaml
    /// </summary>
    public partial class libraryNote : Page
    {
        public bool tempCheck = false;
        public int currentNoteId;
        public Notes dNotes = new Notes();
        public Task pageLoad;
        private DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);

        public libraryNote()
        {
            InitializeComponent();
        }

        public libraryNote(int noteId) : this()
        {
            currentNoteId = noteId;
        }

        public void noteLoad()
        {
            try
            {
                loadAni();
                using (var entitydb = new AyetContext())
                {
                    dNotes = entitydb.Notes.Where(p => p.NotesId == currentNoteId).Where(p => p.NoteLocation == "Kütüphane").FirstOrDefault();

                    this.Dispatcher.Invoke(() =>
                    {
                        if (dNotes != null)
                        {
                            LibHeader.Text = dNotes.NoteLibHeader;
                            header.Text = dNotes.NoteHeader;
                            create.Text = dNotes.Created.ToString();
                            noteDetail.Text = dNotes.NoteDetail;
                            tempCheck = true;
                            saveButton.IsEnabled = false;

                            if (dNotes.SureId == 0) gotoVerseButton.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            MessageBox.Show("Empty");
                        }
                    });
                }
                Thread.Sleep(200);
                loadAniComplated();
            }
            catch (Exception ex)
            {
                App.logWriter("Load", ex);
            }
        }

        private void noteDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tempCheck)
                {
                    saveButton.IsEnabled = true;
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("ChangeText", ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                pageLoad = new Task(noteLoad);
                pageLoad.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("Load", ex);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dNotes.NoteDetail = noteDetail.Text;
                    entitydb.Notes.Update(dNotes);
                    entitydb.SaveChanges();
                    saveButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Save", ex);
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

        private void deleteNotePopupBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    entitydb.Notes.RemoveRange(entitydb.Notes.Where(p => p.NotesId == currentNoteId));
                    entitydb.SaveChanges();
                    voidgobacktimer();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Remove", ex);
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

        private void voidgobacktimer()
        {
            try
            {
                timeSpan.Interval = TimeSpan.FromSeconds(3);
                timeSpan.Start();
                succsessFunc("Not Silme Başarılı", "Not başarılı bir sekilde silindi. Kütüphane Notlarıma yönlendiriliyorsunuz...", 3);
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

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                deleteNotepopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Popup", ex);
            }
        }

        private void gotoBackButton_Click(object sender, RoutedEventArgs e)
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

        private void gotoVerseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new verseFrame((int)dNotes.SureId, (int)dNotes.VerseId, "LibNote");
            }
            catch (Exception ex)
            {
                App.logWriter("Navigation", ex);
            }
        }

        public void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Visible;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }

        public void loadAniComplated()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadinGifContent.Visibility = Visibility.Collapsed;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }
    }
}