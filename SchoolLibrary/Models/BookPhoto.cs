using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    public class BookPhoto
    {
        public int BookPhotoID { get; set; }
        public int BookID { get; set; }
        public byte[] PhotoData { get; set; } // Данные изображения в виде byte[]
        public DateTime DateAdded { get; set; }

        // Связь с Book
        public virtual Book Book { get; set; }

        public override string ToString()
        {
            return $"BookPhotoID: {BookPhotoID}, BookID: {BookID}, DateAdded: {DateAdded}";
        }

        public static byte[] LoadImage(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }
    }
}
