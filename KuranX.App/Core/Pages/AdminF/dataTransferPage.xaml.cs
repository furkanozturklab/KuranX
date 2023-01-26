using KuranX.App.Core.Classes;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace KuranX.App.Core.Pages.AdminF
{
    /// <summary>
    /// Interaction logic for dataTransferPage.xaml
    /// </summary>
    public partial class dataTransferPage : Page
    {
        protected string _connectedString;
        private MySqlConnection connection;

        public dataTransferPage()
        {
            InitializeComponent();
            _connectedString = "Server=LOCALHOST;Database=kuranx;Uid=root;Pwd=;";
            connection = new MySqlConnection(_connectedString);

            try
            {
                connection.Open();
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }
            log.Items.Add("mELTDOWN");
        }

        private void addSureFunc()
        {
            using (var entitydb = new AyetContext())
            {
                if (entitydb.Sure.Count() > 0)
                {
                    MessageBox.Show("Daha Önceden İşlem Yapılmış");
                }
                else
                {
                    string sql = "select * from sureler;";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader rdr = cmd.ExecuteReader();

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

                        this.Dispatcher.Invoke(() => log.Items.Add(DateTime.Now + ":" + Verse.name + " VERİ EKLENDİ"));
                    }

                    entitydb.SaveChanges();
                }
            }
        }

        private void addsure_Click(object sender, RoutedEventArgs e)
        {
            App.mainTask = Task.Run(() => addSureFunc());
        }

        public Page PageCall()
        {
            return this;
        }

        private void addSection_Click(object sender, RoutedEventArgs e)
        {




            using (var entitydb = new AyetContext())
            {
                string sql = "select * from section;";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                int i = 0;
                while (rdr.Read())
                {
                    var newWe = new Classes.Section();


                    newWe.SectionName = $"{(int)rdr[2]}. ve {(int)rdr[3]} ayetler";
                    newWe.SureId = (int)rdr[1];
                    newWe.startVerse = (int)rdr[2];
                    newWe.endVerse = (int)rdr[3];
                    newWe.SectionNumber = (int)rdr[4];
                    newWe.SectionDescription = "";
                    newWe.SectionDetail = "";
                    newWe.IsMark = false;


                    entitydb.Sections.Add(newWe);

                    this.Dispatcher.Invoke(() => log.Items.Add(i + " -> " + DateTime.Now + ":" + newWe.SectionId + " section id VERİ EKLENDİ"));
                    this.Dispatcher.Invoke(() => log.SelectedIndex = i);
                    i++;
                }

                entitydb.SaveChanges();
            }





        }
        private void addwordFunc()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    string sql = "select * from word;";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    int i = 0;
                    while (rdr.Read())
                    {
                        var newWe = new Words();

                        newWe.sureId = (int)rdr[1];
                        newWe.verseId = (int)rdr[2];
                        newWe.arp_read = (string)rdr[3];
                        newWe.tr_read = (string)rdr[4];
                        newWe.word_meal = (string)rdr[5];
                        newWe.root = (string)rdr[6];

                        entitydb.Words.Add(newWe);

                        this.Dispatcher.Invoke(() => log.Items.Add(i + " -> " + DateTime.Now + ":" + newWe.sureId + " sure id VERİ EKLENDİ"));
                        this.Dispatcher.Invoke(() => log.SelectedIndex = i);
                        i++;
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception err)
            {
                Debug.Write(err.Message);
            }
        }

        private void addwords_Click(object sender, RoutedEventArgs e)
        {
            App.mainTask = Task.Run(() => addwordFunc());
        }

        private void addVerseFunc()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    if (entitydb.Verse.Count() > 0)
                    {
                        MessageBox.Show("Daha Önceden İşlem Yapılmış");
                    }
                    else
                    {
                        string sql = "select * from ayetler;";
                        MySqlCommand cmd = new MySqlCommand(sql, connection);
                        MySqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            Debug.WriteLine(rdr[1]);

                            var newVe = new Verse();

                            int ch = (int)rdr[2];
                            ch++;

                            newVe.sureId = (int)rdr[0];
                            newVe.verseId = (int)rdr[1];
                            newVe.relativeDesk = ch;

                            newVe.verseArabic = (string)rdr[4];
                            newVe.verseTr = (string)rdr[5];
                            newVe.verseDesc = (string)rdr[6];
                            newVe.verseCheck = false;
                            newVe.remiderCheck = false;
                            newVe.markCheck = false;

                            entitydb.Verse.Add(newVe);

                            this.Dispatcher.Invoke(() => log.Items.Add(" -> " + DateTime.Now + ":" + newVe.sureId + " verse id VERİ EKLENDİ"));
                        }

                        entitydb.SaveChanges();
                    }
                }
            }
            catch (Exception err)
            {
                Debug.Write(err.Message);
            }
        }

        private void addverse_Click(object sender, RoutedEventArgs e)
        {
            App.mainTask = Task.Run(() => addVerseFunc());
        }

        private void addInterFunc()
        {
            using (var entitydb = new AyetContext())
            {
                string sql = @"select * from interpreter where interpreter_writer='Ömer Çelik'";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                int i = 1;
                int s = 1;
                while (rdr.Read())
                {
                    var newIn = new Interpreter();

                    if (s != (int)rdr[4])
                    {
                        s++;
                        i = 1;
                    }

                    newIn.interpreterWriter = (string)rdr[2];
                    newIn.interpreterDetail = (string)rdr[3];
                    newIn.sureId = (int)rdr[4];
                    newIn.verseId = i;
                    i++;

                    entitydb.Interpreter.Add(newIn);
                    this.Dispatcher.Invoke(() => Debug.WriteLine(" -> " + DateTime.Now + ":" + newIn.interpreterWriter + " verse id VERİ EKLENDİ"));
                }

                entitydb.SaveChanges();
            }
        }

        private void addInterpreter_Click(object sender, RoutedEventArgs e)
        {
            App.mainTask = Task.Run(() => addInterFunc());
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var conn = new SqliteConnection(@"Data Source=C:\Users\furka\AppData\Roaming\KuranSunnetullah\ayet.db");
            conn.Open();

            var command = conn.CreateCommand();
            command.CommandText = "PRAGMA key = meltdown;";
            command.ExecuteNonQuery();
        }

        private void verseClassCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    string sql = "select * from ayetler;";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Debug.WriteLine(rdr[1]);

                        var newVe = new VerseClass();

                        int ch = (int)rdr[2];
                        ch++;

                        newVe.sureId = (int)rdr[0];
                        newVe.relativeDesk = ch;
                        newVe.v_hk = true;
                        newVe.v_tv = false;
                        newVe.v_cz = false;
                        newVe.v_mk = false;
                        newVe.v_du = false;
                        newVe.v_hr = false;
                        newVe.v_sn = false;
                        entitydb.VerseClass.Add(newVe);

                        this.Dispatcher.Invoke(() => log.Items.Add(" -> " + DateTime.Now + ":" + newVe.sureId + " verse id VERİ EKLENDİ"));
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception err)
            {
                Debug.Write(err.Message);
            }
        }
    }
}