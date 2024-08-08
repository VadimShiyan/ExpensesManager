using ExpensesManager.Domain;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;
using ExpensesManager.Infrastructure.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
        public ObservableCollection<ExpenseType> ExpenseTypes { get; private set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

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

            ExpenseTypes = new ObservableCollection<ExpenseType>((ExpenseType[])Enum.GetValues(typeof(ExpenseType)));
            ExpenseTypeComboBox.ItemsSource = ExpenseTypes;
            ExpenseTypeComboBox.SelectedIndex = -1;
        }

        private void LoadData(int page, ExpenseType? expenseType = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (expenseType.HasValue || startDate.HasValue || endDate.HasValue)
                _data = _billService.GetPagingBySorting(page, expenseType, startDate, endDate);

            else
                _data = _billService.GetPaging(page);

            DataList = new ObservableCollection<Bill>(_data.Items);

            BillDataGrid.ItemsSource = DataList;

            _totalCount = _data.TotalCount;

            PageInfo = $"Page {CurrentPage} of {(_totalCount + 9) / PageSize} (Total items:{_totalCount})";
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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Bill bill)
            {
                var copyBill = new Bill
                {
                    Id = bill.Id,
                    Name = bill.Name,
                    Description = bill.Description,
                    Amount = bill.Amount,
                    ExpenseType = bill.ExpenseType,
                    CreatedDate = bill.CreatedDate,
                    UpdatedDate = bill.UpdatedDate
                };

                var editWindow = new EditBillWindow(copyBill, ExpenseTypes);

                if (editWindow.ShowDialog() == true)
                {
                    try
                    {
                        _billService.Update(editWindow.Bill);
                        LoadData(CurrentPage);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                //editWindow.Owner = this;
                //editWindow.Show();

                //if (editWindow.Close() =)
                //{
                //    MessageBox.Show("закрылось");
                //    _billService.Update(editWindow.Bill);
                //    LoadData(CurrentPage);
                //}

            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Bill bill)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the bill: {bill.Name}?",
                    "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _billService.DeleteById(bill.Id);
                        LoadData(CurrentPage);
                    }
                    catch (KeyNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var expenseType = ExpenseTypeComboBox.SelectedItem as ExpenseType?;
            var startDate = StartDateFilter.SelectedDate;
            var endDate = EndDateFilter.SelectedDate;

            if (!expenseType.HasValue && !startDate.HasValue && !endDate.HasValue)
                return;

            CurrentPage = 1;
            LoadData(CurrentPage, expenseType, startDate, endDate);
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            ExpenseTypeComboBox.SelectedIndex = -1;

            StartDateFilter.SelectedDate = null;
            EndDateFilter.SelectedDate = null;

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


    }
}