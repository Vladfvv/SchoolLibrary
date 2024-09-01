using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolLibrary.Views
{
    public class BaseSettings: Window
    {

        public BaseSettings()
        {
            // Подключаем обработчик события MouseDown
           // this.MouseDown += Window_MouseDown;
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
