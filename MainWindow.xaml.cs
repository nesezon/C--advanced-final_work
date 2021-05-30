using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using e_commerce.DBModel;
using e_commerce.Views;

namespace e_commerce {
  public partial class MainWindow : Window {
    public StoreDB db;
    readonly Settings settings = new Settings();
    public ObservableCollection<CartItem> Cart { get; set; }
    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<RolesForCombo> Roles { get; set; }
    public User LoggedUser { get; set; }
    #region свойство зависимости "Orders" для хранения коллекции заказов
    // Регистрация
    public static readonly DependencyProperty OrdersProperty =
      DependencyProperty.Register("Orders", typeof(ObservableCollection<OrdersFiltered>), typeof(MainWindow));
    // Упаковка
    public ObservableCollection<OrdersFiltered> Orders {
      get {
        return (ObservableCollection<OrdersFiltered>)GetValue(OrdersProperty);
      }
      set {
        SetValue(OrdersProperty, value);
      }
    }
    #endregion
    #region свойство зависимости "TotalSum" для хранения общей стоимости заказа
    // Регистрация
    public static readonly DependencyProperty TotalSumProperty =
        DependencyProperty.Register("TotalSum", typeof(decimal), typeof(MainWindow));
    // Упаковка
    public decimal TotalSum {
      get {
        return (decimal)GetValue(TotalSumProperty);
      }
      set {
        SetValue(TotalSumProperty, value);
      }
    }
    #endregion
    #region свойство зависимости "Users" для таблицы пользователей
    // Регистрация
    public static readonly DependencyProperty UsersProperty =
      DependencyProperty.Register("Users", typeof(ObservableCollection<UsersForEdit>), typeof(MainWindow));
    // Упаковка
    public ObservableCollection<UsersForEdit> Users {
      get {
        return (ObservableCollection<UsersForEdit>)GetValue(UsersProperty);
      }
      set {
        SetValue(UsersProperty, value);
      }
    }
    #endregion
    #region свойство зависимости "ProductsAdm" для таблицы товаров
    // Регистрация
    public static readonly DependencyProperty ProductsAdmProperty =
      DependencyProperty.Register("ProductsAdm", typeof(ObservableCollection<ProductsForEdit>), typeof(MainWindow));
    // Упаковка
    public ObservableCollection<ProductsForEdit> ProductsAdm {
      get {
        return (ObservableCollection<ProductsForEdit>)GetValue(ProductsAdmProperty);
      }
      set {
        SetValue(ProductsAdmProperty, value);
      }
    }
    #endregion

    public MainWindow() {
      db = new StoreDB();
      Cart = new ObservableCollection<CartItem>();

      // первый запрос к БД долгий, поэтому
      // делаю его заранее в фоне
      // (простой, ничего не значащий запрос)
      try {
        var roles = db.Roles.ToListAsync();
      } catch (Exception ex) {
        Exception subEx = ex.InnerException;
        MessageBox.Show($"Ошибка в БД: {(subEx == null ? ex.Message : subEx.Message)}");
        Close();
      }

      InitializeComponent();
      
      settings.LoadFromRegistry(this);

      // Сразу открываю окно авторизации
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
      CalcSum();
    }

    /// <summary>
    /// подсчет общей стоимости содержимого корзины
    /// </summary>
    public void CalcSum() {
      decimal sum = 0;
      foreach (var item in Cart)
        sum += item.quantity * item.price;
      TotalSum = sum;
    }

    private void Window_Deactivated(object sender, EventArgs e) {
      // сбрасываем настройки в реестр
      settings.Save2Registry(this);
    }
  }

  /// <summary>
  /// Значение по умолчанию для ComboBox ролей в таблице пользователей в админке
  /// </summary>
  public class ListBoxRolesConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      object result = (short)value == 0 ? 1 : value;
      return result;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return value;
    }
  }
}
