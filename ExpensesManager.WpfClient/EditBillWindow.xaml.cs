using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace ExpensesManager.WpfClient
{
    /// <summary>
    /// Interaction logic for EditBillWindow.xaml
    /// </summary>
    public partial class EditBillWindow : Window
    {
        public Bill Bill { get; set; }
        public ObservableCollection<ExpenseType> ExpenseTypes { get; set; }

        public EditBillWindow()
        {
            InitializeComponent();
        }

        public EditBillWindow(Bill bill, ObservableCollection<ExpenseType> expenseTypes)
        {
            InitializeComponent();

            Bill = bill;
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
