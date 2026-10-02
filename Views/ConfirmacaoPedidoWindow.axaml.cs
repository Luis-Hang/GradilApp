using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GradilApp.Views;

/// <summary>
/// Representa a janela de confirmação de um pedido.
/// </summary>
public partial class ConfirmacaoPedidoWindow : Window
{
	#region Construtor

	/// <summary>
	/// Inicializa uma nova instância da janela de confirmação.
	/// </summary>
	public ConfirmacaoPedidoWindow()
	{
		InitializeComponent();
	}

	#endregion

	#region Eventos

	/// <summary>
	/// Cancela o pedido e fecha a janela de confirmação.
	/// </summary>
	private void OnCancelarPedidoClick(object? sender, RoutedEventArgs e)
	{
		Close(false);
	}

	/// <summary>
	/// Confirma o pedido e fecha a janela de confirmação.
	/// </summary>
	private void OnRealizarPedidoClick(object? sender, RoutedEventArgs e)
	{
		Close(true);
	}

	#endregion
}