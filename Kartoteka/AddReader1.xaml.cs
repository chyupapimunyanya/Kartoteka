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
                PatronymicBox.Text.Trim() == "" ||
                AdressBox.Text.Trim() == "" ||
                DateBox.Text.Trim() == "" ||
                NumberBox.Text.Trim() == "")
            {
                MessageBox.Show("Все поля должны быть заполнены",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            else
            {
                using (var db = new BAZABBIBLIOTEKAEntities())
                {
                    // Преобразуем строку даты в DateTime
                    DateTime dateOfBirth;
                    if (!DateTime.TryParse(DateBox.Text.Trim(), out dateOfBirth))
                    {
                        MessageBox.Show("Некорректная дата рождения",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                        return;
                    }

                    // Проверяем, существует ли читатель с такими данными
                    if (!db.Readers.Any(p => p.Last_name == LastnameCopy.Text &&
                                             p.First_name == NameBox.Text &&
                                             p.Patronymic == PatronymicBox.Text &&
                                             p.Date_birth == dateOfBirth))
                    {
                        // Проверяем, существует ли контакт с таким номером телефона
                        Contacts contact = db.Contacts.FirstOrDefault(c => c.Phone_number == NumberBox.Text.Trim());
                        if (contact == null)
                        {
                            // Создание и добавление нового контакта
                            contact = new Contacts { Phone_number = NumberBox.Text.Trim() };
                            db.Contacts.Add(contact);
                            db.SaveChanges(); // Сохраняем контакт, чтобы получить его ID
                        }

                        // Проверяем, существует ли адрес
                        Address address = db.Address.FirstOrDefault(a => a.Address1 == AdressBox.Text.Trim());
                        if (address == null)
                        {
                            // Создание и добавление нового адреса
                            address = new Address { Address1 = AdressBox.Text.Trim() };
                            db.Address.Add(address);
                            db.SaveChanges(); // Сохраняем адрес, чтобы получить его ID
                        }

                        // Создание и добавление нового читателя
                        Readers readers = new Readers
                        {
                            Last_name = LastnameCopy.Text.Trim(),
                            First_name = NameBox.Text.Trim(),
                            Patronymic = PatronymicBox.Text.Trim(),
                            Date_birth = dateOfBirth, // Используем уже преобразованное значение
                            id_phone = contact.id_phone, // Устанавливаем id_phone
                            id_address = address.id_address // Устанавливаем id_address
                        };

                        db.Readers.Add(readers);
                        db.SaveChanges();

                        MessageBox.Show("Данные успешно сохранены",
                                        "Сохранение",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Такой читатель уже существует",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
            }
        }



    }
}
