using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Library.Entities
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            Books = new HashSet<Book>();
            BookLendings = new HashSet<BookLending>();
        }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<BookLending> BookLendings { get; set; }
    }
}
