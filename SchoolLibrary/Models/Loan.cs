using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    //модель данных Loan, представляющая таблицу Loan в базе данных

    public class Loan
    {
        public int LoanID { get; set; }
        public int BookID { get; set; }
        public int StudentID { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Returned { get; set; }
    }
}
