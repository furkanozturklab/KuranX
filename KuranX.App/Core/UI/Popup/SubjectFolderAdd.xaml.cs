using KuranX.App.Core.Classes;
using KuranX.App.Core.Classes.Tools;
using KuranX.App.Core.Pages.SubjectF;
using KuranX.App.Core.Pages.VerseF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuranX.App.Core.UI.Popup
{
    /// <summary>
    /// Interaction logic for SubjectFolderAdd.xaml
    /// </summary>
    public partial class SubjectFolderAdd : UserControl
    {

        SubjectFrame useFrame;
        public SubjectFolderAdd()
        {
            InitializeComponent();

        }


        public void parentFind()
        {

            DependencyObject parent = this.Parent;
            while (parent != null)
            {
                if (parent is SubjectFrame subjectFrame)
                {
                    useFrame = parent as SubjectFrame;
                    break;
                }

                parent = LogicalTreeHelper.GetParent(parent);
            }
        }


        private void addfolderSubject_Click(object sender, RoutedEventArgs e)
        {
            parentFind();
            try
            {
                Tools.errWrite($"[{DateTime.Now} addfolderSubject_Click ] -> SubjectFrame");

                if (subjectFolderHeader.Text.Length >= 3)
                {
                    if (subjectFolderHeader.Text.Length < 150)
                    {
                        using (var entitydb = new AyetContext())
                        {
                            var dControl = entitydb.Subject.Where(p => p.subjectName == CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subjectpreviewName.Text)).ToList();

                            if (dControl.Count == 0)
                            {
                                var dSubjectFolder = new Subject { subjectName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subjectpreviewName.Text), subjectColor = subjectpreviewColor.Background.ToString(), created = DateTime.Now, modify = DateTime.Now };
                                entitydb.Subject.Add(dSubjectFolder);
                                entitydb.SaveChanges();
                                App.mainScreen.succsessFunc("İşlem Başarılı", " Yeni konu başlığı başarılı bir sekilde oluşturuldu artık ayetleri ekleye bilirsiniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));

                                subjectpreviewName.Text = "";
                                subjectFolderHeader.Text = "";
                                useFrame.drag.Dispose();
                                useFrame.drag = null;
                                useFrame.popup_FolderSubjectPopup.IsOpen = false;

                                dSubjectFolder = null;

                                Task.Run(() => useFrame.loadItem());
                            }
                            else
                            {
                                App.mainScreen.alertFunc("İşlem Başarısız", " Daha önce aynı isimde bir konu zaten mevcut lütfen konu başlığınızı kontrol ediniz.", int.Parse(App.config.AppSettings.Settings["app_warningShowTime"].Value));
                            }
                            dControl = null;
                        }
                    }
                    else
                    {
                        subjectFolderHeader.Focus();
                        subjectHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                        subjectHeaderFolderErrorMesssage.Content = "Konu başlığının çok uzun max 150 karakter olabilir";
                    }
                }
                else
                {
                    subjectFolderHeader.Focus();
                    subjectHeaderFolderErrorMesssage.Visibility = Visibility.Visible;
                    subjectHeaderFolderErrorMesssage.Content = "Konu başlığının uzunluğu minimum 3 karakter olmalı";
                }
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }

        }

        public void close_Click(object sender, RoutedEventArgs e)
        {
            parentFind();
           
            subjectHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            subjectpreviewName.Text = "Önizleme";
            subjectFolderHeader.Text = "";
            useFrame.popupClosed_Click(sender, e);

        }


        private void subjectColorPick_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Tools.errWrite($"[{DateTime.Now} subjectColorPick_Click ] -> SubjectFrame");


                CheckBox? chk;

                foreach (object item in subjectColorStack.Children)
                {
                    chk = null;
                    if (item is FrameworkElement)
                    {
                        chk = ((CheckBox?)(item as FrameworkElement));

                        chk.IsChecked = false;
                    }
                }

                chk = sender as CheckBox;

                chk.IsChecked = true;

                subjectpreviewColor.Background = new BrushConverter().ConvertFromString((string)chk.Tag) as SolidColorBrush;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Click", ex);
            }
        }



        private void subjectFolderHeader_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                subjectHeaderFolderErrorMesssage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void subjectFolderHeader_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                subjectpreviewName.Text = subjectFolderHeader.Text;
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }

        private void subjectFolderHeader_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9a-zA-Z-ğüşöçıİĞÜŞÖÇ?.*()']");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                Tools.logWriter("Change", ex);
            }
        }
    }
}
