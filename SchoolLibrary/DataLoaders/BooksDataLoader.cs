using SchoolLibrary.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.DataLoaders
{
    public class BooksDataLoader : IDataLoader
    {
        private readonly EntityContext _context;

        public BooksDataLoader(EntityContext context)
        {
            _context = context;
        }

        public IEnumerable<object> LoadData()
        {
            // Реализация метода InitBooksList
            return _context.Books
                .Include(b => b.Genre)
                .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                .ToList();
        }
    }


      

}
