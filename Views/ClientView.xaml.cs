using System;
using System.Collections.ObjectModel;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using e_commerce.DBModel;

namespace e_commerce.Views {
  public partial class ClientView : UserControl {
    private static MainWindow MW;
    public ClientView() {
      InitializeComponent();
      MW = Application.Current.MainWindow as MainWindow;
      DataContext = MW;

      MW.Products = new ObservableCollection<Product>(MW.db.Products);

      // Пример заполнения корзины
      // MW.Add2Cart(new CartItem() { product_id = 2, product_name = "Тетрадь", quantity = 11, price = 2});
      // MW.Add2Cart(new CartItem() { product_id = 3, product_name = "Циркуль", quantity = 1, price = 7});
    }

    private void LogOut_Click(object sender, RoutedEventArgs e) {
      if (MW.Cart.Count > 0) {
        // если при выходе корзина не пуста
        MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;
        MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
        MessageBoxResult result = MessageBox.Show(MW, "В корине остались продукты. Удалить?", "Корзина", btnMessageBox, icnMessageBox);
        if (result != MessageBoxResult.OK) return;
      }
      MW.Cart.Clear();
      MW.ActiveItem.Content = new LoginView();
    }

    private void Buy_Click(object sender, RoutedEventArgs e) {
      // сохраняю заказ в рамках одной транзакции
      try {
        using (var transaction = new TransactionScope()) {

          var newOrder = new Order {
            user_id = MW.LoggedUser.user_id,
            order_time = DateTime.Now
          };
          MW.db.Orders.Add(newOrder);
          MW.db.SaveChanges();

          // получаю id добавленного заказа
          int id = newOrder.order_id;

          // сохраняю содержимое корзины для добавленного заказа
          foreach (var cartItem in MW.Cart)
            MW.db.Order_items.Add(new Order_item {
              order_id = id,
              product_id = cartItem.product_id,
              quantity = cartItem.quantity
            });
          MW.db.SaveChanges();

          transaction.Complete();
        }
        MessageBox.Show("Успешно");
        // чищу корзину для новых покупок
        MW.Cart.Clear();
      } catch (Exception ex) {
        Exception subEx = ex.InnerException;
        MessageBox.Show($"Ошибка: {(subEx == null ? ex.Message : subEx.Message)}");
      }

      

    }
  }
}
