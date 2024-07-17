using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    public class LoanStorage
    {
        public int LoanStorageID { get; set; }
        public int InventoryBookStorageID { get; set; }
        public int StudentID { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Returned { get; set; }
    }
}
