using SchoolLibrary.Models;
using System;
using System.Collections.Generic;
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
            context.Students.AddRange(new Student[] { new Student { FirstName="Сергей", LastName="Петров", Age=15, Class = "10A"},
            new Student { FirstName="Иван", LastName="Иванов", Age=16, Class = "10A"},
            new Student { FirstName="Мария", LastName="Петрова", Age=15, Class = "9B"},
            new Student { FirstName="Алексей", LastName="Смирнов", Age=14, Class = "8C"},
            new Student { FirstName="Екатерина", LastName="Козлова", Age=13, Class = "7A"},
            new Student { FirstName="Артем", LastName="Федоров", Age=12, Class = "6B"},
            new Student { FirstName="Анастасия", LastName="Михайлова", Age=11, Class = "5C"},
            new Student { FirstName="Дмитрий", LastName="Николаев", Age=10, Class = "4A"},
            new Student { FirstName="София", LastName="Иванова", Age=9, Class = "3B"},
            new Student { FirstName="Максим", LastName="Смирнов", Age=8, Class = "2C"},
            new Student { FirstName="Алиса", LastName="Петрова", Age=7, Class = "1A"},
            new Student { FirstName="Никита", LastName="Кузнецов", Age=16, Class = "10B"},
            new Student { FirstName="Анна", LastName="Сидорова", Age=15, Class = "9A"},
            new Student { FirstName="Артемий", LastName="Федоров", Age=14, Class = "8B"},
            new Student { FirstName="Полина", LastName="Иванова", Age=13, Class = "7C"},
            new Student { FirstName="Егор", LastName="Петров", Age=12, Class = "6A"},
            new Student { FirstName="Ксения", LastName="Козлова", Age=11, Class = "5B"},
            new Student { FirstName="Александра", LastName="Смирнова", Age=10, Class = "4C"},
            new Student { FirstName="Михаил", LastName="Николаев", Age=9, Class = "3A"},
            new Student { FirstName="Елизавета", LastName="Федорова", Age=8, Class = "2B"},
            new Student { FirstName="Илья", LastName="Иванов", Age=7, Class = "1C"},
            new Student { FirstName="Павел", LastName="Кузнецов", Age=16, Class = "10A"},
            new Student { FirstName="Валерия", LastName="Сидорова", Age=15, Class = "9B"},
            new Student { FirstName="Владимир", LastName="Федоров", Age=14, Class = "8C"},
            new Student { FirstName="Анастасия", LastName="Иванова", Age=13, Class = "7A"},
            new Student { FirstName="Сергей", LastName="Петров", Age=12, Class = "6B"},
            new Student { FirstName="Дарья", LastName="Козлова", Age=11, Class = "5C"},
            new Student { FirstName="Алексей", LastName="Смирнов", Age=10, Class = "4A"},
            new Student { FirstName="Елена", LastName="Николаева", Age=9, Class = "3B"},
            new Student { FirstName="Артем", LastName="Федоров", Age=8, Class = "2C"},
            new Student { FirstName="Надежда", LastName="Иванова", Age=7, Class = "1A"},
            new Student { FirstName="Иван", LastName="Кузнецов", Age=16, Class = "10B"},
            new Student { FirstName="Мария", LastName="Сидорова", Age=15, Class = "9A"},
            new Student { FirstName="Александер", LastName="Федоров", Age=14, Class = "8B"},
            new Student { FirstName="Ольга", LastName="Иванова", Age=13, Class = "7C"},
            new Student { FirstName="Кирилл", LastName="Петров", Age=12, Class = "6A"},
            new Student { FirstName="Татьяна", LastName="Козлова", Age=11, Class = "5B"},
            new Student { FirstName="Евгений", LastName="Смирнов", Age=10, Class = "4C"},
            new Student { FirstName="Анастасия", LastName="Николаева", Age=9, Class = "3A"},
            new Student { FirstName="Игорь", LastName="Федоров", Age=8, Class = "2B"},
            new Student { FirstName="Екатерина", LastName="Иванова", Age=7, Class = "1C"},
            new Student { FirstName="Александр", LastName="Кузнецов", Age=16, Class = "10A"},
            new Student { FirstName="Виктория", LastName="Сидорова", Age=15, Class = "9B"},
            new Student { FirstName="Василий", LastName="Федоров", Age=14, Class = "8C"},
            });

            context.Categories.AddRange(new Category[] { new Category { CategoryName = "Английский язык"},
            new Category { CategoryName = "Белорусский язык"},
            new Category { CategoryName = "Русский язык"},
            new Category { CategoryName = "Математика"},
            new Category { CategoryName = "География"},
            new Category { CategoryName = "Физика"},
            new Category { CategoryName = "Химия"}
            });



            context.Books.AddRange(new Book[] { new Book { Title = "Математика 5 класс", Author = "Щурок В.Ф.", Publisher = "Аверсев", YearPublished = "2000", ISBN = "973-8-25-352", CategoryID = 4, Quantity = 10 },
            new Book { Title = "Математика 1 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2001", ISBN = "978-1-23-456", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 1 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2001", ISBN = "978-2-34-567", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 1 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2001", ISBN = "978-3-45-678", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 1 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2001", ISBN = "978-4-56-789", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 1 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2001", ISBN = "978-5-67-890", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 2 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2002", ISBN = "978-1-23-457", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 2 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2002", ISBN = "978-2-34-568", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 2 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2002", ISBN = "978-3-45-679", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 2 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2002", ISBN = "978-4-56-780", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 2 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2002", ISBN = "978-5-67-891", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 3 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2003", ISBN = "978-1-23-458", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 3 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2003", ISBN = "978-2-34-569", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 3 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2003", ISBN = "978-3-45-670", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 3 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2003", ISBN = "978-4-56-781", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 3 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2003", ISBN = "978-5-67-892", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 4 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2004", ISBN = "978-1-23-459", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 4 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2004", ISBN = "978-2-34-560", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 4 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2004", ISBN = "978-3-45-671", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 4 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2004", ISBN = "978-4-56-782", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 4 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2004", ISBN = "978-5-67-893", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 5 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2005", ISBN = "978-1-23-460", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 5 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2005", ISBN = "978-2-34-561", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 5 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2005", ISBN = "978-3-45-672", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 5 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2005", ISBN = "978-4-56-783", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 5 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2005", ISBN = "978-5-67-894", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 6 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2006", ISBN = "978-1-23-461", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 6 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2006", ISBN = "978-2-34-562", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 6 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2006", ISBN = "978-3-45-673", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 6 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2006", ISBN = "978-4-56-784", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 6 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2006", ISBN = "978-5-67-895", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 7 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2007", ISBN = "978-1-23-462", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 7 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2007", ISBN = "978-2-34-563", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 7 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2007", ISBN = "978-3-45-674", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 7 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2007", ISBN = "978-4-56-785", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 7 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2007", ISBN = "978-5-67-896", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 8 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2008", ISBN = "978-1-23-463", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 8 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2008", ISBN = "978-2-34-564", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 8 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2008", ISBN = "978-3-45-675", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 8 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2008", ISBN = "978-4-56-786", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 8 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2008", ISBN = "978-5-67-897", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 9 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2009", ISBN = "978-1-23-464", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 9 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2009", ISBN = "978-2-34-565", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 9 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2009", ISBN = "978-3-45-676", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 9 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2009", ISBN = "978-4-56-787", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 9 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2009", ISBN = "978-5-67-898", CategoryID = 5, Quantity = 10 },

            new Book { Title = "Математика 10 класс", Author = "Иванов И.И.", Publisher = "Аверсев", YearPublished = "2010", ISBN = "978-1-23-465", CategoryID = 1, Quantity = 10 },
            new Book { Title = "Русский язык 10 класс", Author = "Петров П.П.", Publisher = "Просвещение", YearPublished = "2010", ISBN = "978-2-34-566", CategoryID = 2, Quantity = 10 },
            new Book { Title = "Литература 10 класс", Author = "Сидоров С.С.", Publisher = "Наука", YearPublished = "2010", ISBN = "978-3-45-677", CategoryID = 3, Quantity = 10 },
            new Book { Title = "История 10 класс", Author = "Щурок В.Ф.", Publisher = "Феникс", YearPublished = "2010", ISBN = "978-4-56-788", CategoryID = 4, Quantity = 10 },
            new Book { Title = "География 10 класс", Author = "Васильев В.В.", Publisher = "Экзамен", YearPublished = "2010", ISBN = "978-5-67-899", CategoryID = 5, Quantity = 10 }});



    }
}
}
