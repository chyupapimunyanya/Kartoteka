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
                    Year_publication = b.Year_publication
                }).ToList();

                int i = 1;
                foreach (var book in bookObjects)
                {
                    book.i = i++;
                }

                GridView myGridView = new GridView();
                myGridView.ColumnHeaderToolTip = "Книги";

                // Установить источник данных для GridView
                listView.ItemsSource = bookObjects;

                // Создать столбцы для GridView
                GridViewColumn gvc1 = new GridViewColumn();
                gvc1.DisplayMemberBinding = new Binding("i");
                gvc1.Header = "Номер";

                GridViewColumn gvc2 = new GridViewColumn();
                gvc2.DisplayMemberBinding = new Binding("Title");
                gvc2.Header = "Название";

                GridViewColumn gvc3 = new GridViewColumn();
                gvc3.DisplayMemberBinding = new Binding("Author");
                gvc3.Header = "Автор";

                GridViewColumn gvc4 = new GridViewColumn();
                gvc4.DisplayMemberBinding = new Binding("Genre");
                gvc4.Header = "Жанр";

                GridViewColumn gvc5 = new GridViewColumn();
                gvc5.DisplayMemberBinding = new Binding("Year_publication");
                gvc5.Header = "Год публикации";

                // Добавить столбцы в GridView
                myGridView.Columns.Add(gvc1);
                myGridView.Columns.Add(gvc2);
                myGridView.Columns.Add(gvc3);
                myGridView.Columns.Add(gvc4);
                myGridView.Columns.Add(gvc5);

                // Установить View для ListView
                listView.View = myGridView;
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
                            MessageBox.Show("Сначала удалите книгу из вкладки выдданных книг",
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
            MessageBoxResult result = MessageBox.Show("Вы точно хотите выдать эту книгу?",
                                                      "Предупреждение",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                BookClass selectedBook = (BookClass)listView.SelectedItem;
                if (selectedBook != null)
                {
                    int bookId = selectedBook.id_book;
                    BookClassAditional.Title = selectedBook.Title;
                    BookClassAditional.id_book = bookId;
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
            SearchBox.Text = "";
        }
    }
}
