using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KuranX.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Frame? mainframe;
        public static string currentDesktype = "DeskLanding";
        public static int[] currentVersesPageD = new int[2];

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
    }
}