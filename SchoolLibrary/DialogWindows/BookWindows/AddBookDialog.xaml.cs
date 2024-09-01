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
using System.Windows.Controls;


namespace SchoolLibrary.DialogWindows
{
    public partial class AddBookDialog : Window
    {
        private EntityContext context;
        private List<byte[]> bookPhotoDataList = new List<byte[]>(); // To store multiple photos
        public AddBookDialog(EntityContext dbContext)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.context = dbContext;
            this.Loaded += AddBookDialog_Loaded; // Add Loaded event handler
            dpDateOfReceipt.Language = XmlLanguage.GetLanguage("ru-RU");
            dpDateOfReceipt.FirstDayOfWeek = DayOfWeek.Monday;            
            LoadGenres();
        }
        
        private void LoadGenres()
        {
            try
            {
                var genres = context.Genres.ToList();
                cmbGenre.ItemsSource = genres;
                cmbGenre.DisplayMemberPath = "GenreName";
                cmbGenre.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки жанров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadSubjects(int genreId)
        {
            try
            {
                var subjects = context.Subjects.Where(s => s.GenreID == genreId).ToList();
                cmbSubject.ItemsSource = subjects;
                cmbSubject.DisplayMemberPath = "SubjectName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки предметов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGenre.SelectedItem is Genre selectedGenre)
            {
                LoadSubjects(selectedGenre.GenreID);
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
                if (string.IsNullOrWhiteSpace(txtISBN.Text) || txtISBN.Text.Length != 13 || !long.TryParse(txtISBN.Text, out long isbn))
                    errorMessage += "Пожалуйста, введите корректный ISBN книги (13 цифр).\n";
                if (!double.TryParse(txtPrice.Text, out double price) || price <= 0.0 || !Regex.IsMatch(txtPrice.Text, @"^\d+(\.\d{1,2})?$"))
                    errorMessage += "Пожалуйста, введите корректную цену книги (через точку например 5.5, не более 2 цифр после точки).\n";
                if (string.IsNullOrWhiteSpace(txtInventoryNumber.Text) || !int.TryParse(txtInventoryNumber.Text, out int inventoryNumber))
                    errorMessage += "Пожалуйста, введите корректный инвентарный номер книги (целое число).\n";
                if (!int.TryParse(txtClass.Text, out int bookClass) || bookClass < 1 || bookClass > 11)
                    errorMessage += "Пожалуйста, введите правильный класс, к которому относится книга (число от 1 до 11).\n";
                if (string.IsNullOrWhiteSpace(txtIncomingInvoice.Text))
                    errorMessage += "Пожалуйста, введите номер входящей накладной.\n";
                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 1)
                    errorMessage += "Пожалуйста, введите корректное количество (целое число, больше нуля).\n";

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "Вы неверно ввели следующие данные:\n" + errorMessage;
                    throw new Exception(errorMessage);
                }

                var inventoryNumbers = context.InventoryBooks
                     .Select(ib => ib.InventoryNumber)
                     .ToList();

                // Генерация нового инвентарного номера
                var maxInventoryNumber = inventoryNumbers
                    .Where(num => int.TryParse(num, out _))
                    .Select(num => int.Parse(num))
                    .DefaultIfEmpty(0)
                    .Max();

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
                        InventoryNumber = (maxInventoryNumber + i + 1).ToString(),
                        IncomingInvoice = txtIncomingInvoice.Text,
                        DateOfReceipt = dpDateOfReceipt.SelectedDate ?? DateTime.Now
                    };
                    newInventoryBooks.Add(inventoryBook);
                }

                // Проверка на существующую книгу
                var existingInventoryBook = context.InventoryBooks.FirstOrDefault(ib => ib.ISBN == txtISBN.Text);

                if (existingInventoryBook != null)
                {
                    if (existingInventoryBook.Title != txtTitle.Text ||
                        existingInventoryBook.Author != txtAuthor.Text ||
                        existingInventoryBook.Publisher != txtPublisher.Text ||
                        existingInventoryBook.YearPublished != txtYear.Text ||
                        existingInventoryBook.Book.Class != bookClass ||
                        existingInventoryBook.Book.GenreID != (cmbGenre.SelectedItem as Genre).GenreID)
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
                        if (existingInventoryBook.Book.GenreID != (cmbGenre.SelectedItem as Genre).GenreID)
                            errorMessage += "Жанр не совпадает - в базе данных: " + existingInventoryBook.Book.Genre.GenreName + "\n";

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
                        existingInventoryBook.Book.Description = txtDescription.Text;
                        AddBookPhotos(existingInventoryBook.Book);
                    }
                }
                else
                {     
                    Book newBook = null; // Инициализация переменной

                    if (cmbGenre.SelectedItem is Genre selectedGenre)
                    {
                        // Проверка на наличие жанра "Учебная литература" и отсутствие выбранного предмета
                        if (selectedGenre.GenreName == "Учебная литература" && cmbSubject.SelectedItem == null)
                        {
                            MessageBox.Show("Для жанра 'Учебная литература' необходимо выбрать предмет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return; // Выход из метода, если условие не выполнено
                        }

                        // Если жанр не "Учебная литература" и предмет не выбран
                        if (selectedGenre.GenreName != "Учебная литература" && cmbSubject.SelectedItem == null)
                        {
                            // Установка значения по умолчанию для Subject
                            cmbSubject.SelectedItem = new Subject { SubjectID = 0, SubjectName = "Без предмета" };
                        }

                        // Проверка на наличие значений перед созданием объекта Book
                        if (bookClass != null && quantity > 0 && !string.IsNullOrEmpty(txtDescription.Text))
                        {
                            newBook = new Book
                            {
                                Class = bookClass,
                                GenreID = selectedGenre.GenreID,
                                SubjectID = (cmbSubject.SelectedItem as Subject)?.SubjectID ?? 0, // Используем "??" чтобы избежать NullReferenceException
                                Quantity = quantity,
                                QuantityLeft = quantity,
                                Genre = selectedGenre,
                                Subject = cmbSubject.SelectedItem as Subject,
                                Description = txtDescription.Text
                            };
                        }
                        else
                        {
                            MessageBox.Show("Некоторые обязательные поля не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return; // Выход из метода, если проверка не прошла
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите жанр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // Выход из метода, если жанр не выбран
                    }

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


        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                errorMessage = "Название книги обязательно.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                errorMessage = "Автор книги обязателен.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPublisher.Text))
            {
                errorMessage = "Издатель книги обязателен.";
                return false;
            }
            if (!int.TryParse(txtYear.Text, out var year) || year <= 0)
            {
                errorMessage = "Некорректный год издания.";
                return false;
            }
            if (!decimal.TryParse(txtPrice.Text, out var price) || price <= 0)
            {
                errorMessage = "Некорректная цена.";
                return false;
            }
            if (cmbGenre.SelectedItem == null)
            {
                errorMessage = "Выберите жанр книги.";
                return false;
            }
            if (cmbSubject.SelectedItem == null)
            {
                errorMessage = "Выберите предмет книги.";
                return false;
            }
            if (!int.TryParse(txtQuantity.Text, out var quantity) || quantity <= 0)
            {
                errorMessage = "Некорректное количество экземпляров.";
                return false;
            }

            return true;
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddBookPhotos(Book book)
        {
            // Implement photo addition logic
        }

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //        this.DragMove();
        //}
    }
}
