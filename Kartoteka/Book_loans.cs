//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kartoteka
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book_loans
    {
        public int id_book_loan { get; set; }
        public int id_book { get; set; }
        public int id_reader { get; set; }
        public System.DateTime Date_loan { get; set; }
        public int admin_id { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual Books Books { get; set; }
        public virtual Readers Readers { get; set; }
    }
}
