using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EditStudentDialog.xaml
    /// </summary>
    public partial class EditStudentDialog : Window, INotifyPropertyChanged
    {
        private readonly EntityContext _context;
        public Student student { get; set; }

        public Student Student
        {
            get { return student; }
            set
            {
                student = value;
                OnPropertyChanged();
            }
        }

        public EditStudentDialog(EntityContext context, Student student)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = this;
            _context = context;
            this.student = student;           
        }
                

        private void SaveStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // Валидация возраста
                var age = student.Age;
                if (age < 3 || age > 110)
                {
                    MessageBox.Show("Введите корректную дату рождения. Возраст читателя должен быть от 3 до 110 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Валидация класса
                if (!int.TryParse(student.StudentClass, out int studentClass) || studentClass < 1 || studentClass > 11)
                {
                    MessageBox.Show("Введите корректный класс. Класс читателя должен быть от 1 до 11.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                // Получаем текст из текстового поля телефона
                string phoneNumber = txtPhone.Text;
                if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\+?[0-9]{10,15}$"))
                {
                    MessageBox.Show("Телефон должен начинаться с +. Пожалуйста, введите корректный телефонный номер (10-15 цифр).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                var existingStudent = _context.Students.SingleOrDefault(s => s.StudentID == student.StudentID);
               
                if (existingStudent != null)
                {
                    existingStudent.FirstName = student.FirstName;
                    existingStudent.LastName = student.LastName;
                    existingStudent.DateOfBirth = student.DateOfBirth;
                    existingStudent.StudentClass = student.StudentClass;
                    existingStudent.Prefix = student.Prefix;
                    existingStudent.Address = student.Address;
                    existingStudent.IsActive = student.IsActive;

                    _context.SaveChanges();
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения читателя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отмена действия");
            DialogResult = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }        
    }
}
