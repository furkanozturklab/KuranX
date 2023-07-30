using System;
using System.Windows;
using System.Windows.Controls;


namespace KuranX.App.Core.UC.Settings
{
    /// <summary>
    /// Interaction logic for ChangeButton.xaml
    /// </summary>
    public partial class ChangeButton : UserControl
    {
        public event EventHandler<string> changeType;

        public ChangeButton()
        {
            InitializeComponent();

        }


        public void settingPageChange_click(object sender, RoutedEventArgs e)
        {

            foreach (UIElement child in controlGrid.Children)
            {
                if (child is CheckBox chk)
                {
                    chk.IsChecked = false;
                }
            }

            var schk = sender as CheckBox;
            schk!.IsChecked = true;
            changeType?.Invoke(this, schk!.Uid);

        }
    }
}
