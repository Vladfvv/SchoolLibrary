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
    public class Category : INotifyPropertyChanged, IDeletable
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
        //public virtual ICollection<AvailableBook> AvailableBooks { get; set; }

        // Конструктор по умолчанию
        public Category() {
            Books = new HashSet<Book>();
        }
        // Конструктор
        public Category(string categoryName) : this()
        {
            CategoryName = categoryName;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Delete(EntityContext context)
        {
            if (context.Books.Any(b => b.CategoryID == this.CategoryID))
            {
                throw new InvalidOperationException("Невозможно удалить категорию, так как она связана с существующими книгами.");
            }
            context.Categories.Remove(this);
            context.SaveChanges();
        }
    }
}
