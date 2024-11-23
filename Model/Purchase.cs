namespace inventoryApiDotnet.Model
{
  public class Purchase
  {
    public string? Id { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public long? ProductId { get; set; }
    public string? ProductName { get; set; }
    public long? Quantity { get; set; }
    public string? SupplierName { get; set; }
    public double? InvoiceAmount { get; set; }
    public string? InvoiceNo { get; set; }
    public string? Comment { get; set; }
    public DateTime? transactionDateTime {get;set;}
  }
}