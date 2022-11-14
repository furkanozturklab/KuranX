using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ResultF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for resultScreen.xaml
    /// </summary>
    public partial class resultScreen : Window
    {
        private int selectedResultId = 1;
        private bool tempCheck = false;
        private Result dResul = new Result();

        public resultScreen()
        {
            InitializeComponent();
        }

        public resultScreen(int id) : this()
        {
            selectedResultId = id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var entitydb = new AyetContext())
            {
                loadAni();
                dResul = entitydb.Results.Where(p => p.ResultId == selectedResultId).FirstOrDefault();

                this.Title = dResul.ResultName + " Sonucu";
                screenHeader.Text = dResul.ResultName;
                resultNoteDetail.Text = dResul.ResultFinallyNote;

                if (dResul.ResultNotes == "true")
                {
                    var cmdef = new ComboBoxItem();
                    noteico.IsEnabled = true;

                    noteItems.Items.Clear();
                    cmdef.Content = "Not Başlığını Seçiniz";
                    cmdef.Uid = "0";
                    noteItems.Items.Add(cmdef);
                    noteItems.SelectedIndex = 0;

                    var dlistNotes = entitydb.ResultItems.Where(p => p.ResultId == selectedResultId && p.ResultNoteId != 0).ToList();

                    foreach (var item in dlistNotes)
                    {
                        var rItem = entitydb.Notes.Where(p => p.NotesId == item.ResultNoteId).FirstOrDefault();
                        var cmbitem = new ComboBoxItem();
                        cmbitem.Content = rItem.NoteHeader;
                        cmbitem.Uid = rItem.NotesId.ToString();
                        noteItems.Items.Add(cmbitem);
                    }
                }

                if (dResul.ResultSubject == "true")
                {
                    subico.IsEnabled = true;

                    subjectItems.Items.Clear();
                    subjectItemsNot.Items.Clear();

                    var cmdef5 = new ComboBoxItem();
                    cmdef5.Content = "Konu İçeriğini Seçiniz";
                    cmdef5.Uid = "0";
                    subjectItems.Items.Add(cmdef5);
                    subjectItems.SelectedIndex = 0;

                    var cmdef6 = new ComboBoxItem();
                    cmdef6.Content = "Konu Ayetini Seçiniz";
                    cmdef6.Uid = "0";
                    subjectItemsNot.Items.Add(cmdef6);
                    subjectItemsNot.SelectedIndex = 0;

                    var dListSubject = entitydb.ResultItems.Where(p => p.ResultId == selectedResultId && p.ResultSubjectId != 0).ToList();

                    foreach (var item in dListSubject)
                    {
                        var cmbitem = new ComboBoxItem();

                        var rItem = entitydb.Subject.Where(p => p.SubjectId == item.ResultSubjectId).FirstOrDefault();
                        cmbitem.Content = rItem.SubjectName;
                        cmbitem.Uid = rItem.SubjectId.ToString();
                        subjectBase.Items.Add(cmbitem);
                    }
                }

                if (dResul.ResultLib == "true")
                {
                    var cmdef = new ComboBoxItem();
                    libico.IsEnabled = true;

                    libraryItems.Items.Clear();
                    cmdef.Content = "Kütüphane İçeriğini Seçiniz";
                    cmdef.Uid = "0";
                    libraryItems.Items.Add(cmdef);
                    libraryItems.SelectedIndex = 0;

                    var dListLibrary = entitydb.ResultItems.Where(p => p.ResultId == selectedResultId && p.ResultLibId != 0).ToList();

                    foreach (var item in dListLibrary)
                    {
                        var cmbitem = new ComboBoxItem();

                        var rItem = entitydb.Librarys.Where(p => p.LibraryId == item.ResultLibId).FirstOrDefault();
                        cmbitem.Content = rItem.LibraryName;
                        cmbitem.Uid = rItem.LibraryId.ToString();
                        libraryBase.Items.Add(cmbitem);
                    }
                }
                loadAniComplated();
            }
        }

        private void closedbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void noteItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = noteItems.SelectedItem as ComboBoxItem;

            if (item != null)
            {
                if (item.Uid != "0")
                {
                    using (var entitydb = new AyetContext())
                    {
                        var dNotes = entitydb.Notes.Where(p => p.NotesId == int.Parse(item.Uid)).FirstOrDefault();
                        noteNameHeader.Text = dNotes.NoteHeader;
                        noteNameSubHeader.Text = "Notlarım / " + dNotes.NoteLocation + " Notu";
                        noteNoteDetail.Text = dNotes.NoteDetail;
                    }
                }
                else
                {
                    Debug.WriteLine("İşlem Yapma 0 Geldi");
                }
            }
        }

        private void subjectBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = subjectBase.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                if (item.Uid != "0")
                {
                    using (var entitydb = new AyetContext())
                    {
                        subjectItems.Items.Clear();
                        subjectItemsNot.Items.Clear();

                        var cmdef5 = new ComboBoxItem();
                        cmdef5.Content = "Konu İçeriğini Seçiniz";
                        cmdef5.Uid = "0";
                        subjectItems.Items.Add(cmdef5);
                        subjectItems.SelectedIndex = 0;

                        var cmdef6 = new ComboBoxItem();
                        cmdef6.Content = "Konu Ayetini Seçiniz";
                        cmdef6.Uid = "0";
                        subjectItemsNot.Items.Add(cmdef6);
                        subjectItemsNot.SelectedIndex = 0;

                        var dSubjectItems = entitydb.SubjectItems.Where(p => p.SubjectId == int.Parse(item.Uid)).ToList();
                        foreach (var subitem in dSubjectItems)
                        {
                            var cmbitem = new ComboBoxItem();

                            cmbitem.Content = subitem.SubjectName;
                            cmbitem.Uid = subitem.SubjectItemsId.ToString();
                            subjectItems.Items.Add(cmbitem);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("İşlem Yapma 0 Geldi");
                }
            }
        }

        private void subjectItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = subjectItems.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                if (item.Uid != "0")
                {
                    subjectItemsNot.Items.Clear();

                    var cmdef3 = new ComboBoxItem();
                    cmdef3.Content = "Konu Ayetini Seçiniz";
                    cmdef3.Uid = "0";
                    subjectItemsNot.Items.Add(cmdef3);
                    subjectItemsNot.SelectedIndex = 0;

                    using (var entitydb = new AyetContext())
                    {
                        var dNotesItems = entitydb.Notes.Where(p => p.SubjectId == int.Parse(item.Uid)).ToList();

                        foreach (var subitem in dNotesItems)
                        {
                            var cmbitem = new ComboBoxItem();

                            cmbitem.Content = subitem.NoteHeader;
                            cmbitem.Uid = subitem.NotesId.ToString();
                            subjectItemsNot.Items.Add(cmbitem);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("İşlem Yapma 0 Geldi");
                }
            }
        }

        private void subjectItemsNot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = subjectItemsNot.SelectedItem as ComboBoxItem;
            var subbase = subjectBase.SelectedItem as ComboBoxItem;
            var subheader = subjectItems.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                if (item.Uid != "0")
                {
                    using (var entitydb = new AyetContext())
                    {
                        var dNoteItem = entitydb.Notes.Where(p => p.NotesId == int.Parse(item.Uid)).FirstOrDefault();

                        subNameHeader.Text = dNoteItem.NoteHeader;
                        subNameSubHeader.Text = "Konularım / " + subbase.Content + " / " + subheader.Content;
                        subNoteDetail.Text = dNoteItem.NoteDetail;
                    }
                }
                else
                {
                    Debug.WriteLine("İşlem Yapma 0 Geldi");
                }
            }
        }

        private void libraryBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = libraryBase.SelectedItem as ComboBoxItem;

            if (item != null)
            {
                if (item.Uid != "0")
                {
                    var cmdef = new ComboBoxItem();

                    libraryItems.Items.Clear();
                    cmdef.Content = "Kütüphane İçeriğini Seçiniz";
                    cmdef.Uid = "0";
                    libraryItems.Items.Add(cmdef);
                    libraryItems.SelectedIndex = 0;

                    using (var entitydb = new AyetContext())
                    {
                        var dNotesItems = entitydb.Notes.Where(p => p.LibraryId == int.Parse(item.Uid)).ToList();

                        foreach (var subitem in dNotesItems)
                        {
                            var cmbitem = new ComboBoxItem();

                            cmbitem.Content = subitem.NoteHeader;
                            cmbitem.Uid = subitem.NotesId.ToString();
                            libraryItems.Items.Add(cmbitem);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("İşlem Yapma 0 Geldi");
                }
            }
        }

        private void libraryItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = libraryItems.SelectedItem as ComboBoxItem;
            var sublib = libraryBase.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                if (item.Uid != "0")
                {
                    using (var entitydb = new AyetContext())
                    {
                        var dNotes = entitydb.Notes.Where(p => p.NotesId == int.Parse(item.Uid)).FirstOrDefault();
                        libNameHeader.Text = dNotes.NoteHeader;
                        libNameSubHeader.Text = "Kütüphane / " + sublib.Content + " / " + dNotes.NoteLocation + " Notu";
                        libNoteDetail.Text = dNotes.NoteDetail;
                    }
                }
                else
                {
                    Debug.WriteLine("İşlem Yapma 0 Geldi");
                }
            }
        }

        public void loadAni()
        {
            this.Dispatcher.Invoke(() =>
            {
                closedbtn.IsEnabled = false;
                printbtn.IsEnabled = false;
            });
        }

        public void loadAniComplated()
        {
            this.Dispatcher.Invoke(() =>
            {
                closedbtn.IsEnabled = true;
                printbtn.IsEnabled = true;
            });
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dResul.ResultFinallyNote = resultNoteDetail.Text;
                    entitydb.Results.Update(dResul);
                    entitydb.SaveChanges();
                    savebtn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("saveButton_Click", ex);
            }
        }

        private void noteDetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tempCheck)
                {
                    savebtn.IsEnabled = true;
                }
                else tempCheck = true;
            }
            catch (Exception ex)
            {
                App.logWriter("noteDetail_TextChanged", ex);
            }
        }

        public void Printer_Func(FrameworkElement element)
        {
            try
            {
                if (File.Exists("print_previw.xps") == true) File.Delete("print_previw.xps");
                XpsDocument doc = new XpsDocument("print_previw.xps", FileAccess.ReadWrite);

                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                SerializerWriterCollator output_Document = writer.CreateVisualsCollator();
                output_Document.BeginBatchWrite();
                output_Document.Write(App.navResultPrinter.PageCall(selectedResultId));
                output_Document.EndBatchWrite();

                FixedDocumentSequence preview = doc.GetFixedDocumentSequence();
                doc.Close();

                var dframe = new Frame();
                var dpage = new ResultPrinter();
                var window = new Window();
                dpage.Content = new DocumentViewer { Document = preview };
                dframe.Content = dpage;

                window.Content = dframe;

                window.ShowDialog();

                writer = null;
                output_Document = null;
                doc = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Printer_Func", ex);
            }
        }

        private void printbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Printer_Func(this);
            }
            catch (Exception ex)
            {
                App.logWriter("printButton_Click", ex);
            }
        }
    }
}