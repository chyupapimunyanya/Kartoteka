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
using System.Windows.Shapes;

namespace Kartoteka.pages
{
    /// <summary>
    /// Логика взаимодействия для LoansFromReaderCard.xaml
    /// </summary>
    public partial class LoansFromReaderCard : Window
    {
            private int _readerId; // Поле для хранения ID читателя

            public LoansFromReaderCard(int readerId)
            {
                InitializeComponent();
                _readerId = readerId; // Инициализация поля
                FillListView(); // Заполняем список книг
            }

            private void FillListView()
            {
                using (var db = new BAZABBIBLIOTEKAEntities())
                {
                    var books = db.Books.ToList();
                    var bookObjects = books.Select(b => new BookClass
                    {
                        id_book = b.id_book,
                        Title = b.Title,
                        Author = b.Author,
                        Genre = b.Genre,
                        Year_publication = b.Year_publication,
                        Copies = b.Copies
                    }).ToList();

                    int i = 1;
                    foreach (var book in bookObjects)
                    {
                        book.i = i++; // Устанавливаем номер книги
                    }

                    GridView myGridView = new GridView();
                    myGridView.ColumnHeaderToolTip = "Книги";

                    // Создать столбцы для GridView
                    myGridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("i"), Header = "Номер" });
                    myGridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("Title"), Header = "Название" });
                    myGridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("Author"), Header = "Автор" });
                    myGridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("Genre"), Header = "Жанр" });
                    myGridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("Year_publication"), Header = "Год публикации" });
                    myGridView.Columns.Add(new GridViewColumn { DisplayMemberBinding = new Binding("Copies"), Header = "Кол-во в наличии" });

                    // Установить источник данных для ListView
                    listView.View = myGridView;
                    listView.ItemsSource = bookObjects; // Устанавливаем источник данных
                }
            }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбора книги
            if (listView.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedBook = (BookClass)listView.SelectedItem;

            using (var db = new BAZABBIBLIOTEKAEntities())
            {
                // Проверка доступности экземпляров
                var bookToUpdate = db.Books.FirstOrDefault(b => b.id_book == selectedBook.id_book);
                if (bookToUpdate != null && bookToUpdate.Copies > 0)
                {
                    // Создаем новую запись о выдаче книги
                    Book_loans book_Loans = new Book_loans
                    {
                        admin_id = UserClass.admin_id,
                        id_reader = _readerId,
                        id_book = selectedBook.id_book,
                        Date_loan = DateTime.Now
                    };

                    // Добавляем запись о выдаче в базу данных
                    db.Book_loans.Add(book_Loans);
                    bookToUpdate.Copies -= 1; // Уменьшаем количество доступных экземпляров

                    // Сохраняем изменения в базе данных
                    db.SaveChanges();
                    MessageBox.Show("Книга успешно выдана.");

                    // Обновить таблицу после выдачи книги
                    FillListView();
                    // Если у вас есть метод для обновления карточки читателя, вызовите его здесь
                    // UpdateReaderCard(); // Например, если у вас есть такой метод
                }
                else
                {
                    MessageBox.Show("Нет доступных экземпляров книги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
