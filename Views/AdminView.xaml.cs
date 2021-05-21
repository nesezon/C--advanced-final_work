using System.Windows;
using System.Windows.Controls;

namespace e_commerce.Views {
  public partial class AdminView : UserControl {
    private static MainWindow MW;
    public AdminView() {
      InitializeComponent();
      MW = Application.Current.MainWindow as MainWindow;
      DataContext = MW;
    } 

    private void LogOut_Click(object sender, RoutedEventArgs e) {
      MW.ActiveItem.Content = new LoginView();
    }
  }
}
