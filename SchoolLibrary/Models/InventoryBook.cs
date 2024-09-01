using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SchoolLibrary.Models
{
    public class InventoryBook
    {
        public int InventoryBookID { get; set; }
        public string InventoryNumber { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public double Price { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public string IncomingInvoice { get; set; }
      //  public DateTime? DateOfDisposal { get; set; }
     //   public string OutgoingInvoice { get; set; }
     //   public string ReasonForDisposal { get; set; }
        public int? BookID { get; set; }
        public virtual Book Book { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }

        public InventoryBook()
        {
            Loans = new HashSet<Loan>();
        }

        public override string ToString()
        {
            return $"InventoryBookID: {InventoryBookID}, Inventory Number: {InventoryNumber}, ISBN: {ISBN}, Title: {Title}, Author: {Author}, Year Published: {YearPublished}, Price: {Price}, Date of Receipt: {DateOfReceipt}, Incoming Invoice: {IncomingInvoice}";
        }
    }
}
