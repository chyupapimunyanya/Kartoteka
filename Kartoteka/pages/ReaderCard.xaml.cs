using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;

namespace Kartoteka.pages
{
    public partial class ReaderCard : Window
    {
        private ReaderClass _selectedReader;
        private int readerId; // ID читателя

        public ReaderCard(ReaderClass reader)
        {
            _selectedReader = reader;
            InitializeComponent();
            lastNameTextBlock.Text = reader.last_name;
            firstNameTextBlock.Text = reader.first_name;
            patronymicTextBlock.Text = reader.patronymic;
            phoneNumberTextBlock.Text = reader.phone_number;
            addressTextBlock.Text = reader.adress;
            dateOfBirthTextBlock.Text = reader.Date_birth;

            readerId = reader.id; // Сохраняем ID читателя
            LoadIssuedBooks(readerId); // Загружаем выданные книги для читателя
        }

        private void LoadIssuedBooks(int readerId)
        {
            using (var db = new BAZABBIBLIOTEKAEntities())
            {
                var issuedBooks = db.Book_loans
                    .Where(bl => bl.id_reader == readerId)
                    .Select(bl => new
                    {
                        BookId = bl.id_book,
                        BookTitle = db.Books.FirstOrDefault(b => b.id_book == bl.id_book).Title,
                        LoanDate = bl.Date_loan
                    })
                    .ToList();

                issuedBooksListView.ItemsSource = issuedBooks;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ReturnBook(int bookId, int readerId)
        {
            using (var db = new BAZABBIBLIOTEKAEntities())
            {
                var bookLoan = db.Book_loans.FirstOrDefault(bl => bl.id_book == bookId && bl.id_reader == readerId);
                if (bookLoan != null)
                {
                    // Увеличиваем количество доступных экземпляров книги
                    var bookToUpdate = db.Books.FirstOrDefault(b => b.id_book == bookId);
                    if (bookToUpdate != null)
                    {
                        bookToUpdate.Copies += 1; // Увеличиваем количество доступных экземпляров
                    }

                    db.Book_loans.Remove(bookLoan);
                    db.SaveChanges();

                    MessageBox.Show("Книга успешно возвращена.");
                    LoadIssuedBooks(readerId); // Обновляем список выданных книг
                }
                else
                {
                    MessageBox.Show("Не удалось найти запись о выдаче книги.");
                }
            }
        }

        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (issuedBooksListView.SelectedItem != null)
            {
                var selectedBook = (dynamic)issuedBooksListView.SelectedItem;
                int selectedBookId = selectedBook.BookId;

                ReturnBook(selectedBookId, readerId);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите книгу для возврата.");
            }
        }

        private void IssueBookButton_Click(object sender, RoutedEventArgs e)
        {
            LoansFromReaderCard loansWindow = new LoansFromReaderCard(readerId);
            loansWindow.ShowDialog(); // Открываем окно выдачи книги
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadIssuedBooks(readerId); // Обновляем список выданных книг
        }
    }
}
