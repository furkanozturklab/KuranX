using KuranX.App.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace KuranX.App.Core.Classes.Tools
{
    public class Tools
    {
        public static void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        public static async Task sendMail(string subject, string body, string type = "Support", string err = null)
        {
            try
            {
                bool sendStatus = false;
                if (App.lastSend != null)
                {
                    TimeSpan tt = DateTime.Now - App.lastSend;
                    double totalMinutesDifference = tt.TotalMinutes;
                    if (totalMinutesDifference >= 1)
                    {
                        sendStatus = true;
                        App.lastSend = DateTime.Now;
                    }
                    else
                    {
                        App.mainScreen.alertFunc("İşlem Başarısız", $"Mail göndermek için {(tt.TotalSeconds - 60.00).ToString("0").Replace("-", "")} sn beklemeniz gerekiyor.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                        sendStatus = false;
                    }
                }
                else
                {
                    App.lastSend = DateTime.Now;
                    sendStatus = true;
                }

                if (sendStatus)
                {
                    ApiServices apiServices = new ApiServices();
                    string attachPath = "";
                    string[] mail = new string[5];

                    mail[0] = "Kuransunnetullah";
                    mail[1] = type;
                    mail[2] = subject;
                    mail[3] = body;
                    mail[4] = "kuransunnetullah@gmail.com";

                    if (err != null)
                    {
                        attachPath = "";
                        if (File.Exists(attachPath))
                            attachPath = AppDomain.CurrentDomain.BaseDirectory + "Error" + @"\log.txt";
                    }

                    if (await apiServices.apiSendMail(mail, attachPath, err) == "202") App.mainScreen.succsessFunc("İşlem Başarılı", "Mail başarılı bir sekilde tarfımıza ulaştı size en kısa zamanda size geri dönüş yapıcaz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    else App.mainScreen.alertFunc("İşlem Başarısız", "Mail gönderilemedi lütfen daha sonra tekrar deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                }
            }
            catch (Exception ex)
            {
                logWriter("Mail", ex);
                App.mainScreen.alertFunc("İşlem Başarısız", "Mail gönderilemedi lütfen daha sonra tekrar deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
            }
        }

        public static void logWriter(string type, Exception exe)
        {
            try
            {
                if (!Directory.Exists("Error")) Directory.CreateDirectory("Error");

                File.AppendAllText("Error/log.txt", Environment.NewLine);
                string ExString = "[" + type + ":" + DateTime.Now + "  " + Environment.OSVersion.ToString() + " ]";
                File.AppendAllText("Error/log.txt", ExString);
                File.AppendAllText("Error/log.txt", Environment.NewLine);
                File.AppendAllText("Error/log.txt", "[Error StackTrace]");
                File.AppendAllText("Error/log.txt", Environment.NewLine);
                File.AppendAllText("Error/log.txt", exe.StackTrace);
                File.AppendAllText("Error/log.txt", Environment.NewLine);
                File.AppendAllText("Error/log.txt", "[Error Message]");
                File.AppendAllText("Error/log.txt", exe.Message);

                App.mainframe.Content = App.navHomeFrame.PageCall();
                App.mainScreen.alertFunc("Hata Oluştu", "Program bir hata ile karşılaştı ve log dosyası oluşturuldu bu hatayı alma devam ederseniz log dosyanızı bize gönderiniz.", 5);
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

        public static bool IsNumeric(string value)
        {
            try
            {
                if (int.TryParse(value, out int numericValue)) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
                Tools.logWriter("Other", ex);
            }
        }
    }
}