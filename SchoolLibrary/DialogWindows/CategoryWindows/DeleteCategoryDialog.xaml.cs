using SchoolLibrary.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SchoolLibrary.DialogWindows.CategoryWindows
{
    public partial class DeleteCategoryDialog : Window
    {
        private readonly EntityContext _context;
        private readonly Category _category;

        public DeleteCategoryDialog(EntityContext context, Category category)
        {
            InitializeComponent();
            _context = context;
            _category = category;

            // Заполняем информацию о категории
            txtCategoryInfo.Text = $"Название категории: {_category.CategoryName}";
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, есть ли книги с удаляемой категорией
                var booksWithCategory = _context.Books.Any(b => b.CategoryID == _category.CategoryID);

                if (booksWithCategory)
                {
                    MessageBox.Show("Невозможно удалить категорию, так как существуют книги с этой категорией.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Удаляем категорию из контекста базы данных
                    _context.Categories.Remove(_category);
                    _context.SaveChanges();

                    // Закрываем диалоговое окно с результатом DialogResult = true
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении категории: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалоговое окно с результатом DialogResult = false
            DialogResult = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
