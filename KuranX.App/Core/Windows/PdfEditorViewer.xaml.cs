using CefSharp;
using CefSharp.DevTools.DOM;
using CefSharp.Wpf;
using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.VerseF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfEditorViewer : Window
    {
        public string currentfileUrl, currentFileName;
        public int currentFileId, tempVersId, tempSureId;
        private ChromiumWebBrowser ch;
        private Task? LoadTask;

        public PdfEditorViewer()
        {
            InitializeComponent();
        }

        public PdfEditorViewer(string fileUrl, string fileName, int fileId) : this()
        {
            currentfileUrl = fileUrl;
            currentFileName = fileName;
            currentFileId = fileId;

            ch = new ChromiumWebBrowser();
            ch.Style = (Style)FindResource("chromium");
            ch.Address = fileUrl;
            chromiumBase.Children.Add(ch);

            loadHeader.Text = fileName;
        }

        public void loadNotes()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dInteg = entitydb.Notes.Where(p => p.PdfFileId == currentFileId).ToList();

                    if (dInteg != null)
                    {
                        foreach (var item in dInteg)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                Button allshowButton = new Button();
                                allshowButton.Style = (Style)FindResource("pdfWindowItemsButton");
                                allshowButton.Uid = item.NotesId.ToString();
                                allshowButton.Content = item.PdfPageId + ".Sayfa";
                                allshowButton.Click += noteOpen_Click;
                                allshowButton.Tag = "FilePdf";
                                editorWindowDetailPdf.Children.Add(allshowButton);
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Load", ex);
            }
        }

        private void noteOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NoteListGird.Visibility = Visibility.Collapsed;
                NoteDetail.Visibility = Visibility.Visible;
                chromiumBase.Children.Clear();
                ch.Dispose();
                ch = new ChromiumWebBrowser();
                ch.Style = (Style)FindResource("chromium");
                using (var entitydb = new AyetContext())
                {
                    Button btn = sender as Button;
                    var dNote = entitydb.Notes.Where(p => p.NotesId == int.Parse(btn.Uid)).FirstOrDefault();

                    if (dNote != null)
                    {
                        var dPdf = entitydb.PdfFile.Where(p => p.PdfFileId == dNote.PdfFileId).FirstOrDefault();
                        string url = dPdf.FileUrl + "#page=" + dNote.PdfPageId;
                        ch.Address = url;

                        Header.Text = dNote.NoteHeader;
                        Note.Text = dNote.NoteDetail;

                        if (dNote.SureId != 0)
                        {
                            var dSure = entitydb.Sure.Where(p => p.sureId == dNote.SureId).FirstOrDefault();
                            ConnectNameTxt.Text = dSure.Name + " suresini " + dNote.VerseId + " inci ayeti ile bağlantı mevcut";
                            connectSure.Visibility = Visibility.Visible;
                            tempVersId = (int)dNote.VerseId;
                            tempSureId = (int)dNote.SureId;
                        }

                        chromiumBase.Children.Add(ch);
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("NoteOpen", ex);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window ne = new homeScreen();
                ne.Show();
            }
            catch (Exception ex)
            {
                App.logWriter("NewWindow", ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadTask = new Task(loadNotes);
                LoadTask.Start();
            }
            catch (Exception ex)
            {
                App.logWriter("Load", ex);
            }
        }

        private void gotoSureBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainframe.Content = new verseFrame(tempSureId, tempVersId, "LibEditor");
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                App.logWriter("Frame", ex);
            }
        }

        private void backp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NoteListGird.Visibility = Visibility.Visible;
                NoteDetail.Visibility = Visibility.Collapsed;
                ch.Dispose();
                ch = new ChromiumWebBrowser();
                ch.Style = (Style)FindResource("chromium");
                ch.Address = currentfileUrl;
                chromiumBase.Children.Clear();
                chromiumBase.Children.Add(ch);
            }
            catch (Exception ex)
            {
                App.logWriter("BackBtn", ex);
            }
        }
    }
}