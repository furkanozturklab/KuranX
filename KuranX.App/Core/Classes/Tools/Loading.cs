using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace KuranX.App.Core.Classes.Tools
{
    public class Loading
    {

        public static void changeFont(string fontName)
        {
            App.config.AppSettings.Settings["app_arabicFont"].Value = fontName;
        }


        public static async Task<bool> loadSync()
        {
            try
            {
                List<Uri> loadUriList = new List<Uri>();

                string[] controlConfingValues = new string[19] { "app_version", "app_exe", "app_id", "api_adress", "api_token", "post_apiName", "user_rememberMe", "user_pin", "user_autoLogin", "app_animationSpeed", "app_remiderTime", "app_remiderWaitTime", "app_remiderCount", "app_warningShowTime", "app_arabicFont", "app_arabicFontSize", "app_arabicFontExSize", "app_langues" , "app_write" };
                for (int i = 0; i < 19; i++)
                {

                    if (!await configControl(controlConfingValues[i]))
                    {
                        Application.Current.Shutdown();
                        return false;
                    }

                }

                loadFont(App.config.AppSettings.Settings["app_arabicFont"].Value);


                loadUriList.Add(await loadLangues(App.config.AppSettings.Settings["app_langues"].Value));
                await loadResource(loadUriList);

                ResourceHelper.ArabicFontSize = double.Parse(App.config.AppSettings.Settings["app_arabicFontSize"].Value);
                ResourceHelper.ArabicFontSizeExtended = double.Parse(App.config.AppSettings.Settings["app_arabicFontExSize"].Value);
                ResourceHelper.ReadPanelColorsBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(App.config.AppSettings.Settings["app_readBackColor"].Value)!);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }




        }


        public async static Task loadResource(List<Uri> resource, string type = "")
        {
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    ResourceDictionary dictionary = new ResourceDictionary();
                    dictionary.Source = item;
                    if (item.ToString().Contains("Langues"))
                    {
                        App.resourceLang = (ResourceDictionary)Application.LoadComponent(item);
                        Debug.WriteLine($"item : {item}");
                    }
                    Application.Current.Resources.MergedDictionaries.Add(dictionary);
                }
            }
        }



        public async static Task<bool> configControl(string name)
        {

            if (App.config != null && App.config.AppSettings.Settings[name] != null && App.config.AppSettings.Settings[name].Value != null) return true;
            else
            {
                MessageBox.Show("Confing dosyanızda sorun var lütfen yetkili kişilerle iletişime geçiniz...");
                return false;
            }



        }

        public static void loadFont(string selectedFont)
        {
            switch (selectedFont)
            {
                case "XBZar":
                    ResourceHelper.ArabicFont = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Arabic/XBZar/"), "./#XB Zar");
                    break;
                case "KFGQPC":
                    ResourceHelper.ArabicFont = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Arabic/KFGQPC/"), "./#KFGQPC Uthmanic Script HAFS");
                    break;
                case "MeQuran":
                    ResourceHelper.ArabicFont = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Arabic/MeQuran/"), "./#me_quran");
                    break;
                case "SaleemQuran":
                    ResourceHelper.ArabicFont = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Arabic/SaleemQuran/"), "./#_PDMS_Saleem_QuranFont");
                    break;
                case "ScheherazadeNew":
                    ResourceHelper.ArabicFont = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Arabic/ScheherazadeNew/"), "./#Scheherazade New");
                    break;
                default:
                    ResourceHelper.ArabicFont = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Arabic/XBZar/"), "./#XB Zar");
                    break;
            }
        }

        public async static Task<Uri> loadLangues(string name)
        {
            var resourceDictUri = new Uri($@"..\Resources\Langues\{name}.xaml", UriKind.Relative);
            return resourceDictUri;
        }








    }
}
