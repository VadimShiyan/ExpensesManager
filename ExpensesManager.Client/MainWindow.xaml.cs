using System.Collections.ObjectModel;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ExpensesManager.Domain;
using System.Windows.Controls;
using System.ComponentModel;

namespace ExpensesManager.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IBillService _billService;
        private readonly IServiceProvider _serviceProvider; //Внутри MainWindow, или любого другого окна, вы можете использовать DI для получения экземпляра нового окна и его открытия:
        
        public ObservableCollection<Bill> DataList { get; private set; }
        public int CurrentPage { get; private set; } = 1;

        public string PageInfo
        {
            get => _pageInfo;

            private set
            {
                _pageInfo = value;
                OnPropertyChanged(nameof(PageInfo));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private const int PageSize = 10;
        private string _pageInfo;
        private PagedResult<Bill> _data;
        private int _totalCount;

        public MainWindow(IBillService billService, IServiceProvider serviceProvider)
        {
            _billService = billService;
            _serviceProvider = serviceProvider;

            DataContext = this;// Устанавливаем DataContext на текущий экземпляр MainWindow

            InitializeComponent();

            LoadData(CurrentPage);
        }

        private void LoadData(int page)
        {
            _data = _billService.GetPaging(page);

            DataList = new ObservableCollection<Bill>(_data.Items);

            BillDataGrid.ItemsSource = DataList;

            _totalCount = _data.TotalCount;

            PageInfo = $"Page {CurrentPage} of {(_totalCount + 9) / PageSize}";
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage <= 1) return;
            CurrentPage--;
            LoadData(CurrentPage);
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage * PageSize >= _totalCount) return;
            CurrentPage++;
            LoadData(CurrentPage);
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

        private void ExpenseType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Info_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }



        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteButton1_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Bill bill)
            {
                // Откройте диалог для обновления объекта bill
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Bill bill)
            {
                // Откройте диалог для подтверждения удаления объекта bill
                // Удалите объект bill из коллекции DataList, если подтверждено
            }
        }
    }
}