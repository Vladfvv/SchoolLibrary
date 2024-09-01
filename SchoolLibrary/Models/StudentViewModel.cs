using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    public class StudentViewModel
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private DateTime dateOfBirth { get; set; }
        public string StudentClass { get; set; }
        public string Prefix { get; set; }
        public string Address { get; set; }
        private string Phone { get; set; }

        // Поле для для отображения порядкового номера
        public int Index { get; set; } 
    }
}
