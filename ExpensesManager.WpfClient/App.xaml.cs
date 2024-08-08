using ExpensesManager.Application.Services;
using ExpensesManager.DataAccess.Repositories;
using ExpensesManager.Domain;
using ExpensesManager.Infrastructure.Contracts.Repositories;
using ExpensesManager.Infrastructure.Contracts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;

namespace ExpensesManager.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public ApplicationSettings ApplicationSettings { get; private set; }

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            ApplicationSettings = config.GetSection("ApplicationSettings").Get<ApplicationSettings>()!;
            ApplicationSettings.FilePath = Environment.ExpandEnvironmentVariables(ApplicationSettings.FilePath);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ServiceProvider = serviceCollection.BuildServiceProvider();


            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(ApplicationSettings);

            serviceCollection.AddSingleton<IBillRepository, BillRepository>();
            serviceCollection.AddSingleton<IBillService, BillService>();

            serviceCollection.AddTransient(typeof(MainWindow));
        }
    }
}
