//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace SchoolLibrary.AuthWindows
//{
//    /// <summary>
//    /// Логика взаимодействия для LoginWindow.xaml
//    /// </summary>
//    public partial class LoginWindow : Window
//    {
//        private EntityContext context;

//        public LoginWindow(EntityContext dbContext)
//        {
//            InitializeComponent();
//            this.context = dbContext;
//        }

//        private void LoginButton_Click(object sender, RoutedEventArgs e)
//        {
//            string username = UsernameTextBox.Text;
//            string password = PasswordBox.Password;

//            // Находим пользователя в базе данных по логину
//            var user = context.Users.SingleOrDefault(u => u.Username == username);

//            if (user != null)
//            {
//                // Проверяем пароль
//                if (VerifyPassword(password, user.PasswordHash, user.Salt))
//                {
//                    // Получаем роль пользователя
//                    string role = user.Role;

//                    // Открываем соответствующее окно в зависимости от роли
//                    switch (role)
//                    {
//                        case "Администратор":
//                            MainWindowAdmin mainWindowAdmin = new MainWindowAdmin();
//                            mainWindowAdmin.Show();
//                            break;
//                        case "Библиотекарь":
//                            MainWindowLibrarian mainWindowLibrarian = new MainWindowLibrarian();
//                            mainWindowLibrarian.Show();
//                            break;
//                        case "Читатель":
//                            MainWindowUser mainWindowUser = new MainWindowUser();
//                            mainWindowUser.Show();
//                            break;
//                        default:
//                            MessageBox.Show("Неверная роль пользователя.");
//                            break;
//                    }
//                    this.Close(); // Закрываем окно входа
//                }
//                else
//                {
//                    MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Пользователь с таким именем не найден.\nПожалуйста, зарегистрируйтесь.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }

//        private bool VerifyPassword(string password, byte[] storedHash, byte[] salt)
//        {
//            using (var sha256 = SHA256.Create())
//            {
//                byte[] combinedBytes = salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();
//                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
//                return hashBytes.SequenceEqual(storedHash);
//            }
//        }

//        private void RegisterButton_Click(object sender, RoutedEventArgs e)
//        {
//            var registerWindow = new RegisterWindow(context);
//            registerWindow.Show();
//            this.Close();
//        }

//    }
//}

using SchoolLibrary.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace SchoolLibrary.AuthWindows
{
    public partial class LoginWindow : Window
    {
        private EntityContext context;

        public LoginWindow(EntityContext dbContext)
        {
            InitializeComponent();
            this.context = dbContext;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Поиск пользователя в базе данных по имени пользователя
            var user = context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                MessageBox.Show("Пользователь с таким именем не найден.\nПожалуйста, зарегистрируйтесь.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка пароля
            if (!VerifyPassword(user, password))
            {
                MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Сохранение информации о пользователе в сессии
            UserSession.UserId = user.Id;
            UserSession.Username = user.Username;
            UserSession.Role = user.Role;


            //// Получение роли пользователя
            //string role = GetUserRole(user);

            //// Открытие соответствующего окна в зависимости от роли
            //switch (role)
            //{
            //    case "Администратор":
            //        MainWindowAdmin mainWindowAdmin = new MainWindowAdmin();
            //        mainWindowAdmin.Show();
            //        break;
            //    case "Библиотекарь":
            //        MainWindowLibrarian mainWindowLibrarian = new MainWindowLibrarian();
            //        mainWindowLibrarian.Show();
            //        break;
            //    case "Читатель":
            //        MainWindowUser mainWindowUser = new MainWindowUser();
            //        mainWindowUser.Show();
            //        break;
            //    default:
            //        MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //        break;
            //}

            WelcomeWindow welcomeWindow = new WelcomeWindow();  
            welcomeWindow.Show();
            this.Close();
        }

        private bool VerifyPassword(User user, string password)
        {
            // Хэширование введенного пароля с использованием соли из базы данных
            byte[] hashedPasswordBytes;
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = user.Salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();
                hashedPasswordBytes = sha256.ComputeHash(combinedBytes);
            }

            // Сравнение хэшей
            return hashedPasswordBytes.SequenceEqual(user.PasswordHash);
        }

        private string GetUserRole(User user)
        {
            // Возвращаем роль пользователя из базы данных
            return user.Role;
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(context);
            registerWindow.Show();
            this.Close();
        }
    }
}

