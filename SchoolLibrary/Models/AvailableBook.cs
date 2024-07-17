using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    public class AvailableBook
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public string ISBN { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public int Quantity { get; set; } // Количество доступных книг
    }
}
