using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data.Entity;
using SchoolLibrary.ViewModels;

namespace SchoolLibrary.Service
{
    public class LoansService
    {
        private readonly EntityContext context;

        public LoansService(EntityContext dbContext)
        {
            this.context = dbContext;
        }


        public List<PaginatedLoanBookStudentViewModel> GetLoansListForExport()
        {
            List<PaginatedLoanBookStudentViewModel> loansListForExport = new List<PaginatedLoanBookStudentViewModel>();

            try
            {
                // Загружаем данные о задолженностях книг и связанных сущностях в память
                context.Loans.Include(l => l.InventoryBook).Include(l => l.Student).Load();

                // Преобразуем данные в список с добавлением индексации
                var loansList = context.Loans.Local
                    .Select((loan, index) => new PaginatedLoanBookStudentViewModel
                    {
                        Index = index + 1, // Индекс начинается с 1
                        LoanID = loan.LoanID,
                        StudentName = loan.Student.FirstName,
                        StudentLastName = loan.Student.LastName,
                        BookTitle = loan.InventoryBook.Title,
                        Author = loan.InventoryBook.Author,
                        Publisher = loan.InventoryBook.Publisher,
                        YearPublished = loan.InventoryBook.YearPublished.ToString(),
                        LoanDate = loan.LoanDate,
                        DueDate = loan.DueDate,
                        ReturnDate = loan.ReturnDate,
                        Returned = loan.Returned
                    })
                    .ToList();

                // Добавляем полученный список в результат
                loansListForExport.AddRange(loansList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return loansListForExport; // Возвращаем список задолженностей книг
        }
              

        public List<LoanViewModel> GetNotReturnedBooks()
        {
            context.Loans.Include(l => l.InventoryBook).Include(l => l.Student).Load();

            var notReturnedBooksList = context.Loans.Local
                .Where(l => l.ReturnDate == null)
                .Select(l => new LoanViewModel
                {
                    Title = l.InventoryBook.Title,
                    StudentID = l.Student.StudentID,
                    FirstName = l.Student.FirstName,
                    LastName = l.Student.LastName,
                    LoanDate = l.LoanDate,
                    DueDate = l.DueDate,
                    ReturnDate = l.ReturnDate,
                    Returned = l.Returned
                }).ToList();

            return notReturnedBooksList;
        }


        public List<LoanViewModel> GetReturnedBooks()
        {
            context.Loans.Include(l => l.InventoryBook).Include(l => l.Student).Load();

            var returnedBooksList = context.Loans.Local
                .Where(l => l.ReturnDate != null)
                .Select(l => new LoanViewModel
                {
                    Title = l.InventoryBook.Title,
                    StudentID = l.Student.StudentID,    
                    FirstName = l.Student.FirstName,
                    LastName = l.Student.LastName,
                    LoanDate = l.LoanDate,
                    DueDate = l.DueDate,
                    ReturnDate = l.ReturnDate,
                    Returned = l.Returned
                }).ToList();

            return returnedBooksList;
        }
    }
}
