using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace SchoolLibrary.DialogWindows.LoanWindows
{
    public partial class LoanDialog : Window
    {
        private readonly EntityContext _context;
        private List<Book> _books;
        private List<Student> _students;

        public LoanDialog(EntityContext context)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _context = context;
            LoadBooks();
            //LoadStudents();
            LoadActiveStudents();
            LoanDatePicker.SelectedDate = DateTime.Today;
            LoanDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
            LoanDatePicker.FirstDayOfWeek = DayOfWeek.Monday;
            DueDatePicker.SelectedDate = DateTime.Today.AddDays(14);
            DueDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
            DueDatePicker.FirstDayOfWeek = DayOfWeek.Monday;
        }

        private void LoadBooks()
        {
            _books = _context.Books.Include(b => b.Genre)
                                   .Include(b => b.InventoryBooks)
                                   .Where(b => b.QuantityLeft > 0) // Выбираем только книги с положительным QuantityLeft
                                   .ToList();

            var bookInventoryViewModels = _books.Select(book => new BookInventoryViewModel
            {
                BookID = book.BookID,
                InventoryBookID = book.InventoryBooks.FirstOrDefault()?.InventoryBookID ?? 0,
                Title = book.InventoryBooks.FirstOrDefault()?.Title ?? string.Empty,
                Author = book.InventoryBooks.FirstOrDefault()?.Author ?? string.Empty,
                Publisher = book.InventoryBooks.FirstOrDefault()?.Publisher ?? string.Empty,
                YearPublished = book.InventoryBooks.FirstOrDefault()?.YearPublished ?? string.Empty,
                ISBN = book.InventoryBooks.FirstOrDefault()?.ISBN ?? string.Empty,
                Quantity = book.Quantity,
                QuantityLeft = book.QuantityLeft,
                CategoryName = book.Genre?.GenreName ?? string.Empty
            }).ToList();

            BooksDataGrid.ItemsSource = bookInventoryViewModels;
        }

        private void LoadStudents()
        {
            _students = _context.Students.ToList();
            StudentsDataGrid.ItemsSource = _students;
        }

        private void LoadActiveStudents()
        {
            try
            {
                // Получаем только активных студентов из базы данных
                var activeStudents = _context.Students.Where(s => s.IsActive).ToList();

                // Привязываем отфильтрованный список к DataGrid
                StudentsDataGrid.ItemsSource = activeStudents;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных студентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обработка выбора книги
        }

        private void StudentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem != null)
            {
                Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
                ShowStudentDetailsWindow(selectedStudent);
            }
        }

        private void ShowStudentDetailsWindow(Student student)
        {
            StudentDetailsWindow detailsWindow = new StudentDetailsWindow(student);
            detailsWindow.Owner = this;
            detailsWindow.ShowDialog();
        }



        private void LoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите книгу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (StudentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите читателя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(BooksDataGrid.SelectedItem is BookInventoryViewModel selectedBook))
            {
                MessageBox.Show("Ошибка выбора книги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedBook.QuantityLeft <= 0)
            {
                MessageBox.Show("Выбраная книга отсутствует в библиотеке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(StudentsDataGrid.SelectedItem is Student selectedStudent))
            {
                MessageBox.Show("Ошибка выбора читателя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //DateTime loanDate = LoanDatePicker.SelectedDate ?? DateTime.Today;
            DateTime loanDate = DateTime.Now;
            DateTime dueDate = DueDatePicker.SelectedDate ?? DateTime.Now.AddDays(14);

            // Найти следующую свободную книгу с этим ISBN
            var availableBook = _context.InventoryBooks
                .Where(b => b.BookID == selectedBook.BookID && !_context.Loans.Any(l => l.InventoryBookID == b.InventoryBookID && !l.Returned))
                .FirstOrDefault();

            if (availableBook == null)
            {
                MessageBox.Show("Нет доступных книг.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Loan newLoan = new Loan
            {
                InventoryBookID = availableBook.InventoryBookID,
                StudentID = selectedStudent.StudentID,
                LoanDate = loanDate,
                DueDate = dueDate,
                Returned = false
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Loans.Add(newLoan);
                    _context.SaveChanges();

                    var book = _context.Books.Find(selectedBook.BookID);
                    if (book != null)
                    {
                        book.QuantityLeft -= 1;
                        _context.Entry(book).State = EntityState.Modified;
                        _context.SaveChanges();
                    }

                    transaction.Commit();

                    // MessageBox.Show("Book loaned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Используем формат "день/месяц/год часы:минуты:секунды" для времени займа
                    string loanDateFormatted = loanDate.ToString("dd/MM/yyyy HH:mm:ss");

                    MessageBox.Show($"Книга успешно выдана: {loanDateFormatted}.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshBooksData(); // Обновляем данные после оформления займа
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Ошибка выдачи книги: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        private void RefreshBooksData()
        {
            var books = _context.Books.Include(b => b.Genre)
                                       .Include(b => b.InventoryBooks)
                                       .Where(b => b.QuantityLeft > 0)
                                       .ToList();

            var bookInventoryViewModels = books.Select(book => new BookInventoryViewModel
            {
                BookID = book.BookID,
                InventoryBookID = book.InventoryBooks.FirstOrDefault()?.InventoryBookID ?? 0,
                Title = book.InventoryBooks.FirstOrDefault()?.Title ?? string.Empty,
                Author = book.InventoryBooks.FirstOrDefault()?.Author ?? string.Empty,
                Publisher = book.InventoryBooks.FirstOrDefault()?.Publisher ?? string.Empty,
                YearPublished = book.InventoryBooks.FirstOrDefault()?.YearPublished ?? string.Empty,
                ISBN = book.InventoryBooks.FirstOrDefault()?.ISBN ?? string.Empty,
                Quantity = book.Quantity,
                QuantityLeft = book.QuantityLeft,
                CategoryName = book.Genre?.GenreName ?? string.Empty
            }).ToList();

            BooksDataGrid.ItemsSource = bookInventoryViewModels;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        
    }

   
}
