using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
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
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.DataContext = context;
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
                StatusTextBlock.Text = "Введите корректные данные.";
                return;
            }

            // Получаем список студентов из базы данных
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(s => s.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(s => s.LastName.Contains(lastName));
            }

            // Получаем данные из базы данных
            var students = query.ToList();

            // Выполняем вычисления после получения данных
            var results = students
                .Where(s => (!isMinAgeValid || s.Age >= minAge) &&
                            (!isMaxAgeValid || s.Age <= maxAge) &&
                            (!isMinClassValid || s.StudentClass.CompareTo(minClass.ToString()) >= 0) &&
                            (!isMaxClassValid || s.StudentClass.CompareTo(maxClass.ToString()) <= 0))
                .Select((student, index) => new PaginatedStudentModel
                {
                    StudentID = student.StudentID,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    StudentClass = student.StudentClass,
                    Prefix = student.Prefix,
                    Address = student.Address,
                    Index = index + 1
                })
                .ToList();

            if (results.Count > 0)
            {
                // Устанавливаем результаты в Tag и закрываем диалог
                this.Tag = results;
                this.DialogResult = true;
            }
            else
            {
                StatusTextBlock.Text = "Читателей с заданными параметрами не найдено.";
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }       
    }
}
