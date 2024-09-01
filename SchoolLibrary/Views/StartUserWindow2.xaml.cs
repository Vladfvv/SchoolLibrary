using SchoolLibrary.AuthWindows;
using SchoolLibrary.DataLoaders;
using SchoolLibrary.DialogWindows.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SchoolLibrary.DialogWindows;

namespace SchoolLibrary.Views
{

    //создание псевдонима для типа SchoolLibrary.DialogWindows.LoanWindows.BookInventoryViewModel.
    //Это позволяет использовать псевдоним LoanWindowsViewModel вместо полного имени типа SchoolLibrary.   //DialogWindows.LoanWindows.BookInventoryViewModel
    using LoanWindowsViewModel = SchoolLibrary.ViewModels.BookInventoryViewModel;
    using ModelsViewModel = SchoolLibrary.ViewModels.PaginatedBookInventoryModel;
    public partial class StartUserWindow2 : Window, INotifyPropertyChanged
    {
        protected EntityContext context;
        private readonly DataLoader _dataLoader;
        private string _currentTableName;
        private bool isClosingFromLogout = false; // Флаг для проверки, вызвано ли закрытие через разлогинивание

        public string CurrentTableName
        {
            get => _currentTableName;
            set
            {
                _currentTableName = value;
                OnPropertyChanged(nameof(CurrentTableName));
                this.Title = $"{_currentTableName}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //public StartUserWindow2(DataLoader dataLoader)
        public StartUserWindow2(EntityContext context)
        {
            InitializeComponent();
            this.context = context; 

           // _dataLoader = dataLoader;
           // _dataLoader.LoadData();
            this.Closing += Window_Closing;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, WindowBinding_Executed));
        }
        //В методе Window_Closing проверяется флаг isClosingFromLogout.Если флаг установлен, диалоговое окно подтверждения не будет показано, и окно будет закрыто.
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!isClosingFromLogout && !OnConfirmExit())
            {
                e.Cancel = true; // Отменить закрытие окна, если подтверждение не получено
            }
        }

        protected virtual bool OnConfirmExit()
        {
            var confirmExitWindow = new ConfirmExitWindow
            {
                Owner = this // Установка владельца для центрирования окна
            };
            confirmExitWindow.ShowDialog();
            return confirmExitWindow.IsConfirmed;
        }

       //isClosingFromLogout Флаг устанавливается в true при вызове btnLogout_Click.Это позволяет Window_Closing отличить закрытие, вызванное кнопкой выхода, от других способов закрытия окна.
        protected void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (OnConfirmExit())
            {
                isClosingFromLogout = true; // Устанавливаем флаг, чтобы предотвратить повторное отображение диалогового окна
                var startWindow = new StartWindow();
                startWindow.Show();
                this.Close(); // Закрываем текущее окно
            }
        }

        protected void WindowBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentTableName = "Информация о программе";
            MessageBox.Show("Информация о программе: SchoolLibrary ver 1.0\n" +
                "Программа для автомации школьной библиотеки\n" +
                "Разработчик: студент гр. ПВ2-22ПО\n" +
                "ФИО: Филимонцев В.В.\n" +
                "2024г.");
        }

        protected void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
        private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        {
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

        private void btnListBooks(object sender, RoutedEventArgs e)
        {
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

        protected void InitBooksList()
        {
            try
            {
                // Загрузка связанных сущностей с использованием Include и ThenInclude
                context.Books
                    .Include(b => b.Genre)
                    .Include(b => b.InventoryBooks.Select(ib => ib.Loans)) // используем ThenInclude правильно
                    .Load();

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
                        CategoryName = g.First().Book.Genre.GenreName
                    }).ToList();

                ConfigureBooksColumns();
                dGrid.ItemsSource = groupedBooks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            if (OnConfirmExit())
            {
                Application.Current.Shutdown(); // Завершить приложение
            }
        }

    }



}
