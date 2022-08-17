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
                Debug.WriteLine(err.Message.ToString());
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
                        var Verse = new Verses();

                        Verse.versesId = (int)rdr[0];
                        Verse.Name = (string)rdr[1];
                        Verse.NumberOfVerses = (int)rdr[2];
                        Verse.DeskLanding = (int)rdr[3];
                        Verse.DeskMushaf = (int)rdr[4];
                        Verse.LandingLocation = (string)rdr[5];
                        Verse.Description = (string)rdr[6];
                        Verse.Status = "#ADB5BD";

                        entitydb.Verses.Add(Verse);

                        Debug.WriteLine("VERİ EKLENDİ");
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message.ToString());
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

                        newVe.SureId = (int)rdr[0];
                        newVe.VerseId = (int)rdr[1];
                        newVe.RelativeDesk = (int)rdr[2];
                        newVe.VerseDesk = (int)rdr[3];
                        newVe.VerseArabic = (string)rdr[4];
                        newVe.VerseTr = (string)rdr[5];
                        newVe.VerseDesc = (string)rdr[6];
                        newVe.VerseCheck = false;

                        entitydb.Verse.Add(newVe);

                        Debug.WriteLine("VERİ EKLENDİ");
                    }

                    entitydb.SaveChanges();
                }
            }
            catch (Exception err)
            {
            }
        }

        private void test_click(object sender, RoutedEventArgs e)
        {
            List<Verses> VersesList;

            land = "Mekke";
            DeskType = "Mushaf Sırası";

            using (var entitydb = new AyetContext())
            {
                if (land != "Hepsi")
                {
                    if (DeskType == "İniş Sırası")
                    {
                        VersesList = (List<Verses>)entitydb.Verses.Where(p => p.LandingLocation == land).OrderBy(p => p.DeskLanding).ToList();
                    }
                    else
                    {
                        VersesList = (List<Verses>)entitydb.Verses.Where(p => p.LandingLocation == land).OrderBy(p => p.DeskMushaf).ToList();
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
                        VersesList = (List<Verses>)entitydb.Verses.OrderBy(p => p.DeskLanding).ToList();
                    }
                    else
                    {
                        VersesList = (List<Verses>)entitydb.Verses.OrderBy(p => p.DeskMushaf).ToList();
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