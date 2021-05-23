using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

  public class RelayCommand : ICommand {
    private Action<object> execute;
    private Func<object, bool> canExecute;

    public event EventHandler CanExecuteChanged {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) {
      this.execute = execute;
      this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter) {
      return this.canExecute == null || this.canExecute(parameter);
    }

    public void Execute(object parameter) {
      this.execute(parameter);
    }
  }
}
