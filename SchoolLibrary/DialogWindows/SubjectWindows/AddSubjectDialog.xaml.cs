using SchoolLibrary.Models;
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
using System.Windows.Shapes;

namespace SchoolLibrary.DialogWindows.SubjectWindows
{
    /// <summary>
    /// Логика взаимодействия для AddSubjectDialog.xaml
    /// </summary>
    public partial class AddSubjectDialog : Window
    {
        private readonly EntityContext context;

        public AddSubjectDialog(EntityContext dbContext)
        {
            //InitializeComponent();
            //this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //context = dbContext;
            //cmbGenre.ItemsSource = context.Genres.ToList();
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            context = dbContext;

            // Проверка количества жанров в базе данных
            var genres = context.Genres.ToList();
            if (genres.Count == 0)
            {
                MessageBox.Show("Нет доступных жанров в базе данных.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                cmbGenre.ItemsSource = genres;
            }
        }

        private void AddSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var subjectName = txtSubjectName.Text;
                var selectedGenre = (Genre)cmbGenre.SelectedItem;

                if (string.IsNullOrWhiteSpace(subjectName) || selectedGenre == null)
                {
                    MessageBox.Show("Заполните все поля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var existingSubject = context.Subjects.FirstOrDefault(s => s.SubjectName == subjectName && s.GenreID == selectedGenre.GenreID);
                if (existingSubject != null)
                {
                    MessageBox.Show("Такой предмет уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newSubject = new Subject { SubjectName = subjectName, Genre = selectedGenre };
                context.Subjects.Add(newSubject);
                context.SaveChanges();

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
       
    }
}
