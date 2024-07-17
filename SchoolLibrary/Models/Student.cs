using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.Runtime.CompilerServices;





namespace SchoolLibrary.Models
{
    //модель данных Student, представляющая таблицу Student в базе данных   
    public class Student : IDeletable//, IDataErrorInfo
    {
        private int age;
        private string firstName;
        private string lastName;
        private string studentClass;
        private bool isStudent;
        public int StudentID { get; set; }

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        public string Class
        {
            get => studentClass;
            set
            {
                studentClass = value;
                OnPropertyChanged();
            }
        }

        public int Age
        {
            get => age;
            set
            {
                age = value;
                OnPropertyChanged();
            }
        }

        public bool IsStudent
        {
            get => isStudent;
            set
            {
                isStudent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Delete(EntityContext context)//deleting from class
        {
            if (context.Loans.Any(b => b.StudentID == this.StudentID))
            {
                throw new InvalidOperationException("Невозможно удалить читателя, так как он связан с данными по заему книги.");
            }
            context.Students.Remove(this);
            context.SaveChanges();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        /*
        public string this[string columnName]
        {
            get
            {
                string error = null;
                switch (columnName)
                {
                    case nameof(FirstName):
                        if (string.IsNullOrWhiteSpace(FirstName))
                            error = "Имя не должно быть пустым";
                        break;
                    case nameof(LastName):
                        if (string.IsNullOrWhiteSpace(LastName))
                            error = "Фамилия не должна быть пустой";
                        break;
                    case nameof(Age):
                        if (Age < 5)
                            error = "Возраст должен быть больше 5 лет";
                        break;
                    case nameof(Class):
                        if (string.IsNullOrWhiteSpace(Class))
                            error = "Класс не должен быть пустым";
                        break;
                }
                return error;
            }
        }*/

        //public string Error => null;
    }
}
