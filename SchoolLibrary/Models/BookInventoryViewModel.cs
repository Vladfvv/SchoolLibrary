﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    // Новый класс, объединяющий поля из Book и InventoryBook.
    public class BookInventoryViewModel
    {       
        public int BookID { get; set; }
        public int InventoryBookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public int QuantityLeft { get; set; }
        public string CategoryName { get; set; }
    }
}
