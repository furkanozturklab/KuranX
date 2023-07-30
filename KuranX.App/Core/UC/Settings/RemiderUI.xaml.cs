using KuranX.App.Core.Classes.Tools;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;


namespace KuranX.App.Core.UC.Settings
{
    /// <summary>
    /// Interaction logic for RemiderUI.xaml
    /// </summary>
    public partial class RemiderUI : UserControl
    {
        public RemiderUI()
        {
            InitializeComponent();
            st_remiderCount.Text = App.config.AppSettings.Settings["app_remiderCount"].Value;
            st_remiderRepeartTime.Text = App.config.AppSettings.Settings["app_remiderWaitTime"].Value;
            st_remiderTime.Text = App.config.AppSettings.Settings["app_remiderTime"].Value;
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

        public bool saveAction()
        {

            if (int.Parse(st_remiderTime.Text) > 0 && int.Parse(st_remiderTime.Text) <= 240 && Tools.IsNumeric(st_remiderTime.Text))
            {
                if (int.Parse(st_remiderRepeartTime.Text) > 0 && int.Parse(st_remiderRepeartTime.Text) <= 3600 && Tools.IsNumeric(st_remiderRepeartTime.Text))
                {
                    if (int.Parse(st_remiderCount.Text) > 0 && int.Parse(st_remiderCount.Text) <= 30 && Tools.IsNumeric(st_remiderCount.Text))
                    {

                        App.config.AppSettings.Settings["app_remiderTime"].Value = st_remiderTime.Text;
                        App.config.AppSettings.Settings["app_remiderWaitTime"].Value = st_remiderRepeartTime.Text;
                        App.config.AppSettings.Settings["app_remiderCount"].Value = st_remiderCount.Text;
                        App.config.Save(ConfigurationSaveMode.Modified);
                        return true;

                    }
                    else
                    {
                        if (int.Parse(st_remiderCount.Text) > 3600) st_remiderCountErr.Content = "Maksimum tekrarlama sayısı aşıldı Max:30";
                        st_remiderCount.Focus();
                        return false;
                    }
                }
                else
                {
                    if (int.Parse(st_remiderRepeartTime.Text) > 3600) st_remiderRepeartTimeErr.Content = "3600 sn den uzun değerler kabul edilmez.";
                    st_remiderRepeartTime.Focus();
                    return false;
                }
            }
            else
            {
                if (int.Parse(st_remiderTime.Text) > 240) st_remiderTimeErr.Content = "240 sn den uzun değerler kabul edilmez.";
                st_remiderTime.Focus();
                return false;
            }
        }
    }
}
