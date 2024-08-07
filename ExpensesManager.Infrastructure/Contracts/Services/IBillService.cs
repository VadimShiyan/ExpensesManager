using ExpensesManager.Domain;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;

namespace ExpensesManager.Infrastructure.Contracts.Services
{
    public interface IBillService
    {
        PagedResult<Bill> GetPaging(int page);
        PagedResult<Bill> GetPagingBySorting(int page, ExpenseType? type = null, DateTime? startDate = null, DateTime? endDate = null);
        Bill? GetById(Guid id);
        void AddBill(Bill bill);
        void Update(Bill bill);
        void DeleteById(Guid id);
        decimal GetTotalAmount(ExpenseType? type = null, DateTime? startDate = null, DateTime? endDate = null);
        int GetTotalCount(ExpenseType? type = null, DateTime? startDate = null, DateTime? endDate = null);
    }
}
