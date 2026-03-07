using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventoryApiDotnet.Model
{
  public class SerialNoDto
  {
    public long ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? serial { get; set; }
    public decimal SuggestedSellingPrice { get; set; }
  }
}