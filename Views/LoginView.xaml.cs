using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace e_commerce.Views {
  public partial class LoginView : UserControl {

    private enum role : short { customer = 1, manager = 2 }
    private static MainWindow MW;

    public LoginView() {
      InitializeComponent();
      MW = Application.Current.MainWindow as MainWindow;
      
      // добавляю обработчик нажатия клавиш
      PreviewKeyDown += HandleKeypressing;
    }

    /// <summary>
    /// обработчик нажатия клавиатурных клавиш
    /// </summary>
    private void HandleKeypressing(object sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) Enter_Click(null, null);
    }

    private void Enter_Click(object sender, RoutedEventArgs e) {
      try {
        Message.Text = "... Обработка ...";
        MW.LoggedUser = null;
        var query = from u in MW.db.Users
                    where u.login == Login.Text && u.password == Password.Password && u.deleted == false
                    select u;
        var foundUser = query.SingleOrDefault();
        if (foundUser != null) {
          MW.LoggedUser = foundUser;
          if (foundUser.role_id == (short)(role.customer)) {
            MW.ActiveItem.Content = new ClientView();
          } else if (foundUser.role_id == (short)(role.manager)) {
            MW.ActiveItem.Content = new AdminView();
          } else {
            Message.Text = "Нет такого пользователя";
          }
        } else {
          Message.Text = "Нет такого пользователя";
        }
      } catch (Exception ex) {
        Exception subEx = ex.InnerException;
        Message.Text = (subEx == null) ? ex.Message : subEx.Message;
      }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      Login.Focus();
    }
  }
}
