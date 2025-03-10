using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public string Name { get; set; }

    [Column(TypeName = "decimal(18,2)")] // Specify precision
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; } 

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    [JsonIgnore]
    public Category? Category { get; set; }
}
