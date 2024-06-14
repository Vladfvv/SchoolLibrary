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

namespace SchoolLibrary
{
    /// <summary>
    /// Логика взаимодействия для AddBookDialog.xaml
    /// </summary>
    public partial class AddBookDialog : Window
    {
        public readonly EntityContext context;

        public AddBookDialog(EntityContext dbContext)
        {
            InitializeComponent();
            context = dbContext;
            LoadCategories();
        }

        private void LoadCategories()
        {
            // Загрузка категорий из базы данных в ComboBox
            cmbCategory.ItemsSource = context.Categories.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Создание новой книги и добавление её в контекст данных
            Book newBook = new Book
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Publisher = txtPublisher.Text,
                YearPublished = txtYear.Text,
                ISBN = txtISBN.Text,
                Quantity = int.Parse(txtQuantity.Text),
                Category = cmbCategory.SelectedItem as Category
            };

            context.Books.Add(newBook);
            context.SaveChanges(); // Сохранение изменений в базу данных
            MessageBox.Show("Book added successfully!");
            this.Close();
        }
    }
}
