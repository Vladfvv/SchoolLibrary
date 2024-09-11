using NLog;
using SchoolLibrary.Models;
using SchoolLibrary.Views;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SchoolLibrary.AuthWindows
{
    public partial class LoginWindow : Window
    {
        private readonly EntityContext context;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public LoginWindow(EntityContext dbContext)
        {
            InitializeComponent();
            this.context = dbContext;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameTextBox.Focus(); // Установить фокус на текстовое поле имени пользователя
        }
              


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string adminUsername = "admin";
            string adminPassword = "vlad";

            // Проверка: если поле логина пустое
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Введите логин.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка: если поле пароля пустое
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка на наличие пользователей в базе данных
            if (!context.Users.Any())
            {
                // Если пользователей нет, создаем администратора
                byte[] adminSalt = GenerateSalt();
                byte[] adminPasswordHash = HashPassword(adminPassword, adminSalt);
                // При создании администратора проверить
                Console.WriteLine($"Admin Password Hash: {Convert.ToBase64String(adminPasswordHash)}");
                Console.WriteLine($"Admin Salt: {Convert.ToBase64String(adminSalt)}");


                var adminUser = new User
                {
                    Username = adminUsername,
                    PasswordHash = adminPasswordHash,
                    Salt = adminSalt,
                    Role = "Admin",
                    IsConfirmed = true, // Подтверждаем учетную запись администратора автоматически
                    FailedLoginAttempts = 0
                };

                context.Users.Add(adminUser);
                context.SaveChanges();

                MessageBox.Show("Создан пользователь-администратор с логином 'admin' и паролем 'vlad'.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Получение пользователя из базы данных
            var user = context.Users.SingleOrDefault(u => u.Username == username);

            // Проверка: если пользователь не найден
            if (user == null)
            {
                MessageBox.Show("Пользователь с таким именем не зарегистрирован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка: если учетная запись не подтверждена
            if (!user.IsConfirmed)
            {
                MessageBox.Show("Ваша учетная запись не подтверждена. Пожалуйста, свяжитесь с администратором.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка: если 3 неудачных попытки и последняя попытка была менее 10 минут назад
            if (user.FailedLoginAttempts >= 3 && user.LastFailedLogin.HasValue &&
                (DateTime.Now - user.LastFailedLogin.Value).TotalMinutes < 10)
            {
                MessageBox.Show($"Вы превысили количество попыток входа. Попробуйте снова через {10 - (DateTime.Now - user.LastFailedLogin.Value).Minutes} минут.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isPasswordValid = VerifyPassword(password, user.PasswordHash, user.Salt);

            if (!isPasswordValid)
            {
                // Обновляем информацию о неудачной попытке
                user.FailedLoginAttempts++;
                user.LastFailedLogin = DateTime.Now;
                context.SaveChanges(); // Сохранить изменения в базе данных

                if (user.FailedLoginAttempts >= 3)
                {
                    MessageBox.Show("Вы ввели неправильный пароль 3 раза. Попробуйте снова через 10 минут.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Неправильный пароль. Попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return;
            }

            // Если авторизация успешна, сбрасываем счетчик неудачных попыток
            user.FailedLoginAttempts = 0;
            user.LastFailedLogin = null;
            context.SaveChanges(); // Сохранить изменения в базе данных

            // Сообщение об успешной авторизации
            MessageBox.Show("Вы успешно вошли в систему!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            logger.Info($"Пользователь {user.Username} успешно вошел в систему в {DateTime.Now}");

            // Установка сессии пользователя
            UserSession.Username = user.Username;
            UserSession.Role = user.Role;
            UserSession.UserId = user.Id;

            // Переход на основное окно
            WelcomeWindow welcomeWindow = new WelcomeWindow(context);
            welcomeWindow.Show();
            this.Close();
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 10000, HashAlgorithmName.SHA256))
            {     
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return hashBytes.SequenceEqual(storedHash);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(context);
            registerWindow.Show();
            this.Close();
        }

        private static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }


        private (byte[] hashBytes, byte[] salt) HashPassword(string password)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, 32, 10000, HashAlgorithmName.SHA256))
            {
                byte[] salt = pbkdf2.Salt;
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return (hashBytes, salt);
            }
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var rfc2898 = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000))
            {
                return rfc2898.GetBytes(32); // Возвращаем хеш пароля длиной 32 байта
            }
        }
    }
}

