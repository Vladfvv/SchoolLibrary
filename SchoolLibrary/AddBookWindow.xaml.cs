
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
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {/*
        private readonly DatabaseHelper _databaseHelper;

        public AddBookWindow(DatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var book = new Book
            {
                Title = TitleTextBox.Text,
                Author = AuthorTextBox.Text,
                Publisher = PublisherTextBox.Text,
                YearPublished = int.Parse(YearPublishedTextBox.Text),
                ISBN = ISBNTextBox.Text,
                Quantity = int.Parse(QuantityTextBox.Text)
            };

            _databaseHelper.AddBook(book);
            Close();
        }*/
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
