using System.Data.Entity;
using System.Windows;
using e_commerce.DBModel;
using e_commerce.Views;

namespace e_commerce {
  public partial class MainWindow : Window {

    public User authUser = new User();
    public MainWindow() {

      InitializeComponent();

      // пересоздание БД при каждом запуске
      Database.SetInitializer<StoreDB>(new StoreDB.StoreDbInitializer());

      ActiveItem.Content = new LoginView();
    }
  }
}
