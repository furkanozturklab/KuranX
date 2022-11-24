using KuranX.App.Core.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuranX.App.Core.Pages.NoteF
{
    /// <summary>
    /// Interaction logic for NotePrinter.xaml
    /// </summary>
    public partial class NotePrinter : Page
    {
        public NotePrinter()
        {
            InitializeComponent();
        }

        public Page notePrinterCall(int id)
        {
            using (var entitydb = new AyetContext())
            {
                var dNotes = entitydb.Notes.Where(p => p.notesId == id).FirstOrDefault();

                header.Text = dNotes.noteHeader;
                create.Text = dNotes.created.ToString("D");
                location.Text = dNotes.noteLocation;
                loadNoteDetail.Text = dNotes.noteDetail;

                if (dNotes.sureId != 0)
                {
                    var dSure = entitydb.Sure.Where(p => p.sureId == dNotes.sureId).FirstOrDefault();
                    infoText.Text = "Not Aldığınız Ayet " + Environment.NewLine + dSure.name + " suresini " + dNotes.verseId + " ayeti";
                }

                // PDF Bağlanmış
                if (dNotes.pdfFileId != 0)
                {
                    var dPdf = entitydb.PdfFile.Where(p => p.pdfFileId == dNotes.pdfFileId).FirstOrDefault();
                    infoText.Text = "Not Aldığınız Dosya " + Environment.NewLine + dPdf.fileName;
                }

                // Konu Bağlanmış
                if (dNotes.subjectId != 0)
                {
                    var dSubject = entitydb.SubjectItems.Where(p => p.subjectItemsId == dNotes.subjectId).FirstOrDefault();
                    var dx = entitydb.Subject.Where(p => p.subjectId == dSubject.subjectId).FirstOrDefault();
                    infoText.Text = "Not Aldığınız Konu" + Environment.NewLine + dx.subjectName;
                }
            }

            return this;
        }
    }
}