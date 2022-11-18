using KuranX.App.Core.Pages;
using KuranX.App.Core.Pages.LibraryF;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ReminderF;
using KuranX.App.Core.Pages.ResultF;
using KuranX.App.Core.Pages.SubjectF;
using KuranX.App.Core.Pages.VerseF;
using KuranX.App.Core.Windows;
using System;
using System.Configuration;

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KuranX.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Frame? mainframe;
        public static string currentDesktype = "DeskLanding";

        public static DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        public static Task loadTask;

        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        // Main PANEL

        public static homeScreen mainScreen = new homeScreen();

        // VERSE PANEL

        public static sureFrame navSurePage = new sureFrame();
        public static verseFrame navVersePage = new verseFrame();

        // SUBJECT PANEL
        public static SubjectFrame navSubjectPage = new SubjectFrame();

        public static SubjectFolder navSubjectFolder = new SubjectFolder();
        public static SubjectItem navSubjectItem = new SubjectItem();

        // LİBRARY PANEL

        public static LibraryOpenPage navLibraryOpen = new LibraryOpenPage();
        public static LibraryFileFrame navLibraryFileFrame = new LibraryFileFrame();
        public static LibraryNoteItemsFrame navLibraryNoteItemsFrame = new LibraryNoteItemsFrame();
        public static LibraryNoteFolderFrame navLibraryNoteFolderFrame = new LibraryNoteFolderFrame();

        // NOTE PANEL

        public static NoteFrame navNotesPage = new NoteFrame();
        public static NoteItem navNoteItem = new NoteItem();
        public static NotePrinter notePrinter = new NotePrinter();

        // RESULT PANEL

        public static ResultFrame navResultPage = new ResultFrame();
        public static ResultItem navResultItem = new ResultItem();
        public static ResultPrinter navResultPrinter = new ResultPrinter();

        // REMİDER PANEL

        public static RemiderFrame navRemiderPage = new RemiderFrame();
        public static RemiderItem navRemiderItem = new RemiderItem();

        public static TestFrame navTestPage = new TestFrame();

        public void App_Startup(object sender, StartupEventArgs e)
        {
            mainScreen.Show();
        }

        public static void logWriter(string type, Exception exe)
        {
            File.AppendAllText("log.txt", Environment.NewLine);
            string ExString = "[" + type + ":" + DateTime.Now + "  " + Environment.OSVersion.ToString() + " ]";
            File.AppendAllText("log.txt", ExString);
            File.AppendAllText("log.txt", Environment.NewLine);
            File.AppendAllText("log.txt", "[Error StackTrace]");
            File.AppendAllText("log.txt", Environment.NewLine);
            File.AppendAllText("log.txt", exe.StackTrace);
            File.AppendAllText("log.txt", Environment.NewLine);
            File.AppendAllText("log.txt", "[Error Message]");
            File.AppendAllText("log.txt", exe.Message);
        }

        public static void apploadsystem()
        {
            if (config.AppSettings.Settings["TaskLastUpdate"].Value != DateTime.Now.ToString("d"))
            {
                config.AppSettings.Settings["TaskLastUpdate"].Value = DateTime.Now.ToString("d");
                config.AppSettings.Settings["TaskLastStatus"].Value = "UpdateWait";
                ConfigurationManager.RefreshSection("appSettings");
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}