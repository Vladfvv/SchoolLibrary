//using SchoolLibrary.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Runtime.Remoting.Contexts;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace SchoolLibrary.DialogWindows
//{
//    public partial class SearchBooksDialog : Window
//    {
//        private readonly EntityContext _context;

//        public SearchBooksDialog(EntityContext context)
//        {
//            InitializeComponent();
//            _context = context;
//            LoadCategories();
//        }

//        private void LoadCategories()
//        {
//            var categories = _context.Categories.ToList();
//            CategoryComboBox.ItemsSource = categories;
//        }

//        ////  private void SearchButton_Click(object sender, RoutedEventArgs e)
//        /*{
//            string title = TitleTextBox.Text;
//            string author = AuthorTextBox.Text;
//            string publisher = PublisherTextBox.Text;
//            bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
//            bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
//            string isbn = ISBNTextBox.Text;
//            Category selectedCategory = CategoryComboBox.SelectedItem as Category;

//            // Use Include with the correct navigation properties
//            var query = _context.Books
//                .Include(b => b.InventoryBooks)
//                .Include(b => b.Category)
//                .AsQueryable();

//            if (!string.IsNullOrWhiteSpace(title))
//            {
//                query = query.Where(b => b.InventoryBooks.First().Title.Contains(title));
//            }

//            if (!string.IsNullOrWhiteSpace(author))
//            {
//                query = query.Where(b => b.InventoryBooks.First().Author.Contains(author));
//            }

//            if (!string.IsNullOrWhiteSpace(publisher))
//            {
//                query = query.Where(b => b.InventoryBooks.First().Publisher.Contains(publisher));
//            }

//            if (yearFromParsed)
//            {
//                query = query.Where(b => b.InventoryBooks.First().YearPublished.CompareTo(yearFrom.ToString()) >= 0);
//            }

//            if (yearToParsed)
//            {
//                query = query.Where(b => b.InventoryBooks.First().YearPublished.CompareTo(yearTo.ToString()) <= 0);
//            }

//            if (!string.IsNullOrWhiteSpace(isbn))
//            {
//                query = query.Where(b => b.InventoryBooks.First().ISBN.Contains(isbn));
//            }

//            if (selectedCategory != null)
//            {
//                query = query.Where(b => b.InventoryBooks.First().Book.CategoryID == selectedCategory.CategoryID);
//            }

//            // Group by ISBN and select the required fields
//            var results = query
//                .GroupBy(b => b.InventoryBooks.FirstOrDefault().ISBN)
//                .Select(g => new BookInventoryViewModel
//                {
//                    BookID = g.FirstOrDefault().BookID,
//                    Title = g.FirstOrDefault().InventoryBooks.FirstOrDefault().Title,
//                    Author = g.FirstOrDefault().InventoryBooks.FirstOrDefault().Author,
//                    Publisher = g.FirstOrDefault().InventoryBooks.FirstOrDefault().Publisher,
//                    YearPublished = g.FirstOrDefault().InventoryBooks.FirstOrDefault().YearPublished,
//                    ISBN = g.Key,
//                    Quantity = g.Sum(x => x.Quantity),
//                    QuantityLeft = g.Sum(x => x.Quantity),
//                    CategoryName = g.FirstOrDefault().Category.CategoryName
//                })
//                .ToList();

//            if (results.Any())
//            {
//                MessageBox.Show($"{results.Count} unique ISBNs found.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
//                this.DialogResult = true;
//                this.Tag = results;
//            }
//            else
//            {
//                MessageBox.Show("No books found matching the criteria.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
//                this.DialogResult = false;
//            }*/


//        //    string title = TitleTextBox.Text;
//        //    string author = AuthorTextBox.Text;
//        //    string publisher = PublisherTextBox.Text;
//        //    bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
//        //    bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
//        //    string isbn = ISBNTextBox.Text;
//        //    Category selectedCategory = CategoryComboBox.SelectedItem as Category;

//        //    // Use Include with the correct navigation properties
//        //    var query = _context.InventoryBooks
//        //        .Include(b => b.Book)
//        //        .Include(b => b.Book.Category)
//        //        .AsQueryable();

//        //    if (!string.IsNullOrWhiteSpace(title))
//        //    {
//        //        query = query.Where(b => b.Title.Contains(title));
//        //    }

//        //    if (!string.IsNullOrWhiteSpace(author))
//        //    {
//        //        query = query.Where(b => b.Author.Contains(author));
//        //    }

//        //    if (!string.IsNullOrWhiteSpace(publisher))
//        //    {
//        //        query = query.Where(b => b.Publisher.Contains(publisher));
//        //    }

//        //    if (yearFromParsed)
//        //    {
//        //        query = query.Where(b => b.YearPublished.CompareTo(yearFrom.ToString()) >= 0);
//        //    }

//        //    if (yearToParsed)
//        //    {
//        //        query = query.Where(b => b.YearPublished.CompareTo(yearTo.ToString()) <= 0);
//        //    }

//        //    if (!string.IsNullOrWhiteSpace(isbn))
//        //    {
//        //        query = query.Where(b => b.ISBN.Contains(isbn));
//        //    }

//        //    if (selectedCategory != null)
//        //    {
//        //        query = query.Where(b => b.Book.CategoryID == selectedCategory.CategoryID);
//        //    }

//        //    // Execute the query and load the results
//        //    var inventoryBooks = query.ToList();

//        //    // Group by ISBN and select the required fields
//        //    var results = inventoryBooks
//        //        .GroupBy(b => b.ISBN)
//        //        .Select(g => new BookInventoryViewModel
//        //        {
//        //            BookID = g.First().Book.BookID,
//        //            Title = g.First().Title,
//        //            Author = g.First().Author,
//        //            Publisher = g.First().Publisher,
//        //            YearPublished = g.First().YearPublished,
//        //            ISBN = g.Key,
//        //            Quantity = g.Sum(x => x.Book.Quantity),
//        //            QuantityLeft = g.Sum(x => x.Book.QuantityLeft),
//        //            CategoryName = g.First().Book.Category.CategoryName
//        //        })
//        //        .ToList();

//        //    if (results.Any())
//        //    {
//        //        MessageBox.Show($"{results.Count} unique ISBNs found.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
//        //        this.DialogResult = true;
//        //        this.Tag = results;
//        //    }
//        //    else
//        //    {
//        //        MessageBox.Show("No books found matching the criteria.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
//        //        this.DialogResult = false;
//        //    }

//        //}

//        //private void CancelButton_Click(object sender, RoutedEventArgs e)
//        //{
//        //    this.DialogResult = false;
//        //}

//        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
//        //{
//        //    if (e.ChangedButton == MouseButton.Left)
//        //    {
//        //        this.DragMove();
//        //    }
//        //}
//        //}
//        /* private void SearchButton_Click(object sender, RoutedEventArgs e)
//         {
//             string title = TitleTextBox.Text;
//             string author = AuthorTextBox.Text;
//             string publisher = PublisherTextBox.Text;
//             bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
//             bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
//             string isbn = ISBNTextBox.Text;
//             Category selectedCategory = CategoryComboBox.SelectedItem as Category;

//             var query = _context.InventoryBooks
//                 .Include(b => b.Book)
//                 .Include(b => b.Book.Category)
//                 .AsQueryable();

//             if (!string.IsNullOrWhiteSpace(title))
//             {
//                 query = query.Where(b => b.Title.Contains(title));
//             }

//             if (!string.IsNullOrWhiteSpace(author))
//             {
//                 query = query.Where(b => b.Author.Contains(author));
//             }

//             if (!string.IsNullOrWhiteSpace(publisher))
//             {
//                 query = query.Where(b => b.Publisher.Contains(publisher));
//             }

//             if (yearFromParsed)
//             {
//                 query = query.Where(b => b.YearPublished.CompareTo(yearFrom.ToString()) >= 0);
//             }

//             if (yearToParsed)
//             {
//                 query = query.Where(b => b.YearPublished.CompareTo(yearTo.ToString()) <= 0);
//             }

//             if (!string.IsNullOrWhiteSpace(isbn))
//             {
//                 query = query.Where(b => b.ISBN.Contains(isbn));
//             }

//             if (selectedCategory != null)
//             {
//                 query = query.Where(b => b.Book.CategoryID == selectedCategory.CategoryID);
//             }

//             var inventoryBooks = query.ToList();

//             var results = inventoryBooks
//                 .GroupBy(b => b.Book)
//                 .Select(g => g.Key)
//                 .ToList();

//             if (results.Any())
//             {
//                 MessageBox.Show($"{results.Count} книг найдено.", "Результаты поиска", MessageBoxButton.OK, MessageBoxImage.Information);
//                 this.DialogResult = true;
//                 this.Tag = results;
//             }
//             else
//             {
//                 MessageBox.Show("Книги не найдены.", "Результаты поиска", MessageBoxButton.OK, MessageBoxImage.Information);
//                 this.DialogResult = false;
//             }
//         }*/

//        /* private void SearchButton_Click(object sender, RoutedEventArgs e)
//         {
//             string title = TitleTextBox.Text;
//             string author = AuthorTextBox.Text;
//             string publisher = PublisherTextBox.Text;
//             bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
//             bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
//             string isbn = ISBNTextBox.Text;
//             Category selectedCategory = CategoryComboBox.SelectedItem as Category;

//             var query = _context.InventoryBooks
//                 .Include(ib => ib.Book)
//                 .Include(ib => ib.Book.Category)
//                 .AsQueryable();

//             if (!string.IsNullOrWhiteSpace(title))
//             {
//                 query = query.Where(ib => ib.Title.Contains(title));
//             }

//             if (!string.IsNullOrWhiteSpace(author))
//             {
//                 query = query.Where(ib => ib.Author.Contains(author));
//             }

//             if (!string.IsNullOrWhiteSpace(publisher))
//             {
//                 query = query.Where(ib => ib.Publisher.Contains(publisher));
//             }

//             if (yearFromParsed)
//             {
//                 query = query.Where(ib => ib.YearPublished.CompareTo(yearFrom.ToString()) >= 0);
//             }

//             if (yearToParsed)
//             {
//                 query = query.Where(ib => ib.YearPublished.CompareTo(yearTo.ToString()) <= 0);
//             }

//             if (!string.IsNullOrWhiteSpace(isbn))
//             {
//                 query = query.Where(ib => ib.ISBN.Contains(isbn));
//             }

//             if (selectedCategory != null)
//             {
//                 query = query.Where(ib => ib.Book.CategoryID == selectedCategory.CategoryID);
//             }

//             var inventoryBooks = query.ToList();

//             var results = inventoryBooks
//                 .Select(ib => new BookInventoryViewModel
//                 {
//                     BookID = ib.Book.BookID,
//                     InventoryBookID = ib.InventoryBookID,
//                     Title = ib.Title,
//                     Author = ib.Author,
//                     Publisher = ib.Publisher,
//                     YearPublished = ib.YearPublished,
//                     ISBN = ib.ISBN,
//                     Quantity = ib.Book.Quantity,
//                     QuantityLeft = ib.Book.QuantityLeft,
//                     CategoryName = ib.Book.Category.CategoryName
//                 })
//                 .ToList();

//             if (results.Any())
//             {
//                 MessageBox.Show($"{results.Count} unique ISBNs found.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
//                 this.DialogResult = true;
//                 this.Tag = results;
//             }
//             else
//             {
//                 MessageBox.Show("No books found matching the criteria.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
//                 this.DialogResult = false;
//             }
//         }*/

//        /* private void SearchButton_Click(object sender, RoutedEventArgs e)
//         {
//             string title = TitleTextBox.Text;
//             string author = AuthorTextBox.Text;
//             string publisher = PublisherTextBox.Text;
//             bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
//             bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
//             string isbn = ISBNTextBox.Text;
//             Category selectedCategory = CategoryComboBox.SelectedItem as Category;

//             var query = _context.InventoryBooks
//                 .Include(ib => ib.Book)
//                 .Include(ib => ib.Book.Category)
//                 .Include(ib => ib.Loans)
//                 .AsQueryable();

//             if (!string.IsNullOrWhiteSpace(title))
//             {
//                 query = query.Where(ib => ib.Title.Contains(title));
//             }

//             if (!string.IsNullOrWhiteSpace(author))
//             {
//                 query = query.Where(ib => ib.Author.Contains(author));
//             }

//             if (!string.IsNullOrWhiteSpace(publisher))
//             {
//                 query = query.Where(ib => ib.Publisher.Contains(publisher));
//             }

//             if (yearFromParsed)
//             {
//                 query = query.Where(ib => ib.YearPublished.CompareTo(yearFrom.ToString()) >= 0);
//             }

//             if (yearToParsed)
//             {
//                 query = query.Where(ib => ib.YearPublished.CompareTo(yearTo.ToString()) <= 0);
//             }

//             if (!string.IsNullOrWhiteSpace(isbn))
//             {
//                 query = query.Where(ib => ib.ISBN.Contains(isbn));
//             }

//             if (selectedCategory != null)
//             {
//                 query = query.Where(ib => ib.Book.CategoryID == selectedCategory.CategoryID);
//             }

//             var inventoryBooks = query.ToList();

//             var results = inventoryBooks
//                 .Select(ib => new SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel
//                 {
//                     BookID = ib.Book.BookID,
//                     InventoryBookID = ib.InventoryBookID,
//                     Title = ib.Title,
//                     Author = ib.Author,
//                     Publisher = ib.Publisher,
//                     YearPublished = ib.YearPublished,
//                     ISBN = ib.ISBN,
//                     Quantity = 1,
//                     QuantityLeft = ib.Loans.Any(loan => !loan.Returned) ? 0 : 1,
//                     CategoryName = ib.Book.Category.CategoryName
//                 })
//                 .ToList();

//             if (results.Any())
//             {
//                 string message;
//                 int count = results.Count;

//                 if (count == 1)
//                 {
//                     message = "1 книга найдена.";
//                 }
//                 else if (count > 1 && count < 5)
//                 {
//                     message = $"{count} книги найдены.";
//                 }
//                 else
//                 {
//                     message = $"{count} книг найдено.";
//                 }

//                 MessageBox.Show(message, "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
//                 this.DialogResult = true;
//                 this.Tag = results;
//             }
//             else
//             {
//                 MessageBox.Show("Нет книг по данным критериям поиска.", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
//                 this.DialogResult = false;
//             }
//         }*/
//        private void SearchButton_Click(object sender, RoutedEventArgs e)
//        {
//            string title = TitleTextBox.Text;
//            string author = AuthorTextBox.Text;
//            string publisher = PublisherTextBox.Text;
//            bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
//            bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
//            string isbn = ISBNTextBox.Text;
//            Category selectedCategory = CategoryComboBox.SelectedItem as Category;

//            var query = _context.InventoryBooks
//                .Include(ib => ib.Book)
//                .Include(ib => ib.Book.Category)
//                .Include(ib => ib.Loans)
//                .AsQueryable();

//            if (!string.IsNullOrWhiteSpace(title))
//            {
//                query = query.Where(ib => ib.Title.Contains(title));
//            }

//            if (!string.IsNullOrWhiteSpace(author))
//            {
//                query = query.Where(ib => ib.Author.Contains(author));
//            }

//            if (!string.IsNullOrWhiteSpace(publisher))
//            {
//                query = query.Where(ib => ib.Publisher.Contains(publisher));
//            }

//            if (yearFromParsed)
//            {
//                query = query.Where(ib => ib.YearPublished.CompareTo(yearFrom.ToString()) >= 0);
//            }

//            if (yearToParsed)
//            {
//                query = query.Where(ib => ib.YearPublished.CompareTo(yearTo.ToString()) <= 0);
//            }

//            if (!string.IsNullOrWhiteSpace(isbn))
//            {
//                query = query.Where(ib => ib.ISBN.Contains(isbn));
//            }

//            if (selectedCategory != null)
//            {
//                query = query.Where(ib => ib.Book.CategoryID == selectedCategory.CategoryID);
//            }

//            var inventoryBooks = query.ToList();

//            var results = inventoryBooks
//                .Select(ib => new SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel
//                {
//                    BookID = ib.Book.BookID,
//                    InventoryBookID = ib.InventoryBookID,
//                    Title = ib.Title,
//                    Author = ib.Author,
//                    Publisher = ib.Publisher,
//                    YearPublished = ib.YearPublished,
//                    ISBN = ib.ISBN,
//                    Quantity = 1,
//                    QuantityLeft = ib.Loans.Any(loan => !loan.Returned) ? 0 : 1,
//                    CategoryName = ib.Book.Category.CategoryName
//                })
//                .ToList();

//            if (GroupByIsbnRadioButton.IsChecked == true)
//            {
//                results = results
//                    .GroupBy(b => b.ISBN)
//                    .Select(g => g.First())
//                    .ToList();
//            }

//            if (results.Any())
//            {
//                string message;
//                int count = results.Count;

//                if (count == 1)
//                {
//                    message = "1 книга найдена.";
//                }
//                else if (count > 1 && count < 5)
//                {
//                    message = $"{count} книги найдены.";
//                }
//                else
//                {
//                    message = $"{count} книг найдено.";
//                }

//                MessageBox.Show(message, "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
//                this.DialogResult = true;
//                this.Tag = results;
//            }
//            else
//            {
//                MessageBox.Show("Нет книг по данным критериям поиска.", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
//                this.DialogResult = false;
//            }
//        }

//        private void CancelButton_Click(object sender, RoutedEventArgs e)
//        {
//            DialogResult = false;
//            Close();
//        }



//        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
//        {
//            if (e.ChangedButton == MouseButton.Left)
//            {
//                this.DragMove();
//            }
//        }
//    }
//}

using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolLibrary.DialogWindows
{
    public partial class SearchBooksDialog : Window
    {
        private readonly EntityContext _context;

        public SearchBooksDialog(EntityContext context)
        {
            InitializeComponent();
            _context = context;
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();
            CategoryComboBox.ItemsSource = categories;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string author = AuthorTextBox.Text;
            string publisher = PublisherTextBox.Text;
            bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
            bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
            string isbn = ISBNTextBox.Text;
            Category selectedCategory = CategoryComboBox.SelectedItem as Category;

            var query = _context.InventoryBooks
                .Include(ib => ib.Book)
                .Include(ib => ib.Book.Category)
                .Include(ib => ib.Loans)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(ib => ib.Title.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(ib => ib.Author.Contains(author));
            }

            if (!string.IsNullOrWhiteSpace(publisher))
            {
                query = query.Where(ib => ib.Publisher.Contains(publisher));
            }

            if (yearFromParsed)
            {
                query = query.Where(ib => ib.YearPublished.CompareTo(yearFrom.ToString()) >= 0);
            }

            if (yearToParsed)
            {
                query = query.Where(ib => ib.YearPublished.CompareTo(yearTo.ToString()) <= 0);
            }

            if (!string.IsNullOrWhiteSpace(isbn))
            {
                query = query.Where(ib => ib.ISBN.Contains(isbn));
            }

            if (selectedCategory != null)
            {
                query = query.Where(ib => ib.Book.CategoryID == selectedCategory.CategoryID);
            }

            var inventoryBooks = query.ToList();

            List<BookInventoryViewModel> results;

            if (GroupByIsbnRadioButton.IsChecked == true)
            {
                results = inventoryBooks
                    .GroupBy(ib => ib.ISBN)
                    .Select(g => new BookInventoryViewModel
                    {
                        BookID = g.First().Book.BookID,
                        Title = g.First().Title,
                        Author = g.First().Author,
                        Publisher = g.First().Publisher,
                        YearPublished = g.First().YearPublished,
                        ISBN = g.Key,
                        //Quantity = g.Sum(x => x.Book.Quantity),
                        //QuantityLeft = g.Sum(x => x.Book.QuantityLeft),
                        Quantity = g.First().Book.Quantity,
                        QuantityLeft = g.First().Book.QuantityLeft,
                        CategoryName = g.First().Book.Category.CategoryName
                    })
                    .ToList();
            }
            else
            {
                results = inventoryBooks
                    .Select(ib => new BookInventoryViewModel
                    {
                        BookID = ib.Book.BookID,
                        InventoryBookID = ib.InventoryBookID,
                        Title = ib.Title,
                        Author = ib.Author,
                        Publisher = ib.Publisher,
                        YearPublished = ib.YearPublished,
                        ISBN = ib.ISBN,
                        Quantity = 1,
                        QuantityLeft = ib.Loans.Any(loan => !loan.Returned) ? 0 : 1,
                        CategoryName = ib.Book.Category.CategoryName
                    })
                    .ToList();
            }

            if (results.Any())
            {
                string message;
                int count = results.Count;

                if (count == 1)
                {
                    message = "1 книга найдена.";
                }
                else if (count > 1 && count < 5)
                {
                    message = $"{count} книги найдены.";
                }
                else
                {
                    message = $"{count} книг найдено.";
                }

                MessageBox.Show(message, "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Tag = results;
            }
            else
            {
                MessageBox.Show("Нет книг по данным критериям поиска.", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
