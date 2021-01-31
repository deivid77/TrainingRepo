using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PruebaCompleta
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindowMV mv = new MainWindowMV();            
            Application.Current.MainWindow = new MainWindow(mv);
            Application.Current.MainWindow.Show();
            
        }

    }
}
