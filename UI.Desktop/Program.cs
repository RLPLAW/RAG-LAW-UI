using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UI.Views;
using System;
using System.IO;
using DataAccessLayer;
using UI.ViewModels;

namespace UI.Desktop;

class Program
{
    public static void Main(string[] args)
    {
        // Load config
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Setup DI
        var services = new ServiceCollection();

        // Register DbContext
        services.AddDbContext<RlpContext>(options =>
            options.UseSqlServer(config.GetConnectionString("MyDbConnection")));

        services.AddTransient<MainViewModel>();

        var serviceProvider = services.BuildServiceProvider();

        // Simply start the Avalonia app - let App.axaml.cs handle window creation
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}