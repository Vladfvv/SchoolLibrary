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

namespace SchoolLibrary
{

    /*
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {//7.В коде окна создайте контекст базы данных и поместите свойство
     //  Notices в DataContext элемента DataGrid
        EntityContext context;
       // ObservableCollection<Category> categoryList;

        public MainWindow()
        {
            context = new EntityContext("SchoolLibraryConnectionString");
            InitializeComponent();
            InitStudentsList();
        }
        // задает столбцы для отображения данных студентов
        private void InitStudentsList()
        {
            if (dGrid.DataContext != null) dGrid.DataContext = null;
            try
            {               
                context.Students.Load();
                dGrid.DataContext = context.Students.Local;
                ConfigureStudentColumns();


             //   dGrid.DataContext = context.Students.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        // задает столбцы для отображения данных категорий
        private void InitCategoryList()
        {
            if (dGrid.DataContext != null) dGrid.DataContext = null;
            try
            {
                context.Categories.Load();       
                dGrid.DataContext = context.Categories.Local;
                ConfigureCategoryColumns();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("CategoryID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }


        //10.Обработка событий нажатия кнопок главного окна
        //        Кнопки добавления и редактирования объекта должны вызывать
        //        открытия диалогового окна редактирования.Если при закрытии окна
        //редактирования была нажата кнопка ОК, то данные нужно сохранить:

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var student = new Student();
            EditWindow ew = new EditWindow(student);
            var result = ew.ShowDialog();
            if (result == true)
            {
                context.Students.Add(student);
                context.SaveChanges();
                ew.Close();
            }
        }
        //11.При удалении объекта нужно вывести предупреждение:
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Вы уверены?", "Удалить запись", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Student student = dGrid.SelectedItem as Student;
                    context.Students.Remove(student);                    
                }
                context.SaveChanges();
            }
            catch (Exception ex) { MessageBox.Show("Что-то пошло не так!!!\n" + ex.Message); }

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Student student = dGrid.SelectedItem as Student;

            EditWindow ew = new EditWindow(student);
            var result = ew.ShowDialog();
            if (result == true)
            {
                context.SaveChanges();
                ew.Close();
            }
            else
            {
                // вернуть начальное значение
                context.Entry(student).Reload();
                // перегрузить DataContext
                dGrid.DataContext = null;
                dGrid.DataContext = context.Students.Local;
            }
        }
        //13.Обработка события нумерации при загрузке в dGrid
        private void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btnListCategories(object sender, RoutedEventArgs e)
        {
            try
            {               
                InitCategoryList(); 
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void btnListStudents(object sender, RoutedEventArgs e)
        {
            try
            {               
                InitStudentsList();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }*/



    public partial class MainWindow : Window
    {
        EntityContext context;

        public MainWindow()
        {
            InitializeComponent();
            context = new EntityContext("SchoolLibraryConnectionString");
            InitStudentsList();
        }

        private void InitStudentsList()
        {
            try
            {
                context.Students.Load();
                ConfigureStudentColumns();
                dGrid.ItemsSource = context.Students.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitCategoriesList()
        {
            try
            {
                context.Categories.Load();
                ConfigureCategoryColumns();
                dGrid.ItemsSource = context.Categories.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitBooksList()
        {
            try
            {
                context.Books.Load();
                ConfigureBooksColumns();
                dGrid.ItemsSource = context.Books.Local;
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
                context.Loans.Load();
                ConfigureLoansColumns();
                dGrid.ItemsSource = context.Loans.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /*
        public static Binding Create(object source, string path, BindingMode mode)
        {
            return new Binding()
            {
                Source = source,
                Path = new PropertyPath(path),
                Mode = mode
            };
        }*/



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
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("CategoryID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }

        private void ConfigureBooksColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("BookID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название", Binding = new Binding("Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Автор", Binding = new Binding("Author"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Издательство", Binding = new Binding("Publisher"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Год", Binding = new Binding("YearPublished"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "ISBN", Binding = new Binding("ISBN"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Количество", Binding = new Binding("Quantity"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Категория", Binding = new Binding("Category.CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }


        private void ConfigureLoansColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "BookID", Binding = new Binding("Book.Title"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя", Binding = new Binding("Student.FirstName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия", Binding = new Binding("Student.LastName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда взяли", Binding = new Binding("LoanDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда должны вернуть", Binding = new Binding("DueDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Подтверждение возврата", Binding = new Binding("ReturnDate"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var student = new Student();
            EditWindow ew = new EditWindow(student);
            var result = ew.ShowDialog();
            if (result == true)
            {
                context.Students.Add(student);
                context.SaveChanges();
                ew.Close();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Вы уверены?", "Удалить запись", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Student student = dGrid.SelectedItem as Student;
                    context.Students.Remove(student);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так!!!\n" + ex.Message);
            }
        }

        /* private void btnEdit_Click(object sender, RoutedEventArgs e)
         {
             Student student = dGrid.SelectedItem as Student;

             EditWindow ew = new EditWindow(student);
             var result = ew.ShowDialog();
             if (result == true)
             {
                 context.SaveChanges();
                 ew.Close();
             }
             else
             {
                 context.Entry(student).Reload();
                 dGrid.DataContext = null;
                 dGrid.DataContext = context.Students.Local;
             }
         }*/


        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, есть ли выбранный элемент
            if (dGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите студента для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Преобразуем выбранный элемент в тип Student
            Student student = dGrid.SelectedItem as Student;

            // Проверяем, удалось ли преобразование
            if (student == null)
            {
                MessageBox.Show("Не удалось получить данные выбранного студента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создаем и показываем окно редактирования
            EditWindow ew = new EditWindow(student);
            var result = ew.ShowDialog();

            // Обрабатываем результат работы окна редактирования
            if (result == true)
            {
                context.SaveChanges();
                // Reload the updated data from the database
                context.Entry(student).Reload();
                // Update the DataGrid's DataContext
                dGrid.ItemsSource = null;
                dGrid.ItemsSource = context.Students.Local;
            }
            else
            {
                context.Entry(student).Reload();
                dGrid.DataContext = null;
                dGrid.DataContext = context.Students.Local;
            }

            // Закрываем окно редактирования
            ew.Close();
        }


        private void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void btnListCategories(object sender, RoutedEventArgs e)
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

        private void btnListStudents(object sender, RoutedEventArgs e)
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

        private void btnListBooks(object sender, RoutedEventArgs e)
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

        public void DisplaySearchResults(List<Book> books)
        {
            dGrid.ItemsSource = books;
        }

        private void btnSearchBooks_Click(object sender, RoutedEventArgs e)
        {
            var searchBooksWindow = new SearchBooksWindow(this);
            searchBooksWindow.Show();

        }
        #region
        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {

            AddBookDialog dialog = new AddBookDialog(context);
            dialog.ShowDialog();
            // Обновление данных в DataGrid после добавления новой книги
            dGrid.DataContext = null;
            dGrid.DataContext = context.Books.ToList();

            // Add functionality to add a new book here
            /* dGrid.ItemsSource = null;
             context.Books.Load();
             var bookList = context.Books.ToList();
             ConfigureBooksColumns();
             dGrid.ItemsSource = bookList.ToString();*/
        }

        private void btnDeleteBook_Click(object sender, RoutedEventArgs e)
        {
            // Add functionality to delete a book here
        }

        private void btnSearchStudents_Click(object sender, RoutedEventArgs e)
        {
            // Add search functionality here
        }

        private void btnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            // Add functionality to add a new student here
        }

        private void btnDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            // Add functionality to delete a student here
        }

        private void btnMovementByBook_Click(object sender, RoutedEventArgs e)
        {
            // Add functionality for book movement here
        }

        private void btnMovementByStudent_Click(object sender, RoutedEventArgs e)
        {
            // Add functionality for student movement here
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            // Add help functionality here
        }



        private void btnListStudents_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        private void btnBookReport_Click(object sender, RoutedEventArgs e)
        {
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

        /*
        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            AddBookDialog dialog = new AddBookDialog(context);
            dialog.ShowDialog();
            // Обновление данных в DataGrid после добавления новой книги
            dGrid.DataContext = null;
            dGrid.DataContext = context.Books.ToList();
        }*/
    }
}


