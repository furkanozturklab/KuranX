using KuranX.App.Core.Classes.Tools;
using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using KuranX.App.Core.Windows;
using System.IO.Compression;
using System.IO;

namespace KuranX.App.Core.UI.Settings
{
    /// <summary>
    /// Interaction logic for OtherUI.xaml
    /// </summary>
    public partial class OtherUI : UserControl
    {
        public OtherUI()
        {
            InitializeComponent();
            settingsVersion.Content = $"build {App.project_configuration}_{App.project_platform}_{App.project_version} / {App.project_updateDate}";
        }


        private void deleteData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    App.mainScreen.popup_settingsPage.IsOpen = false;
                    if (MessageBox.Show("Lütfen dikkat bu işlem geri alınamaz ve tüm verileriniz , bağlantılarınız ve ilerlemeniz silinecektir.", "Verileri Sıfırla", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                    {
                        resetData(entitydb);
                        App.mainScreen.succsessFunc("İşlem Başarılı", "Tüm verileriniz başarılı bir sekilde silinmiştir.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                    }
                    else
                    {
                        App.mainScreen.switch_settings(null, "Repeat");
                        App.mainScreen.popup_settingsPage.IsOpen = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void deleteProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    App.mainScreen.popup_settingsPage.IsOpen = false;
                    if (MessageBox.Show("Lütfen dikkat bu işlem geri alınamaz.Tüm verileriniz silinecek ve yeniden giriş yapmanız gerekicektir.", "Hesabı sil", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.No)
                    {
                        resetData(entitydb);
                        entitydb.Users.RemoveRange(entitydb.Users.First());
                        App.config.AppSettings.Settings["user_pin"].Value = "";
                        App.config.AppSettings.Settings["user_rememberMe"].Value = "false";
                        App.config.AppSettings.Settings["user_autoLogin"].Value = "false";
                        App.config.Save(ConfigurationSaveMode.Modified);
                        entitydb.SaveChanges();
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        App.mainScreen.switch_settings(null, "Repeat");
                        App.mainScreen.popup_settingsPage.IsOpen = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void resetData(AyetContext db)
        {
            try
            {
                foreach (var item in db.Notes) db.Notes.Remove(item);

                foreach (var item in db.Subject) db.Subject.Remove(item);

                foreach (var item in db.Subject) db.Subject.Remove(item);



                foreach (var item in db.Remider) db.Remider.Remove(item);

                foreach (var item in db.Tasks) db.Tasks.Remove(item);



                foreach (var item in db.Integrity.Where(p => p.integrityProtected == false)) db.Integrity.Remove(item);



                foreach (var item in db.Sure)
                {
                    db.Sure.Where(p => p.sureId == item.sureId).First().userCheckCount = 0;
                    db.Sure.Where(p => p.sureId == item.sureId).First().userLastRelativeVerse = 0;
                    db.Sure.Where(p => p.sureId == item.sureId).First().completed = false;
                    db.Sure.Where(p => p.sureId == item.sureId).First().status = "#ADB5BD";
                }

                foreach (var item in db.Verse)
                {
                    db.Verse.Where(p => p.verseId == item.verseId).First().markCheck = false;
                    db.Verse.Where(p => p.verseId == item.verseId).First().remiderCheck = false;
                    db.Verse.Where(p => p.verseId == item.verseId).First().verseCheck = false;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Tools.logWriter("Loginscreen", ex);
            }
        }


        private void versionText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                updateNoteStack.Children.Clear();
                this.Dispatcher.Invoke(() => popup_updateNotes.IsOpen = true);

                string[] split = App.updateNotes.Split("½");

                if (split.Length > 0)
                {
                    foreach (var item in split)
                    {
                        var txt = new TextBlock();
                        txt.Style = (Style)FindResource("updateText");
                        txt.Text = "-" + item;
                        updateNoteStack.Children.Add(txt);
                    }
                }
                else
                {
                    var txt = new TextBlock();
                    txt.Style = (Style)FindResource("updateText");
                    txt.Text = "-" + App.updateNotes;
                    updateNoteStack.Children.Add(txt);
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }


        private void openExe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() => popup_updateNotes.IsOpen = false);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Other", ex);
            }
        }

        private void backupDownBtn_Click(object sender, RoutedEventArgs e)
        {
            App.mainScreen.popup_settingsPage.IsOpen = false;
            MessageBoxResult result = MessageBox.Show("Bu işleme devam edebilmeksi için uygulama kapatılacak. Devam etmek istiyor musunuz?", "Onay", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {

                Tools.ExecuteAsAdmin($@"{App.applicationPath}\Backup\KuranBackup.exe");
                Application.Current.Shutdown();

            }
            else
            {
                App.mainScreen.popup_settingsPage.IsOpen = true;
            }
        }


        public string folderSelect()
        {
            // UpClientin dosya secme işlemi
            try
            {
                // folder open dialog
                System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Seçilen klasörün yolu
                    string folderPath = folderDialog.SelectedPath;
                    return folderPath;
                }
                else return "";

                // hatalı secimlerde catch durumunda "" döndür.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
    }
}
