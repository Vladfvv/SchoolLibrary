using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityProject
{//1.Создайте класс, описывающий сущность предметной области. Это класс
  //  впоследствии будет отображаться на таблицу базы данных
    public class Notice
    {
        public int NoticeId { get; set; }
        public String Date { get; set; }
        public String AccountNumber { get; set; }
        public String Client { get; set; }
        public double Summa { get; set; }
    }
}
