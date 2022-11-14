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
                var dNotes = entitydb.Notes.Where(p => p.NotesId == id).FirstOrDefault();

                header.Text = dNotes.NoteHeader;
                create.Text = dNotes.Created.ToString("D");
                location.Text = dNotes.NoteLocation;
                loadNoteDetail.Text = dNotes.NoteDetail;

                if (dNotes.SureId != 0)
                {
                    var dSure = entitydb.Sure.Where(p => p.sureId == dNotes.SureId).FirstOrDefault();
                    infoText.Text = "Not Aldığınız Ayet " + Environment.NewLine + dSure.Name + " suresini " + dNotes.VerseId + " ayeti";
                }

                // PDF Bağlanmış
                if (dNotes.PdfFileId != 0)
                {
                    var dPdf = entitydb.PdfFile.Where(p => p.PdfFileId == dNotes.PdfFileId).FirstOrDefault();
                    infoText.Text = "Not Aldığınız Dosya " + Environment.NewLine + dPdf.FileName;
                }

                // Konu Bağlanmış
                if (dNotes.SubjectId != 0)
                {
                    var dSubject = entitydb.SubjectItems.Where(p => p.SubjectItemsId == dNotes.SubjectId).FirstOrDefault();
                    var dx = entitydb.Subject.Where(p => p.SubjectId == dSubject.SubjectId).FirstOrDefault();
                    infoText.Text = "Not Aldığınız Konu" + Environment.NewLine + dx.SubjectName;
                }
            }

            return this;
        }
    }
}