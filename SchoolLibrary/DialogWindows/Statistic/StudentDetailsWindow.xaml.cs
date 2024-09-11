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

namespace SchoolLibrary.DialogWindows.Statistic
{
    /// <summary>
    /// Логика взаимодействия для StudentDetailsWindow.xaml
    /// </summary>
    public partial class StudentDetailsWindow : Window
    {
        public Student SelectedStudent { get; }
        private readonly EntityContext _context;

        public StudentDetailsWindow(Student selectedStudent, EntityContext context)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SelectedStudent = selectedStudent;
            _context = context;            
            LoadLoans();
            var viewModel = new StudentDetailsViewModel
            {
                Student = selectedStudent,
                Loans = context.Loans.Where(l => l.Student.StudentID == selectedStudent.StudentID).ToList()
            };
            DataContext = viewModel;
        }

        private void LoadLoans()
        {
            var loans = _context.Loans
                .Where(l => l.StudentID == SelectedStudent.StudentID)
                .Select(l => new
                {
                    l.InventoryBook.Title,
                    l.InventoryBook.Author,
                    l.LoanDate,
                    l.ReturnDate
                })
                .ToList();

            LoansDataGrid.ItemsSource = loans;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                //Создание визуального контента для вывода на печать
                var visual = new DrawingVisual();
                using (var context = visual.RenderOpen())
                {
                    var size = new Size(this.ActualWidth, this.ActualHeight);
                    context.DrawRectangle(new VisualBrush(this), null, new Rect(new Point(0, 0), size));
                }

                // рассчет части для вывода
                var printArea = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                var visualBrush = new VisualBrush(this);              
                using (var context = visual.RenderOpen())
                {
                    context.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), printArea));
                }

                // Печать
                printDialog.PrintVisual(visual, "Ведомость по читателю");
            }
        }

    }
}
