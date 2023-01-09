using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ResultF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        public resultScreen(int id) : this()
        {
            try
            {
                selectedResultId = id;
                App.loadTask = Task.Run(() => loadItem());
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        public void Printer_Func(FrameworkElement element)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (File.Exists("print_previw.xps") == true) File.Delete("print_previw.xps");

                    XpsDocument? doc = new XpsDocument("print_previw.xps", FileAccess.ReadWrite);

                    XpsDocumentWriter? writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    SerializerWriterCollator? output_Document = writer.CreateVisualsCollator();

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
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        private void loadItem()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    loadAni();
                    dResul = entitydb.Results.Where(p => p.resultId == selectedResultId).FirstOrDefault();

                    this.Dispatcher.Invoke(() =>
                    {
                        this.Title = dResul.resultName + " Sonucu";
                        screenHeader.Text = dResul.resultName;
                        resultNoteDetail.Text = dResul.resultFinallyNote;
                    });

                    if (dResul.resultNotes == true)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var cmdef = new ComboBoxItem();
                            noteico.IsEnabled = true;

                            noteItems.Items.Clear();
                            cmdef.Content = "Not Başlığını Seçiniz";
                            cmdef.Uid = "0";
                            noteItems.Items.Add(cmdef);
                            noteItems.SelectedIndex = 0;

                            var dlistNotes = entitydb.ResultItems.Where(p => p.resultId == selectedResultId && p.resultNoteId != 0).ToList();

                            foreach (var item in dlistNotes)
                            {
                                var rItem = entitydb.Notes.Where(p => p.notesId == item.resultNoteId).FirstOrDefault();
                                var cmbitem = new ComboBoxItem();
                                cmbitem.Content = rItem.noteHeader;
                                cmbitem.Uid = rItem.notesId.ToString();
                                noteItems.Items.Add(cmbitem);
                            }
                        });
                    }

                    if (dResul.resultSubject == true)
                    {
                        this.Dispatcher.Invoke(() =>
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

                            var dListSubject = entitydb.ResultItems.Where(p => p.resultId == selectedResultId && p.resultSubjectId != 0).ToList();

                            foreach (var item in dListSubject)
                            {
                                var cmbitem = new ComboBoxItem();

                                var rItem = entitydb.Subject.Where(p => p.subjectId == item.resultSubjectId).FirstOrDefault();
                                cmbitem.Content = rItem.subjectName;
                                cmbitem.Uid = rItem.subjectId.ToString();
                                subjectBase.Items.Add(cmbitem);
                            }
                        });
                    }

                    if (dResul.resultLib == true)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            var cmdef = new ComboBoxItem();
                            libico.IsEnabled = true;

                            libraryItems.Items.Clear();
                            cmdef.Content = "Kütüphane İçeriğini Seçiniz";
                            cmdef.Uid = "0";
                            libraryItems.Items.Add(cmdef);
                            libraryItems.SelectedIndex = 0;

                            var dListLibrary = entitydb.ResultItems.Where(p => p.resultId == selectedResultId && p.resultLibId != 0).ToList();

                            foreach (var item in dListLibrary)
                            {
                                var cmbitem = new ComboBoxItem();

                                var rItem = entitydb.Librarys.Where(p => p.libraryId == item.resultLibId).FirstOrDefault();
                                cmbitem.Content = rItem.libraryName;
                                cmbitem.Uid = rItem.libraryId.ToString();
                                libraryBase.Items.Add(cmbitem);
                            }
                        });
                    }
                    loadAniComplated();
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }

        private void closedbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void noteItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = noteItems.SelectedItem as ComboBoxItem;

                if (item != null)
                {
                    if (item.Uid != "0")
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dNotes = entitydb.Notes.Where(p => p.notesId == int.Parse(item.Uid)).FirstOrDefault();
                            noteNameHeader.Text = dNotes.noteHeader;
                            noteNameSubHeader.Text = "Notlarım / " + dNotes.noteLocation + " Notu";
                            noteNoteDetail.Text = dNotes.noteDetail;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void subjectBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
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

                            var dSubjectItems = entitydb.SubjectItems.Where(p => p.subjectId == int.Parse(item.Uid)).ToList();
                            foreach (var subitem in dSubjectItems)
                            {
                                var cmbitem = new ComboBoxItem();

                                cmbitem.Content = subitem.subjectName;
                                cmbitem.Uid = subitem.subjectItemsId.ToString();
                                subjectItems.Items.Add(cmbitem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void subjectItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
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
                            var dNotesItems = entitydb.Notes.Where(p => p.subjectId == int.Parse(item.Uid)).ToList();

                            foreach (var subitem in dNotesItems)
                            {
                                var cmbitem = new ComboBoxItem();

                                cmbitem.Content = subitem.noteHeader;
                                cmbitem.Uid = subitem.notesId.ToString();
                                subjectItemsNot.Items.Add(cmbitem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void subjectItemsNot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
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
                            var dNoteItem = entitydb.Notes.Where(p => p.notesId == int.Parse(item.Uid)).FirstOrDefault();

                            subNameHeader.Text = dNoteItem.noteHeader;
                            subNameSubHeader.Text = "Konularım / " + subbase.Content + " / " + subheader.Content;
                            subNoteDetail.Text = dNoteItem.noteDetail;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void libraryBase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
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
                            var dNotesItems = entitydb.Notes.Where(p => p.libraryId == int.Parse(item.Uid)).ToList();

                            foreach (var subitem in dNotesItems)
                            {
                                var cmbitem = new ComboBoxItem();

                                cmbitem.Content = subitem.noteHeader;
                                cmbitem.Uid = subitem.notesId.ToString();
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
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void libraryItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var item = libraryItems.SelectedItem as ComboBoxItem;
                var sublib = libraryBase.SelectedItem as ComboBoxItem;
                if (item != null)
                {
                    if (item.Uid != "0")
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dNotes = entitydb.Notes.Where(p => p.notesId == int.Parse(item.Uid)).FirstOrDefault();
                            libNameHeader.Text = dNotes.noteHeader;
                            libNameSubHeader.Text = "Kütüphane / " + sublib.Content + " / " + dNotes.noteLocation + " Notu";
                            libNoteDetail.Text = dNotes.noteDetail;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dResul.resultFinallyNote = resultNoteDetail.Text;
                    entitydb.Results.Update(dResul);
                    entitydb.SaveChanges();
                    savebtn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
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
                App.logWriter("Change", ex);
            }
        }

        private void printbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.loadTask = Task.Run(() => Printer_Func(this));
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void resultNoteDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (resultNoteDetail.Text == "Sonuç Metninizi buraya yaza bilirsiniz.") resultNoteDetail.Text = "";
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        public void loadAni()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    closedbtn.IsEnabled = false;
                    printbtn.IsEnabled = false;
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
                    closedbtn.IsEnabled = true;
                    printbtn.IsEnabled = true;
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Animation", ex);
            }
        }
    }
}