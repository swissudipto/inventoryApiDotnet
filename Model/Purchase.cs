using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventoryApiDotnet.Model
{
  public class Purchase
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? Id { get; set; }
    [Key]
    public long? PurchaseId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? SupplierName { get; set; }
    public string? SupplierContactNumber { get; set; }
    public string? SupplierAddress { get; set; }
    public string? Comment { get; set; }
    public DateTime? transactionDateTime { get; set; }
    public ICollection<PurchaseItem>? purchaseItems { get; set; }
    public double? TotalAmount { get; set; }
  }

  public class PurchaseItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? Id { get; set; }
    public long? PurchaseId { get; set; }
    public long Sl { get; set; }
    public string? ProductName { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public long Amount { get; set; }
    [ForeignKey("PurchaseId")]
    public Purchase? Purchase { get; set; } = null!;
  }
}