using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using e_commerce.DBModel;
using e_commerce.Views;

namespace e_commerce {
  public partial class MainWindow : Window {
    public ObservableCollection<CartItem> Cart { get; set; }
    public string selectedName { get; set; }
    public User LoggedUser { get; set; }

    public MainWindow() {

      // первый запрос к БД долгий, поэтому
      // делаю его заранее в фоне
      // (простой, ничего не значащий запрос)
      using (var db = new StoreDB()) {
        var roles = db.Roles.ToListAsync();
      }

      Cart = new ObservableCollection<CartItem>();
      InitializeComponent();

      // пересоздание БД при каждом запуске
      Database.SetInitializer<StoreDB>(new StoreDB.StoreDbInitializer());

      ActiveItem.Content = new LoginView();
    }

    /// <summary>
    /// удаляю из корзины элемент с заданным id продукта
    /// </summary>
    public void RemoveFromCart(int product_id) {
      for (int i = Cart.Count - 1; i >= 0; i--) {
        if (Cart[i].product_id == product_id) Cart.RemoveAt(i);
      }
    }

    /// <summary>
    /// добавляю продукт в корзину если его там еще нет
    /// </summary>
    public void Add2Cart(CartItem product) {
      for (int i = 0; i < Cart.Count; i++) {
        if (Cart[i].product_id == product.product_id) return;
      }
      Cart.Add(product);
    }
  }
}
