using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using e_commerce.DBModel;
using e_commerce.Views;

namespace e_commerce {
  public partial class MainWindow : Window {
    public StoreDB db;
    public ObservableCollection<CartItem> Cart { get; set; }
    public ObservableCollection<Product> Products { get; set; }
    public User LoggedUser { get; set; }

    public MainWindow() {
      db = new StoreDB();

      // первый запрос к БД долгий, поэтому
      // делаю его заранее в фоне
      // (простой, ничего не значащий запрос)
      try {
        var roles = db.Roles.ToListAsync();
      } catch (Exception ex) {
        Exception subEx = ex.InnerException;
        MessageBox.Show($"Ошибка в БД: {(subEx == null ? ex.Message : subEx.Message)}");
        return;
      }

      Cart = new ObservableCollection<CartItem>();

      InitializeComponent();

      // Сразу открываем окно авторизации
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
