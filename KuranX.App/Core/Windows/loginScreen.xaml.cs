using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using KuranX.App.Core.Classes;
using KuranX.App.Core.Pages;
using Microsoft.EntityFrameworkCore;

namespace KuranX.App.Core.Windows
{
    /// <summary>
    /// Interaction logic for loginScreen.xaml
    /// </summary>
    public partial class loginScreen : Window
    {
        public bool rememberMeCheck = false;
        public bool termofuseCheck = false;
        private string screetanw = "", pin = "";

        public loginScreen()
        {
            try
            {
                InitializeComponent();
                App.mainTask = Task.Run(() => mainloadFunc());
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        public void mainloadFunc()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dUser = entitydb.Users.FirstOrDefault();
                    if (dUser != null)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            userScreetQuestion.Text = dUser.screetQuestion;
                            pin = dUser.pin;
                            screetanw = dUser.screetQuestionAnw;
                            registerHomePageBtn.Visibility = Visibility.Collapsed;
                            loadinGifContent.Visibility = Visibility.Collapsed;
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            registerHomePageBtn.Visibility = Visibility.Visible;
                            loadinGifContent.Visibility = Visibility.Collapsed;
                        });
                    }
                }

                this.Dispatcher.Invoke(() =>
                {
                    if (App.config.AppSettings.Settings["user_rememberMe"].Value == "true")
                    {
                        rememberCheckBox.Background = new SolidColorBrush(Color.FromRgb(240, 67, 58));
                        userPin.Text = App.config.AppSettings.Settings["user_pin"].Value;
                        if (App.config.AppSettings.Settings["user_autoLogin"].Value == "true") loginBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    }
                });
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void LoginScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void loginFunc()
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    var dUser = entitydb.Users.FirstOrDefault();

                    if (dUser != null)
                    {
                        if (this.Dispatcher.Invoke(() => userPin.Text) == dUser.pin)
                        {
                            if (this.Dispatcher.Invoke(() => rememberCheckBox.Background.ToString()) == "#FFF0433A")
                            {
                                App.config.AppSettings.Settings["user_rememberMe"].Value = "true";
                                App.config.AppSettings.Settings["user_pin"].Value = this.Dispatcher.Invoke(() => userPin.Text);
                                App.config.Save(ConfigurationSaveMode.Modified);
                            }

                            Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));
                            this.Dispatcher.Invoke(() => App.mainScreen.homeNav.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)));
                            this.Dispatcher.Invoke(() => App.mainScreen.Show());
                            this.Dispatcher.Invoke(() => this.Close());
                        }
                        else
                        {
                            Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));
                            this.Dispatcher.Invoke(() =>
                            {
                                loginPinReturnMessage.Text = "* Pin Yanlış Tekrar Deneyin";
                                loginPinReturnMessage.Visibility = Visibility.Visible;
                                loadinGifContent.Visibility = Visibility.Hidden;
                                userPin.Focus();
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadinGifContent.Visibility = Visibility.Visible;
                App.mainTask = Task.Run(() => loginFunc());
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void resetUser(AyetContext db)
        {
            try
            {
                foreach (var item in db.Notes) db.Notes.Remove(item);

                foreach (var item in db.Subject) db.Subject.Remove(item);

                foreach (var item in db.Subject) db.Subject.Remove(item);

                foreach (var item in db.Librarys) db.Librarys.Remove(item);

                foreach (var item in db.Remider) db.Remider.Remove(item);

                foreach (var item in db.Tasks) db.Tasks.Remove(item);

                foreach (var item in db.ResultItems) db.ResultItems.Remove(item);

                foreach (var item in db.Integrity.Where(p => p.integrityProtected == false)) db.Integrity.Remove(item);

                foreach (var item in db.Results)
                {
                    db.Results.Where(p => p.resultId == item.resultId).First().resultNotes = false;
                    db.Results.Where(p => p.resultId == item.resultId).First().resultSubject = false;
                    db.Results.Where(p => p.resultId == item.resultId).First().resultLib = false;
                    db.Results.Where(p => p.resultId == item.resultId).First().resultFinallyNote = "Sonuç Metninizi buraya yaza bilirsiniz.";
                }

                foreach (var item in db.Sure)
                {
                    db.Sure.Where(p => p.sureId == item.sureId).First().userCheckCount = 0;
                    db.Sure.Where(p => p.sureId == item.sureId).First().userLastRelativeVerse = 0;
                    db.Sure.Where(p => p.sureId == item.sureId).First().complated = false;
                    db.Sure.Where(p => p.sureId == item.sureId).First().status = "#ADB5BD";
                }

                foreach (var item in db.Verse)
                {
                    db.Verse.Where(p => p.verseId == item.verseId).First().markCheck = false;
                    db.Verse.Where(p => p.verseId == item.verseId).First().remiderCheck = false;
                    db.Verse.Where(p => p.verseId == item.verseId).First().verseCheck = false;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        public void registerFunc()
        {
            try
            {
                if (IsValidEmail(this.Dispatcher.Invoke(() => rgstUserEmail.Text)))
                {
                    if (this.Dispatcher.Invoke(() => rgstUserName.Text.Length) >= 3)
                    {
                        if (this.Dispatcher.Invoke(() => rgstUserLastName.Text.Length) >= 3)
                        {
                            if (termofuseCheck)
                            {
                                using (var db = new AyetContext())
                                {
                                    var checkuser = db.Users.Where(u => u.email == this.Dispatcher.Invoke(() => rgstUserEmail.Text)).FirstOrDefault();

                                    if (checkuser == null)
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            var d = new User { avatarUrl = "profile_1", email = rgstUserEmail.Text, firstName = rgstUserName.Text, lastName = rgstUserLastName.Text, createDate = DateTime.Now, pin = rgstUserPin.Text };
                                            db.Users.Add(d);
                                            rgstUserEmail.Text = "";
                                            rgstUserName.Text = "";
                                            rgstUserLastName.Text = "";
                                            termofuseCheckBox.Background = new SolidColorBrush(Color.FromRgb(56, 60, 65));
                                            termofuseCheck = false;

                                            resetUser(db);

                                            MessageBox.Show("Kayıt Başarılı Giriş Yapabilirsiniz.");
                                            loginGrid.Visibility = Visibility.Hidden;
                                            registerGrid.Visibility = Visibility.Hidden;
                                            loginHomeGrid.Visibility = Visibility.Visible;
                                            registerHomePageBtn.Visibility = Visibility.Collapsed;
                                            loadinGifContent.Visibility = Visibility.Hidden;
                                        });

                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            loginResEmailReturnMessage.Text = "* Bu email adresi ile daha önce kayıt yapılmıştır.";
                                            loginResEmailReturnMessage.Visibility = Visibility.Visible;
                                            rgstUserEmail.Focus();
                                        });
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Lütfen Kullanım koşullarını okuyup anladığınızı ve kabul ettiğinizi işaretleyiniz.");
                            }
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                loginResLastNameReturnMessage.Text = "* Soyisminiz minimum 3 karakterden uzun olmalıdır";
                                loginResLastNameReturnMessage.Visibility = Visibility.Visible;
                                rgstUserLastName.Focus();
                            });
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            loginResNameReturnMessage.Text = "* İsiminiz minimum 3 karakterden uzun olmalıdır";
                            loginResNameReturnMessage.Visibility = Visibility.Visible;
                            rgstUserName.Focus();
                        });
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        loginResEmailReturnMessage.Text = "* Geçerli bir email adresi giriniz.";
                        loginResEmailReturnMessage.Visibility = Visibility.Visible;
                        rgstUserEmail.Focus();
                    });
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainTask = Task.Run(() => registerFunc());
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void registerPageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginGrid.Visibility = Visibility.Hidden;
                registerGrid.Visibility = Visibility.Visible;
                loginGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void loginBackBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginGrid.Visibility = Visibility.Hidden;
                registerGrid.Visibility = Visibility.Hidden;
                loginHomeGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        public void loadUser()
        {
            try
            {
                Thread.Sleep(int.Parse(App.config.AppSettings.Settings["app_animationSpeed"].Value));

                using (var entitydb = new AyetContext())
                {
                    var duser = entitydb.Users.FirstOrDefault();
                    if (duser != null)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            login_profileName.Text = duser.firstName + " " + duser.lastName;
                            login_profileImageBrush.ImageSource = (ImageSource)FindResource(duser.avatarUrl);

                            loginHomeGrid.Visibility = Visibility.Hidden;
                            loginGrid.Visibility = Visibility.Visible;
                            registerGrid.Visibility = Visibility.Hidden;
                            loadinGifContent.Visibility = Visibility.Hidden;
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            loginHomeGrid.Visibility = Visibility.Hidden;
                            loginGrid.Visibility = Visibility.Hidden;
                            registerGrid.Visibility = Visibility.Visible;
                            loadinGifContent.Visibility = Visibility.Hidden;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void loginHomeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadinGifContent.Visibility = Visibility.Visible;
                App.mainTask = Task.Run(() => loadUser());
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void registerHomePageBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginHomeGrid.Visibility = Visibility.Hidden;
                loginGrid.Visibility = Visibility.Hidden;
                registerGrid.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        // ----------------- Mouse Down ----------------- //

        private void rgstUserEmailHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                rgstUserEmail.Focus();
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserNameHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                rgstUserName.Focus();
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserLastNameHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                rgstUserLastName.Focus();
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void userPinHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                userPin.Focus();
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserPinHint_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                rgstUserPin.Focus();
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void termofuseContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (termofuseCheckBox.Background.ToString() == "#FFF0433A")
                {
                    termofuseCheck = false;
                    popup_privacyPolicy.IsOpen = false;
                    termofuseCheckBox.Background = new SolidColorBrush(Color.FromRgb(56, 60, 65));
                }
                else
                {
                    popup_privacyPolicy.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void remmeberMeContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        // ----------------- Mouse Down ----------------- //

        // ----------------- LostFocus ----------------- //

        private void rgstUserEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                loginResEmailReturnMessage.Text = "";
                loginResEmailReturnMessage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                loginResLastNameReturnMessage.Text = "";
                loginResLastNameReturnMessage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserName_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                loginResNameReturnMessage.Text = "";
                loginResNameReturnMessage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void userPin_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                loginPinReturnMessage.Text = "";
                loginPinReturnMessage.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        // ----------------- LostFocus  ----------------- //

        // ----------------- Change  ----------------- //

        private void userPin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(userPin.Text) && userPin.Text.Length > 0)
                {
                    userPinHint.Visibility = Visibility.Collapsed;
                }
                else
                {
                    userPinHint.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(rgstUserLastName.Text) && rgstUserLastName.Text.Length > 0)
                {
                    rgstUserLastNameHint.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rgstUserLastNameHint.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(rgstUserName.Text) && rgstUserName.Text.Length > 0)
                {
                    rgstUserNameHint.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rgstUserNameHint.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void rgstUserPin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(rgstUserPin.Text) && rgstUserPin.Text.Length > 0)
                {
                    rgstUserPinHint.Visibility = Visibility.Collapsed;
                }
                else
                {
                    rgstUserPinHint.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void userPin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Change", ex);
            }
        }

        // ----------------- Change  ----------------- //

        // ----------------- Special ----------------- //

        public bool IsValidEmail(string email)
        {
            try
            {
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
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
            catch (Exception ex)
            {
                return false;
                App.logWriter("Loginscreen", ex);
            }
        }

        private void checkPrivacy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (termofuseCheckBox.Background.ToString() == "#FF383C41")
                {
                    termofuseCheck = true;
                    popup_privacyPolicy.IsOpen = false;
                    termofuseCheckBox.Background = new SolidColorBrush(Color.FromRgb(240, 67, 58));
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void resetPin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup_removePin.IsOpen = true;
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void screetQuestionControl()
        {
            try
            {
                if (this.Dispatcher.Invoke(() => userScreetQuestionAnw.Text.Length) > 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if (userScreetQuestionAnw.Text == screetanw) userScreetQuestionAnwErr.Content = "Pin Kodunuz : " + pin;
                        else userScreetQuestionAnwErr.Content = "Cevap Hatalı Lütfen Tekrar Deneyiniz.";
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() => userScreetQuestionAnw.Focus());
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void popupScreetCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.mainTask = Task.Run(() => screetQuestionControl());
            }
            catch (Exception ex)
            {
                App.logWriter("Loginscreen", ex);
            }
        }

        private void popupClosed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btntemp = sender as Button;
                var popuptemp = (Popup)FindName(btntemp.Uid);
                popuptemp.IsOpen = false;

                userScreetQuestionAnwErr.Content = "* Zorunlu Alan";
                userScreetQuestionAnw.Text = "";
                btntemp = null;
            }
            catch (Exception ex)
            {
                App.logWriter("Click", ex);
            }
        }
    }

    // ----------------- Special ----------------- //
}