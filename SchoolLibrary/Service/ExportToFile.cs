using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using OfficeOpenXml;

namespace SchoolLibrary.Service
{
    public class ExportToFile
    {  
        public void ExportDataGridToPDF(IEnumerable<object> listForExport, DataGrid dataGrid, string pdfFilePath)
        {
            // Определяем максимальные ширины столбцов
            var columnWidths = new float[dataGrid.Columns.Count];
            for (int colIndex = 0; colIndex < dataGrid.Columns.Count; colIndex++)
            {
                var column = dataGrid.Columns[colIndex];//Определяем количество колонок
                var maxWidth = column.Header.ToString().Length; // Длина заголовка колонки

                foreach (var item in listForExport)
                {
                    if (item == null) continue;

                    var cellValue = string.Empty;
                    if (column is DataGridBoundColumn boundColumn)
                    {
                        var binding = boundColumn.Binding as Binding;
                        if (binding != null)
                        {
                            var propertyName = binding.Path.Path;
                            var propInfo = item.GetType().GetProperty(propertyName);
                            cellValue = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
                        }
                    }

                    maxWidth = Math.Max(maxWidth, cellValue.Length);
                }

                // Если это первый столбец, увеличиваем его ширину
                if (colIndex == 0)
                {
                    columnWidths[colIndex] = maxWidth * 7 * 2; // Увеличиваем ширину первого столбца
                }
                else
                {
                    columnWidths[colIndex] = maxWidth * 7; // Увеличение коэффициента для шрифта
                }

                // Если столбец называется "Год", добавляем 2 символа
                if (column.Header.ToString() == "Год")
                {
                    columnWidths[colIndex] += 2 * 7; // Добавляем ширину для 2 символов
                }
            }

            using (var doc = new iTextSharp.text.Document())
            {
                PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
                doc.Open();

                // путь к шрифту, который поддерживает кириллицу
                string fontPath = @"C:\Windows\Fonts\Arial Unicode MS.ttf";
                if (!File.Exists(fontPath))
                {
                    MessageBox.Show("Шрифт не найден. Пожалуйста, убедитесь, что он установлен на системе.");
                    return;
                }
                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var headerFont = new Font(baseFont, 8, iTextSharp.text.Font.BOLD);
                var dataFont = new Font(baseFont, 6, iTextSharp.text.Font.NORMAL);

                // Создание таблицы PDF с количеством колонок и шириной столбцов
                PdfPTable pdfTable = new PdfPTable(dataGrid.Columns.Count)
                {
                    WidthPercentage = 100 // Устанавливаем ширину таблицы в 100% от доступного пространства
                };
                pdfTable.SetWidths(columnWidths);

                // Добавляем заголовки колонок
                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    var cell = new PdfPCell(new Phrase(column.Header.ToString(), headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY, // Цвет фона заголовков
                        HorizontalAlignment = Element.ALIGN_CENTER // Выравнивание текста
                    };
                    pdfTable.AddCell(cell);
                }

                // Добавляем строки данных
                foreach (var item in listForExport)
                {
                    if (item == null) continue;

                    foreach (DataGridColumn column in dataGrid.Columns)
                    {
                        if (column is DataGridBoundColumn boundColumn)
                        {
                            var binding = boundColumn.Binding as Binding;
                            if (binding != null)
                            {
                                var propertyName = binding.Path.Path;
                                var propInfo = item.GetType().GetProperty(propertyName);
                                var cellValue = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;

                                var cell = new PdfPCell(new Phrase(cellValue, dataFont))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT, // Выравнивание текста
                                    NoWrap = false // Позволяет перенос строк в ячейках
                                };
                                pdfTable.AddCell(cell);
                            }
                        }
                        else
                        {
                            var cell = new PdfPCell(new Phrase(string.Empty, dataFont))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT
                            };
                            pdfTable.AddCell(cell);
                        }
                    }
                }

                // Добавление таблицы в документ и закрытие документа
                doc.Add(pdfTable);
                doc.Close();
            }

            MessageBox.Show($"Данные экспортированы в PDF файл: {pdfFilePath}", "Экспорт в PDF", MessageBoxButton.OK, MessageBoxImage.Information);
        }




        public void ExportDataGridToExcel(IEnumerable<object> listForExport, DataGrid dataGrid, string excelFilePath)
        {
            // Установка контекста лицензии для EPPlus
            OfficeOpenXml.ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Добавляем заголовки колонок
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header.ToString();
                }

                // Добавляем данные
                int rowIndex = 2;
                foreach (var item in listForExport)
                {
                    if (item == null) continue;

                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        var column = dataGrid.Columns[j] as DataGridBoundColumn;
                        if (column != null)
                        {
                            var binding = column.Binding as Binding;
                            if (binding != null)
                            {
                                var propertyName = binding.Path.Path;
                                var propInfo = item.GetType().GetProperty(propertyName);
                                var cellValue = propInfo?.GetValue(item, null)?.ToString() ?? string.Empty;
                                worksheet.Cells[rowIndex, j + 1].Value = cellValue;
                            }
                        }
                    }
                    rowIndex++;
                }

                // Сохраняем файл
                package.SaveAs(new FileInfo(excelFilePath));
            }

            MessageBox.Show($"Данные экспортированы в Excel файл: {excelFilePath}", "Экспорт в Excel", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

