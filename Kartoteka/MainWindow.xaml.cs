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

namespace Kartoteka
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text == "") MessageBox.Show("Логин не введен! Пожалуйста повторите попытку!", "Ошибка!",
                                        MessageBoxButton.OK,
                            MessageBoxImage.Error);
            else if (passwordBox.Password == "") MessageBox.Show("Пароль не введен! Пожалуйста повторите попытку!", "Ошибка!",
                                   MessageBoxButton.OK,
                            MessageBoxImage.Error);
            else
            {
                using (var BD = new BAZABBIBLIOTEKAEntities())
                {
                    if (BD.Users.FirstOrDefault(u => u.login == textBox1.Text && u.password == passwordBox.Password) != null)
                    {
                        
                        Window1 mainForm = new Window1();
                        mainForm.Show();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Не верный логин или пароль! Пожалуйста повторите попытку!", "Ошибка!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                }
            }
        }
    }
}
