using HyperQuantConnector.CoinLayer;
using HyperQuantConnector.REST;
using HyperQuantConnector.WebSocket;
using HyperQuantUI.Services;
using HyperQuantUI.ViewModel;
using HyperQuantUI.ViewModel.REST;
using HyperQuantUI.ViewModel.Wallet;
using HyperQuantUI.ViewModel.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace HyperQuantUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IRESTClient, RESTClient>();
                    services.AddScoped<IDialogService, DialogService>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<RESTTradeViewModel>();
                    services.AddSingleton<RESTCandleViewModel>();
                    services.AddSingleton<RESTTickerViewModel>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<Func<Type, BaseViewModel>>(viewModelType => (BaseViewModel)AppHost.Services.GetRequiredService(viewModelType));
                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }


        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();


            var startForm = AppHost.Services.GetRequiredService<MainWindow>();
            startForm.DataContext = AppHost.Services.GetRequiredService<MainViewModel>();
            startForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var viewModel = AppHost.Services.GetRequiredService<MainViewModel>();
            await AppHost.StopAsync();
            AppHost.Dispose();

            base.OnExit(e);
        }
    }

}
