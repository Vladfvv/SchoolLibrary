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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//namespace SchoolLibrary.DialogWindows.BookWindows
//{
//    /// <summary>
//    /// Логика взаимодействия для DisposalDetailsDialog.xaml
//    /// </summary>
//    public partial class DisposalDetailsDialog : Window
//    {
//        public DateTime DateOfDisposal { get; private set; }
//        public string OutgoingInvoice { get; private set; }
//        public string ReasonForDisposal { get; private set; }

//        public DisposalDetailsDialog()
//        {
//            InitializeComponent();
//            // Центрирование окна на экране
//            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
//            DateOfDisposalPicker.Language = XmlLanguage.GetLanguage("ru-RU");
//        }

//        private void OkButton_Click(object sender, RoutedEventArgs e)
//        {
//            if (DateOfDisposalPicker.SelectedDate.HasValue)
//            {
//                DateOfDisposal = DateOfDisposalPicker.SelectedDate.Value;
//                OutgoingInvoice = OutgoingInvoiceTextBox.Text;
//                ReasonForDisposal = ReasonForDisposalTextBox.Text;
//                DialogResult = true;
//                Close();
//            }
//            else
//            {
//                MessageBox.Show("Введите корректную дату выбытия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private void CancelButton_Click(object sender, RoutedEventArgs e)
//        {
//            DialogResult = false;
//            Close();
//        }
//    }
//}

namespace SchoolLibrary.DialogWindows.BookWindows
{
    /// <summary>
    /// Логика взаимодействия для DisposalDetailsDialog.xaml
    /// </summary>
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

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        this.DragMove();
        //    }
        //}
    }
}

