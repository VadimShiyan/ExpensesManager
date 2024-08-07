using ExpensesManager.Application.Services;
using ExpensesManager.DataAccess.Repositories;
using ExpensesManager.Infrastructure.Contracts.Repositories;
using ExpensesManager.Infrastructure.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpensesManager.Client
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // Регистрация основных сервисов и окон
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();

                    services.AddTransient<SecondWindow>(); // Пример второго окна

                    // Регистрация других сервисов и репозиториев
                    services.AddTransient<IBillService, BillService>();
                    //services.AddSingleton<IBillRepository, BillRepository>(); //создать один экземпляр Репоз для всего приложения, что бы не чекать каждый запрос наличие файла, либо оставить Transient, что бы каждый раз проверял наличие файла?
                    services.AddTransient<IBillRepository, BillRepository>(); 
                })
                .Build();

            var app = host.Services.GetService<App>();

            app?.Run();
        }
    }
}
