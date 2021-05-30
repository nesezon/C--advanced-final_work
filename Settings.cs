using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace e_commerce {
  public class Settings {
    /// <summary>
    /// Загрузка настроек из реестра
    /// </summary>
    public void LoadFromRegistry(MainWindow mainWindow) {
      RegistryKey regKey = Registry.CurrentUser;
      regKey = regKey.OpenSubKey("Software\\ITEA\\Final");
      if (regKey == null) return;
      setFromRegistry(regKey.GetValue("Window.Height"), mainWindow,
        (key, win) => { win.Height = Convert.ToDouble(key); });
      setFromRegistry(regKey.GetValue("Window.Width"), mainWindow,
        (key, win) => { win.Width = Convert.ToDouble(key); });
      setFromRegistry(regKey.GetValue("Window.State"), mainWindow,
        (key, win) => { win.WindowState = (WindowState)Enum.Parse(typeof(WindowState), key.ToString()); });
      if (regKey.GetValue("Window.Left") != null || regKey.GetValue("Window.Top") != null)
        mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
      setFromRegistry(regKey.GetValue("Window.Left"), mainWindow,
        (key, win) => { win.Left = Convert.ToDouble(key); });
      setFromRegistry(regKey.GetValue("Window.Top"), mainWindow,
        (key, win) => { win.Top = Convert.ToDouble(key); });
    }

    private void setFromRegistry(object key, MainWindow mainWindow, Action<object, MainWindow> doWork) {
      if (key != null) doWork(key, mainWindow);
    }

    /// <summary>
    /// Сохранение настроек в реестр
    /// </summary>
    public void Save2Registry(MainWindow mainWindow) {
      RegistryKey key = Registry.CurrentUser;
      // Создаю новый подраздел или открываю существующий для доступа на запись.
      RegistryKey wrkKey = key.CreateSubKey("Software\\ITEA\\Final");
      if (wrkKey == null) return;
      wrkKey.SetValue("Window.Height", mainWindow.Height);
      wrkKey.SetValue("Window.Width", mainWindow.Width);
      wrkKey.SetValue("Window.State", mainWindow.WindowState);
      wrkKey.SetValue("Window.Left", mainWindow.Left);
      wrkKey.SetValue("Window.Top", mainWindow.Top);
      wrkKey.Close();
    }
  }
}