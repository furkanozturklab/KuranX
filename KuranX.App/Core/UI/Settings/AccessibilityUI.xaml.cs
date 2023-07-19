using KuranX.App.Core.Classes.Tools;
using KuranX.App.Core.Classes.UI;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KuranX.App.Core.UI.Settings
{
    /// <summary>
    /// Interaction logic for AccessibilityUI.xaml
    /// </summary>
    public partial class AccessibilityUI : UserControl
    {
        public ObservableCollection<ComboboxColorsUI> Colors { get; set; }

        public AccessibilityUI()
        {
            InitializeComponent();
            st_font_size.Text = App.config.AppSettings.Settings["app_arabicFontSize"].Value;
            st_fontEx_size.Text = App.config.AppSettings.Settings["app_arabicFontExSize"].Value;

            Colors = new ObservableCollection<ComboboxColorsUI>
            {
                new ComboboxColorsUI { ColorName = "Beyaz", ColorBrush = "#FFFFFF" , ColorIndex = 0 },
                new ComboboxColorsUI { ColorName = "Hafif Krem", ColorBrush = "#FFF231", ColorIndex = 1 },
                new ComboboxColorsUI { ColorName = "Açık Gri", ColorBrush = "#F5F5F5", ColorIndex = 2 },
                new ComboboxColorsUI { ColorName = "Açık Bej", ColorBrush = "#F5F5DC" , ColorIndex =3 },
                new ComboboxColorsUI { ColorName = "Pudra Mavisi", ColorBrush = "#B0E0E6", ColorIndex =4  },
                new ComboboxColorsUI { ColorName = "Açık Yeşil", ColorBrush = "#F0FFF0", ColorIndex = 5 },
                new ComboboxColorsUI { ColorName = "Diğer 1", ColorBrush = "#f7fce3" , ColorIndex =6 },
                new ComboboxColorsUI { ColorName = "Diğer 2", ColorBrush = "#F4F4E0" , ColorIndex =7 },
                new ComboboxColorsUI { ColorName = "Diğer 3", ColorBrush = "#fffde1", ColorIndex = 8 },

                // Diğer renkleri burada ekleyebilirsiniz.
            };
            DataContext = this;

            selectedColor(Colors, App.config.AppSettings.Settings["app_readBackColor"].Value);
            


        }


        private void selectedColor(ObservableCollection<ComboboxColorsUI> Colors,string selColor)
        {
            Debug.WriteLine("selc colr");

        
            var selIndex = 0;
            foreach (var item in Colors)
            {

                Debug.WriteLine($"Colors -> {item.ColorName} || selcColor -> {selColor}");
                if (item.ColorBrush == selColor) st_color.SelectedIndex = item.ColorIndex;
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

        public bool saveAction()
        {
            if (int.Parse(st_font_size.Text) > 10 && int.Parse(st_font_size.Text) <= 128 && Tools.IsNumeric(st_font_size.Text))
            {
                if (int.Parse(st_fontEx_size.Text) > 10 && int.Parse(st_fontEx_size.Text) <= 128 && Tools.IsNumeric(st_fontEx_size.Text))
                {
                    var font = st_font.SelectedItem as ComboBoxItem;
                    var lang = st_langues.SelectedItem as ComboBoxItem;
                    var color = st_color.SelectedItem as ComboboxColorsUI;
                    if (font != null)
                    {
                        switch (font.Content)
                        {
                            case "XBZar":
                                App.config.AppSettings.Settings["app_arabicFont"].Value = "XBZar";
                                Loading.loadFont("XBZar");
                                break;

                            case "KFGQPC":
                                App.config.AppSettings.Settings["app_arabicFont"].Value = "KFGQPC";
                                Loading.loadFont("KFGQPC");
                                break;

                            case "MeQuran":
                                App.config.AppSettings.Settings["app_arabicFont"].Value = "MeQuran";
                                Loading.loadFont("MeQuran");
                                break;

                            case "SaleemQuran":
                                App.config.AppSettings.Settings["app_arabicFont"].Value = "SaleemQuran";
                                Loading.loadFont("SaleemQuran");
                                break;

                            case "ScheherazadeNew":
                                App.config.AppSettings.Settings["app_arabicFont"].Value = "ScheherazadeNew";
                                Loading.loadFont("ScheherazadeNew");
                                break;
                        }
                    }
                    if (lang != null)
                    {
                        switch (lang.Content)
                        {
                            case "tr-TR":
                                App.config.AppSettings.Settings["app_langues"].Value = "tr-TR";
                                break;
                        }
                    }

                    if (color != null)
                    {
                        string colorVal = color.ColorBrush;
                        Application.Current.Resources["readPanelColors"] = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorVal)!);
                        App.config.AppSettings.Settings["app_readBackColor"].Value = colorVal;
                    }

                    App.config.AppSettings.Settings["app_arabicFontSize"].Value = st_font_size.Text;
                    ResourceHelper.ArabicFontSize = double.Parse(st_font_size.Text);
                    App.config.AppSettings.Settings["app_arabicFontExSize"].Value = st_fontEx_size.Text;
                    ResourceHelper.ArabicFontSizeExtended = double.Parse(st_fontEx_size.Text);
                    App.config.Save(ConfigurationSaveMode.Modified);
                    return true;
                }
                else
                {
                    if (!Tools.IsNumeric(st_fontEx_size.Text)) st_fontExsizeErr.Content = "Lütfen sayısal bir değer giriniz.";
                    else st_fontsizeErr.Content = "Lütfen 0 dan büyük bir değer giriniz.";

                    if (int.Parse(st_fontEx_size.Text) > 128) st_fontExsizeErr.Content = "Maksimum üst sınırı geçtiniz Max:10000";
                    st_fontEx_size.Focus();

                    return false;
                }
            }
            else
            {
                if (!Tools.IsNumeric(st_font_size.Text)) st_fontsizeErr.Content = "Lütfen sayısal bir değer giriniz.";
                else st_fontsizeErr.Content = "Lütfen 0 dan büyük bir değer giriniz.";

                if (int.Parse(st_font_size.Text) > 128) st_fontsizeErr.Content = "Maksimum üst sınırı geçtiniz Max:10000";
                st_font_size.Focus();

                return false;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (App.config.AppSettings.Settings["app_arabicFont"].Value)
            {
                case "XBZar":
                    st_font.SelectedIndex = 0;
                    break;

                case "KFGQPC":
                    st_font.SelectedIndex = 1;
                    break;

                case "MeQuran":
                    st_font.SelectedIndex = 2;
                    break;

                case "SaleemQuran":
                    st_font.SelectedIndex = 3;
                    break;

                case "ScheherazadeNew":
                    st_font.SelectedIndex = 4;
                    break;
            }

            st_langues.SelectedIndex = 0;
        }
    }
}