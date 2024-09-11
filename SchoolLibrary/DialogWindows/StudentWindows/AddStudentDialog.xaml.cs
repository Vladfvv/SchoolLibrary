using NLog;
using SchoolLibrary.Models;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace SchoolLibrary.DialogWindows.StudentWindows
{
    public partial class AddStudentDialog : Window
    {
        private readonly EntityContext context;
        private readonly Student newStudent;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public AddStudentDialog(EntityContext dbContext)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            context = dbContext;
            newStudent = new Student();
           // DataContext = newStudent;
        }      

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string errorMessage = string.Empty;

                // Проверка на пустые поля
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                    errorMessage += "Пожалуйста, введите имя читателя.\n";
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                    errorMessage += "Пожалуйста, введите фамилию читателя.\n";
                if (!DateTime.TryParse(txtDateOfBirth.Text, out DateTime dateOfBirth))
                    errorMessage += "Пожалуйста, введите корректную дату рождения читателя.\n";
                if (!int.TryParse(txtStudentClass.Text, out int studentClass) || studentClass < 1 || studentClass > 11)
                    errorMessage += "Пожалуйста, введите корректный класс читателя (от 1 до 11).\n";
                if (string.IsNullOrWhiteSpace(txtPrefix.Text))
                    errorMessage += "Пожалуйста, введите префикс читателя.\n";
                if (string.IsNullOrWhiteSpace(txtAddress.Text))
                    errorMessage += "Пожалуйста, введите адрес читателя.\n";
                if (string.IsNullOrWhiteSpace(txtPhone.Text) || !Regex.IsMatch(txtPhone.Text, @"^\+?[0-9]{10,15}$"))
                    errorMessage += "Пожалуйста, введите корректный телефонный номер (10-15 цифр).\n";


                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "Вы неверно ввели следующие данные:\n" + errorMessage;
                    throw new Exception(errorMessage);
                }

                // Приведение studentClass к строке, если поле StudentClass в базе данных имеет тип string
                var studentClassString = studentClass.ToString();

                // Проверка наличия студента в базе данных
                var existingStudent = context.Students.FirstOrDefault(s =>
                    s.FirstName == txtFirstName.Text &&
                    s.LastName == txtLastName.Text &&
                    s.DateOfBirth == dateOfBirth &&
                    s.StudentClass == studentClassString &&
                    s.Prefix == txtPrefix.Text &&
                    s.Address == txtAddress.Text
                );

                if (existingStudent != null)
                {
                    errorMessage = "Читатель с такими данными уже существует:\n";
                    if (existingStudent.FirstName != txtFirstName.Text)
                        errorMessage += "Имя не совпадает - в базе данных: " + existingStudent.FirstName + "\n";
                    if (existingStudent.LastName != txtLastName.Text)
                        errorMessage += "Фамилия не совпадает - в базе данных: " + existingStudent.LastName + "\n";
                    if (existingStudent.DateOfBirth != dateOfBirth)
                        errorMessage += "Дата рождения не совпадает - в базе данных: " + existingStudent.DateOfBirth.ToShortDateString() + "\n";
                    if (existingStudent.StudentClass != studentClassString)
                        errorMessage += "Класс не совпадает - в базе данных: " + existingStudent.StudentClass + "\n";
                    if (existingStudent.Prefix != txtPrefix.Text)
                        errorMessage += "Префикс не совпадает - в базе данных: " + existingStudent.Prefix + "\n";
                    if (existingStudent.Address != txtAddress.Text)
                        errorMessage += "Адрес не совпадает - в базе данных: " + existingStudent.Address + "\n";

                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Добавление нового студента в базу данных
                var newStudent = new Student
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    DateOfBirth = dateOfBirth,
                    StudentClass = studentClassString,
                    Prefix = txtPrefix.Text,
                    Address = txtAddress.Text,
                    Phone = txtPhone.Text, 
                    IsActive = true
                };

                context.Students.Add(newStudent);
                context.SaveChanges();

                MessageBox.Show("Читатель добавлен успешно!");

                // Логирование с указанием пользователя, даты и времени добавления записи
                DateTime currentDateTime = DateTime.Now;
                logger.Info($"[{currentDateTime:yyyy-MM-dd HH:mm:ss}] Пользователь {UserSession.Username} добавил читателя: {newStudent.FirstName} {newStudent.LastName}, ID: {newStudent.StudentID}, Дата рождения: {newStudent.DateOfBirth.ToShortDateString()}, Класс: {newStudent.StudentClass}, Префикс: {newStudent.Prefix}, Адрес: {newStudent.Address}.");

                DialogResult = true; // Закрываем диалоговое окно с результатом DialogResult = true
            }
            catch (Exception ex)
            {
                DateTime currentDateTime = DateTime.Now;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error(ex, $"[{currentDateTime:yyyy-MM-dd HH:mm:ss}] Ошибка при добавлении читателя. Пользователь {UserSession.Username}");
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалоговое окно с результатом DialogResult = false
            MessageBox.Show("Отмена действия");
            DialogResult = false;
        }       
    }
}