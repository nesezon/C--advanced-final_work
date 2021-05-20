using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.DBModel {
  public class Product {
    public Product() {
      Order_items = new HashSet<Order_item>();
    }

    [Key]
    public int product_id { get; set; }

    [Required]
    [StringLength(80)]
    public string name { get; set; }

    [Column(TypeName = "money")]
    public decimal price { get; set; }

    public bool deleted { get; set; } = false;

    public virtual ICollection<Order_item> Order_items { get; set; }
  }
}
