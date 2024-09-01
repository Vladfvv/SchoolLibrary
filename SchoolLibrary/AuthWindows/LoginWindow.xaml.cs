using SchoolLibrary.Models;
using SchoolLibrary.Views;
using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SchoolLibrary.AuthWindows
{
    public partial class LoginWindow : Window
    {
        private EntityContext context;

        public LoginWindow(EntityContext dbContext)
        {
            InitializeComponent();
            this.context = dbContext;
            //this.MouseDown += Window_MouseDown;
        }

        //Метод Window_Loaded устанавливает фокус на текстовое поле имени пользователя при загрузке окна. - костыль от необходимости двойного нажатия

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameTextBox.Focus(); // Установить фокус на текстовое поле имени пользователя
        }

        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string username = UsernameTextBox.Text;
        //    string password = PasswordBox.Password;

        //    // Проверка на заполненность полей
        //    if (string.IsNullOrWhiteSpace(username))
        //    {
        //        MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(password))
        //    {
        //        MessageBox.Show("Пароль не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    // Поиск пользователя в базе данных по имени пользователя
        //    var user = context.Users.SingleOrDefault(u => u.Username == username);
        //    if (user == null)
        //    {
        //        MessageBox.Show("Пользователь с таким именем не найден.\nПожалуйста, зарегистрируйтесь.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    // Проверка пароля
        //    if (!VerifyPassword(user, password))
        //    {
        //        MessageBox.Show("Неверный пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //    // Сохранение информации о пользователе в сессии
        //    UserSession.UserId = user.Id;
        //    UserSession.Username = user.Username;
        //    UserSession.Role = user.Role;


        //    WelcomeWindow welcomeWindow = new WelcomeWindow(context);  
        //    welcomeWindow.Show();
        //    this.Close();
        //}


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var user = context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null || !user.IsConfirmed)
            {
                MessageBox.Show("Неправильное имя пользователя или пароль, или учетная запись не подтверждена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isPasswordValid = VerifyPassword(password, user.PasswordHash, user.Salt);
            if (!isPasswordValid)
            {
                MessageBox.Show("Неправильное имя пользователя или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Установка сессии пользователя
            UserSession.Username = user.Username;
            UserSession.Role = user.Role;
            UserSession.UserId = user.Id;

            // Переход на основное окно
            WelcomeWindow welcomeWindow = new WelcomeWindow(context);
            welcomeWindow.Show();
            this.Close();
        }

        //private bool VerifyPassword(User user, string password)
        //{
        //    // Хэширование введенного пароля с использованием соли из базы данных
        //    byte[] hashedPasswordBytes;
        //    using (var sha256 = SHA256.Create())
        //    {
        //        byte[] combinedBytes = user.Salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();
        //        hashedPasswordBytes = sha256.ComputeHash(combinedBytes);
        //    }

        //    // Сравнение хэшей
        //    return hashedPasswordBytes.SequenceEqual(user.PasswordHash);
        //}


        private bool VerifyPassword(User user, string password)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, user.Salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashedPasswordBytes = pbkdf2.GetBytes(32);
                return hashedPasswordBytes.SequenceEqual(user.PasswordHash);
            }
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return hashBytes.SequenceEqual(storedHash);
            }
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
        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        this.DragMove();
        //    }
        //}

    }
}

