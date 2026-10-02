using Avalonia.Controls;
using GradilApp.Models;
using GradilApp.ViewModels;

namespace GradilApp.Views;

/// <summary>
/// Representa a janela principal da aplicação.
/// </summary>
public partial class MainWindow : Window
{
	#region Atributos

	private readonly MainViewModel viewModel;

	#endregion

	#region Construtor

	/// <summary>
	/// Inicializa uma nova instância da janela principal.
	/// </summary>
	public MainWindow() : this(new MainViewModel())
	{
	}

	/// <summary>
	/// Inicializa uma nova instância da janela principal com o ViewModel informado.
	/// </summary>
	/// <param name="viewModel">ViewModel associado à janela.</param>
	public MainWindow(MainViewModel viewModel)
	{
		InitializeComponent();

		this.viewModel = viewModel;
		DataContext = viewModel;

		viewModel.OnAberturaPedidosSolicitada += OnAbrirPedidos;
		viewModel.OnConfirmacaoPedidoSolicitada += OnAbrirConfirmacaoPedido;
	}

	#endregion

	#region Eventos

	/// <summary>
	/// Abre a janela que apresenta os pedidos confirmados.
	/// </summary>
	private void OnAbrirPedidos()
	{
		PedidosWindow janelaPedidos = new()
		{
			DataContext = DataContext
		};

		janelaPedidos.Show(this);
	}

	/// <summary>
	/// Abre a janela de confirmação do pedido e processa a decisão do usuário.
	/// </summary>
	/// <param name="pedido">Pedido que será apresentado para confirmação.</param>
	private async void OnAbrirConfirmacaoPedido(Pedido pedido)
	{
		ConfirmacaoPedidoWindow janela = new()
		{
			DataContext = pedido
		};

		bool? resultado = await janela.ShowDialog<bool?>(this);
		if (resultado == true)
			viewModel.RealizarPedido();
		else
			viewModel.CancelarPedido();
	}

	#endregion
}