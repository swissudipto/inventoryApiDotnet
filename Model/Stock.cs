namespace inventoryApiDotnet.Model
{
    public class Stock
    {
        public string? Id { get; set; }
        public long? ProductId { get; set; }
        public string? ProductName { get; set; }
        public long? Quantity { get; set; }
        public double? AvarageBuyingPrice { get; set; }
    }
}