using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using e_commerce.DBModel;
using System.Data.Entity.Validation;

namespace e_commerce.Views {
  public partial class AdminView : UserControl {
    private static MainWindow MW;
    List<int> DelUsers = new List<int>();
    List<int> DelProducts = new List<int>();

    private void PrepsUsers() {
      MW.Roles = new ObservableCollection<RolesForCombo>(
        from r in MW.db.Roles
        select new RolesForCombo() {
          role_id = r.role_id,
          name = r.name
        }
        );
      MW.Users = new ObservableCollection<UsersForEdit>(
        from u in MW.db.Users
        where u.deleted == false
        select new UsersForEdit() {
          user_id = u.user_id,
          full_name = u.full_name,
          login = u.login,
          password = u.password,
          role_id = u.role_id
        }
        );
      DelUsers.Clear();
    }

    private void PrepsProducts() {
      MW.ProductsAdm = new ObservableCollection<ProductsForEdit>(
        from p in MW.db.Products
        where p.deleted == false
        select new ProductsForEdit() {
          product_id = p.product_id,
          name = p.name,
          price = p.price
        }
        );
      DelProducts.Clear();
    }

    public AdminView() {
      MW = Application.Current.MainWindow as MainWindow;
      DataContext = MW;
      PrepsUsers();
      PrepsProducts();
      InitializeComponent();
    }

    private void LogOut_Click(object sender, RoutedEventArgs e) {
      MW.ActiveItem.Content = new LoginView();
    }

    private void LoadUsers_Click(object sender, RoutedEventArgs e) {
      PrepsUsers();
    }

    private void SaveUsers_Click(object sender, RoutedEventArgs e) {
      foreach (UsersForEdit u in MW.Users) {
        var user = MW.db.Users.Find(u.user_id);
        if (user == null) {
          // новая запись
          _ = MW.db.Users.Add(
            new User {
              full_name = u.full_name,
              login = u.login,
              password = u.password,
              role_id = (short)(u.role_id == 0 ? 1 : u.role_id),
              deleted = false
            }
          );
        } else {
          //  измененная запись
          user.full_name = u.full_name;
          user.login = u.login;
          user.password = u.password;
          user.role_id = u.role_id;
        }
      }
      // удаленные записи
      foreach (var user_id in DelUsers) {
        var user = MW.db.Users.Find(user_id);
        if (user != null) user.deleted = true;
      }

      try {
        MW.db.SaveChanges();
        PrepsUsers();
      }
      catch (DbEntityValidationException) {
        MessageBox.Show("Запись не возможна. Проверьте введенные данные.");
      }
    }

    private void UsersGrid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e) {
      DataGrid grid = (DataGrid)sender;
      if (e.Command == DataGrid.DeleteCommand) {
        DelUsers.AddRange(grid.SelectedItems.OfType<UsersForEdit>().Select(i => i.user_id));
      }
    }

    private void LoadProducts_Click(object sender, RoutedEventArgs e) {
      PrepsProducts();
    }

    private void SaveProducts_Click(object sender, RoutedEventArgs e) {
      // TODO: доделать
      foreach (ProductsForEdit p in MW.ProductsAdm) {
        var prod = MW.db.Products.Find(p.product_id);
        if (prod == null) {
          // новая запись
          _ = MW.db.Products.Add(
            new Product {
              name = p.name,
              price = p.price,
              deleted = false
            }
          );
        } else {
          //  измененная запись
          prod.name = p.name;
          prod.price = p.price;
        }
      }
      // удаленные записи
      foreach (var product_id in DelProducts) {
        var prod = MW.db.Products.Find(product_id);
        if (prod != null) prod.deleted = true;
      }

      try {
        MW.db.SaveChanges();
        PrepsProducts();
      }
      catch (DbEntityValidationException) {
        MessageBox.Show("Запись не возможна. Проверьте введенные данные.");
      }
    }

      private void ProductsGrid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e) {
      DataGrid grid = (DataGrid)sender;
      if (e.Command == DataGrid.DeleteCommand) {
        DelProducts.AddRange(grid.SelectedItems.OfType<ProductsForEdit>().Select(i => i.product_id));
      }
    }

  }
}
