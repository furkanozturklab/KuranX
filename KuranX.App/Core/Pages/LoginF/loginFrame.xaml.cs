using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using KuranX.App.Core.Classes;
using KuranX.App.Core.Windows;

namespace KuranX.App.Core.Pages.LoginF
{
    /// <summary>
    /// Interaction logic for loginFrame.xaml
    /// </summary>
    public partial class loginFrame : Page
    {
        public Boolean rememberMeCheck = false;

        public loginFrame()
        {
            InitializeComponent();
        }

        private void userEmailHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            userEmail.Focus();
        }

        private void userEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(userEmail.Text) && userEmail.Text.Length > 0)
            {
                userEmailHint.Visibility = Visibility.Collapsed;
            }
            else
            {
                userEmailHint.Visibility = Visibility.Visible;
            }
        }

        private void userPswHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            userPsw.Focus();
        }

        private void userPsw_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(userPsw.Password) && userPsw.Password.Length > 0)
            {
                userPswHint.Visibility = Visibility.Collapsed;
            }
            else
            {
                userPswHint.Visibility = Visibility.Visible;
            }
        }

        private void remmeberMeContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rememberCheckBox.Background.ToString() == "#FF383C41")
            {
                rememberMeCheck = true;
                rememberCheckBox.Background = new SolidColorBrush(Color.FromRgb(240, 67, 58));
            }
            else if (rememberCheckBox.Background.ToString() == "#FFF0433A")
            {
                rememberMeCheck = false;
                rememberCheckBox.Background = new SolidColorBrush(Color.FromRgb(56, 60, 65));
            }
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new registerFrame());
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidEmail(userEmail.Text))
            {
                if (userPsw.Password.Length >= 8)
                {
                    // All validate true

                    using (var db = new AyetContext())
                    {
                        var checkuser = db.Users.Where(u => u.Email == userEmail.Text).FirstOrDefault();

                        if (checkuser == null)
                        {
                            loginEmailReturnMessage.Text = "* Böyler bir kullanıcı mevcut değil";
                            loginEmailReturnMessage.Visibility = Visibility.Visible;
                            userEmail.Focus();
                        }
                        else
                        {
                            if (checkuser.Password == userPsw.Password)
                            {
                                homeScreen homeScreen = new homeScreen();
                                homeScreen.Show();
                                Window win = Window.GetWindow(this);
                                win.Close();
                            }
                            else
                            {
                                loginPswReturnMessage.Text = "* Lütfen giriş bilgilerinizi kontrol ediniz.";
                                loginPswReturnMessage.Visibility = Visibility.Visible;
                                userPsw.Focus();
                            }
                        }
                    }
                }
                else
                {
                    loginPswReturnMessage.Text = "* Lütfen parolanızı giriniz.";
                    loginPswReturnMessage.Visibility = Visibility.Visible;
                    userPsw.Focus();
                }
            }
            else
            {
                loginEmailReturnMessage.Text = "* Geçerli bir email adresi giriniz.";
                loginEmailReturnMessage.Visibility = Visibility.Visible;
                userEmail.Focus();
            }
        }

        public bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private void userPsw_LostFocus(object sender, RoutedEventArgs e)
        {
            loginPswReturnMessage.Text = "";
            loginPswReturnMessage.Visibility = Visibility.Hidden;
        }

        private void userEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            loginEmailReturnMessage.Text = "";
            loginEmailReturnMessage.Visibility = Visibility.Hidden;
        }
    }
}