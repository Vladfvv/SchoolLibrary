using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{// интерфейс IDeletable, который определяет метод Delete(EntityContext context) для удаления элемента из базы данных через контекст EntityContext
    public interface IDeletable
    {
        void Delete(EntityContext context);
    }
}
