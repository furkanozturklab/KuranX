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
    }
}