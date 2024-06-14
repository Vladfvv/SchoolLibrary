using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary
{//2.Создайте класс контекста данных:
    public class EntityContext : DbContext
    {
        public EntityContext(string v) : base("SchoolLibrary")
        {
            //5.В конструкторе класса контекста укажите необходимость
            //использования созданного инициализатора БД:
            Database.SetInitializer(new DataBaseInitializer()); 

        }
        //  public DbSet<Notice> Notices { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}
