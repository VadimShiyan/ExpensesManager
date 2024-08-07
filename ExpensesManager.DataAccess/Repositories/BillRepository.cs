using ExpensesManager.Domain;
using ExpensesManager.Domain.Entities;
using ExpensesManager.Infrastructure.Contracts.Repositories;
using Newtonsoft.Json;

namespace ExpensesManager.DataAccess.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly string _filePath;

        public BillRepository(ApplicationSettings applicationSettings)
        {
            _filePath = applicationSettings.FilePath;
        }

        public List<Bill> GetAll()
        {
            var jsonData = File.ReadAllText(_filePath);

            return string.IsNullOrEmpty(jsonData)
                ? new List<Bill>()
                : JsonConvert.DeserializeObject<List<Bill>>(jsonData) ?? new List<Bill>();
        }

        public bool IsFileExists()
        {
            return File.Exists(_filePath);
        }

        public void CreateFile()
        {
            string directoryPath = Path.GetDirectoryName(_filePath) ?? string.Empty;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (File.Create(_filePath))
            {
            }
        }

        public void SaveData(List<Bill> items)
        {
            var jsonData = JsonConvert.SerializeObject(items, Formatting.Indented);

            File.WriteAllText(_filePath, jsonData);
        }
    }
}
