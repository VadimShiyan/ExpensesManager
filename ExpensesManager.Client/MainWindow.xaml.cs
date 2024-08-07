using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ExpensesManager.Domain;

namespace ExpensesManager.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBillService _billService;
        private readonly IServiceProvider _serviceProvider; //Внутри MainWindow, или любого другого окна, вы можете использовать DI для получения экземпляра нового окна и его открытия:
        
        private readonly PagedResult<Bill> _data;
        public MainWindow(IBillService billService, IServiceProvider serviceProvider)
        {
            _billService = billService;
            _serviceProvider = serviceProvider;

            _data = _billService.GetPaging(1);

            DataContext = billService; //need check this impl
            InitializeComponent();
        }

        private void OpenSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            var secondWindow = _serviceProvider.GetRequiredService<SecondWindow>();
            secondWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var count = _billService.GetTotalCount();

            MessageBox.Show("Hello" + count + _data.TotalCount);
        }
    }
}