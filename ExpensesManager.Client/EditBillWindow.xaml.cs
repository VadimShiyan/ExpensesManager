using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace ExpensesManager.Client
{
    /// <summary>
    /// Interaction logic for EditBillWindow.xaml
    /// </summary>
    public partial class EditBillWindow : Window
    {
        public Bill Bill { get; private set; }
        public ObservableCollection<ExpenseType> ExpenseTypes { get; set; }



        public EditBillWindow(Bill bill, ObservableCollection<ExpenseType> expenseTypes)
        {
            InitializeComponent();

            Bill = bill;
            ExpenseTypes = expenseTypes;
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //Close();
        }

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //    //Close();
        //}
    }
}
