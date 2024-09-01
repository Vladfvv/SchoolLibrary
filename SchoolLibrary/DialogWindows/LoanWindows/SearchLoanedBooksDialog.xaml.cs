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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolLibrary.DialogWindows.LoanWindows
{
    public partial class SearchLoanedBooksDialog : Window
    {
        private readonly EntityContext context;

        public SearchLoanedBooksDialog(EntityContext context)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dpLoanDateFrom.Language = XmlLanguage.GetLanguage("ru-RU");
            dpLoanDateFrom.FirstDayOfWeek = DayOfWeek.Monday;
            dpLoanDateTo.Language = XmlLanguage.GetLanguage("ru-RU");
            dpLoanDateTo.FirstDayOfWeek = DayOfWeek.Monday;
            dpReturnFromPicker.Language = XmlLanguage.GetLanguage("ru-RU");
            dpReturnFromPicker.FirstDayOfWeek = DayOfWeek.Monday;
            dpReturnToPicker.Language = XmlLanguage.GetLanguage("ru-RU");
            dpReturnToPicker.FirstDayOfWeek = DayOfWeek.Monday;
            this.context = context;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string studentName = txtStudentName.Text;
            string bookTitle = txtBookTitle.Text;
            string author = txtAuthor.Text;
            string publisher = txtPublisher.Text;
            string yearPublished = txtYearPublished.Text;
            DateTime? loanDateFrom = dpLoanDateFrom.SelectedDate;
            DateTime? loanDateTo = dpLoanDateTo.SelectedDate;
            DateTime? dueDateFrom = dpReturnFromPicker.SelectedDate;
            DateTime? dueDateTo = dpReturnToPicker.SelectedDate;

            var query = context.Loans.AsQueryable();

            if (!string.IsNullOrEmpty(studentName))
            {
                query = query.Where(l => l.Student.FirstName.Contains(studentName) || l.Student.LastName.Contains(studentName));
            }

            if (!string.IsNullOrEmpty(bookTitle))
            {
                query = query.Where(l => l.InventoryBook.Title.Contains(bookTitle));
            }

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(l => l.InventoryBook.Author.Contains(author));
            }

            if (!string.IsNullOrEmpty(publisher))
            {
                query = query.Where(l => l.InventoryBook.Publisher.Contains(publisher));
            }

            if (!string.IsNullOrEmpty(yearPublished) && int.TryParse(yearPublished, out int year))
            {
                query = query.Where(l => l.InventoryBook.YearPublished == year.ToString());
            }

            if (loanDateFrom.HasValue)
            {
                query = query.Where(l => l.LoanDate >= loanDateFrom.Value);
            }

            if (loanDateTo.HasValue)
            {
                query = query.Where(l => l.LoanDate <= loanDateTo.Value);
            }

            if (dueDateFrom.HasValue)
            {
                query = query.Where(l => l.DueDate >= dueDateFrom.Value);
            }

            if (dueDateTo.HasValue)
            {
                query = query.Where(l => l.DueDate <= dueDateTo.Value);
            }

            var results = query.Select(l => new LoanBookStudentViewModel
            {
                LoanID = l.LoanID,
                StudentName = l.Student.FirstName,
                StudentLastName = l.Student.LastName,
                BookTitle = l.InventoryBook.Title,
                Author = l.InventoryBook.Author,
                Publisher = l.InventoryBook.Publisher,
                YearPublished = l.InventoryBook.YearPublished,
                LoanDate = l.LoanDate,
                DueDate = l.DueDate,
                ReturnDate = l.ReturnDate,
                Returned = l.Returned
            }).ToList();

            Tag = results;
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
       
    }

}