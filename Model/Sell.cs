
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventoryApiDotnet.Model
{
  public class Sell
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? Id { get; set; }
    public DateTime? SellDate { get; set; }
    [Key]
    public string? InvoiceNo { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Comment { get; set; }
    public DateTime? transactionDateTime { get; set; }
    public ICollection<SellItem>? SellItems { get; set; }
    public double? TotalAmount { get; set; }
  }

  public class SellItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? Id { get; set; }
    public string? InvoiceNo { get; set; }
    public long Sl { get; set; }
    public string? ProductName { get; set; }
    public string? Serial {get;set;}
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public decimal Amount { get; set; }
    public long Gstpercentage { get; set; }
    public decimal Gstamount { get; set; }
    public decimal Taxableamount { get; set; }
    [ForeignKey("InvoiceNo")]
    public Sell? Sell { get; set; } = null!;
  }
}