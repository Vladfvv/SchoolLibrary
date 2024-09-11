using SchoolLibrary.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SchoolLibrary.DialogWindows.GenreWindows
{
    public partial class AddGenreDialog : Window
    {
        private readonly EntityContext context;
        public Genre newGenre;
        public ObservableCollection<Subject> Subjects { get; set; }
        public Subject SelectedSubject { get; set; }
        public Visibility SubjectSelectionVisibility { get; set; }

        public AddGenreDialog(EntityContext dbContext)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            context = dbContext;
            newGenre = new Genre();
            DataContext = this;

            // Инициализируем Subjects и привязку к жанру
            Subjects = new ObservableCollection<Subject>(context.Subjects.ToList());
            SubjectSelectionVisibility = Visibility.Collapsed;
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновляем привязку данных
                txtGenreName.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                newGenre.GenreName = txtGenreName.Text;
                // Проверяем наличие жанра в базе данных
                var existingGenre = context.Genres.FirstOrDefault(g => g.GenreName == newGenre.GenreName);
                if (existingGenre != null)
                {
                    MessageBox.Show("Такой жанр уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);

                    // Сбросить состояние newGenre, чтобы можно было вводить новое значение
                    newGenre.GenreName = string.Empty;
                    txtGenreName.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                    return;
                }

                // Если выбран жанр "Учебная литература", устанавливаем выбранный предмет
                if (newGenre.IsEducationalLiterature() && SelectedSubject != null)
                {
                    newGenre.Subjects.Add(SelectedSubject);
                }

                // Если жанр не "Учебная литература", добавляем "Без предмета"
                if (!newGenre.IsEducationalLiterature())
                {
                    newGenre.Subjects.Add(new Subject { SubjectName = "Без предмета" });
                }

                // Добавляем новый жанр в контекст базы данных и сохраняем изменения
                context.Genres.Add(newGenre);
                context.SaveChanges();

                // Закрываем окно с положительным результатом
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false; // Установить false при ошибке
            }
        }
          

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрыть окно с отрицательным результатом
            DialogResult = false;
        }

        private void txtGenreName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Отображаем или скрываем выбор предмета в зависимости от жанра
            SubjectSelectionVisibility = newGenre.IsEducationalLiterature() ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}