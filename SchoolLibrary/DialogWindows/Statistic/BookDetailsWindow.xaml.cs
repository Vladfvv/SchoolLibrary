using SchoolLibrary.ViewModels;
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

namespace SchoolLibrary.DialogWindows.Statistic
{
    /// <summary>
    /// Логика взаимодействия для BookDetailsWindow.xaml
    /// </summary>
    public partial class BookDetailsWindow : Window
    {
        public BookDetailsWindow(InventoryBookDetailsViewModel viewModel)
        {
            InitializeComponent();
            // Центрирование окна на экране
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Print the visual content of the window
                var visual = new DrawingVisual();
                using (var context = visual.RenderOpen())
                {
                    var size = new Size(this.ActualWidth, this.ActualHeight);
                    context.DrawRectangle(new VisualBrush(this), null, new Rect(new Point(0, 0), size));
                }

                // Measure and arrange the visual
                var printArea = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                var visualBrush = new VisualBrush(this);
                // var visual = new DrawingVisual();
                using (var context = visual.RenderOpen())
                {
                    context.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), printArea));
                }

                // Print the visual
                printDialog.PrintVisual(visual, "Ведомость по читателю");
            }
        }
    }
}
