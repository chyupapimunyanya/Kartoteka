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

namespace Kartoteka.pages
{
    /// <summary>
    /// Логика взаимодействия для Readers1.xaml
    /// </summary>
    public partial class Readers1 : Page
    {
        public Readers1()
        {
            InitializeComponent();
            FillListView();
        }

        private void FillListView()
        {
            using (var db = new BAZABBIBLIOTEKAEntities())
            {
                var reader = db.Readers.ToList();
                var contact = db.Contacts.ToList();
                var adres = db.Address.ToList();
                var pca = reader.Join(contact,
                    Readers => Readers.id_phone,
                    Contacts => Contacts.id_phone,
                    (Readers, Contacts) => new { Readers1 = Readers, Contacts1 = Contacts })
                    .Join(adres,
                    ReadContacts => ReadContacts.Readers1.id_address,
                    Address => Address.id_address,
                    (ReadContacts, Address) => new { readContacts = ReadContacts, address = Address });
                var readObjects = pca.Select(co => new ReaderClass
                {
                    id = co.readContacts.Readers1.id_reader,
                    last_name = co.readContacts.Readers1.Last_name,
                    first_name = co.readContacts.Readers1.First_name,
                    phone_number = co.readContacts.Contacts1.Phone_number,
                    Date_birth = co.readContacts.Readers1.Date_birth.ToString("d"),
                    adress = co.address.Address1,
                }).ToList();
                int i = 1;
                foreach (var reader1 in readObjects)
                {
                    reader1.i = i++;
                }
                GridView myGridView = new GridView();
                myGridView.ColumnHeaderToolTip = "Читатели";
                listView.ItemsSource = readObjects;

                GridViewColumn gvc1 = new GridViewColumn();
                gvc1.DisplayMemberBinding = new Binding("i");
                gvc1.Header = "Номер";

                GridViewColumn gvc2 = new GridViewColumn();
                gvc2.DisplayMemberBinding = new Binding("last_name");
                gvc2.Header = "Фамилию";

                GridViewColumn gvc3 = new GridViewColumn();
                gvc3.DisplayMemberBinding = new Binding("first_name");
                gvc3.Header = "Имя";

                GridViewColumn gvc4 = new GridViewColumn();
                gvc4.DisplayMemberBinding = new Binding("phone_number");
                gvc4.Header = "Номер телефона";

                GridViewColumn gvc5 = new GridViewColumn();
                gvc5.DisplayMemberBinding = new Binding("Date_birth");
                gvc5.Header = "Дата рождения";

                GridViewColumn gvc6 = new GridViewColumn();
                gvc6.DisplayMemberBinding = new Binding("adress");
                gvc6.Header = "Адрес";

                myGridView.Columns.Add(gvc1);
                myGridView.Columns.Add(gvc2);
                myGridView.Columns.Add(gvc3);
                myGridView.Columns.Add(gvc4);
                myGridView.Columns.Add(gvc5);
                myGridView.Columns.Add(gvc6);
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
                ReaderClass selectedRead = (ReaderClass)listView.SelectedItem;
                if (selectedRead != null)
                {
                    int readId = selectedRead.id;
                    string addressText = selectedRead.adress;
                    string phoneText = selectedRead.phone_number;
                    using (var db = new BAZABBIBLIOTEKAEntities())
                    {
                        if (db.Book_loans.Any(o => o.id_reader == readId))
                        {
                            MessageBox.Show("Сначала удалите книгу, выданную этому читателю",
                                                                "Ошибка",
                                                                 MessageBoxButton.OK,
                                                                 MessageBoxImage.Error);
                        }
                        else
                        {
                            Readers readers = db.Readers.FirstOrDefault(a => a.id_reader == readId);
                            Address address = db.Address.FirstOrDefault(c => c.Address1 == addressText);
                            Contacts contacts = db.Contacts.FirstOrDefault(p => p.Phone_number == phoneText);
                            db.Readers.Remove(readers);
                            db.Address.Remove(address);
                            db.Contacts.Remove(contacts);
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

        private void vidachaBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы точно хотите выдать книгу этому читателю?",
                                                      "Предупреждение",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                ReaderClass selectedRead = (ReaderClass)listView.SelectedItem;
                if (selectedRead != null)
                {
                    int readId = selectedRead.id;
                    ReaderClassAdditional.last_name = selectedRead.last_name;
                    ReaderClassAdditional.first_name = selectedRead.first_name;
                    ReaderClassAdditional.id = readId;
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
            AddReader1 addReader1 = new AddReader1();
            addReader1.ShowDialog();
        }
    }
}
