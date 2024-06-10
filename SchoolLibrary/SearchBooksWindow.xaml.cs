using EntityProject;
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
    /// Логика взаимодействия для SearchBooksWindow.xaml
    /// </summary>
    public partial class SearchBooksWindow : Window
    {



        private MainWindow mainWindow;

        public SearchBooksWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string author = txtAuthor.Text;
            string title = txtTitle.Text;
            string publisher = txtPublisher.Text;

            using (var context = new EntityContext("SchoolLibrary"))
            {
                var query = context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(author))
                {
                    query = query.Where(b => b.Author.Contains(author));
                }

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(b => b.Title.Contains(title));
                }

                if (!string.IsNullOrEmpty(publisher))
                {
                    query = query.Where(b => b.Publisher.Contains(publisher));
                }

                var result = query.ToList();
                mainWindow.DisplaySearchResults(result);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
