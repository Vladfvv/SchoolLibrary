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

namespace SchoolLibrary.DialogWindows.LoanWindows
{
    /// <summary>
    /// Логика взаимодействия для LoanDialogSelectStudent.xaml
    /// </summary>
    public partial class LoanDialogSelectStudent : Window
    {
        private EntityContext _context;
        public PaginatedStudentModel SelectedStudent { get; private set; }

        public LoanDialogSelectStudent(EntityContext context)
        {
            InitializeComponent();
            _context = context;
            LoadStudents();
        }

        private void LoadStudents()
        {
            dGrid.ItemsSource = _context.Students.ToList(); // Загружаем всех студентов
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is PaginatedStudentModel student)
            {
                SelectedStudent = student;
                DialogResult = true; // Устанавливаем результат диалога
                Close();
            }
            else
            {
                MessageBox.Show("Выберите студента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

}
