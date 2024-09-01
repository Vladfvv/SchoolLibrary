using SchoolLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;

namespace SchoolLibrary.Service
{
    public class InventoryBookService
    {
        private readonly EntityContext context;

        public InventoryBookService(EntityContext dbContext)
        {
            this.context = dbContext;
        }


        public List<PaginatedBookInventoryModel> GetInventoryBooksListForExport()
        {
            List<PaginatedBookInventoryModel> bookList = new List<PaginatedBookInventoryModel>();

            try
            {
                // Загружаем книги и связанные данные
                var allBooks = context.Books
                    .Include(b => b.Genre)
                    .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                    .ToList();

                // Создание списка инвентарных книг
                //bookList = allBooks
                //    .SelectMany(b => b.InventoryBooks, (b, ib) => new PaginatedBookInventoryModel
                //    {
                //        BookID = b.BookID,
                //        Title = ib.Title,
                //        Author = ib.Author,
                //        Publisher = ib.Publisher,
                //        YearPublished = ib.YearPublished,
                //        ISBN = ib.ISBN,
                //        Quantity = 1, // Количество по умолчанию 1, если не требуется агрегация
                //        QuantityLeft = 1 - ib.Loans.Count(loan => !loan.Returned), // Количество доступных книг
                //        GenreName = b.Genre != null ? b.Genre.GenreName : "Неизвестно",
                //        SubjectName = b.Genre != null ? b.Genre.GenreName : "Неизвестно"
                //    })
                //    .ToList();
                bookList = allBooks
            .SelectMany(b => b.InventoryBooks, (b, ib) => new PaginatedBookInventoryModel
            {
                // Индексы добавляются после создания списка
                BookID = b.BookID,
                Title = ib.Title,
                Author = ib.Author,
                Publisher = ib.Publisher,
                YearPublished = ib.YearPublished,
                ISBN = ib.ISBN,
                Quantity = 1, // Количество по умолчанию 1, если не требуется агрегация
                QuantityLeft = 1 - ib.Loans.Count(loan => !loan.Returned), // Количество доступных книг
                GenreName = b.Genre != null ? b.Genre.GenreName : "Неизвестно",
                SubjectName = b.Genre != null ? b.Subject.SubjectName : "Неизвестно"
            })
            .Select((model, index) =>
            {
                model.Index = index + 1; // Индекс с 1
                return model;
            })
            .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return bookList; // Возвращаем список книг
        }


        public List<PaginatedBookInventoryModel> InitInventoryBooksListForExport()
        {
            List<PaginatedBookInventoryModel> bookList = new List<PaginatedBookInventoryModel>();

            try
            {
                // Загружаем книги и связанные данные
                //context.Books
                //    .Include(b => b.Genre)
                //    .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                //    .Load();
                var allBooks = context.Books
            .Include(b => b.Genre)
            .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
            .ToList();

                // Группируем книги по ISBN
                var groupedBooks = context.Books
                    .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
                    .GroupBy(x => x.InventoryBook.ISBN)
                    .Select(g => new PaginatedBookInventoryModel // Или используйте другую подходящую модель
                    {
                        BookID = g.First().Book.BookID,
                        Title = g.First().InventoryBook.Title,
                        Author = g.First().InventoryBook.Author,
                        Publisher = g.First().InventoryBook.Publisher,
                        YearPublished = g.First().InventoryBook.YearPublished,
                        ISBN = g.Key,
                        Quantity = g.Count(),
                        QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                        GenreName = g.First().Book.Genre != null ? g.First().Book.Genre.GenreName : "Неизвестно",
                        SubjectName = g.First().Book.Genre != null ? g.First().Book.Genre.GenreName : "Неизвестно"
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
    }
}