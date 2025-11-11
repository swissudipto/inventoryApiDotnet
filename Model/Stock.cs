using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventoryApiDotnet.Model
{
    public class Stock
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        public long? ProductId { get; set; }
        public string? ProductName { get; set; }
        public long? Quantity { get; set; }
        public double? AvarageBuyingPrice { get; set; }
    }
}