using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string Role { get; set; }
        public bool IsConfirmed { get; set; }
        public int FailedLoginAttempts { get; set; } = 0; // Счетчик неверных попыток
        public DateTime? LastFailedLogin { get; set; } // Время последней неудачной попытки
    }
}
