using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SchoolLibrary.Service;

namespace SchoolLibrary
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SplashScreen splashScreen;
        //private void Application_Startup(object sender, StartupEventArgs e)
        //{


        // Установка культуры на русский язык для всего приложения
        //System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ru-RU");
        //cultureInfo.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
        //System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
        //System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //// Show the splash screen
        //SplashScreen splash = new SplashScreen();
        //        splash.Show();


        //        // Initialize the main window
        //        //       MainWindow mainWindow = new MainWindow();
        //        AuthWindows.StartWindow startWindow = new AuthWindows.StartWindow();

        //        /*    // Simulate loading delay (replace with actual initialization)
        //            Task.Run(() =>
        //            {
        //                Thread.Sleep(3000); // Simulate a 3-second loading delay
        //                Dispatcher.Invoke(() =>
        //                {
        //                    // Close the splash screen
        //                    splash.Close();*/

        //        // Show the main window/*
        //        //              mainWindow.Show();
        //        startWindow.Show();
        //        /* });
        //      });*/
        //    }

        //    private void CloseButton_Click(object sender, RoutedEventArgs e)
        //    {

        //    }
        //}

        /*
        public partial class App : Application
        {
            private SplashScreen splashScreen;

            protected override void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);

                // Показать загрузочный экран
                splashScreen = new SplashScreen();
                splashScreen.Show();

                // Инициализация основного окна асинхронно
                Task.Run(() =>
                {
                    // Имитируем задержку для загрузки данных
                    System.Threading.Thread.Sleep(6000); // Замените это реальной загрузкой

                    Dispatcher.Invoke(() =>
                    {
                        // Создание и отображение главного окна
                       // var mainWindow = new MainWindow();
                       // mainWindow.Show();

                        // Закрытие загрузочного экрана
                        splashScreen.Close();
                    });
                });
            }
        }
        */

        /* 
         public partial class App : Application
         {
             private SplashScreen splashScreen;
     */
        protected override void OnStartup(StartupEventArgs e)
        {

            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ru-RU");
            cultureInfo.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            base.OnStartup(e);

            // Показать загрузочный экран
            splashScreen = new SplashScreen();
            splashScreen.Show();

            // Инициализация основного окна асинхронно
            Task.Run(() => InitializeMainWindowAsync());
        }

        private async Task InitializeMainWindowAsync()
        {
            //// Имитируем задержку для загрузки данных (например, инициализация контекста)
            //await Task.Delay(5000); // Замените это реальной загрузкой данных

            //// Инициализация основного окна и контекста данных
            //await Dispatcher.InvokeAsync(() =>
            //{
            //    var mainWindow = new MainWindow();
            //    mainWindow.Show();
            //    splashScreen.Close();
            //});
            // Имитируем задержку для загрузки данных (например, инициализация контекста)
            await Task.Delay(5000); // Замените это реальной загрузкой данных

            // Инициализация основного окна и контекста данных
            await Dispatcher.InvokeAsync(() =>
            {
                splashScreen.Close();
            });
        }
    }
    





















    /*
     В файле App.xaml.cs или в начале вашей программы
    using System.Globalization;
    using System.Threading;

    public partial class App : Application
    {
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Устанавливаем культуру для приложения
        CultureInfo culture = new CultureInfo("ru-RU");
        culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
    }*/

}




