using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    //Cтатический класс для хранения информации о текущем пользователе
    //Это подход с глобальными статическими классами или singleton,
    //чтобы сохранять информацию о текущем пользователе
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; }

        public static void Clear()
        {
            UserId = 0;
            Username = null;
            Role = null;
        }
    }
}
