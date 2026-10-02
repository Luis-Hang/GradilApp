using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GradilApp.ViewModels;

namespace GradilApp;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    /// <summary>
    /// Cria a View correspondente ao ViewModel informado.
    /// </summary>
    /// <param name="param">ViewModel utilizado para localizar a View.</param>
    /// <returns>A View correspondente ao ViewModel ou null quando não encontrada.</returns>
    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        
        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }
        
        return new TextBlock { Text = "Not Found: " + name };
    }

    /// <summary>
    /// Determina se o objeto informado pode ser associado a este ViewLocator.
    /// </summary>
    /// <param name="data">Objeto utilizado para verificar a correspondência.</param>
    /// <returns>True quando o objeto pode ser localizado por este ViewLocator.</returns>
    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
