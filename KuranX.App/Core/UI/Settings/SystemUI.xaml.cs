using KuranX.App.Core.Classes.Tools;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace KuranX.App.Core.UI.Settings
{
    /// <summary>
    /// Interaction logic for SystemUI.xaml
    /// </summary>
    public partial class SystemUI : UserControl
    {
        public event EventHandler<string> TestMy;

        public SystemUI()
        {
            InitializeComponent();
            st_aniSecond.Text = App.config.AppSettings.Settings["app_animationSpeed"].Value;
            st_warningSecond.Text = App.config.AppSettings.Settings["app_warningShowTime"].Value;
            if (App.config.AppSettings.Settings["user_autoLogin"].Value == "false") st_start.SelectedIndex = 0;
            else st_start.SelectedIndex = 1;
        }

        public bool saveAction()
        {
            if (int.Parse(st_aniSecond.Text) > 0 && int.Parse(st_aniSecond.Text) <= 10000 && Tools.IsNumeric(st_aniSecond.Text))
            {
                if (int.Parse(st_warningSecond.Text) > 0 && int.Parse(st_warningSecond.Text) <= 30 && Tools.IsNumeric(st_warningSecond.Text))
                {
                    var item = st_start.SelectedItem as ComboBoxItem;
                    if (item != null)
                    {
                        switch (item.Tag)
                        {
                            case "false":
                                App.config.AppSettings.Settings["user_autoLogin"].Value = "false";
                                break;

                            case "true":
                                App.config.AppSettings.Settings["user_autoLogin"].Value = "true";
                                break;
                        }

                        App.config.AppSettings.Settings["app_animationSpeed"].Value = st_aniSecond.Text;
                        App.config.AppSettings.Settings["app_warningShowTime"].Value = st_warningSecond.Text;
                        App.config.Save(ConfigurationSaveMode.Modified);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (int.Parse(st_warningSecond.Text) > 30) st_warningSecondErr.Content = "30 sn den uzun değerler kabul edilmez.";
                    st_warningSecond.Focus();
                    return false;
                }
            }
            else
            {
                if (!Tools.IsNumeric(st_aniSecond.Text)) st_aniSecondErr.Content = "Lütfen sayısal bir değer giriniz.";
                else st_aniSecondErr.Content = "Lütfen 0 dan büyük bir değer giriniz.";

                if (int.Parse(st_aniSecond.Text) > 10000) st_aniSecondErr.Content = "Maksimum üst sınırı geçtiniz Max:10000";
                st_aniSecond.Focus();
                return false;
            }
        }

        private void st_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void st_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.SelectionStart = textBox.Text.Length;
        }
    }
}