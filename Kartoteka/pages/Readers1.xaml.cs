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
        }

        private void chitsaveBtn_Click(object sender, RoutedEventArgs e)
        {
            AddReader1 addReader1 = new AddReader1();
            addReader1.ShowDialog();
        }
    }
}
