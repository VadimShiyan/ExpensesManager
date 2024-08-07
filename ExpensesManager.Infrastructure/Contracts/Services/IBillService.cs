using ExpensesManager.Domain.DtoModels;

namespace ExpensesManager.Infrastructure.Contracts.Services
{
    public interface IBillService
    {
        string StatusMessage { get; }
        public List<BillModel> GetAll();
        public List<BillModel> GetAllByType(string type); //где оптимальнее трайПарсить строку в Енам - в сервисе или репозитории?
        public BillModel? GetById(Guid id);
        public void Create(BillModel billModel);
        public void Update(BillModel billModel);
        public void DeleteById(Guid id);
        public decimal GetTotalAmount(string? type, DateTime startDate, DateTime endDate); //сделать один "универспальный" сервис для статистики или 3 разных, как в репозитории?
        public decimal GetTotalAmount();
        public decimal GetTotalAmountByType(string type);//где оптимальнее трайПарсить строку в Енам - в сервисе или репозитории?
        public decimal GetTotalAmountByDateRange(DateTime startDate, DateTime endDate);
    }
}
