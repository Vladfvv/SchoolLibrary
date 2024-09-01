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

namespace SchoolLibrary.DialogWindows.StudentWindows
{
    /// <summary>
    /// Логика взаимодействия для DeleteStudentDialog.xaml
    /// </summary>
    public partial class DeleteStudentDialog : Window
    {
        private readonly EntityContext context;
        private readonly PaginatedStudentModel student;

        public DeleteStudentDialog(EntityContext ec, PaginatedStudentModel st)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            context = ec;
            student = st;

            // Заполняем информацию о студенте
            txtStudentInfo.Text = $"Имя: {student.FirstName}\n Фамилия: {student.LastName}\n Возраст: {student.Age}\n Класс: {student.StudentClass + student.Prefix}";
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Извлекаем данные студента из PaginatedStudentModel
                string firstName = student.FirstName;
                string lastName = student.LastName;
                DateTime dateOfBirth = student.DateOfBirth;
                string studentClass = student.StudentClass;

                // Находим студента в контексте базы данных по имени, фамилии, возрасту и классу
                var studentInDb = context.Students
                    .FirstOrDefault(s =>
                        s.FirstName == firstName &&
                        s.LastName == lastName &&
                        s.DateOfBirth == dateOfBirth &&
                        s.StudentClass == studentClass);

                if (studentInDb != null)
                {
                    // Переводим студента в неактивное состояние
                    studentInDb.IsActive = false;
                    context.SaveChanges();

                    // Закрываем диалоговое окно с результатом DialogResult = true
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Студент не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления читателя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалоговое окно с результатом DialogResult = false
            DialogResult = false;
        }
      
    }
}
