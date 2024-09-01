using SchoolLibrary.DialogWindows.LoanWindows;
using SchoolLibrary.Models;
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
    /// Логика взаимодействия для StatisticsPageStudent.xaml
    /// </summary>
    public partial class StatisticsPageStudent : Window
    {
        private readonly EntityContext _context;
        private int _currentPage = 1;
        private const int _pageSize = 20;
        private int _totalPages;

        public StatisticsPageStudent(EntityContext context)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _context = context;
            LoadStudents();
        }

        private void LoadStudents(string searchQuery = "")
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(s => s.LastName.Contains(searchQuery) ||
                                         s.FirstName.Contains(searchQuery) ||
                                         s.StudentClass.Contains(searchQuery) ||
                                         s.Address.Contains(searchQuery));
            }

            // StudentsDataGrid.ItemsSource = query.Where(s => s.IsActive).ToList();
            // Count the total number of records
            _totalPages = (int)Math.Ceiling(query.Count() / (double)_pageSize);

            // Get the students for the current page
            var students = query
                .Where(s => s.IsActive)
                .OrderBy(s => s.StudentID) // Ensure consistent ordering
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            StudentsDataGrid.ItemsSource = students;
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
            LoadStudents(SearchTextBox.Text);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadStudents(SearchTextBox.Text);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadStudents(SearchTextBox.Text);
            }
        }

        private void StudentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem is Student selectedStudent)
            {
                var detailsWindow = new StudentDetailsWindow(selectedStudent, _context);
                detailsWindow.Show();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If needed, you can add logic to handle text changes
        }        
    }
}
