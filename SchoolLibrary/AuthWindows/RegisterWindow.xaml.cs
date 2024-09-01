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
    public partial class RegisterWindow : Window
    {
        private EntityContext context;

        public RegisterWindow(EntityContext dbContext)
        {
            InitializeComponent();
            this.context = dbContext;
        }

        //Метод Window_Loaded устанавливает фокус на текстовое поле имени пользователя при загрузке окна. - костыль от необходимости двойного нажатия

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UsernameTextBox.Focus(); // Установите фокус на текстовое поле имени пользователя
        }
              

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            var selectedRole = RoleComboBox.SelectedItem as ComboBoxItem;
            string role = selectedRole?.Content.ToString();

            // Проверка на заполненность полей
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пароль не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedRole == null)
            {
                MessageBox.Show("Пожалуйста, выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на наличие пользователя с таким же именем
            if (context.Users.Any(u => u.Username == username))
            {
                MessageBox.Show("Пользователь с таким именем уже существует.\n " +
                    "Если вы ранее регистрировались, то нажмите кнопку Войти.\n " +
                    "Если не регистрировались - пройдите повторную регистрацию",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Генерация соли и хэширование пароля
            (byte[] hashedPassword, byte[] salt) = HashPassword(password);

            // Сохранение пользователя в базу данных
            SaveUser(username, hashedPassword, salt, role);

            MessageBox.Show("Регистрация выполнена");

            // Переход на страницу входа после успешной регистрации
            var loginWindow = new LoginWindow(context);
            loginWindow.Show();
            this.Close();
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
               
        private void SaveUser(string username, byte[] hashedPassword, byte[] salt, string role)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Salt = salt,
                Role = role,
               // IsConfirmed = true // Установка isConfirmed true
                IsConfirmed = role == "Читатель" // Установка isConfirmed в зависимости от роли
                //станавливает значение isConfirmed в true, если выбранная роль равна "Читатель". В противном случае, isConfirmed будет false
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
                        
        public bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, storedSalt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return hashBytes.SequenceEqual(storedHash);
            }
        }
       

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {            
            // Переход на основное окно
            var loginWindow = new LoginWindow(context);
            loginWindow.Show();
            this.Close();
        }

    }
}
    

