
namespace inventoryApiDotnet.Model
{
  public class Sell
  {
    public string? Id { get; set; }
    public DateTime? SellDate { get; set; }
    public long? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? InvoiceNo { get; set; }
    public long? Quantity { get; set; }
    public double? sellAmount { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public DateTime? transactionDateTime {get;set;}
  }
}