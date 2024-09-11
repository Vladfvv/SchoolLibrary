using SchoolLibrary.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolLibrary.DialogWindows.CategoryWindows
{
    public partial class AddCategoryDialog : Window
    {
        private readonly EntityContext context;
        private readonly Category newCategory;

        public AddCategoryDialog(EntityContext dbContext)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            context = dbContext;
            newCategory = new Category();
            DataContext = newCategory;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                // Валидация
                txtCategoryName.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                // Проверка категории если сущнствует в БД
                var existingCategory = context.Categories.FirstOrDefault(c => c.CategoryName == newCategory.CategoryName);
                if (existingCategory != null)
                {
                    MessageBox.Show("Такая категория уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Выход без добавления если категория существует
                }

                // Добавляем категорию, сохраняем изменения
                context.Categories.Add(newCategory);
                context.SaveChanges();

                // Закрываем диалог с результатом true
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем диалог с результатом false
            DialogResult = false;
        }

        
    }
}
