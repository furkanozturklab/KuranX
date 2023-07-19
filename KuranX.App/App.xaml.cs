using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Tools;
using KuranX.App.Core.Pages;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ReminderF;
using KuranX.App.Core.Pages.SectionF;
using KuranX.App.Core.Pages.SubjectF;
using KuranX.App.Core.Pages.VerseF;
using KuranX.App.Core.Windows;
using KuranX.App.Services;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
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
        public static string lastversion, lastlocation;
        public static string errPath = AppDomain.CurrentDomain.BaseDirectory + "usingTree.txt";
        public static ResourceDictionary? resourceLang;

        public static Frame? mainframe;
        public static Frame? secondFrame;

        public static DispatcherTimer timeSpan = new DispatcherTimer(DispatcherPriority.Render);
        public static DispatcherTimer lifeCycler = new DispatcherTimer(DispatcherPriority.Render);
        public static Task? loadTask;
        public static Task? mainTask;

        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static string applicationPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        // Main PANEL

        public static homeScreen mainScreen = new homeScreen();

        public static HomePage navHomeFrame = new HomePage();

        // VERSE PANEL

        public static sureFrame navSurePage = new sureFrame();
        public static verseFrame navVersePage = new verseFrame();
        public static verseStickFrame navVerseStickPage = new verseStickFrame();
        public static SectionFrame navSectionPage = new SectionFrame();

        // SUBJECT PANEL
        public static SubjectFrame navSubjectFrame = new SubjectFrame();

        public static SubjectFolder navSubjectFolder = new SubjectFolder();
        public static SubjectItem navSubjectItem = new SubjectItem();

        // NOTE PANEL

        public static NoteFrame navNotesPage = new NoteFrame();
        public static NoteItem navNoteItem = new NoteItem();
        public static NotePrinter notePrinter = new NotePrinter();

        // RESULT PANEL
        /*
        public static ResultFrame navResultPage = new ResultFrame();
        public static Core.Pages.ResultF.ResultItem navResultItem = new Core.Pages.ResultF.ResultItem();
        public static ResultPrinter navResultPrinter = new ResultPrinter();
        */
        // REMİDER PANEL

        public static RemiderFrame navRemiderPage = new RemiderFrame();
        public static RemiderItem navRemiderItem = new RemiderItem();

        // Helper Panel

        public static UserHelpPage navUserHelp = new UserHelpPage();

        public static TestFrame navTestPage = new TestFrame();

        public static string project_configuration = "", project_platform = "", project_version = "", project_updateDate = "";

        public static string InterpreterWriter = "";
        public static string updateNotes = "";
        public static string beforeFrameName = "";
        public static bool starup = true;



        private void login(bool autoLogin = false)
        {
            if (autoLogin)
            {
                using (var entitydb = new AyetContext())
                {
                    if (entitydb.Users.Count() > 0) this.Dispatcher.Invoke(() => mainScreen.Show());
                    else
                    {
                        config.AppSettings.Settings["user_pin"].Value = "";
                        config.AppSettings.Settings["user_rememberMe"].Value = "false";
                        config.AppSettings.Settings["user_autoLogin"].Value = "false";
                        config.Save(ConfigurationSaveMode.Modified);
                        MessageBox.Show("Lütfen Yeniden Başlatınız...");
                        Current.Shutdown();
                    }
                }
            }
            else
            {
                loginScreen lg = new loginScreen();
                lg.Show();
            }
        }

        public async void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (await Loading.loadSync())
                {
                    if (File.Exists(errPath)) File.Delete(errPath);

                    if (NetworkInterface.GetIsNetworkAvailable() == true)
                    {
                        ApiServices apiServices = new ApiServices();

                        var project = await apiServices.projectGet(config.AppSettings.Settings["app_id"].Value);

                        if (project != null)
                        {
                            var updateNote = await apiServices.updateGet(config.AppSettings.Settings["app_id"].Value, project[0].project_version!);

                            project_configuration = project[0].project_configuration!;
                            project_platform = project[0].project_platform!;
                            project_version = project[0].project_version!;
                            project_updateDate = project[0].project_updateDate!;

                            if (updateNote[0].update_detail != null)
                            {

                               
                                updateNotes = updateNote[0].update_detail!;
                            }
                            else
                            {
                                project_version = config.AppSettings.Settings["app_version"].Value;
                                updateNotes = "";
                            }


                            if (project[0].project_version != config.AppSettings.Settings["app_version"].Value)
                            {
                                MessageBox.Show("Yeni güncelleme mevcut devam etmeden önce güncelleme yapılmalı.");
                                Tools.ExecuteAsAdmin(AppDomain.CurrentDomain.BaseDirectory + @"Updater\Updater.exe");
                                Current.Shutdown();
                            }
                            else
                            {
                                if (config.AppSettings.Settings["user_autoLogin"].Value == "true") login(true);
                                else login(false);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme alınamadı lütfen daha sonra tekrar deneyin. Eski versiyonlardan birisi ile devam ediyorsunuz.");
                            login(false);
                        }
                    }
                    else
                    {
                        MessageBox.Show("İnternet bağlantısı yok eski bir versiyon kullanıyor olabilirsiniz.");
                        login(false);
                    }

                }
                else
                {

                    Current.Shutdown();
                }


            }
            catch (Exception ex)
            {
                starup = false;
                Tools.logWriter("AppStartUp", ex);
                Debug.WriteLine(ex.Message);
            }
        }




    }
}