using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kartoteka
{
    public partial class ReaderClass
    {
        public int i { get; set; } // Возможно, это не нужно, если это не используется
        public int id { get; set; } // ID читателя
        public string last_name { get; set; } // Фамилия
        public string first_name { get; set; } // Имя
        public string patronymic { get; set; } // Отчество
        public string phone_number { get; set; } // Номер телефона
        public string adress { get; set; } // Адрес
        public string Date_birth { get; set; } // Дата рождения

    }
}
