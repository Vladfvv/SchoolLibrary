using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.AuthWindows
{
    //Валидация логина и пароля
    public class AuthService
    {
        private readonly string connectionString = "SchoolLibraryConnectionString";

        public bool ValidateUser(string username, string password, out string role)
        {
            role = null;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT PasswordHash, Salt, Role FROM Users WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var storedHash = (byte[])reader["PasswordHash"];
                        var storedSalt = (byte[])reader["Salt"];
                        role = reader["Role"].ToString();

                        var hash = PasswordHelper.HashPassword(password, storedSalt);
                        return hash.SequenceEqual(storedHash);
                    }
                }
            }
            return false;
        }
    }
}
