using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace SchoolLibrary.DialogWindows.LoanWindows
{
    public partial class ReturnBookDialog : Window
    {
        private readonly EntityContext _context;

        public ReturnBookDialog(EntityContext context)
        {
            InitializeComponent();
            _context = context;
            LoadLoans();
        }

        private void LoadLoans()
        {
            var loans = _context.Loans
                .Where(l => !l.Returned)
                .Include(l => l.InventoryBook)
                .Include(l => l.Student)
                .ToList();
            LoansDataGrid.ItemsSource = loans;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбора займа
            if (LoansDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a loan.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка выбора даты
            if (ReturnDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select a return date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка корректности введенной даты
            DateTime selectedDate;
            if (!DateTime.TryParse(ReturnDatePicker.Text, out selectedDate))
            {
                MessageBox.Show("Please enter a valid date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedLoan = (Loan)LoansDataGrid.SelectedItem;
            selectedLoan.ReturnDate = ReturnDatePicker.SelectedDate;
            selectedLoan.Returned = true;

            var inventoryBook = selectedLoan.InventoryBook;
            inventoryBook.Book.QuantityLeft += 1;
           // inventoryBook.Book.Quantity += 1;

            _context.Entry(selectedLoan).State = EntityState.Modified;
            _context.Entry(inventoryBook).State = EntityState.Modified;
            _context.SaveChanges();

            MessageBox.Show("Book returned successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshLoansData();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Implement search logic here
        }

        private void RefreshLoansData()
        {
            // Refresh data grid logic after changes
            LoadLoans();
        }
    }
}