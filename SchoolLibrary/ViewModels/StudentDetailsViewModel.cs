using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.ViewModels
{
    public class StudentDetailsViewModel
    {
        public Student Student { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
