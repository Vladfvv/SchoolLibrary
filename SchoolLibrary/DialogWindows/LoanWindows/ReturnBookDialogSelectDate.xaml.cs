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
    /// Логика взаимодействия для ReturnBookDialogSelectDate.xaml
    /// </summary>
    public partial class ReturnBookDialogSelectDate : Window
    {
        private Loan _loan;
        public DateTime ReturnDate { get; private set; }

        public ReturnBookDialogSelectDate(EntityContext context, Loan loan)
        {
            InitializeComponent();
            _loan = loan;
            ReturnDatePicker.SelectedDate = DateTime.Now; // Устанавливаем текущую дату как дату возврата по умолчанию
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReturnDatePicker.SelectedDate.HasValue)
            {
                ReturnDate = ReturnDatePicker.SelectedDate.Value;
                DialogResult = true; // Устанавливаем результат диалога
                Close();
            }
            else
            {
                MessageBox.Show("Выберите дату возврата.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

}
