using Google.Protobuf.WellKnownTypes;
using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages;
using KuranX.App.Core.Pages.NoteF;
using KuranX.App.Core.Pages.ReminderF;
using KuranX.App.Core.Pages.SectionF;
using KuranX.App.Core.Pages.SubjectF;
using KuranX.App.Core.Pages.VerseF;
using KuranX.App.Core.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
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
        public static string InterpreterWriter = "";
        public static string beforeFrameName = "";
        public static bool starup = true;

        public static ApiPostProjectNotes returnPostNotes;
        public static ApiPostProject returnPostProject;
        public static ApiPostErr returnPostError;

        private void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        public static async Task apiPostRun(Dictionary<string, string> data, string action)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var endpoint = new Uri(config.AppSettings.Settings["api_adress"].Value);
                    var content = new FormUrlEncodedContent(data);
                    var result = await client.PostAsync(endpoint, content);
                    string json = result.Content.ReadAsStringAsync().Result;

                    if (action == "version") returnPostProject = JsonConvert.DeserializeObject<ApiPostProject>(json);
                    if (action == "UpdateNotes") returnPostNotes = JsonConvert.DeserializeObject<ApiPostProjectNotes>(json);
                    if (action == "AddError") returnPostError = JsonConvert.DeserializeObject<ApiPostErr>(json);

                    lastversion = returnPostProject.Data[0].project_version_x86;
                    starup = true;
                }
            }
            catch (Exception ex)
            {
                starup = false;
                logWriter("AppStartUp", ex);
            }
        }

        public async void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {

                if (File.Exists(errPath)) File.Delete(errPath);


                config.AppSettings.Settings["application_location"].Value = AppDomain.CurrentDomain.BaseDirectory;
                config.Save(ConfigurationSaveMode.Modified);

                if (NetworkInterface.GetIsNetworkAvailable() == true)
                {
                    var postingdata = new Dictionary<string, string>
                    {
                    { config.AppSettings.Settings["api_tokenName"].Value, config.AppSettings.Settings["api_token"].Value},
                    { "post_projectid", "3" },
                    { "post_table", "table_project" },
                    { "post_action", "GET" }
                    };

                    loadTask = Task.Run(() => apiPostRun(postingdata, "version"));
                    await loadTask;

                    postingdata = new Dictionary<string, string>
                    {
                            { config.AppSettings.Settings["api_tokenName"].Value, config.AppSettings.Settings["api_token"].Value},
                            { "post_projectid", config.AppSettings.Settings["application_id"].Value },
                            { "post_action", "POST" },
                            { "post_type", "UpdateNotes" },
                            { "post_version", lastversion },
                            { "post_platform", config.AppSettings.Settings["application_platform"].Value },
                    };

                    loadTask = Task.Run(() => apiPostRun(postingdata, "UpdateNotes"));
                    await loadTask;

                    if (starup == false)
                    {
                        if (config.AppSettings.Settings["user_autoLogin"].Value == "true") this.Dispatcher.Invoke(() => mainScreen.Show());
                        else
                        {
                            loginScreen lg = new loginScreen();
                            lg.Show();
                        }
                    }
                    else
                    {
                        if (config.AppSettings.Settings["application_version"].Value != lastversion)
                        {
                            MessageBox.Show("Yeni güncelleme mevcut devam etmeden önce güncelleme yapılmalı.");
                            ExecuteAsAdmin(AppDomain.CurrentDomain.BaseDirectory + @"Updater\Updater.exe");
                            Current.Shutdown();
                        }
                        else
                        {
                            if (config.AppSettings.Settings["user_autoLogin"].Value == "true")
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
                    }
                }
                else
                {
                    MessageBox.Show("İnternet bağlantısı yok eski bir versiyon kullanıyor olabilirsiniz.");
                    loginScreen lg = new loginScreen();
                    lg.Show();
                }
            }
            catch (Exception ex)
            {
                starup = false;
                logWriter("AppStartUp", ex);
            }
        }

        public static void logWriter(string type, Exception exe)
        {
            try
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

                App.mainframe.Content = navHomeFrame.PageCall();
                mainScreen.alertFunc("Hata Oluştu", "Program bir hata ile karşılaştı ve log dosyası oluşturuldu bu hatayı alma devam ederseniz log dosyanızı bize gönderiniz.", 5);
            }
            catch (Exception ex)
            {


            }

        }


        public static void errWrite(string msg)
        {
            try
            {
                /*  File.AppendAllText(errPath, msg);
                    File.AppendAllText(errPath, Environment.NewLine);*/
            }
            catch (Exception ex)
            {


            }
        }

        public static bool sendMail(string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.outlook.com";
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("kuransunnetullah@outlook.com", "muhammed1AB");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("kuransunnetullah@outlook.com", "Kuransunetullah Application Support");
                mail.To.Add("kuransunnetullah@outlook.com");
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;

                client.Send(mail);

                mainScreen.succsessFunc("İşlem Başarılı", "Mail başarılı bir sekilde tarfımıza ulaştı size en kısa zamanda size geri dönüş yapıcaz.", int.Parse(config.AppSettings.Settings["app_warningShowTime"].Value));
                return true;
            }
            catch (Exception ex)
            {
                logWriter("Mail", ex);
                mainScreen.alertFunc("İşlem Başarısız", "Mail gönderilemedi lütfen daha sonra tekrar deneyiniz.", int.Parse(config.AppSettings.Settings["app_warningShowTime"].Value));
                return false;
            }
        }


        public static bool sendMailErr(string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.outlook.com";
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("kuransunnetullah@outlook.com", "muhammed1AB");



                MailMessage mail = new MailMessage();
                Attachment attachment, attachment2;
                attachment = new Attachment(AppDomain.CurrentDomain.BaseDirectory + "usingTree.txt");
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
                {

                    attachment2 = new Attachment(AppDomain.CurrentDomain.BaseDirectory + "log.txt");
                    mail.Attachments.Add(attachment2);
                }



                mail.From = new MailAddress("kuransunnetullah@outlook.com", "Kuransunetullah Application Support");
                mail.To.Add("nakruf5884@gmail.com");
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                mail.Attachments.Add(attachment);



                client.Send(mail);

                mainScreen.succsessFunc("İşlem Başarılı", "Karşılaştığınız hata bir sekilde tarfımıza ulaştı. Hata gerekli kontrollerden sonra düzeltilecektir.", int.Parse(config.AppSettings.Settings["app_warningShowTime"].Value));
                return true;
            }
            catch (Exception ex)
            {
                logWriter("Mail", ex);
                mainScreen.alertFunc("İşlem Başarısız", "Hata gönderilemedi lütfen daha sonra tekrar deneyiniz.", int.Parse(config.AppSettings.Settings["app_warningShowTime"].Value));
                return false;
            }
        }
    }
}