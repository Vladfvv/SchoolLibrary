using SchoolLibrary.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary
{
    //3.Для того, чтобы новая база данных при создании заполнялась
    // исходными данными, создайте класс инициализации базы данных
    class DataBaseInitializer : DropCreateDatabaseIfModelChanges<EntityContext>
    {//4.В созданном классе переопределите метод
        //Seed для инициализации начальных значений :

        protected override void Seed(EntityContext context)
        {

            // Очистка базы данных
            context.Loans.RemoveRange(context.Loans);
            context.InventoryBooks.RemoveRange(context.InventoryBooks);
            context.Categories.RemoveRange(context.Categories);
            context.Books.RemoveRange(context.Books);
            context.BookPhotos.RemoveRange(context.BookPhotos);
            context.Students.RemoveRange(context.Students);
            context.SaveChanges();

            Student ivanIvanov = new Student { FirstName = "Иван", LastName = "Иванов", Age = 15, Class = "10A" };
            Student mariyaPetrova = new Student { FirstName = "Мария", LastName = "Петрова", Age = 15, Class = "9B" };
            Student alexeySmirnov = new Student { FirstName = "Алексей", LastName = "Смирнов", Age = 14, Class = "8C" };
            Student ekaterinaKozlova = new Student { FirstName = "Екатерина", LastName = "Козлова", Age = 13, Class = "7A" };
            Student artemFedorov = new Student { FirstName = "Артем", LastName = "Федоров", Age = 12, Class = "6B" };
            Student anastasiaMikhaylova = new Student { FirstName = "Анастасия", LastName = "Михайлова", Age = 11, Class = "5C" };
            Student dmitriyNikolaev = new Student { FirstName = "Дмитрий", LastName = "Николаев", Age = 10, Class = "4A" };
            Student sofiaIvanova = new Student { FirstName = "София", LastName = "Иванова", Age = 9, Class = "3B" };
            Student maximSmirnov = new Student { FirstName = "Максим", LastName = "Смирнов", Age = 8, Class = "2C" };
            Student alisaPetrova = new Student { FirstName = "Алиса", LastName = "Петрова", Age = 7, Class = "1A" };
            Student nikitaKuznetsov = new Student { FirstName = "Никита", LastName = "Кузнецов", Age = 15, Class = "10B" };
            Student annaSidorova = new Student { FirstName = "Анна", LastName = "Сидорова", Age = 15, Class = "9A" };
            Student artemiyFedorov = new Student { FirstName = "Артемий", LastName = "Федоров", Age = 14, Class = "8B" };
            Student polinaIvanova = new Student { FirstName = "Полина", LastName = "Иванова", Age = 13, Class = "7C" };
            Student egorPetrov = new Student { FirstName = "Егор", LastName = "Петров", Age = 12, Class = "6A" };
            Student kseniaKozlova = new Student { FirstName = "Ксения", LastName = "Козлова", Age = 11, Class = "5B" };
            Student alexandraSmirnova = new Student { FirstName = "Александра", LastName = "Смирнова", Age = 10, Class = "4C" };
            Student mikhailNikolaev = new Student { FirstName = "Михаил", LastName = "Николаев", Age = 9, Class = "3A" };
            Student elizavetaFedorova = new Student { FirstName = "Елизавета", LastName = "Федорова", Age = 8, Class = "2B" };
            Student ilyaIvanov = new Student { FirstName = "Илья", LastName = "Иванов", Age = 7, Class = "1C" };
            Student pavelKuznetsov = new Student { FirstName = "Павел", LastName = "Кузнецов", Age = 15, Class = "10A" };
            Student valeriaSidorova = new Student { FirstName = "Валерия", LastName = "Сидорова", Age = 15, Class = "9B" };
            Student vladimirFedorov = new Student { FirstName = "Владимир", LastName = "Федоров", Age = 14, Class = "8C" };
            Student anastasiaIvanova = new Student { FirstName = "Анастасия", LastName = "Иванова", Age = 13, Class = "7A" };
            Student sergeyPetrov = new Student { FirstName = "Сергей", LastName = "Петров", Age = 12, Class = "6B" };
            Student daryaKozlova = new Student { FirstName = "Дарья", LastName = "Козлова", Age = 11, Class = "5C" };
            Student alexeySmirnov2 = new Student { FirstName = "Алексей", LastName = "Смирнов", Age = 10, Class = "4A" };
            Student elenaNikolaeva = new Student { FirstName = "Елена", LastName = "Николаева", Age = 9, Class = "3B" };
            Student artemFedorov2 = new Student { FirstName = "Артем", LastName = "Федоров", Age = 8, Class = "2C" };
            Student nadezhdaIvanova = new Student { FirstName = "Надежда", LastName = "Иванова", Age = 7, Class = "1A" };
            Student ivanKuznetsov = new Student { FirstName = "Иван", LastName = "Кузнецов", Age = 15, Class = "10B" };
            Student mariyaSidorova = new Student { FirstName = "Мария", LastName = "Сидорова", Age = 15, Class = "9A" };
            Student alexanderFedorov = new Student { FirstName = "Александер", LastName = "Федоров", Age = 14, Class = "8B" };
            Student olgaIvanova = new Student { FirstName = "Ольга", LastName = "Иванова", Age = 13, Class = "7C" };
            Student kirillPetrov = new Student { FirstName = "Кирилл", LastName = "Петров", Age = 12, Class = "6A" };
            Student tatyanaKozlova = new Student { FirstName = "Татьяна", LastName = "Козлова", Age = 11, Class = "5B" };
            Student evgeniySmirnov = new Student { FirstName = "Евгений", LastName = "Смирнов", Age = 10, Class = "4C" };
            Student anastasiaNikolaeva = new Student { FirstName = "Анастасия", LastName = "Николаева", Age = 9, Class = "3A" };
            Student igorFedorov = new Student { FirstName = "Игорь", LastName = "Федоров", Age = 8, Class = "2B" };
            Student ekaterinaIvanova = new Student { FirstName = "Екатерина", LastName = "Иванова", Age = 7, Class = "1C" };
            Student alexanderKuznetsov = new Student { FirstName = "Александр", LastName = "Кузнецов", Age = 15, Class = "10A" };
            Student viktoriaSidorova = new Student { FirstName = "Виктория", LastName = "Сидорова", Age = 15, Class = "9B" };
            Student vasiliyFedorov = new Student { FirstName = "Василий", LastName = "Федоров", Age = 14, Class = "8C" };

            context.Students.AddRange(new Student[] {
            ivanIvanov, mariyaPetrova, alexeySmirnov, ekaterinaKozlova, artemFedorov, anastasiaMikhaylova,
            dmitriyNikolaev, sofiaIvanova, maximSmirnov, alisaPetrova, nikitaKuznetsov, annaSidorova, artemiyFedorov,
            polinaIvanova, egorPetrov, kseniaKozlova, alexandraSmirnova, mikhailNikolaev, elizavetaFedorova, ilyaIvanov,
            pavelKuznetsov, valeriaSidorova, vladimirFedorov, anastasiaIvanova, sergeyPetrov, daryaKozlova,
            alexeySmirnov2, elenaNikolaeva, artemFedorov2, nadezhdaIvanova, ivanKuznetsov, mariyaSidorova,
            alexanderFedorov, olgaIvanova, kirillPetrov, tatyanaKozlova, evgeniySmirnov, anastasiaNikolaeva,
            igorFedorov, ekaterinaIvanova, alexanderKuznetsov, viktoriaSidorova, vasiliyFedorov});


            Category informatics = new Category { CategoryName = "Информатика" };
            Category english = new Category { CategoryName = "Английский язык" };
            Category belarusian = new Category { CategoryName = "Белорусский язык" };
            Category mathematics = new Category { CategoryName = "Математика" };
            Category russian = new Category { CategoryName = "Русский язык" };
            Category geografy = new Category { CategoryName = "География" };
            Category physics = new Category { CategoryName = "Физика" };
            Category chemistry = new Category { CategoryName = "Химия" };
            Category history = new Category { CategoryName = "История" };
            Category literature = new Category { CategoryName = "Литература" };
            Category biology = new Category { CategoryName = "Биология" };

            context.Categories.AddRange(new Category[] { informatics, english, belarusian, mathematics, russian, geografy, physics, chemistry, history, literature, biology });


            /*           InventoryBook bookMath1Class1 = new InventoryBook
                       {
                           InventoryNumber = "INV001",
                           ISBN = "978-5-907172-47-6",
                           Title = "Математика 1 класс",
                           Author = "Иванов И.И.",
                           Publisher = "Аверсев",
                           YearPublished = "2001",
                           Price = 50.0,
                           DateOfReceipt = DateTime.Now,
                           IncomingInvoice = "123456 ТЕ"
                       };
                       var bookMath1Class2 = new InventoryBook
                       {
                           InventoryBookID = 2,
                           InventoryNumber = "INV002",
                           ISBN = "978-5-907172-47-6",
                           Title = "Математика 1 класс",
                           Author = "Иванов И.И.",
                           Publisher = "Аверсев",
                           YearPublished = "2023",
                           Price = 45.5,
                           DateOfReceipt = DateTime.Now,
                           IncomingInvoice = "123456 ТЕ"
                       };



                       //         string relativePath = @"Resources\22412.jpg";
                       //             string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

                       // Теперь используйте fullPath для загрузки изображения
                       //byte[] imageBytes = File.ReadAllBytes(fullPath);

                       // Пример фотографий книги
                       var bookPhoto1 = new BookPhoto
                       {
                           BookPhotoID = 1,

                           PhotoData = BookPhoto.LoadImage("Resources/22412.jpg"),
                           // PhotoData = BookPhoto.LoadImage(fullPath),
                           DateAdded = DateTime.Now
                       };
                       context.BookPhotos.AddRange(new BookPhoto[] { bookPhoto1 });
           */
            /* var bookPhoto2 = new BookPhoto
             {
                 BookPhotoID = 2,
                 PhotoData = BookPhoto.LoadImage("Resources/25554.jpg"),
                 DateAdded = DateTime.Now
             };**/
            /*            context.InventoryBooks.AddRange(new InventoryBook[] { bookMath1Class1, bookMath1Class2 });
                        Book booksMath1Class = new Book
                        {
                            BookID = 1,
                            Class = 10,
                            Description = "Учебник по математике для начальных классов",
                            CategoryID = 4,
                            Quantity = 2,
                            QuantityLeft = 2,
                            Category = mathematics,
                            InventoryBooks = new List<InventoryBook> { bookMath1Class1, bookMath1Class2 },
                            BookPhotos = new List<BookPhoto> { bookPhoto1 }
                            //BookPhotos = new List<BookPhoto> { bookPhoto1, bookPhoto2 }
                        };

                        // Установка BookID для фотографий
                        bookPhoto1.BookID = booksMath1Class.BookID;
                        //  bookPhoto2.BookID = booksMath1Class.BookID;

                        // Установка связи с книгой для фотографий
                        bookPhoto1.Book = booksMath1Class;
                        //   bookPhoto2.Book = booksMath1Class;



                        context.Books.AddRange(new Book[] { booksMath1Class });

             */




          


           










            InventoryBook bookMath1Class1 = new InventoryBook
            {
                InventoryNumber = "1",
                ISBN = "978-5-907172-47-6",
                Title = "Математика 1 класс",
                Author = "Иванов И.И.",
                Publisher = "Аверсев",
                YearPublished = "2023",
                Price = 50.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123456 ТЕ"
            };

            var bookMath1Class2 = new InventoryBook
            {
                InventoryNumber = "2",
                ISBN = "978-5-907172-47-6",
                Title = "Математика 1 класс",
                Author = "Иванов И.И.",
                Publisher = "Аверсев",
                YearPublished = "2023",
                Price = 45.5,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "654321 ТЕ"
            };

            // Создаем объект Book
            Book booksMath1Class = new Book
            {
                Class = 1,
                Description = "Учебник по математике для начальных классов",
                //CategoryID = 4,
                Quantity = 2,
                QuantityLeft = 2,
                Category = mathematics,
                InventoryBooks = new List<InventoryBook> { bookMath1Class1, bookMath1Class2 },
            };

            // Устанавливаем связь между InventoryBook и Book
            bookMath1Class1.Book = booksMath1Class;
            bookMath1Class2.Book = booksMath1Class;

            // Создаем объект BookPhoto
            var bookPhoto1 = new BookPhoto
            {
                PhotoData = BookPhoto.LoadImage("Resources/22412.jpg"),
                DateAdded = DateTime.Now,
                Book = booksMath1Class
            };

            // Добавляем объекты в контекст один раз
            context.InventoryBooks.AddRange(new InventoryBook[] { bookMath1Class1, bookMath1Class2 });
            context.Books.Add(booksMath1Class);
            context.BookPhotos.Add(bookPhoto1);





            InventoryBook inventoryEnglish3Book1 = new InventoryBook
            {
                InventoryNumber = "3",
                ISBN = "978-5-907172-47-1",
                Title = "Английский язык. 3 класс.Практикум - 1",
                Author = "Лапицкая Л. М.",
                Publisher = "Аверсев",
                YearPublished = "2020",
                Price = 18.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "111111 ТЕ"
            };

            InventoryBook inventoryEnglish3Book2 = new InventoryBook
            {
                InventoryNumber = "4",
                ISBN = "978-5-907172-47-1",
                Title = "Английский язык. 3 класс.Практикум - 1",
                Author = "Лапицкая Л. М.",
                Publisher = "Аверсев",
                YearPublished = "2020",
                Price = 20.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "222222 ТЕ"
            };

            Book booksEnglish3Class = new Book
            {
                Class = 10,
                Description = "Учебник по английскому языку для начальных классов",
               // CategoryID = 2,
                Quantity = 2,
                QuantityLeft = 2,
                Category = mathematics,
                InventoryBooks = new List<InventoryBook> { inventoryEnglish3Book1, inventoryEnglish3Book2 },
            };


            // Устанавливаем связь между InventoryBook и Book
            inventoryEnglish3Book1.Book = booksEnglish3Class;
            inventoryEnglish3Book2.Book = booksEnglish3Class;

            // Создаем объект BookPhoto
            var bookPhoto2 = new BookPhoto {PhotoData = BookPhoto.LoadImage("Resources/26061.jpg"), DateAdded = DateTime.Now, Book = booksEnglish3Class };
            var bookPhoto3 = new BookPhoto {PhotoData = BookPhoto.LoadImage("Resources/3.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class};
            var bookPhoto4 = new BookPhoto {PhotoData = BookPhoto.LoadImage("Resources/4.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class};
            var bookPhoto5 = new BookPhoto {PhotoData = BookPhoto.LoadImage("Resources/5.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class};
            var bookPhoto6 = new BookPhoto {PhotoData = BookPhoto.LoadImage("Resources/7.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class };

            context.BookPhotos.AddRange(new BookPhoto[] { bookPhoto2, bookPhoto3, bookPhoto4, bookPhoto5, bookPhoto6 });
            // Добавляем объекты в контекст один раз
            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryEnglish3Book1, inventoryEnglish3Book2 });
            context.Books.Add(booksEnglish3Class);
           


            context.SaveChanges();
            /*           Book bookMath1Class = new Book { Title = "Математика 1 класс", Author = "Иванов И.И.", Class = 1, Description = "Математика для начальных классов", Publisher = "Аверсев", YearPublished = "2001", ISBN = "978-1-23-456", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath2Class = new Book { Title = "Математика 2 класс", Author = "Петров П.П.", Class = 2, Description = "Математика для начальных классов", Publisher = "Аверсев", YearPublished = "2002", ISBN = "978-2-34-567", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath3Class = new Book { Title = "Математика 3 класс", Author = "Сидоров С.С.", Class = 3, Description = "Математика для начальных классов", Publisher = "Просвещение", YearPublished = "2003", ISBN = "978-3-45-678", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath4Class = new Book { Title = "Математика 4 класс", Author = "Кузнецов К.К.", Class = 4, Description = "Математика для начальных классов", Publisher = "Аверсев", YearPublished = "2004", ISBN = "978-4-56-789", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath5Class = new Book { Title = "Математика 5 класс", Author = "Щурок В.Ф.", Class = 5, Description = "Математика для средней школы", Publisher = "Аверсев", YearPublished = "2000", ISBN = "973-8-25-352", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath6Class = new Book { Title = "Математика 6 класс", Author = "Головин Г.Г.", Class = 6, Description = "Математика для средней школы", Publisher = "Аверсев", YearPublished = "2005", ISBN = "978-5-67-890", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath7Class = new Book { Title = "Математика 7 класс", Author = "Левин Л.Л.", Class = 7, Description = "Математика для средней школы", Publisher = "Просвещение", YearPublished = "2006", ISBN = "978-6-78-901", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath8Class = new Book { Title = "Математика 8 класс", Author = "Рубинштейн Р.Р.", Class = 8, Description = "Математика для средней школы", Publisher = "Аверсев", YearPublished = "2007", ISBN = "978-7-89-012", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath9Class = new Book { Title = "Математика 9 класс", Author = "Соколова С.С.", Class = 9, Description = "Математика для средней школы", Publisher = "Просвещение", YearPublished = "2008", ISBN = "978-8-90-123", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath10Class = new Book { Title = "Математика 10 класс", Author = "Дмитриев Д.Д.", Class = 10, Description = "Математика для старшей школы", Publisher = "Аверсев", YearPublished = "2009", ISBN = "978-9-01-234", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookMath11Class = new Book { Title = "Математика 11 класс", Author = "Иванова И.И.", Class = 11, Description = "Математика для старшей школы", Publisher = "Просвещение", YearPublished = "2010", ISBN = "978-0-12-345", Category = mathematics, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu1Class = new Book { Title = "Русский язык 1 класс", Author = "Петров П.П.", Class = 1, Description = "Русский язык для начальных классов", Publisher = "Просвещение", YearPublished = "2001", ISBN = "978-2-34-567", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu2Class = new Book { Title = "Русский язык 2 класс", Author = "Сидоров С.С.", Class = 2, Description = "Русский язык для начальных классов", Publisher = "Аверсев", YearPublished = "2002", ISBN = "978-3-45-678", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu3Class = new Book { Title = "Русский язык 3 класс", Author = "Кузнецов К.К.", Class = 3, Description = "Русский язык для начальных классов", Publisher = "Просвещение", YearPublished = "2003", ISBN = "978-4-56-789", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu4Class = new Book { Title = "Русский язык 4 класс", Author = "Щурок В.Ф.", Class = 4, Description = "Русский язык для начальных классов", Publisher = "Аверсев", YearPublished = "2004", ISBN = "978-5-67-890", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu5Class = new Book { Title = "Русский язык 5 класс", Author = "Головин Г.Г.", Class = 5, Description = "Русский язык для средней школы", Publisher = "Просвещение", YearPublished = "2005", ISBN = "978-6-78-901", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu6Class = new Book { Title = "Русский язык 6 класс", Author = "Левин Л.Л.", Class = 6, Description = "Русский язык для средней школы", Publisher = "Аверсев", YearPublished = "2006", ISBN = "978-7-89-012", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu7Class = new Book { Title = "Русский язык 7 класс", Author = "Рубинштейн Р.Р.", Class = 7, Description = "Русский язык для средней школы", Publisher = "Просвещение", YearPublished = "2007", ISBN = "978-8-90-123", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu8Class = new Book { Title = "Русский язык 8 класс", Author = "Соколова С.С.", Class = 8, Description = "Русский язык для средней школы", Publisher = "Аверсев", YearPublished = "2008", ISBN = "978-9-01-234", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu9Class = new Book { Title = "Русский язык 9 класс", Author = "Дмитриев Д.Д.", Class = 9, Description = "Русский язык для средней школы", Publisher = "Просвещение", YearPublished = "2009", ISBN = "978-0-12-345", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu10Class = new Book { Title = "Русский язык 10 класс", Author = "Иванова И.И.", Class = 10, Description = "Русский язык для старшей школы", Publisher = "Аверсев", YearPublished = "2010", ISBN = "978-1-23-456", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookRu11Class = new Book { Title = "Русский язык 11 класс", Author = "Петров П.П.", Class = 11, Description = "Русский язык для старшей школы", Publisher = "Просвещение", YearPublished = "2011", ISBN = "978-2-34-567", Category = russian, Quantity = 10, QuantityLeft = 10 };
                       Book bookInf5Class = new Book { Title = "Информатика 5 класс", Author = "Левин Л.Л.", Class = 5, Description = "Информатика для средней школы", Publisher = "Аверсев", YearPublished = "2005", ISBN = "978-7-89-012", Category = informatics, Quantity = 10, QuantityLeft = 10 };
                       Book bookInf6Class = new Book { Title = "Информатика 6 класс", Author = "Рубинштейн Р.Р.", Class = 6, Description = "Информатика для средней школы", Publisher = "Просвещение", YearPublished = "2006", ISBN = "978-8-90-123", Category = informatics, Quantity = 10, QuantityLeft = 10 };
                       Book bookInf7Class = new Book { Title = "Информатика 7 класс", Author = "Соколова С.С.", Class = 7, Description = "Информатика для средней школы", Publisher = "Аверсев", YearPublished = "2007", ISBN = "978-9-01-234", Category = informatics, Quantity = 10, QuantityLeft = 10 };
                       Book bookInf8Class = new Book { Title = "Информатика 8 класс", Author = "Дмитриев Д.Д.", Class = 8, Description = "Информатика для средней школы", Publisher = "Просвещение", YearPublished = "2008", ISBN = "978-0-12-345", Category = informatics, Quantity = 10, QuantityLeft = 10 };
                       Book bookInf9Class = new Book { Title = "Информатика 9 класс", Author = "Иванова И.И.", Class = 9, Description = "Информатика для средней школы", Publisher = "Аверсев", YearPublished = "2009", ISBN = "978-1-23-456", Category = informatics, Quantity = 10, QuantityLeft = 10 };
                       Book bookInf10Class = new Book { Title = "Информатика 10 класс", Author = "Петров П.П.", Class = 10, Description = "Информатика для старшей школы", Publisher = "Просвещение", YearPublished = "2010", ISBN = "978-2-34-567", Category = informatics, Quantity = 10, QuantityLeft = 10 };
                       Book bookInf11Class = new Book { Title = "Информатика 11 класс", Author = "Сидоров С.С.", Class = 11, Description = "Информатика для старшей школы", Publisher = "Аверсев", YearPublished = "2011", ISBN = "978-3-45-678", Category = informatics, Quantity = 10, QuantityLeft = 10 };










                       Book bookEng3Class = new Book { Title = "Английский язык 3 класс", Author = "Головин Г.Г.", Publisher = "Просвещение", YearPublished = "2003", ISBN = "978-6-78-901", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng4Class = new Book { Title = "Английский язык 4 класс", Author = "Левин Л.Л.", Publisher = "Аверсев", YearPublished = "2004", ISBN = "978-7-89-012", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng5Class = new Book { Title = "Английский язык 5 класс", Author = "Рубинштейн Р.Р.", Publisher = "Просвещение", YearPublished = "2005", ISBN = "978-8-90-123", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng6Class = new Book { Title = "Английский язык 6 класс", Author = "Соколова С.С.", Publisher = "Аверсев", YearPublished = "2006", ISBN = "978-9-01-234", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng7Class = new Book { Title = "Английский язык 7 класс", Author = "Дмитриев Д.Д.", Publisher = "Просвещение", YearPublished = "2007", ISBN = "978-0-12-345", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng8Class = new Book { Title = "Английский язык 8 класс", Author = "Иванова И.И.", Publisher = "Аверсев", YearPublished = "2008", ISBN = "978-1-23-456", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng9Class = new Book { Title = "Английский язык 9 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2009", ISBN = "978-2-34-567", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng10Class = new Book { Title = "Английский язык 10 класс", Author = "Сидоров С.С.", Publisher = "Аверсев", YearPublished = "2010", ISBN = "978-3-45-678", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookEng11Class = new Book { Title = "Английский язык 11 класс", Author = "Кузнецов К.К.", Publisher = "Просвещение", YearPublished = "2011", ISBN = "978-4-56-789", Category = english, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel1Class = new Book { Title = "Белорусский язык 1 класс", Author = "Щурок В.Ф.", Publisher = "Аверсев", YearPublished = "2001", ISBN = "978-5-67-890", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel2Class = new Book { Title = "Белорусский язык 2 класс", Author = "Головин Г.Г.", Publisher = "Просвещение", YearPublished = "2002", ISBN = "978-6-78-901", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel3Class = new Book { Title = "Белорусский язык 3 класс", Author = "Левин Л.Л.", Publisher = "Аверсев", YearPublished = "2003", ISBN = "978-7-89-012", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel4Class = new Book { Title = "Белорусский язык 4 класс", Author = "Рубинштейн Р.Р.", Publisher = "Просвещение", YearPublished = "2004", ISBN = "978-8-90-123", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel5Class = new Book { Title = "Белорусский язык 5 класс", Author = "Соколова С.С.", Publisher = "Аверсев", YearPublished = "2005", ISBN = "978-9-01-234", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel6Class = new Book { Title = "Белорусский язык 6 класс", Author = "Дмитриев Д.Д.", Publisher = "Просвещение", YearPublished = "2006", ISBN = "978-0-12-345", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel7Class = new Book { Title = "Белорусский язык 7 класс", Author = "Иванова И.И.", Publisher = "Аверсев", YearPublished = "2007", ISBN = "978-1-23-456", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel8Class = new Book { Title = "Белорусский язык 8 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2008", ISBN = "978-2-34-567", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel9Class = new Book { Title = "Белорусский язык 9 класс", Author = "Лесной С.С.", Publisher = "Аверсев", YearPublished = "2009", ISBN = "978-3-45-678", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel10Class = new Book { Title = "Белорусский язык 10 класс", Author = "Парфенок С.С.", Publisher = "Аверсев", YearPublished = "2009", ISBN = "978-3-45-678", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookBel11Class = new Book { Title = "Белорусский язык 11 класс", Author = "Кабушкин С.С.", Publisher = "Аверсев", YearPublished = "2009", ISBN = "978-3-45-678", Category = belarusian, Quantity = 10, QuantityLeft = 10 };
                       Book bookGeography6Class = new Book { Title = "География 6 класс", Author = "Некрасов В.В.", Publisher = "Экзамен", YearPublished = "2010", ISBN = "978-5-67-899", Category = geografy, Quantity = 10, QuantityLeft = 10 };
                       Book bookGeography7Class = new Book { Title = "География 7 класс", Author = "Гуртьев В.В.", Publisher = "Аверсев", YearPublished = "2010", ISBN = "978-5-67-899", Category = geografy, Quantity = 10, QuantityLeft = 10 };
                       Book bookGeography8Class = new Book { Title = "География 8 класс", Author = "Алексеев В.В.", Publisher = "Экзамен", YearPublished = "2006", ISBN = "978-5-67-899", Category = geografy, Quantity = 10, QuantityLeft = 10 };
                       Book bookGeography9Class = new Book { Title = "География 9 класс", Author = "Никифоров В.В.", Publisher = "Аверсев", YearPublished = "2011", ISBN = "978-5-67-899", Category = geografy, Quantity = 10, QuantityLeft = 10 };
                       Book bookGeography10Class = new Book { Title = "География 10 класс", Author = "Конашенко В.В.", Publisher = "Аверсев", YearPublished = "2011", ISBN = "978-5-67-899", Category = geografy, Quantity = 10, QuantityLeft = 10 };
                       Book bookGeography11Class = new Book { Title = "География 11 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2010", ISBN = "978-5-67-899", Category = geografy, Quantity = 10, QuantityLeft = 10 };
                       Book bookPhysics7Class = new Book { Title = "Физика 7 класс", Author = "Иванов И.И.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-000", Category = physics, Quantity = 10, QuantityLeft = 10 };
                       Book bookPhysics8Class = new Book { Title = "Физика 8 класс", Author = "Петров П.П.", Publisher = "Аверсев", YearPublished = "2016", ISBN = "978-1-23-001", Category = physics, Quantity = 10, QuantityLeft = 10 };
                       Book bookPhysics9Class = new Book { Title = "Физика 9 класс", Author = "Сидоров С.С.", Publisher = "Просвещение", YearPublished = "2017", ISBN = "978-1-23-002", Category = physics, Quantity = 10, QuantityLeft = 10 };
                       Book bookPhysics10Class = new Book { Title = "Физика 10 класс", Author = "Кузнецов К.К.", Publisher = "Аверсев", YearPublished = "2018", ISBN = "978-1-23-003", Category = physics, Quantity = 10, QuantityLeft = 10 };
                       Book bookPhysics11Class = new Book { Title = "Физика 11 класс", Author = "Михайлов М.М.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-004", Category = physics, Quantity = 10, QuantityLeft = 10 };
                       Book bookChemical7Class = new Book { Title = "Химия 7 класс", Author = "Васильев В.В.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-005", Category = chemistry, Quantity = 10, QuantityLeft = 10 };
                       Book bookChemical8Class = new Book { Title = "Химия 8 класс", Author = "Федоров Ф.Ф.", Publisher = "Аверсев", YearPublished = "2016", ISBN = "978-1-23-006", Category = chemistry, Quantity = 10, QuantityLeft = 10 };
                       Book bookChemical9Class = new Book { Title = "Химия 9 класс", Author = "Смирнов С.С.", Publisher = "Просвещение", YearPublished = "2017", ISBN = "978-1-23-007", Category = chemistry, Quantity = 10, QuantityLeft = 10 };
                       Book bookChemical10Class = new Book { Title = "Химия 10 класс", Author = "Николаев Н.Н.", Publisher = "Аверсев", YearPublished = "2018", ISBN = "978-1-23-008", Category = chemistry, Quantity = 10, QuantityLeft = 10 };
                       Book bookChemical11Class = new Book { Title = "Химия 11 класс", Author = "Попов П.П.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-009", Category = chemistry, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistoryBel6Class = new Book { Title = "История Беларуси 6 класс", Author = "Агутин Г.Г.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-010", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistoryBel7Class = new Book { Title = "История Беларуси 7 класс", Author = "Ломоносов Г.Г.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-010", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistoryBel8Class = new Book { Title = "История Беларуси 8 класс", Author = "Покемонов Г.Г.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-010", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistoryBel9Class = new Book { Title = "История Беларуси 9 класс", Author = "Юрьев Г.Г.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-010", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistoryBel10Class = new Book { Title = "История Беларуси 10 класс", Author = "Монахов Г.Г.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-010", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistoryBel11Class = new Book { Title = "История Беларуси 11 класс", Author = "Григорьев Г.Г.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-010", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistory5Class = new Book { Title = "История 5 класс", Author = "Воронцов В.В.", Publisher = "Аверсев", YearPublished = "2016", ISBN = "978-1-23-011", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistory6Class = new Book { Title = "История 6 класс", Author = "Капустин К.К.", Publisher = "Просвещение", YearPublished = "2017", ISBN = "978-1-23-012", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistory7Class = new Book { Title = "История 7 класс", Author = "Тихонов Т.Т.", Publisher = "Аверсев", YearPublished = "2018", ISBN = "978-1-23-013", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistory8Class = new Book { Title = "История 8 класс", Author = "Киселев К.К.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-014", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistory9Class = new Book { Title = "История 9 класс", Author = "Труханов К.К.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-014", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistory10Class = new Book { Title = "История 10 класс", Author = "Киселев К.К.", Publisher = "Известия", YearPublished = "2019", ISBN = "978-1-23-014", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookHistory11Class = new Book { Title = "История 11 класс", Author = "Норманов К.К.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-014", Category = history, Quantity = 10, QuantityLeft = 10 };
                       Book bookRuLiteratura5Class = new Book { Title = "Русская литература 5 класс", Author = "Ильин И.И.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-015", Category = literature, Quantity = 10, QuantityLeft = 10 };
                       Book bookRuLiteratura6Class = new Book { Title = "Русская литература 6 класс", Author = "Максимов М.М.", Publisher = "Аверсев", YearPublished = "2016", ISBN = "978-1-23-016", Category = literature, Quantity = 10, QuantityLeft = 10 };
                       Book bookRuLiteratura7Class = new Book { Title = "Русская литература 7 класс", Author = "Семенов С.С.", Publisher = "Просвещение", YearPublished = "2017", ISBN = "978-1-23-017", Category = literature, Quantity = 10, QuantityLeft = 10 };
                       Book bookRuLiteratura8Class = new Book { Title = "Русская литература 8 класс", Author = "Власов В.В.", Publisher = "Аверсев", YearPublished = "2018", ISBN = "978-1-23-018", Category = literature, Quantity = 10, QuantityLeft = 10 };
                       Book bookRuLiteratura9Class = new Book { Title = "Русская литература 9 класс", Author = "Ефимов Е.Е.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-019", Category = literature, Quantity = 10, QuantityLeft = 10 };
                       Book bookBiology6Class = new Book { Title = "Биология 6 класс", Author = "Морозов М.М.", Publisher = "Просвещение", YearPublished = "2015", ISBN = "978-1-23-020", Category = biology, Quantity = 10, QuantityLeft = 10 };
                       Book bookBiology7Class = new Book { Title = "Биология 7 класс", Author = "Гаврилов Г.Г.", Publisher = "Аверсев", YearPublished = "2016", ISBN = "978-1-23-021", Category = biology, Quantity = 10, QuantityLeft = 10 };
                       Book bookBiology8Class = new Book { Title = "Биология 8 класс", Author = "Михайлов М.М.", Publisher = "Просвещение", YearPublished = "2017", ISBN = "978-1-23-022", Category = biology, Quantity = 10, QuantityLeft = 10 };
                       Book bookBiology9Class = new Book { Title = "Биология 9 класс", Author = "Васильев В.В.", Publisher = "Аверсев", YearPublished = "2018", ISBN = "978-1-23-023", Category = biology, Quantity = 10, QuantityLeft = 10 };
                       Book bookBiology10Class = new Book { Title = "Биология 10 класс", Author = "Федоров Ф.Ф.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-024", Category = biology, Quantity = 10, QuantityLeft = 10 };
                       Book bookBiology11Class = new Book { Title = "Биология 11 класс", Author = "Федоров Ф.Ф.", Publisher = "Просвещение", YearPublished = "2019", ISBN = "978-1-23-024", Category = biology, Quantity = 10, QuantityLeft = 10 };

                       context.Books.AddRange(new Book[]
                       {
                           bookRu1Class, bookRu2Class, bookRu3Class, bookRu4Class, bookRu5Class,
                           bookRu6Class, bookRu7Class, bookRu8Class, bookRu9Class, bookRu10Class, bookRu11Class,
                           bookInf5Class, bookInf6Class, bookInf7Class, bookInf8Class, bookInf9Class,
                           bookInf10Class, bookInf11Class, bookEng3Class, bookEng4Class, bookEng5Class,
                           bookEng6Class, bookEng7Class, bookEng8Class, bookEng9Class, bookEng10Class,
                           bookEng11Class, bookBel1Class, bookBel2Class, bookBel3Class, bookBel4Class,
                           bookBel5Class, bookBel6Class, bookBel7Class, bookBel8Class, bookBel9Class,
                           bookBel10Class, bookBel11Class, bookGeography6Class, bookGeography7Class, bookGeography8Class,
                           bookGeography9Class, bookGeography10Class, bookGeography11Class, bookPhysics7Class, bookPhysics8Class,
                           bookPhysics9Class, bookPhysics10Class, bookPhysics11Class, bookChemical7Class, bookChemical8Class,
                           bookChemical9Class, bookChemical10Class, bookChemical11Class, bookHistoryBel6Class, bookHistoryBel7Class,
                           bookHistoryBel8Class, bookHistoryBel9Class, bookHistoryBel10Class, bookHistoryBel11Class, bookHistory5Class,
                           bookHistory6Class, bookHistory7Class, bookHistory8Class, bookHistory9Class, bookHistory10Class,
                           bookHistory11Class, bookRuLiteratura5Class, bookRuLiteratura6Class, bookRuLiteratura7Class, bookRuLiteratura8Class,
                           bookRuLiteratura9Class, bookBiology6Class, bookBiology7Class, bookBiology8Class, bookBiology9Class,
                           bookBiology10Class, bookBiology11Class, bookMath1Class, bookMath2Class, bookMath3Class,
                           bookMath4Class, bookMath5Class, bookMath6Class, bookMath7Class, bookMath8Class,
                           bookMath9Class, bookMath10Class, bookMath11Class
                       });*/
            /* context.Books.AddRange(new Book[] { });
             {*/
            /*
            context.Loans.AddRange(new Loan[]
               {

           // Loan loan1 = new Loan { Book = bookBel10Class, Student = ivanIvanov, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false };


<<<<<<< HEAD


                   new Loan { BookID = 1, StudentID = 1, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false },
=======
            context.Loans.AddRange(new Loan[]
               {
                                        new Loan { BookID = 1, StudentID = 1, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false },
>>>>>>> e7df47228f9744bf27b7531522afb52de91a54d8
                    new Loan { BookID = 2, StudentID = 2, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12), ReturnDate = DateTime.Now.AddDays(-1), Returned = true },
                    new Loan { BookID = 3, StudentID = 3, LoanDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(5), ReturnDate = null, Returned = false },
                    new Loan { BookID = 4, StudentID = 4, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(0), ReturnDate = DateTime.Now.AddDays(-5), Returned = true },
                    new Loan { BookID = 5, StudentID = 5, LoanDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(15), ReturnDate = null, Returned = false },
                    new Loan { BookID = 6, StudentID = 6, LoanDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(20), ReturnDate = null, Returned = false },
                    new Loan { BookID = 7, StudentID = 7, LoanDate = DateTime.Now.AddDays(-35), DueDate = DateTime.Now.AddDays(25), ReturnDate = null, Returned = false },
                    new Loan { BookID = 8, StudentID = 8, LoanDate = DateTime.Now.AddDays(-40), DueDate = DateTime.Now.AddDays(30), ReturnDate = null, Returned = false },
                    new Loan { BookID = 9, StudentID = 9, LoanDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(35), ReturnDate = DateTime.Now.AddDays(-10), Returned = true },
                    new Loan { BookID = 10, StudentID = 10, LoanDate = DateTime.Now.AddDays(-50), DueDate = DateTime.Now.AddDays(40), ReturnDate = null, Returned = false },
                    new Loan { BookID = 11, StudentID = 11, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false },
                    new Loan { BookID = 12, StudentID = 12, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12), ReturnDate = DateTime.Now.AddDays(-1), Returned = true },
                    new Loan { BookID = 13, StudentID = 13, LoanDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(5), ReturnDate = null, Returned = false },
                    new Loan { BookID = 14, StudentID = 14, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(0), ReturnDate = DateTime.Now.AddDays(-5), Returned = true },
                    new Loan { BookID = 15, StudentID = 15, LoanDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(15), ReturnDate = null, Returned = false },
                    new Loan { BookID = 16, StudentID = 16, LoanDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(20), ReturnDate = null, Returned = false },
                    new Loan { BookID = 17, StudentID = 17, LoanDate = DateTime.Now.AddDays(-35), DueDate = DateTime.Now.AddDays(25), ReturnDate = null, Returned = false },
                    new Loan { BookID = 18, StudentID = 18, LoanDate = DateTime.Now.AddDays(-40), DueDate = DateTime.Now.AddDays(30), ReturnDate = null, Returned = false },
                    new Loan { BookID = 19, StudentID = 19, LoanDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(35), ReturnDate = DateTime.Now.AddDays(-10), Returned = true },
                    new Loan { BookID = 20, StudentID = 20, LoanDate = DateTime.Now.AddDays(-50), DueDate = DateTime.Now.AddDays(40), ReturnDate = null, Returned = false },
                    new Loan { BookID = 21, StudentID = 21, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false },
                    new Loan { BookID = 22, StudentID = 22, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12), ReturnDate = DateTime.Now.AddDays(-1), Returned = true },
                    new Loan { BookID = 23, StudentID = 23, LoanDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(5), ReturnDate = null, Returned = false },
                    new Loan { BookID = 24, StudentID = 24, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(0), ReturnDate = DateTime.Now.AddDays(-5), Returned = true },
                    new Loan { BookID = 25, StudentID = 25, LoanDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(15), ReturnDate = null, Returned = false },
                    new Loan { BookID = 26, StudentID = 26, LoanDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(20), ReturnDate = null, Returned = false },
                    new Loan { BookID = 27, StudentID = 27, LoanDate = DateTime.Now.AddDays(-35), DueDate = DateTime.Now.AddDays(25), ReturnDate = null, Returned = false },
                    new Loan { BookID = 28, StudentID = 28, LoanDate = DateTime.Now.AddDays(-40), DueDate = DateTime.Now.AddDays(30), ReturnDate = null, Returned = false },
                    new Loan { BookID = 29, StudentID = 29, LoanDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(35), ReturnDate = DateTime.Now.AddDays(-10), Returned = true },
                    new Loan { BookID = 30, StudentID = 30, LoanDate = DateTime.Now.AddDays(-50), DueDate = DateTime.Now.AddDays(40), ReturnDate = null, Returned = false },
                    new Loan { BookID = 31, StudentID = 31, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false },
                    new Loan { BookID = 32, StudentID = 32, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12), ReturnDate = DateTime.Now.AddDays(-1), Returned = true },
                    new Loan { BookID = 33, StudentID = 33, LoanDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(5), ReturnDate = null, Returned = false },
                    new Loan { BookID = 34, StudentID = 34, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(0), ReturnDate = DateTime.Now.AddDays(-5), Returned = true },
                    new Loan { BookID = 35, StudentID = 35, LoanDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(15), ReturnDate = null, Returned = false },
                    new Loan { BookID = 36, StudentID = 36, LoanDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(20), ReturnDate = null, Returned = false },
                    new Loan { BookID = 37, StudentID = 37, LoanDate = DateTime.Now.AddDays(-35), DueDate = DateTime.Now.AddDays(25), ReturnDate = null, Returned = false },
                    new Loan { BookID = 38, StudentID = 38, LoanDate = DateTime.Now.AddDays(-40), DueDate = DateTime.Now.AddDays(30), ReturnDate = null, Returned = false },
                    new Loan { BookID = 39, StudentID = 39, LoanDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(35), ReturnDate = DateTime.Now.AddDays(-10), Returned = true },
                    new Loan { BookID = 40, StudentID = 40, LoanDate = DateTime.Now.AddDays(-50), DueDate = DateTime.Now.AddDays(40), ReturnDate = null, Returned = false },
                    new Loan { BookID = 41, StudentID = 41, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false },
                    new Loan { BookID = 42, StudentID = 42, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12), ReturnDate = DateTime.Now.AddDays(-1), Returned = true },
                    new Loan { BookID = 43, StudentID = 43, LoanDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(5), ReturnDate = null, Returned = false },
                    new Loan { BookID = 44, StudentID = 44, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(0), ReturnDate = DateTime.Now.AddDays(-5), Returned = true },
                    new Loan { BookID = 45, StudentID = 45, LoanDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(15), ReturnDate = null, Returned = false },
                    new Loan { BookID = 46, StudentID = 46, LoanDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(20), ReturnDate = null, Returned = false },
                    new Loan { BookID = 47, StudentID = 47, LoanDate = DateTime.Now.AddDays(-35), DueDate = DateTime.Now.AddDays(25), ReturnDate = null, Returned = false },
                    new Loan { BookID = 48, StudentID = 48, LoanDate = DateTime.Now.AddDays(-40), DueDate = DateTime.Now.AddDays(30), ReturnDate = null, Returned = false },
                    new Loan { BookID = 49, StudentID = 49, LoanDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(35), ReturnDate = DateTime.Now.AddDays(-10), Returned = true },
                    new Loan { BookID = 50, StudentID = 50, LoanDate = DateTime.Now.AddDays(-50), DueDate = DateTime.Now.AddDays(40), ReturnDate = null, Returned = false },
                    new Loan { BookID = 51, StudentID = 51, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10), ReturnDate = null, Returned = false },
                    new Loan { BookID = 52, StudentID = 52, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12), ReturnDate = DateTime.Now.AddDays(-1), Returned = true },
                    new Loan { BookID = 53, StudentID = 53, LoanDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(5), ReturnDate = null, Returned = false },
                    new Loan { BookID = 54, StudentID = 54, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(0), ReturnDate = DateTime.Now.AddDays(-5), Returned = true },
                    new Loan { BookID = 55, StudentID = 55, LoanDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(15), ReturnDate = null, Returned = false },
                    new Loan { BookID = 56, StudentID = 56, LoanDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(20), ReturnDate = null, Returned = false },
                    new Loan { BookID = 57, StudentID = 57, LoanDate = DateTime.Now.AddDays(-35), DueDate = DateTime.Now.AddDays(25), ReturnDate = null, Returned = false },
                    new Loan { BookID = 58, StudentID = 58, LoanDate = DateTime.Now.AddDays(-40), DueDate = DateTime.Now.AddDays(30), ReturnDate = null, Returned = false },
                    new Loan { BookID = 59, StudentID = 59, LoanDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(35), ReturnDate = DateTime.Now.AddDays(-10), Returned = true },
                    new Loan { BookID = 60, StudentID = 60, LoanDate = DateTime.Now.AddDays(-50), DueDate = DateTime.Now.AddDays(40), ReturnDate = null, Returned = false },
<<<<<<< HEAD
                });*/


            //          Loan loan1 = new Loan { Book = booksMath1Class, Student = ivanIvanov, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10).Date, ReturnDate = null, Returned = false };



            
 //           Loan loan1 = new Loan { InventoryBook = bookMath1Class1, Student = ivanIvanov, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10).Date, ReturnDate = null, Returned = false };
//            Loan loan2 = new Loan { InventoryBook = inventoryEnglish3Book1, Student = ivanIvanov, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10).Date, ReturnDate = null, Returned = false };
 //           Loan loan3 = new Loan { InventoryBook = inventoryEnglish3Book2, Student = mariyaPetrova, LoanDate = DateTime.Now.AddDays(-9), DueDate = DateTime.Now.AddDays(11).Date, ReturnDate = null, Returned = false };
           /* Loan loan2 = new Loan { Book = bookBel9Class, Student = mariyaPetrova, LoanDate = DateTime.Now.AddDays(-9), DueDate = DateTime.Now.AddDays(11).Date, ReturnDate = null, Returned = false };
            Loan loan3 = new Loan { Book = bookBel8Class, Student = alexeySmirnov, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12).Date, ReturnDate = null, Returned = false };
            Loan loan4 = new Loan { Book = bookBel7Class, Student = ekaterinaKozlova, LoanDate = DateTime.Now.AddDays(-7), DueDate = DateTime.Now.AddDays(13).Date, ReturnDate = null, Returned = false };
            Loan loan5 = new Loan { Book = bookBel6Class, Student = artemFedorov, LoanDate = DateTime.Now.AddDays(-6), DueDate = DateTime.Now.AddDays(14).Date, ReturnDate = null, Returned = false };
            Loan loan6 = new Loan { Book = bookBel10Class, Student = ivanIvanov, LoanDate = DateTime.Now.AddDays(-5), DueDate = DateTime.Now.AddDays(15).Date, ReturnDate = null, Returned = false };
            Loan loan7 = new Loan { Book = bookBel9Class, Student = mariyaPetrova, LoanDate = DateTime.Now.AddDays(-4), DueDate = DateTime.Now.AddDays(16).Date, ReturnDate = null, Returned = false };
            Loan loan8 = new Loan { Book = bookBel8Class, Student = alexeySmirnov, LoanDate = DateTime.Now.AddDays(-3), DueDate = DateTime.Now.AddDays(17).Date, ReturnDate = null, Returned = false };
            Loan loan9 = new Loan { Book = bookBel7Class, Student = ekaterinaKozlova, LoanDate = DateTime.Now.AddDays(-2), DueDate = DateTime.Now.AddDays(18).Date, ReturnDate = null, Returned = false };
            Loan loan10 = new Loan { Book = bookBel6Class, Student = artemFedorov, LoanDate = DateTime.Now.AddDays(-1), DueDate = DateTime.Now.AddDays(19).Date, ReturnDate = null, Returned = false };
            Loan loan11 = new Loan { Book = bookBel10Class, Student = ivanIvanov, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(20).Date, ReturnDate = null, Returned = false };
            Loan loan12 = new Loan { Book = bookBel9Class, Student = mariyaPetrova, LoanDate = DateTime.Now.AddDays(1), DueDate = DateTime.Now.AddDays(21).Date, ReturnDate = null, Returned = false };
            Loan loan13 = new Loan { Book = bookBel8Class, Student = alexeySmirnov, LoanDate = DateTime.Now.AddDays(2), DueDate = DateTime.Now.AddDays(22).Date, ReturnDate = null, Returned = false };
            Loan loan14 = new Loan { Book = bookBel7Class, Student = ekaterinaKozlova, LoanDate = DateTime.Now.AddDays(3), DueDate = DateTime.Now.AddDays(23).Date, ReturnDate = null, Returned = false };
            Loan loan15 = new Loan { Book = bookBel6Class, Student = artemFedorov, LoanDate = DateTime.Now.AddDays(4), DueDate = DateTime.Now.AddDays(24).Date, ReturnDate = null, Returned = false };
            Loan loan16 = new Loan { Book = bookBel10Class, Student = ivanIvanov, LoanDate = DateTime.Now.AddDays(5), DueDate = DateTime.Now.AddDays(25).Date, ReturnDate = null, Returned = false };
            Loan loan17 = new Loan { Book = bookBel9Class, Student = mariyaPetrova, LoanDate = DateTime.Now.AddDays(6), DueDate = DateTime.Now.AddDays(26).Date, ReturnDate = null, Returned = false };
            Loan loan18 = new Loan { Book = bookBel8Class, Student = alexeySmirnov, LoanDate = DateTime.Now.AddDays(7), DueDate = DateTime.Now.AddDays(27).Date, ReturnDate = null, Returned = false };
            Loan loan19 = new Loan { Book = bookBel7Class, Student = ekaterinaKozlova, LoanDate = DateTime.Now.AddDays(8), DueDate = DateTime.Now.AddDays(28).Date, ReturnDate = null, Returned = false };
            Loan loan20 = new Loan { Book = bookBel6Class, Student = artemFedorov, LoanDate = DateTime.Now.AddDays(9), DueDate = DateTime.Now.AddDays(29).Date, ReturnDate = null, Returned = false };
            Loan loan51 = new Loan { Book = bookMath11Class, Student = ekaterinaKozlova, LoanDate = DateTime.Now.AddDays(-10), DueDate = DateTime.Now.AddDays(10).Date, ReturnDate = null, Returned = false };
            Loan loan52 = new Loan { Book = bookMath10Class, Student = ivanIvanov, LoanDate = DateTime.Now.AddDays(-8), DueDate = DateTime.Now.AddDays(12).Date, ReturnDate = DateTime.Now.AddDays(-1).Date, Returned = true };
            Loan loan53 = new Loan { Book = bookMath10Class, Student = artemFedorov, LoanDate = DateTime.Now.AddDays(-15), DueDate = DateTime.Now.AddDays(5).Date, ReturnDate = null, Returned = false };
            Loan loan54 = new Loan { Book = bookMath9Class, Student = mariyaPetrova, LoanDate = DateTime.Now.AddDays(-20), DueDate = DateTime.Now.AddDays(0).Date, ReturnDate = DateTime.Now.AddDays(-5).Date, Returned = true };
            Loan loan55 = new Loan { Book = bookMath10Class, Student = alexeySmirnov, LoanDate = DateTime.Now.AddDays(-25), DueDate = DateTime.Now.AddDays(15).Date, ReturnDate = null, Returned = false };
            Loan loan56 = new Loan { Book = bookMath11Class, Student = nadezhdaIvanova, LoanDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(20).Date, ReturnDate = null, Returned = false };
            Loan loan57 = new Loan { Book = bookMath10Class, Student = elizavetaFedorova, LoanDate = DateTime.Now.AddDays(-35), DueDate = DateTime.Now.AddDays(25).Date, ReturnDate = null, Returned = false };
            Loan loan58 = new Loan { Book = bookMath8Class, Student = anastasiaIvanova, LoanDate = DateTime.Now.AddDays(-40), DueDate = DateTime.Now.AddDays(30).Date, ReturnDate = null, Returned = false };
            Loan loan59 = new Loan { Book = bookMath11Class, Student = anastasiaMikhaylova, LoanDate = DateTime.Now.AddDays(-45), DueDate = DateTime.Now.AddDays(35).Date, ReturnDate = DateTime.Now.AddDays(-10).Date, Returned = true };
            Loan loan60 = new Loan { Book = bookMath10Class, Student = nikitaKuznetsov, LoanDate = DateTime.Now.AddDays(-50), DueDate = DateTime.Now.AddDays(40).Date, ReturnDate = null, Returned = false };*/
            // Добавление переменных Loan в контекст
            //context.Loans.AddRange(new Loan[] { loan1, loan2, loan3, loan4, loan5, loan6, loan7, loan8, loan9, loan10, loan11, loan12, loan13, loan14, loan15, loan16, loan17, loan18, loan19, loan20, loan51, loan52, loan53, loan54, loan55, loan56, loan57, loan58, loan59, loan60 });
//                     context.Loans.AddRange(new Loan[] { loan1, loan2, loan3});



            /*

            //   context.AvailableBooks.AddRange(new AvailableBook[] { bookMath2Class, bookMath1Class });
            // Получаем список всех книг
            var allBooks = context.Books.ToList();

            // Получаем список книг, которые находятся в займах
            var booksInLoans = context.Loans.Select(l => l.BookID).Distinct().ToList();

            // Создаем список доступных книг, исключая те, которые находятся в займах
            var availableBooks = allBooks.Where(b => !booksInLoans.Contains(b.BookID)).ToList();

            // Создаем объекты AvailableBook на основе доступных книг
            var availableBooksToAdd = availableBooks.Select(b => new AvailableBook
            {
                BookID = b.BookID,
                Title = b.Title,
                Author = b.Author,
                Publisher = b.Publisher,
                YearPublished = b.YearPublished,
                ISBN = b.ISBN,
                CategoryID = b.CategoryID,
                Category = b.Category,
                Quantity = b.Quantity,
               // Loans = b.Loans // Возможно, вам не нужно копировать здесь связь Loans
            }).ToList();

            // Добавляем созданные AvailableBook в контекст
            context.AvailableBooks.AddRange(availableBooksToAdd);

            // Сохраняем изменения в базе данных
            context.SaveChanges();

            base.Seed(context);

            */


        }
    }




}

