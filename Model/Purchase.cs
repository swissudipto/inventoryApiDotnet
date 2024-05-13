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
       [BsonRepresentation(BsonType.ObjectId)] 
       public string Id { get; set;}
       public long PurchaseId {get;set;}
       public DateTime PurchaseDate {get;set;} 
       public long ProductId {get;set;}
       public int Quantity {get;set;}
       public string? SupplierName {get;set;}
       public long InvoiceAmount {get;set;}
       public long InvoiceNo {get;set;}
    }
}