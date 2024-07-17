using SchoolLibrary.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;
using System.Windows.Markup;
using System.IO;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;


namespace SchoolLibrary.DialogWindows
{
    public partial class AddBookDialog : Window
    {
        private EntityContext context;
        private List<byte[]> bookPhotoDataList = new List<byte[]>(); // To store multiple photos
        public AddBookDialog(EntityContext dbContext)
        {
            InitializeComponent();
            this.context = dbContext;
            this.Loaded += AddBookDialog_Loaded; // Add Loaded event handler
            dpDateOfReceipt.Language = XmlLanguage.GetLanguage("ru-RU");
            LoadCategories();
        }
        private void LoadCategories()
        {
            try
            {
                var categories = context.Categories.ToList();
                cmbCategory.ItemsSource = categories;
                cmbCategory.DisplayMemberPath = "CategoryName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBookDialog_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeInventoryNumber();
        }
            

        private void InitializeInventoryNumber()
        {
            // Get all inventory numbers from the database
            var inventoryNumbers = context.InventoryBooks
                .Select(ib => ib.InventoryNumber)
                .ToList(); // Materialize the query to avoid multiple enumerations

            // Find the maximum inventory number that can be parsed to int
            var maxInventoryNumberString = inventoryNumbers
                .Where(num => int.TryParse(num, out _))
                .OrderByDescending(num => int.Parse(num))
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(maxInventoryNumberString) && int.TryParse(maxInventoryNumberString, out int maxInventoryNumber))
            {
                txtInventoryNumber.Text = (maxInventoryNumber + 1).ToString();
            }
            else
            {
                txtInventoryNumber.Text = "1";
            }

        }
             
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string errorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                    errorMessage += "Пожалуйста, введите название книги.\n";
                if (string.IsNullOrWhiteSpace(txtAuthor.Text))
                    errorMessage += "Пожалуйста, введите автора книги.\n";
                if (string.IsNullOrWhiteSpace(txtPublisher.Text))
                    errorMessage += "Пожалуйста, введите издателя книги.\n";
                if (!int.TryParse(txtYear.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
                    errorMessage += "Пожалуйста, введите корректный год издания книги (от 1900 до текущего года).\n";
                if (string.IsNullOrWhiteSpace(txtISBN.Text))
                    errorMessage += "Пожалуйста, введите ISBN книги(13 цифр).\n";
                if (!long.TryParse(txtISBN.Text, out long isbn))
                    errorMessage += "Пожалуйста, введите корректный ISBN книги(13 цифр).\n";
                if (txtISBN.Text.Length > 13)
                    errorMessage += "Пожалуйста, введите корректный ISBN книги(не более 13 цифр).\n";
                if (!double.TryParse(txtPrice.Text, out double price) || price <= 0.0 )
                    errorMessage += "Пожалуйста, введите корректную цену книги (через точку например 5.5).\n";
                if (!Regex.IsMatch(txtPrice.Text, @"^\d+(\.\d{1,2})?$"))
                    errorMessage += "Пожалуйста, введите корректную цену книги (число после точки - только 2 цифры).\n";
                if (string.IsNullOrWhiteSpace(txtInventoryNumber.Text))
                    errorMessage += "Пожалуйста, введите инвентарный номер книги(целое число).\n";
                if (!int.TryParse(txtInventoryNumber.Text, out int invetoryNumber))
                    errorMessage += "Пожалуйста, введите корректный инвентарный номер книги(целое число).\n";
                if (!int.TryParse(txtClass.Text, out int bookClass) || bookClass < 1 || bookClass > 11)
                    errorMessage += "Пожалуйста, введите правильный класс к которому относится книга (число от 1 до 11).\n";
                if (string.IsNullOrWhiteSpace(txtIncomingInvoice.Text))
                    errorMessage += "Пожалуйста, введите номер входящей накладной.\n";
                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 1)
                    errorMessage += "Пожалуйста, введите корректное количество (целое число, больше нуля).\n";

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "Вы неверно ввели следующие данные:\n" + errorMessage;
                    throw new Exception(errorMessage);
                }

                // var inventoryNumber = int.Parse(txtInventoryNumber.Text);
                // Инициализация переменной инвентарного номера
                var inventoryNumber = 0;

                // Извлечение всех инвентарных номеров как строк
                var inventoryNumbers = context.InventoryBooks
                    .Select(ib => ib.InventoryNumber)
                    .ToList();

                // Конвертирование строк в целые числа и поиск максимального номера
                var maxInventoryNumber = inventoryNumbers
                    .Where(num => int.TryParse(num, out _))
                    .Select(num => int.Parse(num))
                    .DefaultIfEmpty(0)
                    .Max();

                inventoryNumber = maxInventoryNumber + 1;






                var newInventoryBooks = new List<InventoryBook>();

                for (int i = 0; i < quantity; i++)
                {
                    var inventoryBook = new InventoryBook
                    {
                        Title = txtTitle.Text,
                        Author = txtAuthor.Text,
                        Publisher = txtPublisher.Text,
                        YearPublished = txtYear.Text,
                        ISBN = txtISBN.Text,
                        Price = price,
                        InventoryNumber = (inventoryNumber + i).ToString(),
                        IncomingInvoice = txtIncomingInvoice.Text,
                        DateOfReceipt = dpDateOfReceipt.SelectedDate ?? DateTime.Now
                    };
                    newInventoryBooks.Add(inventoryBook);
                }

                var existingInventoryBook = context.InventoryBooks.FirstOrDefault(ib => ib.ISBN == txtISBN.Text);

                if (existingInventoryBook != null)
                {
                    if (existingInventoryBook.Title != txtTitle.Text ||
                        existingInventoryBook.Author != txtAuthor.Text ||
                        existingInventoryBook.Publisher != txtPublisher.Text ||
                        existingInventoryBook.YearPublished != txtYear.Text ||
                        existingInventoryBook.Book.Class != bookClass ||
                        existingInventoryBook.Book.CategoryID != (cmbCategory.SelectedItem as Category).CategoryID)
                    {
                        errorMessage = "Книга с таким ISBN существует - вы ввели неверные данные для данного ISBN:\n";
                        if (existingInventoryBook.Title != txtTitle.Text)
                            errorMessage += "Название не совпадает - в базе данных: " + existingInventoryBook.Title + "\n";
                        if (existingInventoryBook.Author != txtAuthor.Text)
                            errorMessage += "Автор не совпадает - в базе данных: " + existingInventoryBook.Author + "\n";
                        if (existingInventoryBook.Publisher != txtPublisher.Text)
                            errorMessage += "Издатель не совпадает - в базе данных: " + existingInventoryBook.Publisher + "\n";
                        if (existingInventoryBook.YearPublished != txtYear.Text)
                            errorMessage += "Год издания не совпадает - в базе данных: " + existingInventoryBook.YearPublished + "\n";
                        if (existingInventoryBook.Book.Class != bookClass)
                            errorMessage += "Класс не совпадает - в базе данных: " + existingInventoryBook.Book.Class + "\n";
                        if (existingInventoryBook.Book.CategoryID != (cmbCategory.SelectedItem as Category).CategoryID)
                            errorMessage += "Категория не совпадает - в базе данных: " + existingInventoryBook.Book.Category.CategoryName + "\n";

                        MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        foreach (var inventoryBook in newInventoryBooks)
                        {
                            existingInventoryBook.Book.InventoryBooks.Add(inventoryBook);
                            context.InventoryBooks.Add(inventoryBook);
                        }
                        existingInventoryBook.Book.Quantity += quantity;
                        existingInventoryBook.Book.QuantityLeft += quantity;
                        existingInventoryBook.Book.Description = txtDescription.Text; // Update the description
                        AddBookPhotos(existingInventoryBook.Book);
                    }
                }
                else
                {
                    var newBook = new Book
                    {
                        Class = bookClass,
                        CategoryID = (cmbCategory.SelectedItem as Category).CategoryID,
                        Quantity = quantity,
                        QuantityLeft = quantity,
                        Category = cmbCategory.SelectedItem as Category,
                        Description = txtDescription.Text // Save the description
                    };
                    foreach (var inventoryBook in newInventoryBooks)
                    {
                        newBook.InventoryBooks.Add(inventoryBook);
                        context.InventoryBooks.Add(inventoryBook);
                    }
                    context.Books.Add(newBook);
                    AddBookPhotos(newBook);
                }

                context.SaveChanges();
                MessageBox.Show("Книга добавлена успешно!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectPhotos_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    var photoData = File.ReadAllBytes(fileName);
                    bookPhotoDataList.Add(photoData);
                    var image = new BitmapImage();
                    using (var stream = new MemoryStream(photoData))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = stream;
                        image.EndInit();
                    }
                    lbBookPhotos.Items.Add(image);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddBookPhotos(Book book)
        {
            // Implement photo addition logic
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
