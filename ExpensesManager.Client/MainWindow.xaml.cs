using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExpensesManager.Infrastructure.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesManager.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBillService _billService;
        private readonly IServiceProvider _serviceProvider; //Внутри MainWindow, или любого другого окна, вы можете использовать DI для получения экземпляра нового окна и его открытия:
        public MainWindow(IBillService billService, IServiceProvider serviceProvider)
        {
            _billService = billService;
            _serviceProvider = serviceProvider;

            DataContext = billService; //need check this
            InitializeComponent();
        }

        private void OpenSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            var secondWindow = _serviceProvider.GetRequiredService<SecondWindow>();
            secondWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var count = _billService.GetAll().Count;
            var statusMess = _billService.StatusMessage;
            MessageBox.Show(statusMess + count);
        }
    }
}