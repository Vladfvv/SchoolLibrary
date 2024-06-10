using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    //модель данных Category, представляющая таблицу Category в базе данных
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
