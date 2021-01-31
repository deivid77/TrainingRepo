using System.Windows;

namespace PruebaCompleta
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
           
        public MainWindow(MainWindowMV mv)
        {
            InitializeComponent();
            this.DataContext = mv;
        }
           
    }

}
