using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SchoolLibrary.DataLoaders
{
    public class InventoryBooksDataLoader : IDataLoader
    {
        private readonly EntityContext _context;

        public InventoryBooksDataLoader(EntityContext context)
        {
            _context = context;
        }

        public IEnumerable<object> LoadData()
        {
            // Загружаем данные из InventoryBooks и связанные данные из Book
            return _context.InventoryBooks.Include(ib => ib.Book).ToList();
        }

        //public IEnumerable<InventoryBook> LoadData()
        //{
        //    // Загружаем InventoryBooks и связанные Book
        //    return _context.InventoryBooks
        //        .Include(ib => ib.Book) // Убедитесь, что Book существует в InventoryBook
        //        .ToList();
        //}

    }
}
