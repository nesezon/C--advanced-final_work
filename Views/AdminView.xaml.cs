using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using e_commerce.DBModel;

namespace e_commerce.Views {
  public partial class AdminView : UserControl {
    private static MainWindow MW;
    List<int> deletions = new List<int>();

    private void Preps() {
      MW = Application.Current.MainWindow as MainWindow;
      MW.Roles = new ObservableCollection<RolesForCombo>(
        from r in MW.db.Roles
        select new RolesForCombo() {
          role_id = r.role_id,
          name = r.name
        }
        );
      MW.Users = new ObservableCollection<UsersFiltered>(
        from u in MW.db.Users
        where u.deleted == false
        select new UsersFiltered() {
          user_id = u.user_id,
          full_name = u.full_name,
          login = u.login,
          password = u.password,
          role_id = u.role_id
        }
        );
      DataContext = MW;
      deletions.Clear();
    }

    public AdminView() {
      Preps();
      InitializeComponent();
    }

    private void LogOut_Click(object sender, RoutedEventArgs e) {
      MW.ActiveItem.Content = new LoginView();
    }

    private void Load_Click(object sender, RoutedEventArgs e) {
      Preps();
    }

    private void Save_Click(object sender, RoutedEventArgs e) {
      foreach (UsersFiltered u in MW.Users) {
        var user = MW.db.Users.Find(u.user_id);
        if (user == null) {
          // новая запись
          _ = MW.db.Users.Add(
            new User {
              full_name = u.full_name,
              login = u.login,
              password = u.password,
              role_id = u.role_id,
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
      foreach (var user_id in deletions) {
        var user = MW.db.Users.Find(user_id);
        if (user != null) user.deleted = true;
      }
      MW.db.SaveChanges();
      Preps();
    }

    private void DataGrid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e) {
      DataGrid grid = (DataGrid)sender;
      if (e.Command == DataGrid.DeleteCommand) {
        deletions.AddRange(grid.SelectedItems.OfType<UsersFiltered>().Select(i => i.user_id));
      }
    }
  }
}
