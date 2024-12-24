using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для Books.xaml
    /// </summary>
    public partial class Books1 : Page
    {
        public Books1()
        {
            InitializeComponent();
            FillListView();
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
                    Copies = b.Copies // Убедитесь, что это свойство присутствует в BookClass
                }).ToList();

                int i = 1;
                foreach (var book in bookObjects)
                {
                    book.i = i++; // Устанавливаем номер книги
                }

                GridView myGridView = new GridView();
                myGridView.ColumnHeaderToolTip = "Книги";

                // Создать столбцы для GridView
                GridViewColumn gvc1 = new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("i"),
                    Header = "Номер"
                };

                GridViewColumn gvc2 = new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("Title"),
                    Header = "Название"
                };

                GridViewColumn gvc3 = new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("Author"),
                    Header = "Автор"
                };

                GridViewColumn gvc4 = new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("Genre"),
                    Header = "Жанр"
                };

                GridViewColumn gvc5 = new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("Year_publication"),
                    Header = "Год публикации"
                };

                GridViewColumn gvc6 = new GridViewColumn
                {
                    DisplayMemberBinding = new Binding("Copies"), // Изменено на Copies
                    Header = "Кол-во в наличии" // Изменено на Кол-во в наличии
                };

                // Добавить столбцы в GridView
                myGridView.Columns.Add(gvc1);
                myGridView.Columns.Add(gvc2);
                myGridView.Columns.Add(gvc3);
                myGridView.Columns.Add(gvc4);
                myGridView.Columns.Add(gvc5);
                myGridView.Columns.Add(gvc6);

                // Установить источник данных для ListView
                listView.View = myGridView;
                listView.ItemsSource = bookObjects; // Устанавливаем источник данных
            }
        }


        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить выделенную строку?",
                                                      "Предупреждение",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                BookClass selectedBook = (BookClass)listView.SelectedItem;
                if (selectedBook != null)
                {
                    int BookId = selectedBook.id_book;
                    using (var db = new BAZABBIBLIOTEKAEntities())
                    {
                        if (db.Book_loans.Any(o => o.id_book == BookId))
                        {
                            MessageBox.Show("Сначала удалите книгу из вкладки выданных книг",
                                                                "Ошибка",
                                                                 MessageBoxButton.OK,
                                                                 MessageBoxImage.Error);
                        }
                        else
                        {

                            Books book = db.Books.FirstOrDefault(p => p.id_book == BookId);

                            db.Books.Remove(book);
                            db.SaveChanges();
                        }
                    }
                    listView.ItemsSource = null;
                    FillListView();
                }
                else
                {
                    MessageBox.Show("Нельзя удалить пустую строку",
                                    "Ошибка",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Error);
                }
            }
        }
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Поиск...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = Brushes.Black; // Устанавливаем цвет текста в черный
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Поиск...";
                SearchBox.Foreground = Brushes.Gray; // Устанавливаем цвет текста в серый
            }
        }


        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = null;
            FillListView();
            deleteBtn.IsEnabled = true;
            saveBtn.IsEnabled = true;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            //Вызывает окошко с текстбоксами
            AddBook addBook = new AddBook();
            addBook.ShowDialog();

        }

        private void vidachaBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите выбрать для выдачи эту книгу?",
                                                      "Предупреждение",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                BookClass selectedBook = (BookClass)listView.SelectedItem; // Предполагается, что listView содержит список книг
                if (selectedBook != null)
                {
                    // Проверка доступности книги
                    if (selectedBook.Copies <= 0)
                    {
                        MessageBox.Show("Книга недоступна для выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int bookId = selectedBook.id_book;
                    BookClassAditional.Title = selectedBook.Title;
                    BookClassAditional.id_book = bookId;

                    // Логика выдачи книги
                    selectedBook.Copies -= 1; // Уменьшаем количество доступных экземпляров на 1

                    // Здесь можно добавить логику для записи в базу данных о выдаче книги.
                    // Например, сохранить информацию о выдаче в базе данных или в вашей коллекции.

                    MessageBox.Show("Книга выбрана.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите книгу для выдачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }




        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            int filterType = filterCombo.SelectedIndex;
            saveBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
            listView.ItemsSource = null;
            using (var db = new BAZABBIBLIOTEKAEntities())
            {
                var books = db.Books.ToList();
                var bookObjects = books.Select(b => new BookClass
                {
                    id_book = b.id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    Year_publication = b.Year_publication
                }).ToList();

                switch (filterType)
                {
                    case 0:
                        {
                            var filteredBooks = bookObjects.FindAll(x => x.Title == SearchBox.Text.Trim().Trim());
                            int i = 1;
                            foreach (var client in filteredBooks)
                            {
                                client.i = i++;
                            }
                            listView.ItemsSource = filteredBooks;
                        }
                        break;
                    case 1:
                        {
                            var filteredBooks = bookObjects.FindAll(x => x.Author == SearchBox.Text.Trim());
                            int i = 1;
                            foreach (var client in filteredBooks)
                            {
                                client.i = i++;
                            }
                            listView.ItemsSource = filteredBooks;
                        }
                        break;
                    case 2:
                        {
                            var filteredBooks = bookObjects.FindAll(x => x.Year_publication.ToString() == SearchBox.Text.Trim());
                            int i = 1;
                            foreach (var client in filteredBooks)
                            {
                                client.i = i++;
                            }
                            listView.ItemsSource = filteredBooks;
                        }
                        break;
                    case 3:
                        {
                            var filteredBooks = bookObjects.FindAll(x => x.Genre.Contains(SearchBox.Text.Trim()));
                            int i = 1;
                            foreach (var client in filteredBooks)
                            {
                                client.i = i++;
                            }
                            listView.ItemsSource = filteredBooks;
                        }
                        break;
                }
            }
            SearchBox.Text = "Поиск...";
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
