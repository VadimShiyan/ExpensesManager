using ExpensesManager.Domain.Enums;

namespace ExpensesManager.Domain.DtoModels
{
    public class BillModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal BillAmount { get; set; }
        public string BillName { get; set; }
        public string BillDescription { get; set; }
        public ExpenseType ExpenseType { get; set; }
    }
}
