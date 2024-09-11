using System;
using System.Windows;
using System.Windows.Markup;

namespace SchoolLibrary.DialogWindows.BookWindows
{   
    public partial class DisposalDetailsDialog : Window
    {
        public DateTime DateOfDisposal { get; private set; }
        public string OutgoingInvoice { get; private set; }
        public string ReasonForDisposal { get; private set; }

        public DisposalDetailsDialog()
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Установка языка
            DateOfDisposalPicker.Language = XmlLanguage.GetLanguage("ru-RU");
            DateOfDisposalPicker.FirstDayOfWeek = DayOfWeek.Monday;
            // Установка фокуса на первое поле ввода
            OutgoingInvoiceTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что выбрана дата
            if (DateOfDisposalPicker.SelectedDate.HasValue)
            {
                DateOfDisposal = DateOfDisposalPicker.SelectedDate.Value;
                OutgoingInvoice = OutgoingInvoiceTextBox.Text;
                ReasonForDisposal = ReasonForDisposalTextBox.Text;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Введите корректную дату выбытия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }     
    }
}

