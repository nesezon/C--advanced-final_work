using System.Windows;
using System.Windows.Controls;

namespace e_commerce.Views {
  public partial class ClientView : UserControl {
    private static MainWindow MW;
    public ClientView() {
      InitializeComponent();
      MW = Application.Current.MainWindow as MainWindow;

      // TODO: добавлено для проверки. после проверки удалить
      MW.Cart.Add(new CartItem() { product_id = 2, product_name = "Тетрадь", quantity = 11 });
      MW.Cart.Add(new CartItem() { product_id = 3, product_name = "Циркуль", quantity = 1 });

      DataContext = MW;
    }

    private void LogOut_Click(object sender, RoutedEventArgs e) {
      if (MW.Cart.Count > 0) {
        // если при выходе корзина не пуста
        MessageBoxButton btnMessageBox = MessageBoxButton.OKCancel;
        MessageBoxImage icnMessageBox = MessageBoxImage.Warning;
        MessageBoxResult result = MessageBox.Show(MW,"В корине остались продукты. Удалить?", "Корзина", btnMessageBox, icnMessageBox);
        if (result != MessageBoxResult.OK) return;
      }
      MW.Cart.Clear();
      MW.ActiveItem.Content = new LoginView();
    }

  }
}
