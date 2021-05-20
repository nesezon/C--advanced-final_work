using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.DBModel {
  [Table("Order_items")]
  public class Order_item {
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int order_id { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int product_id { get; set; }

    public int quantity { get; set; } = 0;

    public virtual Order Orders { get; set; }

    public virtual Product Products { get; set; }
  }
}
