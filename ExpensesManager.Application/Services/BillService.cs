using ExpensesManager.Domain;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;
using ExpensesManager.Infrastructure.Contracts.Repositories;
using ExpensesManager.Infrastructure.Contracts.Services;

namespace ExpensesManager.Application.Services
{
    public class BillService : IBillService
    {
        private const int PageSize = 10;
        private readonly IBillRepository _billRepository;
        private readonly List<Bill> _bills;

        public BillService(IBillRepository billRepository)
        {
            _billRepository = billRepository;
            _bills = ReadFromFile();
        }

        public PagedResult<Bill> GetPaging(int page)
        {
            var items = _bills.OrderByDescending(b => b.UpdatedDate).Skip((page - 1) * PageSize).Take(PageSize).ToList();
            var totalCount = _bills.Count();

            return new PagedResult<Bill>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public PagedResult<Bill> GetPagingBySorting(int page, ExpenseType? type = null, DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _bills.AsQueryable();

            if (type.HasValue)
                query = query.Where(b => b.ExpenseType.Equals(type.Value));

            if (startDate.HasValue)
                query = query.Where(b => b.UpdatedDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.UpdatedDate <= endDate.Value);

            var totalCount = query.Count();

            var items = query.OrderByDescending(b => b.UpdatedDate).Skip((page - 1) * PageSize).Take(PageSize).ToList();

            return new PagedResult<Bill>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public Bill? GetById(Guid id)
        {
            return _bills.SingleOrDefault(c => c.Id.Equals(id));
        }

        public void AddBill(Bill bill)
        {
            if (bill == null)
                throw new ArgumentNullException(nameof(bill));

            bill.Id = Guid.NewGuid();
            bill.CreatedDate = DateTime.UtcNow;
            bill.UpdatedDate = bill.CreatedDate;

            _bills.Add(bill);

            _billRepository.SaveData(_bills);
        }

        public void Update(Bill bill)
        {
            if (bill is null)
                throw new ArgumentNullException(nameof(bill));

            var model = _bills.SingleOrDefault(c => c.Id.Equals(bill.Id));

            if (model is null)
                throw new KeyNotFoundException($"The bill with ID '{bill.Id}' was not found.");

            model.ExpenseType = bill.ExpenseType;
            model.Amount = bill.Amount;
            model.Description = bill.Description;
            model.Name = bill.Name;
            model.UpdatedDate = DateTime.UtcNow;

            _billRepository.SaveData(_bills);
        }

        public void DeleteById(Guid id)
        {
            var model = _bills.SingleOrDefault(c => c.Id.Equals(id));

            if (model is null)
                throw new KeyNotFoundException($"The bill with ID '{id}' was not found.");

            _bills.Remove(model);

            _billRepository.SaveData(_bills);
        }

        public decimal GetTotalAmount(ExpenseType? type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _bills.AsQueryable();

            if (type.HasValue)
                query = query.Where(b => b.ExpenseType == type.Value);

            if (startDate.HasValue)
                query = query.Where(b => b.UpdatedDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.UpdatedDate <= endDate.Value);

            return query.Sum(b => b.Amount);
        }

        public int GetTotalCount(ExpenseType? type = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _bills.AsQueryable();

            if (type.HasValue)
                query = query.Where(b => b.ExpenseType == type.Value);

            if (startDate.HasValue)
                query = query.Where(b => b.UpdatedDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(b => b.UpdatedDate <= endDate.Value);

            return query.Count();
        }

        private List<Bill> ReadFromFile()
        {
            if (_billRepository.IsFileExists())
            {
                return _billRepository.GetAll();
            }

            _billRepository.CreateFile();

            return new List<Bill>();
        }
    }
}
