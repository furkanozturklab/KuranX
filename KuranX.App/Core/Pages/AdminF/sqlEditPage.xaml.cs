using KuranX.App.Core.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace KuranX.App.Core.Pages.AdminF
{
    /// <summary>
    /// Interaction logic for sqlEditPage.xaml
    /// </summary>
    public partial class sqlEditPage : Page
    {
        private Sure dSure = new Sure();
        private Verse dVerse = new Verse();

        private int ssure = 0, sverse = 0, sverseMax;

        public sqlEditPage()
        {
            InitializeComponent();
        }

        public void loadItem(int sId, int rId = 1)
        {
            try
            {
                using (var entitydb = new AyetContext())
                {
                    dSure = entitydb.Sure.Where(p => p.sureId == sId).First();
                    dVerse = entitydb.Verse.Where(p => p.sureId == sId && p.relativeDesk == rId).First();

                    this.Dispatcher.Invoke(() =>
                    {
                        intel1.Text = "";
                        intel2.Text = "";
                        intel3.Text = "";
                    });

                    var dintel1 = entitydb.Interpreter.Where(p => p.sureId == sId && p.verseId == rId && p.interpreterWriter == "Yorumcu 1").FirstOrDefault();
                    var dintel2 = entitydb.Interpreter.Where(p => p.sureId == sId && p.verseId == rId && p.interpreterWriter == "Yorumcu 2").FirstOrDefault();
                    var dintel3 = entitydb.Interpreter.Where(p => p.sureId == sId && p.verseId == rId && p.interpreterWriter == "Yorumcu 3").FirstOrDefault();

                    if (dintel1 != null) this.Dispatcher.Invoke(() => intel1.Text = dintel1.interpreterDetail);
                    if (dintel2 != null) this.Dispatcher.Invoke(() => intel2.Text = dintel2.interpreterDetail);
                    if (dintel3 != null) this.Dispatcher.Invoke(() => intel3.Text = dintel3.interpreterDetail);

                    sverseMax = (int)dSure.numberOfVerses;
                    ssure = (int)dSure.sureId;
                    sverse = (int)dVerse.relativeDesk;

                    this.Dispatcher.Invoke(() =>
                    {
                        ARABIC.Text = dVerse.verseArabic;
                        TR.Text = dVerse.verseTr;
                        DESC.Text = dVerse.verseDesc;
                        setname.Text = dSure.name;
                        setverse.Text = dVerse.relativeDesk.ToString();
                        setverse.IsReadOnly = false;
                    });
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void selectSureCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var comboBox = (ComboBox)sender;
                if (!comboBox.IsLoaded) return;

                var item = selectSureCombobox.SelectedItem as ComboBoxItem;

                if (int.Parse(item.Uid) != 0) App.loadTask = Task.Run(() => loadItem(this.Dispatcher.Invoke(() => int.Parse(item.Uid))));
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void nextSure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ssure++;
                if (ssure < 115 && ssure > 0) App.loadTask = Task.Run(() => loadItem(ssure, 1));
                else
                {
                    ssure--;
                    MessageBox.Show("Gidilecek Max Süre Sınırı Gecildi.");
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void beforeAyet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sverse--;
                if (sverse > 0 && sverse < sverseMax)
                {
                    if (ssure > 0) App.loadTask = Task.Run(() => loadItem(ssure, sverse));
                }
                else
                {
                    sverse++;
                    MessageBox.Show("Önceki Süreye Geciniz");
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void dynamicVerse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ssure > 0)
                {
                    if (int.Parse(setverse.Text) > sverseMax)
                    {
                        MessageBox.Show("Gidilecek Ayet Sınırı Aştınız max : " + sverseMax);
                    }
                    else
                    {
                        if (int.Parse(setverse.Text) > 0)
                        {
                            App.loadTask = Task.Run(() => loadItem(ssure, this.Dispatcher.Invoke(() => int.Parse(setverse.Text))));
                        }
                        else
                        {
                            MessageBox.Show("Minimun Ayet Sınırı 0 dan büyük olamalıdır ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void setverse_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void saveIntel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ssure > 0)
                {
                    using (var entitydb = new AyetContext())
                    {
                        if (intel1.Text.Length > 0)
                        {
                            if (entitydb.Interpreter.Where(p => p.sureId == ssure && p.verseId == sverse && p.interpreterWriter == "Yorumcu 1").Count() > 0)
                            {
                                entitydb.Interpreter.Where(p => p.sureId == ssure && p.verseId == sverse && p.interpreterWriter == "Yorumcu 1").First().interpreterDetail = intel1.Text;
                                entitydb.SaveChanges();
                            }
                            else
                            {
                                var Integ = new Interpreter { verseId = sverse, sureId = ssure, interpreterWriter = "Yorumcu 1", interpreterDetail = intel1.Text };
                                entitydb.Interpreter.Add(Integ);
                                entitydb.SaveChanges();
                            }
                        }

                        if (intel2.Text.Length > 0)
                        {
                            if (entitydb.Interpreter.Where(p => p.sureId == ssure && p.verseId == sverse && p.interpreterWriter == "Yorumcu 2").Count() > 0)
                            {
                                entitydb.Interpreter.Where(p => p.sureId == ssure && p.verseId == sverse && p.interpreterWriter == "Yorumcu 2").First().interpreterDetail = intel2.Text;
                                entitydb.SaveChanges();
                            }
                            else
                            {
                                var Integ = new Interpreter { verseId = sverse, sureId = ssure, interpreterWriter = "Yorumcu 2", interpreterDetail = intel2.Text };
                                entitydb.Interpreter.Add(Integ);
                                entitydb.SaveChanges();
                            }
                        }

                        if (intel3.Text.Length > 0)
                        {
                            if (entitydb.Interpreter.Where(p => p.sureId == ssure && p.verseId == sverse && p.interpreterWriter == "Yorumcu 3").Count() > 0)
                            {
                                entitydb.Interpreter.Where(p => p.sureId == ssure && p.verseId == sverse && p.interpreterWriter == "Yorumcu 3").First().interpreterDetail = intel3.Text;
                                entitydb.SaveChanges();
                            }
                            else
                            {
                                var Integ = new Interpreter { verseId = sverse, sureId = ssure, interpreterWriter = "Yorumcu 3", interpreterDetail = intel3.Text };
                                entitydb.Interpreter.Add(Integ);
                                entitydb.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Önce Sureyi ve ayeti seciniz.");
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void saveVerse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ssure > 0)
                {
                    using (var entitydb = new AyetContext())
                    {
                        entitydb.Verse.Where(p => p.sureId == ssure && p.relativeDesk == sverse).First().verseArabic = ARABIC.Text;
                        entitydb.Verse.Where(p => p.sureId == ssure && p.relativeDesk == sverse).First().verseTr = TR.Text;
                        entitydb.Verse.Where(p => p.sureId == ssure && p.relativeDesk == sverse).First().verseDesc = DESC.Text;

                        entitydb.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void nextAyet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sverse++;
                if (sverseMax >= sverse)
                {
                    if (ssure > 0) App.loadTask = Task.Run(() => loadItem(ssure, sverse));
                }
                else
                {
                    sverse--;
                    MessageBox.Show("Sıradaki Süreye Geciniz");
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }

        private void beforeSure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ssure--;
                if (ssure > 0) App.loadTask = Task.Run(() => loadItem(ssure, 1));
                else
                {
                    ssure++;
                    MessageBox.Show("Gidilecek Min Süre Sınırı Gecildi.");
                }
            }
            catch (Exception ex)
            {
                App.logWriter("Admin", ex);
            }
        }
    }
}