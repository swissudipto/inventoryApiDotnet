using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace inventoryApiDotnet.Model
{
    public class Stock
    {
       public string? Id { get; set;}
       public long? ProductId {get;set;}
       public string? ProductName {get;set;}
       public long? Quantity {get;set;}
    }
}