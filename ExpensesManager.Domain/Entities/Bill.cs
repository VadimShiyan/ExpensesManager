using ExpensesManager.Domain.Enums;

namespace ExpensesManager.Domain.Entities
{
    public class Bill
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set;} = string.Empty;
        public ExpenseType ExpenseType { get; set; }
    }
}
