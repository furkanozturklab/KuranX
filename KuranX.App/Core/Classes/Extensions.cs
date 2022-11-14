using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KuranX.App.Core.Classes
{
    public class Extensions
    {
        public static readonly DependencyProperty DataStorage =
        DependencyProperty.RegisterAttached("DataStorage", typeof(string), typeof(Extensions), new PropertyMetadata(default(string)));

        public static void SetDataStorage(UIElement element, string value)
        {
            element.SetValue(DataStorage, value);
        }

        public static string GetDataStorage(UIElement element)
        {
            return (string)element.GetValue(DataStorage);
        }

        public static readonly DependencyProperty ExtendsStatus =
        DependencyProperty.RegisterAttached("ExtendsStatus", typeof(bool), typeof(Extensions), new PropertyMetadata(default(bool)));

        public static void SetExtendsStatus(UIElement element, bool value)
        {
            element.SetValue(ExtendsStatus, value);
        }

        public static bool GetExtendsStatus(UIElement element)
        {
            return (bool)element.GetValue(ExtendsStatus);
        }

        public static readonly DependencyProperty BaseColor =
        DependencyProperty.RegisterAttached("BaseColor", typeof(string), typeof(Extensions), new PropertyMetadata(default(string)));

        public static void SetBaseColor(UIElement element, string value)
        {
            element.SetValue(BaseColor, value);
        }

        public static string GetBaseColor(UIElement element)
        {
            return (string)element.GetValue(BaseColor);
        }

        public static readonly DependencyProperty SecondColor =
        DependencyProperty.RegisterAttached("SecondColor", typeof(string), typeof(Extensions), new PropertyMetadata(default(string)));

        public static void SetSecondColor(UIElement element, string value)
        {
            element.SetValue(SecondColor, value);
        }

        public static string GetSecondColor(UIElement element)
        {
            return (string)element.GetValue(SecondColor);
        }
    }
}