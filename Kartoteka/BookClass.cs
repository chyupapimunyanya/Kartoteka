using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka
{
    public class BookClass
    {

        public int i { get; set; }
        public int id_book { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year_publication { get; set; }
        public string Genre { get; set; }
        public int Copies { get; set; } // Общее количество экземпляров книги
    }
}
