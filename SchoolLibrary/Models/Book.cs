using SchoolLibrary.Models;
using System.Collections.Generic;
using System.Linq;

public class Book
{
    public int BookID { get; set; }
    public int Class { get; set; }
    public string Description { get; set; }
    public int GenreID { get; set; }  // Ссылка на жанр
    public virtual Genre Genre { get; set; }  // Связь с жанром    
    public int SubjectID { get; set; }  // Внешний ключ к Subject
    public virtual Subject Subject { get; set; }  // Связь с Subject
    public int Quantity { get; set; }
    public int QuantityLeft { get; set; }    
    public virtual ICollection<InventoryBook> InventoryBooks { get; set; }
    public virtual ICollection<BookPhoto> BookPhotos { get; set; }

    public Book()
    {
        InventoryBooks = new HashSet<InventoryBook>();
        BookPhotos = new HashSet<BookPhoto>();
    }
   
    public void UpdateQuantities()
    {
        Quantity = InventoryBooks.Count;
        QuantityLeft = InventoryBooks.Count(ib => !ib.Loans.Any(loan => !loan.Returned));
    }
    public void AddInventoryBook(InventoryBook inventoryBook)
    {
        InventoryBooks.Add(inventoryBook);
        UpdateQuantities();
    }
    public void RemoveInventoryBook(InventoryBook inventoryBook)
    {
        InventoryBooks.Remove(inventoryBook);
        UpdateQuantities();
    }
}