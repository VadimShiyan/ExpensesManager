using System.ComponentModel;
using ExpensesManager.DataAccess.Repositories;
using ExpensesManager.Domain.DtoModels;
using ExpensesManager.Infrastructure.Contracts.Repositories;
using ExpensesManager.Infrastructure.Contracts.Services;

namespace ExpensesManager.Application.Services
{
    public class BillService : IBillService, INotifyPropertyChanged
    {
        private readonly IBillRepository _billRepository;
        private string _statusMessage;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public BillService(IBillRepository billRepository)
        {
            if (billRepository is BillRepository repository)
            {
                repository.FileStatusChanged += OnFileStatusChanged;
            }
            _billRepository = billRepository;
        }


        private void OnFileStatusChanged(string statusMessage)
        {
            StatusMessage = statusMessage;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public List<BillModel> GetAll()
        {
            var entities = _billRepository.GetAll();

            return entities.Select(bill => new BillModel
                {
                    Id = bill.Id,
                    CreatedDate = bill.CreatedDate,
                    UpdatedDate = bill.UpdatedDate,
                    ExpenseType = bill.ExpenseType,
                    BillAmount = bill.BillAmount,
                    BillDescription = bill.BillDescription,
                    BillName = bill.BillName
                })
                .ToList();
        }

        public List<BillModel> GetAllByType(string type)
        {
            throw new NotImplementedException();
        }

        public BillModel? GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Create(BillModel billModel)
        {
            throw new NotImplementedException();
        }

        public void Update(BillModel billModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalAmount(string? type, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalAmount()
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalAmountByType(string type)
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalAmountByDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
