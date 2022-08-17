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

namespace KuranX.App.Core.Pages
{
    /// <summary>
    /// Interaction logic for registerFrame.xaml
    /// </summary>
    public partial class registerFrame : Page
    {
        public Boolean termofuseCheck = false;
        public bool passcheck, passrecheck;

        public registerFrame()
        {
            InitializeComponent();
        }

        private void userEmailHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rgstUserEmail.Focus();
        }

        private void userEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(rgstUserEmail.Text) && rgstUserEmail.Text.Length > 0)
            {
                rgstUserEmailHint.Visibility = Visibility.Collapsed;
            }
            else
            {
                rgstUserEmailHint.Visibility = Visibility.Visible;
            }
        }

        private void userPswHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rgstUserPsw.Focus();
        }

        private void userPsw_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (rgstUserPsw.Password.Length >= 8)
            {
                passwordReturnMessage.Visibility = Visibility.Hidden;
                passcheck = true;
            }
            else
            {
                passwordReturnMessage.Text = "* Yeterince Uzun Değil Min 8 Karakter Olmalıdır Şuan " + rgstUserPsw.Password.Length;
                passwordReturnMessage.Visibility = Visibility.Visible;
                passcheck = false;
            }

            if (!string.IsNullOrEmpty(rgstUserPsw.Password) && rgstUserPsw.Password.Length > 0)
            {
                rgstUserPswHint.Visibility = Visibility.Collapsed;
            }
            else
            {
                rgstUserPswHint.Visibility = Visibility.Visible;
            }
        }

        private void userPswHintRe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rgstReUserPsw.Focus();
        }

        private void userPswRe_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (rgstReUserPsw.Password == rgstUserPsw.Password)
            {
                passwordReReturnMessage.Visibility = Visibility.Hidden;
                passrecheck = true;
            }
            else
            {
                passwordReReturnMessage.Text = "* Tekrar Parolanız uyuşmuyor";
                passwordReReturnMessage.Visibility = Visibility.Visible;
                passrecheck = false;
            }

            if (!string.IsNullOrEmpty(rgstReUserPsw.Password) && rgstReUserPsw.Password.Length > 0)
            {
                rgstReUserPswHint.Visibility = Visibility.Collapsed;
            }
            else
            {
                rgstReUserPswHint.Visibility = Visibility.Visible;
            }
        }

        private void termofuseContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (termofuseCheckBox.Background.ToString() == "#FF383C41")
            {
                termofuseCheck = true;
                termofuseCheckBox.Background = new SolidColorBrush(Color.FromRgb(240, 67, 58));
            }
            else if (termofuseCheckBox.Background.ToString() == "#FFF0433A")
            {
                termofuseCheck = false;
                termofuseCheckBox.Background = new SolidColorBrush(Color.FromRgb(56, 60, 65));
            }
        }

        private void loginBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidEmail(rgstUserEmail.Text) != false)
            {
                if (passcheck)
                {
                    if (passrecheck)
                    {
                        if (termofuseCheck)
                        {
                            // All validate true

                            using (var db = new AyetContext())
                            {
                                var User = new User();
                                var checkuser = db.Users.Where(u => u.Email == rgstUserEmail.Text).FirstOrDefault();

                                if (checkuser == null)
                                {
                                    User.Email = rgstUserEmail.Text.ToString();
                                    User.Password = rgstUserPsw.Password.ToString();
                                    User.CreateDate = DateTime.Now;
                                    User.UpdateDate = DateTime.Now;
                                    db.Users.Add(User);
                                    db.SaveChanges();

                                    MessageBox.Show("Kayıt Başarılı Giriş Sayfasına Yönlendiriliyorsunuz...");
                                    this.NavigationService.GoBack();
                                }
                                else
                                {
                                    emailReturnMessage.Text = "* Bu email adresi ile daha önce giriş yapılmıştır.";
                                    emailReturnMessage.Visibility = Visibility.Visible;
                                    rgstUserEmail.Focus();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lütfen kullanım şartlarını kabul ediniz.");
                        }
                    }
                    else
                    {
                        passwordReReturnMessage.Text = "* Lütfen parola kısmını kontrol ediniz.";
                        passwordReReturnMessage.Visibility = Visibility.Visible;
                        rgstReUserPsw.Focus();
                    }
                }
                else
                {
                    passwordReturnMessage.Text = "* Lütfen parola kısmını kontrol ediniz.";
                    passwordReturnMessage.Visibility = Visibility.Visible;
                    rgstUserPsw.Focus();
                }
            }
            else
            {
                emailReturnMessage.Text = "* Geçerli bir email adresi giriniz.";
                emailReturnMessage.Visibility = Visibility.Visible;
                rgstUserEmail.Focus();
            }
        }

        private void rgstUserEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            emailReturnMessage.Text = "";
            emailReturnMessage.Visibility = Visibility.Hidden;
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
    }
}