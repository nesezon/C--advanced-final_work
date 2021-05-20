using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using e_commerce.DBModel;

namespace e_commerce.Views {
  public partial class LoginView : UserControl {

    private enum role : short { customer = 1, manager = 2 }

    private static MainWindow MW;

    public LoginView() {
      InitializeComponent();
      MW = Application.Current.MainWindow as MainWindow;
    }

    private void Enter_Click(object sender, RoutedEventArgs e) {
      try {
        IQueryable<User> query = null;
        using (var db = new StoreDB()) {
          query = from u in db.Users
                  where u.login == Login.Text && u.password == Password.Password && u.deleted == false
                  select u;
          var foundUser = query.SingleOrDefault();
          if (foundUser != null) {
            MW.authUser = foundUser;
            if (foundUser.role_id == (short)(role.customer)) {
              MW.ActiveItem.Content = new ClientView();
            } else if (foundUser.role_id == (short)(role.manager)) {
              MW.ActiveItem.Content = new AdminView();
            }
          }
        }
      } catch (Exception ex) {
        Message.Text = ex.Message;
      }
    }
  }
}
