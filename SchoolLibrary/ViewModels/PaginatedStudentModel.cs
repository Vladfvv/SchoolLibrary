using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.ViewModels
{
    public class PaginatedStudentModel
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
        public DateTime DateOfBirth { get; set; }
        public string StudentClass { get; set; }
        public string Prefix { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        // Поле для для отображения порядкового номера
        public int Index { get; set; }

        public static PaginatedStudentModel ConvertToPaginatedStudentModel(Student student)
        {
            return new PaginatedStudentModel
            {
                StudentID = student.StudentID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                StudentClass = student.StudentClass,
                Prefix = student.Prefix,
                Address = student.Address,              
                Phone = student.Phone              
            };
        }

        // Вычисляемое свойство Age
        public int Age
        {
            get
            {
                if (DateOfBirth == default(DateTime))
                    return 0; // Возраст не определен, если дата не задана

                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }

}
