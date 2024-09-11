using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchoolLibrary.Models
{
    public class Subject : INotifyPropertyChanged
    {
        private int _subjectID;
        private string _subjectName;
        private int _genreID;
        private Genre _genre;        
        public int SubjectID
        {
            get { return _subjectID; }
            set
            {
                _subjectID = value;
                OnPropertyChanged();
            }
        }

        public string SubjectName
        {
            get { return _subjectName; }
            set
            {
                _subjectName = value;
                OnPropertyChanged();
            }
        }

        public int GenreID
        {
            get { return _genreID; }
            set
            {
                _genreID = value;
                OnPropertyChanged();
            }
        }

        public Genre Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual ICollection<Book> Books { get; set; }  // Связь с Book

        public Subject()
        {
            Books = new HashSet<Book>();
        }
    }

    public class Genre
    {
        public int GenreID { get; set; }
        public string GenreName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }

        public Genre()
        {
            Books = new HashSet<Book>();
            Subjects = new HashSet<Subject>();
        }

        public Genre(string genreName) : this()
        {
            GenreName = genreName;
        }

        public bool IsEducationalLiterature()
        {
            return GenreName == "Учебная литература";
        }

        public Subject GetDefaultSubject()
        {
            // Возвращает предмет по умолчанию
            return new Subject { SubjectID = 0, SubjectName = "Без предмета", Genre = this };
        }
    }


}
