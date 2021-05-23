using System.ComponentModel;
using System.Windows;

namespace e_commerce {
  public class CartItem : INotifyPropertyChanged {
    public int product_id { get; set; }
    public string product_name { get; set; }
    public int quantity { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    // команда увеличения количества
    private RelayCommand addCommand;
    public RelayCommand AddCommand {
      get {
        return addCommand ??
               (addCommand = new RelayCommand(obj => {
                 quantity++;
                 PropertyChanged(this, new PropertyChangedEventArgs("quantity"));
               }));
      }
    }

    // команда уменьшения количества
    private RelayCommand subCommand;
    public RelayCommand SubCommand {
      get {
        return subCommand ??
               (subCommand = new RelayCommand(obj => {
                 if (quantity > 1) {
                   quantity--;
                   PropertyChanged(this, new PropertyChangedEventArgs("quantity"));
                 }
               }));
      }
    }

    // команда удаления
    private RelayCommand delCommand;
    public RelayCommand DelCommand {
      get {
        return delCommand ??
               (delCommand = new RelayCommand(obj => {
                 MainWindow MW = Application.Current.MainWindow as MainWindow;
                 MW.RemoveFromCart(product_id);
               }));
      }
    }
  }
}
