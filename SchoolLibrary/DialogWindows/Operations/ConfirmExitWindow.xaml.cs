using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SchoolLibrary.DialogWindows.Operations
{
    /// <summary>
    /// Логика взаимодействия для ConfirmExitWindow.xaml
    /// </summary>
    public partial class ConfirmExitWindow : Window
    {
        public bool IsConfirmed { get; private set; }

        public ConfirmExitWindow()
        {
            InitializeComponent();
            // Настройка диалоговое окно, чтобы оно не блокировало ввод в основной форме
           // this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.Close();
        }
    }
}
