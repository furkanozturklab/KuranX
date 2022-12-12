using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages;
using KuranX.App.Core.Pages.LibraryF;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ReminderF;
using KuranX.App.Core.Pages.ResultF;
using KuranX.App.Core.Pages.SubjectF;
using KuranX.App.Core.Pages.VerseF;
using KuranX.App.Core.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

using System.Diagnostics;
using System.IO;
using System.Net.Http;
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
        public string lastversion;

        public static Frame? mainframe;
        public static Frame? secondFrame;

        public static DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        public static DispatcherTimer lifeCycler = new DispatcherTimer(DispatcherPriority.Render);
        public static Task? loadTask;
        public static Task? mainTask;

        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        // Main PANEL

        public static homeScreen mainScreen = new homeScreen();

        public static HomePage navHomeFrame = new HomePage();

        // VERSE PANEL

        public static sureFrame navSurePage = new sureFrame();
        public static verseFrame navVersePage = new verseFrame();
        public static verseStickFrame navVerseStickPage = new verseStickFrame();

        // SUBJECT PANEL
        public static SubjectFrame navSubjectFrame = new SubjectFrame();

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
        public static Core.Pages.ResultF.ResultItem navResultItem = new Core.Pages.ResultF.ResultItem();
        public static ResultPrinter navResultPrinter = new ResultPrinter();

        // REMİDER PANEL

        public static RemiderFrame navRemiderPage = new RemiderFrame();
        public static RemiderItem navRemiderItem = new RemiderItem();

        public static TestFrame navTestPage = new TestFrame();
        public static string InterpreterWriter = "";

        private void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        private async Task versionCheck()
        {
            using (var client = new HttpClient())
            {
                var postingdata = new Dictionary<string, string>
                    {
                    { "post_securityCode","meltdown"},
                    { "post_projectid", "3" },
                    { "post_table", "table_project" },
                    { "post_action", "GET" }
                    };

                var endpoint = new Uri(config.AppSettings.Settings["api_adress"].Value);
                var content = new FormUrlEncodedContent(postingdata);
                var result = await client.PostAsync(endpoint, content);
                string json = result.Content.ReadAsStringAsync().Result;

                ApiPostProject appInfo = JsonConvert.DeserializeObject<ApiPostProject>(json);

                lastversion = appInfo.Data[0].project_version_x86;
            }
        }

        public async void App_Startup(object sender, StartupEventArgs e)
        {
            loadTask = Task.Run(() => versionCheck());
            await loadTask;

            if (config.AppSettings.Settings["application_version"].Value != lastversion)
            {
                MessageBox.Show("Yeni güncelleme mevcut devam etmeden önce güncelleme yapılmalı.");
                ExecuteAsAdmin(AppDomain.CurrentDomain.BaseDirectory + @"Updater\Updater.exe");
                Current.Shutdown();
            }
            else
            {
                if (config.AppSettings.Settings["user_autoLogin"].Value == "true") this.Dispatcher.Invoke(() => mainScreen.Show());
                else
                {
                    loginScreen lg = new loginScreen();
                    lg.Show();
                }
            }
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