using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data.Entity;
using System.Windows.Data;
using System.ComponentModel;
using SchoolLibrary.DialogWindows.CategoryWindows;
using SchoolLibrary.Models;

namespace SchoolLibrary.Service
{
    public class CategoryService
    {

        private readonly EntityContext context;
        private string _currentTableName;
        private readonly Window _window; // Ссылка на окно
        private readonly DataGrid dGrid; // Ссылка на DataGrid
        public string CurrentTableName
        {
            get => _currentTableName;
            set
            {
                _currentTableName = value;
                OnPropertyChanged("CurrentTableName");//свойсиво CurrentTableName для управления заголовком окна
                _window.Title = $"{_currentTableName}";//для смены названия основного окна при отображении различных данных
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public CategoryService(EntityContext dbContext, Window window, DataGrid dGrid)
        {
            this.context = dbContext;
            this._window = window;
            this.dGrid = dGrid;
        }

        public void updateListCategories()
        {
            try
            {
                dGrid.ItemsSource = null;
                InitCategoriesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void InitCategoriesList()
        {
            try
            {
                context.Categories.Load();
                ConfigureCategoryColumns();
                dGrid.ItemsSource = context.Categories.Local;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ConfigureCategoryColumns()
        {
            dGrid.Columns.Clear();
            dGrid.Columns.Add(new DataGridTextColumn { Header = "InventoryBookID", Binding = new Binding("CategoryID"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
            dGrid.Columns.Add(new DataGridTextColumn { Header = "Название категории", Binding = new Binding("CategoryName"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        }

        
    }
}
