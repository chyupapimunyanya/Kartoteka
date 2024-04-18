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
                    Users user = BD.Users.FirstOrDefault(u => u.login == textBox1.Text.Trim() && u.password == passwordBox.Password.Trim());
                    if (user != null)
                    {   ///Присвоил некоторые данные классу, чтобы использовать их в следующих окнах
                        UserClass.admin_id = user.admin_id;
                        user_name user_Name = BD.user_name.FirstOrDefault(un => un.admin_id == UserClass.admin_id);
                        if (user_Name != null)
                        {
                            UserClass.first_name = user_Name.First_name;
                            UserClass.last_name = user_Name.Last_name;
                        }
                        Window1 main = new Window1();
                        main.Show();
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
