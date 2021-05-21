﻿using System.Data.Entity;
using System.Windows;
using e_commerce.DBModel;
using e_commerce.Views;

namespace e_commerce {
  public partial class MainWindow : Window {
    public MainWindow() {

      // первый запрос к БД долгий, поэтому
      // делаю его заранее в фоне
      // (простой, ничего не значащий запрос)
      using (var db = new StoreDB()) {
        var users = db.Roles.ToListAsync();
      }

      InitializeComponent();

      // пересоздание БД при каждом запуске
      Database.SetInitializer<StoreDB>(new StoreDB.StoreDbInitializer());

      ActiveItem.Content = new LoginView();
    }
  }
}
