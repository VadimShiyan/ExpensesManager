using ExpensesManager.Domain;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;
using ExpensesManager.Infrastructure.Contracts.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ExpensesManager.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IBillService _billService;

        public ObservableCollection<Bill> DataList { get; private set; }
        public ObservableCollection<ExpenseType> ExpenseTypes { get; private set; }
        public ObservableCollection<ExpenseType> ExpenseTypesStat { get; private set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StartDateStat { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EndDateStat { get; set; }

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

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(IBillService billService)
        {
            _billService = billService;

            DataContext = this;

            InitializeComponent();

            LoadData(CurrentPage);

            ExpenseTypes = new ObservableCollection<ExpenseType>((ExpenseType[])Enum.GetValues(typeof(ExpenseType)));
            ExpenseTypeComboBox.ItemsSource = ExpenseTypes;
            ExpenseTypeComboBox.SelectedIndex = -1;

            ExpenseTypesStat = new ObservableCollection<ExpenseType>((ExpenseType[])Enum.GetValues(typeof(ExpenseType)));
            ExpenseTypeComboBoxStat.ItemsSource = ExpenseTypesStat;
            ExpenseTypeComboBoxStat.SelectedIndex = -1;
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

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateBillWindow(ExpenseTypes);

            if (createWindow.ShowDialog() == true)
            {
                try
                {
                    _billService.AddBill(createWindow.Bill);
                    LoadData(CurrentPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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

        private void UpdateStatistics_Click(object sender, RoutedEventArgs e)
        {
            var totalAmount = _billService.GetTotalAmount();
            var totalCount = _billService.GetTotalCount();
            TotalAmountText.Text = $"The sum of all receipts: {totalAmount}";
            TotalCountText.Text = $"Total count of receipts: {totalCount}";

            if (ExpenseTypeComboBoxStat.SelectedItem is ExpenseType selectedType)
            {
                var categoryAmount = _billService.GetTotalAmount(selectedType);
                var categoryCount = _billService.GetTotalCount(selectedType);
                CategoryAmountText.Text = $"Sum: {categoryAmount}";
                CategoryCountText.Text = $"Count: {categoryCount}";
            }
            else
            {
                CategoryAmountText.Text = "Sum: N/A";
                CategoryCountText.Text = "Count: N/A";
            }

            if (StartDateFilterStat.SelectedDate.HasValue || EndDateFilterStat.SelectedDate.HasValue)
            {
                var startDate = StartDateFilterStat.SelectedDate ?? DateTime.MinValue;
                var endDate = EndDateFilterStat.SelectedDate ?? DateTime.MaxValue;

                var dateAmount = _billService.GetTotalAmount(null, startDate, endDate);
                var dateCount = _billService.GetTotalCount(null, startDate, endDate);

                DateAmountText.Text = $"Sum: {dateAmount}";
                DateCountText.Text = $"Count: {dateCount}";
            }
            else
            {
                DateAmountText.Text = "Sum: N/A";
                DateCountText.Text = "Count: N/A";
            }
        }

        private void ClearStatistics_Click(object sender, RoutedEventArgs e)
        {
            ExpenseTypeComboBoxStat.SelectedItem = null;
            StartDateFilterStat.SelectedDate = null;
            EndDateFilterStat.SelectedDate = null;

            TotalAmountText.Text = "The sum of all receipts: 0";
            TotalCountText.Text = "Total count of receipts: 0";
            CategoryAmountText.Text = "Sum: N/A";
            CategoryCountText.Text = "Count: N/A";
            DateAmountText.Text = "Sum: N/A";
            DateCountText.Text = "Count: N/A";
        }
    }
}