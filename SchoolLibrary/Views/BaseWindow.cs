using SchoolLibrary.AuthWindows;
using SchoolLibrary.DialogWindows.Operations;
using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SchoolLibrary.ViewModels;
//using static SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel;
using SchoolLibrary.DialogWindows;
using SchoolLibrary.DialogWindows.CategoryWindows;
using System.IO;        // Для FileStream
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using SchoolLibrary.Service;
using SchoolLibrary.Converters;
using System.Windows.Media;  // Для SaveFileDialog


namespace SchoolLibrary.Views
{  
    public abstract class BaseWindow : BaseSettings, INotifyPropertyChanged
    {
        private readonly List<Student> studentsWithoutLoans; // Объявляем переменную на уровне класса

        protected DataGrid dGrid;
        protected EntityContext context;
        private bool isClosingFromLogout = false; // Флаг для проверки, вызвано ли закрытие через разлогинивание
        protected string _currentTableName;
        protected const int PageSize = 24; // Количество строк на странице
        protected int currentPage = 1;
        protected int totalPages = 1;  // Всего страниц
        protected string currentListType;//переменная для хранения текущего типа списка будет использоваться для определения, какой именно метод инициализации должен быть вызван при смене страницы
        public string CurrentTableName
        {
            get => _currentTableName;
            set
            {
                _currentTableName = value;
                OnPropertyChanged(nameof(CurrentTableName));
                this.Title = $"{_currentTableName + "  \\ Вы вошли как пользователь: "} {UserSession.Username}";
                // Устанавливаем шрифт заголовка программно
                this.FontSize = 16;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public BaseWindow(EntityContext context)
        {
            this.context = context;
            // Убедитесь, что привязка события выполняется только один раз
            this.Closing += Window_Closing;
            // Подключаем обработчик события MouseDown
            //this.MouseDown += Window_MouseDown;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, WindowBinding_Executed));
            // CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, Window_Closing));

            // Инициализация DataGrid (или привязывание к существующему XAML элементу)
            this.DataContext = this;
            // ConfigureBooksColumns();
            LoadData();
            //           Loaded += BaseWindow_Loaded;

        }

       // Если флаг установлен, диалоговое окно подтверждения не будет показано, и окно будет закрыто.
        protected void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!isClosingFromLogout && !OnConfirmExit())
            {
                e.Cancel = true; // Отменить закрытие окна, если подтверждение не получено
            }
        }
        ///.........................................
        protected virtual bool OnConfirmExit()
        {
            var confirmExitWindow = new ConfirmExitWindow
            {
                Owner = this // Установка владельца для центрирования окна
            };
            confirmExitWindow.ShowDialog();
            return confirmExitWindow.IsConfirmed;
        }

        // Флаг isClosingFromLogout //Флаг устанавливается в true при вызове btnLogout_Click.Это позволяет Window_Closing отличить закрытие, вызванное кнопкой выхода, от других способов закрытия окна.
        protected void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (OnConfirmExit())
            {
                isClosingFromLogout = true; // Устанавливаем флаг, чтобы предотвратить повторное отображение диалогового окна
                var startWindow = new StartWindow();
                startWindow.Show();
                this.Close(); // Закрываем текущее окно
            }
        }

        protected void WindowBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentTableName = "Информация о программе";
            MessageBox.Show("Информация о программе: SchoolLibrary ver 1.0\n" +
                "Программа для автомации школьной библиотеки\n" +
                "Разработчик: студент гр. ПВ2-22ПО\n" +
                "ФИО: Филимонцев В.В.\n" +
                "2024г.");
        }

        protected void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        protected void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            if (OnConfirmExit())
            {
                Application.Current.Shutdown(); // Завершить приложение
            }
        }

        protected void LoadData()
        {
            context.Categories.Load();
            context.InventoryBooks.Load();
            context.BookPhotos.Load();
            context.Books.Load();
            context.Loans.Load();
            context.Students.Load();
        }

        //protected void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        this.DragMove();
        //    }
        //}



        protected void ApplyRowStyle(DataGrid dGrid)//подсветка строк
        {
            var rowStyle = new Style(typeof(DataGridRow));

            // Стандартный цвет фона
            rowStyle.Setters.Add(new Setter(DataGridRow.BackgroundProperty, System.Windows.Media.Brushes.Transparent));

            // Триггер для просроченных книг (если DueDate < текущей даты и книга не возвращена)
            var overdueTrigger = new MultiDataTrigger();
            overdueTrigger.Conditions.Add(new Condition(new Binding("DueDate") { Converter = new DueDateToIsOverdueConverter() }, true));
            overdueTrigger.Conditions.Add(new Condition(new Binding("ReturnDate"), null));
            overdueTrigger.Setters.Add(new Setter(DataGridRow.BackgroundProperty, System.Windows.Media.Brushes.LightCoral));
            rowStyle.Triggers.Add(overdueTrigger);

            // Триггер для книг, возвращенных сегодня (ReturnDate = текущая дата)
            var returnedTodayTrigger = new MultiDataTrigger();
            returnedTodayTrigger.Conditions.Add(new Condition(new Binding("ReturnDate"), DateTime.Now.Date));
            returnedTodayTrigger.Conditions.Add(new Condition(new Binding("DueDate") { Converter = new DueDateToIsOverdueConverter() }, false));
            returnedTodayTrigger.Setters.Add(new Setter(DataGridRow.BackgroundProperty, Brushes.LightGreen));
            rowStyle.Triggers.Add(returnedTodayTrigger);

            // Триггер для книг, которые были выданы, но ещё не просрочены (DueDate >= текущей даты и книга не возвращена)
            var notOverdueTrigger = new MultiDataTrigger();
            notOverdueTrigger.Conditions.Add(new Condition(new Binding("DueDate") { Converter = new DueDateToIsOverdueConverter() }, false));
            notOverdueTrigger.Conditions.Add(new Condition(new Binding("ReturnDate"), null));
            notOverdueTrigger.Setters.Add(new Setter(DataGridRow.BackgroundProperty, Brushes.LightGray));
            rowStyle.Triggers.Add(notOverdueTrigger);

            // Применяем стиль к DataGrid
            dGrid.RowStyle = rowStyle;
        }





        protected List<LoanViewModel> GetNotReturnedBooks()
        {
            //try
            //{
            // Загрузка данных о книгах и студентов
            context.Loans.Include(l => l.InventoryBook).Include(l => l.Student).Load();

            // Фильтрация невозвращенных книг
            var notReturnedBooksList = context.Loans.Local
                .Where(l => l.ReturnDate == null) // Выбираем только те записи, где ReturnDate == null
                .Select(l => new LoanViewModel
                {
                    Title = l.InventoryBook.Title,
                    StudentID = l.StudentID,
                    FirstName = l.Student.FirstName,
                    LastName = l.Student.LastName,
                    LoanDate = l.LoanDate,
                    DueDate = l.DueDate,
                    ReturnDate = l.ReturnDate,
                    Returned = l.Returned
                }).ToList();

            // Установка фильтрованного списка как источник данных для DataGrid
            return notReturnedBooksList;            
        }

        protected List<LoanViewModel> GetReturnedBooks()
        {
            context.Loans.Include(l => l.InventoryBook).Include(l => l.Student).Load();

            var returnedBooksList = context.Loans.Local
                .Where(l => l.ReturnDate != null)
                .Select(l => new LoanViewModel
                {
                    Title = l.InventoryBook.Title,
                    StudentID = l.StudentID,
                    FirstName = l.Student.FirstName,
                    LastName = l.Student.LastName,
                    LoanDate = l.LoanDate,
                    DueDate = l.DueDate,
                    ReturnDate = l.ReturnDate,
                    Returned = l.Returned
                }).ToList();

            return returnedBooksList;
        }
                
        protected void ConfigureLoansColumns(DataGrid dGrid)
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1.15, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя читателя", Binding = new Binding("FirstName"), Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия читателя", Binding = new Binding("LastName"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выдачи", Binding = new Binding("LoanDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Должны вернуть", Binding = new Binding("DueDate") { StringFormat = "dd/MM/yyyy" }, Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });

        }
        protected abstract void ConfigureLoansColumns();


        protected List<PaginatedStudentModel> GetPaginatedStudentsList(EntityContext context)
        {
            context.Students.Load();

            // Выбираем только активных студентов
            var activeStudents = context.Students.Local
                .Where(s => s.IsActive)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .Select((student, index) => new PaginatedStudentModel
                {
                    Index = (currentPage - 1) * PageSize + index + 1, // Вычисляем порядковый номер
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    StudentClass = student.StudentClass,
                    Prefix = student.Prefix,
                    Address = student.Address,
                    Phone = student.Phone
                })
                .ToList();

            return activeStudents;
        }


        protected void ConfigureStudentColumns(DataGrid dGrid)
        {
            dGrid.Columns.Clear();
            //Колонка для порядкового номера
            var indexColumn = new DataGridTextColumn { Header = "№", Width = new DataGridLength(0.25, DataGridLengthUnitType.Star), Binding = new Binding("Index") };
            dGrid.Columns.Add(indexColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            /*var ageColumn = new DataGridTextColumn
 {
     Header = "Возраст",
     Binding = new Binding("Age"), // Вычисляемый возраст
     Width = new DataGridLength(0.35, DataGridLengthUnitType.Star)
 };
 ageColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
 dGrid.Columns.Add(ageColumn);*/
            var ageColumn = new DataGridTextColumn
            {
                Header = "Возраст",
                Binding = new Binding("Age"), // Вычисляемый возраст
                Width = new DataGridLength(0.35, DataGridLengthUnitType.Star)
            };
            ageColumn.CellStyle = new Style(typeof(DataGridCell))
            {
                Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) }
            };
            dGrid.Columns.Add(ageColumn);
            var classColumn = new DataGridTextColumn { Header = "Класс", Binding = new Binding("StudentClass"), Width = new DataGridLength(0.3, DataGridLengthUnitType.Star) };
            classColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(classColumn);
            var prefixColumn = new DataGridTextColumn { Header = "Префикс", Binding = new Binding("Prefix"), Width = new DataGridLength(0.4, DataGridLengthUnitType.Star) };
            prefixColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(prefixColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Адрес", Binding = new Binding("Address"), Width = new DataGridLength(1.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Телефон", Binding = new Binding("Phone"), Width = new DataGridLength(0.8, DataGridLengthUnitType.Star) });
        }



        //protected void InitStudentsList()
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
        //        dGrid.Tag = "Students"; //тэг для использования при идентификации данных что сейчас в dDrid

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        protected List<LoanViewModel> GetPaginatedLoansList(EntityContext context)
        {
            context.Loans.Load();

            var loansOnPage = context.Loans.Local
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .Select((loan, index) => new LoanViewModel
                {
                    LoanID = loan.LoanID,
                    StudentID = loan.StudentID,
                    FirstName = loan.Student.FirstName,
                    LastName = loan.Student.LastName,
                    Title = loan.InventoryBook.Title,
                    Author = loan.InventoryBook.Author,
                    YearPublished = loan.InventoryBook.YearPublished.ToString(),
                    LoanDate = loan.LoanDate,
                    DueDate = loan.DueDate,
                    ReturnDate = loan.ReturnDate,
                    Returned = loan.Returned
                })
                .ToList();

            return loansOnPage;
        }

        //protected List<PaginatedStudentModel> GetPaginatedStudentsWithoutLoansList(EntityContext context)
        //{
        //    // Загружаем студентов, которые не брали книги
        //    var studentsWithoutLoansOnPage = context.Students
        //        .Where(s => !context.Loans.Any(l => l.StudentID == s.StudentID))
        //        .OrderBy(s => s.LastName)
        //        .ThenBy(s => s.FirstName)
        //        .Skip((currentPage - 1) * PageSize)
        //        .Take(PageSize)
        //        .Select((student, index) => new PaginatedStudentModel
        //        {
        //            StudentID = student.StudentID,
        //            FirstName = student.FirstName,
        //            LastName = student.LastName,
        //            DateOfBirth = student.DateOfBirth,
        //            StudentClass = student.StudentClass,
        //            Prefix = student.Prefix,
        //            Address = student.Address,
        //            Index = index + 1 + (currentPage - 1) * PageSize // Рассчитываем порядковый номер
        //        })
        //        .ToList();

        //    return studentsWithoutLoansOnPage;
        //}

        protected List<PaginatedStudentModel> GetPaginatedStudentsWithoutLoansList(EntityContext context)
        {
            // Загружаем студентов, которые не брали книги в память
            var studentsWithoutLoans = context.Students
                .Where(s => !context.Loans.Any(l => l.StudentID == s.StudentID))
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToList(); // Загружаем в память для дальнейшей обработки

            // Применяем пагинацию и преобразование к модели после загрузки данных в память
            var studentsWithoutLoansOnPage = studentsWithoutLoans
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .Select((student, index) => new PaginatedStudentModel
                {
                    StudentID = student.StudentID,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    StudentClass = student.StudentClass,
                    Prefix = student.Prefix,
                    Address = student.Address,
                    Index = index + 1 + (currentPage - 1) * PageSize // Рассчитываем порядковый номер
                })
                .ToList();

            return studentsWithoutLoansOnPage;
        }





        //protected List<PaginatedLoanModel> GetPaginatedLoansList(EntityContext context)
        //{
        //    context.Loans.Load();

        //    var loansOnPage = context.Loans.Local
        //        .Skip((currentPage - 1) * PageSize)
        //        .Take(PageSize)
        //        .Select((loan, index) => new PaginatedLoanModel
        //        {
        //            Index = (currentPage - 1) * PageSize + index + 1, // Вычисляем порядковый номер
        //            BookTitle = loan.Book.Title,
        //            StudentFullName = loan.Student.FirstName + " " + loan.Student.LastName,
        //            LoanDate = loan.LoanDate,
        //            DueDate = loan.DueDate,
        //            ReturnDate = loan.ReturnDate,
        //            Status = loan.Returned ? "Возвращена" : "На руках"
        //        })
        //        .ToList();

        //    return loansOnPage;
        //}

        protected void ConfigureLoanColumns(DataGrid dGrid)
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "№", Binding = new Binding("LoanID"), Width = new DataGridLength(0.2, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия читателя", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя читателя", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });                      
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Год издания", Binding = new Binding("YearPublished"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выдачи", Binding = new Binding("LoanDate"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Выдана до", Binding = new Binding("DueDate"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата возврата", Binding = new Binding("ReturnDate"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new Binding("Returned"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
        }


        //protected void ExportDataGridToPDF(DataGrid dataGrid, string pdfFilePath)
        //{
        //    using (var doc = new iTextSharp.text.Document())
        //    {
        //        PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //        doc.Open();

        //        // Создаем таблицу с количеством колонок, соответствующим количеству колонок в DataGrid
        //        PdfPTable pdfTable = new PdfPTable(dataGrid.Columns.Count);

        //        // Добавляем заголовки колонок
        //        foreach (var column in dataGrid.Columns)
        //        {
        //            pdfTable.AddCell(new Phrase(column.Header.ToString()));
        //        }

        //        // Добавляем строки данных
        //        foreach (var item in dataGrid.Items)
        //        {
        //            if (item == null) continue;

        //            foreach (var column in dataGrid.Columns)
        //            {
        //                // Получаем значение свойства через рефлексию
        //                var binding = (column as DataGridBoundColumn)?.Binding as Binding;
        //                if (binding != null)
        //                {
        //                    var propertyName = binding.Path.Path;
        //                    var propInfo = item.GetType().GetProperty(propertyName);
        //                    var cellValue = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
        //                    pdfTable.AddCell(new Phrase(cellValue));
        //                }
        //                else
        //                {
        //                    pdfTable.AddCell(new Phrase(string.Empty));
        //                }
        //            }
        //        }

        //        doc.Add(pdfTable);
        //        doc.Close();
        //    }

        //    MessageBox.Show($"Данные экспортированы в PDF файл: {pdfFilePath}", "Экспорт в PDF", MessageBoxButton.OK, MessageBoxImage.Information);
        //}


        /*      protected void ExportDataGridToExcel(DataGrid dataGrid, string excelFilePath)
               {
                   using (var package = new OfficeOpenXml.ExcelPackage())
                   {
                       var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                       // Добавляем заголовки колонок
                       for (int i = 0; i < dataGrid.Columns.Count; i++)
                       {
                           worksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header.ToString();
                       }

                       // Добавляем данные
                       for (int i = 0; i < dataGrid.Items.Count; i++)
                       {
                           var item = dataGrid.Items[i];
                           for (int j = 0; j < dataGrid.Columns.Count; j++)
                           {
                               var column = dataGrid.Columns[j] as DataGridBoundColumn;
                               if (column != null)
                               {
                                   var binding = column.Binding as Binding;
                                   if (binding != null)
                                   {
                                       var propertyName = binding.Path.Path;
                                       var propInfo = item.GetType().GetProperty(propertyName);
                                       var cellValue = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
                                       worksheet.Cells[i + 2, j + 1].Value = cellValue;
                                   }
                               }
                           }
                       }

                       // Сохраняем файл
                       package.SaveAs(new FileInfo(excelFilePath));
                   }

                   MessageBox.Show($"Данные экспортированы в Excel файл: {excelFilePath}", "Экспорт в Excel", MessageBoxButton.OK, MessageBoxImage.Information);
               }
       */

        //protected void ExportDataGridToPDF(DataGrid dataGrid, string pdfFilePath)
        //// {
        //using (var doc = new iTextSharp.text.Document())
        //{
        //    PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        //    doc.Open();

        //    // Создаем таблицу с количеством колонок, соответствующим количеству колонок в DataGrid
        //    PdfPTable pdfTable = new PdfPTable(dataGrid.Columns.Count);

        //    // Добавляем заголовки колонок
        //    foreach (var column in dataGrid.Columns)
        //    {
        //        pdfTable.AddCell(new Phrase(column.Header.ToString()));
        //    }

        //    // Добавляем строки данных
        //    foreach (var item in dataGrid.Items)
        //    {
        //        if (item == null) continue;

        //        foreach (var column in dataGrid.Columns)
        //        {
        //            // Получаем значение свойства через рефлексию
        //            var binding = (column as DataGridBoundColumn)?.Binding as Binding;
        //            if (binding != null)
        //            {
        //                var propertyName = binding.Path.Path;
        //                var propInfo = item.GetType().GetProperty(propertyName);
        //                var cellValue = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
        //                pdfTable.AddCell(new Phrase(cellValue));
        //            }
        //            else
        //            {
        //                pdfTable.AddCell(new Phrase(string.Empty));
        //            }
        //        }
        //    }

        //    doc.Add(pdfTable);
        //    doc.Close();
        //}

        //MessageBox.Show($"Данные экспортированы в PDF файл: {pdfFilePath}", "Экспорт в PDF", MessageBoxButton.OK, MessageBoxImage.Information);
        //}

        //private void ConfigureBooksColumns()
        //{
        //    if (dGrid == null)
        //    {
        //        MessageBox.Show("DataGrid is not initialized.");
        //        return;
        //    }
        //    dGrid.Columns.Clear();
        //    //dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Количество", Binding = new Binding("Quantity"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Остаток", Binding = new Binding("QuantityLeft"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}
        //private void ConfigureCategoryColumns()
        //{
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("CategoryID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}

        //private void ConfigureBookPhotosColumns()
        //{
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("BookPhotoID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID книги", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата добавления", Binding = new Binding("DateAdded"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}

        //protected void ConfigureInventoryBooksColumns()
        //{
        //    context.Books.Include(b => b.Category).Load();
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Инв. номер", Binding = new Binding("InventoryNumber"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("Book.Category.CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Год выпуска", Binding = new Binding("YearPublished"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Стоимость", Binding = new Binding("Price"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата прихода", Binding = new Binding("DateOfReceipt"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Приходный документ", Binding = new Binding("IncomingInvoice"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выбытия", Binding = new Binding("DateOfDisposal"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Расходный документ", Binding = new Binding("OutgoingInvoice"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Причина выбытия", Binding = new Binding("ReasonForDisposal"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}

        //protected void InitCategoriesList()
        //{
        //    try
        //    {
        //        context.Categories.Load();
        //        ConfigureCategoryColumns();
        //        dGrid.ItemsSource = context.Categories.Local;
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        //protected void InitBookPhotosListList()
        //{
        //    try
        //    {
        //        context.BookPhotos.Load();
        //        ConfigureBookPhotosColumns();
        //        dGrid.ItemsSource = context.BookPhotos.Local;
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        //protected void InitInventoryBooksList()
        //{
        //    try
        //    {
        //        context.InventoryBooks.Load();
        //        ConfigureInventoryBooksColumns();
        //        dGrid.ItemsSource = context.InventoryBooks.Local;
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        //protected void InitBooksList()
        //{
        //    try
        //    {
        //        context.Books.Include(b => b.Category).Include(b => b.InventoryBooks.Select(ib => ib.Loans)).Load();

        //        var groupedBooks = context.Books.Local
        //            .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
        //            .GroupBy(x => x.InventoryBook.ISBN)
        //            .Select(g => new LoanWindowsViewModel
        //            {
        //                BookID = g.First().Book.BookID,
        //                Title = g.First().InventoryBook.Title,
        //                Author = g.First().InventoryBook.Author,
        //                Publisher = g.First().InventoryBook.Publisher,
        //                YearPublished = g.First().InventoryBook.YearPublished,
        //                ISBN = g.Key,
        //                Quantity = g.Count(),
        //                QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
        //                CategoryName = g.First().Book.Category.CategoryName
        //            }).ToList();

        //        ConfigureBooksColumns();
        //        dGrid.ItemsSource = groupedBooks;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void updateListInventoryBook()
        //{
        //    try
        //    {
        //        dGrid.ItemsSource = null;
        //        InitInventoryBooksList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void updateListCategories()
        //{
        //    try
        //    {
        //        dGrid.ItemsSource = null;
        //        InitCategoriesList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private List<Category> GetCategories()
        //{
        //    return context.Categories.ToList();
        //}

        //private List<InventoryBook> GetInventoryBooks()
        //{
        //    return context.InventoryBooks.ToList();
        //}

        //protected void InitStudentsList()
        //{
        //    try
        //    {
        //        context.Students.Load();
        //        ConfigureStudentColumns();
        //        dGrid.ItemsSource = context.Students.Local;
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        //protected void InitCategoriesList()
        //{
        //    try
        //    {
        //        context.Categories.Load();
        //        ConfigureCategoryColumns();
        //        dGrid.ItemsSource = context.Categories.Local;
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        //protected void InitBookPhotosListList()
        //{
        //    try
        //    {
        //        context.BookPhotos.Load();
        //        ConfigureBookPhotosColumns();
        //        dGrid.ItemsSource = context.BookPhotos.Local;
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        //protected void InitInventoryBooksList()
        //{
        //    try
        //    {
        //        context.InventoryBooks.Load();
        //        ConfigureInventoryBooksColumns();
        //        dGrid.ItemsSource = context.InventoryBooks.Local;
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

        //protected void InitLoansList()
        //{
        //    try
        //    {
        //        context.Loans.Include(l => l.InventoryBook).Include(l => l.Student).Load();

        //        var loansList = context.Loans.Local
        //            .Select(l => new
        //            {
        //                l.InventoryBook.Title,
        //                l.Student.FirstName,
        //                l.Student.LastName,
        //                l.LoanDate,
        //                l.DueDate,
        //                l.ReturnDate,
        //                l.Returned
        //            }).ToList();

        //        dGrid.ItemsSource = loansList;
        //        ConfigureLoansColumns();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //protected void ConfigureLoansColumns()
        //{
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя студента", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия студента", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда взяли", Binding = new Binding("LoanDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда должны вернуть", Binding = new Binding("DueDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Подтверждение возврата", Binding = new Binding("Returned"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}
        //protected void ConfigureBooksColumns()
        //{
        //    //if (dGrid == null)
        //    //{
        //    //    MessageBox.Show("DataGrid is not initialized.");
        //    //    return;
        //    //}
        //    dGrid.Columns.Clear();
        //    //dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Количество", Binding = new Binding("Quantity"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Остаток", Binding = new Binding("QuantityLeft"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}
        //protected void ConfigureStudentColumns()
        //{
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Возраст", Binding = new Binding("Age"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Класс", Binding = new Binding("Class"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}
        //protected void ConfigureCategoryColumns()
        //{
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("CategoryID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}
        //protected void ConfigureBookPhotosColumns()
        //{
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("BookPhotoID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID книги", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата добавления", Binding = new Binding("DateAdded"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}
        //private void ConfigureInventoryBooksColumns()
        //{
        //    context.Books.Include(b => b.Category).Load();
        //    dGrid.Columns.Clear();
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Инв. номер", Binding = new Binding("InventoryNumber"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("Book.Category.CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Год выпуска", Binding = new Binding("YearPublished"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Стоимость", Binding = new Binding("Price"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата прихода", Binding = new Binding("DateOfReceipt"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Приходный документ", Binding = new Binding("IncomingInvoice"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выбытия", Binding = new Binding("DateOfDisposal"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Расходный документ", Binding = new Binding("OutgoingInvoice"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //    dGrid.Columns.Add(new DataGridTextColumn { Header = "Причина выбытия", Binding = new Binding("ReasonForDisposal"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        //}

        //protected void updateListCategories()
        //{
        //    try
        //    {
        //        dGrid.ItemsSource = null;
        //        InitCategoriesList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //protected void updateListStudent()
        //{
        //    try
        //    {
        //        dGrid.ItemsSource = null;
        //        InitStudentsList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //protected void updateListInventoryBook()
        //{
        //    try
        //    {
        //        dGrid.ItemsSource = null;
        //        InitInventoryBooksList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //protected List<Student> GetStudents()
        //{
        //    return context.Students.ToList();
        //}

        //protected List<Category> GetCategories()
        //{
        //    return context.Categories.ToList();
        //}

        //protected List<InventoryBook> GetInventoryBooks()
        //{
        //    return context.InventoryBooks.ToList();
        //}


        //protected void btnListBooks(object sender, RoutedEventArgs e)
        //{
        //    CurrentTableName = "Перечень книг";
        //    try
        //    {
        //        dGrid.ItemsSource = null;
        //        InitBooksList();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void btnEditCategory_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dGrid.SelectedItem is Category selectedCategory)
        //    {
        //        var categoryCopy = new Category
        //        {
        //            CategoryID = selectedCategory.CategoryID,
        //            CategoryName = selectedCategory.CategoryName
        //        };

        //        EditCategoryDialog dialog = new EditCategoryDialog(context, categoryCopy);
        //        if (dialog.ShowDialog() == true)
        //        {
        //            selectedCategory.CategoryName = categoryCopy.CategoryName;
        //            context.SaveChanges();

        //            dGrid.ItemsSource = context.Categories.ToList();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Выберите категорию для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        //        updateListCategories();
        //    }
        //}


        protected void DisplayPaginatedStudentsWithoutLoans()
        {
            var paginatedStudentsWithoutLoans = studentsWithoutLoans
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .Select((student, index) => new PaginatedStudentModel
                {
                    StudentID = student.StudentID,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    StudentClass = student.StudentClass,
                    Prefix = student.Prefix,
                    Address = student.Address,
                    Index = index + 1 + (currentPage - 1) * PageSize // Рассчитываем порядковый номер
                })
                .ToList();

            // Настройка колонок и обновление DataGrid
            ConfigureStudentColumns(dGrid);
            dGrid.ItemsSource = paginatedStudentsWithoutLoans;
            dGrid.UpdateLayout();
        }


    }

}
