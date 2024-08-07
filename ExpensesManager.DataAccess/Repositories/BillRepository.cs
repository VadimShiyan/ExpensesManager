using ExpensesManager.Domain.Entities;
using ExpensesManager.Domain.Enums;
using ExpensesManager.Infrastructure.Contracts.Repositories;
using Newtonsoft.Json;

namespace ExpensesManager.DataAccess.Repositories
{
    public class BillRepository : IBillRepository
    {
        private const string FilePath = "expenses.json";

        public event Action<string> FileStatusChanged;

        public BillRepository()
        {
            //FileStatusChanged = delegate { };
            EnsureFileExist();
        }

        public List<Bill> GetAll()
        {
            return ReadFromFile();
        }

        public List<Bill> GetAllByType(string type)
        {
            if (!Enum.TryParse<ExpenseType>(type, true, out var enumValue))
                throw new ArgumentException("Can't parse enum");

            return ReadFromFile().Where(b => b.ExpenseType.Equals(enumValue)).ToList();
        }

        public Bill? GetById(Guid id)
        {
            return ReadFromFile().FirstOrDefault(b => b.Id.Equals(id));
        }

        public void Create(Bill bill)
        {
            if (bill is null)
                throw new ArgumentNullException(nameof(bill));

            var bills = ReadFromFile();
            bills.Add(bill);
            WriteToFile(bills);
        }

        public void Update(Bill bill)
        {
            if (bill is null)
                throw new ArgumentNullException(nameof(bill));

            var bills = ReadFromFile();

            var index = bills.FindIndex(b => b.Id.Equals(bill.Id));

            if (index < 0)
                throw new KeyNotFoundException($"Bill with Id {bill.Id} not found");

            bills[index] = bill;
            WriteToFile(bills);
        }

        public void DeleteById(Guid id)
        {
            var bills = ReadFromFile();
            var bill = bills.FirstOrDefault(b => b.Id.Equals(id));

            if (bill is null)
                throw new KeyNotFoundException($"Bill with Id {id} not found");

            bills.Remove(bill);
            WriteToFile(bills);
        }

        public decimal GetTotalAmount()
        {
            var bills = ReadFromFile();

            return bills.Any() ? bills.Sum(b => b.BillAmount) : 0;
        }

        public decimal GetTotalAmountByType(string type)
        {
            var bills = GetAllByType(type);

            return bills.Any() ? bills.Sum(b => b.BillAmount) : 0;
        }

        public decimal GetTotalAmountByDateRange(DateTime startDate, DateTime endDate)
        {
            var bills = ReadFromFile().Where(b => b.UpdatedDate >= startDate && b.UpdatedDate <= endDate).ToList();

            return bills.Any() ? bills.Sum(b => b.BillAmount) : 0;
        }

        private void EnsureFileExist()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    FileStatusChanged?.Invoke("File found and used.");
                    return;
                }

                File.Create(FilePath).Dispose();
                FileStatusChanged?.Invoke("File not found and created.");
            }
            catch (Exception e)
            {
                FileStatusChanged?.Invoke($"Error checking/creating file: {e.Message}");
            }
        }

        private List<Bill> ReadFromFile()
        {
            try
            {
                var jsonData = File.ReadAllText(FilePath);

                return (string.IsNullOrEmpty(jsonData)
                    ? new List<Bill>()
                    : JsonConvert.DeserializeObject<List<Bill>>(jsonData))!;
            }
            catch (Exception e)
            {
                FileStatusChanged?.Invoke($"Error reading file: {e.Message}");

                return new List<Bill>();
            }
        }

        private void WriteToFile(List<Bill> items)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(FilePath, jsonData);
            }
            catch (Exception e)
            {
                FileStatusChanged?.Invoke($"Error writing to file: {e.Message}");
            }
        }
    }
}
