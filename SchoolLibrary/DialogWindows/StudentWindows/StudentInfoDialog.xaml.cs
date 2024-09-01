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
    /// Логика взаимодействия для StudentInfoDialog.xaml
    /// </summary>
    public partial class StudentInfoDialog : Window
    {
        public StudentInfoDialog(Student student)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            StudentInfo(student);
        }

        private void StudentInfo(Student student)
        {
            FirstNameTextBlock.Text = student.FirstName;
            LastNameTextBlock.Text = student.LastName;
            DateOfBirthTextBlock.Text = student.DateOfBirth.ToShortDateString();
            StudentClassTextBlock.Text = student.StudentClass.ToString() + student.Prefix.ToString();
            StudentAddressTextBlock.Text = student.Address.ToString();
            PhoneTextBlock.Text = student.Phone;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалоговое окно с результатом DialogResult = false            
            DialogResult = false;
        }
    }
}
