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
    
    public partial class Readers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Readers()
        {
            this.Book_loans = new HashSet<Book_loans>();
        }
    
        public int id_reader { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public System.DateTime Date_birth { get; set; }
        public int id_address { get; set; }
        public int id_phone { get; set; }
    
        public virtual Address Address { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book_loans> Book_loans { get; set; }
        public virtual Contacts Contacts { get; set; }
    }
}
