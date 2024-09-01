using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.ViewModels
{
    public class PaginatedInventoryBookModel
    {
        public int Index { get; set; } // Новый индекс
        public int InventoryBookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public string ISBN { get; set; }
        //public string CategoryName { get; set; }
        public string GenreName { get; set; }
        public string SubjectName { get; set; }
    }
}
