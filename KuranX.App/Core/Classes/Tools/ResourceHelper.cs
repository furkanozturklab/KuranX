using System;

using System.Windows;

using System.Windows.Media;

namespace KuranX.App.Core.Classes.Tools
{
    public static class ResourceHelper
    {

        public static double ArabicFontSize
        {
            get
            {
                if (Application.Current.Resources.Contains("arabicFontSize"))
                {
                    return (double)Application.Current.Resources["arabicFontSize"];
                }

                return 24;
            }
            set
            {
                if (Application.Current.Resources.Contains("arabicFontSize"))
                {
                    Application.Current.Resources["arabicFontSize"] = value;
                }
                else
                {
                    Application.Current.Resources.Add("arabicFontSize", value);
                }
            }
        }

        public static double ArabicFontSizeExtended
        {
            get
            {
                if (Application.Current.Resources.Contains("arabicFontSizeExtended"))
                {
                    return (double)Application.Current.Resources["arabicFontSizeExtended"];
                }

                return 36;
            }
            set
            {
                if (Application.Current.Resources.Contains("arabicFontSizeExtended"))
                {
                    Application.Current.Resources["arabicFontSizeExtended"] = value;
                }
                else
                {
                    Application.Current.Resources.Add("arabicFontSizeExtended", value);
                }
            }
        }




        public static FontFamily ArabicFont
        {
            get
            {
                if (Application.Current.Resources.Contains("arabicFont"))
                {
                    return (FontFamily)Application.Current.Resources["arabicFont"];
                }

                return new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Arabic/XBZar/"), "./#XB Zar");
            }
            set
            {
                if (Application.Current.Resources.Contains("arabicFont"))
                {
                    Application.Current.Resources["arabicFont"] = value;
                }
                else
                {
                    Application.Current.Resources.Add("arabicFont", value);
                }
            }
        }


        public static SolidColorBrush ReadPanelColorsBrush
        {
            get
            {
                if (Application.Current.Resources.Contains("readPanelColors"))
                {
                    return (SolidColorBrush)Application.Current.Resources["readPanelColors"];
                }

                return new SolidColorBrush(Colors.White);
            }
            set
            {
                if (Application.Current.Resources.Contains("readPanelColors"))
                {
                    Application.Current.Resources["readPanelColors"] = value;
                }
                else
                {
                    Application.Current.Resources.Add("readPanelColors", value);
                }
            }
        }



    }
}
