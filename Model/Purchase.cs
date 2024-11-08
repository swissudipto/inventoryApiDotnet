using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace inventoryApiDotnet.Model
{
  public class Purchase
  {
    public string? Id { get; set; }
    public string? PurchaseId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public long? ProductId { get; set; }
    public string? ProductName { get; set; }
    public long? Quantity { get; set; }
    public string? SupplierName { get; set; }
    public long? InvoiceAmount { get; set; }
    public string? InvoiceNo { get; set; }
    public string? Comment { get; set; }
    public DateTime? transactionDateTime {get;set;}
  }
}