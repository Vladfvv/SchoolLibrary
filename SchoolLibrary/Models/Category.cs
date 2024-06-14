using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    //модель данных Category, представляющая таблицу Category в базе данных
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        // свойство для связи с книгами
        public virtual ICollection<Book> Books { get; set; }

        // Конструктор по умолчанию
        public Category()
        {
        }
        // Конструктор
        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
