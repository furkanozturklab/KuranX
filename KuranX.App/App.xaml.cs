using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace KuranX.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Frame? mainframe;
        public static string currentDesktype = "DeskLanding", currentLanding = "Hepsi", lastLocation = "App";
        public static int[] currentVersesPageD = new int[2];
        public static TextBlock locationTxt;
        public static bool selectedBlock = true;
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static void logWriter(string type, Exception exe)
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
        }

        public static void apploadsystem()
        {
            if (config.AppSettings.Settings["TaskLastUpdate"].Value != DateTime.Now.ToString("d"))
            {
                config.AppSettings.Settings["TaskLastUpdate"].Value = DateTime.Now.ToString("d");
                config.AppSettings.Settings["TaskLastStatus"].Value = "UpdateWait";
                ConfigurationManager.RefreshSection("appSettings");
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static void processKiller()
        {
            foreach (var item in Process.GetProcesses())
            {
                if (item.ProcessName == "CefSharp.BrowserSubprocess") item.Kill();
            }
        }
    }
}