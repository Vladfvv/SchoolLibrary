using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    //модель данных Category, представляющая таблицу Category в базе данных
    public class Category : INotifyPropertyChanged
    {
        private int categoryID;
        private string categoryName;  
        public int CategoryID
        {
            get => categoryID;
            set
            {
                categoryID = value;
                OnPropertyChanged();
            }
        }
        
        public string CategoryName
        {
            get => categoryName;
            set
            {
                categoryName = value;
                OnPropertyChanged();
            }
        }


        // свойство для связи с книгами
        public virtual ICollection<Book> Books { get; set; }        

        // Конструктор по умолчанию
        public Category() {
            Books = new HashSet<Book>();
        }       
        public Category(string categoryName) : this()
        {
            CategoryName = categoryName;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }       
    }
}
