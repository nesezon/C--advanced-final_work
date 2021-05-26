using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using e_commerce.DBModel;
using System.Text;

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
      if (MW.Cart.Count == 0) return;
      int order_id = -1;
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
          order_id = newOrder.order_id;

          // сохраняю содержимое корзины для добавленного заказа
          foreach (var cartItem in MW.Cart)
            MW.db.Order_items.Add(new Order_item {
              order_id = order_id,
              product_id = cartItem.product_id,
              quantity = cartItem.quantity
            });
          MW.db.SaveChanges();

          transaction.Complete();
        }

        // формирование чека продажи
        if (order_id >= 0) Invoice(order_id);

        // чищу корзину для новых покупок
        MW.Cart.Clear();
        MW.TotalSum = 0;
      } catch (Exception ex) {
        Exception subEx = ex.InnerException;
        MessageBox.Show($"Ошибка: {(subEx == null ? ex.Message : subEx.Message)}");
      }
    }

    // путь ко временной папке
    string invoiceFile = Path.Combine(Path.GetTempPath(), "invoice.txt");

    /// <summary>
    /// по id заказа выписываю чек продажи
    /// все беру из БД чтобы пользователь точно видел то же что и в БД
    /// </summary>
    private void Invoice(int order_id) {
      var qOrder = (from o in MW.db.Orders
                    where o.order_id == order_id
                    select o).SingleOrDefault();

      var qUser = (from u in MW.db.Users
                   where u.user_id == qOrder.user_id && u.deleted == false
                   select u).SingleOrDefault();

      var qProducts = from oi in
        (from o in MW.db.Order_items
         where o.order_id == order_id
         select o)
              join p in MW.db.Products
          on oi.product_id equals p.product_id
              select new { oi.quantity, p.name, p.price };

      StringBuilder message = new StringBuilder();
      message.Append($"ЧЕК ПРОДАЖИ № {qOrder?.order_id}\r\n");
      message.Append($"за {qOrder?.order_time}\r\n");
      message.Append($"Покупатель: {qUser?.full_name}\r\n");
      message.Append(new string('-', 25));
      message.Append(Environment.NewLine);

      decimal sum = 0;
      foreach (var prod in qProducts) {
        message.Append($"{prod.name,-15}{prod.price.ToString("F"),-8}{prod.quantity}\r\n");
        sum += prod.price * prod.quantity;
      }
      message.Append(new string('-', 10));
      message.Append(Environment.NewLine);
      message.Append($"Итого: {sum.ToString("F")}");

      // асинхронная запись в файл
      var bytes = Encoding.UTF8.GetBytes(message.ToString());
      Stream stream = new FileStream(invoiceFile, FileMode.Create, FileAccess.Write);
      stream.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(ShowInoice), stream);
    }

    private void ShowInoice(IAsyncResult ar) {
      // Как только файл с чеком продажи записан, закрываю поток
      Stream stream = ar.AsyncState as Stream;
      if (stream != null) stream.Close();

      // запуск блокнота с чеком продажи
      // дочерний поток ждет завершения работы с файлом
      var process = Process.Start(invoiceFile);
      process.WaitForExit();

      // чищу за собой файл
      File.Delete(invoiceFile);
    }
  }
}
