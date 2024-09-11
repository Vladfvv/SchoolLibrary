using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SchoolLibrary.ViewModels;
using System.Windows;
using SchoolLibrary.Models;

namespace SchoolLibrary.Service
{
    public class BookService
    {
        private readonly EntityContext context;

        public BookService(EntityContext dbContext)
        {
            this.context = dbContext;
        }

        public List<PaginatedBookInventoryModel> GetBooksListForExport()
        {
            List<PaginatedBookInventoryModel> bookList = new List<PaginatedBookInventoryModel>();

            try
            {
                // Загружаем книги и связанные данные в память
                var allBooks = context.Books
                    .Include(b => b.Genre)
                    .Include(b => b.Subject)
                    .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                    .ToList();

                // Группируем книги по ISBN и выполняем последующую обработку на стороне клиента
                var groupedBooks = allBooks
                    .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
                    .GroupBy(x => x.InventoryBook.ISBN)
                    .ToList() // Выполняем материализацию данных, чтобы обработка происходила на стороне клиента
                    .Select((g, index) => new PaginatedBookInventoryModel
                    {
                        Index = index + 1, // Индексация начинается с 1
                        BookID = g.FirstOrDefault().Book.BookID,
                        Title = g.FirstOrDefault().InventoryBook.Title,
                        Author = g.FirstOrDefault().InventoryBook.Author,
                        Publisher = g.FirstOrDefault().InventoryBook.Publisher,
                        YearPublished = g.FirstOrDefault().InventoryBook.YearPublished,
                        ISBN = g.Key,
                        Quantity = g.Count(),
                        QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                        GenreName = g.FirstOrDefault().Book.Genre != null ? g.FirstOrDefault().Book.Genre.GenreName : "Неизвестно",
                        SubjectName = g.FirstOrDefault().Book.Subject != null ? g.FirstOrDefault().Book.Subject.SubjectName : "Неизвестно"
                    })
                    .ToList();

                // Добавляем сгруппированные книги в результат
                bookList.AddRange(groupedBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return bookList; // Возвращаем список книг
        }

        public List<InventoryBook> GetBooksNotLoaned()
        {
            return context.InventoryBooks
                .Where(ib => !ib.Loans.Any()) // Фильтрация книг, которые не были взяты
                .ToList();
        }
        
        public List<PaginatedInventoryBookModel> GetPaginatedBooksNotLoaned(int currentPage, int pageSize)
        {
            // Получаем книги, которые не были взяты
            List<InventoryBook> booksNotLoaned = GetBooksNotLoaned();

            List<PaginatedInventoryBookModel> paginatedBooksNotLoaned = booksNotLoaned               
                .Select((book, index) => new PaginatedInventoryBookModel
                {
                    Index = index + 1,
                    InventoryBookID = book.InventoryBookID,
                    Title = book.Title,
                    Author = book.Author,
                    Publisher = book.Publisher,
                    YearPublished = book.YearPublished,
                    ISBN = book.ISBN,
                    GenreName = book.Book?.Genre?.GenreName ?? "Неизвестно",
                    SubjectName = book.Book?.Subject?.SubjectName ?? "Неизвестно"
                })
                .ToList();

            return paginatedBooksNotLoaned;
        }


    }
}
