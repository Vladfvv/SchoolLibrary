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

        //переход читателей-школьников в следующий класс
        //public void UpdateStudentClasses()
        //{
        //    try
        //    {
        //        var students = context.Students.ToList();

        //        foreach (var student in students)
        //        {
        //            if (student.StudentClass == "11")
        //            {
        //                student.StudentClass = "Graduate";
        //                student.Prefix = "";
        //            }
        //            else if (int.TryParse(student.StudentClass, out int classNumber))
        //            {
        //                student.StudentClass = (classNumber + 1).ToString();
        //            }
        //        }

        //        context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка обновления классов студентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

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


        //public List<Book> InitBooksListForExport()
        //{
        //    List<Book> bookList = new List<Book>();

        //    try
        //    {
        //        // Загружаем книги и связанные данные
        //        context.Books
        //            .Include(b => b.Genre)
        //            .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
        //            .Load();

        //        // Группируем книги по ISBN
        //        var groupedBooks = context.Books.Local
        //            .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
        //            .GroupBy(x => x.InventoryBook.ISBN)
        //            .Select(g => new PaginatedBookInventoryModel // Или используйте другую подходящую модель, если PaginatedBookInventoryModel не подходит
        //            {
        //                BookID = g.First().Book.BookID,
        //                Title = g.First().InventoryBook.Title,
        //                Author = g.First().InventoryBook.Author,
        //                Publisher = g.First().InventoryBook.Publisher,
        //                YearPublished = g.First().InventoryBook.YearPublished,
        //                ISBN = g.Key,
        //                Quantity = g.Count(),
        //                QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
        //                GenreName = g.First().Book.Genre != null ? g.First().Book.Genre.GenreName : "Неизвестно",
        //                SubjectName = g.First().Book.Genre != null ? g.First().Book.Genre.GenreName : "Неизвестно"
        //            })
        //            .ToList();


        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //    return bookList; // Можно вернуть bookList, если нужно использовать его в других частях программы
        //}

        //private void InitStudentsList()
        //{
        //    try
        //    {
        //        // Загружаем студентов
        //        context.Students.Load();

        //        // Подсчитываем общее количество студентов
        //        var totalStudents = context.Students.Local.Count;

        //        // Вычисляем общее количество страниц
        //        totalPages = (int)Math.Ceiling((double)totalStudents / PageSize);

        //        // Получаем студентов для текущей страницы
        //        var studentsOnPage = context.Students.Local
        //            .Skip((currentPage - 1) * PageSize)
        //            .Take(PageSize)
        //            .Select((student, index) => new PaginatedStudentModel//Select((student, index) => new { Index = (currentPage - 1) * PageSize + index + 1, ... }): В этом выражении Index вычисляется таким образом, чтобы он продолжал нумерацию между страницами
        //            {
        //                Index = (currentPage - 1) * PageSize + index + 1, // Вычисляем порядковый номер
        //                FirstName = student.FirstName,
        //                LastName = student.LastName,
        //                DateOfBirth = student.DateOfBirth,
        //                //Age = student.Age,
        //                StudentClass = student.StudentClass,
        //                Prefix = student.Prefix,
        //                Address = student.Address
        //            })
        //            .ToList();
        //        //Index - отображает порядковый номер строки, который продолжает нумерацию между страницами
        //        // Обновляем источник данных и колонки
        //        ConfigureStudentColumns();
        //        dGrid.ItemsSource = studentsOnPage;

        //        // Обновляем отображение DataGrid
        //        dGrid.Items.Refresh();

        //        // Обновляем видимость и активное состояние кнопок пагинации
        //        UpdatePaginationButtons(totalPages);


        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
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

        //public List<PaginatedStudentModel> GetPaginatedStudentsWithoutLoans()
        //{
        //    List<Student> studentsWithoutLoans = studentService.GetStudentsWithoutLoans();
        //    List<PaginatedStudentModel> paginatedStudentsWithoutLoans = studentsWithoutLoans
        //        .OrderBy(s => s.LastName)
        //        .ThenBy(s => s.FirstName)
        //        .Select((student, index) => new PaginatedStudentModel
        //        {
        //            StudentID = student.StudentID,
        //            FirstName = student.FirstName,
        //            LastName = student.LastName,
        //            DateOfBirth = student.DateOfBirth,
        //            StudentClass = student.StudentClass,
        //            Prefix = student.Prefix,
        //            Address = student.Address,
        //            Index = index + 1 + (currentPage - 1) * PageSize
        //        })
        //        .ToList();
        //    return paginatedStudentsWithoutLoans;
        //}

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
