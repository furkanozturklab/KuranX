
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using KuranX.App.Core.Classes;
using Microsoft.EntityFrameworkCore;

namespace KuranX.App.Core.Pages.AdminF
{
    /// <summary>
    /// Interaction logic for exportDataPage.xaml
    /// </summary>
    public partial class exportDataPage : Page
    {

        private List<string> queryList = new List<string>();
        private List<dynamic> allData;
        private List<dynamic> tempData;
        private List<dynamic> selectedItems = new List<dynamic>();
        private string selectedTable;
        private int pageSize = 20; // Bir sayfada kaç veri gösterileceği
        private int currentPage = 1; // Şu anki sayfa numarası
        private int totalDataCount;

        public exportDataPage()
        {
            InitializeComponent();
        }


        /* ---------------- Binding and pageing ---------------- */
        private void LoadData(string tableName)
        {
            selectedTable = tableName;
            using (var dbContext = new AyetContext())
            {
                // Tablodan tüm verileri al

                switch (tableName)
                {
                    case "Users":
                        allData = dbContext.Users.ToList<dynamic>();
                        break;
                    case "Sure":
                        allData = dbContext.Sure.ToList<dynamic>();
                        break;
                    case "Verse":
                        allData = dbContext.Verse.ToList<dynamic>();
                        break;
                    case "Section":
                        allData = dbContext.Sections.ToList<dynamic>();
                        break;
                    case "VerseClass":
                        allData = dbContext.VerseClass.ToList<dynamic>();
                        break;
                    case "Interpreter":
                        allData = dbContext.Interpreter.ToList<dynamic>();
                        break;
                    case "Notes":
                        allData = dbContext.Notes.ToList<dynamic>();
                        break;
                    case "Integrity":
                        allData = dbContext.Integrity.ToList<dynamic>();
                        break;
                    case "Subject":
                        allData = dbContext.Subject.ToList<dynamic>();
                        break;
                    case "SubjectItems":
                        allData = dbContext.SubjectItems.ToList<dynamic>();
                        break;
                    case "Words":
                        allData = dbContext.Words.ToList<dynamic>();
                        break;
                    case "Remider":
                        allData = dbContext.Remider.ToList<dynamic>();
                        break;
                    case "Tasks":
                        allData = dbContext.Tasks.ToList<dynamic>();
                        break;
                    case "Userhelp":
                        allData = dbContext.UserHelp.ToList<dynamic>();
                        break;
                }

                totalDataCount = allData.Count;
                tempData = allData;
                BindDataGrid();
            }
        }

        private int GetTotalPageCount()
        {
            return (int)Math.Ceiling((double)totalDataCount / pageSize);
        }

        private void BindDataGrid()
        {
            // Mevcut sayfadaki verileri al ve DataGrid'e bağla
            var currentPageData = GetCurrentPageData();
            dataGrid.ItemsSource = currentPageData;
        }

        private List<dynamic> GetCurrentPageData()
        {
            int startIndex = (currentPage - 1) * pageSize;
            return allData.Skip(startIndex).Take(pageSize).ToList();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < GetTotalPageCount())
            {
                currentPage++;
                BindDataGrid();
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                BindDataGrid();
            }
        }

        private void selectTable_Click(object sender, RoutedEventArgs e)
        {

            var x = sender as Button;

            LoadData(x.Uid);
        }

        /* ---------------- Binding and pageing ---------------- */


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                selectedItems.Clear();

                // Seçili öğeleri al ve selectedItems listesine ekle
                foreach (var selectedItem in dataGrid.SelectedItems)
                {
                    selectedItems.Add(selectedItem as dynamic);
                }
            }
        }


        public string insertString(dynamic value)
        {
            /*
            dynamic sel;
            switch (selectedTable)
            {
                case "User":
                    sel = value as User;
                    return $@"INSERT INTO {selectedTable} ('user_email','user_firstName','user_lastName','pin','user_screetQuestion','user_screetAnw','user_createDate','user_updateDate','user_avatarUrl') VALUES ('{sel.email}','{sel.firstName}','{sel.lastName}',{sel.pin},'{sel.screetQuestion}','{sel.screetQuestion}','{sel.screetQuestionAnw}','{sel.createDate}','{sel.updateDate}','{sel.avatarUrl}');";
                case "Sure":
                    sel = value as Sure;
                    return $@"INSERT INTO {selectedTable} ('name','numberOfVerses','numberOfSection','userCheckCount','userLastRelativeVerse','landingLocation','deskLanding','deskMushaf','deskList','status','description','completed') VALUES('{sel.name}',{sel.numberOfVerses},{sel.numberOfSection},{sel.userCheckCount},{sel.userLastRelativeVerse},'{sel.landingLocation}',{sel.deskLanding},{sel.deskMushaf},{sel.deskList},'{sel.status}','{sel.description}',{sel.completed})";
                    break;
                case "Verse":
                    sel = value as Verse;
                    return $@"INSERT INTO {selectedTable} ('sureId','relativeDesk','verseArabic','verseTr','verseDesc','verseCheck','markCheck','remiderCheck',) VALUES({sel.sre},{},{},'{}','{}','{}',{},{},{})";
                    break;
                case "Section":
                    sel = value as Section;
                    return $@"INSERT INTO {selectedTable} ('SureId','startVerse','endVerse','SectionName','SectionDescription','SectionDetail','IsMark','SectionNumber',) VALUES({},{},{},'{}','{}','{}',{},{})";
                    break;
                case "VerseClass":
                    sel = value as VerseClass;
                    return $@"INSERT INTO {selectedTable} ('sureId','relativeDesk','v_hk','v_tv','v_cz','v_mk','v_du','v_hr','v_sn') VALUES({},{},{},{},{},{},{},{},{},{})";
                    break;
                case "Interpreter":
                    sel = value as Interpreter;
                    return $@"INSERT INTO {selectedTable} ('verseId','sureId','interpreterWriter','interpreterDetail',) VALUES({},{},{},'{}')";
                    break;
                case "Notes":
                    sel = value as Notes;
                    return $@"INSERT INTO {selectedTable} ('verseId','sureId','subjectId','sectionId','noteHeader','noteDetail','noteLocation','created','modify',) VALUES({},{},{},{},'{}',' {}','{}','{}','{}')";
                    break;
                case "Integrity":
                    sel = value as Integrity;
                    return $@"INSERT INTO {selectedTable} ('integrityName','connectVerseId','connectSureId','connectedVerseId','connectedSureId','integrityNote','integrityProtected','created','modify',) VALUES ('{}',{},{},{},{},'{}',{},'{}','{}',{})";
                    break;
                case "Subject":
                    sel = value as Subject;
                    return $@"INSERT INTO {selectedTable} ('subjectName','subjectColor','created','modify',) VALUES({},'{}','{}','{}','{}')";
                    break;
                case "SubjectItems":
                    sel = value as SubjectItems;
                    return $@"INSERT INTO {selectedTable} ('subjectId','sureId','verseId','subjectNotesId','subjectName','created','modify',) VALUES({},{},{},{},'{}','{}','{}')";
                    break;
                case "Words":
                    sel = value as Words;
                    return $@"INSERT INTO {selectedTable} ('sureId','verseId','arp_read','tr_read','word_meal','root',) VALUES({},{},'{}','{}','{}','{}')";
                    break;
                case "Remider":
                    sel = value as Remider;
                    return $@"INSERT INTO {selectedTable}('remiderName','remiderDetail','loopType','status','remiderDate','create','lastAction','connectVerseId','connectSureId','priority') VALUES ('{}','{}','{}','{}','{}','{}','{}',{},{},{});";
                    break;
                case "Tasks":
                    sel = value as Tasks;
                    return $@"INSERT INTO {selectedTable} ('missonsId','missonsTime','missonsRepeart','missonsType','missonsColor',) VALUES({},{},{},'{}','{}')";
                    break;
                case "Userhelp":
                    sel = value as Userhelp;
                    return $@"INSERT INTO {selectedTable} ('baseName','infoName','description','infoImage',) VALUES('{}','{}','{}','{}')";
                    break;


            }
             */
            return "";
           
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in selectedItems)
            {
                queryList.Add(insertString(item));
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {

            if (selectedTable == "Verse" || selectedTable == "VerseClass" || selectedTable == "Sure" || selectedTable == "Words" || selectedTable == "Interpreter" || selectedTable == "UserHelp")
            {


            }
            else
            {
                MessageBox.Show("Sadece Sureler , Ayet , Ayet Sınıfı , Yorumcular , Kelimeler ve Kullanıcı yardımı tablolarının verilerinde güncelleme yapabilirsiniz.");
            }

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
