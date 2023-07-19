using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

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

                App.mainScreen.succsessFunc("İşlem Başarılı", "Mail başarılı bir sekilde tarfımıza ulaştı size en kısa zamanda size geri dönüş yapıcaz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                return true;
            }
            catch (Exception ex)
            {
                logWriter("Mail", ex);
                App.mainScreen.alertFunc("İşlem Başarısız", "Mail gönderilemedi lütfen daha sonra tekrar deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
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

                App.mainScreen.succsessFunc("İşlem Başarılı", "Karşılaştığınız hata bir sekilde tarfımıza ulaştı. Hata gerekli kontrollerden sonra düzeltilecektir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                return true;
            }
            catch (Exception ex)
            {
                logWriter("Mail", ex);
                App.mainScreen.alertFunc("İşlem Başarısız", "Hata gönderilemedi lütfen daha sonra tekrar deneyiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                return false;
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