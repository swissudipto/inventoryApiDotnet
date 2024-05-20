using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace inventoryApiDotnet.Model
{
    public class Product
    {
       [BsonRepresentation(BsonType.ObjectId)] 
       public string? Id { get; set;}
       public long? ProductId {get;set;}
       public string? ProductName {get;set;} 
    }
}