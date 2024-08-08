using System.Windows;

namespace ExpensesManager.Client
{
    public class App : System.Windows.Application
    {
        private readonly MainWindow _mainWindow;

        public App(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            _mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
