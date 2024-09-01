using SchoolLibrary.AuthWindows;
using SchoolLibrary.DataLoaders;
using SchoolLibrary.DialogWindows.Operations;
using SchoolLibrary.DialogWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Data.Entity;
using SchoolLibrary.DialogWindows.LoanWindows;
using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
using SchoolLibrary.DialogWindows.BookWindows;
using System.Runtime.Remoting.Contexts;
using SchoolLibrary.DialogWindows.StudentWindows;
using System.Diagnostics;
using SchoolLibrary.DialogWindows.Statistic;

namespace SchoolLibrary.Views
{   
    public partial class StartAdminWindow : BaseWindow
    {
        private readonly DataLoader _dataLoader;
        private User _selectedUser;

        public StartAdminWindow(EntityContext context) : base(context)
        {
            InitializeComponent();
            this.context = context;

            _dataLoader = new UserDataLoader(context);
            _dataLoader.LoadData();

            LoadPendingRegistrations();
        }

        private void ConfigureDataGridColumns()
        {
            PendingUsersDataGrid.Columns.Clear();

            // Определение столбцов
            var idColumn = new DataGridTextColumn
            {
                Header = "ID",
                Binding = new Binding("Id"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            var usernameColumn = new DataGridTextColumn
            {
                Header = "Имя пользователя",
                Binding = new Binding("Username"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            var roleColumn = new DataGridTextColumn
            {
                Header = "Роль",
                Binding = new Binding("Role"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };

            var confirmColumn = new DataGridTemplateColumn
            {
                Header = "Подтвердить",
                Width = DataGridLength.Auto,
                CellTemplate = (DataTemplate)Resources["ConfirmButtonTemplate"]
            };

            // Добавление столбцов в DataGrid
            PendingUsersDataGrid.Columns.Add(idColumn);
            PendingUsersDataGrid.Columns.Add(usernameColumn);
            PendingUsersDataGrid.Columns.Add(roleColumn);
            PendingUsersDataGrid.Columns.Add(confirmColumn);
        }

             protected override void ConfigureLoansColumns() //Очищает все текущие колонки в DataGrid и добавляет новые колонки с соответствующими заголовками и привязками
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название книги", Binding = new Binding("Title"), Width = new DataGridLength(1.15, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Имя читателя", Binding = new Binding("FirstName"), Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Фамилия читателя", Binding = new Binding("LastName"), Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Дата выдачи", Binding = new Binding("LoanDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            var dueDate = new DataGridTextColumn { Header = "Должны вернуть", Binding = new Binding("DueDate") { StringFormat = "dd/MM/yyyy" }, Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) };
            dueDate.CellStyle = new Style(typeof(DataGridCell)) { Setters = { new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center) } };
            dGrid.Columns.Add(dueDate);
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Когда вернули", Binding = new Binding("ReturnDate") { StringFormat = "dd/MM/yyyy HH:mm:ss" }, Width = new DataGridLength(0.5, DataGridLengthUnitType.Star) });
            // dGrid.Columns.Add(new DataGridTextColumn { Header = "Подтверждение", Binding = new Binding("Returned"), Width = new DataGridLength(0.45, DataGridLengthUnitType.Star) });
              }

        private void LoadPendingRegistrations()
        {
            ConfigureDataGridColumns();
            var pendingUsers = context.Users
                .Where(u => !u.IsConfirmed && u.Role == "Библиотекарь")
                .ToList();

            if (pendingUsers.Count > 0)
            {
                PendingUsersDataGrid.ItemsSource = pendingUsers;
                PendingUsersDataGrid.Visibility = Visibility.Visible;
                NoDataTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                PendingUsersDataGrid.Visibility = Visibility.Collapsed;
                NoDataTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void LoadAllUsers()
        {
            ConfigureDataGridColumns();
            var allUsers = context.Users.ToList();

            if (allUsers.Count > 0)
            {
                PendingUsersDataGrid.ItemsSource = allUsers;
                PendingUsersDataGrid.Visibility = Visibility.Visible;
                NoDataTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                PendingUsersDataGrid.Visibility = Visibility.Collapsed;
                NoDataTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void btnShowAllUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadAllUsers();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (PendingUsersDataGrid.Items.Count == 0)
            {
                MessageBox.Show("Список пуст. Подтверждать никого не нужно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для подтверждения.");
                return;
            }

            _selectedUser.IsConfirmed = true;
            context.SaveChanges();

            MessageBox.Show("Пользователь подтвержден.");
            LoadPendingRegistrations(); // Обновляем список неподтвержденных регистраций
        }

        private void PendingUsersDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var selectedCell = PendingUsersDataGrid.SelectedCells.FirstOrDefault();
            if (selectedCell != null)
            {
                _selectedUser = selectedCell.Item as User;
            }
        }

        private void ConfirmMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для подтверждения.");
                return;
            }

            _selectedUser.IsConfirmed = true;
            context.SaveChanges();

            MessageBox.Show("Пользователь подтвержден.");
            LoadPendingRegistrations();
        }

        private void CancelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Операция отменена.");
        }

        private void ConfirmButton_Click2(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var user = button.DataContext as User;
            if (user == null) return;

            user.IsConfirmed = true;
            context.SaveChanges();

            // Обновляем список неподтвержденных регистраций
            LoadPendingRegistrations();
        }








    }

}
