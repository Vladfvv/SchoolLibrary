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
                // Force validation
                txtCategoryName.GetBindingExpression(TextBox.TextProperty).UpdateSource();

                // Check if the category already exists in the database
                var existingCategory = context.Categories.FirstOrDefault(c => c.CategoryName == newCategory.CategoryName);
                if (existingCategory != null)
                {
                    MessageBox.Show("Такая категория уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Exit without adding if category already exists
                }

                // Add category to database context and save changes
                context.Categories.Add(newCategory);
                context.SaveChanges();

                // Close dialog with DialogResult set to true
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Close dialog with DialogResult set to false
            DialogResult = false;
        }

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        DragMove();
        //    }
        //}
    }
}
