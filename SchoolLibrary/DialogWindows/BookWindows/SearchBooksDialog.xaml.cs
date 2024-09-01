using SchoolLibrary.ViewModels;
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
            var genres = _context.Genres.ToList();

            //CategoryComboBox.ItemsSource = categories;
            GenreComboBox.ItemsSource = genres;
            GenreComboBox.DisplayMemberPath = "GenreName"; // Укажите свойство для отображения
            GenreComboBox.SelectedValuePath = "GenreID";
        }

        private void LoadSubjects(int genreId)
        {
            using (var context = new EntityContext("SchoolLibrary"))
            {
                // Убедитесь, что у вас есть таблица Subjects в контексте Entity Framework
                var subjects = context.Subjects
                                      .Where(s => s.GenreID == genreId)
                                      .ToList();

                // Проверка на наличие данных
                if (subjects.Any())
                {
                    SubjectComboBox.ItemsSource = subjects;
                    SubjectComboBox.DisplayMemberPath = "SubjectName";
                    SubjectComboBox.SelectedValuePath = "SubjectID";
                }
                else
                {
                    SubjectComboBox.ItemsSource = null; // Очистите ComboBox, если нет данных
                }
            }
        }


        // Пример инициализации ComboBox жанров
        private void LoadGenres()
        {
            using (var context = new EntityContext("SchoolLibrary"))
            {
                var genres = context.Genres.ToList();
                GenreComboBox.ItemsSource = genres;
                GenreComboBox.DisplayMemberPath = "GenreName";
                GenreComboBox.SelectedValuePath = "GenreID";
            }
        }

        private void GenreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GenreComboBox.SelectedItem != null)
            {
                var selectedGenre = (Genre)GenreComboBox.SelectedItem;

                if (selectedGenre.IsEducationalLiterature())
                {
                    SubjectLabel.Visibility = Visibility.Visible;
                    SubjectComboBox.Visibility = Visibility.Visible;
                    var genreId = selectedGenre.GenreID;
                    LoadSubjects(genreId);
                }
                else
                {
                    SubjectLabel.Visibility = Visibility.Collapsed;
                    SubjectComboBox.Visibility = Visibility.Collapsed;
                    SubjectComboBox.ItemsSource = null; // Очистите ComboBox, если не учебная литература
                }
            }
        }





        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string title = TitleTextBox.Text;
        //    string author = AuthorTextBox.Text;
        //    string publisher = PublisherTextBox.Text;
        //    string description = DescriptionTextBox.Text;
        //    bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
        //    bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
        //    string isbn = ISBNTextBox.Text;
        //    Genre selectedCategory = GenreComboBox.SelectedItem as Genre;

        //    // Получаем данные из базы данных
        //    var query = _context.InventoryBooks
        //        .Include(ib => ib.Book)
        //        .Include(ib => ib.Book.Genre)
        //        .Include(ib => ib.Loans)
        //        .AsQueryable();

        //    // Применяем фильтрацию
        //    if (!string.IsNullOrWhiteSpace(title))
        //    {
        //        query = query.Where(ib => ib.Title.Contains(title));
        //    }

        //    if (!string.IsNullOrWhiteSpace(author))
        //    {
        //        query = query.Where(ib => ib.Author.Contains(author));
        //    }

        //    if (!string.IsNullOrWhiteSpace(publisher))
        //    {
        //        query = query.Where(ib => ib.Publisher.Contains(publisher));
        //    }

        //    if (!string.IsNullOrWhiteSpace(description))
        //    {
        //        query = query.Where(ib => ib.Book.Description.Contains(description));
        //    }

        //    if (yearFromParsed)
        //    {
        //        // Преобразуем год в строку и применяем фильтрацию
        //        string yearFromStr = yearFrom.ToString();
        //        query = query.Where(ib => string.Compare(ib.YearPublished, yearFromStr) >= 0);
        //    }

        //    if (yearToParsed)
        //    {
        //        // Преобразуем год в строку и применяем фильтрацию
        //        string yearToStr = yearTo.ToString();
        //        query = query.Where(ib => string.Compare(ib.YearPublished, yearToStr) <= 0);
        //    }

        //    if (!string.IsNullOrWhiteSpace(isbn))
        //    {
        //        query = query.Where(ib => ib.ISBN.Contains(isbn));
        //    }

        //    if (selectedCategory != null)
        //    {
        //        query = query.Where(ib => ib.Book.GenreID == selectedCategory.GenreID);
        //    }

        //    var inventoryBooks = query.ToList();

        //    List<PaginatedBookInventoryModel> results;

        //    if (GroupByIsbnRadioButton.IsChecked == true)
        //    {
        //        results = inventoryBooks
        //            .GroupBy(ib => ib.ISBN)
        //            .Select((g, index) => new PaginatedBookInventoryModel
        //            {
        //                Index = index + 1,
        //                BookID = g.First().Book.BookID,
        //                Title = g.First().Title,
        //                Author = g.First().Author,
        //                Publisher = g.First().Publisher,
        //                YearPublished = g.First().YearPublished,
        //                ISBN = g.Key,
        //                Quantity = g.First().Book.Quantity,
        //                QuantityLeft = g.First().Book.QuantityLeft,
        //                CategoryName = g.First().Book.Genre.GenreName
        //            })
        //            .ToList();
        //    }
        //    else
        //    {
        //        results = inventoryBooks
        //            .Select((ib, index) => new PaginatedBookInventoryModel
        //            {
        //                Index = index + 1,
        //                BookID = ib.Book.BookID,
        //                InventoryBookID = ib.InventoryBookID,
        //                Title = ib.Title,
        //                Author = ib.Author,
        //                Publisher = ib.Publisher,
        //                YearPublished = ib.YearPublished,
        //                ISBN = ib.ISBN,
        //                Quantity = 1,
        //                QuantityLeft = ib.Loans.Any(loan => !loan.Returned) ? 0 : 1,
        //                CategoryName = ib.Book.Genre.GenreName
        //            })
        //            .ToList();
        //    }

        //    if (results.Any())
        //    {
        //        string message;
        //        int count = results.Count;

        //        if (count == 1)
        //        {
        //            message = "1 книга найдена.";
        //        }
        //        else if (count > 1 && count < 5)
        //        {
        //            message = $"{count} книги найдены.";
        //        }
        //        else
        //        {
        //            message = $"{count} книг найдено.";
        //        }

        //        MessageBox.Show(message, "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
        //        this.DialogResult = true;
        //        this.Tag = results;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Нет книг по данным критериям поиска.", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
        //        this.DialogResult = false;
        //    }
        //}

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string author = AuthorTextBox.Text;
            string publisher = PublisherTextBox.Text;
            string description = DescriptionTextBox.Text;
            bool yearFromParsed = int.TryParse(YearFromTextBox.Text, out int yearFrom);
            bool yearToParsed = int.TryParse(YearToTextBox.Text, out int yearTo);
            string isbn = ISBNTextBox.Text;
            Genre selectedGenre = GenreComboBox.SelectedItem as Genre;
            Subject selectedSubject = SubjectComboBox.SelectedItem as Subject;

            // Получаем данные из базы данных
            var query = _context.InventoryBooks
                .Include(ib => ib.Book)
                .Include(ib => ib.Book.Genre)
                .Include(ib => ib.Loans)
                .AsQueryable();

            // Применяем фильтрацию
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

            if (!string.IsNullOrWhiteSpace(description))
            {
                query = query.Where(ib => ib.Book.Description.Contains(description));
            }

            if (yearFromParsed)
            {
                string yearFromStr = yearFrom.ToString();
                query = query.Where(ib => string.Compare(ib.YearPublished, yearFromStr) >= 0);
            }

            if (yearToParsed)
            {
                string yearToStr = yearTo.ToString();
                query = query.Where(ib => string.Compare(ib.YearPublished, yearToStr) <= 0);
            }

            if (!string.IsNullOrWhiteSpace(isbn))
            {
                query = query.Where(ib => ib.ISBN.Contains(isbn));
            }

            if (selectedGenre != null)
            {
                query = query.Where(ib => ib.Book.GenreID == selectedGenre.GenreID);
            }

            if (selectedGenre != null && selectedGenre.IsEducationalLiterature() && selectedSubject != null)
            {
                // Фильтрация по предмету
                query = query.Where(ib => ib.Book.SubjectID == selectedSubject.SubjectID);

                // Подсчет количества книг по предмету
                int countBySubject = query.Count();

                if (countBySubject > 0)
                {
                    MessageBox.Show($"Найдено {countBySubject} книг по предмету: {selectedSubject.SubjectName}", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Нет книг по выбранному предмету.", "Результат поиска", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = false;
                    return;
                }
            }

            var inventoryBooks = query.ToList();

            List<PaginatedBookInventoryModel> results;

            if (GroupByIsbnRadioButton.IsChecked == true)
            {
                results = inventoryBooks
                    .GroupBy(ib => ib.ISBN)
                    .Select((g, index) => new PaginatedBookInventoryModel
                    {
                        Index = index + 1,
                        BookID = g.First().Book.BookID,
                        Title = g.First().Title,
                        Author = g.First().Author,
                        Publisher = g.First().Publisher,
                        YearPublished = g.First().YearPublished,
                        ISBN = g.Key,
                        Quantity = g.First().Book.Quantity,
                        QuantityLeft = g.First().Book.QuantityLeft,
                        CategoryName = g.First().Book.Genre.GenreName
                    })
                    .ToList();
            }
            else
            {
                results = inventoryBooks
                    .Select((ib, index) => new PaginatedBookInventoryModel
                    {
                        Index = index + 1,
                        BookID = ib.Book.BookID,
                        InventoryBookID = ib.InventoryBookID,
                        Title = ib.Title,
                        Author = ib.Author,
                        Publisher = ib.Publisher,
                        YearPublished = ib.YearPublished,
                        ISBN = ib.ISBN,
                        Quantity = 1,
                        QuantityLeft = ib.Loans.Any(loan => !loan.Returned) ? 0 : 1,
                        CategoryName = ib.Book.Genre.GenreName
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

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        this.DragMove();
        //    }
        //}
    }
}
