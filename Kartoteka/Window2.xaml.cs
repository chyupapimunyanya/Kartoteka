using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.Data.SqlClient;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Kartoteka;

namespace Kartoteka
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            FillListView();
        }
        private void FillListView()
        {
            using (var db = new BAZABBIBLIOTEKAEntities())
            {
                var books = db.Books.ToList();
                var bookObjects = books.Select(b => new BooksClass
                {
                    Id = b.id_book,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    year_publication = b.Year_publication
                }).ToList();

                int i = 1;
                foreach (var book in bookObjects)
                {
                    book.Id = i++;
                }

                GridView myGridView = new GridView();
                myGridView.ColumnHeaderToolTip = "Книги";

                // Установить источник данных для GridView
                listView.ItemsSource = bookObjects;

                // Создать столбцы для GridView
                GridViewColumn gvc1 = new GridViewColumn();
                gvc1.DisplayMemberBinding = new Binding("Id");
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
                gvc5.DisplayMemberBinding = new Binding("year_publication");
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

    }

}
    

