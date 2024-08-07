using ExpensesManager.Domain.Entities;

namespace ExpensesManager.Infrastructure.Contracts.Repositories
{
    public interface IBillRepository
    {
        public List<Bill> GetAll();
        bool IsFileExists();
        void CreateFile();
        void SaveData(List<Bill> items);
    }
}
