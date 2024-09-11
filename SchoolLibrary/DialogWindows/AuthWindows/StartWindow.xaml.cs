using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private readonly EntityContext context;
        private string adminUsername = "admin";
        private string adminPassword = "vlad";
        public StartWindow()
        {
            InitializeComponent();
            this.context = new EntityContext("SchoolLibraryConnectionString");

            // Проверка на наличие пользователей в базе данных tc
            if (!context.Users.Any())
            {
                CreateAdminAccount(context); 
                MessageBox.Show("Создан пользователь-администратор с логином 'admin' и паролем 'vlad'.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(context);          

            // Сохраняем изменения
            context.SaveChanges();
            loginWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow(context);
            context.SaveChanges();
            registerWindow.Show();
            this.Close();
        }


        private void CreateAdminAccount(EntityContext context)
        {


            if (!context.Users.Any(u => u.Username == adminUsername))
            {
                var (hashedPassword, salt) = HashPassword(adminPassword);

                var adminUser = new User
                {
                    Username = adminUsername,
                    PasswordHash = hashedPassword,
                    Salt = salt,
                    Role = "Администратор",
                    IsConfirmed = true
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
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


        public bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, storedSalt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return hashBytes.SequenceEqual(storedHash);
            }
        }


    }
}
