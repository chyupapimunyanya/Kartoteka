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

namespace Kartoteka.pages
{
    /// <summary>
    /// Логика взаимодействия для BooksLoans1.xaml
    /// </summary>
    public partial class BooksLoans1 : Page
    {
        public BooksLoans1()
        {
            InitializeComponent();
            if (ReaderClassAdditional.id != 0) ChitName.Text = "Читатель: " + ReaderClassAdditional.last_name + " " + ReaderClassAdditional.first_name;
            if (BookClassAditional.id_book != 0) BookName.Text = "Книга: " + BookClassAditional.Title;
            BiblName.Text = "Библиотекарь: " + UserClass.last_name + " " + UserClass.first_name;
        }

        private void FillListView()
        {
            using (var db = new BAZABBIBLIOTEKAEntities())
            {
                var book1 = db.Books.ToList();
                var reader1 = db.Readers.ToList();
                var user_name1 = db.user_name.ToList();
                var book_loans1 = db.Book_loans.ToList();

                var opu = book_loans1.Join(user_name1,
                                       Book_loans => Book_loans.admin_id,
                                       user_name => user_name.admin_id,
                                       (Book_loans, user_name) => new { Book_loans1 = Book_loans, User_name = user_name })
                                        .Join(reader1,
                                          Book_loans1_User_name => Book_loans1_User_name.Book_loans1.id_reader,
                                          Readers => Readers.id_reader,
                                          (Book_loans1_User_name, Readers) => new { Book_loans1_User_name1 = Book_loans1_User_name, Readers1 = Readers })
                                        .Join(book1,
                                          Book_loans1_User_name_Read => Book_loans1_User_name_Read.Book_loans1_User_name1.Book_loans1.id_book,
                                          Books => Books.id_book,
                                          (Book_loans1_User_name_Read, Books) => new { Book_loans1_User_name1_Read1 = Book_loans1_User_name_Read, Books1 = Books }); ;

                var booksObjects = opu.Select(p => new BookLoansClass
                {
                    id_book_loan = p.Book_loans1_User_name1_Read1.Book_loans1_User_name1.Book_loans1.id_book,
                    admin_id = p.Book_loans1_User_name1_Read1.Book_loans1_User_name1.Book_loans1.admin_id,
                    id_book = p.Book_loans1_User_name1_Read1.Book_loans1_User_name1.Book_loans1.id_book,
                    id_reader = p.Book_loans1_User_name1_Read1.Book_loans1_User_name1.Book_loans1.id_reader,
                    reader_last_name = p.Book_loans1_User_name1_Read1.Readers1.Last_name,
                    reader_first_name = p.Book_loans1_User_name1_Read1.Readers1.First_name,
                    book_title = p.Books1.Title,
                    Date_loan = p.Book_loans1_User_name1_Read1.Book_loans1_User_name1.Book_loans1.Date_loan.ToString("d"),
                    user_Name = p.Book_loans1_User_name1_Read1.Book_loans1_User_name1.User_name.Last_name + " " + p.Book_loans1_User_name1_Read1.Book_loans1_User_name1.User_name.First_name,
                }).ToList();

                int i = 1;
                foreach (var book2 in booksObjects)
                {
                    book2.i = i++;
                }
                GridView myGridView = new GridView();
                myGridView.ColumnHeaderToolTip = "Выданные книги";
                listView.ItemsSource = booksObjects;
                GridViewColumn gvc1 = new GridViewColumn();
                gvc1.DisplayMemberBinding = new Binding("i");
                gvc1.Header = "Номер";

                GridViewColumn gvc2 = new GridViewColumn();
                gvc2.DisplayMemberBinding = new Binding("reader_last_name");
                gvc2.Header = "Фамилия читателя";


                GridViewColumn gvc3 = new GridViewColumn();
                gvc3.DisplayMemberBinding = new Binding("reader_first_name");
                gvc3.Header = "Имя читателя";

                GridViewColumn gvc4 = new GridViewColumn();
                gvc4.DisplayMemberBinding = new Binding("book_title");
                gvc4.Header = "Название книги";

                GridViewColumn gvc5 = new GridViewColumn();
                gvc5.DisplayMemberBinding = new Binding("user_Name");
                gvc5.Header = "Библиотекарь";

                GridViewColumn gvc6 = new GridViewColumn();
                gvc6.DisplayMemberBinding = new Binding("Date_loan");
                gvc6.Header = "Дата выдачи";

                myGridView.Columns.Add(gvc1);
                myGridView.Columns.Add(gvc2);
                myGridView.Columns.Add(gvc3);
                myGridView.Columns.Add(gvc4);
                myGridView.Columns.Add(gvc5);
                myGridView.Columns.Add(gvc6);
                listView.View = myGridView;
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = null;
            FillListView();
            deleteBtn.IsEnabled = true;
            addBtn.IsEnabled = true;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ReaderClassAdditional.id == 0) MessageBox.Show("Выберите читателя",
                                                "Ошибка",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Error);
            else if(BookClassAditional.id_book == 0) MessageBox.Show("Выберите книгу",
                                                "Ошибка",
                                                 MessageBoxButton.OK,
                                                 MessageBoxImage.Error);
            else {
                using(var db = new BAZABBIBLIOTEKAEntities()){
                    Book_loans book_Loans = new Book_loans();
                    book_Loans.admin_id = UserClass.admin_id;
                    book_Loans.id_reader = ReaderClassAdditional.id;
                    book_Loans.id_book = BookClassAditional.id_book;
                    book_Loans.Date_loan = DateTime.Now;
                    db.Book_loans.Add(book_Loans);
                    db.SaveChanges();
                }
            }
        }
    }
}
