using ExpensesManager.Application.Services;
using ExpensesManager.DataAccess.Repositories;
using ExpensesManager.Domain;
using ExpensesManager.Infrastructure.Contracts.Repositories;
using ExpensesManager.Infrastructure.Contracts.Services;
using Microsoft.Extensions.Configuration;
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
                    //добавление json файла с конфигом
                    IConfiguration config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();

                    // Получение секции конфигурации
                    var applicationSettings = config.GetSection("ApplicationSettings").Get<ApplicationSettings>()!;

                    // Расширение переменных окружения и создание пути
                    applicationSettings.FilePath = Environment.ExpandEnvironmentVariables(applicationSettings.FilePath);

                    services.AddSingleton(applicationSettings);

                    // Регистрация основных сервисов и окон
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    //services.AddTransient<SecondWindow>(); // Пример второго окна
                    services.AddTransient<EditBillWindow>();
                    services.AddTransient<CreateBillWindow>();

                    // Регистрация других сервисов и репозиториев
                    services.AddSingleton<IBillService, BillService>();
                    services.AddSingleton<IBillRepository, BillRepository>(); 
                })
                .Build();

            var app = host.Services.GetService<App>();

            app?.Run();
        }
    }
}
