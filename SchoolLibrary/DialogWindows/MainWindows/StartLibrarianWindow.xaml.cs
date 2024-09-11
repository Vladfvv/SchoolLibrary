using SchoolLibrary.AuthWindows;
using SchoolLibrary.DialogWindows.Operations;
using SchoolLibrary.DialogWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;
using SchoolLibrary.DialogWindows.LoanWindows;
using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
using SchoolLibrary.DialogWindows.BookWindows;
using System.Runtime.Remoting.Contexts;
using SchoolLibrary.DialogWindows.StudentWindows;
using System.Diagnostics;
using SchoolLibrary.DialogWindows.Statistic;
using SchoolLibrary.DialogWindows.CategoryWindows;
using SchoolLibrary.DialogWindows.GenreWindows;
using SchoolLibrary.DialogWindows.SubjectWindows;
using Microsoft.Win32;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using SchoolLibrary.Service;
using SchoolLibrary.Converters;
using NLog;


namespace SchoolLibrary.Views
{
    //создание псевдонима для типа SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel.
    //Это позволяет использовать псевдоним LoanWindowsViewModel вместо полного имени типа SchoolLibrary.   //DialogWindows.LoanWindows.BookInventoryViewModel
    using LoanWindowsViewModel = SchoolLibrary.ViewModels.BookInventoryViewModel;
    using ModelsViewModel = SchoolLibrary.ViewModels.PaginatedBookInventoryModel;
    public partial class StartLibrarianWindow : BaseWindow
    {
        readonly StudentService studentService;
        readonly InventoryBookService inventoryBookService;
        readonly BookService bookService;
        readonly LoansService loanService;
        readonly String nameBooklist = "Books";
        readonly PaginatedBookInventoryModel groupedBooks;
        private List<object> listForExport = new List<object>();//список для хранения данных на экспорт
        private readonly List<Student> studentsWithoutLoans; // Объявляем переменную на уровне класса
        public StartLibrarianWindow(EntityContext context) : base(context)
        {
            InitializeComponent();
            this.context = context;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, WindowBinding_Executed));
            studentService = new StudentService(context);
            inventoryBookService = new InventoryBookService(context);
            bookService = new BookService(context);
            BarcodeTextBox.Visibility = Visibility.Hidden;
        }



        private void UpdatePaginationButtons(int pageCount)
        {
            // Обновляем количество страниц и текущую страницу
            totalPages = pageCount;

            // Устанавливаем видимость и активность кнопок в зависимости от количества страниц
            if (totalPages <= 1)
            {
                // Если страниц 1 или меньше, прячем обе кнопки
                btnPrevious.Visibility = Visibility.Collapsed;
                btnNext.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Иначе показываем кнопки, если есть более одной страницы
                btnPrevious.Visibility = currentPage > 1 ? Visibility.Visible : Visibility.Collapsed;
                btnNext.Visibility = currentPage < totalPages ? Visibility.Visible : Visibility.Collapsed;
            }

            // Управляем активностью кнопок
            btnPrevious.IsEnabled = currentPage > 1;
            btnNext.IsEnabled = currentPage < totalPages;
        }

        private void btnListBooks(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            dGrid.Tag = "Books"; // Устанавливаем тег для идентификации текущих данных
            dGrid.ContextMenu = null;
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Перечень книг";
            currentListType = "Books";
            updateListBooks();
            BarcodeTextBox.Visibility = Visibility.Hidden;
            // Инициализация контекстного меню для книг
            InitializeContextMenuForBooks();
        }

        private void updateListBooks()
        {
            try
            {
                currentPage = 1;
                totalPages = 1;
                dGrid.ItemsSource = null;
                currentListType = nameBooklist;
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateListStudent()
        {
            try
            {
                dGrid.ItemsSource = null;
                currentListType = "Students";
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void updateListCategories()
        {
            try
            {
                dGrid.ItemsSource = null;
                currentListType = "Category";
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateListGenres()
        {
            try
            {
                dGrid.ItemsSource = null;
                currentListType = "Genre";
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateListSubjects()
        {
            try
            {
                dGrid.ItemsSource = null;
                currentListType = "Subject";
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateStudentDataGrid(IEnumerable<PaginatedStudentModel> students)
        {
            dGrid.ItemsSource = students;
            dGrid.Items.Refresh(); // Обновление отображения данных

            // Обновляем количество страниц и текущую страницу
            totalPages = (int)Math.Ceiling((double)students.Count() / PageSize);
            currentPage = 1; // Начнем с первой страницы после поиска

            // Обновляем видимость и активность кнопок пагинации
            UpdatePaginationButtons(totalPages);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                InitList(currentListType);
                UpdatePaginationButtons(totalPages);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                InitList(currentListType);
                UpdatePaginationButtons(totalPages);
            }
        }

        private void updateListInventoryBook()
        {
            try
            {
                dGrid.ItemsSource = null;
                currentListType = "InventoryBooks";
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateListStudentsWithoutLoans()
        {
            try
            {
                dGrid.ItemsSource = null;
                currentListType = "StudentsWithoutLoans";
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void InitList(string listType)
        {
            switch (listType)
            {
                case "Books":
                    InitBooksList();
                    break;
                case "Students":
                    InitStudentsList();
                    break;
                case "InventoryBooks":
                    InitInventoryBooksList();
                    break;
                case "Genre":
                    InitGenresList();
                    break;
                case "Subject":
                    InitSubjectsList();
                    break;
                case "Loans":
                    InitLoansList();
                    break;
                case "StudentsWithoutLoans":
                    InitStudentsWithoutLoansList();
                    break;
                case "BooksNotLoaned":
                    InitBooksNotLoanedList();
                    break;
                default:
                    currentPage = 1;
                    totalPages = 1;
                    btnPrevious.Visibility = Visibility.Collapsed;
                    btnNext.Visibility = Visibility.Collapsed;
                    break;
            }
        }


        private void initListForExport(string listType)
        {

            switch (listType)
            {
                case "Books":
                    listForExport = bookService.GetBooksListForExport().Cast<object>().ToList();
                    break;
                case "Students":
                    //InitStudentsList();
                    listForExport = studentService.GetStudentsListForExport().Cast<object>().ToList();
                    break;
                case "InventoryBooks":
                    // InitInventoryBooksList();
                    listForExport = inventoryBookService.GetInventoryBooksListForExport().Cast<object>()
                .ToList();
                    break;
                case "Loans":
                    // InitLoansList();
                    listForExport = loanService.GetLoansListForExport().Cast<object>()
                .ToList();
                    break;
                case "Genre":
                    //InitGenresList();
                    // listForExport = genresService.InitGenresListForExport(); - нужно реализовать
                    break;
                case "Subject":
                    //InitSubjectsList();
                    // listForExport = subjectsService.InitSubjectsListForExport(); - нужно реализовать
                    break;
                //и так далеее
                default:
                    MessageBox.Show("Извините, эта часть кода еще в разработке");
                    break;
            }
        }

        private void InitStudentsList()
        {
            try
            {
                // Загружаем студентов
                context.Students.Load();

                // Подсчитываем общее количество активных студентов
                var totalStudents = context.Students.Local.Count(s => s.IsActive);

                // Вычисляем общее количество страниц
                totalPages = (int)Math.Ceiling((double)totalStudents / PageSize);

                var studentsOnPage = GetPaginatedStudentsList(context);

                // Index - отображает порядковый номер строки, который продолжает нумерацию между страницами
                // Обновляем источник данных и колонки
                ConfigureStudentColumns(dGrid);
                dGrid.ItemsSource = studentsOnPage;

                // Обновляем отображение DataGrid
                dGrid.Items.Refresh();

                // Обновляем видимость и активное состояние кнопок пагинации
                UpdatePaginationButtons(totalPages);
                dGrid.Tag = "Students"; //тэг для использования при идентификации данных что сейчас в dGrid

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitCategoriesList()
        {
            try
            {
                context.Categories.Load();
                ConfigureCategoryColumns();
                dGrid.ItemsSource = context.Categories.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InitGenresList()
        {
            try
            {
                context.Genres.Load();
                ConfigureGenreColumns();
                dGrid.ItemsSource = context.Genres.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void InitSubjectsList()
        {
            try
            {
                // Загрузка предметов вместе с жанрами
                context.Subjects.Include(s => s.Genre).Load();

                // Конфигурация столбцов DataGrid
                ConfigureSubjectColumns();

                // Установка источника данных для DataGrid
                dGrid.ItemsSource = context.Subjects.Local;
                dGrid.Tag = "Subjects"; //тэг для использования при идентификации данных что сейчас в dDrid
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void InitBookPhotosListList()
        {
            try
            {
                context.BookPhotos.Load();
                ConfigureBookPhotosColumns();
                dGrid.ItemsSource = context.BookPhotos.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private void InitInventoryBooksList()
        {
            try
            {
                // Загрузка данных InventoryBooks
                context.InventoryBooks.Load();

                // Получаем все данные InventoryBooks
                var allInventoryBooks = context.InventoryBooks.Local.ToList();

                // Применение пагинации
                totalPages = (int)Math.Ceiling((double)allInventoryBooks.Count / PageSize);
                var paginatedBooks = allInventoryBooks
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize)
                    .Select((book, index) => new PaginatedInventoryBookModel
                    {
                        Index = (currentPage - 1) * PageSize + index + 1, // Вычисляем индекс с учетом текущей страницы
                        InventoryBookID = book.InventoryBookID,
                        Title = book.Title,
                        Author = book.Author,
                        Publisher = book.Publisher,
                        YearPublished = book.YearPublished,
                        ISBN = book.ISBN,
                        GenreName = book.Book.Genre.GenreName != null ? book.Book.Genre.GenreName : "Неизвестно",
                        SubjectName = book.Book.Subject.SubjectName != null ? book.Book.Subject.SubjectName : "Неизвестно"
                    })
                    .ToList();

                // Обновление источника данных и конфигурации колонок
                ConfigureInventoryBooksColumns2();
                dGrid.ItemsSource = paginatedBooks;

                // Обновление отображения DataGrid
                dGrid.Items.Refresh();

                // Обновление кнопок пагинации
                UpdatePaginationButtons(totalPages);
                dGrid.Tag = "InventoryBooks"; //тэг для использования при идентификации данных что сейчас в dDrid
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void InitBooksList()
        {
            try
            {
                // Загружаем книги и связанные данные
                context.Books
                    .Include(b => b.Genre)
                    .Include(b => b.Subject)
                    .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                    .Load();

                // Группируем книги по ISBN
                var groupedBooks = context.Books.Local
                    .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
                    .GroupBy(x => x.InventoryBook.ISBN)
                      .Select(g => new PaginatedBookInventoryModel
                      {
                          BookID = g.First().Book.BookID,
                          Title = g.First().InventoryBook.Title,
                          Author = g.First().InventoryBook.Author,
                          Publisher = g.First().InventoryBook.Publisher,
                          YearPublished = g.First().InventoryBook.YearPublished,
                          ISBN = g.Key,
                          Quantity = g.Count(),
                          QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                          GenreName = g.First().Book.Genre.GenreName != null ? g.First().Book.Genre.GenreName : "Неизвестно",
                          SubjectName = g.First().Book.Subject.SubjectName != null ? g.First().Book.Subject.SubjectName : "Неизвестно"
                      }).ToList();


                // Обновляем пагинацию
                totalPages = (int)Math.Ceiling((double)groupedBooks.Count / PageSize);
                var paginatedBooks = groupedBooks
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize)
                    .Select((book, index) => new PaginatedBookInventoryModel
                    {
                        Index = (currentPage - 1) * PageSize + index + 1, // Вычисляем индекс с учетом текущей страницы
                        BookID = book.BookID,
                        InventoryBookID = book.InventoryBookID, // Добавлено
                        Title = book.Title,
                        Author = book.Author,
                        Publisher = book.Publisher,
                        YearPublished = book.YearPublished,
                        ISBN = book.ISBN,
                        Quantity = book.Quantity,
                        QuantityLeft = book.QuantityLeft,
                        CategoryName = book.CategoryName
                    })
                    .ToList();

                // Обновляем источник данных и колонки
                ConfigureBooksColumns();
                dGrid.ItemsSource = paginatedBooks;

                // Обновляем отображение DataGrid
                dGrid.Items.Refresh();

                // Обновляем видимость кнопок
                UpdatePaginationButtons(totalPages);
                dGrid.Tag = "Books"; //тэг для использования при идентификации данных что сейчас в dDrid
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ConfigureBooksColumns()
        {
            dGrid.Columns.Clear();
            // Колонка для порядкового номера
            var indexColumn = new DataGridTextColumn { Header = "№", Binding = new Binding("Index"), Width = new DataGridLength(0.3, DataGridLengthUnitType.Star) };
            indexColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(indexColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) });//2 - ширина в 2 раза больше стандартной
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(0.9, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            var yearColumn = new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(0.4, DataGridLengthUnitType.Star) };// Ширина меньше, чем у остальных
            yearColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(yearColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(0.9, DataGridLengthUnitType.Star) });
            var quantityColumn = new DataGridTextColumn { Header = "Количество", Binding = new Binding("Quantity"), Width = new DataGridLength(0.6, DataGridLengthUnitType.Star) };// Ширина меньше, чем у остальных
            quantityColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(quantityColumn);
        }


        private void ConfigureGenreColumns()
        {
            dGrid.Columns.Clear();
            var idColumn = new DataGridTextColumn
            {
                Header = "ID",
                Binding = new Binding("GenreID"),
                Width = new DataGridLength(0.5, DataGridLengthUnitType.Star),
                CanUserSort = true // Включаем возможность сортировки
            };
            idColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(idColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название жанра", Binding = new Binding("GenreName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }

        private void ConfigureSubjectColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Clear();
            var indexColumn = new DataGridTextColumn { Header = "№", Binding = new Binding("SubjectID"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) };
            indexColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(indexColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название предмета", Binding = new Binding("SubjectName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Жанр", Binding = new Binding("Genre.GenreName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureCategoryColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("CategoryID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureBookPhotosColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("BookPhotoID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID книги", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата добавления", Binding = new Binding("DateAdded"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }


        private void ConfigureInventoryBooksColumns()
        {
            dGrid.Columns.Clear();
            // Колонка для порядкового номера
            var indexColumn = new DataGridTextColumn { Header = "№", Width = new DataGridLength(0.25, DataGridLengthUnitType.Star), Binding = new Binding("Index") };
            dGrid.Columns.Add(indexColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(0.8, DataGridLengthUnitType.Star) });
            var yearColumn = new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(0.4, DataGridLengthUnitType.Star) };
            yearColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(yearColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            var quantityColumn = new DataGridTextColumn { Header = "Количество", Binding = new Binding("Quantity"), Width = new DataGridLength(0.65, DataGridLengthUnitType.Star) };
            quantityColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(quantityColumn);
            var quantityLeftColumn = new DataGridTextColumn { Header = "Остаток", Binding = new Binding("QuantityLeft"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) };
            quantityLeftColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(quantityLeftColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Жанр", Binding = new Binding("GenreName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Предмет", Binding = new Binding("SubjectName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureInventoryBooksColumns2()
        {
            dGrid.Columns.Clear();

            var indexColumn = new DataGridTextColumn { Header = "№", Binding = new Binding("Index"), Width = new DataGridLength(0.3, DataGridLengthUnitType.Star) };
            indexColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(indexColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(0.8, DataGridLengthUnitType.Star) });
            var yearColumn = new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(0.4, DataGridLengthUnitType.Star) };
            yearColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(yearColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Жанр", Binding = new Binding("GenreName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Предмет", Binding = new Binding("SubjectName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }


        protected override void ConfigureLoansColumns() //Очищает все текущие колонки в DataGrid и добавляет новые колонки с соответствующими заголовками и привязками
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1.15, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя читателя", Binding = new Binding("FirstName"), Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия читателя", Binding = new Binding("LastName"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выдачи", Binding = new Binding("LoanDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            var dueDate = new DataGridTextColumn { Header = "Должны вернуть", Binding = new Binding("DueDate") { StringFormat = "dd/MM/yyyy" }, Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) };
            dueDate.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(dueDate);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            // Применяем стили
            //ApplyRowStyle();
        }



        private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            totalPages = 1;
            dGrid.ItemsSource = null;  // Очистка DataGrid перед загрузкой новых данных                                      
            currentListType = "SearchBooks";
            InitList(currentListType);
            RemoveRowStyle();
            CurrentTableName = "Поиск книги";
            updateListBooks();
            var searchDialog = new SearchBooksDialog(context);
            if (searchDialog.ShowDialog() == true)
            {
                var results = searchDialog.Tag as List<ViewModels.PaginatedBookInventoryModel>;
                if (results != null)
                {
                    dGrid.ItemsSource = results;
                    if (results.Any())
                    {
                        CurrentTableName = $"Найдено книг с заданными параметрами поиска: {results.First().ISBN}";
                    }
                }
            }
        }

        private void btnSearchBooksByBarcode_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            dGrid.Tag = "SearchBooksByBarcode"; // Устанавливаем тег для идентификации текущих данных
            dGrid.ContextMenu = null;
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Перечень книг";
            currentListType = "BooksByBarcode";
            updateListBooks();
            BarcodeTextBox.Visibility = Visibility.Visible;
            BarcodeTextBox.Focus();
            // Инициализация контекстного меню для книг
            InitializeContextMenuForBooks();
        }



        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            var addBookDialog = new SchoolLibrary.DialogWindows.AddBookDialog(context);
            if (addBookDialog.ShowDialog() == true)
            {
                var newBook = (Book)addBookDialog.DataContext;
                context.Books.Add(newBook);
                // Получаем информацию о названии книги из коллекции InventoryBooks
                var inventoryBookTitles = newBook.InventoryBooks.Select(ib => ib.Title).ToList();
                var titles = string.Join(", ", inventoryBookTitles);

                // Логирование с информацией о названии книги
                logger.Info($"Пользователь {UserSession.Username}, дата сохранения: {DateTime.Now}, добавил книгу с названиями: {titles}.");

                context.SaveChanges();

                // Обновляем книги и отображение
                updateListBooks();
            }
        }


        private void InitLoansList()
        {
            try
            {
                // Загружаем данные о заемах
                var loansOnPage = GetPaginatedLoansList(context);

                // Настраиваем колонки для DataGrid
                ConfigureLoanColumns(dGrid);
                dGrid.ItemsSource = loansOnPage;

                // Обновляем отображение DataGrid
                dGrid.Items.Refresh();
                dGrid.Tag = "Loans"; // Тэг для идентификации данных в DataGrid
                InitializeContextMenu();
                // Обновляем видимость и активное состояние кнопок пагинации
                totalPages = (int)Math.Ceiling((double)context.Loans.Local.Count() / PageSize);
                UpdatePaginationButtons(totalPages);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoanBookButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyRowStyle(dGrid);

            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Движение книг";
            dGrid.ItemsSource = null;  // Очистка DataGrid перед загрузкой новых данных
                                       // Инициализация списка заемов
            currentListType = "Loans";
            InitList(currentListType);
            LoanDialog dialog = new LoanDialog(context);

            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Новая запись в журнале оформлена");
                
                DateTime currentDateTime = DateTime.Now;
                logger.Info($"[{currentDateTime:yyyy-MM-dd HH:mm:ss}] Пользователь {UserSession.Username} оформил новый заем.");

                dGrid.ItemsSource = null;
                dGrid.UpdateLayout();
                InitList(currentListType);            // Загрузка и отображение обновленного списка заемов
                UpdatePaginationButtons(totalPages);  // Обновление кнопок пагинации в зависимости от количества страниц
            }
            else
            {
                MessageBox.Show("Отмена действия...");
               
                DateTime currentDateTime = DateTime.Now;
                logger.Info($"[{currentDateTime:yyyy-MM-dd HH:mm:ss}] Пользователь {UserSession.Username} отменил оформление займа.");

                // Перезагрузка данных и обновление отображения
                dGrid.ItemsSource = null;
                dGrid.UpdateLayout();
                InitList(currentListType);            // Загрузка и отображение обновленного списка заемов
                UpdatePaginationButtons(totalPages);
            }
        }



        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyRowStyle(dGrid);
            
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Движение книг";
            dGrid.ItemsSource = null;  // Очистка DataGrid перед загрузкой новых данных
            currentListType = "Loans";
            InitList(currentListType); // Инициализация списка заемов

            var dialog = new ReturnBookDialog(context);

            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Возврат в журнале оформлен");
                
                DateTime currentDateTime = DateTime.Now;
                logger.Info($"[{currentDateTime:yyyy-MM-dd HH:mm:ss}] Пользователь {UserSession.Username} оформил возврат книги.");

                dGrid.ItemsSource = null;   // Очистка DataGrid перед обновлением данных
                dGrid.UpdateLayout();
                InitList(currentListType);  // Загрузка и отображение обновленного списка заемов
                UpdatePaginationButtons(totalPages);  // Обновление кнопок пагинации в зависимости от количества страниц
            }
            else
            {
                MessageBox.Show("Отмена действия");
               
                DateTime currentDateTime = DateTime.Now;
                logger.Info($"[{currentDateTime:yyyy-MM-dd HH:mm:ss}] Пользователь {UserSession.Username} отменил возврат книги.");

                dGrid.ItemsSource = null;   // Очистка DataGrid перед обновлением данных
                dGrid.UpdateLayout();
                InitList(currentListType);  // Загрузка и отображение обновленного списка заемов
                UpdatePaginationButtons(totalPages);  // Обновление кнопок пагинации
            }
        }




        private void btnShowAvailableBooksButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            context.Students.Load();
            context.Categories.Load();
            context.Books.Load();
            context.Loans.Load();

            dGrid.ItemsSource = null;
            InitBooksListAvailableNow();
        }


        private void InitBooksListAvailableNow()
        {
            try
            {
                // Загрузка связанных сущностей
                context.Books
                    .Include(b => b.Genre)
                    .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                    .Load();

                var groupedBooks = context.Books.Local
                    .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
                    .GroupBy(x => x.InventoryBook.ISBN)
                    //.Select(g => new SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel
                    //.Select(g => new PaginatedBookInventoryModel
                    .Select((g, index) => new PaginatedBookInventoryModel
                    {
                        BookID = g.First().Book.BookID,
                        Title = g.First().InventoryBook.Title,
                        Author = g.First().InventoryBook.Author,
                        Publisher = g.First().InventoryBook.Publisher,
                        YearPublished = g.First().InventoryBook.YearPublished,
                        ISBN = g.Key,
                        Quantity = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                        QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                        CategoryName = g.First().Book.Genre.GenreName,
                        Index = index + 1 // Устанавливаем индекс
                    })
                    .Where(b => b.Quantity > 0) // Фильтрация книг с нулевым количеством
                    .ToList();

                // Обновляем пагинацию
                totalPages = (int)Math.Ceiling((double)groupedBooks.Count / PageSize);
                var paginatedBooks = groupedBooks
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize)
                     .Select((b, index) =>
                     {
                         b.Index = (currentPage - 1) * PageSize + index + 1; // Корректировка индекса
                         return b;
                     })
                    .ToList();

                ConfigureBooksColumns();
                dGrid.ItemsSource = paginatedBooks;
                dGrid.Tag = "InventoryBooks"; //тэг для использования при идентификации данных что сейчас в dDrid
                // Обновляем отображение DataGrid
                dGrid.Items.Refresh();

                // Обновляем видимость кнопок
                UpdatePaginationButtons(totalPages);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnEditBook_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedBook(); // Вызов нового метода для редактирования книги
        }

        private void btnDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенную книгу из DataGrid как PaginatedBookInventoryModel
            PaginatedInventoryBookModel selectedBook = dGrid.SelectedItem as PaginatedInventoryBookModel;

            if (selectedBook != null)
            {
                // Проверяем, есть ли активные займы
                var activeLoan = context.Loans
                    .FirstOrDefault(l => l.InventoryBookID == selectedBook.InventoryBookID && !l.Returned);

                if (activeLoan != null)
                {
                    MessageBox.Show("Книгу невозможно списать, так как она на руках.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить книгу '{selectedBook.Title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Открываем диалоговое окно для ввода деталей выбытия
                    DisposalDetailsDialog disposalDetailsDialog = new DisposalDetailsDialog();
                    if (disposalDetailsDialog.ShowDialog() == true)
                    {
                        try
                        {
                            // Получаем инвентарную книгу и займы, связанные с данной книгой
                            var inventoryBook = context.InventoryBooks
                                .Include(ib => ib.Loans)
                                .SingleOrDefault(ib => ib.InventoryBookID == selectedBook.InventoryBookID);

                            if (inventoryBook != null)
                            {
                                // Перемещаем данные в таблицы Storage
                                var inventoryBookStorage = new InventoryBookStorage
                                {
                                    InventoryNumber = inventoryBook.InventoryNumber,
                                    ISBN = inventoryBook.ISBN,
                                    Title = inventoryBook.Title,
                                    Author = inventoryBook.Author,
                                    Publisher = inventoryBook.Publisher,
                                    YearPublished = inventoryBook.YearPublished,
                                    Price = inventoryBook.Price,
                                    DateOfReceipt = inventoryBook.DateOfReceipt,
                                    IncomingInvoice = inventoryBook.IncomingInvoice,
                                    DateOfDisposal = disposalDetailsDialog.DateOfDisposal, // Из диалога
                                    OutgoingInvoice = disposalDetailsDialog.OutgoingInvoice, // Из диалога
                                    ReasonForDisposal = disposalDetailsDialog.ReasonForDisposal, // Из диалога
                                    BookID = inventoryBook.BookID
                                };

                                context.InventoryBooksStorage.Add(inventoryBookStorage);

                                // Перемещаем связанные займы в таблицу LoanStorage
                                foreach (var loan in inventoryBook.Loans)
                                {
                                    var loanStorage = new LoanStorage
                                    {
                                        InventoryBookStorageID = inventoryBookStorage.InventoryBookStorageID,
                                        StudentID = loan.StudentID,
                                        LoanDate = loan.LoanDate,
                                        DueDate = loan.DueDate,
                                        ReturnDate = loan.ReturnDate,
                                        Returned = loan.Returned
                                    };

                                    context.LoansStorage.Add(loanStorage);
                                }

                                // Удаляем связанные займы
                                context.Loans.RemoveRange(inventoryBook.Loans);

                                // Уменьшаем количество экземпляров книги в таблице Books
                                var book = context.Books.SingleOrDefault(b => b.BookID == inventoryBook.BookID);
                                if (book != null)
                                {
                                    book.Quantity--;
                                    if (book.QuantityLeft > 0) book.QuantityLeft--;
                                    else book.Quantity = 0;

                                    // Проверяем количество экземпляров книги
                                    if (book.Quantity == 0 && book.QuantityLeft == 0)
                                    {
                                        context.Books.Remove(book);
                                    }
                                }

                                // Удаляем инвентарную книгу
                                context.InventoryBooks.Remove(inventoryBook);

                                // Сохраняем изменения
                                context.SaveChanges();

                                MessageBox.Show("Книга успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                                // Обновляем источник данных DataGrid
                                updateListInventoryBook();
                            }
                            else
                            {
                                MessageBox.Show("Инвентарная книга для удаления не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при удалении книги: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateListInventoryBook();
            }
        }


        private int checkInt(String number)
        {
            int num1 = 0;
            bool success = int.TryParse(number.Trim(), out num1);
            if (success) return num1;
            else
            {
                Console.WriteLine("Не удалось преобразовать строку в число.");
                return 0;
            }
        }

        private void btnListStudents(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Читатели";
            updateListStudent();
        }

        private void btnListInventoryBook(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Инвентарный журнал книг";

            // Устанавливаем тег для инвентарных книг
            dGrid.Tag = "InventoryBooks";

            updateListInventoryBook();

            // Устанавливаем контекстное меню для инвентарных книг
            InitializeContextMenuForInventoryBooks();
        }

        private void btnSearchStudents_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            // Устанавливаем текущее имя таблицы и обновляем список студентов
            CurrentTableName = "Поиск студента";
            updateListStudent();

            // Создаем и отображаем диалог поиска студентов
            var searchDialog = new SearchStudentDialog(context);
            if (searchDialog.ShowDialog() == true)
            {
                var results = searchDialog.Tag as List<PaginatedStudentModel>;
                if (results != null)
                {
                    UpdateStudentDataGrid(results);
                    if (results.Any())
                    {
                        // Обновляем название текущей таблицы с количеством найденных студентов
                        CurrentTableName = $"Найдено студентов с заданными параметрами поиска: {results.Count}";
                    }
                }
            }
        }

        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            AddStudentDialog asd = new AddStudentDialog(context);
            var result = asd.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Новый студент был создан");
                updateListStudent();
            }
        }


        private void UpdateRowIndex(List<PaginatedStudentModel> students)
        {
            for (int i = 0; i < students.Count; i++)
            {
                students[i].Index = (currentPage - 1) * PageSize + i + 1; // Обновляем индекс с учетом текущей страницы
            }

            dGrid.ItemsSource = null;
            dGrid.ItemsSource = students; // Переназначаем источник данных для обновления
        }


        private void btnEditStudent_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            if (dGrid.SelectedItem is PaginatedStudentModel selectedStudent)
            {
                int actualStudentID = 0;

                // Получаем реальный StudentID выбранного читателя
                int indexOnPage = selectedStudent.Index;
                actualStudentID = indexOnPage;

                // var studentToEdit = context.Students.SingleOrDefault(s => s.StudentID == actualStudentID);
                var studentToEdit = context.Students
                            .SingleOrDefault(s => s.FirstName == selectedStudent.FirstName
                            && s.LastName == selectedStudent.LastName
                            && s.DateOfBirth == selectedStudent.DateOfBirth
                            && s.StudentClass == selectedStudent.StudentClass
                            && s.Prefix == selectedStudent.Prefix
                            && s.Address == selectedStudent.Address
                            && s.Phone == selectedStudent.Phone);
                if (studentToEdit != null)
                {
                    var studentCopy = new Student
                    {
                        StudentID = studentToEdit.StudentID,
                        FirstName = studentToEdit.FirstName,
                        LastName = studentToEdit.LastName,
                        DateOfBirth = studentToEdit.DateOfBirth,
                        StudentClass = studentToEdit.StudentClass,
                        Prefix = studentToEdit.Prefix,
                        Address = studentToEdit.Address,
                        Phone = studentToEdit.Phone,
                        IsActive = true
                    };
                    //передаем выделенного читателя в форму
                    EditStudentDialog dialog = new EditStudentDialog(context, studentCopy);
                    if (dialog.ShowDialog() == true)//если в результате вернем true, т.е. нажали ОК то сохраняем этого читателя
                    {
                        studentToEdit.FirstName = studentCopy.FirstName;
                        studentToEdit.LastName = studentCopy.LastName;
                        studentToEdit.DateOfBirth = studentCopy.DateOfBirth;
                        studentToEdit.StudentClass = studentCopy.StudentClass;
                        studentToEdit.Prefix = studentCopy.Prefix;
                        studentToEdit.Address = studentCopy.Address;
                        studentToEdit.Phone = studentCopy.Phone;
                        studentToEdit.IsActive = studentCopy.IsActive;

                        context.SaveChanges();

                        // Сохранить текущую страницу перед обновлением данных
                        int savedCurrentPage = currentPage;

                        // Перезагрузить читателей и обновить источник данных
                        var students = context.Students.ToList();
                        dGrid.ItemsSource = students.Select(PaginatedStudentModel.ConvertToPaginatedStudentModel).ToList();

                        // Пересчитать индексацию строк
                        UpdateRowIndex(dGrid.ItemsSource.Cast<PaginatedStudentModel>().ToList());

                        // Вызовить метод обновления списка читателей
                        updateListStudent();

                        // Восстановить сохраненную страницу
                        currentPage = savedCurrentPage;
                        // Инициализировать снова с учетом текущей страницы
                        InitList(currentListType);
                    }
                }
                else
                {
                    MessageBox.Show("Читатель не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите читателя для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListStudent();
            }
        }



        private void btnFilterStudentsWithoutLoans_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RemoveRowStyle();
                currentPage = 1;
                totalPages = 1;
                dGrid.ItemsSource = null;  // Очистка DataGrid перед загрузкой новых данных
                currentListType = "StudentsWithoutLoans"; // Текущий тип списка
                InitList(currentListType);

                // Получаем пагинированных студентов без займов из сервиса
                var paginatedStudentsWithoutLoans = studentService.GetPaginatedStudentsWithoutLoans(currentPage, PageSize);

                if (paginatedStudentsWithoutLoans != null && paginatedStudentsWithoutLoans.Any())
                {
                    ConfigureStudentColumns(dGrid);
                    dGrid.ItemsSource = paginatedStudentsWithoutLoans;
                    dGrid.UpdateLayout();
                }
                else
                {
                    MessageBox.Show("Нет студентов без займов.");
                }

                UpdatePaginationButtons(totalPages);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dGrid.ItemsSource = null;
                dGrid.UpdateLayout();
                InitList(currentListType);
                UpdatePaginationButtons(totalPages);
            }
        }


        private void InitStudentsWithoutLoansList()
        {
            try
            {
                // Загружаем данные о студентах, которые не брали книги
                var studentsWithoutLoansOnPage = GetPaginatedStudentsWithoutLoansList(context);

                // Настраиваем колонки для DataGrid
                ConfigureStudentColumns(dGrid);
                dGrid.ItemsSource = studentsWithoutLoansOnPage;

                // Обновляем отображение DataGrid
                dGrid.Items.Refresh();

                // Обновляем видимость и активное состояние кнопок пагинации
                totalPages = (int)Math.Ceiling((double)context.Students
                    .Count(s => !context.Loans.Any(l => l.StudentID == s.StudentID)) / PageSize);
                UpdatePaginationButtons(totalPages);

                dGrid.Tag = "StudentsWithoutLoans"; // Тэг для идентификации данных в DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #region
        //Statistics      
        private void StatisticsStudentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var statisticsPage = new StatisticsPageStudent(context);
            statisticsPage.Show();
        }

        private void StatisticsBooksMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var statisticsPage = new StatisticsPageInventoryBook(context);
            statisticsPage.Show();
        }

        private void StatisticsBooksByPeriodMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var statisticsPage = new StatisticsByPeriodWindow(context);
            statisticsPage.Show();
        }

        private void LoanedAndNotReturnedBooksButton_Click(object sender, RoutedEventArgs e)
        {
            var issuedBooksListPage = new IssuedBooksListPage(context);
            issuedBooksListPage.Show();
        }

        private void btnBookReport_Click(object sender, RoutedEventArgs e) //метод сбрасывает ItemsSource для DataGrid и вызывает метод InitLoansList для загрузки данных
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Отчет по книгам";
            try
            {
                dGrid.ItemsSource = null;
                InitLoansList(); // Метод для инициализации данных о книгах
                // Применяем стиль после обновления источника данных
                ApplyRowStyle(dGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void RemoveRowStyle()// Убирает стиль//Скидываем тег
        {
            dGrid.RowStyle = null;
            //  dGrid.Tag = null;  
            dGrid.ContextMenu = null; // Удаляем контекстное меню
        }


        private void btnAddGenre_Click(object sender, RoutedEventArgs e)
        {
            AddGenreDialog acd = new AddGenreDialog(context);
            var result = acd.ShowDialog();

            // Проверка на null и значение DialogResult
            if (result == true)
            {
                MessageBox.Show("Новый жанр книг был создан");
                updateListGenres();
            }
            else if (result == false)
            {
                MessageBox.Show("Добавление жанра было отменено.");
            }
        }

        private void btnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {

            if (dGrid.SelectedItem is PaginatedStudentModel selectedStudent)
            {
                DeleteStudentDialog dialog = new DeleteStudentDialog(context, selectedStudent);
                if (dialog.ShowDialog() == true)
                {
                    // Обновляем DataGrid после удаления студента                    
                    updateListStudent();
                }
            }
            else
            {
                MessageBox.Show("Выберите читателя для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListStudent();
            }
        }

        private void btnDeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is Genre selectedGenre)
            {
                DeleteGenreDialog dialog = new DeleteGenreDialog(context, selectedGenre);
                if (dialog.ShowDialog() == true)
                {
                    // Обновляем DataGrid после удаления жанра, если удаление прошло успешно
                    try
                    {
                        dGrid.ItemsSource = context.Genres.ToList();
                        MessageBox.Show("Жанр был успешно удален");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки жанров после удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListGenres();
            }
        }

        private void btnEditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is Genre selectedGenre)
            {
                var genreCopy = new Genre
                {
                    GenreID = selectedGenre.GenreID,
                    GenreName = selectedGenre.GenreName
                };

                EditGenreDialog dialog = new EditGenreDialog(context, genreCopy);
                if (dialog.ShowDialog() == true)
                {
                    selectedGenre.GenreName = genreCopy.GenreName;
                    context.SaveChanges();

                    dGrid.ItemsSource = context.Genres.ToList();
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListGenres();
            }
        }

        private void btnAllGenresShow_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            updateListGenres();
        }

        private void btnAllSubjectsShow_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            updateListSubjects();
        }

        private void btnAddSubject_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            // Создание экземпляра окна для добавления предмета
            AddSubjectDialog addSubjectDialog = new AddSubjectDialog(context);

            // Отображение диалогового окна
            var result = addSubjectDialog.ShowDialog();

            // Проверка на результат диалога
            if (result == true)
            {
                // Сообщение о успешном добавлении нового предмета
                MessageBox.Show("Новый предмет был создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновление списка предметов
                updateListSubjects();
            }
            else if (result == false)
            {
                // Сообщение о том, что добавление предмета было отменено или произошла ошибка
                MessageBox.Show("Добавление предмета было отменено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEditSubject_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            if (dGrid.SelectedItem is Subject selectedSubject)
            {
                // Создаем копию выбранного предмета
                var subjectCopy = new Subject
                {
                    SubjectID = selectedSubject.SubjectID,
                    SubjectName = selectedSubject.SubjectName,
                    GenreID = selectedSubject.GenreID,
                    Genre = selectedSubject.Genre
                };

                // Открываем окно редактирования
                EditSubjectDialog dialog = new EditSubjectDialog(context, subjectCopy);
                if (dialog.ShowDialog() == true)
                {
                    // Если изменения сохранены, обновляем объект в базе данных
                    selectedSubject.SubjectName = subjectCopy.SubjectName;
                    selectedSubject.GenreID = subjectCopy.GenreID;
                    selectedSubject.Genre = subjectCopy.Genre;

                    context.SaveChanges();

                    // Обновляем источник данных для DataGrid
                    dGrid.ItemsSource = context.Subjects.ToList();
                }
            }
            else
            {
                // Если предмет не выбран, показываем сообщение
                MessageBox.Show("Выберите предмет для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListSubjects(); // Обновляем список предметов
            }
        }

        private string GetDataGridDataType()
        {
            if (dGrid.ItemsSource != null)
            {
                // Получаем первый элемент из источника данных
                var firstItem = dGrid.ItemsSource.Cast<object>().FirstOrDefault();
                if (firstItem != null)
                {
                    // Определяем тип первого элемента
                    return firstItem.GetType().Name;
                }
            }

            // Возвращаем значение по умолчанию, если не удалось определить тип
            return "Unknown";
        }

        private void btnExportToFile_Click(object sender, RoutedEventArgs e)
        {
            string listType = dGrid.Tag as string;
            // Инициализируем список для экспорта в зависимости от типа данных
            initListForExport(listType);

            // Проверяем, что DataGrid и список данных существуют
            if (dGrid.ItemsSource == null || !dGrid.ItemsSource.OfType<object>().Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создаем диалоговое окно для сохранения файла
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx|PDF Files (*.pdf)|*.pdf",
                DefaultExt = "xlsx",
                AddExtension = true
            };

            // Открываем диалоговое окно и проверяем, был ли выбран файл
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                // Получаем путь к файлу и расширение
                string filePath = saveFileDialog.FileName;
                string extension = System.IO.Path.GetExtension(filePath).ToLower();

                // Создаем экземпляр класса ExportToFile
                var exporter = new ExportToFile();

                try
                {
                    if (extension == ".xlsx")
                    {
                        // Экспортируем данные в Excel
                        exporter.ExportDataGridToExcel(listForExport, dGrid, filePath);
                    }
                    else if (extension == ".pdf")
                    {
                        // Экспортируем данные в PDF
                        //exporter.ExportDataGridToPDF(dGrid.ItemsSource.OfType<object>().ToList(), dGrid, filePath);
                        exporter.ExportDataGridToPDF(listForExport, dGrid, filePath);
                    }
                    else
                    {
                        MessageBox.Show("Выбранный формат файла не поддерживается.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    // Информируем пользователя об успешном экспорте
                    MessageBox.Show($"Данные успешно экспортированы в файл: {filePath}", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnFilterNotReturnedBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var notReturnedBooksList = GetNotReturnedBooks();
                dGrid.ItemsSource = notReturnedBooksList;

                ConfigureLoansColumns(dGrid); // Настройка колонок
                ApplyRowStyle(dGrid);         // Применение стилей строк
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFilterReturnedBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var returnedBooks = GetReturnedBooks();
                dGrid.ItemsSource = returnedBooks;

                ConfigureLoansColumns(dGrid); // Настройка колонок
                ApplyRowStyle(dGrid);         // Применение стилей строк
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        // Обработчик для показа книг, которые не были взяты
        private void ShowBooksNotLoanedButton_Click(object sender, RoutedEventArgs e)
        {
            var booksNotLoaned = context.Books
                .Include(b => b.InventoryBooks)
                .Where(b => !b.InventoryBooks.Any(ib => ib.Loans.Any()))
                .ToList();

            if (booksNotLoaned.Any())
            {
                // Вывод книг, которые еще не были взяты
                MessageBox.Show($"Найдено книг, которые не брали читатели: {booksNotLoaned.Count}");
                // Отображение книг в DataGrid или другом элементе управления
                dGrid.ItemsSource = booksNotLoaned;
            }
            else
            {
                MessageBox.Show("Все книги были взяты хотя бы один раз.");
            }
        }


        private void btnFilterBooksNotLoaned_Click(object sender, RoutedEventArgs e)
        {
            RemoveRowStyle();
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Книги которые ни разу не взяли";
            updateListBooksNotLoaned();
        }

        private void updateListBooksNotLoaned()
        {
            try
            {
                dGrid.ItemsSource = null;
                currentListType = "BooksNotLoaned";
                InitList(currentListType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





        private void InitBooksNotLoanedList()
        {
            try
            {
                // Загрузка данных InventoryBooks
                context.InventoryBooks.Load();

                // Запрос для получения всех книг, которые не были взяты читателями
                var booksNotLoaned = context.InventoryBooks
                    .Where(ib => !ib.Loans.Any())
                    .ToList();

                // Применение пагинации
                totalPages = (int)Math.Ceiling((double)booksNotLoaned.Count / PageSize);
                var paginatedBooksNotLoaned = booksNotLoaned
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize)
                    .Select((book, index) => new PaginatedInventoryBookModel
                    {
                        Index = (currentPage - 1) * PageSize + index + 1, // Вычисляем индекс с учетом текущей страницы
                        InventoryBookID = book.InventoryBookID,
                        Title = book.Title,
                        Author = book.Author,
                        Publisher = book.Publisher,
                        YearPublished = book.YearPublished,
                        ISBN = book.ISBN,
                        GenreName = book.Book?.Genre?.GenreName ?? "Неизвестно",
                        SubjectName = book.Book?.Subject?.SubjectName ?? "Неизвестно"
                    })
                    .ToList();

                // Обновление источника данных и конфигурации колонок
                ConfigureInventoryBooksColumns2(); // Используем существующий метод для настройки колонок
                dGrid.ItemsSource = paginatedBooksNotLoaned;

                // Обновление отображения DataGrid
                dGrid.Items.Refresh();

                // Обновление кнопок пагинации
                UpdatePaginationButtons(totalPages);
                dGrid.Tag = "BooksNotLoaned"; // Используем тег для идентификации текущего отображения
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeContextMenu()
        {
            // Создаем контекстное меню
            ContextMenu contextMenu = new ContextMenu();

            // Создаем элементы контекстного меню
            MenuItem returnYesMenuItem = new MenuItem { Header = "Оформить возврат книги?", FontSize = 16 };
            returnYesMenuItem.Click += ReturnBookYes_Click; // Подписываем обработчик события

            // Добавляем элементы в контекстное меню
            contextMenu.Items.Add(returnYesMenuItem);

            // Устанавливаем контекстное меню в зависимости от значения Tag
            if (dGrid.Tag != null && dGrid.Tag.ToString() == "Loans")
            {
                dGrid.ContextMenu = contextMenu;
            }
            else
            {
                dGrid.ContextMenu = null;
            }

            // Подписываемся на событие закрытия контекстного меню
            contextMenu.Closed += (s, args) =>
            {
                // Пересоздаем контекстное меню после его закрытия
                InitializeContextMenu();
            };
        }

        private void ReturnBookYes_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный элемент из DataGrid
            if (dGrid.SelectedItem is LoanViewModel selectedLoanViewModel)
            {
                // Проверяем, была ли книга уже возвращена
                if (selectedLoanViewModel.Returned)
                {
                    MessageBox.Show("Эта книга уже была возвращена ранее.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    return; // Выходим из метода, чтобы предотвратить повторный возврат
                }

                // возврат книги
                try
                {
                    selectedLoanViewModel.Returned = true;
                    selectedLoanViewModel.ReturnDate = DateTime.Now;

                    // Обновляем данные в базе данных или выполняем другие действия
                    UpdateLoanInDatabase(selectedLoanViewModel);

                    // Обновляем отображение
                    dGrid.Items.Refresh();

                    MessageBox.Show("Книга успешно возвращена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при возврате книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите книгу для возврата.", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ReturnBookNo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Возврат книги отменен.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void UpdateLoanInDatabase(LoanViewModel loanViewModel)
        {

            // Находим запись о займе по идентификатору
            var loan = context.Loans.FirstOrDefault(l => l.LoanID == loanViewModel.LoanID);
            if (loan != null)
            {
                // Обновляем свойства записи
                loan.Returned = loanViewModel.Returned;
                loan.ReturnDate = loanViewModel.ReturnDate;

                // Сохраняем изменения в базе данных
                context.SaveChanges();

            }
        }


        private void EditBookYes_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedBook(); // Вызов метода для редактирования книги
        }

        private void EditBookNo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Действие отменено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            btnEditBook_Click(sender, e); // Вызываем существующий метод редактирования книги
        }
        private void EditSelectedBook()
        {
            RemoveRowStyle();
            dGrid.Tag = "Books"; // Устанавливаем тег для идентификации текущих данных
            dGrid.ContextMenu = null;

            if (dGrid.SelectedItem is PaginatedBookInventoryModel selectedBookViewModel)
            {
                // Найти исходную сущность Book по BookID
                var selectedBook = context.Books.Include(b => b.InventoryBooks).FirstOrDefault(b => b.BookID == selectedBookViewModel.BookID);

                if (selectedBook != null)
                {
                    var editBookDialog = new EditBookDialog(context, selectedBook);
                    if (editBookDialog.ShowDialog() == true)
                    {
                        context.SaveChanges();
                        dGrid.Items.Refresh();
                    }
                }
                else
                {
                    MessageBox.Show("Книга не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            InitBooksList();
        }


        private void InitializeContextMenuForBooks()
        {
            // Создаем контекстное меню
            ContextMenu contextMenu = new ContextMenu();

            // Создаем элемент для редактирования книги
            MenuItem editBookMenuItem = new MenuItem { Header = "Редактировать книгу", FontSize = 16 };
            editBookMenuItem.Click += EditBook_Click; // Подписываем обработчик события

            // Создаем элемент для списания книги
            MenuItem deleteBookMenuItem = new MenuItem { Header = "Списать книгу", FontSize = 16 };
            deleteBookMenuItem.Click += btnDeleteBook_Click; // Подписываем обработчик события

            // Добавляем элементы в контекстное меню
            contextMenu.Items.Add(editBookMenuItem);
            contextMenu.Items.Add(deleteBookMenuItem);

            // Устанавливаем контекстное меню
            dGrid.ContextMenu = contextMenu;
        }


        private void dGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Проверяем значение тега DataGrid
            if (dGrid.Tag != null)
            {
                if (dGrid.Tag.ToString() == "Books" && dGrid.SelectedItem is PaginatedBookInventoryModel)
                {
                    InitializeContextMenuForBooks(); // Инициализируем контекстное меню для книг
                }
                else if (dGrid.Tag.ToString() == "InventoryBooks" && dGrid.SelectedItem is PaginatedInventoryBookModel)
                {
                    InitializeContextMenuForInventoryBooks(); // Инициализируем контекстное меню для инвентарных книг
                }
                else if (dGrid.Tag.ToString() == "Loans" && dGrid.SelectedItem is LoanViewModel)
                {
                    InitializeContextMenuForLoans(); // Инициализируем контекстное меню для возврата книг
                }
                else if (dGrid.Tag.ToString() == "Students" && dGrid.SelectedItem is PaginatedStudentModel)
                {
                    InitializeContextMenuForStudents(); // Инициализируем контекстное меню для читателей
                }
                else if (dGrid.Tag.ToString() == "BookMovements" && dGrid.SelectedItem is LoanViewModel)
                {
                    InitializeContextMenuForBookMovements(); // Инициализируем контекстное меню для движения книг
                }
                else
                {
                    dGrid.ContextMenu = null; // Удаляем контекстное меню, если данные не соответствуют
                }
            }
            else
            {
                dGrid.ContextMenu = null; // Удаляем контекстное меню, если тег отсутствует
            }
        }

        //private void dGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    // Проверяем значение тега DataGrid
        //    if (dGrid.Tag != null)
        //    {
        //        switch (dGrid.Tag.ToString())
        //        {
        //            case "Books" when dGrid.SelectedItem is PaginatedBookInventoryModel:
        //                InitializeContextMenuForBooks(); // Инициализируем контекстное меню для книг
        //                break;

        //            case "InventoryBooks" when dGrid.SelectedItem is PaginatedInventoryBookModel:
        //                InitializeContextMenuForInventoryBooks(); // Инициализируем контекстное меню для инвентарных книг
        //                break;

        //            case "Loans" when dGrid.SelectedItem is LoanViewModel:
        //                InitializeContextMenuForLoans(); // Инициализируем контекстное меню для возврата книг
        //                break;

        //            case "Students" when dGrid.SelectedItem is PaginatedStudentModel:
        //                InitializeContextMenuForStudents(); // Инициализируем контекстное меню для читателей
        //                break;

        //            case "BookMovements" when dGrid.SelectedItem is LoanViewModel:
        //                InitializeContextMenuForBookMovements(); // Инициализируем контекстное меню для движения книг
        //                break;   
        //            //case "SearchBooksByBarcode" when dGrid.SelectedItem is PaginatedBookInventoryModel:
        //            case "SearchBooksByBarcode" when dGrid.SelectedItem is PaginatedInventoryBookModel:
        //                InitializeContextMenuForSearchBooksByBarcode(); // Инициализируем контекстное меню для движения книг
        //                break;

        //            default:
        //                dGrid.ContextMenu = null; // Удаляем контекстное меню, если данные не соответствуют
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        dGrid.ContextMenu = null; // Удаляем контекстное меню, если тег отсутствует
        //    }
        //}



        //private void InitializeContextMenuForSearchBooksByBarcode() {
        //    // Инициализация контекстного меню для поиска книг по штрих-коду
        //    // Создаем контекстное меню
        //    ContextMenu contextMenu = new ContextMenu();


        //  //  contextMenu.Items.Add(new MenuItem { Header = "Выдать книгу", Command = new RelayCommand(IssueBook_Click) });
        // //   contextMenu.Items.Add(new MenuItem { Header = "Вернуть книгу", Command = new RelayCommand(ReturnBook_Click) });

        //    MenuItem issueBookMenuItem = new MenuItem { Header = "Выдать книгу", FontSize = 16 };
        //    issueBookMenuItem.Click += IssueBookButton_Click; // Подписываем обработчик события

        //    MenuItem returnBookMenuItem = new MenuItem { Header = "Вернуть книгу", FontSize = 16 };
        //    returnBookMenuItem.Click += ReturnBookButton_Click; // Подписываем обработчик события


        //    contextMenu.Items.Add(issueBookMenuItem);
        //    contextMenu.Items.Add(returnBookMenuItem);

        //    if (dGrid.SelectedItem is PaginatedInventoryBookModel selectedBook)
        //    {
        //        if (selectedBook.QuantityLeft > 0)
        //        {
        //            issueBookMenuItem.Visibility = Visibility.Visible;      // Выдать книгу
        //            returnBookMenuItem.Visibility = Visibility.Collapsed; // Вернуть книгу
        //        }
        //        else
        //        {
        //            issueBookMenuItem.Visibility = Visibility.Collapsed;      // Выдать книгу
        //            returnBookMenuItem.Visibility = Visibility.Visible; // Вернуть книгу
        //        }

        //    }

        //}

        private void IssueBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = dGrid.SelectedItem as BookInventoryViewModel;
            if (selectedBook != null)
            {
                // Проверим, если книга на руках
                var loan = context.Loans.FirstOrDefault(l => l.InventoryBookID == selectedBook.InventoryBookID && l.Returned);
                if (loan == null)
                {
                    // Откроем диалог для возврата книги
                    var returnDialog = new ReturnBookDialogSelectDate(context, loan);
                    returnDialog.Owner = this; // Установить владельца окна
                    if (returnDialog.ShowDialog() == true)
                    {
                        // Логика для возврата книги
                        RefreshBooksData();
                    }
                }
                else
                {
                    MessageBox.Show("Эта книга не на руках.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void InitializeContextMenuForBookMovements()
        {
            // Создаем контекстное меню
            ContextMenu contextMenu = new ContextMenu();

            // Создаем элементы контекстного меню
            MenuItem returnBookMenuItem = new MenuItem { Header = "Оформить возврат книги", FontSize = 16 };
            returnBookMenuItem.Click += ReturnBookButton_Click; // Подписываем обработчик события

            MenuItem infoStudentMenuItem = new MenuItem { Header = "Информация о читателе", FontSize = 16 };
            infoStudentMenuItem.Click += btnInfoStudent_Click; // Подписываем обработчик события

            // Добавляем элементы в контекстное меню
            contextMenu.Items.Add(returnBookMenuItem);
            contextMenu.Items.Add(infoStudentMenuItem);

            // Устанавливаем контекстное меню
            dGrid.ContextMenu = contextMenu;
        }

        private void btnInfoStudent_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is LoanViewModel selectedLoan)
            {
                var student = context.Students.SingleOrDefault(s => s.StudentID == selectedLoan.StudentID);

                if (student != null)
                {
                    // Создаем диалоговое окно для отображения информации о читателе
                    StudentInfoDialog studentInfoDialog = new StudentInfoDialog(student);
                    studentInfoDialog.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Читатель не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу, чтобы просмотреть информацию о читателе.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void InitializeContextMenuForStudents()
        {
            // Создаем контекстное меню
            ContextMenu contextMenu = new ContextMenu();

            // Создаем элементы контекстного меню
            MenuItem editMenuItem = new MenuItem { Header = "Редактировать читателя", FontSize = 16 };
            editMenuItem.Click += btnEditStudent_Click; // Подписываем обработчик события

            MenuItem deleteMenuItem = new MenuItem { Header = "Удалить читателя", FontSize = 16 };
            deleteMenuItem.Click += btnDeleteStudent_Click; // Подписываем обработчик события

            // Добавляем элементы в контекстное меню
            contextMenu.Items.Add(editMenuItem);
            contextMenu.Items.Add(deleteMenuItem);

            // Устанавливаем контекстное меню
            dGrid.ContextMenu = contextMenu;
        }


        private void InitializeContextMenuForLoans()
        {
            // Создаем контекстное меню
            ContextMenu contextMenu = new ContextMenu();

            // Создаем элементы контекстного меню
            MenuItem returnBookMenuItem = new MenuItem { Header = "Оформить возврат книги", FontSize = 16 };
            returnBookMenuItem.Click += ReturnBookButton_Click; // Подписываем обработчик события

            MenuItem infoStudentMenuItem = new MenuItem { Header = "Информация о читателе", FontSize = 16 };
            infoStudentMenuItem.Click += btnInfoStudent_Click; // Подписываем обработчик события

            // Добавляем элементы в контекстное меню
            contextMenu.Items.Add(returnBookMenuItem);
            contextMenu.Items.Add(infoStudentMenuItem);

            // Устанавливаем контекстное меню
            dGrid.ContextMenu = contextMenu;
        }


        private void InitializeContextMenuForInventoryBooks()
        {
            // Создаем контекстное меню
            ContextMenu contextMenu = new ContextMenu();

            // Создаем элемент для списания книги
            MenuItem deleteBookMenuItem = new MenuItem { Header = "Списать книгу", FontSize = 16 };
            deleteBookMenuItem.Click += btnDeleteBook_Click; // Подписываем обработчик события для удаления книги

            // Добавляем элементы в контекстное меню          
            contextMenu.Items.Add(deleteBookMenuItem);

            // Устанавливаем контекстное меню для DataGrid
            dGrid.ContextMenu = contextMenu;
        }

        //private void BarcodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(BarcodeTextBox.Text))
        //    {
        //        var isbn = BarcodeTextBox.Text.Trim();
        //        SearchInventoryBooksByISBN(isbn);
        //        BarcodeTextBox.Clear();
        //    }
        //}

        //private void BarcodeTextBox_KeyDown(object sender, KeyEventArgs e)      
        //{
        //    // Проверяем, что нажат Enter (или другой символ окончания сканирования)
        //    if (e.Key == Key.Enter)
        //    {
        //        string scannedBarcode = BarcodeTextBox.Text.Trim();
        //       // MessageBox.Show(scannedBarcode);
        //        if (!string.IsNullOrEmpty(scannedBarcode))
        //        {
        //            SearchInventoryBooksByISBN(scannedBarcode);
        //        }

        //        // Очищаем TextBox для следующего ввода
        //        BarcodeTextBox.Clear();
        //    }
        //}


        //private void SearchInventoryBooksByISBN(string inputISBN)
        //{
        //    // Очищаем ввод от всех символов, кроме цифр и удаляем пробелы и переносы строк
        //    string cleanedInputISBN = CleanISBN(inputISBN.Trim());
        //   // MessageBox.Show("+"+cleanedInputISBN+"+");
        //   // Загружаем книги в память и выполняем фильтрацию на стороне клиента
        //   var book = context.InventoryBooks
        //        .ToList() // Загружаем все книги в память
        //        .FirstOrDefault(b => CleanISBN(b.ISBN.Trim()) == cleanedInputISBN);

        //    if (book != null)
        //    {
        //        MessageBox.Show($"Книга найдена: {book.Title}, автор: {book.Author}");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Книга с таким ISBN не найдена.");
        //    }

        //    // Сбрасываем фокус на BarcodeTextBox после поиска
        //    BarcodeTextBox.Focus();
        //}

        // Метод для очистки ISBN от всех символов, кроме цифр
        private string CleanISBN(string isbn)
        {
            return new string(isbn.Where(char.IsDigit).ToArray());
        }


        private void BarcodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            dGrid.Tag = "SearchBooksByBarcode";
            // Проверяем, что нажат Enter (или другой символ окончания сканирования)
            if (e.Key == Key.Enter)
            {
                string scannedBarcode = BarcodeTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(scannedBarcode))
                {
                    // Ищем книги по ISBN
                    SearchInventoryBooksByISBN(scannedBarcode);
                }

                // Очищаем TextBox для следующего ввода
                BarcodeTextBox.Clear();
            }
        }

        private void SearchInventoryBooksByISBN(string inputISBN)
        {
            // Очищаем ввод от всех символов, кроме цифр
            string cleanedInputISBN = CleanISBN(inputISBN.Trim());

            // Загружаем книги из базы данных и выполняем фильтрацию по ISBN
            var allBooks = context.InventoryBooks
     .ToList(); // This will execute the query and bring the data to memory

            // Then, apply the custom filtering logic in memory
            var matchedBooks = allBooks
                .Where(b => CleanISBN(b.ISBN.Trim()) == cleanedInputISBN)
                .ToList();

            // Проверяем, найдены ли книги
            if (matchedBooks.Any())
            {
                // Выводим книги в DataGrid
                dGrid.ItemsSource = matchedBooks
                    .Select((book, index) => new PaginatedInventoryBookModel
                    {
                        Index = index + 1, // Индексация книг
                        InventoryBookID = book.InventoryBookID,
                        Title = book.Title,
                        Author = book.Author,
                        Publisher = book.Publisher,
                        YearPublished = book.YearPublished,
                        ISBN = book.ISBN,
                        GenreName = book.Book.Genre.GenreName ?? "Неизвестно",
                        SubjectName = book.Book.Subject.SubjectName ?? "Неизвестно"
                    })
                    .ToList();

                // Обновляем DataGrid и заголовок
                ConfigureInventoryBooksColumns2();
                dGrid.Items.Refresh();
                CurrentTableName = $"Найдено книг с ISBN: {cleanedInputISBN}";
            }
            else
            {
                MessageBox.Show("Книга с таким ISBN не найдена.");
            }

            // Сбрасываем фокус на BarcodeTextBox после поиска
            BarcodeTextBox.Focus();
        }



        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = dGrid.SelectedItem as BookInventoryViewModel;
            if (selectedBook != null)
            {
                // Проверим, если книга на руках
                var loan = context.Loans.FirstOrDefault(l => l.InventoryBookID == selectedBook.InventoryBookID && !l.Returned);
                if (loan != null)
                {
                    // Откроем диалог для возврата книги
                    var returnDialog = new ReturnBookDialogSelectDate(context, loan);
                    returnDialog.Owner = this; // Установить владельца окна
                    if (returnDialog.ShowDialog() == true)
                    {
                        // Логика для возврата книги
                        RefreshBooksData();
                    }
                }
                else
                {
                    MessageBox.Show("Эта книга не на руках.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void RefreshBooksData()
        {
            try
            {
                // Получение актуального списка инвентарных книг из базы данных
                var inventoryBooks = context.InventoryBooks
                    .Include(ib => ib.Book)
                    .Include(ib => ib.Book.Genre)
                    .Include(ib => ib.Book.Subject)
                    .ToList();

                // Создание списка модели PaginatedBookInventoryModel на основе списка инвентарных книг
                var paginatedBooks = inventoryBooks
                    .Select(ib => new PaginatedBookInventoryModel
                    {
                        InventoryBookID = ib.InventoryBookID,
                        BookID = ib.BookID ?? 0,
                        Title = ib.Title,
                        Author = ib.Author,
                        Publisher = ib.Publisher,
                        YearPublished = ib.YearPublished,
                        ISBN = ib.ISBN,
                        Quantity = ib.Book.Quantity,
                        QuantityLeft = ib.Book.QuantityLeft,
                        //CategoryName = ib.Book.Genre?.GenreName, // Предполагаем, что Genre имеет свойство Name
                        GenreName = ib.Book.Genre?.GenreName,
                        SubjectName = ib.Book.Subject?.SubjectName
                    })
                    .ToList();

                // Обновление DataGrid с новыми данными
                dGrid.ItemsSource = paginatedBooks;

                // Очистка выделения
                dGrid.SelectedItem = null;
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка при обновлении данных книг: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void InitializeContextMenuForSearchBooksByBarcode()
        {
            // Создаем контекстное меню
            ContextMenu contextMenu = new ContextMenu();

            // Создаем пункты меню
            MenuItem issueBookMenuItem = new MenuItem { Header = "Выдать книгу", FontSize = 16 };
            issueBookMenuItem.Click += IssueBookButton_Click; // Подписываем обработчик события

            MenuItem returnBookMenuItem = new MenuItem { Header = "Вернуть книгу", FontSize = 16 };
            returnBookMenuItem.Click += ReturnBookButton_Click; // Подписываем обработчик события

            // Добавляем пункты меню в контекстное меню
            contextMenu.Items.Add(issueBookMenuItem);
            contextMenu.Items.Add(returnBookMenuItem);

            if (dGrid.SelectedItem is PaginatedInventoryBookModel selectedBook)
            {
                bool isBookIssued = IsBookIssued(selectedBook.InventoryBookID);

                if (isBookIssued)
                {
                    // Если книга выдана, показываем опцию для возврата
                    issueBookMenuItem.Visibility = Visibility.Collapsed;
                    returnBookMenuItem.Visibility = Visibility.Visible;
                }
                else
                {
                    // Если книга не выдана, показываем опцию для выдачи
                    issueBookMenuItem.Visibility = Visibility.Visible;
                    returnBookMenuItem.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                // Если элемент не выбран или не является книгой, скрываем меню
                contextMenu = null;
            }

            // Присваиваем контекстное меню DataGrid
            dGrid.ContextMenu = contextMenu;
        }

        private bool IsBookIssued(int inventoryBookID)
        {
            // Здесь нужно реализовать логику проверки, есть ли активные выдачи для данной книги
            // Пример использования Entity Framework для проверки в базе данных


            return context.Loans.Any(loan => loan.InventoryBookID == inventoryBookID && !loan.Returned);

        }

        #endregion
    }
}
