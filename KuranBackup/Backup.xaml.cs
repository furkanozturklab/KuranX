using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Shell;

namespace KuranBackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Backup : Window
    {
        public Backup()
        {
            InitializeComponent();
        }

        private async void backup_MouseDown(object sender, MouseButtonEventArgs e)
        {


            Process[] runningProcesses = Process.GetProcesses();
            var x = runningProcesses.ToList().Find(e => e.ProcessName == "KuranSunnetullah");
            if (x != null)
            {
                x.Kill();
            }

            try
            {
                var dwPath = folderSelect();
                string folderPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuranSunnetullah";
                FileInfo filePath = new FileInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuranSunnetullah\Ayet.db");
                File.WriteAllText($@"{folderPath}\hash.txt", await getHash(filePath.FullName));
                string zipPath = $@"{dwPath}\kuransunnetullah.zip";


                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                ZipFile.CreateFromDirectory(folderPath, zipPath);
                File.Delete($@"{folderPath}\hash.txt");
                MessageBoxResult result = MessageBox.Show("Güncelleme başarılı programı açmak istermisiniz ?", "Aç", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ExecuteAsAdmin(App.applicationPath);
                    Application.Current.Shutdown();
                }
                else
                {
                    Application.Current.Shutdown();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlemler sırasında bir hata meydana geldi lütfen daha sonra yeniden deneyiniz.");
            }


        }


        private async void reWrite(string upPath, string filePath)
        {

            ZipFile.ExtractToDirectory(upPath, filePath);
            var ayetdb = new FileInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuranSunnetullah\temp\Ayet.db");
            var writePath = ayetdb.FullName.Replace(@"\temp", "");

            if (File.Exists(ayetdb.FullName))
            {

                var hashTxt = new FileInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuranSunnetullah\temp\hash.txt");
                var hash = File.ReadAllText(hashTxt.FullName);
                if (hash == await getHash(ayetdb.FullName))
                {

                    File.Copy(ayetdb.FullName, writePath, true);
                    Directory.Delete(filePath, true);
                    MessageBox.Show("Geri yükleme başarılı...");
                    ExecuteAsAdmin(App.applicationPath);
                    Application.Current.Shutdown();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Veri bozuk olabilir. Bu işleme devam etmek isterseniz verileriniz tamemen kaybola bilir. Devam etmek istiyor musunuz?", "Onay", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {

                        File.Copy(ayetdb.FullName, writePath, true);
                        Directory.Delete(filePath, true);
                        MessageBox.Show("Geri yükleme başarılı...");
                        ExecuteAsAdmin(App.applicationPath);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        Directory.Delete(filePath, true);
                    }
                }
            }
            else
            {
                MessageBox.Show("Gerekli dosya bulunamadı. Lütfen doğru yedekleme dosyasını seçtiğinizden emin olunuz.");
                Directory.Delete(filePath, true);
            }
        }


        private async void recover_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Process[] runningProcesses = Process.GetProcesses();
            var x = runningProcesses.ToList().Find(e => e.ProcessName == "KuranSunnetullah");
            if (x != null)
            {
                x.Kill();
            }


            string upPath = fileSelect();
            string filePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KuranSunnetullah\temp";


            if (Directory.Exists(filePath))
            {
                try
                {
                    Directory.Delete(filePath, true);
                    reWrite(upPath, filePath);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Bir hata oluştu daha sonra yeniden deneyiniz.");
                }
            }
            else
            {
                reWrite(upPath, filePath);
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

        public string fileSelect()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Zip Dosyaları (*.zip)|*.zip";
            openFileDialog.DefaultExt = ".zip";
            openFileDialog.Title = "Lütfen Yedeklediğiniz dosyayı seçiniz.";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }


        public static async Task<string> getHash(string file)
        {
            // dosyanın hash cıkarmak için kullanıyoruz.
            try
            {
                byte[] hash;
                using (FileStream stream = File.OpenRead(file))
                {
                    SHA256Managed sha = new SHA256Managed();
                    hash = sha.ComputeHash(stream);
                }

                // oluşan bit değerini string türüne ceviriyorum ve döndürüyorum.
                Debug.WriteLine("SHA-256 hash of file : '{0}' -> {1}", file, BitConverter.ToString(hash).Replace("-", "").ToLower());
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hash Error:" + ex.Message);
                return null;
            }
        }


        public static void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
    }
}
