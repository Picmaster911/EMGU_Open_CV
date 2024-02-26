using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Open_CV_WPF.Service;
using Open_CV_WPF.View;
using Open_CV_WPF.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using static Emgu.CV.VideoCapture;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Open_CV_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public  IHostBuilder CreateHostBuilder () =>
             Host.CreateDefaultBuilder()
           .ConfigureAppConfiguration((hostContext, config) =>
           {
               // Здесь можно настроить конфигурацию, например, добавить источники конфигурации
               config.SetBasePath(Directory.GetCurrentDirectory());
               config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
               config.AddEnvironmentVariables();
           })
            .ConfigureServices((hostContext, services) => {
                ConfigureServices(hostContext, services);
            });

        public App()
        {
            _host = CreateHostBuilder()
               .Build();      
        }

        private void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            string connection = hostContext.Configuration.GetConnectionString("AppDbContext");
            services.AddDbContext<AppDbContext>(options => {options.UseSqlite(connection);});

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<CameraViewModel>();
            services.AddSingleton<IDataWorker, DataWorker>();
            services.AddTransient<CameraService>();
            services.AddSingleton<IFabricInstance,FabricInstance>();

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }

            var fabricInstance = _host.Services.GetRequiredService<IFabricInstance>();
            fabricInstance.Init();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            var mainWindowVM = _host.Services.GetRequiredService<MainWindowViewModel>();

            mainWindow.DataContext = mainWindowVM;
            mainWindow.Show();
        
            //сameraService.EventFromVideoCams += mainWindowVM.EventFromCamera;
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }
            base.OnExit(e);
        }
    }

}

