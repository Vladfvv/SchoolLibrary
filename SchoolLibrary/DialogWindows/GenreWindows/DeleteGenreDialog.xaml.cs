using SchoolLibrary.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SchoolLibrary.DialogWindows.GenreWindows
{
    public partial class DeleteGenreDialog : Window
    {
        private readonly EntityContext _context;
        private readonly Genre _genre;

        public DeleteGenreDialog(EntityContext context, Genre genre)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _context = context;
            _genre = genre;

            // Заполняем информацию о жанре
            txtGenreInfo.Text = $"Название жанра: {_genre.GenreName}";
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, есть ли книги с удаляемым жанром
                var booksWithGenre = _context.Books.Any(b => b.Genre.GenreID == _genre.GenreID);

                if (booksWithGenre)
                {
                    MessageBox.Show("Невозможно удалить жанр, так как существуют книги с этим жанром.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Удаляем жанр из контекста базы данных
                    _context.Genres.Remove(_genre);
                    _context.SaveChanges();

                    // Закрываем диалоговое окно с результатом DialogResult = true
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении жанра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалоговое окно с результатом DialogResult = false
            DialogResult = false;
        }
    }
}
