using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

namespace KuranX.App.Core.Pages.ResultF
{
    /// <summary>
    /// Interaction logic for NotePrinter.xaml
    /// </summary>
    public partial class ResultPrinter : Page
    {
        public ResultPrinter()
        {
            InitializeComponent();
        }

        public ResultPrinter(int currentId) : this()
        {
            using (var entitydb = new AyetContext())
            {
                create.Text = DateTime.Now.ToString("D");

                var dResul = entitydb.Results.Where(p => p.ResultId == currentId).FirstOrDefault();

                header.Text = dResul.ResultName;
                noteDetail.Text = dResul.ResultFinallyNote;
                if (dResul.ResultNotes == "true")
                {
                    noteico.IsEnabled = true;
                    notecount.Text = entitydb.ResultItems.Where(p => p.ResultId == currentId && p.ResultNoteId != 0).Count().ToString() + " Adet Not";
                }
                if (dResul.ResultSubject == "true")
                {
                    subico.IsEnabled = true;
                    subcount.Text = entitydb.ResultItems.Where(p => p.ResultId == currentId && p.ResultSubjectId != 0).Count().ToString() + " Adet Konu";
                }
                if (dResul.ResultLib == "true")
                {
                    libico.IsEnabled = true;
                    libcount.Text = entitydb.ResultItems.Where(p => p.ResultId == currentId && p.ResultLibId != 0).Count().ToString() + " Adet Kütüphane ";
                }
            }
        }
    }
}