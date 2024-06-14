using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolLibrary
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Show the splash screen
            SplashScreen splash = new SplashScreen();
            splash.Show();

            // Initialize the main window
            MainWindow mainWindow = new MainWindow();

            // Simulate loading delay (replace with actual initialization)
            Task.Run(() =>
            {
                Thread.Sleep(3000); // Simulate a 3-second loading delay
                Dispatcher.Invoke(() =>
                {
                    // Close the splash screen
                    splash.Close();

                    // Show the main window
                    mainWindow.Show();
                });
            });
        }
    }
}
