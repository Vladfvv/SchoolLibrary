using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SchoolLibrary.DialogWindows.StudentWindows
{
    /// <summary>
    /// Логика взаимодействия для SearchStudentDialog.xaml
    /// </summary>
    public partial class SearchStudentDialog : Window
    {
        private readonly EntityContext _context;

        public SearchStudentDialog(EntityContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();

            int minAge, maxAge, minClass, maxClass;
            bool isMinAgeValid = int.TryParse(MinAgeTextBox.Text, out minAge);
            bool isMaxAgeValid = int.TryParse(MaxAgeTextBox.Text, out maxAge);
            bool isMinClassValid = int.TryParse(MinClassTextBox.Text, out minClass);
            bool isMaxClassValid = int.TryParse(MaxClassTextBox.Text, out maxClass);

            if ((MinAgeTextBox.Text != string.Empty && !isMinAgeValid) ||
                (MaxAgeTextBox.Text != string.Empty && !isMaxAgeValid) ||
                (MinClassTextBox.Text != string.Empty && !isMinClassValid) ||
                (MaxClassTextBox.Text != string.Empty && !isMaxClassValid))
            {
                StatusTextBlock.Text = "Please enter valid numerical values.";
                return;
            }

            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(s => s.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(s => s.LastName.Contains(lastName));
            }

            if (isMinAgeValid)
            {
                query = query.Where(s => s.Age >= minAge);
            }

            if (isMaxAgeValid)
            {
                query = query.Where(s => s.Age <= maxAge);
            }

            if (isMinClassValid)
            {
                query = query.Where(s => s.Class.CompareTo(minClass.ToString()) >= 0);
            }

            if (isMaxClassValid)
            {
                query = query.Where(s => s.Class.CompareTo(maxClass.ToString()) <= 0);
            }

            var results = query.ToList();

            if (results.Count > 0)
            {
                // Assuming MainWindow has a method to update the DataGrid with the search results
                (Application.Current.MainWindow as MainWindow)?.UpdateStudentDataGrid(results);
                this.Close();
            }
            else
            {
                StatusTextBlock.Text = "No students found matching the criteria.";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
