using SchoolLibrary.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SchoolLibrary.DialogWindows.GenreWindows
{
    public partial class EditGenreDialog : Window, INotifyPropertyChanged
    {
        private readonly EntityContext _context;
        private Genre _genre;

        public Genre Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnPropertyChanged();
            }
        }

        public EditGenreDialog(EntityContext context, Genre genre)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = this;
            _context = context;
            Genre = genre;
        }

        private void SaveGenre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Сохраняем изменения в базе данных
                _context.SaveChanges();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения жанра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
             

       
    }
}
