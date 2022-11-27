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
using System.Windows.Shapes;
using KuranX.App.Core.Classes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.IO;
using KuranX.App.Core.Pages.AdminF;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        protected string _connectedString = "";
        private MySqlConnection connection;
        private string DbPath { get; set; }

        public AdminPanel()
        {
            InitializeComponent();
        }

        public void wamp_sql()
        {
            _connectedString = "Server=LOCALHOST;Database=kuranx;Uid=root;Pwd=;";
            connection = new MySqlConnection(_connectedString);

            try
            {
                connection.Open();
                this.Dispatcher.Invoke(() =>
                {
                    btnWmp.IsEnabled = false;
                    btnWmpEdit.IsEnabled = true;
                    wampConnectBtn.SetValue(Extensions.DataStorage, "ok");
                });
            }
            catch (Exception err)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Debug.WriteLine(err);
                    btnWmp.IsEnabled = true;
                    btnWmpEdit.IsEnabled = false;
                    wampConnectBtn.SetValue(Extensions.DataStorage, "err");
                });
            }
        }

        public void lite_sql()
        {
            try
            {
                var folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KuranX");
                Directory.CreateDirectory(folder);
                DbPath = System.IO.Path.Combine(folder, "Ayet.db");

                if (File.Exists(DbPath))
                {
                    this.Dispatcher.Invoke(() => sqlConnectBtn.SetValue(Extensions.DataStorage, "ok"));
                }
                else
                {
                    this.Dispatcher.Invoke(() => sqlConnectBtn.SetValue(Extensions.DataStorage, "err"));
                }
            }
            catch (Exception err)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Debug.WriteLine(err);
                    this.Dispatcher.Invoke(() => sqlConnectBtn.SetValue(Extensions.DataStorage, "err"));
                });
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.loadTask = Task.Run(() => wamp_sql());
            App.loadTask = Task.Run(() => lite_sql());
            adminFrame.Content = new sqlEditPage().PageCall();
        }

        private void btnwmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                this.Dispatcher.Invoke(() =>
                {
                    btnWmp.IsEnabled = false;
                    btnWmpEdit.IsEnabled = true;
                    wampConnectBtn.SetValue(Extensions.DataStorage, "ok");
                });
            }
            catch (Exception err)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Debug.WriteLine(err);
                    btnWmp.IsEnabled = true;
                    btnWmpEdit.IsEnabled = false;
                    wampConnectBtn.SetValue(Extensions.DataStorage, "err");
                });
            }
        }

        private void btnEditing_Click(object sender, RoutedEventArgs e)
        {
            adminFrame.Content = new sqlEditPage();
        }

        private void btnWmpEdit_Click(object sender, RoutedEventArgs e)
        {
            adminFrame.Content = new dataTransferPage().PageCall();
        }
    }
}