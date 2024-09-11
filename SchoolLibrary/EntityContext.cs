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

        public EntityContext(string v) : base("SchoolLibrary")
        {
            //В конструкторе класса контекста указываем необходимость
            //использования созданного инициализатора БД:
            Database.SetInitializer(new DataBaseInitializer());

        }      
        public DbSet<Student> Students { get; set; }
        public DbSet<Category> Categories { get; set; }        
        public DbSet<InventoryBook> InventoryBooks { get; set; }
        public DbSet<BookPhoto> BookPhotos { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<InventoryBookStorage> InventoryBooksStorage { get; set; }
        public DbSet<LoanStorage> LoansStorage { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //создаем связи через билдер
            modelBuilder.Entity<Subject>()
           .HasRequired(s => s.Genre)
           .WithMany(g => g.Subjects)
           .HasForeignKey(s => s.GenreID)
           .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Book>()
                .HasRequired(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreID);            
        }
    }
}

