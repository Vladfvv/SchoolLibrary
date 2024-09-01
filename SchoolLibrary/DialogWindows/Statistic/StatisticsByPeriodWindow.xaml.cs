using SchoolLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolLibrary.DialogWindows.Statistic
{
    /// <summary>
    /// Логика взаимодействия для StatisticsByPeriodWindow.xaml
    /// </summary>
    public partial class StatisticsByPeriodWindow : Window
    {
        private readonly EntityContext _context;
        private int _currentPage = 1;
        private const int _pageSize = 20;
        private int _totalPages;

        public StatisticsByPeriodWindow(EntityContext context)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            StartDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
            StartDatePicker.FirstDayOfWeek = DayOfWeek.Monday;
            EndDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
            EndDatePicker.FirstDayOfWeek = DayOfWeek.Monday;
            _context = context;
            LoadBooks();
        }

        //private void LoadBooks(DateTime? startDate = null, DateTime? endDate = null)
        //{
        //    var query = _context.Loans.AsQueryable();

        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(l => l.LoanDate >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        query = query.Where(l => l.LoanDate <= endDate.Value);
        //    }

        //    _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

        //    var books = query
        //        .Select(l => new
        //        {
        //            l.InventoryBook.InventoryNumber,
        //            l.InventoryBook.Title,
        //            l.InventoryBook.Author,
        //            l.InventoryBook.ISBN,
        //            l.LoanDate,
        //            l.ReturnDate
        //        })
        //        .OrderBy(b => b.LoanDate)
        //        .Skip((_currentPage - 1) * _pageSize)
        //        .Take(_pageSize)
        //        .ToList();

        //    BooksDataGrid.ItemsSource = books;
        //    UpdatePaginationControls();
        //}

        //private void LoadBooks(DateTime? startDate = null, DateTime? endDate = null)
        //{
        //    var query = _context.Loans.AsQueryable();

        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(l => l.LoanDate >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        query = query.Where(l => l.LoanDate <= endDate.Value);
        //    }

        //    _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

        //    var loans = query
        //        .OrderBy(l => l.LoanDate)
        //        .Skip((_currentPage - 1) * _pageSize)
        //        .Take(_pageSize)
        //        .Select(l => new LoanInventoryBookStatisticViewModel
        //        {
        //            InventoryNumber = l.InventoryBook.InventoryNumber,
        //            Title = l.InventoryBook.Title,
        //            Author = l.InventoryBook.Author,
        //            ISBN = l.InventoryBook.ISBN,
        //            LoanDate = l.LoanDate,
        //            ReturnDate = l.ReturnDate
        //        })
        //        .ToList();

        //    BooksDataGrid.ItemsSource = loans; // Устанавливаем источник данных для DataGrid
        //    UpdatePaginationControls(); // Обновляем элементы управления пагинацией
        //}


        //private void LoadBooks(DateTime? startDate = null, DateTime? endDate = null)
        //{
        //    var query = _context.Loans.AsQueryable();

        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(l => l.LoanDate >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        query = query.Where(l => l.LoanDate <= endDate.Value);
        //    }

        //    _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

        //    var loans = query
        //        .Select(l => new LoanInventoryBookStatisticViewModel
        //        {
        //            InventoryNumber = l.InventoryBook.InventoryNumber,
        //            Title = l.InventoryBook.Title,
        //            Author = l.InventoryBook.Author,
        //            ISBN = l.InventoryBook.ISBN,
        //            LoanDate = l.LoanDate,
        //            ReturnDate = l.ReturnDate
        //        })
        //        .OrderBy(b => b.InventoryNumber)  // Сортировка по инвентарному номеру
        //        .ThenBy(b => b.LoanDate)          // Затем по дате выдачи
        //        .Skip((_currentPage - 1) * _pageSize)
        //        .Take(_pageSize)
        //        .ToList();

        //    BooksDataGrid.ItemsSource = loans; // Устанавливаем источник данных для DataGrid
        //    UpdatePaginationControls(); // Обновляем элементы управления пагинацией
        //}


        private void LoadBooks(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Loans.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(l => l.LoanDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(l => l.LoanDate <= endDate.Value);
            }

            _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

            var loans = query
                .OrderBy(l => l.LoanDate) // Сортировка по дате выдачи
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .Select(l => new LoanInventoryBookStatisticViewModel
                {
                    InventoryNumber = l.InventoryBook.InventoryNumber,
                    Title = l.InventoryBook.Title,
                    Author = l.InventoryBook.Author,
                    ISBN = l.InventoryBook.ISBN,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate
                })
                .ToList();

            BooksDataGrid.ItemsSource = loans; // Устанавливаем источник данных для DataGrid
            UpdatePaginationControls(); // Обновляем элементы управления пагинацией
        }


        private void UpdatePaginationControls()
        {
            PreviousButton.IsEnabled = _currentPage > 1;
            NextButton.IsEnabled = _currentPage < _totalPages;
            PageNumberTextBlock.Text = $"Страница {_currentPage} из {_totalPages}";
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1; // Reset to first page on search
            LoadBooks(StartDatePicker.SelectedDate, EndDatePicker.SelectedDate);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadBooks(StartDatePicker.SelectedDate, EndDatePicker.SelectedDate);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadBooks(StartDatePicker.SelectedDate, EndDatePicker.SelectedDate);
            }
        }

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optionally handle row selection if needed
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // This event handler can be used to update the search results when dates change, if needed
        }
    }
}