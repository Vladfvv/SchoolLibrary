using SchoolLibrary.DialogWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
using SchoolLibrary.ViewModels;

namespace SchoolLibrary.Views
{
       
    public partial class StartUserWindow : BaseWindow
    {       

        public StartUserWindow(EntityContext context) : base(context)
        {
            InitializeComponent();
            this.context = context;            
        }



        private void UpdatePaginationButtons(int pageCount)
        {
            // Обновляем количество страниц и текущую страницу
            totalPages = pageCount;

            // Устанавливаем видимость и активность кнопок в зависимости от количества страниц
            if (totalPages <= 1)
            {
                // Если страниц 1 или меньше, прячем обе кнопки
                btnPrevious.Visibility = Visibility.Collapsed;
                btnNext.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Иначе показываем кнопки, если есть более одной страницы
                btnPrevious.Visibility = currentPage > 1 ? Visibility.Visible : Visibility.Collapsed;
                btnNext.Visibility = currentPage < totalPages ? Visibility.Visible : Visibility.Collapsed;
            }

            // Управляем активностью кнопок
            btnPrevious.IsEnabled = currentPage > 1;
            btnNext.IsEnabled = currentPage < totalPages;
        }

        private void btnListBooks(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            totalPages = 1;
            CurrentTableName = "Перечень книг";
            updateListBooks();
        }

        private void updateListBooks()
        {
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

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                InitBooksList();
                UpdatePaginationButtons(totalPages);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                InitBooksList();
                UpdatePaginationButtons(totalPages);
            }
        }


        private void ConfigureBooksColumns()
        {
            dGrid.Columns.Clear();
            // Колонка для порядкового номера
            var indexColumn = new DataGridTextColumn { Header = "№", Width = new DataGridLength(0.25, DataGridLengthUnitType.Star), Binding = new Binding("Index") };
            indexColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(indexColumn);            
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(2, DataGridLengthUnitType.Star) });//2 - ширина в 2 раза больше стандартной
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(0.9, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            var yearColumn = new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(0.4, DataGridLengthUnitType.Star) };// Ширина меньше, чем у остальных
            yearColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(yearColumn);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(0.9, DataGridLengthUnitType.Star) });
            var quantityColumn = new DataGridTextColumn { Header = "Количество", Binding = new Binding("Quantity"), Width = new DataGridLength(0.6, DataGridLengthUnitType.Star) };// Ширина меньше, чем у остальных
            quantityColumn.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(quantityColumn);           
        }

        protected override void ConfigureLoansColumns() //Очищаем все текущие колонки в DataGrid и добавляем новые колонки с соответствующими заголовками и привязками
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1.15, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя читателя", Binding = new Binding("FirstName"), Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия читателя", Binding = new Binding("LastName"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выдачи", Binding = new Binding("LoanDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            var dueDate = new DataGridTextColumn { Header = "Должны вернуть", Binding = new Binding("DueDate") { StringFormat = "dd/MM/yyyy" }, Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) };
            dueDate.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(dueDate);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            
                       
        }
                

        private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            totalPages = 1;
            dGrid.ItemsSource = null;  // Очистка DataGrid перед загрузкой новых данных                                      
            currentListType = "SearchBooks";
            CurrentTableName = "Поиск книги";
            updateListBooks();
            var searchDialog = new SearchBooksDialog(context);
            if (searchDialog.ShowDialog() == true)
            {
                var results = searchDialog.Tag as List<ViewModels.PaginatedBookInventoryModel>;
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


        private void InitBooksList()
        {
            try
            {
                // Загружаем книги и связанные данные
                context.Books
                    .Include(b => b.Genre)
                    .Include(b => b.Subject)
                    .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                    .Load();

                // Группируем книги по ISBN
                var groupedBooks = context.Books.Local
                    .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
                    .GroupBy(x => x.InventoryBook.ISBN)
                      .Select(g => new PaginatedBookInventoryModel                     
                      {
                          BookID = g.First().Book.BookID,
                          Title = g.First().InventoryBook.Title,
                          Author = g.First().InventoryBook.Author,
                          Publisher = g.First().InventoryBook.Publisher,
                          YearPublished = g.First().InventoryBook.YearPublished,
                          ISBN = g.Key,
                          Quantity = g.Count(),
                          QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),                         
                          GenreName = g.First().Book.Genre.GenreName != null ? g.First().Book.Genre.GenreName : "Неизвестно",
                          SubjectName = g.First().Book.Subject.SubjectName != null ? g.First().Book.Subject.SubjectName : "Неизвестно"
                      }).ToList();


                // Обновляем пагинацию
                totalPages = (int)Math.Ceiling((double)groupedBooks.Count / PageSize);
                var paginatedBooks = groupedBooks
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize)
                    .Select((book, index) => new PaginatedBookInventoryModel
                    {
                        Index = (currentPage - 1) * PageSize + index + 1, // Вычисляем индекс с учетом текущей страницы
                        BookID = book.BookID,
                        InventoryBookID = book.InventoryBookID, 
                        Title = book.Title,
                        Author = book.Author,
                        Publisher = book.Publisher,
                        YearPublished = book.YearPublished,
                        ISBN = book.ISBN,
                        Quantity = book.Quantity,
                        QuantityLeft = book.QuantityLeft,
                        CategoryName = book.CategoryName
                    })
                    .ToList();

                // Обновляем источник данных и колонки
                ConfigureBooksColumns();
                dGrid.ItemsSource = paginatedBooks;

                // Обновляем отображение DataGrid
                dGrid.Items.Refresh();

                // Обновляем видимость кнопок
                UpdatePaginationButtons(totalPages);
                dGrid.Tag = "Books"; //тэг для использования при идентификации данных что сейчас в dDrid
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ConfigureInventoryBooksColumns()
        {
            context.Books.Include(b => b.Genre).Load();
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
               

        private void btnShowAvailableBooksButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            totalPages = 1;
            context.Students.Load();
            context.Categories.Load();
            context.Books.Load();
            context.Loans.Load();

            dGrid.ItemsSource = null;
            InitBooksListAvailableNow();
        }

        private void InitBooksListAvailableNow()
        {
            try
            {
                // Загрузка связанных сущностей
                context.Books
                    .Include(b => b.Genre)
                    .Include(b => b.InventoryBooks.Select(ib => ib.Loans))
                    .Load();

                var groupedBooks = context.Books.Local
                    .SelectMany(b => b.InventoryBooks, (b, ib) => new { Book = b, InventoryBook = ib })
                    .GroupBy(x => x.InventoryBook.ISBN)                   
                    .Select((g, index) => new PaginatedBookInventoryModel
                    {
                        BookID = g.First().Book.BookID,
                        Title = g.First().InventoryBook.Title,
                        Author = g.First().InventoryBook.Author,
                        Publisher = g.First().InventoryBook.Publisher,
                        YearPublished = g.First().InventoryBook.YearPublished,
                        ISBN = g.Key,
                        Quantity = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                        QuantityLeft = g.Count() - g.Sum(x => x.InventoryBook.Loans.Count(loan => !loan.Returned)),
                        CategoryName = g.First().Book.Genre.GenreName,
                        Index = index + 1 // Устанавливаем индекс
                    })
                    .Where(b => b.Quantity > 0) // Фильтрация книг с нулевым количеством
                    .ToList();

                // Обновляем пагинацию
                totalPages = (int)Math.Ceiling((double)groupedBooks.Count / PageSize);
                var paginatedBooks = groupedBooks
                    .Skip((currentPage - 1) * PageSize)
                    .Take(PageSize)
                     .Select((b, index) =>
                     {
                         b.Index = (currentPage - 1) * PageSize + index + 1; // Корректировка индекса
                         return b;
                     })
                    .ToList();

                ConfigureBooksColumns();
                dGrid.ItemsSource = paginatedBooks;

                // Обновляем отображение DataGrid
                dGrid.Items.Refresh();

                // Обновляем видимость кнопок
                UpdatePaginationButtons(totalPages);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

