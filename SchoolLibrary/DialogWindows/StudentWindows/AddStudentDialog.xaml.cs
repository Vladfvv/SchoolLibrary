using SchoolLibrary.Models;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
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

        public AddStudentDialog(EntityContext dbContext)
        {
            InitializeComponent();
            context = dbContext;
            newStudent = new Student();
            DataContext = newStudent;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Force validation
                txtFirstName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                txtLastName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                txtAge.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                txtClass.GetBindingExpression(TextBox.TextProperty).UpdateSource();


                //if (IsValid(newStudent))
                if (txtFirstName.Text != ""
                        && txtFirstName.Text.ToString() != ""
                        && int.Parse(txtAge.Text) > 5 && int.Parse(txtAge.Text) < 100
                        && int.Parse(txtClass.Text) > 1 && int.Parse(txtClass.Text) < 11)
                {
                    // Добавляем студента в контекст базы данных и сохраняем изменения
                    context.Students.Add(newStudent);
                    context.SaveChanges();
                    // Закрываем диалоговое окно с результатом DialogResult = true
                    DialogResult = true;
                }
                else throw new Exception("Пожалуйста, заполните все поля корректно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // MessageBox.Show($"Error adding student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
        }


        //private bool IsValid(object obj)
        //{
        //    return !Validation.GetHasError(txtFirstName) &&
        //           !Validation.GetHasError(txtLastName) &&
        //           !Validation.GetHasError(txtAge) &&
        //           !Validation.GetHasError(txtClass);
        //}

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