using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.DataLoaders
{
    public class AdminDataLoader : DataLoader
    {
        public AdminDataLoader(EntityContext context) : base(context) { }

        public override void LoadData()
        {
            // Реализация для загрузки данных для администратора
            dbcontext.Categories.Load();
            dbcontext.BookPhotos.Load();
            dbcontext.InventoryBooks.Load();
            dbcontext.Books.Load();
        }
    }
}
