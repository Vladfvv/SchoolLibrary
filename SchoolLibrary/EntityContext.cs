using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary
{//2.Создайте класс контекста данных:
    public class EntityContext : DbContext
    {
        /*  public class AvailableBook
          {
              public int BookID { get; set; }
              public string Title { get; set; }
              public string Author { get; set; }
              public string Publisher { get; set; }
              public string YearPublished { get; set; }
              public string ISBN { get; set; }
              public int CategoryID { get; set; }
              public virtual Category Category { get; set; }
              public int Quantity { get; set; } // Количество доступных книг
          }

          */






        public EntityContext(string v) : base("SchoolLibrary")
        {
            //5.В конструкторе класса контекста укажите необходимость
            //использования созданного инициализатора БД:
            Database.SetInitializer(new DataBaseInitializer());

        }
        //  public DbSet<Notice> Notices { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryBook> InventoryBooks { get; set; }
        public DbSet<BookPhoto> BookPhotos  { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }     
        public DbSet<InventoryBookStorage> InventoryBooksStorage { get; set; }
        public DbSet<LoanStorage> LoansStorage { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Loan>()
                .HasRequired(l => l.InventoryBook)
                .WithMany(ib => ib.Loans)
                .HasForeignKey(l => l.InventoryBookID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InventoryBook>()
                .HasRequired(ib => ib.Book)
                .WithMany(b => b.InventoryBooks)
                .HasForeignKey(ib => ib.BookID);
        }




        //   public DbSet<AvailableBook> AvailableBooks { get; set; } // Добавленное свойство для доступа к таблице AvailableBooks

        // Метод для начальной инициализации данных
        /*  public void InitializeAvailableBooks()
          {
              using (var context = new EntityContext("SchoolLibrary"))
              {
                  // Очищаем таблицу AvailableBooks перед начальной загрузкой
                  context.Database.ExecuteSqlCommand("DELETE FROM AvailableBooks");

                  // Заполняем список AvailableBooks на основе данных из Books и Loans
                  var books = context.Books.ToList();

                  foreach (var book in books)
                  {
                      // Вычисляем количество доступных книг как общее количество минус количество занятых займами
                      int availableQuantity = book.Quantity - book.Loans.Count;

                      // Создаем объект AvailableBook
                      var availableBook = new AvailableBook
                      {
                          BookID = book.BookID,
                          Title = book.Title,
                          Author = book.Author,
                          Publisher = book.Publisher,
                          YearPublished = book.YearPublished,
                          ISBN = book.ISBN,
                          CategoryID = book.CategoryID,
                          Category = book.Category,
                          Quantity = availableQuantity
                      };

                      // Добавляем объект AvailableBook в контекст
                      context.AvailableBooks.Add(availableBook);
                  }

                  // Сохраняем изменения в базе данных
                  context.SaveChanges();
              }

              */



        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Добавьте конфигурации сущностей, если необходимо
        }

        public void CreateAvailableBooksList()
        {
            var availableBooks = new List<AvailableBook>();

            using (var context = new EntityContext("SchoolLibrary"))
            {
                // Запрос книг из Books, за вычетом количества книг в займах (Loans)
                var query = from book in context.Books
                            select new AvailableBook
                            {
                                BookID = book.BookID,
                                Title = book.Title,
                                Author = book.Author,
                                Publisher = book.Publisher,
                                YearPublished = book.YearPublished,
                                ISBN = book.ISBN,
                                CategoryID = book.CategoryID,
                                Category = book.Category,
                                Quantity = book.Quantity - book.Loans.Count() // Вычитаем количество книг в займах
                            };

                availableBooks.AddRange(query.ToList());

                // Добавляем полученные доступные книги в контекст AvailableBooks
                context.AvailableBooks.AddRange(availableBooks);
                context.SaveChanges();
            }
        }

        */

        /*  public List<Book> GetAvailableBooks()
          {
              var availableBooks = new List<Book>();

              using (var connection = new SqlConnection(this.Database.Connection.ConnectionString))
              {
                  connection.Open();
                  using (var command = new SqlCommand("EXEC dbo.CreateAvailableBooksTable", connection))
                  {
                      using (var reader = command.ExecuteReader())
                      {
                          if (!reader.HasRows)
                          {
                              Console.WriteLine("No rows found.");
                          }
                          else
                          {
                              while (reader.Read())
                              {
                                  var book = new Book
                                  {
                                      BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                                      Title = reader.GetString(reader.GetOrdinal("Title")),
                                      Author = reader.GetString(reader.GetOrdinal("Author")),
                                      Publisher = reader.GetString(reader.GetOrdinal("Publisher")),
                                      YearPublished = reader.GetString(reader.GetOrdinal("YearPublished")),
                                      ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                                      CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                                      Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                  };

                                  availableBooks.Add(book);
                              }
                          }
                      }
                  }
              }

              return availableBooks;
          }*/


        /*  public List<Book> GetAvailableBooks()
          {
              var availableBooks = Books
                  .Where(b => b.Quantity > 0 && !b.Loans.Any()) // Выбираем книги с Quantity > 0 и без активных займов
                  .Include(b => b.Category) // Включаем связанную категорию, если нужно
                  .ToList();

              return availableBooks;

          }*/
    }
}

