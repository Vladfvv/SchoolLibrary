using SchoolLibrary.Models;
using SchoolLibrary.Service;
using SchoolLibrary.Views;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SchoolLibrary.AuthWindows
{
    public partial class WelcomeWindow : Window
    {
        private StudentService studentService;
        private readonly EntityContext context;
        public WelcomeWindow(EntityContext dbContext)
        {
            InitializeComponent();
            this.context = dbContext;
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

            // Таймер для автоматического закрытия окна через 7 секунд
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
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
            string role = UserSession.Role; // Предположим, UserSession хранит текущую роль                                

            Window window;
            // Метод обновления классов студентов - при наступлении 1 сентября происходит перевод
            studentService = new StudentService(context);
            DateTime today = DateTime.Today;
            if (today.Month == 9 && today.Day == 1) studentService.UpdateStudentClasses();

            switch (role) 
            {
                case "Администратор":                   
                    window = new StartAdminWindow(context);
                    //this.Close();
                    break;
                case "Библиотекарь":                   
                    window = new StartLibrarianWindow(context);
                   // this.Close();
                    break;
                case "Читатель":
                    window = new StartUserWindow(context);
                    //this.Close();
                    break;
                default:
                    throw new ArgumentException("Недопустимая роль", nameof(role));
            }

            window.Show();
            this.Close();
        }

    }
}
