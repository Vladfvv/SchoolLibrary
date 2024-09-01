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

namespace SchoolLibrary.DialogWindows.Operations
{
    /// <summary>
    /// Логика взаимодействия для CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        private string tag;  // Переменная для хранения значения Tag

        public CustomMessageBox(string message, string dataGridTag)
        {
            InitializeComponent();
            MessageText.Text = message; // Устанавливаем текст сообщения
            tag = dataGridTag; // Сохраняем значение Tag, переданное из DataGrid
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (tag == "Option1")
            {
                // Действие для первой кнопки, если Tag = "Option1"
                MessageBox.Show("Вы выбрали действие 1 для Option1.");
            }
            else if (tag == "Option2")
            {
                // Другое действие для первой кнопки, если Tag = "Option2"
                MessageBox.Show("Вы выбрали действие 1 для Option2.");
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (tag == "Option1")
            {
                // Действие для второй кнопки, если Tag = "Option1"
                MessageBox.Show("Вы выбрали действие 2 для Option1.");
            }
            else if (tag == "Option2")
            {
                // Другое действие для второй кнопки, если Tag = "Option2"
                MessageBox.Show("Вы выбрали действие 2 для Option2.");
            }
            this.DialogResult = false;
            this.Close();
        }
    }
}
