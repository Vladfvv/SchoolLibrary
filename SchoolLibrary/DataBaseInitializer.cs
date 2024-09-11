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
using SchoolLibrary.Converters;
using System.Security.Cryptography;

namespace SchoolLibrary
{
    //чтобы новая база данных при создании заполнялась
    // исходными данными, создаем класс инициализации базы данных
    class DataBaseInitializer : DropCreateDatabaseIfModelChanges<EntityContext>
    {

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

            Student ivanIvanov = new Student { FirstName = "Иван", LastName = "Иванов", DateOfBirth = new DateTime(2008, 9, 1), StudentClass = "10", Prefix = "A", Address = "Минск, ул. Притыцкого, 1", IsActive = true, Phone = "+375447654321" };
            Student mariyaPetrova = new Student { FirstName = "Мария", LastName = "Петрова", DateOfBirth = new DateTime(2008, 9, 1), StudentClass = "9", Prefix = "B", Address = "Минск, ул. Ленина, 20", IsActive = true, Phone = "+375447654666" };
            Student alexeySmirnov = new Student { FirstName = "Алексей", LastName = "Смирнов", DateOfBirth = new DateTime(2009, 9, 1), StudentClass = "8", Prefix = "C", Address = "Минск, пр. Независимости, 30", IsActive = true, Phone = "+375447657777" };
            Student ekaterinaKozlova = new Student { FirstName = "Екатерина", LastName = "Козлова", DateOfBirth = new DateTime(2010, 9, 1), StudentClass = "7", Prefix = "A", Address = "Минск, ул. Московская, 15", IsActive = true, Phone = "+37544555543" };
            Student artemFedorov = new Student { FirstName = "Артем", LastName = "Федоров", DateOfBirth = new DateTime(2011, 9, 1), StudentClass = "6", Prefix = "B", Address = "Минск, ул. Советская, 40", IsActive = true, Phone = "+375447654388" };
            Student anastasiaMikhaylova = new Student { FirstName = "Анастасия", LastName = "Михайлова", DateOfBirth = new DateTime(2012, 9, 1), StudentClass = "5", Prefix = "C", Address = "Минск, ул. Романовская, 25", IsActive = true, Phone = "+375447651111" };
            Student dmitriyNikolaev = new Student { FirstName = "Дмитрий", LastName = "Николаев", DateOfBirth = new DateTime(2013, 9, 1), StudentClass = "4", Prefix = "A", Address = "Минск, ул. Сурганова, 5", IsActive = true, Phone = "+3754476524242" };
            Student sofiaIvanova = new Student { FirstName = "София", LastName = "Иванова", DateOfBirth = new DateTime(2014, 9, 1), StudentClass = "3", Prefix = "B", Address = "Минск, ул. Станиславского, 10", IsActive = true, Phone = "+375447654001" };
            Student maximSmirnov = new Student { FirstName = "Максим", LastName = "Смирнов", DateOfBirth = new DateTime(2015, 9, 1), StudentClass = "2", Prefix = "C", Address = "Минск, ул. Купалы, 12", IsActive = true, Phone = "+37544761221" };
            Student alisaPetrova = new Student { FirstName = "Алиса", LastName = "Петрова", DateOfBirth = new DateTime(2016, 9, 1), StudentClass = "1", Prefix = "A", Address = "Минск, ул. Пушкина, 8", IsActive = true, Phone = "+375447677721" };
            Student nikitaKuznetsov = new Student { FirstName = "Никита", LastName = "Кузнецов", DateOfBirth = new DateTime(2008, 9, 1), StudentClass = "10", Prefix = "B", Address = "Минск, ул. Червякова, 22", IsActive = true, Phone = "+375295654321" };
            Student annaSidorova = new Student { FirstName = "Анна", LastName = "Сидорова", DateOfBirth = new DateTime(2008, 9, 1), StudentClass = "9", Prefix = "A", Address = "Минск, ул. Зелёная, 33", IsActive = true, Phone = "+375257654901" };
            Student artemiyFedorov = new Student { FirstName = "Артемий", LastName = "Федоров", DateOfBirth = new DateTime(2009, 9, 1), StudentClass = "8", Prefix = "B", Address = "Минск, ул. Орловская, 44", IsActive = true, Phone = "+3754476567821" };
            Student polinaIvanova = new Student { FirstName = "Полина", LastName = "Иванова", DateOfBirth = new DateTime(2010, 9, 1), StudentClass = "7", Prefix = "C", Address = "Минск, ул. Щербакова, 55", IsActive = true, Phone = "+375447654441" };
            Student egorPetrov = new Student { FirstName = "Егор", LastName = "Петров", DateOfBirth = new DateTime(2011, 9, 1), StudentClass = "6", Prefix = "A", Address = "Минск, ул. Белинского, 66", IsActive = true, Phone = "+375447609891" };
            Student kseniaKozlova = new Student { FirstName = "Ксения", LastName = "Козлова", DateOfBirth = new DateTime(2012, 9, 1), StudentClass = "5", Prefix = "B", Address = "Минск, ул. Янки Купалы, 77", IsActive = true, Phone = "+3754476776321" };
            Student alexandraSmirnova = new Student { FirstName = "Александра", LastName = "Смирнова", DateOfBirth = new DateTime(2013, 9, 1), StudentClass = "4", Prefix = "C", Address = "Минск, ул. Тимирязева, 88", IsActive = true, Phone = "+375299054321" };
            Student mikhailNikolaev = new Student { FirstName = "Михаил", LastName = "Николаев", DateOfBirth = new DateTime(2014, 9, 1), StudentClass = "3", Prefix = "A", Address = "Минск, ул. Гражданская, 99", IsActive = true, Phone = "+375339870980" };
            Student elizavetaFedorova = new Student { FirstName = "Елизавета", LastName = "Федорова", DateOfBirth = new DateTime(2015, 9, 1), StudentClass = "2", Prefix = "B", Address = "Минск, ул. Гагарина, 100", IsActive = true, Phone = "+375299283888" };
            Student ilyaIvanov = new Student { FirstName = "Илья", LastName = "Иванов", DateOfBirth = new DateTime(2016, 9, 1), StudentClass = "1", Prefix = "C", Address = "Минск, ул. Воронежская, 101", IsActive = true, Phone = "+375297609123" };
            Student pavelKuznetsov = new Student { FirstName = "Павел", LastName = "Кузнецов", DateOfBirth = new DateTime(2008, 9, 1), StudentClass = "10", Prefix = "A", Address = "Минск, ул. Дружбы, 102", IsActive = true, Phone = "+375447898989" };
            Student valeriaSidorova = new Student { FirstName = "Валерия", LastName = "Сидорова", DateOfBirth = new DateTime(2008, 9, 1), StudentClass = "9", Prefix = "B", Address = "Минск, ул. Калинина, 103", IsActive = true, Phone = "+375447643431" };
            Student vladimirFedorov = new Student { FirstName = "Владимир", LastName = "Федоров", DateOfBirth = new DateTime(2009, 9, 1), StudentClass = "8", Prefix = "C", Address = "Минск, ул. Пролетарская, 104", IsActive = true, Phone = "+375296544321" };
            Student anastasiaIvanova = new Student { FirstName = "Анастасия", LastName = "Иванова", DateOfBirth = new DateTime(2010, 9, 1), StudentClass = "7", Prefix = "A", Address = "Минск, ул. Октябрьская, 105", IsActive = true, Phone = "+375291234321" };
            Student sergeyPetrov = new Student { FirstName = "Сергей", LastName = "Петров", DateOfBirth = new DateTime(2011, 9, 1), StudentClass = "6", Prefix = "B", Address = "Минск, ул. Белорусская, 106", IsActive = true, Phone = "+375299394845" };
            Student daryaKozlova = new Student { FirstName = "Дарья", LastName = "Козлова", DateOfBirth = new DateTime(2012, 9, 1), StudentClass = "5", Prefix = "C", Address = "Минск, ул. Тургенева, 107", IsActive = true, Phone = "+375293334455" };
            Student alexeySmirnov2 = new Student { FirstName = "Алексей", LastName = "Смирнов", DateOfBirth = new DateTime(2013, 9, 1), StudentClass = "4", Prefix = "A", Address = "Минск, ул. Крылова, 108", IsActive = true, Phone = "+375337565666" };
            Student elenaNikolaeva = new Student { FirstName = "Елена", LastName = "Николаева", DateOfBirth = new DateTime(2014, 9, 1), StudentClass = "3", Prefix = "B", Address = "Минск, ул. Грибоедова, 109", IsActive = true, Phone = "+375259056784" };
            Student pavelSidorov = new Student { FirstName = "Павел", LastName = "Сидоров", DateOfBirth = new DateTime(2015, 9, 1), StudentClass = "2", Prefix = "C", Address = "Минск, ул. Белинского, 110", IsActive = true, Phone = "+375333454321" };
            Student nataliaFedorova = new Student { FirstName = "Наталья", LastName = "Федорова", DateOfBirth = new DateTime(2016, 9, 1), StudentClass = "1", Prefix = "A", Address = "Минск, ул. Маяковского, 111", IsActive = true, Phone = "+375296654321" };
            Student aleksandrEmelyanov = new Student { FirstName = "Александр", LastName = "Емельянов", DateOfBirth = new DateTime(2016, 9, 1), StudentClass = "5", Prefix = "В", Address = "Минск, ул. Нестерова, 2", IsActive = true, Phone = "+375296745327" };

            // Добавляем студентов в контекст
            context.Students.AddRange(new[]        {
            ivanIvanov, mariyaPetrova, alexeySmirnov, ekaterinaKozlova, artemFedorov,
            anastasiaMikhaylova, dmitriyNikolaev, sofiaIvanova, maximSmirnov, alisaPetrova,
            nikitaKuznetsov, annaSidorova, artemiyFedorov, polinaIvanova, egorPetrov,
            kseniaKozlova, alexandraSmirnova, mikhailNikolaev, elizavetaFedorova, ilyaIvanov,
            pavelKuznetsov, valeriaSidorova, vladimirFedorov, anastasiaIvanova, sergeyPetrov,
            daryaKozlova, alexeySmirnov2, elenaNikolaeva, pavelSidorov, nataliaFedorova, aleksandrEmelyanov
            });

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

            //// Сохраняем изменения
            context.SaveChanges();

            // Создаем жанр "Учебная литература"
            var educationalLiteratureGenre = new Genre("Учебная литература");

            // Добавляем жанр в контекст и сохраняем изменения, чтобы получить GenreID
            context.Genres.Add(educationalLiteratureGenre);
            context.SaveChanges();

            // Создаем предметы и связываем их с существующим жанром "Учебная литература"           
            var math2 = new Subject { SubjectName = "Математика", GenreID = educationalLiteratureGenre.GenreID };
            var eng2 = new Subject { SubjectName = "Английский", GenreID = educationalLiteratureGenre.GenreID };
            var physics2 = new Subject { SubjectName = "Физика", GenreID = educationalLiteratureGenre.GenreID };
            var chemistry2 = new Subject { SubjectName = "Химия", GenreID = educationalLiteratureGenre.GenreID };
            var ruLang2 = new Subject { SubjectName = "Русский язык", GenreID = educationalLiteratureGenre.GenreID };
            var hist2 = new Subject { SubjectName = "История", GenreID = educationalLiteratureGenre.GenreID };
            var biology2 = new Subject { SubjectName = "Биология", GenreID = educationalLiteratureGenre.GenreID };
            var geography2 = new Subject { SubjectName = "География", GenreID = educationalLiteratureGenre.GenreID };


            // Добавляем предметы в контекст
            context.Subjects.AddRange(new[] { math2, eng2, physics2, chemistry2, ruLang2, hist2, biology2, geography2 });
            // context.Subjects.AddRange(subjects);
            context.SaveChanges(); // Сохраняем изменения в базе данных

            // Создаем жанры "Художественная литература", "Детектив", "Ужасы", "Комедия"
            var fictionGenre = new Genre("Художественная литература");
            var mysteryGenre = new Genre("Детектив");
            var horrorGenre = new Genre("Ужасы");
            var comedyGenre = new Genre("Комедия");

            // Добавляем жанры в контекст и сохраняем изменения
            context.Genres.AddRange(new Genre[] { fictionGenre, mysteryGenre, horrorGenre, comedyGenre });
            context.SaveChanges();

            // Создаем предметы для не учебной литературы
            var nonEducationalSubjects = new[]
            {
                new Subject { SubjectName = "Без предмета", GenreID = fictionGenre.GenreID },
                new Subject { SubjectName = "Без предмета", GenreID = mysteryGenre.GenreID },
                new Subject { SubjectName = "Без предмета", GenreID = horrorGenre.GenreID },
                new Subject { SubjectName = "Без предмета", GenreID = comedyGenre.GenreID }
            };

            // Добавляем предметы в контекст и сохраняем изменения
            context.Subjects.AddRange(nonEducationalSubjects);
            context.SaveChanges();

            InventoryBook inventoryBookMath1Class1 = new InventoryBook
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

            InventoryBook inventoryBookMath1Class2 = new InventoryBook
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
            Book booksMath1Class = new Book         {
                Class = 1, Description = "Учебник по математике для начальных классов", //CategoryID = 4,
                Quantity = 2, QuantityLeft = 2,
                Genre = educationalLiteratureGenre, Subject = math2,
                InventoryBooks = new List<InventoryBook> { inventoryBookMath1Class1, inventoryBookMath1Class2 }
            };

            // Сохраняем изменения
            context.SaveChanges();

            // Устанавливаем связь между InventoryBook и Book
            inventoryBookMath1Class1.Book = booksMath1Class;
            inventoryBookMath1Class2.Book = booksMath1Class;

            // Создаем объект BookPhoto
            var bookPhoto1 = new BookPhoto
            {
                PhotoData = BookPhoto.LoadImage("Resources/22412.jpg"),
                DateAdded = DateTime.Now,
                Book = booksMath1Class
            };

            // Добавляем объекты в контекст один раз
            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookMath1Class1, inventoryBookMath1Class2 });
            context.Books.Add(booksMath1Class);
            context.BookPhotos.Add(bookPhoto1);
            // Сохраняем изменения
            context.SaveChanges();



            InventoryBook inventoryBookMath11Class1 = new InventoryBook
            {
                InventoryNumber = "30",
                ISBN = "979-5-907179-47-6",
                Title = "Математика 11 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2020",
                Price = 80.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123450 ТЕ"
            };
            InventoryBook inventoryBookMath11Class2 = new InventoryBook
            {
                InventoryNumber = "31",
                ISBN = "979-5-907179-47-6",
                Title = "Математика 11 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2020",
                Price = 85.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "121111 ТЕ"
            };
            InventoryBook inventoryBookMath11Class3 = new InventoryBook
            {
                InventoryNumber = "32",
                ISBN = "979-5-907179-47-6",
                Title = "Математика 11 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2020",
                Price = 88.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "121115 ТЕ"
            };


            // Создаем объект Book
            Book booksMath11Class = new Book
            {
                Class = 11,
                Description = "Учебник по математике 11 класс", 
                Quantity = 3,
                QuantityLeft = 3,
                Genre = educationalLiteratureGenre,
                Subject = math2,
                InventoryBooks = new List<InventoryBook> { inventoryBookMath11Class1, inventoryBookMath11Class2, inventoryBookMath11Class3 }
            };
            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookMath11Class1, inventoryBookMath11Class2, inventoryBookMath11Class3 });
            context.Books.Add(booksMath11Class);


            //10класс математика
            InventoryBook inventoryBookMath10Class1 = new InventoryBook
            {
                InventoryNumber = "33",
                ISBN = "979-5-905559-47-6",
                Title = "Математика 10 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2018",
                Price = 45.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123450 ТЕ"
            };
            InventoryBook inventoryBookMath10Class2 = new InventoryBook
            {
                InventoryNumber = "34",
                ISBN = "979-5-905559-47-6",
                Title = "Математика 10 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2018",
                Price = 48.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123551 ТЕ"
            };
            InventoryBook inventoryBookMath10Class3 = new InventoryBook
            {
                InventoryNumber = "35",
                ISBN = "979-5-905559-47-6",
                Title = "Математика 10 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2018",
                Price = 50.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "120000 ТЕ"
            };
            InventoryBook inventoryBookMath10Class4 = new InventoryBook
            {
                InventoryNumber = "36",
                ISBN = "979-5-905559-47-6",
                Title = "Математика 10 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2018",
                Price = 52.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "121115 ТЕ"
            };


            // Создаем объект Book
            Book booksMath10Class = new Book
            {
                Class = 11,
                Description = "Учебник по математике 10 класс", 
                Quantity = 4,
                QuantityLeft = 4,
                Genre = educationalLiteratureGenre,
                Subject = math2,
                InventoryBooks = new List<InventoryBook> { inventoryBookMath10Class1, inventoryBookMath10Class2, inventoryBookMath10Class3, inventoryBookMath10Class4 }
            };
            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookMath10Class1, inventoryBookMath10Class2, inventoryBookMath10Class3, inventoryBookMath10Class4 });
            context.Books.Add(booksMath10Class);


            //9класс математика
            InventoryBook inventoryBookMath9Class1 = new InventoryBook
            {
                InventoryNumber = "37",
                ISBN = "979-5-905550-47-6",
                Title = "Математика 9 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2019",
                Price = 45.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123450 ТЕ"
            };
            InventoryBook inventoryBookMath9Class2 = new InventoryBook
            {
                InventoryNumber = "38",
                ISBN = "979-5-905550-47-6",
                Title = "Математика 9 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2019",
                Price = 48.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123551 ТЕ"
            };
            InventoryBook inventoryBookMath9Class3 = new InventoryBook
            {
                InventoryNumber = "39",
                ISBN = "979-5-905550-47-6",
                Title = "Математика 9 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2019",
                Price = 50.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "120000 ТЕ"
            };
            InventoryBook inventoryBookMath9Class4 = new InventoryBook
            {
                InventoryNumber = "40",
                ISBN = "979-5-905550-47-6",
                Title = "Математика 9 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2019",
                Price = 52.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "121115 ТЕ"
            };


            // Создаем объект Book
            Book booksMath9Class = new Book
            {
                Class = 11,
                Description = "Учебник по математике 9 класс",
                Quantity = 4,
                QuantityLeft = 4,
                Genre = educationalLiteratureGenre,
                Subject = math2,
                InventoryBooks = new List<InventoryBook> { inventoryBookMath9Class1, inventoryBookMath9Class2, inventoryBookMath9Class3, inventoryBookMath9Class4 }
            };
            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookMath9Class1, inventoryBookMath9Class2, inventoryBookMath9Class3, inventoryBookMath9Class4 });
            context.Books.Add(booksMath9Class);



            //9класс математика
            InventoryBook inventoryBookMath8Class1 = new InventoryBook
            {
                InventoryNumber = "41",
                ISBN = "979-5-911150-47-6",
                Title = "Математика 8 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2017",
                Price = 40.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123411 ТЕ"
            };
            InventoryBook inventoryBookMath8Class2 = new InventoryBook
            {
                InventoryNumber = "42",
                ISBN = "979-5-911150-47-6",
                Title = "Математика 8 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2017",
                Price = 42.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123771 ТЕ"
            };
            InventoryBook inventoryBookMath8Class3 = new InventoryBook
            {
                InventoryNumber = "43",
                ISBN = "979-5-911150-47-6",
                Title = "Математика 8 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2017",
                Price = 45.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123611 ТЕ"
            };
            InventoryBook inventoryBookMath8Class4 = new InventoryBook
            {
                InventoryNumber = "44",
                ISBN = "979-5-911150-47-6",
                Title = "Математика 8 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2017",
                Price = 40.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "128811 ТЕ"
            };
            InventoryBook inventoryBookMath8Class5 = new InventoryBook
            {
                InventoryNumber = "45",
                ISBN = "979-5-911150-47-6",
                Title = "Математика 8 класс",
                Author = "Щурок В.В.",
                Publisher = "Асвета",
                YearPublished = "2017",
                Price = 50.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "120411 ТЕ"
            };

            // Создаем объект Book
            Book booksMath8Class = new Book
            {
                Class = 11,
                Description = "Учебник по математике 8 класс",
                Quantity = 5,
                QuantityLeft = 5,
                Genre = educationalLiteratureGenre,
                Subject = math2,
                InventoryBooks = new List<InventoryBook> { inventoryBookMath8Class1, inventoryBookMath8Class2, inventoryBookMath8Class3, inventoryBookMath8Class4, inventoryBookMath8Class5 }
            };
            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookMath8Class1, inventoryBookMath8Class2, inventoryBookMath8Class3, inventoryBookMath8Class4, inventoryBookMath8Class5 });
            context.Books.Add(booksMath8Class);


            //английский язык
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
                Quantity = 2,  QuantityLeft = 2,
                Genre = educationalLiteratureGenre,
                Subject = eng2,
                //Category = english,
                InventoryBooks = new List<InventoryBook> { inventoryEnglish3Book1, inventoryEnglish3Book2 }};

            // Устанавливаем связь между InventoryBook и Book
            inventoryEnglish3Book1.Book = booksEnglish3Class;
            inventoryEnglish3Book2.Book = booksEnglish3Class;           


            InventoryBook inventoryRussianLanguage1Book1 = new InventoryBook
            {
                InventoryNumber = "5",
                ISBN = "978-5-907172-47-8",
                Title = "Русский язык. 1 класс. Основы",
                Author = "Турчинов Н. В.",
                Publisher = "АСТ",
                YearPublished = "2021",
                Price = 25.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "222222 ТЕ"
            };                      

            // Создаем объект BookPhoto
            var bookPhoto2 = new BookPhoto { PhotoData = BookPhoto.LoadImage("Resources/26061.jpg"), DateAdded = DateTime.Now, Book = booksEnglish3Class };
            var bookPhoto3 = new BookPhoto { PhotoData = BookPhoto.LoadImage("Resources/3.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class };
            var bookPhoto4 = new BookPhoto { PhotoData = BookPhoto.LoadImage("Resources/4.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class };
            var bookPhoto5 = new BookPhoto { PhotoData = BookPhoto.LoadImage("Resources/5.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class };
            var bookPhoto6 = new BookPhoto { PhotoData = BookPhoto.LoadImage("Resources/7.png"), DateAdded = DateTime.Now, Book = booksEnglish3Class };

            context.BookPhotos.AddRange(new BookPhoto[] { bookPhoto2, bookPhoto3, bookPhoto4, bookPhoto5, bookPhoto6 });
            // Добавляем объекты в контекст один раз
            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryEnglish3Book1, inventoryEnglish3Book2 });
            context.Books.Add(booksEnglish3Class);

            Book booksRussianLanguage1Class = new Book
            {
                Class = 1,
                Description = "Учебник по русскому языку для начальных классов",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = ruLang2,
                //Category = russian,
                InventoryBooks = new List<InventoryBook> { inventoryRussianLanguage1Book1 }
            };
            // Устанавливаем связь между InventoryBook и Book
            inventoryRussianLanguage1Book1.Book = booksRussianLanguage1Class;
            context.InventoryBooks.Add(inventoryRussianLanguage1Book1);
            context.Books.Add(booksRussianLanguage1Class);


            InventoryBook inventoryBookHistoryClass5 = new InventoryBook
            {
                InventoryNumber = "6",
                ISBN = "978-5-907172-47-9",
                Title = "История Беларуси, 5 класс",
                Author = "Пушкарев И.И.",
                Publisher = "Асвета",
                YearPublished = "2000",
                Price = 100.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123123 ТЕ"
            };
            InventoryBook inventoryBookHistoryClass6 = new InventoryBook
            {
                InventoryNumber = "7",
                ISBN = "978-5-908172-47-9",
                Title = "История Беларуси, 6 класс",
                Author = "Пушкарев И.И.",
                Publisher = "Асвета",
                YearPublished = "2001",
                Price = 110.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123423 ТЕ"
            };
            InventoryBook inventoryBookHistory2Class6 = new InventoryBook
            {
                InventoryNumber = "8",
                ISBN = "978-5-908172-47-9",
                Title = "История Беларуси, 6 класс",
                Author = "Пушкарев И.И.",
                Publisher = "Асвета",
                YearPublished = "2001",
                Price = 110.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123423 ТЕ"
            };
            InventoryBook inventoryBookHistoryClass7 = new InventoryBook
            {
                InventoryNumber = "9",
                ISBN = "978-5-90072-48-9",
                Title = "История Беларуси, 7 класс",
                Author = "Токарев И.И.",
                Publisher = "Асвета",
                YearPublished = "2003",
                Price = 105.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "120023 ТЕ"
            };
            InventoryBook inventoryBookHistoryClass8 = new InventoryBook
            {
                InventoryNumber = "10",
                ISBN = "978-5-904442-47-9",
                Title = "История Беларуси, 8 класс",
                Author = "Григорьева Н.П.",
                Publisher = "Асвета",
                YearPublished = "2002",
                Price = 90.5,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "121123 ТЕ"
            };

            InventoryBook inventoryBookHistoryClass9 = new InventoryBook
            {
                InventoryNumber = "11",
                ISBN = "978-5-905542-47-9",
                Title = "История Беларуси, 9 класс",
                Author = "Потапова А.Г.",
                Publisher = "Асвета",
                YearPublished = "2008",
                Price = 95.5,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123303 ТЕ"
            };

            Book booksBelHistory1Class5 = new Book
            {
                Class = 5,
                Description = "Учебник по истории Беларуси 5 класс",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = hist2,
                //Category = history,
                InventoryBooks = new List<InventoryBook> { inventoryBookHistoryClass5 }
            };
            Book booksBelHistory1Class6 = new Book
            {
                Class = 6,
                Description = "Учебник по истории Беларуси 6 класс",
                Quantity = 2,
                QuantityLeft = 2,
                Genre = educationalLiteratureGenre,
                Subject = hist2,
                //Category = history,
                InventoryBooks = new List<InventoryBook> { inventoryBookHistoryClass6, inventoryBookHistory2Class6 }
            };

            Book booksBelHistory1Class7 = new Book
            {
                Class = 7,
                Description = "Учебник по истории Беларуси 7 класс",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = hist2,
                //Category = history,
                InventoryBooks = new List<InventoryBook> { inventoryBookHistoryClass7 }
            };
            Book booksBelHistory1Class8 = new Book
            {
                Class = 8,
                Description = "Учебник по истории Беларуси 8 класс",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = hist2,
                //Category = history,
                InventoryBooks = new List<InventoryBook> { inventoryBookHistoryClass8 }
            };
            Book booksBelHistory1Class9 = new Book
            {
                Class = 9,
                Description = "Учебник по истории Беларуси 9 класс",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = hist2,
                //Category = history,
                InventoryBooks = new List<InventoryBook> { inventoryBookHistoryClass9 }
            };

            inventoryBookHistoryClass5.Book = booksBelHistory1Class5;
            inventoryBookHistoryClass6.Book = booksBelHistory1Class6;
            inventoryBookHistory2Class6.Book = booksBelHistory1Class6;
            inventoryBookHistoryClass7.Book = booksBelHistory1Class7;
            inventoryBookHistoryClass8.Book = booksBelHistory1Class8;
            inventoryBookHistoryClass9.Book = booksBelHistory1Class9;

            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookHistoryClass5, inventoryBookHistoryClass6, inventoryBookHistory2Class6, inventoryBookHistoryClass7, inventoryBookHistoryClass8, inventoryBookHistoryClass9 });
            context.Books.AddRange(new Book[] { booksBelHistory1Class5, booksBelHistory1Class6, booksBelHistory1Class7, booksBelHistory1Class8, booksBelHistory1Class9 });

            ///////////////////////////////////////////////////////////
            ///

            InventoryBook inventoryBookChemicalClass7 = new InventoryBook
            {
                InventoryNumber = "12",
                ISBN = "988-5-907172-44-9",
                Title = "Общая химия, 7 класс",
                Author = "Деточкин А.И.",
                Publisher = "АСТ",
                YearPublished = "2010",
                Price = 40.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "122223 ТЕ"
            };


            InventoryBook inventoryBookChemicalClass8 = new InventoryBook
            {
                InventoryNumber = "13",
                ISBN = "988-5-907172-44-9",
                Title = "Неорганическая химия, 8 класс",
                Author = "Севостьянова А.И.",
                Publisher = "Литрес",
                YearPublished = "2005",
                Price = 34.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "124523 ТЕ"
            };

            InventoryBook inventoryBookChemicalClass81 = new InventoryBook
            {
                InventoryNumber = "14",
                ISBN = "988-5-907172-44-9",
                Title = "Неорганическая химия, 8 класс",
                Author = "Севостьянова А.И.",
                Publisher = "Литрес",
                YearPublished = "2005",
                Price = 37.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "128523 ТЕ"
            };

            InventoryBook inventoryBookChemicalClass9 = new InventoryBook
            {
                InventoryNumber = "15",
                ISBN = "988-5-904072-44-9",
                Title = "Неорганическая химия, 9 класс",
                Author = "Никулин С.П.",
                Publisher = "Питер",
                YearPublished = "2012",
                Price = 55.5,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "128523 ТЕ"
            };

            InventoryBook inventoryBookChemicalClass91 = new InventoryBook
            {
                InventoryNumber = "16",
                ISBN = "988-5-904072-44-9",
                Title = "Неорганическая химия, 9 класс",
                Author = "Никулин С.П.",
                Publisher = "Питер",
                YearPublished = "2012",
                Price = 65.7,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "128599 ТЕ"
            };


            Book books1Chemical7Class = new Book
            {
                Class = 7,
                Description = "Учебник общая химия, 7 класс",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = chemistry2,
                //Category = chemistry,
                InventoryBooks = new List<InventoryBook> { inventoryBookChemicalClass7 }
            };

            Book books1Chemical8Class = new Book
            {
                Class = 8,
                Description = "Учебник неорганическая химия для 8 класса",
                Quantity = 2,
                QuantityLeft = 2,
                Genre = educationalLiteratureGenre,
                Subject = chemistry2,
                // Category = chemistry,
                InventoryBooks = new List<InventoryBook> { inventoryBookChemicalClass8, inventoryBookChemicalClass81 }
            };

            Book books1Chemical9Class = new Book
            {
                Class = 9,
                Description = "Учебник неорганическая химия для 9 класса",
                Quantity = 2,
                QuantityLeft = 2,
                Genre = educationalLiteratureGenre,
                Subject = chemistry2,
                //Category = chemistry,
                InventoryBooks = new List<InventoryBook> { inventoryBookChemicalClass9, inventoryBookChemicalClass91 }
            };

            inventoryBookChemicalClass7.Book = books1Chemical7Class;
            inventoryBookChemicalClass8.Book = books1Chemical8Class;
            inventoryBookChemicalClass81.Book = books1Chemical8Class;
            inventoryBookChemicalClass9.Book = books1Chemical9Class;
            inventoryBookChemicalClass91.Book = books1Chemical9Class;

            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookChemicalClass7, inventoryBookChemicalClass8, inventoryBookChemicalClass81, inventoryBookChemicalClass9, inventoryBookChemicalClass91 });
            context.Books.AddRange(new Book[] { books1Chemical7Class, books1Chemical8Class, books1Chemical9Class });




            InventoryBook inventoryBookPhysicsClass7 = new InventoryBook
            {
                InventoryNumber = "17",
                ISBN = "123-7-904982-44-9",
                Title = "Основы физики, 7 класс",
                Author = "Потапов Т.Г.",
                Publisher = "Питер",
                YearPublished = "2017",
                Price = 67.5,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "128090 ТЕ"
            };
            InventoryBook inventoryBookPhysicsClass8 = new InventoryBook
            {
                InventoryNumber = "18",
                ISBN = "123-7-903382-44-9",
                Title = "Физика, 8 класс",
                Author = "Каменская Н.П.",
                Publisher = "Питер",
                YearPublished = "2007",
                Price = 24.5,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "100090 ТЕ"
            };
            InventoryBook inventoryBookPhysicsClass9 = new InventoryBook
            {
                InventoryNumber = "19",
                ISBN = "123-7-903112-44-9",
                Title = "Физика, 9 класс",
                Author = "Пугачева А.Б.",
                Publisher = "Питер",
                YearPublished = "2007",
                Price = 17.3,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "110080 ТЕ"
            };
            InventoryBook inventoryBookPhysicsClass92 = new InventoryBook
            {
                InventoryNumber = "20",
                ISBN = "123-7-903112-44-9",
                Title = "Физика, 9 класс",
                Author = "Пугачева А.Б.",
                Publisher = "Питер",
                YearPublished = "2007",
                Price = 20.1,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "110991 ТЕ"
            };
            InventoryBook inventoryBookPhysicsClass93 = new InventoryBook
            {
                InventoryNumber = "21",
                ISBN = "123-7-903112-44-9",
                Title = "Физика, 9 класс",
                Author = "Пугачева А.Б.",
                Publisher = "Питер",
                YearPublished = "2007",
                Price = 22.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123891 ТЕ"
            };


            Book books1Physical7Class = new Book
            {
                Class = 7,
                Description = "Учебник по физике 7 класса",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = physics2,               
                InventoryBooks = new List<InventoryBook> { inventoryBookPhysicsClass7 }
            };
            Book books1Physical8Class = new Book
            {
                Class = 8,
                Description = "Учебник по физике 8 класса",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = physics2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookPhysicsClass8 }
            };
            Book books1Physical9Class = new Book
            {
                Class = 9,
                Description = "Учебник по физике 9 класса",
                Quantity = 3,
                QuantityLeft = 3,
                Genre = educationalLiteratureGenre,
                Subject = physics2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookPhysicsClass9, inventoryBookPhysicsClass92, inventoryBookPhysicsClass93 }
            };

            inventoryBookPhysicsClass7.Book = books1Physical7Class;
            inventoryBookPhysicsClass8.Book = books1Physical8Class;
            inventoryBookPhysicsClass9.Book = books1Physical9Class;
            inventoryBookPhysicsClass92.Book = books1Physical9Class;
            inventoryBookPhysicsClass93.Book = books1Physical9Class;

            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookPhysicsClass7, inventoryBookPhysicsClass8, inventoryBookPhysicsClass9, inventoryBookPhysicsClass92, inventoryBookPhysicsClass93 });
            context.Books.AddRange(new Book[] { books1Physical7Class, books1Physical8Class, books1Physical9Class });

            InventoryBook inventoryBookGeografyClass6 = new InventoryBook
            {
                InventoryNumber = "22",
                ISBN = "123-7-905512-11-9",
                Title = "География, 6 класс",
                Author = "Антонов Н.Б.",
                Publisher = "Питер",
                YearPublished = "2009",
                Price = 28.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "123001 ТЕ"
            };
            InventoryBook inventoryBookGeografyClass7 = new InventoryBook
            {
                InventoryNumber = "23",
                ISBN = "173-7-907712-11-7",
                Title = "География, 7 класс",
                Author = "Коростоянов Н.Ф.",
                Publisher = "АСТ",
                YearPublished = "2018",
                Price = 56.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "221341 ТЕ"
            };
            InventoryBook inventoryBookGeografyClass71 = new InventoryBook
            {
                InventoryNumber = "24",
                ISBN = "173-7-907712-11-7",
                Title = "География, 7 класс",
                Author = "Коростоянов Н.Ф.",
                Publisher = "АСТ",
                YearPublished = "2018",
                Price = 58.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "221349 ТЕ"
            };
            InventoryBook inventoryBookGeografyClass72 = new InventoryBook
            {
                InventoryNumber = "25",
                ISBN = "173-7-907712-11-7",
                Title = "География, 7 класс",
                Author = "Коростоянов Н.Ф.",
                Publisher = "АСТ",
                YearPublished = "2018",
                Price = 60.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "221355 ТЕ"
            };
            InventoryBook inventoryBookGeografyClass8 = new InventoryBook
            {
                InventoryNumber = "26",
                ISBN = "173-7-9077553-11-9",
                Title = "География, 8 класс",
                Author = "Киркоров Ф.Б.",
                Publisher = "АСТ",
                YearPublished = "2023",
                Price = 85.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "299355 ТЕ"
            };
            InventoryBook inventoryBookGeografyClass9 = new InventoryBook
            {
                InventoryNumber = "27",
                ISBN = "173-7-9077553-51-0",
                Title = "География, 9 класс",
                Author = "Никоноров П.Б.",
                Publisher = "Питер",
                YearPublished = "2008",
                Price = 15.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "335525 ТЕ"
            };
            InventoryBook inventoryBookGeografyClass10 = new InventoryBook
            {
                InventoryNumber = "28",
                ISBN = "173-7-9077553-51-7",
                Title = "География, 10 класс",
                Author = "Герман П.Б.",
                Publisher = "Питербург",
                YearPublished = "1990",
                Price = 4.0,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "103125 ТЕ"
            };
            InventoryBook inventoryBookGeografyClass11 = new InventoryBook
            {
                InventoryNumber = "29",
                ISBN = "173-7-9078853-51-7",
                Title = "География, 11 класс",
                Author = "Богданов Н.Н.",
                Publisher = "Питербург",
                YearPublished = "1992",
                Price = 4.8,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "105525 ТЕ"
            };

            Book booksGeograpfy6Class = new Book
            {
                Class = 6,
                Description = "Учебник по географии 6 класса",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = geography2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookGeografyClass6 }
            };
            Book booksGeograpfy7Class = new Book
            {
                Class = 7,
                Description = "Учебник по географии 7 класса",
                Quantity = 3,
                QuantityLeft = 3,
                Genre = educationalLiteratureGenre,
                Subject = geography2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookGeografyClass7, inventoryBookGeografyClass71, inventoryBookGeografyClass72 }
            };
            Book booksGeograpfy8Class = new Book
            {
                Class = 8,
                Description = "Учебник по географии 8 класса",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = geography2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookGeografyClass8 }
            };
            Book booksGeograpfy9Class = new Book
            {
                Class = 9,
                Description = "Учебник по географии 9 класса",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = geography2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookGeografyClass9 }
            };
            Book booksGeograpfy10Class = new Book
            {
                Class = 10,
                Description = "Учебник по географии 10 класса",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = geography2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookGeografyClass10 }
            };
            Book booksGeograpfy11Class = new Book
            {
                Class = 11,
                Description = "Учебник по географии 11 класса",
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = geography2,                
                InventoryBooks = new List<InventoryBook> { inventoryBookGeografyClass11 }
            };
            inventoryBookGeografyClass6.Book = booksGeograpfy6Class;
            inventoryBookGeografyClass7.Book = booksGeograpfy7Class;
            inventoryBookGeografyClass71.Book = booksGeograpfy7Class;
            inventoryBookGeografyClass72.Book = booksGeograpfy7Class;
            inventoryBookGeografyClass8.Book = booksGeograpfy8Class;
            inventoryBookGeografyClass9.Book = booksGeograpfy9Class;
            inventoryBookGeografyClass10.Book = booksGeograpfy10Class;
            inventoryBookGeografyClass11.Book = booksGeograpfy11Class;

            context.InventoryBooks.AddRange(new InventoryBook[] { inventoryBookGeografyClass6, inventoryBookGeografyClass7, inventoryBookGeografyClass71, inventoryBookGeografyClass72, inventoryBookGeografyClass8, inventoryBookGeografyClass9, inventoryBookGeografyClass10, inventoryBookGeografyClass11 });
            context.Books.AddRange(new Book[] { booksGeograpfy6Class, booksGeograpfy7Class, booksGeograpfy8Class, booksGeograpfy9Class, booksGeograpfy10Class, booksGeograpfy11Class });


            InventoryBook inventoryTheLittleHouse = new InventoryBook
            {
                InventoryNumber = "30",
                ISBN = "9785811257843",
                Title = "The Little House",
                Author = "Наумова Н.А.",
                Publisher = "Айрис-пресс",
                YearPublished = "2020",
                Price = 15.5,
                DateOfReceipt = DateTime.Now,
                IncomingInvoice = "12345 ТЕ"
            };

           

            //InventoryBook inventoryTheLittleHouse2 = new InventoryBook
            //{
            //    InventoryNumber = "31",
            //    ISBN = "9785811257843",
            //    Title = "The Little House",
            //    Author = "Наумова Н.А.",
            //    Publisher = "Айрис-пресс",
            //    YearPublished = "2020",
            //    Price = 15.5,
            //    DateOfReceipt = DateTime.Now,
            //    IncomingInvoice = "12345 ТЕ"
            //};


            Book booksTheLittleHouse = new Book
            {
                Class = 1,
                Description = "Книга по английскому для детей",
                //  Quantity = 2,
                //  QuantityLeft = 2,
                Quantity = 1,
                QuantityLeft = 1,
                Genre = educationalLiteratureGenre,
                Subject = eng2,
                InventoryBooks = new List<InventoryBook> { inventoryTheLittleHouse
                //, inventoryTheLittleHouse2 
                }
            };
            context.InventoryBooks.Add(inventoryTheLittleHouse);
            context.Books.Add(booksTheLittleHouse);
         
            // Сохраняем изменения
            context.SaveChanges();
        }  
    }
}

