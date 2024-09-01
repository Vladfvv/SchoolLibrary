using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.DataLoaders
{
    public class LibrarianDataLoader : DataLoader
    {
        public LibrarianDataLoader(EntityContext context) : base(context) { }

        public override void LoadData()
        {
            // Реализация для загрузки данных для библиотекаря
            dbcontext.Categories.Load();
            dbcontext.BookPhotos.Load();
            dbcontext.InventoryBooks.Load();
            dbcontext.Books.Load();
        }
    }
}
