using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace SchoolLibrary.AuthWindows
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private EntityContext context;

        public RegisterWindow(EntityContext dbContext)
        {
            InitializeComponent();
            this.context = dbContext;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem).Content.ToString();

            // Проверка на наличие пользователя с таким же именем
            if (context.Users.Any(u => u.Username == username))
            {
                MessageBox.Show("Пользователь с таким именем уже существует.\n " +
                    "Если вы ранее регистрировались то нажмите кнопку Войти.\n " +
                    "Если не регистрировались - пройдите повторную регистрацию", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Генерация соли и хэширование пароля
            (string hashedPassword, byte[] salt) = HashPassword(password);

            // Сохранение пользователя в базу данных
            SaveUser(username, hashedPassword, salt, role);

            MessageBox.Show("Регистрация выполнена");

            // Переход на страницу входа после успешной регистрации
            var loginWindow = new LoginWindow(context);
            loginWindow.Show();
            this.Close();
        }

        private (string hashedPassword, byte[] salt) HashPassword(string password)
        {
            // Создание соли
            byte[] salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Хэширование пароля с использованием соли
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = salt.Concat(Encoding.UTF8.GetBytes(password)).ToArray();
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return (Convert.ToBase64String(hashBytes), salt);
            }
        }

        private void SaveUser(string username, string hashedPassword, byte[] salt, string role)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = Convert.FromBase64String(hashedPassword),
                Salt = salt,
                Role = role
            };

            context.Users.Add(user);
            context.SaveChanges();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow(context);
            loginWindow.Show();
            this.Close();
        }
    }
}
    

