using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BookBtn_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Uri("pages/Books1.xaml", UriKind.Relative));
            Razdel.Text = "Книги";
        }

        private void VidBookBtn_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Uri("pages/BooksLoans1.xaml", UriKind.Relative));
            Razdel.Text = "Выданные Книги";
        }

        private void ChitBtn_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Uri("pages/Readers1.xaml", UriKind.Relative));
            Razdel.Text = "Читатели";
        }
    }       
}

