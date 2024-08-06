using ExpensesManager.Domain.Enums;

namespace ExpensesManager.Domain.Entities
{
    public class Bill : BaseEntity
    {
        public decimal BillAmount { get; set; }
        public string BillName { get; set; } = string.Empty;
        public string BillDescription { get; set;} = string.Empty;
        public ExpenseType ExpenseType { get; set; }
    }
}
