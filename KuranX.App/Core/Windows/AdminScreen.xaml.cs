using KuranX.App.Core.Classes;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for AdminScreen.xaml
    /// </summary>
    public partial class AdminScreen : Window
    {
        protected string _connectedString;
        private MySqlConnection connection;
        private string land;
        private Task? fileDialogTask;
        private string DeskType;
        private OpenFileDialog openFileDialog = new OpenFileDialog();

        public AdminScreen()
        {
            InitializeComponent();
            _connectedString = "Server=LOCALHOST;Database=kuranx;Uid=root;Pwd=;";
            connection = new MySqlConnection(_connectedString);
            land = "Hepsi";

            try
            {
                connection.Open();
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }

        private void add_verses(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "select * from sureler;";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                using (var entitydb = new AyetContext())
                {
                    while (rdr.Read())
                    {
                        var Verse = new Sure();

                        Verse.sureId = (int)rdr[0];
                        Verse.name = (string)rdr[1];
                        Verse.numberOfVerses = (int)rdr[2];

                        // DB DEN YANLIŞ YER YÜZÜNDEN LANDİNG VE MUSHAF KARIŞMIŞ
                        // Düzelttim ama kontrol edilmesi lazım
                        Debug.WriteLine((int)rdr[4]);

                        Verse.deskLanding = (int)rdr[3];
                        Verse.deskMushaf = (int)rdr[4];

                        // DB DEN YANLIŞ YER YÜZÜNDEN LANDİNG VE MUSHAF KARIŞMIŞ
                        Verse.landingLocation = (string)rdr[5];
                        Verse.description = (string)rdr[6];
                        Verse.status = "#ADB5BD";

                        entitydb.Sure.Add(Verse);

                        Debug.WriteLine("VERİ EKLENDİ");
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void funcs2()
        {
            for (int i = 0; i < 100000; i++)
            {
                Debug.WriteLine(i);
                Thread.Sleep(1000);
            }
        }

        private void add_verse(object sender, RoutedEventArgs e)
        {
            string sql = "select * from ayetler;";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader rdr = cmd.ExecuteReader();

            using (var entitydb = new AyetContext())
            {
                while (rdr.Read())
                {
                    Debug.WriteLine(rdr[1]);

                    var newVe = new Verse();

                    int ch = (int)rdr[2];
                    ch++;

                    newVe.sureId = (int)rdr[0];
                    newVe.verseId = (int)rdr[1];
                    newVe.relativeDesk = ch;
                    newVe.status = "#FFFFFF";
                    newVe.verseArabic = (string)rdr[4];
                    newVe.verseTr = (string)rdr[5];
                    newVe.verseDesc = (string)rdr[6];
                    newVe.verseCheck = false;
                    newVe.remiderCheck = false;

                    entitydb.Verse.Add(newVe);

                    Debug.WriteLine("VERİ EKLENDİ");
                }

                entitydb.SaveChanges();
            }
        }

        private void add_interpreter(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "select * from interpreter;";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                using (var entitydb = new AyetContext())
                {
                    while (rdr.Read())
                    {
                        var newIn = new Interpreter();

                        newIn.interpreterId = (int)rdr[0];
                        newIn.sureId = (int)rdr[4];
                        newIn.verseId = (int)rdr[1];
                        newIn.interpreterWriter = (string)rdr[2];
                        newIn.interpreterDetail = (string)rdr[3];

                        entitydb.Interpreter.Add(newIn);
                        Debug.WriteLine("VERİ EKLENDİ");
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception err)
            {
                Debug.Write(err.Message);
            }
        }

        private void fixed_verse(object sender, RoutedEventArgs e)
        {
        }

        private void add_word(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "select * from word;";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                using (var entitydb = new AyetContext())
                {
                    while (rdr.Read())
                    {
                        var newWe = new Words();

                        newWe.verseId = (int)rdr[1];
                        newWe.wordText = (string)rdr[2];
                        newWe.wordRe = (string)rdr[3];
                        newWe.sureId = (int)rdr[4];

                        entitydb.Words.Add(newWe);

                        Debug.WriteLine("VERİ EKLENDİ");
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception err)
            {
                Debug.Write(err.Message);
            }
        }

        private void test_click(object sender, RoutedEventArgs e)
        {
            List<Sure> VersesList;

            land = "Mekke";
            DeskType = "Mushaf Sırası";

            using (var entitydb = new AyetContext())
            {
                if (land != "Hepsi")
                {
                    if (DeskType == "İniş Sırası")
                    {
                        VersesList = (List<Sure>)entitydb.Sure.Where(p => p.landingLocation == land).OrderBy(p => p.deskLanding).ToList();
                    }
                    else
                    {
                        VersesList = (List<Sure>)entitydb.Sure.Where(p => p.landingLocation == land).OrderBy(p => p.deskMushaf).ToList();
                    }

                    foreach (var item in VersesList)
                    {
                        Debug.WriteLine(item.name);
                    }
                }
                else
                {
                    if (DeskType == "İniş Sırası")
                    {
                        VersesList = (List<Sure>)entitydb.Sure.OrderBy(p => p.deskLanding).ToList();
                    }
                    else
                    {
                        VersesList = (List<Sure>)entitydb.Sure.OrderBy(p => p.deskMushaf).ToList();
                    }

                    foreach (var item in VersesList)
                    {
                        Debug.WriteLine(item.name);
                    }
                }
            }
        }

        private void filedialogtask()
        {
            this.Dispatcher.Invoke(() =>
            {
                openFileDialog.Filter = "Pdf Files|*.pdf";
                openFileDialog.CheckFileExists = true;
                bool? response = openFileDialog.ShowDialog();

                if (response == true)
                {
                    string fileS = string.Format("{0} {1}", (new FileInfo(openFileDialog.FileName).Length / 1.049e+6).ToString("0.0"), "Mb");
                    var newSoruceLocation = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) + @"\KuranX\Publisher\" + openFileDialog.FileName.Split(@"\").Last();

                    using (var entitydb = new AyetContext())
                    {
                        string name = openFileDialog.FileName.Split(@"\").Last();
                        var dControl = entitydb.PdfFile.Where(p => p.fileName == name).ToList();

                        if (File.Exists(newSoruceLocation) && dControl.Count != 0)
                        {
                            MessageBox.Show("Eklenmiş Dosya");
                        }
                        else
                        {
                            File.Copy(openFileDialog.FileName, newSoruceLocation, true);

                            var newFile = new PdfFile { fileName = openFileDialog.FileName.Split(@"\").Last(), fileUrl = newSoruceLocation, fileSize = fileS, created = DateTime.Now, modify = DateTime.Now, fileType = "Editor" };
                            entitydb.PdfFile.Add(newFile);
                            entitydb.SaveChanges();

                            MessageBox.Show("Dosya Eklendi");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Dosya yükleme sırasında bir hata meydana geldi.");
                }
            });
        }

        private void editorFileUpdate_Click(object sender, RoutedEventArgs e)
        {
            fileDialogTask = new Task(filedialogtask);
            fileDialogTask.Start();
            /*
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\KuranX\Publisher";

            var allFiles = Directory.GetFiles(path, "*.pdf*", SearchOption.AllDirectories);

            totalcount = allFiles.Count();

            for (int x = 1; x < 21; x++)
            {
                ItemsControl itemslist = (ItemsControl)this.FindName("pdf" + x);
                itemslist.ItemsSource = null;
            }
            int i = 1;

            foreach (var file in allFiles)
            {
                tempPdfFileitems = new List<PdfFile>()
                            {
                                new PdfFile()
                                {
                                    FileName = file.Split(@"\").Last(),
                                    FileUrl = file,
                                }
                            };

                ItemsControl itemslist = (ItemsControl)this.FindName("pdf" + i);
                itemslist.ItemsSource = tempPdfFileitems;
                tempPdfFileitems.Clear();
                i++;

                if (i == 21) break; // 12 den fazla varmı kontrol etmek için koydum

                if (lastPdfItems == 0) previusPageButton.IsEnabled = false;
                else previusPageButton.IsEnabled = true;

                if (dPdfFile.Count() <= 20) nextpageButton.IsEnabled = false;
                if (lastPdfItems == 0 && dPdfFile.Count() > 20) nextpageButton.IsEnabled = true;

                totalcountText.Tag = totalcount.ToString();

                nowPageStatus.Tag = NowPage + " / " + Math.Ceiling(decimal.Parse(totalcount.ToString()) / 20).ToString();
            }
            */
        }

        private void resultCreate_Click(object sender, RoutedEventArgs e)
        {
            string sql = "select * from sureler";
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader rdr = cmd.ExecuteReader();

            using (var entitydb = new AyetContext())
            {
                while (rdr.Read())
                {
                    var newWe = new Result();

                    newWe.resultName = rdr[1].ToString();

                    entitydb.Results.Add(newWe);

                    Debug.WriteLine("VERİ EKLENDİ");
                }

                entitydb.SaveChanges();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            App.loadTask = Task.Run(() => funcs2());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}