using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
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

namespace KuranX.App.Core.Pages.ResultF
{
    /// <summary>
    /// Interaction logic for ResultPrinter.xaml
    /// </summary>
    public partial class ResultPrinter : Page
    {
        public ResultPrinter()
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

        public Page PageCall(int currentId)
        {
            try
            {
                App.loadTask = Task.Run(() => loadItem(currentId));
                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
                return this;
            }
        }

        private void loadItem(int currentId)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dResul = entitydb.Results.Where(p => p.resultId == currentId).FirstOrDefault();

                    this.Dispatcher.Invoke(() =>
                    {
                        create.Text = DateTime.Now.ToString("D");
                        header.Text = dResul.resultName;
                        noteDetail.Text = dResul.resultFinallyNote;

                        if (dResul.resultNotes == true)
                        {
                            noteico.IsEnabled = true;
                            notecount.Text = entitydb.ResultItems.Where(p => p.resultId == currentId && p.resultNoteId != 0).Count().ToString() + " Adet Not";
                        }
                        if (dResul.resultSubject == true)
                        {
                            subico.IsEnabled = true;
                            subcount.Text = entitydb.ResultItems.Where(p => p.resultId == currentId && p.resultSubjectId != 0).Count().ToString() + " Adet Konu";
                        }
                        if (dResul.resultLib == true)
                        {
                            libico.IsEnabled = true;
                            libcount.Text = entitydb.ResultItems.Where(p => p.resultId == currentId && p.resultLibId != 0).Count().ToString() + " Adet Kütüphane ";
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
            }
        }
    }
}