using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KuranDb
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        public static string applicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location).Replace("Recovery", "") + "KuranSunnetullah.exe";

        public void App_Startup(object sender, StartupEventArgs e)
        {
            Db db = new Db();
            db.Show();
        }   

    }
}
