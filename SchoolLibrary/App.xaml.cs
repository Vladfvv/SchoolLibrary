using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using SchoolLibrary.Service;

namespace SchoolLibrary
{  
    public partial class App : Application
    {
        private SplashScreen splashScreen;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected override void OnStartup(StartupEventArgs e)
        {

            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ru-RU");
            cultureInfo.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            base.OnStartup(e);
            logger.Info("Программа запущена");
            // Показать загрузочный экран
            splashScreen = new SplashScreen();
            splashScreen.Show();

            // Инициализация основного окна асинхронно
            Task.Run(() => InitializeMainWindowAsync());
        }

        private async Task InitializeMainWindowAsync()
        {           
            // Имитируем задержку для загрузки данных (например, инициализация контекста)
            await Task.Delay(5000); // Замените это реальной загрузкой данных

            // Инициализация основного окна и контекста данных
            await Dispatcher.InvokeAsync(() =>
            {
                splashScreen.Close();
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            logger.Info("Программа завершена");
            base.OnExit(e);
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




