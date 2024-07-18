using SchoolLibrary.Models;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SchoolLibrary.AuthWindows
{
    /// <summary>
    /// Логика взаимодействия для WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Начальные размеры окна
            this.Width = 200;
            this.Height = 150;

            // Анимация увеличения окна
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 200,
                To = 600,
                Duration = TimeSpan.FromSeconds(1)
            };
            DoubleAnimation heightAnimation = new DoubleAnimation
            {
                From = 150,
                To = 400,
                Duration = TimeSpan.FromSeconds(1)
            };

            // Запуск анимации
            this.BeginAnimation(Window.WidthProperty, widthAnimation);
            this.BeginAnimation(Window.HeightProperty, heightAnimation);

            // Таймер для автоматического закрытия окна через 5 секунд
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(7)
            };
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
            // Открытие соответствующего окна в зависимости от роли
            switch (UserSession.Role)
            {
                case "Администратор":
                    MainWindowAdmin mainWindowAdmin = new MainWindowAdmin();
                    mainWindowAdmin.Show();
                    break;
                case "Библиотекарь":
                    MainWindowLibrarian mainWindowLibrarian = new MainWindowLibrarian();
                    mainWindowLibrarian.Show();
                    break;
                case "Читатель":
                    MainWindowUser mainWindowUser = new MainWindowUser();
                    mainWindowUser.Show();
                    break;
                default:
                    MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            this.Close();
        }
    }
}
