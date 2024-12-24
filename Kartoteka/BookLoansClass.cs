    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Kartoteka
    {
        public partial class BookLoansClass
        {
            public int i { get; set; }
            public int id_book_loan { get; set; }
            public int admin_id { get; set; }
            public int id_book { get; set; }
            public int id_reader { get; set; }
            public string Date_loan { get; set; }
            public string user_Name { get; set; }
            public string reader_last_name { get; set; }
            public string reader_first_name { get; set; }
            public string book_title { get; set; }
        }
    }
