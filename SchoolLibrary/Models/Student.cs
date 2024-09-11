using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace SchoolLibrary.Models
{
    // Модель данных Student, представляющая таблицу Student в базе данных   
    public class Student : IDeletable 
    {
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
        private string studentClass;
        private string prefix;
        private string address;
        private string phone;
        private bool isActive;

        public DateTime? lastClassUpdateDate { get; set; } // Поле для отслеживания последнего обновления класса       
        public DateTime? lastAgeUpdateDate { get; set; }   // Поле для отслеживания последнего обновления возраста

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
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                dateOfBirth = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Age)); // Обновляем возраст при изменении даты рождения
            }
        }

        public int Age => CalculateAge(); 

        private int CalculateAge()
        {
            DateTime today = DateTime.Today;

            // Проверяем, был ли возраст обновлен в текущем году
            if (lastAgeUpdateDate.HasValue && lastAgeUpdateDate.Value.Year == today.Year)
            {
                return DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
            }

            int age = DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

            // Обновляем дату последнего обновления возраста
            lastAgeUpdateDate = today;

            return age;
        }

        public string StudentClass
        {
            get => studentClass;
            set
            {
                studentClass = value;
                OnPropertyChanged();
            }
        }

        public string Prefix
        {
            get => prefix;
            set
            {
                prefix = value;
                OnPropertyChanged();
            }
        }

        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged();
            }
        }

        // Конструктор
        public Student()
        {
            // Инициализация или логика в конструкторе
            UpdateAge();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Delete(EntityContext context) // Удаление из класса
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

        public void UpdateAge()
        {
            DateTime today = DateTime.Today;

            // Проверяем, был ли возраст обновлен в текущем году
            if (lastAgeUpdateDate.HasValue && lastAgeUpdateDate.Value.Year == today.Year)
            {
                return; // Пропускаем обновление, если возраст уже был обновлен в этом году
            }

            // Логика обновления возраста
            int age = DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

            // Обновляем дату последнего обновления возраста
            lastAgeUpdateDate = today;
        }
    }
}
