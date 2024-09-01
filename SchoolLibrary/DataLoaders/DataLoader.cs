
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.DataLoaders
{
    //базовый класс для общего функционала и наследников для специализированных реализаций
    public abstract class DataLoader
    {
        protected readonly EntityContext dbcontext;

        protected DataLoader(EntityContext context)
        {
            this.dbcontext = context;
        }

        public abstract void LoadData();
    }
}
