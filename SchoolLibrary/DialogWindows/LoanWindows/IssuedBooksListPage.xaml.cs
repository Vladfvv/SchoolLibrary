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

namespace SchoolLibrary.DialogWindows.LoanWindows
{
    /// <summary>
    /// Логика взаимодействия для IssuedBooksListPage.xaml
    /// </summary>
    public partial class IssuedBooksListPage : Window
    {
        private readonly EntityContext _context;
        private int _currentPage = 1;
        private const int _pageSize = 20;
        private int _totalPages;

        public IssuedBooksListPage(EntityContext context)
        {
            InitializeComponent();
            _context = context;
            LoadIssuedBooks();
        }

        private void LoadIssuedBooks(string searchQuery = "")
        {
            var query = _context.Loans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(l => l.InventoryBook.Title.Contains(searchQuery) ||
                                         l.InventoryBook.Author.Contains(searchQuery) ||
                                         l.InventoryBook.ISBN.Contains(searchQuery));
            }

            _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

            var issuedBooks = query
                .Select(l => new
                {
                    l.InventoryBook.InventoryNumber,
                    l.InventoryBook.Title,
                    l.InventoryBook.Author,
                    l.InventoryBook.ISBN,
                    l.LoanDate,
                    l.ReturnDate
                })
                .OrderBy(b => b.Title)
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            IssuedBooksDataGrid.ItemsSource = issuedBooks;
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
            LoadIssuedBooks(SearchTextBox.Text);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadIssuedBooks(SearchTextBox.Text);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadIssuedBooks(SearchTextBox.Text);
            }
        }

        private void IssuedBooksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optionally handle row selection if needed
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Optionally handle text changes if needed
        }
    }
}
