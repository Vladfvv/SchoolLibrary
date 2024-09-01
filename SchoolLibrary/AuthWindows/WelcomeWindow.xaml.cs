//using SchoolLibrary.DialogWindowsLibrarian;
//using SchoolLibrary.Models;
//using System;
//using System.Windows;
//using System.Windows.Media.Animation;
//using System.Windows.Threading;

//namespace SchoolLibrary.AuthWindows
//{
//    / <summary>
//    / Логика взаимодействия для WelcomeWindow.xaml
//    / </summary>
//    public partial class WelcomeWindow : Window
//    {
//        public WelcomeWindow()
//        {
//            InitializeComponent();
//        }

//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {
//            Начальные размеры окна
//            this.Width = 200;
//            this.Height = 150;

//            Анимация увеличения окна
//           DoubleAnimation widthAnimation = new DoubleAnimation
//           {
//               From = 200,
//               To = 600,
//               Duration = TimeSpan.FromSeconds(1)
//           };
//            DoubleAnimation heightAnimation = new DoubleAnimation
//            {
//                From = 150,
//                To = 400,
//                Duration = TimeSpan.FromSeconds(1)
//            };

//            Запуск анимации
//            this.BeginAnimation(Window.WidthProperty, widthAnimation);
//            this.BeginAnimation(Window.HeightProperty, heightAnimation);

//            Таймер для автоматического закрытия окна через 5 секунд
//           DispatcherTimer timer = new DispatcherTimer
//           {
//               Interval = TimeSpan.FromSeconds(7)
//           };
//            timer.Tick += Timer_Tick;
//            timer.Start();
//        }

//        private void Timer_Tick(object sender, EventArgs e)
//        {
//            ((DispatcherTimer)sender).Stop();
//            OpenRoleBasedWindow();
//        }

//        private void OpenRoleBasedWindow()
//        {
//            Открытие соответствующего окна в зависимости от роли
//            switch (UserSession.Role)
//            {
//                case "Администратор":
//                    MainWindowAdmin mainWindowAdmin = new MainWindowAdmin();
//                    MainWindowAdmin mainWindowAdmin = new MainWindowAdmin();
//                    mainWindowAdmin.Show();
//                    break;
//                case "Библиотекарь":
//                    MainWindowLibrarian mainWindowLibrarian = new MainWindowLibrarian();
//                    StartWindowLibrarian startWindowLibrarian = new StartWindowLibrarian();
//                    startWindowLibrarian.Show();
//                    break;
//                case "Читатель":
//                    MainWindowUser mainWindowUser = new MainWindowUser();
//                    mainWindowUser.Show();
//                    break;
//                default:
//                    MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//                    break;
//            }

//            this.Close();
//        }
//    }
//}


using SchoolLibrary.DataLoaders;
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
        private EntityContext context;
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

        //private void OpenRoleBasedWindow()
        //{
        //    string role = UserSession.Role; // Предположим, UserSession хранит текущую роль
        //    EntityContext context = new EntityContext(); // или получаем его через DI

        //    DataLoader dataLoader = role switch
        //    {
        //        "Администратор" => new AdminDataLoader(context),
        //        "Библиотекарь" => new LibrarianDataLoader(context),
        //        "Читатель" => new UserDataLoader(context),
        //        _ => throw new ArgumentException("Недопустимая роль", nameof(role))
        //    };

        //    Window window = role switch
        //    {
        //        "Администратор" => new StartAdminWindow(dataLoader),
        //        "Библиотекарь" => new StartLibrarianWindow(dataLoader),
        //        "Читатель" => new StartUserWindow(dataLoader),
        //        _ => throw new ArgumentException("Недопустимая роль", nameof(role))
        //    };

        //    window.Show();
        //    this.Close();
        //}

        private void OpenRoleBasedWindow()
        {
            string role = UserSession.Role; // Предположим, UserSession хранит текущую роль
           // EntityContext context = new EntityContext(dbContext); // или получаем его через DI

            DataLoader dataLoader;
            switch (role)
            {
                case "Администратор":
                    dataLoader = new AdminDataLoader(context);                    
                    break;
                case "Библиотекарь":
                    dataLoader = new LibrarianDataLoader(context);                    
                    break;
                case "Читатель":
                    dataLoader = new UserDataLoader(context);                   
                    break;
                default:
                    throw new ArgumentException("Недопустимая роль", nameof(role));
            }

            Window window;
            // Метод обновления классов студентов - при наступлении 1 сентября происходит перевод
            studentService = new StudentService(context);
            DateTime today = DateTime.Today;
            if (today.Month == 9 && today.Day == 1) studentService.UpdateStudentClasses();

            switch (role) 
            {
                case "Администратор":
                   // window = new StartAdminWindow(dataLoader);
                    window = new StartAdminWindow(context);
                    this.Close();
                    break;
                case "Библиотекарь":
                    //window = new StartLibrarianWindow(dataLoader);
                    window = new StartLibrarianWindow(context);
                    this.Close();
                    break;
                case "Читатель":
                    window = new StartUserWindow(context);
                    this.Close();
                    break;
                default:
                    throw new ArgumentException("Недопустимая роль", nameof(role));
            }

            window.Show();
            this.Close();
        }

    }
}
