using System.Windows;
using System.Windows.Controls;

namespace e_commerce.Views {
  public partial class ClientView : UserControl {
    private static MainWindow MW;
    public ClientView() {
      InitializeComponent();
      MW = Application.Current.MainWindow as MainWindow;
      DataContext = MW;
    }

    private void LogOut_Click(object sender, RoutedEventArgs e) {
      MW.ActiveItem.Content = new LoginView();
    }
  }
}
