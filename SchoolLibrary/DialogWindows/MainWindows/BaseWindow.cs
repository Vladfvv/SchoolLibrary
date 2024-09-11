using SchoolLibrary.AuthWindows;
using SchoolLibrary.DialogWindows.Operations;
using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using SchoolLibrary.ViewModels;
using SchoolLibrary.Converters;
using System.Windows.Media;
using NLog;  // Для SaveFileDialog


namespace SchoolLibrary.Views
{
    public abstract class BaseWindow : BaseSettings, INotifyPropertyChanged
    {
        private readonly List<Student> studentsWithoutLoans; // Объявляем переменную на уровне класса
        protected static Logger logger = LogManager.GetCurrentClassLogger();
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
            // привязка события закрытия окна
            this.Closing += Window_Closing;

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, WindowBinding_Executed));

            // Инициализация DataGrid (или привязывание к существующему XAML элементу)
            this.DataContext = this;

            //LoadData();            

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
                // Логируем выход пользователя из системы
                logger.Info($"Пользователь {UserSession.Username} вышел из системы в {DateTime.Now}");

                // Очищаем данные сессии 
                UserSession.Username = null;
                UserSession.Role = null;
                UserSession.UserId = 0;

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

        //protected void LoadData()
        //{
        //    context.Categories.Load();
        //    context.InventoryBooks.Load();
        //    context.BookPhotos.Load();
        //    context.Books.Load();
        //    context.Loans.Load();
        //    context.Students.Load();
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

            // Триггер для изменения стиля при наведении мыши
            var mouseOverTrigger = new Trigger
            {
                Property = DataGridRow.IsMouseOverProperty,
                Value = true
            };
            mouseOverTrigger.Setters.Add(new Setter(DataGridRow.BackgroundProperty, Brushes.LightBlue)); // Изменение фона при наведении
            mouseOverTrigger.Setters.Add(new Setter(DataGridRow.ForegroundProperty, Brushes.White)); // Изменение цвета текста при наведении
            rowStyle.Triggers.Add(mouseOverTrigger);

            // Применяем стиль к DataGrid
            dGrid.RowStyle = rowStyle;
        }


        protected List<LoanViewModel> GetNotReturnedBooks()
        {
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
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1.2, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя читателя", Binding = new Binding("FirstName"), Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия читателя", Binding = new Binding("LastName"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
                     
            dGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Дата выдачи",
                Binding = new Binding("LoanDate")
                {
                    StringFormat = "dd/MM/yyyy HH:mm"
                },
                Width = new DataGridLength(0.5, DataGridLengthUnitType.Star),
                ElementStyle = new Style(typeof(TextBlock))
                {
                    Setters = { new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center) }
                }
            });
            
            dGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Должны вернуть",
                Binding = new Binding("DueDate")
                {
                    StringFormat = "dd/MM/yyyy"
                },
                Width = new DataGridLength(0.45, DataGridLengthUnitType.Star),
                ElementStyle = new Style(typeof(TextBlock))
                {
                    Setters = { new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center) }
                }
            });
           
            dGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Когда вернули",
                Binding = new Binding("ReturnDate")
                {
                    StringFormat = "dd/MM/yyyy HH:mm"
                },
                Width = new DataGridLength(0.5, DataGridLengthUnitType.Star),
                ElementStyle = new Style(typeof(TextBlock))
                {
                    Setters = { new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center) }
                }
            });
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
            var indexColumn = new DataGridTextColumn { Header = "№", Binding = new Binding("Index"), Width = new DataGridLength(0.3, DataGridLengthUnitType.Star) };
            indexColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(indexColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
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



        protected void ConfigureLoanColumns(DataGrid dGrid)
        {
            dGrid.Columns.Clear();
            var indexColumn = new DataGridTextColumn { Header = "№", Binding = new Binding("LoanID"), Width = new DataGridLength(0.25, DataGridLengthUnitType.Star) };
            indexColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(indexColumn);

            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1.4, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(0.7, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия читателя", Binding = new Binding("LastName"), Width = new DataGridLength(0.8, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя читателя", Binding = new Binding("FirstName"), Width = new DataGridLength(0.65, DataGridLengthUnitType.Star) });
                       
            var loanDateColumn = new DataGridTextColumn
            {
                Header = "Дата выдачи",
                Binding = new Binding("LoanDate")
                {
                    StringFormat = "dd/MM/yyyy HH:mm" // Формат даты и времени
                },
                Width = new DataGridLength(0.5, DataGridLengthUnitType.Star)
            };

            // Выровнять текст по центру
            loanDateColumn.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters = { new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center) }
            };

            dGrid.Columns.Add(loanDateColumn);
            //dGrid.Columns.Add(new DataGridTextColumn { Header = "Выдана до", Binding = new Binding("DueDate"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            var dueDateColumn = new DataGridTextColumn
            {
                Header = "Выдана до",
                Binding = new Binding("DueDate")
                {
                    StringFormat = "dd/MM/yyyy" // Формат отображения только даты
                },
                Width = new DataGridLength(0.4, DataGridLengthUnitType.Star)
            };

            // Выровнять текст по центру
            dueDateColumn.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters = { new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center) }
            };

            dGrid.Columns.Add(dueDateColumn);

            // dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата возврата", Binding = new Binding("ReturnDate"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            
            var returnDateColumn = new DataGridTextColumn
            {
                Header = "Дата возврата",
                Binding = new Binding("ReturnDate")
                {
                    StringFormat = "dd/MM/yyyy HH:mm" // Формат даты и времени
                },
                Width = new DataGridLength(0.5, DataGridLengthUnitType.Star)
            };

            // Выровнять текст по центру
            returnDateColumn.ElementStyle = new Style(typeof(TextBlock))
            {
                Setters = { new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center) }
            };

            dGrid.Columns.Add(returnDateColumn);
            //var stateColumn = new DataGridTextColumn { Header = "№", Binding = new Binding("Returned"), Width = new DataGridLength(0.25, DataGridLengthUnitType.Star) };
            //stateColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            //dGrid.Columns.Add(stateColumn);

        }


    }

}
