using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using SchoolLibrary.Models;
using SchoolLibrary.DialogWindows;
using SchoolLibrary.DialogWindows.StudentWindows;
using SchoolLibrary.DialogWindows.LoanWindows;
using System.Data;
using SchoolLibrary.DialogWindows.Operations;
using System.ComponentModel;
using SchoolLibrary.DialogWindows.CategoryWindows;
using SchoolLibrary.ViewModels;

namespace SchoolLibrary
{
    using LoanWindowsViewModel = SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel;
    using ModelsViewModel = SchoolLibrary.Models.BookInventoryViewModel;



    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        EntityContext context;
        private string _currentTableName;
        public string CurrentTableName
        {
            get => _currentTableName;
            set
            {
                _currentTableName = value;
                OnPropertyChanged("CurrentTableName");//свойсиво CurrentTableName для управления заголовком окна
                this.Title = $"{_currentTableName}";//для смены названия основного окна при отображении различных данных
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public MainWindow()
        {
            InitializeComponent();
            context = new EntityContext("SchoolLibraryConnectionString");
            this.DataContext = this;
            LoadData();
            InitBooksList();
            //InitStudentsList();
        }

        private void LoadData()
        {
            context.Categories.Load();
            context.InventoryBooks.Load();
            context.BookPhotos.Load();
            context.Books.Load();
            context.Loans.Load();
            context.Students.Load();
        }


        private void InitStudentsList()
        {
            try
            {
                context.Students.Load();
                ConfigureStudentColumns();
                dGrid.ItemsSource = context.Students.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InitCategoriesList()
        {
            try
            {
                context.Categories.Load();
                ConfigureCategoryColumns();
                dGrid.ItemsSource = context.Categories.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InitBookPhotosListList()
        {
            try
            {
                context.BookPhotos.Load();
                ConfigureBookPhotosColumns();
                dGrid.ItemsSource = context.BookPhotos.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InitInventoryBooksList()
        {
            try
            {
                context.InventoryBooks.Load();
                ConfigureInventoryBooksColumns();
                dGrid.ItemsSource = context.InventoryBooks.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void InitBooksList()
        {
            try
            {
                context.Books.Include(b => b.Category).Include(b => b.InventoryBooks.Select(ib => ib.Loans)).Load();

                var groupedBooks = context.Books.Local
                    .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
                    .GroupBy(x => x.InventoryBook.ISBN)
                    .Select(g => new LoanWindowsViewModel
                    {
                        BookID = g.First().Book.BookID,
                        Title = g.First().InventoryBook.Title,
                        Author = g.First().InventoryBook.Author,
                        Publisher = g.First().InventoryBook.Publisher,
                        YearPublished = g.First().InventoryBook.YearPublished,
                        ISBN = g.Key,
                        Quantity = g.Count(),
                        QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                        CategoryName = g.First().Book.Category.CategoryName
                    }).ToList();

                ConfigureBooksColumns();
                dGrid.ItemsSource = groupedBooks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void InitLoansList()
        {
            try
            {
                context.Loans.Include(l => l.InventoryBook).Include(l => l.Student).Load();

                var loansList = context.Loans.Local
                    .Select(l => new
                    {
                        l.InventoryBook.Title,
                        l.Student.FirstName,
                        l.Student.LastName,
                        l.LoanDate,
                        l.DueDate,
                        l.ReturnDate,
                        l.Returned
                    }).ToList();

                dGrid.ItemsSource = loansList;
                ConfigureLoansColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConfigureLoansColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя студента", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия студента", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда взяли", Binding = new Binding("LoanDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда должны вернуть", Binding = new Binding("DueDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Подтверждение возврата", Binding = new Binding("Returned"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureBooksColumns()
        {
            dGrid.Columns.Clear();
            //dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Количество", Binding = new Binding("Quantity"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Остаток", Binding = new Binding("QuantityLeft"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureStudentColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя", Binding = new Binding("FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия", Binding = new Binding("LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Возраст", Binding = new Binding("Age"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Класс", Binding = new Binding("Class"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureCategoryColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("CategoryID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureBookPhotosColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("BookPhotoID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID книги", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата добавления", Binding = new Binding("DateAdded"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }
        private void ConfigureInventoryBooksColumns()
        {
            context.Books.Include(b => b.Category).Load();
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Инв. номер", Binding = new Binding("InventoryNumber"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("Book.Category.CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Год выпуска", Binding = new Binding("YearPublished"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Стоимость", Binding = new Binding("Price"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата прихода", Binding = new Binding("DateOfReceipt"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Приходный документ", Binding = new Binding("IncomingInvoice"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выбытия", Binding = new Binding("DateOfDisposal"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Расходный документ", Binding = new Binding("OutgoingInvoice"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Причина выбытия", Binding = new Binding("ReasonForDisposal"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }

        private void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void updateListCategories()
        {
            try
            {
                dGrid.ItemsSource = null;
                InitCategoriesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void updateListStudent()
        {
            try
            {
                dGrid.ItemsSource = null;
                InitStudentsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void updateListInventoryBook()
        {
            try
            {
                dGrid.ItemsSource = null;
                InitInventoryBooksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }









        private void btnListCategories(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Категории книг";
            updateListCategories();

        }
        private void btnListStudents(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Читатели";
            updateListStudent();
        }
        private void btnListInventoryBook(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Инвентарный журнал книг";
            updateListInventoryBook();
        }
        private void btnListBooks(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Перечень книг";
            try
            {
                dGrid.ItemsSource = null;
                InitBooksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /*private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Поиск книги";
            var searchDialog = new SearchBooksDialog(context);
            if (searchDialog.ShowDialog() == true)
            {
                var results = searchDialog.Tag as List<InventoryBook>;
                if (results != null)
                {
                    dGrid.ItemsSource = results;
                    foreach (var result in results)
                    {
                        result.Book.AddInventoryBook(result);
                    }
                    // Предположим, что результаты поиска относятся к одной книге
                    var firstBook = results.First().Book;
                    CurrentTableName = $"Books with ISBN: {firstBook.InventoryBooks.First().ISBN}";
                }
            }
        }*/
        /* private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
         {
             CurrentTableName = "Поиск книги";
             var searchDialog = new SearchBooksDialog(context);
             if (searchDialog.ShowDialog() == true)
             {
                 var results = searchDialog.Tag as List<Book>;
                 if (results != null)
                 {
                     dGrid.ItemsSource = results;
                     CurrentTableName = $"Найдено книг: {results.Count}";
                 }
             }
         }*/
        private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Поиск книги без группировки по ISBN";
            var searchDialog = new SearchBooksDialog(context);
            if (searchDialog.ShowDialog() == true)
            {
                var results = searchDialog.Tag as List<Models.BookInventoryViewModel>;
                if (results != null)
                {
                    dGrid.ItemsSource = results;
                    if (results.Any())
                    {
                        CurrentTableName = $"Найдено книг с заданными параметрами поиска: {results.First().ISBN}";
                    }
                }
            }
        }

        //private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentTableName = "Поиск книги без группировки по ISBN";
        //    var searchDialog = new SearchBooksDialog(context);

        //    if (searchDialog.ShowDialog() == true)
        //    {
        //        var results = searchDialog.Tag as List<Models.BookInventoryViewModel>;

        //        if (results != null && results.Any())
        //        {
        //            dGrid.ItemsSource = results;
        //            CurrentTableName = $"Найдено книг с заданными параметрами поиска: {results.First().ISBN}";
        //        }
        //        else
        //        {
        //            // Обработка случая, когда результаты пусты или null
        //            MessageBox.Show("Книги не найдены.");
        //            dGrid.ItemsSource = null;
        //        }
        //    }
        //}


        private void btnSearchBooksWithGroupByISBN_Click(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Поиск книги с группировкой по ISBN";
            var searchDialog = new SearchBooksDialog(context);
            if (searchDialog.ShowDialog() == true)
            {
                var results = searchDialog.Tag as List<SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel>;
                if (results != null)
                {
                    dGrid.ItemsSource = results;
                    if (results.Any())
                    {
                        CurrentTableName = $"Найдено книг с заданными параметрами поиска: {results.First().ISBN}";
                    }
                }
            }
        }



        public void DisplaySearchResults(List<Book> books)
        {
            dGrid.ItemsSource = books;
        }






        #region
        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            var addBookDialog = new SchoolLibrary.DialogWindows.AddBookDialog(context);
            var categories = context.Categories.ToList();
            dGrid.ItemsSource = null;
            InitBooksList();
            if (addBookDialog.ShowDialog() == true)
            {
                context.Books.Add((Book)addBookDialog.DataContext);
                context.SaveChanges();
                // Обновляем источник данных DataGrid
                dGrid.ItemsSource = context.Books.ToList(); // метод загрузки даных
                dGrid.Items.Refresh(); // Обновляем отображение DataGrid
                /// context.Entry(Book).Reload();
                dGrid.DataContext = null;
                dGrid.DataContext = context.Books.Local;
            }
            dGrid.ItemsSource = null;
            InitBooksList();
        }
        /*
        public void DeleteBook(Book bookToDelete)
        {
            try
            {
                context.Books.Remove(bookToDelete);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Обработка исключений при удалении
                MessageBox.Show($"Ошибка при удалении книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        */
        private void btnDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенную книгу из DataGrid
            /*  Book selectedBook = dGrid.SelectedItem as Book;

              if (selectedBook != null)
              {
                  MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить книгу '{selectedBook.Title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                  if (result == MessageBoxResult.Yes)
                  {
                      try
                      {
                          context.Books.Remove(selectedBook);
                          context.SaveChanges();
                          MessageBox.Show("Книга успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show($"Ошибка при удалении книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                      }

                      // Обновляем источник данных DataGrid
                      dGrid.ItemsSource = context.Books.ToList();
                  }
              }
              else
              {
                  MessageBox.Show("Выберите книгу для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
              }*/


            // Получаем выделенную книгу из DataGrid
            InventoryBook selectedBook = dGrid.SelectedItem as InventoryBook;

            /* if (selectedBook != null)
             {
                 MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить книгу '{selectedBook.Title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                 if (result == MessageBoxResult.Yes)
                 {
                     try
                     {
                         var bookIDForDelete = selectedBook.BookID;

                         context.InventoryBooks.Remove(selectedBook);
                         context.Books.Remove(bookForDelete);
                         context.SaveChanges();
                         MessageBox.Show("Книга успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                         // Обновляем источник данных DataGrid
                         dGrid.ItemsSource = context.InventoryBooks.ToList();
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show($"Ошибка при удалении книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                 }
             }*/
            /* if (selectedBook != null)
             {
                 MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить книгу '{selectedBook.Title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                 if (result == MessageBoxResult.Yes)
                 {
                     try
                     {
                         // Получаем BookID для удаления книги
                         var bookIDForDelete = selectedBook.BookID;

                         // Находим запись в таблице InventoryBooks
                         var inventoryBook = context.InventoryBooks.FirstOrDefault(ib => ib.BookID == bookIDForDelete);

                         if (inventoryBook != null)
                         {
                             // Удаляем запись из таблицы InventoryBooks
                             context.InventoryBooks.Remove(inventoryBook);
                         }

                         // Находим запись в таблице Books
                         var bookForDelete = context.Books.FirstOrDefault(b => b.BookID == bookIDForDelete);

                         if (bookForDelete != null)
                         {
                             // Удаляем запись из таблицы Books
                             context.Books.Remove(bookForDelete);
                         }

                         // Сохраняем изменения в базе данных
                         context.SaveChanges();

                         MessageBox.Show("Книга успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                         // Обновляем источник данных DataGrid
                         dGrid.ItemsSource = context.InventoryBooks.ToList();
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show($"Ошибка при удалении книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                 }
             }*/

            /*  if (selectedBook != null)
              {
                  MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить книгу '{selectedBook.Title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                  if (result == MessageBoxResult.Yes)
                  {
                      try
                      {
                          // Получаем все инвентарные книги и займы, связанные с данной книгой
                          var inventoryBook = context.InventoryBooks.FirstOrDefault(ib => ib.BookID == selectedBook.BookID);
                          if (inventoryBook == null)
                          {
                              MessageBox.Show("Инвентарная книга не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }

                          var loans = context.Loans.Where(l => l.InventoryBookID == inventoryBook.InventoryBookID).ToList();

                          // Перемещаем данные в таблицы Storage
                          var inventoryBookStorage = new InventoryBookStorage
                          {
                              InventoryNumber = inventoryBook.InventoryNumber,
                              ISBN = inventoryBook.ISBN,
                              Title = inventoryBook.Title,
                              Author = inventoryBook.Author,
                              Publisher = inventoryBook.Publisher,
                              YearPublished = inventoryBook.YearPublished,
                              Price = inventoryBook.Price,
                              DateOfReceipt = inventoryBook.DateOfReceipt,
                              IncomingInvoice = inventoryBook.IncomingInvoice,
                              DateOfDisposal = inventoryBook.DateOfDisposal,
                              OutgoingInvoice = inventoryBook.OutgoingInvoice,
                              ReasonForDisposal = inventoryBook.ReasonForDisposal,
                              BookID = inventoryBook.BookID
                          };

                          context.InventoryBooksStorage.Add(inventoryBookStorage);

                          foreach (var loan in loans)
                          {
                              var loanStorage = new LoanStorage
                              {
                                  InventoryBookStorageID = inventoryBookStorage.InventoryBookStorageID, // Связываем с InventoryBookStorage
                                  StudentID = loan.StudentID,
                                  LoanDate = loan.LoanDate,
                                  DueDate = loan.DueDate,
                                  ReturnDate = loan.ReturnDate,
                                  Returned = loan.Returned
                              };

                              context.LoansStorage.Add(loanStorage);
                              context.Loans.Remove(loan);
                          }

                          // Удаляем книгу из таблицы Books
                          var bookForDelete = context.Books.Find(selectedBook.BookID);
                          if (bookForDelete != null)
                          {
                              context.Books.Remove(bookForDelete);
                          }

                          // Удаляем инвентарную книгу из таблицы InventoryBooks
                          context.InventoryBooks.Remove(inventoryBook);

                          context.SaveChanges();

                          MessageBox.Show("Книга успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                          // Обновляем источник данных DataGrid
                          dGrid.ItemsSource = context.InventoryBooks.ToList();
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show($"Ошибка при удалении книги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                      }
                  }
              }*/
            if (selectedBook != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить книгу '{selectedBook.Title}'?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Получаем инвентарную книгу и займы, связанные с данной книгой
                        // var inventoryBook = context.InventoryBooks.SingleOrDefault(ib => ib.BookID == selectedBook.BookID);
                        var inventoryBook = context.InventoryBooks.SingleOrDefault(ib => ib.InventoryBookID == selectedBook.InventoryBookID);
                        // var inventoryBook = context.InventoryBooks.SingleOrDefault(ib => ib.BookID == selectedBook.BookID);
                        var loans = context.Loans.Where(l => l.InventoryBookID == inventoryBook.InventoryBookID).ToList();
                        if (inventoryBook != null)
                        {
                            // Перемещаем данные в таблицы Storage
                            var inventoryBookStorage = new InventoryBookStorage
                            {
                                InventoryNumber = inventoryBook.InventoryNumber,
                                ISBN = inventoryBook.ISBN,
                                Title = inventoryBook.Title,
                                Author = inventoryBook.Author,
                                Publisher = inventoryBook.Publisher,
                                YearPublished = inventoryBook.YearPublished,
                                Price = inventoryBook.Price,
                                DateOfReceipt = inventoryBook.DateOfReceipt,
                                IncomingInvoice = inventoryBook.IncomingInvoice,
                                DateOfDisposal = inventoryBook.DateOfDisposal,
                                OutgoingInvoice = inventoryBook.OutgoingInvoice,
                                ReasonForDisposal = inventoryBook.ReasonForDisposal,
                                BookID = inventoryBook.BookID
                            };

                            context.InventoryBooksStorage.Add(inventoryBookStorage);
                            context.SaveChanges();

                            // Получаем ID только что добавленной записи в InventoryBookStorage
                            int inventoryBookStorageID = inventoryBookStorage.InventoryBookStorageID;

                            // Перемещаем связанные займы в таблицу LoanStorage
                            foreach (var loan in loans)
                            {
                                var loanStorage = new LoanStorage
                                {
                                    InventoryBookStorageID = inventoryBookStorageID,
                                    StudentID = loan.StudentID,
                                    LoanDate = loan.LoanDate,
                                    DueDate = loan.DueDate,
                                    ReturnDate = loan.ReturnDate,
                                    Returned = loan.Returned
                                };

                                context.LoansStorage.Add(loanStorage);
                                context.Loans.Remove(loan);
                            }
                            // Уменьшаем количество экземпляров книги в таблице Books
                            var book = context.Books.SingleOrDefault(b => b.BookID == inventoryBook.BookID);
                            if (book != null)
                            {
                                book.Quantity--;
                                book.QuantityLeft--;
                                context.SaveChanges();
                            }
                            context.SaveChanges();
                            // Удаляем книгу из таблицы Books сначала
                            var bookForDelete = context.Books.Find(selectedBook.BookID);

                            // Удаляем инвентарную книгу после удаления всех связанных записей
                            context.InventoryBooks.Remove(selectedBook);
                            context.SaveChanges();
                            MessageBox.Show("Книга успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            if (bookForDelete.Quantity == 0 && bookForDelete.QuantityLeft == 0)
                            {
                                context.Books.Remove(bookForDelete);////////////////////
                                //context.Books.Remove(selectedBook.Book);////////////////////
                                context.SaveChanges();///////////////////////////////////
                            }
                            /*else
                            {
                                MessageBox.Show("Книга для удаления не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }*/
                            // Обновляем источник данных DataGrid
                            dGrid.ItemsSource = context.InventoryBooks.ToList();
                        }
                        else
                        {
                            MessageBox.Show("Инвентарная книга для удаления не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении книги: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            else
            {
                MessageBox.Show("Выберите книгу для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateListInventoryBook();
            }
        }


        private void LoadStudents()
        {
            context.Students.Load();
            dGrid.ItemsSource = context.Students.Local;
        }

        public void UpdateStudentDataGrid(List<Student> students)
        {
            dGrid.ItemsSource = null;
            dGrid.ItemsSource = students;
        }



        private void btnSearchStudents_Click(object sender, RoutedEventArgs e)
        {
            dGrid.ItemsSource = null;
            InitStudentsList();
            SearchStudentDialog searchDialog = new SearchStudentDialog(context);
            searchDialog.ShowDialog();
        }

        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentDialog asd = new AddStudentDialog(context);
            var result = asd.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Новый студент был создан");
                updateListStudent();
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryDialog acd = new AddCategoryDialog(context);
            var result = acd.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Новая категория книг была создана");
                updateListCategories();
            }
        }




        private void btnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {

            if (dGrid.SelectedItem is Student selectedStudent)
            {
                DeleteStudentDialog dialog = new DeleteStudentDialog(context, selectedStudent);
                if (dialog.ShowDialog() == true)
                {
                    // Обновляем DataGrid после удаления студента
                    dGrid.ItemsSource = context.Students.ToList();
                }
            }
            else
            {
                MessageBox.Show("Выберите студента для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListStudent();
            }
        }


        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is Category selectedCategory)
            {
                DeleteCategoryDialog dialog = new DeleteCategoryDialog(context, selectedCategory);
                if (dialog.ShowDialog() == true)
                {
                    // Обновляем DataGrid после удаления категории, если удаление прошло успешно
                    try
                    {
                        dGrid.ItemsSource = context.Categories.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки категорий после удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListCategories();
            }
        }


        #endregion


        private void btnBookReport_Click(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Отчет по книгам";
            try
            {
                dGrid.ItemsSource = null;
                InitLoansList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void WindowBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentTableName = "Информация о программе";
            MessageBox.Show("Информация о программе: SchoolLibrary ver 1.0\n" +
                "Программа для автомации школьной библиотеки\n" +
                "Разработчик: студент гр. ПВ2-22ПО\n" +
                "ФИО: Филимонцев В.В.\n" +
                "2024г.");

        }

        private void btnEditBook_Click(object sender, RoutedEventArgs e)
        {
            dGrid.ItemsSource = null;
            updateListInventoryBook();
            if (dGrid.SelectedItem is Book selectedBook)
            {
                var editBookDialog = new EditBookDialog(context, selectedBook);
                if (editBookDialog.ShowDialog() == true)
                {
                    context.SaveChanges();
                    dGrid.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateListInventoryBook();
            }
            dGrid.ItemsSource = null;
            InitBooksList();
        }

        private void btnEditStudent_Click(object sender, RoutedEventArgs e)
        {

            if (dGrid.SelectedItem is Student selectedStudent)
            {
                var studentCopy = new Student
                {
                    StudentID = selectedStudent.StudentID,
                    FirstName = selectedStudent.FirstName,
                    LastName = selectedStudent.LastName,
                    Age = selectedStudent.Age,
                    Class = selectedStudent.Class
                };

                EditStudentDialog dialog = new EditStudentDialog(context, studentCopy);
                if (dialog.ShowDialog() == true)
                {
                    selectedStudent.FirstName = studentCopy.FirstName;
                    selectedStudent.LastName = studentCopy.LastName;
                    selectedStudent.Age = studentCopy.Age;
                    selectedStudent.Class = studentCopy.Class;
                    context.SaveChanges();

                    dGrid.ItemsSource = context.Students.ToList();
                }
            }
            else
            {
                MessageBox.Show("Выберите читателя для редактирования", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListStudent();
            }
        }

        private void btnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is Category selectedCategory)
            {
                var categoryCopy = new Category
                {
                    CategoryID = selectedCategory.CategoryID,
                    CategoryName = selectedCategory.CategoryName
                };

                EditCategoryDialog dialog = new EditCategoryDialog(context, categoryCopy);
                if (dialog.ShowDialog() == true)
                {
                    selectedCategory.CategoryName = categoryCopy.CategoryName;
                    context.SaveChanges();

                    dGrid.ItemsSource = context.Categories.ToList();
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                updateListCategories();
            }
        }



        private void LoanBookButton_Click(object sender, RoutedEventArgs e)
        {
            dGrid.ItemsSource = null;
            context.Books.Include(b => b.Category).Load();
            context.Students.Load(); // Загрузка студентов
            context.Loans.Load();
            InitLoansList();

            LoanDialog dialog = new LoanDialog(context);
            {
                if (dialog.ShowDialog() == true)
                {
                    MessageBox.Show("Новая запись в журнале оформлена");
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так...");
                }

                dGrid.ItemsSource = null;
                InitLoansList();
            }
        }



        private void btnShowAvailableBooksButton_Click(object sender, RoutedEventArgs e)
        {

            context.Students.Load();
            context.Categories.Load();
            context.Books.Load();
            context.Loans.Load();

            dGrid.ItemsSource = null;
            InitBooksList();

        }

        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            context.Students.Load();
            context.Categories.Load();
            context.Books.Load();
            context.Loans.Load();
            dGrid.ItemsSource = null;
            InitLoansList();
            var dialog = new ReturnBookDialog(context);

            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Возврат в журнале оформлен");
            }
            else
            {
                MessageBox.Show("Что-то пошло не так...");
            }
            dGrid.ItemsSource = null;
            InitLoansList();

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Показ пользовательского диалогового окна с подтверждением
            ConfirmExitWindow confirmExitWindow = new ConfirmExitWindow
            {
                Owner = this // Установка владельца для центрирования окна
            };
            confirmExitWindow.ShowDialog();

            // Если пользователь не подтвердил выход, отменить закрытие
            if (!confirmExitWindow.IsConfirmed)
            {
                e.Cancel = true;
            }
        }

        private List<Student> GetStudents()
        {
            return context.Students.ToList();
        }

        private List<Category> GetCategories()
        {
            return context.Categories.ToList();
        }

        private List<InventoryBook> GetInventoryBooks()
        {
            return context.InventoryBooks.ToList();
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is IDeletable deletableItem)
            {
                try
                {
                    deletableItem.Delete(context);

                    // Обновляем DataGrid в зависимости от типа элемента
                    if (dGrid.SelectedItem is Student)
                    {
                        dGrid.ItemsSource = GetStudents();
                    }
                    else if (dGrid.SelectedItem is Category)
                    {
                        dGrid.ItemsSource = GetCategories();
                    }
                    else if (dGrid.SelectedItem is InventoryBook)
                    {
                        dGrid.ItemsSource = GetInventoryBooks();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении элемента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnSearchLoanedBooks_Click(object sender, RoutedEventArgs e)
        {
            CurrentTableName = "Поиск взятых книг";
            var searchDialog = new SearchLoanedBooksDialog(context);

            if (searchDialog.ShowDialog() == true)
            {
                var results = searchDialog.Tag as List<LoanBookStudentViewModel>;

                if (results != null && results.Any())
                {
                    // Очистим существующие столбцы (если нужно)
                    dGrid.Columns.Clear();

                    // Добавляем столбцы с привязками
                    dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("BookTitle"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя студента", Binding = new Binding("StudentName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия студента", Binding = new Binding("StudentLastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда взяли", Binding = new Binding("LoanDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда должны вернуть", Binding = new Binding("DueDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
                    dGrid.Columns.Add(new DataGridTextColumn { Header = "Подтверждение возврата", Binding = new Binding("Returned"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });

                    // Установим ItemsSource для DataGrid
                    dGrid.ItemsSource = results;

                    CurrentTableName = $"Найдено книг с заданными параметрами поиска: {results.Count}";
                }
                else
                {
                    MessageBox.Show("Книги не найдены.");
                    dGrid.ItemsSource = null;
                }
            }
        }
    }
}
