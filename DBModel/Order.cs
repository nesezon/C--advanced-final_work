using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.DBModel {
  public class Order {
    public Order() {
      Order_items = new HashSet<Order_item>();
    }

    [Key]
    public int order_id { get; set; }

    public int user_id { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime order_time { get; set; } = DateTime.Now;

    public virtual ICollection<Order_item> Order_items { get; set; }

    public virtual User Users { get; set; }
  }
}
