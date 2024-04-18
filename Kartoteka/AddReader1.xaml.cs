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
    /// Логика взаимодействия для AddReader1.xaml
    /// </summary>
    public partial class AddReader1 : Window
    {
        public AddReader1()
        {
            InitializeComponent();
        }

        private void AddreadeerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text.Trim() == "" ||
               LastnameCopy.Text.Trim() == "" ||
                AdressBox.Text.Trim() == "" ||
                DateBox.Text.Trim() == "" || NumberBox.Text.Trim() == ""
                )
                MessageBox.Show("Все поля должны быть заполнены",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            else
            {
                using (var db = new BAZABBIBLIOTEKAEntities())
                {
                    if (!db.Readers.Any(p => p.Last_name == LastnameCopy.Text &&
                         p.First_name == NameBox.Text &&
                         p.Date_birth.ToString() == DateBox.Text) &&
                        !db.Contacts.Any(c => c.Phone_number == NumberBox.Text) &&
                        !db.Address.Any(a => a.Address1 == AdressBox.Text))
                    {
                        Contacts contacts = new Contacts();
                        contacts.Phone_number = NumberBox.Text.Trim();
                        db.Contacts.Add(contacts);
                        Address addresses = new Address();
                        addresses.Address1 = AdressBox.Text.Trim();
                        db.Address.Add(addresses);
                        Readers readers = new Readers();
                        readers.Last_name = LastnameCopy.Text.Trim();
                        readers.First_name = NameBox.Text.Trim();
                        readers.Date_birth = Convert.ToDateTime(DateBox.Text.Trim());
                        db.Readers.Add(readers);
                        db.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены",
                                        "Сохранение",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                        this.Close();
                    }
                    else MessageBox.Show("Такой читатель уже существует",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }
    }
}
