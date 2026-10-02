using System;
using System.Windows.Input;

namespace GradilApp.Commands;

/// <summary>
/// Representa um comando que executa uma ação fornecida pela aplicação.
/// </summary>
public class RelayCommand : ICommand
{
	#region Atributos

	private readonly Action execute;
	private readonly Func<bool>? canExecute;

	#endregion

	#region Construtor

	/// <summary>
	/// Inicializa uma nova instância do comando.
	/// </summary>
	/// <param name="execute">Ação executada pelo comando.</param>
	/// <param name="canExecute">Função que determina se o comando pode ser executado.</param>
	public RelayCommand(Action execute, Func<bool>? canExecute = null)
	{
		this.execute = execute;
		this.canExecute = canExecute;
	}

	#endregion

	#region Eventos

	/// <summary>
	/// Ocorre quando a condição de execução do comando é alterada.
	/// </summary>
	public event EventHandler? CanExecuteChanged;

	#endregion

	#region Métodos

	/// <summary>
	/// Determina se o comando pode ser executado.
	/// </summary>
	/// <param name="parameter">Parâmetro fornecido ao comando.</param>
	/// <returns>True quando o comando pode ser executado.</returns>
	public bool CanExecute(object? parameter)
	{
		return canExecute?.Invoke() ?? true;
	}

	/// <summary>
	/// Executa a ação associada ao comando.
	/// </summary>
	/// <param name="parameter">Parâmetro fornecido ao comando.</param>
	public void Execute(object? parameter)
	{
		execute();
	}

	/// <summary>
	/// Notifica a interface sobre a alteração na condição de execução do comando.
	/// </summary>
	public void RaiseCanExecuteChanged()
	{
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	#endregion
}

/// <summary>
/// Representa um comando que executa uma ação recebendo um parâmetro.
/// </summary>
/// <typeparam name="T">Tipo do parâmetro recebido pelo comando.</typeparam>
public class RelayCommand<T> : ICommand
{
	#region Atributos

	private readonly Action<T> execute;

	#endregion

	#region Construtor

	/// <summary>
	/// Inicializa uma nova instância do comando parametrizado.
	/// </summary>
	/// <param name="execute">Ação executada pelo comando.</param>
	public RelayCommand(Action<T> execute)
	{
		this.execute = execute;
	}

	#endregion

	#region Eventos

	/// <summary>
	/// Ocorre quando a condição de execução do comando é alterada.
	/// </summary>
	public event EventHandler? CanExecuteChanged;

	#endregion

	#region Métodos

	/// <summary>
	/// Determina se o comando pode ser executado com o parâmetro informado.
	/// </summary>
	/// <param name="parameter">Parâmetro fornecido ao comando.</param>
	/// <returns>True quando o parâmetro possui o tipo esperado.</returns>
	public bool CanExecute(object? parameter)
	{
		return parameter is T;
	}

	/// <summary>
	/// Executa a ação associada ao comando quando o parâmetro possui o tipo esperado.
	/// </summary>
	/// <param name="parameter">Parâmetro fornecido ao comando.</param>
	public void Execute(object? parameter)
	{
		if (parameter is T valor)
		{
			execute(valor);
		}
	}

	/// <summary>
	/// Notifica a interface sobre a alteração na condição de execução do comando.
	/// </summary>
	public void RaiseCanExecuteChanged()
	{
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	#endregion
}