using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace ExpensesManager.WpfClient
{
    /// <summary>
    /// Interaction logic for CreateBillWindow.xaml
    /// </summary>
    public partial class CreateBillWindow : Window
    {
        public Bill Bill { get; private set; }
        public ObservableCollection<ExpenseType> ExpenseTypes { get; set; }

        public CreateBillWindow()
        {
            InitializeComponent();
        }

        public CreateBillWindow(ObservableCollection<ExpenseType> expenseTypes)
        {
            InitializeComponent();

            Bill = new Bill();
            ExpenseTypes = expenseTypes;
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Please fill out all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.DialogResult = true;
        }

        private bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Bill.Name) || Bill.Amount <= 0 || Bill.ExpenseType == null)
            {
                return false;
            }

            return true;
        }
    }
}
