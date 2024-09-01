﻿using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SchoolLibrary.DialogWindows.LoanWindows
{
    public partial class ReturnBookDialog : Window
    {       
        private readonly EntityContext _context;
        private const int PageSize = 15; // Количество элементов на странице
        private int CurrentPage = 1; // Текущая страница

        public ReturnBookDialog(EntityContext context)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _context = context;
            LoadLoans();
            ReturnDatePicker.Language = XmlLanguage.GetLanguage("ru-RU");
            ReturnDatePicker.FirstDayOfWeek = DayOfWeek.Monday;

            // Ограничение диапазона выбора даты
            ReturnDatePicker.DisplayDateEnd = DateTime.Now; // Максимальная допустимая дата - сегодня
            ReturnDatePicker.DisplayDateStart = DateTime.Now.AddYears(-1); // Минимальная допустимая дата - год назад
        }

        private void LoansDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var loan = e.Row.DataContext as Loan;

            if (loan != null && loan.DueDate < DateTime.Now && !loan.Returned)
            {
                e.Row.Background = new SolidColorBrush(Colors.LightCoral); // Подсветка просроченных книг
            }
            else if (loan != null && loan.Returned)
            {
                e.Row.Background = new SolidColorBrush(Colors.LightGreen); // Подсветка возвращенных книг
            }
            else
            {
                e.Row.Background = new SolidColorBrush(Colors.Transparent); // Убираем подсветку для остальных строк
            }
        }

        private void AdjustWindowHeight()
        {
            double rowHeight = 30; // Задайте нужное значение
            int rowCount = LoansDataGrid.Items.Count;

            double requiredHeight = (rowHeight * rowCount) + 150; // 150 - дополнительное пространство для заголовков, кнопок и т.д.
            this.MinHeight = 400; // Пример минимальной высоты
            this.Height = requiredHeight;
        }

        private void RefreshLoansData()
        {
            LoadLoans();
            AdjustWindowHeight(); // Обновите высоту окна после загрузки данных
        }

        private void LoadLoans()
        {
            var totalLoans = _context.Loans.Count(l => !l.Returned);
            var totalPages = (totalLoans + PageSize - 1) / PageSize;

            var loans = _context.Loans
                .Where(l => !l.Returned)
                .Include(l => l.InventoryBook)
                .Include(l => l.Student)
                .OrderBy(l => l.LoanDate)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            LoansDataGrid.ItemsSource = loans;

            AdjustWindowHeight();
            UpdateNavigationButtons(totalPages);
        }

        private void UpdateNavigationButtons(int totalPages)
        {
            if (totalPages <= 1)
            {
                PreviousButton.Visibility = Visibility.Collapsed;
                NextButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                PreviousButton.Visibility = Visibility.Visible;
                NextButton.Visibility = Visibility.Visible;

                PreviousButton.IsEnabled = CurrentPage > 1;
                NextButton.IsEnabled = CurrentPage < totalPages;
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoansDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите выдачу книги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ReturnDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime selectedDate = ReturnDatePicker.SelectedDate.Value;

            if (selectedDate > DateTime.Now)
            {
                MessageBox.Show("Невозможно оформить возврат, эта дата еще не наступила.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedLoan = (Loan)LoansDataGrid.SelectedItem;
            selectedLoan.ReturnDate = selectedDate;
            selectedLoan.Returned = true;

            var inventoryBook = selectedLoan.InventoryBook;
            inventoryBook.Book.QuantityLeft += 1;

            _context.Entry(selectedLoan).State = EntityState.Modified;
            _context.Entry(inventoryBook).State = EntityState.Modified;
            _context.SaveChanges();

            MessageBox.Show("Книга возвращена успешно.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshLoansData();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
               

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadLoans();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var totalLoans = _context.Loans.Count(l => !l.Returned);
            if (CurrentPage < (totalLoans + PageSize - 1) / PageSize)
            {
                CurrentPage++;
                LoadLoans();
            }
        }
    }
}