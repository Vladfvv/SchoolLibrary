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
using System.Windows.Threading;

namespace SchoolLibrary.AuthWindows
{
    /// <summary>
    /// Логика взаимодействия для WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        //автоматически закрывать приветственное окно через 3 секунды и открывать соответствующее окно в зависимости от роли пользователя
        public WelcomeWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            OpenRoleBasedWindow();
        }

        private void OpenRoleBasedWindow()
        {
            // Проверка роли пользователя и открытие соответствующего окна
            string role = GetUserRole(); //  метод для получения роли пользователя

            switch (role)
            {
                case "Administrator":
                    new MainWindowAdmin().Show();
                    break;
                case "Librarian":
                    new MainWindowLibrarian().Show();
                    break;
                case "User":
                    new MainWindowUser().Show();
                    break;
                default:
                    MessageBox.Show("Неизвестная роль пользователя.");
                    break;
            }

            this.Close();
        }

        private string GetUserRole()
        {
            // Здесь нужно реализовать логику для получения роли пользователя, например, из базы данных
            return "User"; // Для примера возвращаем роль User
        }
    }
}
