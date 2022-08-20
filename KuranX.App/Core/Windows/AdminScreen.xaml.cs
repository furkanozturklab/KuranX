using KuranX.App.Core.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
        private string DeskType;

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
                        Verse.Name = (string)rdr[1];
                        Verse.NumberOfVerses = (int)rdr[2];
                        Verse.DeskLanding = (int)rdr[3];
                        Verse.DeskMushaf = (int)rdr[4];
                        Verse.LandingLocation = (string)rdr[5];
                        Verse.Description = (string)rdr[6];
                        Verse.Status = "#ADB5BD";

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

        private void add_verse(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "select * from ayetler;";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                using (var entitydb = new AyetContext())
                {
                    while (rdr.Read())
                    {
                        var newVe = new Verse();

                        int ch = (int)rdr[2];
                        ch++;

                        newVe.SureId = (int)rdr[0];
                        newVe.VerseId = (int)rdr[1];
                        newVe.RelativeDesk = ch;
                        newVe.Status = "#FFFFFF";
                        newVe.VerseArabic = (string)rdr[4];
                        newVe.VerseTr = (string)rdr[5];
                        newVe.VerseDesc = (string)rdr[6];
                        newVe.VerseCheck = "false";

                        entitydb.Verse.Add(newVe);

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
                        VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == land).OrderBy(p => p.DeskLanding).ToList();
                    }
                    else
                    {
                        VersesList = (List<Sure>)entitydb.Sure.Where(p => p.LandingLocation == land).OrderBy(p => p.DeskMushaf).ToList();
                    }

                    foreach (var item in VersesList)
                    {
                        Debug.WriteLine(item.Name);
                    }
                }
                else
                {
                    if (DeskType == "İniş Sırası")
                    {
                        VersesList = (List<Sure>)entitydb.Sure.OrderBy(p => p.DeskLanding).ToList();
                    }
                    else
                    {
                        VersesList = (List<Sure>)entitydb.Sure.OrderBy(p => p.DeskMushaf).ToList();
                    }

                    foreach (var item in VersesList)
                    {
                        Debug.WriteLine(item.Name);
                    }
                }
            }
        }
    }
}