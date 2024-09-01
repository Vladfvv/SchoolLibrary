using SchoolLibrary.Models;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolLibrary.DialogWindows.Statistic
{
    /// <summary>
    /// Логика взаимодействия для StatisticsPageInventoryBook.xaml
    /// </summary>
    public partial class StatisticsPageInventoryBook : Window
    {
        private readonly EntityContext _context;
        private int _currentPage = 1;
        private const int _pageSize = 20;
        private int _totalPages;

        public StatisticsPageInventoryBook(EntityContext context)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _context = context;
            LoadBooks();
        }

        //private void LoadBooks(string searchQuery = "")
        //{
        //    var query = _context.InventoryBooks.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(searchQuery))
        //    {
        //        query = query.Where(b => b.Title.Contains(searchQuery) ||
        //                                 b.Author.Contains(searchQuery) ||
        //                                 b.ISBN.Contains(searchQuery));
        //    }

        //    // BooksDataGrid.ItemsSource = query.ToList();

        //    _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

        //    var books = query
        //        .OrderBy(b => b.InventoryBookID) // Ensure consistent ordering
        //        .Skip((_currentPage - 1) * _pageSize)
        //        .Take(_pageSize)
        //        .ToList();

        //    BooksDataGrid.ItemsSource = books;
        //    UpdatePaginationControls();
        //}

        private void LoadBooks(string searchQuery = "")
        {
            var query = _context.InventoryBooks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(b => b.Title.Contains(searchQuery) ||
                                         b.Author.Contains(searchQuery) ||
                                         b.ISBN.Contains(searchQuery));
            }

            _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

            var books = query
                .OrderBy(b => b.InventoryBookID) // Ensure consistent ordering
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            var bookViewModels = books.Select((b, index) => new InventoryBookViewModel
            {
                RowNumber = (_currentPage - 1) * _pageSize + index + 1, // Calculate the row number
                InventoryBook = b
            }).ToList();

            BooksDataGrid.ItemsSource = bookViewModels;
            UpdatePaginationControls();
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
            LoadBooks(SearchTextBox.Text);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadBooks(SearchTextBox.Text);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadBooks(SearchTextBox.Text);
            }
        }

        //private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (BooksDataGrid.SelectedItem is InventoryBook selectedBook)
        //    {
        //        var viewModel = new InventoryBookDetailsViewModel
        //        {
        //            InventoryBook = selectedBook,
        //            // Loans = _context.Loans.Where(l => l.InventoryBookId == selectedBook.Id).ToList()
        //            Loans = _context.Loans.Where(l => l.InventoryBookID == selectedBook.InventoryBookID).ToList()
        //        };

        //        var detailsWindow = new BookDetailsWindow(viewModel);
        //        detailsWindow.Show();
        //    }
        //}

        private void BooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is InventoryBookViewModel selectedBookViewModel)
            {
                var selectedBook = selectedBookViewModel.InventoryBook;

                var viewModel = new InventoryBookDetailsViewModel
                {
                    InventoryBook = selectedBook,
                    Loans = _context.Loans.Where(l => l.InventoryBookID == selectedBook.InventoryBookID).ToList()
                };

                var detailsWindow = new BookDetailsWindow(viewModel);
                detailsWindow.Show();
            }
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If needed, you can add logic to handle text changes
        }
    }
}
