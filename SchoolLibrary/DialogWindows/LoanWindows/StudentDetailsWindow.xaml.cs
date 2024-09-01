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

namespace SchoolLibrary.DialogWindows.LoanWindows
{
    /// <summary>
    /// Логика взаимодействия для StudentDetailsWindow.xaml
    /// </summary>
    public partial class StudentDetailsWindow : Window
    {
        public StudentDetailsWindow(Student student)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = student; // Привязка данных к окну
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь можно добавить логику для выбора студента и закрытия окна
            DialogResult = true; // Устанавливаем DialogResult в true
            Close(); // Закрываем окно
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Просто закрываем окно при нажатии кнопки "Отмена"
            DialogResult = false; // Устанавливаем DialogResult в false
            Close(); // Закрываем окно
        }
    }
}
