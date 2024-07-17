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

namespace SchoolLibrary.DialogWindows.StudentWindows
{
    /// <summary>
    /// Логика взаимодействия для DeleteStudentDialog.xaml
    /// </summary>
    public partial class DeleteStudentDialog : Window
    {
        private readonly EntityContext context;
        private readonly Student student;

        public DeleteStudentDialog(EntityContext ec, Student st)
        {
            InitializeComponent();
            context = ec;
            student = st;

            // Заполняем информацию о студенте
            txtStudentInfo.Text = $"Имя: {student.FirstName}\n Фамилия: {student.LastName}\n Age: {student.Age}\n Class: {student.Class}";
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Удаляем студента из контекста базы данных
                context.Students.Remove(student);
                context.SaveChanges();

                // Закрываем диалоговое окно с результатом DialogResult = true
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалоговое окно с результатом DialogResult = false
            DialogResult = false;
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
