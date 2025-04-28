using Microsoft.Extensions.DependencyInjection;
using Quiz.Stores;
using Quiz.View;
using Quiz.ViewModel;
using System.Diagnostics;
using System.Windows;
using Quiz.Core;
using System.Windows.Navigation;
using Quiz.Services;
using NavigationService = Quiz.Services.NavigationService;
using Quiz.Model;

namespace Quiz;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _servicesProvider;

    public App()
    {
        IServiceCollection _services = new ServiceCollection();

        // singletons
        _services.AddSingleton<MainWindow>(provider => new MainWindow
        { 
            DataContext = provider.GetRequiredService<MainViewModel>()
        });
        _services.AddSingleton<MainViewModel>();
        _services.AddSingleton<INavigationService, NavigationService>();
        _services.AddSingleton<QuizStore>();
        _services.AddSingleton<QuizViewModel>();
        _services.AddSingleton<GeneratorViewModel>();
        _services.AddSingleton<Func<Type, Core.ViewModel>>(serviceProvider => viewModelType => (Core.ViewModel)serviceProvider.GetRequiredService(viewModelType));

        // Transients
        _services.AddTransient<EditQuestionViewModel>();

        _servicesProvider = _services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _servicesProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}

