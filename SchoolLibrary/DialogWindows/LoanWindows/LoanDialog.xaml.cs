
//using SchoolLibrary.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Markup;

//namespace SchoolLibrary.DialogWindows.LoanWindows
//{
//    public partial class LoanDialog : Window
//    {
//        private readonly EntityContext _context;
//        private List<Book> _books;
//        private List<Student> _students;

//        public LoanDialog(EntityContext context)
//        {
//            InitializeComponent();
//            _context = context;
//            LoadBooks();
//            LoadStudents();
//            LoanDatePicker.SelectedDate = DateTime.Today;
//            LoanDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
//            DueDatePicker.SelectedDate = DateTime.Today.AddDays(14);
//            DueDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
//        }

//        //private void LoadBooks()
//        //{
//        //    _books = _context.Books.Include(b => b.Category)
//        //                           .Include(b => b.InventoryBooks)
//        //                           .ToList();

//        //    var bookInventoryViewModels = _books.Select(book => new BookInventoryViewModel
//        //    {
//        //        BookID = book.BookID,
//        //        Title = book.InventoryBooks.FirstOrDefault()?.Title ?? string.Empty,
//        //        Author = book.InventoryBooks.FirstOrDefault()?.Author ?? string.Empty,
//        //        Publisher = book.InventoryBooks.FirstOrDefault()?.Publisher ?? string.Empty,
//        //        YearPublished = book.InventoryBooks.FirstOrDefault()?.YearPublished ?? string.Empty,
//        //        ISBN = book.InventoryBooks.FirstOrDefault()?.ISBN ?? string.Empty,
//        //        Quantity = book.Quantity,
//        //        QuantityLeft = book.QuantityLeft,
//        //        CategoryName = book.Category?.CategoryName ?? string.Empty
//        //    }).ToList();

//        //    BooksDataGrid.ItemsSource = bookInventoryViewModels;
//        //}

//        private void LoadBooks()
//        {
//            _books = _context.Books.Include(b => b.Category)
//                                   .Include(b => b.InventoryBooks)
//                                   .Where(b => b.QuantityLeft > 0) // Выбираем только книги с положительным QuantityLeft
//                                   .ToList();

//            var bookInventoryViewModels = _books.Select(book => new BookInventoryViewModel
//            {
//                BookID = book.BookID,
//                Title = book.InventoryBooks.FirstOrDefault()?.Title ?? string.Empty,
//                Author = book.InventoryBooks.FirstOrDefault()?.Author ?? string.Empty,
//                Publisher = book.InventoryBooks.FirstOrDefault()?.Publisher ?? string.Empty,
//                YearPublished = book.InventoryBooks.FirstOrDefault()?.YearPublished ?? string.Empty,
//                ISBN = book.InventoryBooks.FirstOrDefault()?.ISBN ?? string.Empty,
//                Quantity = book.Quantity,
//                QuantityLeft = book.QuantityLeft,
//                CategoryName = book.Category?.CategoryName ?? string.Empty
//            }).ToList();

//            BooksDataGrid.ItemsSource = bookInventoryViewModels;
//        }

//        private void LoadStudents()
//        {
//            _students = _context.Students.ToList();
//            StudentsDataGrid.ItemsSource = _students;
//        }


//        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            // Обработка выбора книги, если нужно
//        }


//        private void StudentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            if (StudentsDataGrid.SelectedItem != null)
//            {
//                Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
//                ShowStudentDetailsWindow(selectedStudent);
//            }
//        }

//        private void ShowStudentDetailsWindow(Student student)
//        {
//            StudentDetailsWindow detailsWindow = new StudentDetailsWindow(student);
//            detailsWindow.Owner = this;
//            detailsWindow.ShowDialog();
//        }

//        //private void LoanButton_Click(object sender, RoutedEventArgs e)
//        //{
//        //    if (BooksDataGrid.SelectedItem == null || StudentsDataGrid.SelectedItem == null)
//        //    {
//        //        MessageBox.Show("Please select a book and a student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//        //        return;
//        //    }

//        //    BookInventoryViewModel selectedBook = (BookInventoryViewModel)BooksDataGrid.SelectedItem;
//        //    Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
//        //    DateTime loanDate = LoanDatePicker.SelectedDate ?? DateTime.Today;
//        //    DateTime dueDate = DueDatePicker.SelectedDate ?? DateTime.Today.AddDays(14);

//        //    Loan newLoan = new Loan
//        //    {
//        //        InventoryBookID = selectedBook.BookID,
//        //        StudentID = selectedStudent.StudentID,
//        //        LoanDate = loanDate,
//        //        DueDate = dueDate,
//        //        Returned = false
//        //    };

//        //    using (var transaction = _context.Database.BeginTransaction())
//        //    {
//        //        try
//        //        {
//        //            _context.Loans.Add(newLoan);
//        //            _context.SaveChanges();

//        //            var book = _context.Books.Find(selectedBook.BookID);
//        //            book.QuantityLeft -= 1;
//        //            _context.Entry(book).State = EntityState.Modified;
//        //            _context.SaveChanges();

//        //            transaction.Commit();

//        //            MessageBox.Show("Book loaned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
//        //            this.DialogResult = true;
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            transaction.Rollback();
//        //            MessageBox.Show("An error occurred while processing the loan: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//        //        }
//        //    }
//        //}


//        private void LoanButton_Click(object sender, RoutedEventArgs e)
//        {
//            if (BooksDataGrid.SelectedItem == null || StudentsDataGrid.SelectedItem == null)
//            {
//                MessageBox.Show("Please select a book and a student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//                return;
//            }

//            BookInventoryViewModel selectedBook = (BookInventoryViewModel)BooksDataGrid.SelectedItem;
//            if (selectedBook.QuantityLeft <= 0)
//            {
//                MessageBox.Show("Selected book is out of stock.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//                return;
//            }

//            Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
//            DateTime loanDate = LoanDatePicker.SelectedDate ?? DateTime.Today;
//            DateTime dueDate = DueDatePicker.SelectedDate ?? DateTime.Today.AddDays(14);

//            Loan newLoan = new Loan
//            {
//                InventoryBookID = selectedBook.BookID,
//                StudentID = selectedStudent.StudentID,
//                LoanDate = loanDate,
//                DueDate = dueDate,
//                Returned = false
//            };

//            using (var transaction = _context.Database.BeginTransaction())
//            {
//                try
//                {
//                    _context.Loans.Add(newLoan);
//                    _context.SaveChanges();

//                    var book = _context.Books.Find(selectedBook.BookID);
//                    book.QuantityLeft -= 1;
//                    _context.Entry(book).State = EntityState.Modified;
//                    _context.SaveChanges();

//                    transaction.Commit();

//                    MessageBox.Show("Book loaned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
//                    RefreshBooksData(); // Обновляем данные после оформления займа
//                    this.DialogResult = true;
//                }
//                catch (Exception ex)
//                {
//                    transaction.Rollback();
//                    MessageBox.Show("An error occurred while processing the loan: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//                }
//            }
//        }


//        private void RefreshBooksData()
//        {
//            var books = _context.Books.Include(b => b.Category)
//                                       .Include(b => b.InventoryBooks)
//                                       .Where(b => b.QuantityLeft > 0)
//                                       .ToList();

//            var bookInventoryViewModels = books.Select(book => new BookInventoryViewModel
//            {
//                BookID = book.BookID,
//                Title = book.InventoryBooks.FirstOrDefault()?.Title ?? string.Empty,
//                Author = book.InventoryBooks.FirstOrDefault()?.Author ?? string.Empty,
//                Publisher = book.InventoryBooks.FirstOrDefault()?.Publisher ?? string.Empty,
//                YearPublished = book.InventoryBooks.FirstOrDefault()?.YearPublished ?? string.Empty,
//                ISBN = book.InventoryBooks.FirstOrDefault()?.ISBN ?? string.Empty,
//                Quantity = book.Quantity,
//                QuantityLeft = book.QuantityLeft,
//                CategoryName = book.Category?.CategoryName ?? string.Empty
//            }).ToList();

//            BooksDataGrid.ItemsSource = bookInventoryViewModels;
//        }

//        private void CancelButton_Click(object sender, RoutedEventArgs e)
//        {
//            this.DialogResult = false;
//        }

//        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (e.ChangedButton == MouseButton.Left)
//            {
//                this.DragMove();
//            }
//        }
//    }
//}



using SchoolLibrary.Models;
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
            _context = context;
            LoadBooks();
            LoadStudents();
            LoanDatePicker.SelectedDate = DateTime.Today;
            LoanDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
            DueDatePicker.SelectedDate = DateTime.Today.AddDays(14);
            DueDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
        }

        private void LoadBooks()
        {
            _books = _context.Books.Include(b => b.Category)
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
                CategoryName = book.Category?.CategoryName ?? string.Empty
            }).ToList();

            BooksDataGrid.ItemsSource = bookInventoryViewModels;
        }

        private void LoadStudents()
        {
            _students = _context.Students.ToList();
            StudentsDataGrid.ItemsSource = _students;
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

        /*       private void LoanButton_Click(object sender, RoutedEventArgs e)
               {
                   if (BooksDataGrid.SelectedItem == null || StudentsDataGrid.SelectedItem == null)
                   {
                       MessageBox.Show("Please select a book and a student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                       return;
                   }

                   BookInventoryViewModel selectedBook = (BookInventoryViewModel)BooksDataGrid.SelectedItem;
                   if (selectedBook.QuantityLeft <= 0)
                   {
                       MessageBox.Show("Selected book is out of stock.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                       return;
                   }

                   Student selectedStudent = (Student)StudentsDataGrid.SelectedItem;
                   DateTime loanDate = LoanDatePicker.SelectedDate ?? DateTime.Today;
                   DateTime dueDate = DueDatePicker.SelectedDate ?? DateTime.Today.AddDays(14);

                   Loan newLoan = new Loan
                   {
                       InventoryBookID = selectedBook.InventoryBookID,
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
                           book.QuantityLeft -= 1;
                           _context.Entry(book).State = EntityState.Modified;
                           _context.SaveChanges();

                           transaction.Commit();

                           MessageBox.Show("Book loaned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                           RefreshBooksData(); // Обновляем данные после оформления займа
                           this.DialogResult = true;
                       }
                       catch (Exception ex)
                       {
                           transaction.Rollback();
                           MessageBox.Show("An error occurred while processing the loan: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                       }
                   }
               }*/


        private void LoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem == null || StudentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a book and a student.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(BooksDataGrid.SelectedItem is BookInventoryViewModel selectedBook))
            {
                MessageBox.Show("Invalid book selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedBook.QuantityLeft <= 0)
            {
                MessageBox.Show("Selected book is out of stock.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(StudentsDataGrid.SelectedItem is Student selectedStudent))
            {
                MessageBox.Show("Invalid student selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime loanDate = LoanDatePicker.SelectedDate ?? DateTime.Today;
            DateTime dueDate = DueDatePicker.SelectedDate ?? DateTime.Today.AddDays(14);

            Loan newLoan = new Loan
            {
                InventoryBookID = selectedBook.InventoryBookID,
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

                    MessageBox.Show("Book loaned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshBooksData(); // Обновляем данные после оформления займа
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("An error occurred while processing the loan: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void RefreshBooksData()
        {
            var books = _context.Books.Include(b => b.Category)
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
                CategoryName = book.Category?.CategoryName ?? string.Empty
            }).ToList();

            BooksDataGrid.ItemsSource = bookInventoryViewModels;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }

    public class BookInventoryViewModel
    {
        public int BookID { get; set; }
        public int InventoryBookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public int QuantityLeft { get; set; }
        public string CategoryName { get; set; }
    }
}
