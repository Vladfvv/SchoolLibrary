using NLog;
using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
        //Student studentInDb = null; // Объявляем переменную вне блока try
        private static readonly Logger logger = LogManager.GetCurrentClassLogger(); // Создание логгера

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
            Student studentInDb = null;
            try
            {
                // Извлекаем данные студента из PaginatedStudentModel
                string firstName = student.FirstName;
                string lastName = student.LastName;
                DateTime dateOfBirth = student.DateOfBirth;
                string studentClass = student.StudentClass;

                // Находим студента в контексте базы данных по имени, фамилии, возрасту и классу
                studentInDb = context.Students
                    .FirstOrDefault(s =>
                        s.FirstName == firstName &&
                        s.LastName == lastName &&
                        s.DateOfBirth == dateOfBirth &&
                        s.StudentClass == studentClass);

                //        if (studentInDb != null)
                //        {
                //            // Переводим студента в неактивное состояние
                //            studentInDb.IsActive = false;
                //            context.SaveChanges();

                //            // Закрываем диалоговое окно с результатом DialogResult = true
                //            DialogResult = true;
                //        }
                //        else
                //        {
                //            MessageBox.Show("Студент не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show($"Ошибка удаления читателя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //    }
                //}
                if (studentInDb != null)
                {
                    // Проверяем наличие книг у студента
                    var loans = context.Loans
                        .Where(l => l.StudentID == studentInDb.StudentID && !l.Returned)
                        .ToList();

                    if (loans.Count > 0)
                    {
                        MessageBox.Show("Невозможно удалить студента, так как у него на руках есть книги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                        // Логируем неудачную попытку удаления
                        LogStudentDeletion(studentInDb, false, "Читатель не может быть удален, так как на руках есть книги.");
                    }
                    else
                    {
                        // Логируем успешную попытку удаления
                        LogStudentDeletion(studentInDb, true, "Читатель успешно удален.");

                        // Удаление студента
                        studentInDb.IsActive = false;
                        context.SaveChanges();

                        MessageBox.Show("Читатель успешно удален.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;
                    }
                }
                else
                {
                    MessageBox.Show("Студент не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления читателя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                // Логируем ошибку удаления
                LogStudentDeletion(studentInDb, false, $"Ошибка: {ex.Message}");
            }
        }

        // Метод логирования удаления студента
        private void LogStudentDeletion(Student student, bool success, string message)
        {
            if (student != null)
            {
                logger.Info($"Пользователь {UserSession.Username}, дата операции: {DateTime.Now}, попытка удаления студента: {student.FirstName} {student.LastName}, ID: {student.StudentID}, результат: {(success ? "успешно" : "неудачно")}. Сообщение: {message}");
            }
            else
            {
                logger.Info($"Пользователь {UserSession.Username}, дата операции: {DateTime.Now}, результат удаления: неудачно. Сообщение: {message}");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалоговое окно с результатом DialogResult = false
            DialogResult = false;
        }      
    }
}
