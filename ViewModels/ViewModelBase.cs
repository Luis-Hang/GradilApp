using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GradilApp.ViewModels;

/// <summary>
/// Representa a classe base dos ViewModels da aplicação.
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
	#region Eventos

	/// <summary>
	/// Ocorre quando o valor de uma propriedade é alterado.
	/// </summary>
	public event PropertyChangedEventHandler? PropertyChanged;

	#endregion

	#region Métodos

	/// <summary>
	/// Notifica a interface sobre a alteração de uma propriedade.
	/// </summary>
	/// <param name="propertyName">Nome da propriedade alterada.</param>
	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// Atualiza uma propriedade quando o novo valor é diferente do atual.
	/// </summary>
	/// <typeparam name="T">Tipo do valor da propriedade.</typeparam>
	/// <param name="field">Campo que armazena o valor.</param>
	/// <param name="value">Novo valor.</param>
	/// <param name="propertyName">Nome da propriedade.</param>
	/// <returns>True quando o valor foi alterado.</returns>
	protected bool SetProperty<T>(ref T field, T value,
		[CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value))
			return false;

		field = value;
		OnPropertyChanged(propertyName);
		return true;
	}

	#endregion
}