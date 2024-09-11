using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using iTextSharp.text;


namespace SchoolLibrary.Service
{
    public class StudentService
    {
        private readonly EntityContext context;
        

        public StudentService(EntityContext dbContext)
        {
            this.context = dbContext;
        }

        public IEnumerable<Student> SearchStudents(string firstName, string lastName, String MinAgeTextBox, String MaxAgeTextBox, String MinClassTextBox, String MaxClassTextBox)
        {
            var query = context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(s => s.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(s => s.LastName.Contains(lastName));
            }
            if (!string.IsNullOrEmpty(MinAgeTextBox))
            {
                query = query.Where(s => s.LastName.Contains(MinAgeTextBox));
            }
            if (!string.IsNullOrEmpty(MaxAgeTextBox))
            {
                query = query.Where(s => s.LastName.Contains(MaxAgeTextBox));
            }
            if (!string.IsNullOrEmpty(MinClassTextBox))
            {
                query = query.Where(s => s.LastName.Contains(MinClassTextBox));
            }
            if (!string.IsNullOrEmpty(MaxClassTextBox))
            {
                query = query.Where(s => s.LastName.Contains(MaxClassTextBox));
            }
            return query.ToList();
        }

       
        public void UpdateStudentClasses()
        {
            try
            {
                var students = context.Students.ToList();
                DateTime today = DateTime.Today;

                foreach (var student in students)
                {
                    // Проверка, был ли студент уже обновлен в текущем году
                    if (student.lastClassUpdateDate.HasValue && student.lastClassUpdateDate.Value.Year == today.Year)
                    {
                        continue; // Пропускаем студента, если его класс уже был обновлен в этом году
                    }

                    if (student.StudentClass == "11")
                    {
                        student.StudentClass = "Graduate";
                        student.Prefix = "";
                    }
                    else if (int.TryParse(student.StudentClass, out int classNumber))
                    {
                        student.StudentClass = (classNumber + 1).ToString();
                    }

                    // Обновляем дату последнего обновления класса
                    student.lastClassUpdateDate = today;
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления классов студентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      
        public List<PaginatedStudentModel> GetStudentsListForExport()
        {
            List<PaginatedStudentModel> studentsList = new List<PaginatedStudentModel>();

            try
            {
                // Загружаем студентов в память
                context.Students.Load();

                // Получаем всех студентов и добавляем индексацию
                var students = context.Students.Local
                    .Select((student, index) => new PaginatedStudentModel
                    {
                        Index = index + 1, // Индекс начинается с 1
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        DateOfBirth = student.DateOfBirth,
                        StudentClass = student.StudentClass,
                        Prefix = student.Prefix,
                        Address = student.Address
                    })
                    .ToList();

                // Добавляем всех студентов в список для экспорта
                studentsList.AddRange(students);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return studentsList; // Возвращаем список студентов
        }

        public List<PaginatedStudentModel> GetStudentsWithoutLoans1()
        {
            // Выполняем запрос к базе данных, получаем студентов без займов
            var studentsWithoutLoans = context.Students
                .Where(s => !context.Loans.Any(l => l.StudentID == s.StudentID))
                .ToList() // Загружаем данные в память
                .Select(s => PaginatedStudentModel.ConvertToPaginatedStudentModel(s)) // Преобразуем в PaginatedStudentModel
                .ToList();

            return studentsWithoutLoans;
        }

        public List<Student> GetStudentsWithoutLoans()
        {
            // Выполняем запрос к базе данных, получаем студентов без займов
            var studentsWithoutLoans = context.Students
                .Where(s => !context.Loans.Any(l => l.StudentID == s.StudentID))
                .ToList(); // Загружаем данные в память

            return studentsWithoutLoans;
        }
                

        public List<PaginatedStudentModel> GetPaginatedStudentsWithoutLoans(int currentPage, int pageSize)
        {
            // Используем GetStudentsWithoutLoans для получения студентов без займов
            List<Student> studentsWithoutLoans = GetStudentsWithoutLoans();

            List<PaginatedStudentModel> paginatedStudentsWithoutLoans = studentsWithoutLoans
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select((student, index) => new PaginatedStudentModel
                {
                    StudentID = student.StudentID,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    StudentClass = student.StudentClass,
                    Prefix = student.Prefix,
                    Address = student.Address,
                    Index = index + 1 + (currentPage - 1) * pageSize
                })
                .ToList();

            return paginatedStudentsWithoutLoans;
        }

    }
}
