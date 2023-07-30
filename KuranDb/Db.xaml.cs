
using System;
using System.Collections.Generic;
using System.IO;
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
using KuranDb.Core;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace KuranDb
{
    /// <summary>
    /// Interaction logic for Db.xaml
    /// </summary>
    public partial class Db : Window
    {

      

        public Db()
        {
            InitializeComponent();

            using(DbContext dbContext = new DbContext())
            {


                var x = dbContext.LoadSqlFile($@"C:\Users\furka\OneDrive\Masaüstü\test.sql");

            }



        }
    }
}
