using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.DataLoaders
{
    public class StudentsDataLoader : IDataLoader
    {
        private readonly EntityContext _context;

        public StudentsDataLoader(EntityContext context)
        {
            _context = context;
        }

        public IEnumerable<Object> LoadData()
        {
            return _context.Students.ToList();
        }

       
    }
}
