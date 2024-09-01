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
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;

namespace SchoolLibrary.DialogWindows
{
    public partial class EditBookDialog : Window
    {
        private readonly EntityContext _context;
        public Book Book { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Genre> Genres { get; set; }
        public ObservableCollection<Subject> Subjects { get; set; }

        public EditBookDialog(EntityContext context, Book book)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _context = context;
            Book = book;
            Categories = new ObservableCollection<Category>(_context.Categories.ToList());
            Genres = new ObservableCollection<Genre>(_context.Genres.ToList());
            Subjects = new ObservableCollection<Subject>(_context.Subjects.ToList());
            DataContext = this;
            Loaded += Window_Loaded; // Добавление обработчика события Loaded
        }



        ////private void OkButton_Click(object sender, RoutedEventArgs e)
        ////{
        ////    if (Book.InventoryBooks != null && Book.InventoryBooks.Any())
        ////    {
        ////        // Обновляем первую InventoryBook
        ////        var inventoryBook = Book.InventoryBooks.First();
        ////        inventoryBook.Title = TitleTextBox.Text;
        ////        inventoryBook.Author = AuthorTextBox.Text;
        ////        inventoryBook.Publisher = PublisherTextBox.Text;
        ////        inventoryBook.YearPublished = YearPublishedTextBox.Text;
        ////        inventoryBook.ISBN = ISBNTextBox.Text;
        ////    }

        ////    Book.Class = int.Parse(ClassTextBox.Text);
        ////    Book.Genre = (Genre)GenreComboBox.SelectedItem;
        ////    Book.Subject = (Subject)SubjectComboBox.SelectedItem;
        ////    //Book.Genre = (Genre)CategoryComboBox.SelectedItem;
        ////    // Book.Quantity = int.Parse(QuantityTextBox.Text); убрал чтобы не мог редактировать количество - только admin может

        ////    //_context.SaveChanges();
        ////    //DialogResult = true;
        ////    //Close();

        ////    try
        ////    {
        ////        _context.SaveChanges();
        ////        DialogResult = true;
        ////        Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        MessageBox.Show("Ошибка сихранения изменений: " + ex.Message);
        ////    }
        ////}


        //private void OkButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // Получаем новый ISBN из текстового поля
        //        string newISBN = ISBNTextBox.Text;

        //        // Находим все книги с таким же ISBN
        //        //var booksToUpdate = _context.Books
        //        //    .Include(b => b.InventoryBooks)
        //        //    .Where(b => b.InventoryBooks.Any(ib => ib.ISBN == Book.InventoryBooks.First().ISBN))
        //        //.ToList();


        //        var booksToUpdate = _context.Books
        //                              .Include(b => b.InventoryBooks)
        //                              .ToList();

        //        foreach (var book in booksToUpdate)
        //        {
        //            foreach (var inventoryBook in book.InventoryBooks)
        //            {
        //                // Обновляем данные для каждой книги
        //                inventoryBook.Title = TitleTextBox.Text;
        //                inventoryBook.Author = AuthorTextBox.Text;
        //                inventoryBook.Publisher = PublisherTextBox.Text;
        //                inventoryBook.YearPublished = YearPublishedTextBox.Text;
        //                inventoryBook.ISBN = newISBN;
        //            }

        //            // Обновляем данные книги
        //            book.Class = int.Parse(ClassTextBox.Text);
        //            book.Genre = (Genre)GenreComboBox.SelectedItem;
        //            book.Subject = (Subject)SubjectComboBox.SelectedItem;
        //        }

        //        // Сохраняем изменения в базе данных
        //        _context.SaveChanges();
        //        DialogResult = true;
        //        Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка сохранения изменений: " + ex.Message);
        //    }
        //}

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем новый ISBN из текстового поля
                string newISBN = ISBNTextBox.Text;

                // Проверка на пустое или некорректное значение ISBN
                if (string.IsNullOrWhiteSpace(newISBN))
                {
                    MessageBox.Show("ISBN не может быть пустым.");
                    return;
                }

                // Попробуем преобразовать год публикации в числовое значение
                if (!int.TryParse(YearPublishedTextBox.Text, out int yearPublished))
                {
                    MessageBox.Show("Некорректный год публикации.");
                    return;
                }

                // Получаем ISBN текущей книги
                string currentISBN = Book.InventoryBooks.First().ISBN;

                // Находим все книги с таким же ISBN, что и у текущей книги
                var booksToUpdate = _context.Books
                                            .Include(b => b.InventoryBooks)
                                            .Where(b => b.InventoryBooks.Any(ib => ib.ISBN == currentISBN))
                                            .ToList();

                foreach (var book in booksToUpdate)
                {
                    foreach (var inventoryBook in book.InventoryBooks)
                    {
                        // Обновляем данные только для инвентарных книг с совпадающим ISBN
                        if (inventoryBook.ISBN == currentISBN)
                        {
                            inventoryBook.Title = TitleTextBox.Text;
                            inventoryBook.Author = AuthorTextBox.Text;
                            inventoryBook.Publisher = PublisherTextBox.Text;
                            inventoryBook.YearPublished = yearPublished.ToString();
                            inventoryBook.ISBN = newISBN; // Обновляем ISBN
                        }
                    }

                    // Обновляем данные книги
                    if (int.TryParse(ClassTextBox.Text, out int bookClass))
                    {
                        book.Class = bookClass;
                    }
                    else
                    {
                        MessageBox.Show("Некорректный класс.");
                        return;
                    }

                    book.Genre = (Genre)GenreComboBox.SelectedItem;
                    book.Subject = (Subject)SubjectComboBox.SelectedItem;
                }

                // Сохраняем изменения в базе данных
                _context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения изменений: " + ex.Message);
            }
        }




        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        this.DragMove();
        //    }
        //}



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
            GenreComboBox.ItemsSource = Genres;
            GenreComboBox.SelectedItem = Book.Genre;
            //QuantityTextBox.Text = Book.Quantity.ToString();  убрал чтобы не мог редактировать количество - только admin может
        }
    }
}

