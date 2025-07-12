namespace inventoryApiDotnet.Model
{
  public class Purchase
  {
    public string? Id { get; set; }
    public long? PurchaseId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public long? Quantity { get; set; }
    public string? SupplierName { get; set; }
    public double? InvoiceAmount { get; set; }
    public string? SupplierContactNumber { get; set; }
    public string? SupplierAddress { get; set; }
    public string? Comment { get; set; }
    public DateTime? transactionDateTime { get; set; }
    public List<PurchaseItem>? purchaseItems { get; set; }
    public double? TotalAmount { get; set; }
  }

  public class PurchaseItem
  {
    public long Sl { get; set; }
    public string? ProductName { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public long Amount { get; set; }
  }
}