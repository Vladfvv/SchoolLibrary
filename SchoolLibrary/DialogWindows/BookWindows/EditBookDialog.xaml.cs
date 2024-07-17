using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SchoolLibrary.DialogWindows
{
    public partial class EditBookDialog : Window
    {
        private readonly EntityContext _context;
        public Book Book { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public EditBookDialog(EntityContext context, Book book)
        {
            InitializeComponent();
            _context = context;
            Book = book;
            Categories = new ObservableCollection<Category>(_context.Categories.ToList());
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Book.InventoryBooks != null && Book.InventoryBooks.Any())
            {
                // Update the first InventoryBook (assuming you want to update all similarly)
                var inventoryBook = Book.InventoryBooks.First();
                inventoryBook.Title = TitleTextBox.Text;
                inventoryBook.Author = AuthorTextBox.Text;
                inventoryBook.Publisher = PublisherTextBox.Text;
                inventoryBook.YearPublished = YearPublishedTextBox.Text;
                inventoryBook.ISBN = ISBNTextBox.Text;
            }

            Book.Class = int.Parse(ClassTextBox.Text);
            Book.Category = (Category)CategoryComboBox.SelectedItem;
            Book.Quantity = int.Parse(QuantityTextBox.Text);

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Book.InventoryBooks != null && Book.InventoryBooks.Any())
            {
                var inventoryBook = Book.InventoryBooks.First();
                TitleTextBox.Text = inventoryBook.Title;
                AuthorTextBox.Text = inventoryBook.Author;
                PublisherTextBox.Text = inventoryBook.Publisher;
                YearPublishedTextBox.Text = inventoryBook.YearPublished;
                ISBNTextBox.Text = inventoryBook.ISBN;
            }

            ClassTextBox.Text = Book.Class.ToString();
            CategoryComboBox.ItemsSource = Categories;
            CategoryComboBox.SelectedItem = Book.Category;
            QuantityTextBox.Text = Book.Quantity.ToString();
        }
    }
}

