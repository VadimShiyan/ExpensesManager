using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Infrastructure.Contracts.Repositories
{
    public interface IBillRepository
    {
        public List<Bill> GetAll();
        public List<Bill> GetAllByType(string type);
        public Bill? GetById(Guid id);
        public void Create (Bill bill);
        public void Update (Bill bill);
        public void DeleteById (Guid id);
        public decimal GetTotalAmount();
        public decimal GetTotalAmountByType(string type);
        public decimal GetTotalAmountByDateRange(DateTime startDate, DateTime endDate);
    }
}
