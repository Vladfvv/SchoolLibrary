﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SchoolLibrary.Models
{
    //модель данных Book, представляющая таблицу Book в базе данных
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public string ISBN { get; set; }
        public int CategoryID { get; set; }
<<<<<<< HEAD
       
=======
        public Category Category { get; set; }
>>>>>>> e7df47228f9744bf27b7531522afb52de91a54d8
        public int Quantity { get; set; }
        // Навигационное свойство для связи с категорией
        public virtual Category Category { get; set; }




    }
}
