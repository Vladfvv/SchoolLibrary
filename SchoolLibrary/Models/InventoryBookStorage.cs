using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Models
{
    public class InventoryBookStorage
    {
        public int InventoryBookStorageID { get; set; }
        public string InventoryNumber { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public double Price { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public string IncomingInvoice { get; set; }
        public DateTime? DateOfDisposal { get; set; }
        public string OutgoingInvoice { get; set; }
        public string ReasonForDisposal { get; set; }
        public int? BookID { get; set; }
        public virtual Book Book { get; set; }
    }
}
