using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.ViewModels
{
    public class PaginatedLoanBookStudentViewModel
    {
        public int Index { get; set; } 
        public int LoanID { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Returned { get; set; }
    }
}
