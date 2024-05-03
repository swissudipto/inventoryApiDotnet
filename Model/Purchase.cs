using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventoryApiDotnet.Model
{
    public class Purchase
    {
       public long PurchaseId {get;set;}
       public DateTime PurchaseDate {get;set;} 
       public long ProductId {get;set;}
       public int Quantity {get;set;}
       public string? SupplierName {get;set;}
       public long InvoiceAmount {get;set;}
       public long InvoiceNo {get;set;}
    }
}