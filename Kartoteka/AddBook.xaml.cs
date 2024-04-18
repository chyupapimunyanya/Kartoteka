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

namespace Kartoteka
{
    /// <summary>
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        public AddBook()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TitleBox.Text.Trim() == "" ||
                TitleBox_Copy.Text.Trim() == "" ||
                TitleBox_Copy1.Text.Trim() == "" ||
                TitleBox_Copy2.Text.Trim() == ""
                )
                MessageBox.Show("Все поля должны быть заполнены",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            else
            {
                using (var db = new BAZABBIBLIOTEKAEntities())
                {
                    if (!db.Books.Any(u => u.Title == TitleBox.Text &&
                                           u.Author == TitleBox_Copy.Text && u.Year_publication.ToString() == TitleBox_Copy1.Text
                                                    && u.Genre == TitleBox_Copy2.Text))
                    {
                        Books books = new Books();
                        books.Title = TitleBox.Text.Trim();
                        books.Author = TitleBox_Copy.Text.Trim();
                        books.Year_publication = Convert.ToInt32(TitleBox_Copy1.Text);
                        books.Genre = TitleBox_Copy2.Text.Trim();
                        db.Books.Add(books);
                        db.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены",
                                        "Сохранение",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                        this.Close();
                    }
                    else MessageBox.Show("Такая книга уже существует",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }
    }
}
