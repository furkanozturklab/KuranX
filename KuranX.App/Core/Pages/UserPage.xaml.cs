using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KuranX.App.Core.Pages
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                App.logWriter("InitializeComponent", ex);
            }
        }

        public Page PageCall()
        {
            try
            {
                App.mainScreen.navigationWriter("home", "Profil");
                App.loadTask = Task.Run(() => loadItem());

                return this;
            }
            catch (Exception ex)
            {
                App.logWriter("Loading", ex);
                return this;
            }
        }

        private void loadItem()
        {
            try
            {
            }
            catch (Exception ex)
            {
                App.logWriter("", ex);
            }
            using (var entitydb = new AyetContext())
            {
                var dUser = entitydb.Users.FirstOrDefault();

                this.Dispatcher.Invoke(() =>
                {
                    hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(dUser.avatarUrl);
                    App.mainScreen.hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(dUser.avatarUrl);

                    firstName.Text = dUser.firstName;
                    lastName.Text = dUser.lastName;
                    email.Text = dUser.email;
                    phone.Text = dUser.phone;
                    screetQuestionAnswer.Text = dUser.screetQuestionAnswer;
                    screetQuestion.Text = dUser.screetQuestion;
                    city.Text = dUser.city;
                    country.Text = dUser.country;

                    showLocation.Text = dUser.city + "/" + dUser.country;
                    showName.Text = dUser.firstName + " " + dUser.lastName;
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                App.logWriter("", ex);
            }
            popup_editImage.IsOpen = true;
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);
                popuptemp.IsOpen = false;
                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }

        private void changeImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;

                using (var entitydb = new AyetContext())
                {
                    entitydb.Users.First().avatarUrl = btntemp.Tag.ToString();
                    entitydb.SaveChanges();

                    hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(btntemp.Tag);
                    App.mainScreen.hmwd_profileImageBrush.ImageSource = (ImageSource)FindResource(btntemp.Tag);
                    App.mainScreen.succsessFunc("İşlem Başarılı", "Profil resminiz başarılı bir sekilde güncellenmiştir.", 3);
                    popup_editImage.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dUser = entitydb.Users.FirstOrDefault();

                    dUser.firstName = firstName.Text;
                    dUser.lastName = lastName.Text;
                    dUser.email = email.Text;
                    dUser.phone = phone.Text;
                    dUser.screetQuestionAnswer = screetQuestionAnswer.Text;
                    dUser.screetQuestion = screetQuestion.Text;
                    dUser.city = city.Text;
                    dUser.country = country.Text;

                    entitydb.SaveChanges();

                    App.mainScreen.hmwd_profileName.Text = dUser.firstName + " " + dUser.lastName;
                    showLocation.Text = dUser.city + "/" + dUser.country;
                    showName.Text = dUser.firstName + " " + dUser.lastName;
                    App.mainScreen.succsessFunc("İşlem Başarılı", "Profil bilgileriniz başarılı bir sekilde güncellenmiştir.", 3);
                }
            }
            catch (Exception ex)
            {
                App.logWriter("", ex);
            }
        }
    }
}