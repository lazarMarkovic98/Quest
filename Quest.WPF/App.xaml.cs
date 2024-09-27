using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quest.WPF.Client.Interfaces;
using Quest.WPF.Configuration;
using System.IO;
using System.Windows;

namespace Quest.WPF;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider? _serviceProvider { get; private set; }

    public IConfiguration? _configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        _configuration = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();



        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, _configuration);
        var _serviceProvider = serviceCollection.BuildServiceProvider();


        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
    private void ConfigureServices(ServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<SetupOptions>(configuration.GetSection(SetupOptions.ConfigKey));


        serviceCollection.AddSingleton<IClient, Client.Implementations.Client>();
        serviceCollection.AddTransient(typeof(MainWindow));

    }
}

