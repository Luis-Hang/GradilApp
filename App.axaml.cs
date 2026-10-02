using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GradilApp.ViewModels;
using GradilApp.Views;

namespace GradilApp;

/// <summary>
/// Representa a aplicação Avalonia.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Inicializa os componentes da aplicação.
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Conclui a inicialização da aplicação e configura a janela principal.
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
	    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
	    {
	    	MainViewModel viewModel = new();
    
	    	desktop.MainWindow = new MainWindow(viewModel);
	    }
    
	    base.OnFrameworkInitializationCompleted();
    }
}