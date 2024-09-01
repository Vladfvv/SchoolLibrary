using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.ViewModels
{//для отображения статистики по инвеентарному журналу книг
    public class InventoryBookViewModel
    {
        public int RowNumber { get; set; }
        public InventoryBook InventoryBook { get; set; }
    }
}
