using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

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

    // команда отправки продукта в корзину
    private RelayCommand toCartCommand;
    public RelayCommand ToCartCommand {
      get {
        return toCartCommand ??
               (toCartCommand = new RelayCommand(obj => {
                 MainWindow MW = Application.Current.MainWindow as MainWindow;
                 MW.Add2Cart(new CartItem() {
                   product_id = product_id, product_name = name, quantity = 1, price = price
                 });
               }));
      }
    }
  }

  public class ProductsForEdit {
    public int product_id { get; set; }
    public string name { get; set; }
    public decimal price { get; set; }
  }
}
