using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cart
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }  // ✅ Keep as int (not string)

    [ForeignKey("UserId")]
    public virtual User User { get; set; } // ✅ Ensure this exists

    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }

    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}

