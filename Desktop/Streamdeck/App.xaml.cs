using Microsoft.Extensions.DependencyInjection;
using Streamdeck.ViewModels;
using System;
using System.Windows;

namespace Streamdeck
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            //services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<MainWindowViewModel>();
            return services.BuildServiceProvider();
        }

        public MainWindowViewModel MainWindowVM => Services.GetService<MainWindowViewModel>();
    }
}
